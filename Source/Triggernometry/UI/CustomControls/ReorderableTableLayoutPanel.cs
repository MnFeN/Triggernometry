using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Triggernometry.UI.CustomControls
{
    /// <summary>固定行列的可排序标签网格，支持整套顺序读写、拖拽高亮和半透明残影。</summary>
    public class ReorderableTableLayoutPanel : TableLayoutPanel
    {
        private Label pressedLabel;
        private Point dragStart;
        private DragPreview dragPreview;
        private Label dragSource;
        private Label dropTarget;

        /// <summary>拖拽或应用整套顺序后通知调用方；序号按行优先计算。</summary>
        public event EventHandler OrderChanged;

        /// <summary>创建一个单元格，供设计器或派生控件随后设置行列。</summary>
        public ReorderableTableLayoutPanel() : this(1, 1) { }

        /// <summary>创建指定行列的等宽、等高标签网格。</summary>
        public ReorderableTableLayoutPanel(int rows, int columns)
        {
            Dock = DockStyle.Fill;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
            AllowDrop = true;
            DoubleBuffered = true;
            SetGridSize(rows, columns);
        }

        /// <summary>设置固定行列并保留现有标签顺序；单元格不足时不修改布局。</summary>
        public void SetGridSize(int rows, int columns)
        {
            if (rows <= 0)
                throw new ArgumentOutOfRangeException(nameof(rows));
            if (columns <= 0)
                throw new ArgumentOutOfRangeException(nameof(columns));
            var labels = GetLabels();
            if (labels.Length > rows * columns)
                throw new ArgumentException("The grid cannot hold all labels.");

            SuspendLayout();
            try
            {
                RowCount = rows;
                ColumnCount = columns;
                RowStyles.Clear();
                ColumnStyles.Clear();
                for (int row = 0; row < rows; row++)
                    RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rows));
                for (int column = 0; column < columns; column++)
                    ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / columns));
                for (int i = 0; i < labels.Length; i++)
                    SetCellPosition(labels[i], new TableLayoutPanelCellPosition(i % columns, i / columns));
            }
            finally
            {
                ResumeLayout(true);
            }
        }

        /// <summary>在末尾添加标签；调用方可用 Tag 保存预设映射需要的业务值。</summary>
        public void AddLabel(Label label)
        {
            if (label == null)
                throw new ArgumentNullException(nameof(label));
            if (label.Parent == this)
                throw new ArgumentException("The label already belongs to this grid.", nameof(label));
            int order = GetLabels().Length;
            if (order >= RowCount * ColumnCount)
                throw new InvalidOperationException("The grid is full.");
            Controls.Add(label, order % ColumnCount, order / ColumnCount);
        }

        /// <summary>按当前排列返回标签，供调用方读取优先级或保存预设。</summary>
        public Label[] GetLabels() => Controls.OfType<Label>().OrderBy(GetOrder).ToArray();

        /// <summary>获取标签从零开始的排列序号，位置只以网格单元格为准。</summary>
        public int GetOrder(Label label)
        {
            var position = GetCellPosition(label);
            return position.Row * ColumnCount + position.Column;
        }

        /// <summary>一次应用全部现有标签的排列，完成后只发出一次顺序变化通知。</summary>
        public void SetOrder(IEnumerable<Label> labels)
        {
            if (labels == null)
                throw new ArgumentNullException(nameof(labels));
            var ordered = labels.ToArray();
            var current = GetLabels();
            // 先校验完整排列，错误的预设不能把表格改到一半。
            if (ordered.Length != current.Length || ordered.Distinct().Count() != ordered.Length
                || ordered.Any(label => label?.Parent != this))
                throw new ArgumentException("The order must contain every label exactly once.", nameof(labels));
            if (ordered.SequenceEqual(current))
                return;

            SuspendLayout();
            try
            {
                for (int i = 0; i < ordered.Length; i++)
                    SetCellPosition(ordered[i], new TableLayoutPanelCellPosition(i % ColumnCount, i / ColumnCount));
                OnOrderChanged(EventArgs.Empty);
            }
            finally
            {
                ResumeLayout(true);
            }
        }

        /// <summary>统一标签显示与拖拽行为，业务子类只需设置文字、颜色等内容。</summary>
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (!(e.Control is Label label))
                return;
            label.AutoSize = false;
            label.Anchor = AnchorStyles.None;
            label.TextAlign = ContentAlignment.MiddleCenter;
            // 默认背景透出单元格的高亮；调用方显式指定的背景色仍保留。
            if (!TypeDescriptor.GetProperties(label)[nameof(BackColor)].ShouldSerializeValue(label))
                label.BackColor = Color.Transparent;
            label.Padding = new Padding(10);
            label.Margin = new Padding(10);
            label.Cursor = Cursors.SizeAll;
            label.MouseDown += Label_MouseDown;
            label.MouseMove += Label_MouseMove;
            label.MouseUp += Label_MouseUp;
            label.GiveFeedback += Label_GiveFeedback;
            label.TextChanged += LabelContentChanged;
            label.FontChanged += LabelContentChanged;
            label.PaddingChanged += LabelContentChanged;
            AdjustLabelSizes();
        }

        /// <summary>标签移出网格后解除事件，避免仍调用原网格。</summary>
        protected override void OnControlRemoved(ControlEventArgs e)
        {
            if (e.Control is Label label)
            {
                label.MouseDown -= Label_MouseDown;
                label.MouseMove -= Label_MouseMove;
                label.MouseUp -= Label_MouseUp;
                label.GiveFeedback -= Label_GiveFeedback;
                label.TextChanged -= LabelContentChanged;
                label.FontChanged -= LabelContentChanged;
                label.PaddingChanged -= LabelContentChanged;
            }
            if (pressedLabel == e.Control)
                pressedLabel = null;
            if (dragSource == e.Control || dropTarget == e.Control)
                SetDragHighlights(dragSource == e.Control ? null : dragSource, dropTarget == e.Control ? null : dropTarget);
            base.OnControlRemoved(e);
            if (!Disposing)
                AdjustLabelSizes();
        }

        /// <summary>文字或字体变化后重新计算统一标签尺寸。</summary>
        private void LabelContentChanged(object sender, EventArgs e) => AdjustLabelSizes();

        /// <summary>按最大首选尺寸统一标签大小，测量包含各自的字体、内边距和最小尺寸。</summary>
        private void AdjustLabelSizes()
        {
            var labels = GetLabels();
            Size size = Size.Empty;
            foreach (var label in labels)
            {
                Size preferred = label.GetPreferredSize(Size.Empty);
                size.Width = Math.Max(size.Width, preferred.Width);
                size.Height = Math.Max(size.Height, preferred.Height);
            }
            foreach (var label in labels)
                label.Size = size;
        }

        /// <summary>句柄创建后按实际继承的字体重新测量。</summary>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AdjustLabelSizes();
        }

        /// <summary>仅接受本表格内的标签移动。</summary>
        private Label GetDraggedLabel(DragEventArgs e)
        {
            var control = e.Data.GetData(typeof(Label)) as Label;
            return control?.Parent == this && (e.AllowedEffect & DragDropEffects.Move) != 0 ? control : null;
        }

        /// <summary>更新允许的拖拽效果以及源格子和当前落点的高亮。</summary>
        private void UpdateDropTarget(DragEventArgs e)
        {
            var source = GetDraggedLabel(e);
            var target = source != null ? GetDropLabel(e) : null;
            e.Effect = source != null ? DragDropEffects.Move : DragDropEffects.None;
            SetDragHighlights(source, target != source ? target : null);
        }

        /// <summary>按整个单元格定位标签，标签周围的留白也属于该落点。</summary>
        private Label GetDropLabel(DragEventArgs e)
        {
            Point point = PointToClient(new Point(e.X, e.Y));
            if (!ClientRectangle.Contains(point))
                return null;
            int column = GetCellIndex(point.X - DisplayRectangle.Left, GetColumnWidths());
            int row = GetCellIndex(point.Y - DisplayRectangle.Top, GetRowHeights());
            return column < 0 || row < 0 ? null : GetControlFromPosition(column, row) as Label;
        }

        /// <summary>将相对于网格起点的坐标转换为行或列的下标。</summary>
        private static int GetCellIndex(int coordinate, int[] sizes)
        {
            if (coordinate < 0)
                return -1;
            for (int i = 0; i < sizes.Length; i++)
            {
                if (coordinate < sizes[i])
                    return i;
                coordinate -= sizes[i];
            }
            return -1;
        }

        /// <summary>只在源或目标变化时重绘，透明标签也同步刷新父背景。</summary>
        private void SetDragHighlights(Label source, Label target)
        {
            if (dragSource == source && dropTarget == target)
                return;
            dragSource = source;
            dropTarget = target;
            Invalidate(true);
        }

        /// <summary>在单元格背景上区分拖拽源和落点，不修改标签本身的颜色。</summary>
        protected override void OnCellPaint(TableLayoutCellPaintEventArgs e)
        {
            base.OnCellPaint(e);
            var label = GetControlFromPosition(e.Column, e.Row);
            if (label == null || (label != dragSource && label != dropTarget))
                return;
            using (var brush = new SolidBrush(Color.FromArgb(label == dropTarget ? 80 : 40, SystemColors.Highlight)))
                e.Graphics.FillRectangle(brush, e.CellBounds);
        }

        /// <summary>进入网格时标出当前落点。</summary>
        protected override void OnDragEnter(DragEventArgs e)
        {
            UpdateDropTarget(e);
            base.OnDragEnter(e);
        }

        /// <summary>拖动期间持续更新落点，拒绝其他表格的标签。</summary>
        protected override void OnDragOver(DragEventArgs e)
        {
            UpdateDropTarget(e);
            base.OnDragOver(e);
        }

        /// <summary>移出网格时清除落点，拖拽源仍保持高亮直到拖拽结束。</summary>
        protected override void OnDragLeave(EventArgs e)
        {
            SetDragHighlights(dragSource, null);
            base.OnDragLeave(e);
        }

        /// <summary>放到另一个标签的单元格时交换位置，放到空单元格保持原顺序。</summary>
        protected override void OnDragDrop(DragEventArgs e)
        {
            var source = GetDraggedLabel(e);
            var target = GetDropLabel(e);
            SetDragHighlights(null, null);
            if (source != null && target != null && source != target)
            {
                dragPreview?.Hide();
                var sourcePosition = GetCellPosition(source);
                var targetPosition = GetCellPosition(target);
                SuspendLayout();
                try
                {
                    SetCellPosition(source, targetPosition);
                    SetCellPosition(target, sourcePosition);
                    OnOrderChanged(EventArgs.Empty);
                }
                finally
                {
                    ResumeLayout(true);
                }
            }
            base.OnDragDrop(e);
        }

        /// <summary>让派生控件更新与位置有关的显示，再通知调用方。</summary>
        protected virtual void OnOrderChanged(EventArgs e) => OrderChanged?.Invoke(this, e);

        /// <summary>记录按下位置，普通点击不立即进入拖拽。</summary>
        private void Label_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            pressedLabel = (Label)sender;
            dragStart = e.Location;
        }

        /// <summary>移出系统拖拽阈值后，开始标签交换。</summary>
        private void Label_MouseMove(object sender, MouseEventArgs e)
        {
            if (pressedLabel != sender || e.Button != MouseButtons.Left)
                return;
            Size threshold = SystemInformation.DragSize;
            var bounds = new Rectangle(dragStart.X - threshold.Width / 2, dragStart.Y - threshold.Height / 2,
                threshold.Width, threshold.Height);
            if (bounds.Contains(e.Location))
                return;

            var control = pressedLabel;
            pressedLabel = null;
            using (var image = new Bitmap(control.Width, control.Height))
            {
                control.DrawToBitmap(image, new Rectangle(Point.Empty, image.Size));
                using (var preview = new DragPreview(image))
                {
                    dragPreview = preview;
                    try
                    {
                        SetDragHighlights(control, null);
                        UpdatePreviewPosition();
                        preview.Show(FindForm());
                        var data = new DataObject();
                        data.SetData(typeof(Label), control);
                        control.DoDragDrop(data, DragDropEffects.Move);
                    }
                    finally
                    {
                        // DoDragDrop 退出涵盖正常放下、Esc 取消及异常，残影和截图均由 using 释放。
                        dragPreview = null;
                        SetDragHighlights(null, null);
                    }
                }
            }
        }

        /// <summary>鼠标释放后清除尚未开始的拖拽。</summary>
        private void Label_MouseUp(object sender, MouseEventArgs e) => pressedLabel = null;

        /// <summary>OLE 拖拽期间通过反馈事件移动残影，此时不再依赖普通 MouseMove。</summary>
        private void Label_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (dragPreview != null)
                UpdatePreviewPosition();
        }

        /// <summary>保留鼠标按下时相对于控件的位置，避免残影突然跳到鼠标中央。</summary>
        private void UpdatePreviewPosition()
        {
            Point cursor = Cursor.Position;
            dragPreview.Location = new Point(cursor.X - dragStart.X, cursor.Y - dragStart.Y);
        }

        /// <summary>仅用于显示截图的半透明窗口，不抢焦点，也不遮挡拖放目标。</summary>
        private sealed class DragPreview : Form
        {
            /// <summary>按源控件截图建立固定尺寸的半透明窗口。</summary>
            public DragPreview(Bitmap image)
            {
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.Manual;
                AutoScaleMode = AutoScaleMode.None;
                // 固定截图尺寸，避免较窄的残影被系统最小窗体宽度拉大。
                MinimumSize = image.Size;
                MaximumSize = image.Size;
                ClientSize = image.Size;
                BackgroundImage = image;
                Opacity = 0.5;
                ShowInTaskbar = false;
                TopMost = true;
            }

            protected override bool ShowWithoutActivation => true;

            protected override CreateParams CreateParams
            {
                get
                {
                    var parameters = base.CreateParams;
                    // 透明鼠标命中并禁止激活，拖放才能继续命中残影后面的真实控件。
                    parameters.ExStyle |= 0x20 | 0x08000000;
                    return parameters;
                }
            }
        }
    }
}
