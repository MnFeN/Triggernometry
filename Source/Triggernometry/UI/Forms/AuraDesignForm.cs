using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Triggernometry.Core;

namespace Triggernometry.UI.Forms
{

    public partial class AuraDesignForm : Form
    {

        public bool blinky = false;

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        const int ResizeThreshold = 10;

        internal AuraContainerForm.AuraTypeEnum AuraType { get; set; }
        internal Core.Actions.ActionOverlayText.TextAlignment TextAlignment { get; set; }
        internal Font AuraFont { get; set; }

        internal SolidBrush Brush = null;

        internal Color TextColor { get; set; }
        internal Color OutlineColor { get; set; }
        internal bool UseOutline { get; set; }

        int DynaLeft { get { return 0; } }
        int DynaTop { get { return 0; } }
        int DynaWidth { get { return ClientSize.Width; } }
        int DynaHeight { get { return ClientSize.Height; } }
        int DynaRight { get { return DynaLeft + DynaWidth; } }
        int DynaBottom { get { return DynaTop + DynaHeight; } }

        Rectangle TopRct { get { return new Rectangle(DynaLeft, DynaTop, DynaWidth, ResizeThreshold); } }
        Rectangle LeftRct { get { return new Rectangle(DynaLeft, DynaTop, ResizeThreshold, DynaHeight); } }
        Rectangle BottomRct { get { return new Rectangle(DynaLeft, DynaBottom - ResizeThreshold, DynaWidth, ResizeThreshold); } }
        Rectangle RightRct { get { return new Rectangle(DynaRight - ResizeThreshold, DynaTop, ResizeThreshold, DynaHeight); } }
        Rectangle TopLeftRct { get { return new Rectangle(DynaLeft, DynaTop, ResizeThreshold, ResizeThreshold); } }
        Rectangle TopRightRct { get { return new Rectangle(DynaRight - ResizeThreshold, DynaTop, ResizeThreshold, ResizeThreshold); } }
        Rectangle BottomLeftRct { get { return new Rectangle(DynaLeft, DynaBottom - ResizeThreshold, ResizeThreshold, ResizeThreshold); } }
        Rectangle BottomRightRct { get { return new Rectangle(DynaRight - ResizeThreshold, DynaBottom - ResizeThreshold, ResizeThreshold, ResizeThreshold); } }

        private void setWidthToImageWidthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Width = picOverlayPreview.Image.Width;
        }

