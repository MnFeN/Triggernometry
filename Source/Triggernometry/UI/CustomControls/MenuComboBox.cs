using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Triggernometry.UI.CustomControls
{
    /// <summary>具有 ComboBox 外观、可弹出分级菜单的 ContextMenuStrip 选择按钮。</summary>
    public class MenuComboBox : Button
    {
        private ContextMenuStrip dropDown;
        private bool skipNextOpen;
        private int wheelDelta;

        /// <summary>点击选择器时展开的分级菜单。</summary>
        public ContextMenuStrip DropDown
        {
            get => dropDown;
            set
            {
                if (dropDown != null) dropDown.Closed -= DropDown_Closed;
                dropDown = value;
                if (dropDown != null) dropDown.Closed += DropDown_Closed;
            }
        }

        /// <summary>初始化具有下拉框外观和辅助功能角色的菜单按钮。</summary>
        public MenuComboBox()
        {
            AutoSize = true;
            UseMnemonic = false;
            AccessibleRole = AccessibleRole.ComboBox;
            DoubleBuffered = true;
        }

        /// <summary>展开鼠标所指分类，并收起同级分类的次级菜单。</summary>
        internal static void Category_MouseEnter(object sender, EventArgs e)
        {
            var category = (ToolStripMenuItem)sender;
            foreach (ToolStripMenuItem sibling in category.Owner.Items)
            {
                if (sibling != category)
                    sibling.HideDropDown();
            }
            category.Select();
            if (category.HasDropDownItems)
                category.ShowDropDown();
        }

        /// <summary>按当前字体和下拉箭头宽度计算首选尺寸。</summary>
        public override Size GetPreferredSize(Size proposedSize)
            => new Size(base.GetPreferredSize(proposedSize).Width + SystemInformation.VerticalScrollBarWidth, Font.Height + 8);

        /// <summary>响应按钮点击并切换下拉菜单。</summary>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (skipNextOpen) { skipNextOpen = false; return; }
            ShowDropDown();
        }

        /// <summary>按菜单顺序用滚轮切换可用叶子选项，保留不足一格的滚动量；到达首尾时停止。</summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e is HandledMouseEventArgs handled)
                handled.Handled = true;
            base.OnMouseWheel(e);
            if (!Enabled || dropDown == null || dropDown.Visible)
                return;

            wheelDelta += e.Delta;
            int steps = wheelDelta / SystemInformation.MouseWheelScrollDelta;
            wheelDelta %= SystemInformation.MouseWheelScrollDelta;
            if (steps == 0)
                return;

            var choices = new List<ToolStripMenuItem>();
            void CollectChoices(ToolStripItemCollection items)
            {
                foreach (ToolStripItem item in items)
                {
                    if (!(item is ToolStripMenuItem choice) || !choice.Available || !choice.Enabled)
                        continue;
                    if (choice.HasDropDownItems)
                        CollectChoices(choice.DropDownItems);
                    else
                        choices.Add(choice);
                }
            }
            CollectChoices(dropDown.Items);
            int current = choices.FindIndex(choice => choice.Checked);
            if (current < 0)
                return;
            int next = Math.Max(0, Math.Min(choices.Count - 1, current - steps));
            if (next != current)
                choices[next].PerformClick();
        }

        /// <summary>在按钮下方显示菜单，已展开时将其关闭。</summary>
        private void ShowDropDown()
        {
            if (!Enabled || dropDown == null) return;
            if (dropDown.Visible) { dropDown.Close(); return; }
            dropDown.Font = Font;
            dropDown.Show(this, new Point(0, Height));
            Invalidate();
        }

        /// <summary>在菜单关闭后更新按钮状态。</summary>
        private void DropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            // 再次点击选择器时只关闭菜单，避免同一次点击立即将其重新打开。
            skipNextOpen = e.CloseReason == ToolStripDropDownCloseReason.AppClicked && ClientRectangle.Contains(PointToClient(MousePosition));
            Invalidate();
        }

        /// <summary>处理展开菜单的方向键、F4 和 Alt 组合键。</summary>
        protected override bool ProcessCmdKey(ref Message message, Keys keyData)
        {
            if (keyData == Keys.F4 || keyData == (Keys.Alt | Keys.Down) || keyData == Keys.Down)
            {
                skipNextOpen = false;
                ShowDropDown();
                return true;
            }
            return base.ProcessCmdKey(ref message, keyData);
        }

        /// <summary>使用系统主题绘制 ComboBox 外观及当前交互状态。</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            ComboBoxState state = !Enabled 
                ? ComboBoxState.Disabled 
                : dropDown?.Visible == true 
                    ? ComboBoxState.Pressed
                    : ClientRectangle.Contains(PointToClient(MousePosition)) 
                        ? ComboBoxState.Hot 
                        : ComboBoxState.Normal;
            int width = SystemInformation.VerticalScrollBarWidth;
            var arrow = new Rectangle(Width - width, 0, width, Height);
            var text = new Rectangle(4, 2, Math.Max(0, Width - width - 8), Math.Max(0, Height - 4));
            if (ComboBoxRenderer.IsSupported)
            {
                ComboBoxRenderer.DrawTextBox(e.Graphics, ClientRectangle, state);
                ComboBoxRenderer.DrawDropDownButton(e.Graphics, arrow, state);
            }
            else
            {
                // 未启用系统主题时使用基础绘制接口。
                e.Graphics.Clear(Enabled ? SystemColors.Window : SystemColors.Control);
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, SystemColors.WindowFrame, ButtonBorderStyle.Solid);
                ControlPaint.DrawComboButton(e.Graphics, arrow, Enabled ? ButtonState.Normal : ButtonState.Inactive);
            }
            TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;
            TextRenderer.DrawText(e.Graphics, Text, Font, text, Enabled ? ForeColor : SystemColors.GrayText, flags);
            if (Focused && ShowFocusCues)
                ControlPaint.DrawFocusRectangle(e.Graphics, text);
        }

        /// <summary>解除下拉菜单的关闭事件订阅。</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && dropDown != null) dropDown.Closed -= DropDown_Closed;
            base.Dispose(disposing);
        }
    }
}
