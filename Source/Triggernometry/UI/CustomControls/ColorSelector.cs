using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Triggernometry.Localization;

namespace Triggernometry.UI.CustomControls
{

    public partial class ColorSelector : UserControl
    {
        private Font sampleFont;

        /// <summary>由控件持有并负责释放的独立预览字体。</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Font SampleFont
        {
            get => sampleFont ?? Font;
            set
            {
                if (sampleFont == value) return;
                sampleFont?.Dispose();
                sampleFont = value;
                Invalidate();
            }
        }

        public bool ChangeTextColor { get; set; }

        internal Color _textColor;
        public Color TextColor
        {
            get => _textColor;
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    Invalidate();
                    OnColorChanged(new ColorChangedEventArgs(value, ColorChangedType.TextColor));
                }
            }
        }

        public bool ChangeTextOutlineColor { get; set; }

        internal Color _textOutlineColor;
        public Color TextOutlineColor
        {
            get => _textOutlineColor;
            set
            {
                if (_textOutlineColor != value)
                {
                    _textOutlineColor = value;
                    Invalidate();
                    OnColorChanged(new ColorChangedEventArgs(value, ColorChangedType.TextOutlineColor));
                }
            }
        }

        public bool ChangeBackgroundColor { get; set; }

        internal Color _backgroundColor;
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (_backgroundColor != value)
                {
                    _backgroundColor = value;
                    BackColor = value;
                    Invalidate();
                    OnColorChanged(new ColorChangedEventArgs(value, ColorChangedType.BackgroundColor));
                }
            }
        }

        private StringFormat stringFormat = null;
        private SolidBrush brush = null;

        public ColorSelector()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Disposed += ColorSelector_Disposed;
            brush = new SolidBrush(Color.Black);
            stringFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
        }

        /// <summary>释放预览字体、画刷和文本格式资源。</summary>
        private void ColorSelector_Disposed(object sender, EventArgs e)
        {
            sampleFont?.Dispose();
            sampleFont = null;
            brush?.Dispose();
            brush = null;
            stringFormat?.Dispose();
            stringFormat = null;
        }

        /// <summary>绘制当前字体的颜色和描边预览。</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics gr = e.Graphics;
            // 使用 ClearType 呈现小字号字体的平滑笔画。
            gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Rectangle r = new Rectangle(ClientRectangle.Location, ClientRectangle.Size);
            r.Width -= button1.Width;
            string smpl = I18n.Translate("internal/ColorSelector/sample", "Sample");
            if (ChangeTextOutlineColor == true)
            {
                brush.Color = TextOutlineColor;
                r.Y -= 1;
                gr.DrawString(smpl, SampleFont, brush, r, stringFormat);
                r.Y += 2;
                gr.DrawString(smpl, SampleFont, brush, r, stringFormat);
                r.Y -= 1;
                r.X -= 1;
                gr.DrawString(smpl, SampleFont, brush, r, stringFormat);
                r.X += 2;
                gr.DrawString(smpl, SampleFont, brush, r, stringFormat);
                r.X -= 1;
            }
            if (ChangeTextColor == true)
            {
                brush.Color = TextColor;
                gr.DrawString(smpl, SampleFont, brush, r, stringFormat);
            }
        }

        private void setTextColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = TextColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                TextColor = colorDialog1.Color;
            }
        }

        private void setTextOutlineColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = TextOutlineColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                TextOutlineColor = colorDialog1.Color;
            }
        }

        private void setBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = BackgroundColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                BackgroundColor = colorDialog1.Color;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(button1, new Point(0, 0));
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ctxSetTextColor.Visible = ChangeTextColor;
            ctxSetTextOutline.Visible = ChangeTextOutlineColor;
            ctxSetBackground.Visible = ChangeBackgroundColor;
            toolStripSeparator1.Visible = ChangeBackgroundColor;
            ctxSetBackgroundTransparent.Visible = ChangeBackgroundColor;
        }

        private void setBackgroundAsTransparentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BackgroundColor = Color.Transparent;
        }

        public enum ColorChangedType
        {
            TextColor,
            TextOutlineColor,
            BackgroundColor
        }

        public class ColorChangedEventArgs : EventArgs
        {
            public Color NewColor { get; private set; }
            public ColorChangedType ChangedType { get; private set; }

            public ColorChangedEventArgs(Color newColor, ColorChangedType changedType)
            {
                NewColor = newColor;
                ChangedType = changedType;
            }
        }

        public event EventHandler<ColorChangedEventArgs> ColorChanged;

        protected virtual void OnColorChanged(ColorChangedEventArgs e)
        {
            ColorChanged?.Invoke(this, e);
        }

    }

}