        private void setHeightToImageHeightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Height = picOverlayPreview.Image.Height;
        }

        private void applyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void discardChangesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        internal void SetImage(Bitmap i)
        {
            picOverlayPreview.Image = i;
        }

        internal void SetImageMode(PictureBoxSizeMode i)
        {
            picOverlayPreview.SizeMode = i;
        }

        internal PictureBoxSizeMode GetImageDisplay()
        {
            return picOverlayPreview.SizeMode;
        }

        private void onceToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (ctxDisplayNormal.Checked == true)
            {
                picOverlayPreview.SizeMode = PictureBoxSizeMode.Normal;
                ctxDisplayCentered.Checked = false;                
                ctxDisplayStretch.Checked = false;
                ctxDisplayZoomed.Checked = false;
            }
        }

        private void centeredToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ctxDisplayCentered.Checked == true)
            {
                picOverlayPreview.SizeMode = PictureBoxSizeMode.CenterImage;
                ctxDisplayNormal.Checked = false;                
                ctxDisplayStretch.Checked = false;
                ctxDisplayZoomed.Checked = false;
            }
        }

        private void stretchedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ctxDisplayStretch.Checked == true)
            {
                picOverlayPreview.SizeMode = PictureBoxSizeMode.StretchImage;
                ctxDisplayNormal.Checked = false;                
                ctxDisplayCentered.Checked = false;
                ctxDisplayZoomed.Checked = false;
            }
        }

        private void stretchedWithAspectRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ctxDisplayZoomed.Checked == true)
            {
                picOverlayPreview.SizeMode = PictureBoxSizeMode.Zoom;
                ctxDisplayNormal.Checked = false;                
                ctxDisplayStretch.Checked = false;
                ctxDisplayCentered.Checked = false;
            }
        }

        /// <summary>根据当前设计模式与文本样式更新右键菜单。</summary>
        private void ctxOverlayDesign_Opening(object sender, CancelEventArgs e)
        {
            switch (AuraType)
            {
                case AuraContainerForm.AuraTypeEnum.Image:
                    {
                        switch (picOverlayPreview.SizeMode)
                        {
                            case PictureBoxSizeMode.Normal:
                                ctxDisplayNormal.Checked = true;
                                break;
                            case PictureBoxSizeMode.CenterImage:
                                ctxDisplayCentered.Checked = true;
                                break;
                            case PictureBoxSizeMode.StretchImage:
                                ctxDisplayStretch.Checked = true;
                                break;
                            case PictureBoxSizeMode.Zoom:
                                ctxDisplayZoomed.Checked = true;
                                break;
                        }
                        ctxImage.Visible = true;
                        ctxText.Visible = false;
                    }
                    break;
                case AuraContainerForm.AuraTypeEnum.Text:
                    {
                        switch (TextAlignment)
                        {
                            case Core.Actions.ActionOverlayText.TextAlignment.TopLeft:
                                ctxTextAlignmentTopLeft.Checked = true;
                                break;
                            case Core.Actions.ActionOverlayText.TextAlignment.TopCenter:
                                ctxTextAlignmentTopCenter.Checked = true;
                                break;
                            case Core.Actions.ActionOverlayText.TextAlignment.TopRight:
                                ctxTextAlignmentTopRight.Checked = true;
                                break;
                            case Core.Actions.ActionOverlayText.TextAlignment.MiddleLeft:
                                ctxTextAlignmentMiddleLeft.Checked = true;
                                break;
                            case Core.Actions.ActionOverlayText.TextAlignment.MiddleCenter:
                                ctxTextAlignmentMiddleCenter.Checked = true;
                                break;
                            case Core.Actions.ActionOverlayText.TextAlignment.MiddleRight:
                                ctxTextAlignmentMiddleRight.Checked = true;
                                break;
                            case Core.Actions.ActionOverlayText.TextAlignment.BottomLeft:
                                ctxTextAlignmentBottomLeft.Checked = true;
                                break;
                            case Core.Actions.ActionOverlayText.TextAlignment.BottomCenter:
                                ctxTextAlignmentBottomCenter.Checked = true;
                                break;
                            case Core.Actions.ActionOverlayText.TextAlignment.BottomRight:
                                ctxTextAlignmentBottomRight.Checked = true;
                                break;
                        }
                        ctxImage.Visible = false;
                        ctxText.Visible = true;
                    }
                    break;
            }
        }

        private void textToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
        }

        private HatchBrush hb;

        private void onceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>将文本对齐方式切换为左上，并同步菜单勾选状态。</summary>
        private void ctxTextAlignmentTopLeft_Click(object sender, EventArgs e)
        {
            if (ctxTextAlignmentTopLeft.Checked == true)
            {
                TextAlignment = Core.Actions.ActionOverlayText.TextAlignment.TopLeft;
                ctxTextAlignmentTopCenter.Checked = false;
                ctxTextAlignmentTopRight.Checked = false;
                ctxTextAlignmentMiddleLeft.Checked = false;
                ctxTextAlignmentMiddleCenter.Checked = false;
                ctxTextAlignmentMiddleRight.Checked = false;
                ctxTextAlignmentBottomLeft.Checked = false;
                ctxTextAlignmentBottomCenter.Checked = false;
                ctxTextAlignmentBottomRight.Checked = false;
                Invalidate();
            }
        }

        /// <summary>将文本对齐方式切换为顶部居中，并同步菜单勾选状态。</summary>
        private void ctxTextAlignmentTopCenter_Click(object sender, EventArgs e)
        {
            if (ctxTextAlignmentTopCenter.Checked == true)
            {
                TextAlignment = Core.Actions.ActionOverlayText.TextAlignment.TopCenter;
                ctxTextAlignmentTopLeft.Checked = false;
                ctxTextAlignmentTopRight.Checked = false;
                ctxTextAlignmentMiddleLeft.Checked = false;
                ctxTextAlignmentMiddleCenter.Checked = false;
                ctxTextAlignmentMiddleRight.Checked = false;
                ctxTextAlignmentBottomLeft.Checked = false;
                ctxTextAlignmentBottomCenter.Checked = false;
                ctxTextAlignmentBottomRight.Checked = false;
                Invalidate();
            }
        }

        /// <summary>将文本对齐方式切换为右上，并同步菜单勾选状态。</summary>
        private void ctxTextAlignmentTopRight_Click(object sender, EventArgs e)
        {
            if (ctxTextAlignmentTopRight.Checked == true)
            {
                TextAlignment = Core.Actions.ActionOverlayText.TextAlignment.TopRight;
                ctxTextAlignmentTopLeft.Checked = false;
                ctxTextAlignmentTopCenter.Checked = false;
                ctxTextAlignmentMiddleLeft.Checked = false;
                ctxTextAlignmentMiddleCenter.Checked = false;
                ctxTextAlignmentMiddleRight.Checked = false;
                ctxTextAlignmentBottomLeft.Checked = false;
                ctxTextAlignmentBottomCenter.Checked = false;
                ctxTextAlignmentBottomRight.Checked = false;
                Invalidate();
            }
        }

        /// <summary>将文本对齐方式切换为左侧居中，并同步菜单勾选状态。</summary>
        private void ctxTextAlignmentMiddleLeft_Click(object sender, EventArgs e)
        {
            if (ctxTextAlignmentMiddleLeft.Checked == true)
            {
                TextAlignment = Core.Actions.ActionOverlayText.TextAlignment.MiddleLeft;
                ctxTextAlignmentTopLeft.Checked = false;
                ctxTextAlignmentTopCenter.Checked = false;
                ctxTextAlignmentTopRight.Checked = false;
                ctxTextAlignmentMiddleCenter.Checked = false;
                ctxTextAlignmentMiddleRight.Checked = false;
                ctxTextAlignmentBottomLeft.Checked = false;
                ctxTextAlignmentBottomCenter.Checked = false;
                ctxTextAlignmentBottomRight.Checked = false;
                Invalidate();
            }
        }

        /// <summary>将文本对齐方式切换为居中，并同步菜单勾选状态。</summary>
        private void ctxTextAlignmentMiddleCenter_Click(object sender, EventArgs e)
        {
            if (ctxTextAlignmentMiddleCenter.Checked == true)
            {
                TextAlignment = Core.Actions.ActionOverlayText.TextAlignment.MiddleCenter;
                ctxTextAlignmentTopLeft.Checked = false;
                ctxTextAlignmentTopCenter.Checked = false;
                ctxTextAlignmentTopRight.Checked = false;
                ctxTextAlignmentMiddleLeft.Checked = false;
                ctxTextAlignmentMiddleRight.Checked = false;
                ctxTextAlignmentBottomLeft.Checked = false;
                ctxTextAlignmentBottomCenter.Checked = false;
                ctxTextAlignmentBottomRight.Checked = false;
                Invalidate();
            }
        }

        /// <summary>将文本对齐方式切换为右侧居中，并同步菜单勾选状态。</summary>
        private void ctxTextAlignmentMiddleRight_Click(object sender, EventArgs e)
        {
            if (ctxTextAlignmentMiddleRight.Checked == true)
            {
                TextAlignment = Core.Actions.ActionOverlayText.TextAlignment.MiddleRight;
                ctxTextAlignmentTopLeft.Checked = false;
                ctxTextAlignmentTopCenter.Checked = false;
                ctxTextAlignmentTopRight.Checked = false;
                ctxTextAlignmentMiddleLeft.Checked = false;
                ctxTextAlignmentMiddleCenter.Checked = false;
                ctxTextAlignmentBottomLeft.Checked = false;
                ctxTextAlignmentBottomCenter.Checked = false;
                ctxTextAlignmentBottomRight.Checked = false;
                Invalidate();
            }
        }

        /// <summary>将文本对齐方式切换为左下，并同步菜单勾选状态。</summary>
        private void ctxTextAlignmentBottomLeft_Click(object sender, EventArgs e)
        {
            if (ctxTextAlignmentBottomLeft.Checked == true)
            {
                TextAlignment = Core.Actions.ActionOverlayText.TextAlignment.BottomLeft;
                ctxTextAlignmentTopLeft.Checked = false;
                ctxTextAlignmentTopCenter.Checked = false;
                ctxTextAlignmentTopRight.Checked = false;
                ctxTextAlignmentMiddleLeft.Checked = false;
                ctxTextAlignmentMiddleCenter.Checked = false;
                ctxTextAlignmentMiddleRight.Checked = false;
                ctxTextAlignmentBottomCenter.Checked = false;
                ctxTextAlignmentBottomRight.Checked = false;
                Invalidate();
            }
        }

        /// <summary>将文本对齐方式切换为底部居中，并同步菜单勾选状态。</summary>
        private void ctxTextAlignmentBottomCenter_Click(object sender, EventArgs e)
        {
            if (ctxTextAlignmentBottomCenter.Checked == true)
            {
                TextAlignment = Core.Actions.ActionOverlayText.TextAlignment.BottomCenter;
                ctxTextAlignmentTopLeft.Checked = false;
                ctxTextAlignmentTopCenter.Checked = false;
                ctxTextAlignmentTopRight.Checked = false;
                ctxTextAlignmentMiddleLeft.Checked = false;
                ctxTextAlignmentMiddleCenter.Checked = false;
                ctxTextAlignmentMiddleRight.Checked = false;
                ctxTextAlignmentBottomLeft.Checked = false;
                ctxTextAlignmentBottomRight.Checked = false;
                Invalidate();
            }
        }

        /// <summary>将文本对齐方式切换为右下，并同步菜单勾选状态。</summary>
        private void ctxTextAlignmentBottomRight_Click(object sender, EventArgs e)
        {
            if (ctxTextAlignmentBottomRight.Checked == true)
            {
                TextAlignment = Core.Actions.ActionOverlayText.TextAlignment.BottomRight;
                ctxTextAlignmentTopLeft.Checked = false;
                ctxTextAlignmentTopCenter.Checked = false;
                ctxTextAlignmentTopRight.Checked = false;
                ctxTextAlignmentMiddleLeft.Checked = false;
                ctxTextAlignmentMiddleCenter.Checked = false;
                ctxTextAlignmentMiddleRight.Checked = false;
                ctxTextAlignmentBottomLeft.Checked = false;
                ctxTextAlignmentBottomCenter.Checked = false;
                Invalidate();
            }
        }

        public AuraDesignForm(AuraContainerForm.AuraTypeEnum at)
        {
            InitializeComponent();
            AuraType = at;
            switch (AuraType)
            {
                case AuraContainerForm.AuraTypeEnum.Image:
                    picOverlayPreview.Visible = true;
                    break;
                case AuraContainerForm.AuraTypeEnum.Text:
                    picOverlayPreview.Visible = false;
                    Brush = new SolidBrush(Color.Black);
                    break;
            }
            DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            Disposed += AuraDesignForm_Disposed;
            hb = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.FromArgb(120, 120, 170), Color.FromArgb(150, 150, 200));
        }

        private void AuraDesignForm_Disposed(object sender, EventArgs e)
        {
            if (Brush != null)
            {
                Brush.Dispose();
                Brush = null;
            }
            hb.Dispose();
        }

        /// <summary>绘制设计器中的文本、描边和对齐预览。</summary>
        internal void PaintText(Graphics gr)
        {                        
            using (StringFormat sf = new StringFormat())
            {
                Rectangle r = new Rectangle(ClientRectangle.Location, ClientRectangle.Size);
                switch (TextAlignment)
                {
                    case Core.Actions.ActionOverlayText.TextAlignment.TopLeft:
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Near;                        
                        break;
                    case Core.Actions.ActionOverlayText.TextAlignment.TopCenter:
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Near;
                        break;
                    case Core.Actions.ActionOverlayText.TextAlignment.TopRight:
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Near;
                        break;
                    case Core.Actions.ActionOverlayText.TextAlignment.MiddleLeft:
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Center;
                        break;
                    case Core.Actions.ActionOverlayText.TextAlignment.MiddleCenter:
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        break;
                    case Core.Actions.ActionOverlayText.TextAlignment.MiddleRight:
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Center;
                        break;
                    case Core.Actions.ActionOverlayText.TextAlignment.BottomLeft:
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Far;
                        break;
                    case Core.Actions.ActionOverlayText.TextAlignment.BottomCenter:
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Far;
                        break;
                    case Core.Actions.ActionOverlayText.TextAlignment.BottomRight:
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Far;
                        break;
                }
                gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                if (UseOutline == true)
                {
                    Brush.Color = OutlineColor;
                    r.Y -= 1;
                    gr.DrawString("Test", AuraFont, Brush, r, sf);
                    r.Y += 2;
                    gr.DrawString("Test", AuraFont, Brush, r, sf);
                    r.Y -= 1;
                    r.X -= 1;
                    gr.DrawString("Test", AuraFont, Brush, r, sf);
                    r.X += 2;
                    gr.DrawString("Test", AuraFont, Brush, r, sf);
                    r.X -= 1;
                }
                Brush.Color = TextColor;
                gr.DrawString("Test", AuraFont, Brush, r, sf);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Rectangle r = new Rectangle(ClientRectangle.Left, ClientRectangle.Top, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            bool bl = blinky;
            g.DrawRectangle(bl == true ? Pens.Red : Pens.Yellow, r);
            r = new Rectangle(ClientRectangle.Left + 1, ClientRectangle.Top + 1, ClientRectangle.Width - 3, ClientRectangle.Height - 3);
            g.DrawRectangle(bl == true ? Pens.Red : Pens.Yellow, r);
            r = new Rectangle(ClientRectangle.Left + 2, ClientRectangle.Top + 2, ClientRectangle.Width - 5, ClientRectangle.Height - 5);
            g.DrawRectangle(bl == true ? Pens.Red : Pens.Yellow, r);
            if (AuraType == AuraContainerForm.AuraTypeEnum.Text)
            {
                PaintText(e.Graphics);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(hb, ClientRectangle);
        }

        private void tmrSelectionBlink_Tick(object sender, EventArgs e)
        {
            blinky = (blinky == false);
            Invalidate();
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == 0xA3)
            {
                message.Result = IntPtr.Zero;
                return;
            }
            else
            {
                base.WndProc(ref message);
            }
            if (message.Msg == 164)
            {
                ctxOverlayDesign.Show(this, PointToClient(Cursor.Position));
            }
            else if (message.Msg == 0x84) // WM_NCHITTEST
            {
                var cursor = this.PointToClient(Cursor.Position);
                if (TopLeftRct.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRightRct.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeftRct.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRightRct.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (TopRct.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (LeftRct.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (RightRct.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (BottomRct.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
                else
                {
                    if ((int)message.Result == 0x1)
                        message.Result = (IntPtr)0x2;
                }
            }
        }

    }

}
