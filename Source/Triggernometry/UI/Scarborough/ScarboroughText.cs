using Scarborough.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.Core.Actions;

namespace Scarborough
{

    class ScarboroughText : ScarboroughItem
    {

        internal string TextExpression { get; set; }
        internal bool NeedFont { get; set; }
        internal string FontName { get; set; }

        private ActionOverlayText.TextAlignment _TextAlignment;
        internal ActionOverlayText.TextAlignment TextAlignment
        {
            get
            {
                return _TextAlignment;
            }
            set
            {
                if (value != _TextAlignment)
                {
                    Changed = true;
                    _TextAlignment = value;
                    layoutChanged = true;
                }
            }
        }

        private bool _UseOutline;
        internal bool UseOutline
        {
            get
            {
                return _UseOutline;
            }
            set
            {
                if (value != _UseOutline)
                {
                    Changed = true;
                    _UseOutline = value;
                }
            }
        }

        private float _FontSize;
        internal float FontSize
        {
            get
            {
                return _FontSize;
            }
            set
            {
                if (value != _FontSize)
                {
                    Changed = true;
                    _FontSize = value;
                    layoutChanged = true;
                }
            }
        }

        private string _Text;
        internal string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                if (value != _Text)
                {
                    Changed = true;
                    _Text = value;
                    layoutChanged = true;
                }
            }
        }

        private System.Drawing.Color _TextColor;
        internal System.Drawing.Color TextColor
        {
            get
            {
                return _TextColor;
            }
            set
            {
                if (value != _TextColor)
                {
                    Changed = true;
                    _TextColor = value;
                }
            }
        }

        private System.Drawing.Color _OutlineColor;
        internal System.Drawing.Color OutlineColor
        {
            get
            {
                return _OutlineColor;
            }
            set
            {
                if (value != _OutlineColor)
                {
                    Changed = true;
                    _OutlineColor = value;
                }
            }
        }

        private System.Drawing.Color _BackgroundColor;
        internal System.Drawing.Color BackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                if (value != _BackgroundColor)
                {
                    Changed = true;
                    _BackgroundColor = value;
                    if (_BackgroundColor != System.Drawing.Color.Transparent)
                    {
                        _bgColor.R = _BackgroundColor.R / 255.0f;
                        _bgColor.G = _BackgroundColor.G / 255.0f;
                        _bgColor.B = _BackgroundColor.B / 255.0f;
                    }
                    else
                    {
                        _bgColor = new Color();
                    }
                }
            }
        }

        private System.Drawing.FontStyle _FontStyle;
        internal System.Drawing.FontStyle FontStyle
        {
            get
            {
                return _FontStyle;
            }
            set
            {
                if (value != _FontStyle)
                {
                    Changed = true;
                    _FontStyle = value;
                    layoutChanged = true;
                }
            }
        }

        private Font TextFont { get; set; } = null;
        private SolidBrush TextBrush { get; set; } = null;
        private SharpDX.DirectWrite.TextLayout textLayout;
        private bool layoutChanged = true;
        private int layoutWidth, layoutHeight;

        public ScarboroughText(Scarborough own) : base(own)
        {
        }

        public override void Free()
        {
            textLayout?.Dispose();
            textLayout = null;
            layoutChanged = true;
            if (TextFont != null)
            {
                TextFont.Dispose();
                TextFont = null;
            }
            if (TextBrush != null)
            {
                TextBrush.Dispose();
                TextBrush = null;
            }
        }

        public void LoadFontOnDemand()
        {
            Free();
            LoadFontData(plug);
            CreateBrush();
        }

        public void CreateBrush()
        {
            TextBrush = new SolidBrush(_graphics.GetRenderTarget());            
        }

        /// <summary>更新文本布局并使用 Direct2D 绘制悬浮窗。</summary>
        public override void Render()
        {
            if (NeedFont == true)
            {
                if (_window == null)
                {
                    if (AdjustSurface() == false)
                    {
                        AdjustVisibility();
                        return;
                    }
                }
                LoadFontOnDemand();
                NeedFont = false;
                NeedRender = true;
            }
            if (Owner.RenderingActive == false)
            {
                if (WasHidden == false)
                {
                    NeedRender = true;
                    WasHidden = true;
                }
            }
            else
            {
                if (WasHidden == true)
                {
                    NeedRender = true;
                    WasHidden = false;
                }
            }
            if (NeedRender == false)
            {
                return;
            }
            if (AdjustSurface() == false)
            {
                AdjustVisibility();
                return;
            }
            AdjustVisibility();
            NeedRender = false;
            _graphics.BeginScene();
            if (Owner.RenderingActive == false || InvalidSize == true)
            {
                Color tempBgColor = new Color();
                _graphics.ClearScene(tempBgColor);
                _graphics.EndScene();
                return;
            }
            if (BackgroundColor != System.Drawing.Color.Transparent)
            {
                _bgColor.A = Opacity / 100.0f;
            }
            _graphics.ClearScene(_bgColor);
            var layout = GetTextLayout();
            if (layout != null)
            {
                if (UseOutline == true)
                {
                    TextBrush.Color = new Color(OutlineColor.R, OutlineColor.G, OutlineColor.B, Opacity / 100.0f);
                    _graphics.DrawTextLayout(layout, TextBrush, 1, 0);
                    _graphics.DrawTextLayout(layout, TextBrush, -1, 0);
                    _graphics.DrawTextLayout(layout, TextBrush, 0, 1);
                    _graphics.DrawTextLayout(layout, TextBrush, 0, -1);
                }
                TextBrush.Color = new Color(TextColor.R, TextColor.G, TextColor.B, Opacity / 100.0f);
                _graphics.DrawTextLayout(layout, TextBrush, 0, 0);
            }
            _graphics.EndScene();
        }

        /// <summary>仅在文字、字体、对齐或尺寸改变时重建排版，移动和颜色变化复用现有排版。</summary>
        private SharpDX.DirectWrite.TextLayout GetTextLayout()
        {
            if (!layoutChanged && layoutWidth == Width && layoutHeight == Height)
                return textLayout;

            textLayout?.Dispose();
            textLayout = null;
            SharpDX.DirectWrite.ParagraphAlignment pa = SharpDX.DirectWrite.ParagraphAlignment.Center;
            SharpDX.DirectWrite.TextAlignment ta = SharpDX.DirectWrite.TextAlignment.Center;
            switch (TextAlignment)
            {
                case ActionOverlayText.TextAlignment.TopLeft:
                    pa = SharpDX.DirectWrite.ParagraphAlignment.Near;
                    ta = SharpDX.DirectWrite.TextAlignment.Leading;
                    break;
                case ActionOverlayText.TextAlignment.TopCenter:
                    pa = SharpDX.DirectWrite.ParagraphAlignment.Near;
                    break;
                case ActionOverlayText.TextAlignment.TopRight:
                    pa = SharpDX.DirectWrite.ParagraphAlignment.Near;
                    ta = SharpDX.DirectWrite.TextAlignment.Trailing;
                    break;
                case ActionOverlayText.TextAlignment.MiddleLeft:
                    ta = SharpDX.DirectWrite.TextAlignment.Leading;
                    break;
                case ActionOverlayText.TextAlignment.MiddleCenter:
                    break;
                case ActionOverlayText.TextAlignment.MiddleRight:
                    ta = SharpDX.DirectWrite.TextAlignment.Trailing;
                    break;
                case ActionOverlayText.TextAlignment.BottomLeft:
                    pa = SharpDX.DirectWrite.ParagraphAlignment.Far;
                    ta = SharpDX.DirectWrite.TextAlignment.Leading;
                    break;
                case ActionOverlayText.TextAlignment.BottomCenter:
                    pa = SharpDX.DirectWrite.ParagraphAlignment.Far;
                    break;
                case ActionOverlayText.TextAlignment.BottomRight:
                    pa = SharpDX.DirectWrite.ParagraphAlignment.Far;
                    ta = SharpDX.DirectWrite.TextAlignment.Trailing;
                    break;
            }
            textLayout = _graphics.CreateTextLayout(TextFont, FontStyle, FontSize * 1.3f, pa, ta, Width, Height, Text);
            layoutWidth = Width;
            layoutHeight = Height;
            layoutChanged = false;
            return textLayout;
        }

        /// <summary>绘图目标重建后重新加载字体和画刷，并在下一帧重绘。</summary>
        protected override void OnRenderTargetRecreated()
        {
            base.OnRenderTargetRecreated();
            NeedFont = true;
        }

        internal void LoadFontData(RealPlugin plug)
        {
            TextFont = _graphics.CreateFont(
                FontName,
                FontSize,
                (FontStyle & System.Drawing.FontStyle.Bold) == System.Drawing.FontStyle.Bold,
                (FontStyle & System.Drawing.FontStyle.Italic) == System.Drawing.FontStyle.Italic,
                true
            );
        }

        public override bool InternalLogic(int numTicks)
        {
            while (numTicks > 0)
            {
                if (GenericLogic() == false)
                {
                    return false;
                }
                numTicks--;
            }
            Text = EvaluateStringExpression(ctx, TextExpression);
            return true;
        }

    }

}
