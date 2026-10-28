using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.Core.Actions;
using Triggernometry.Localization;

namespace Triggernometry.UI.CustomControls
{
    public partial class ActionEditor
    {
        private ColorSelector fontPreview;
        private Label fontSummary;

        /// <summary>在字体选择器下方添加字体摘要和实时预览。</summary>
        private Control WithFontPreview(Control picker)
        {
            // 将原来的字体选择控件包装成一个整体，三行依次放选择器、字体摘要、示例预览。
            // 整体作为配置表中的一个字段，宽度填满参数列，高度随内部控件调整。
            var panel = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, RowCount = 3 };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 3; i++) panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            // 去掉选择器自带的外边距，使三行的左右边缘对齐。
            picker.Dock = DockStyle.Fill;
            picker.Margin = Padding.Empty;
            // 摘要由字体名称、字号和样式实时组成，不再交给控件自动翻译。
            fontSummary = new Label { AutoSize = true, Dock = DockStyle.Fill, Margin = new Padding(0, 4, 0, 4), Tag = I18n.DoNotTranslate };
            // 复用颜色选择控件绘制示例文字，并允许从其菜单修改文字、描边和背景颜色。
            fontPreview = new ColorSelector
            {
                Dock = DockStyle.Fill, Height = 64, Margin = Padding.Empty,
                ChangeTextColor = true, ChangeTextOutlineColor = true, ChangeBackgroundColor = true
            };
            // 翻译预览控件的文字和菜单；菜单字体需要显式跟随编辑器字体。
            I18n.TranslateControl("", fontPreview);
            fontPreview.ContextMenuStrip.Font = Font;
            // 这里选出的颜色通过事件写回颜色输入框，与直接编辑输入框保持一致。
            fontPreview.ColorChanged += FontPreview_ColorChanged;
            panel.Controls.Add(picker, 0, 0);
            panel.Controls.Add(fontSummary, 0, 1);
            panel.Controls.Add(fontPreview, 0, 2);
            return panel;
        }

        /// <summary>将预览区选定的颜色同步到对应表达式。</summary>
        private void FontPreview_ColorChanged(object sender, ColorSelector.ColorChangedEventArgs e)
        {
            switch (e.ChangedType)
            {
                case ColorSelector.ColorChangedType.TextColor:
                    expressions[nameof(ActionOverlayText.ForeColor)].Expression = ExpressionTextBox.ColorToString(e.NewColor, Color.Black);
                    break;
                case ColorSelector.ColorChangedType.TextOutlineColor:
                    expressions[nameof(ActionOverlayText.OutlineColor)].Expression = ExpressionTextBox.ColorToString(e.NewColor, Color.Empty);
                    break;
                case ColorSelector.ColorChangedType.BackgroundColor:
                    expressions[nameof(ActionOverlayText.BackColor)].Expression = ExpressionTextBox.ColorToString(e.NewColor, Color.Transparent);
                    break;
            }
        }

        /// <summary>计算预览颜色，表达式无效时使用后备颜色。</summary>
        private Color EvaluatePreviewColor(string field, Color fallback)
        {
            var context = UiContext ?? Context.Unbound;
            try 
            { 
                return ExpressionTextBox.ParseColor(context.EvaluateStringExpression(null, context, expressions[field].Expression), fallback); 
            }
            catch
            { 
                return fallback; 
            }
        }

        /// <summary>读取当前编辑值并刷新字体和颜色预览。</summary>
        private void UpdateOverlayFontPreview()
        {
            // 只有文本悬浮窗的字体字段创建了预览控件，其他动作无需处理。
            if (fontPreview == null) return;
            // 使用当前编辑上下文计算预览表达式；没有上下文时使用未绑定上下文。
            var context = UiContext ?? Context.Unbound;
            // 从控件读取尚未保存的样式选择，使预览立即反映当前编辑值。
            var effect = (ActionOverlayText.EffectEnum)configurationEditor.GetEditor(nameof(ActionOverlayText.Effect)).GetValue();
            try
            {
                // 字体名称直接读取输入文本，字号则按数值表达式求值，并限制为至少 1 磅。
                string family = expressions[nameof(ActionOverlayText.Font)].Expression;
                float size = Math.Max(1, (float)context.EvaluateNumericExpression(null, context, expressions[nameof(ActionOverlayText.FontSize)].Expression));
                // Effect 还可能包含自定义描边等标记；这里只取 System.Drawing.Font 支持的四种样式位。
                FontStyle style = (FontStyle)((int)effect & (int)(FontStyle.Bold | FontStyle.Italic | FontStyle.Underline | FontStyle.Strikeout));
                // 仅在名称、字号或样式发生变化时创建新字体，避免每次刷新都分配 Font。
                // SampleFont 由预览控件持有，替换时会释放旧字体，不改变编辑器本身的界面字体。
                var current = fontPreview.SampleFont;
                if (current.Name != family || current.SizeInPoints != size || current.Style != style)
                    fontPreview.SampleFont = RealPlugin.CreateFontFromDefinition(family, size, effect);
                // 摘要使用实际创建出的字体；预览高度随字号增大，但限制在 48～160 像素之间。
                fontSummary.Text = DescribeFont(fontPreview.SampleFont);
                fontPreview.Height = Math.Max(48, Math.Min(160, fontPreview.SampleFont.Height + 12));
            }
            catch
            {
                // 输入未完成或表达式无效时，保留上一次有效的预览。
            }
            // 颜色单独求值，字体输入无效时仍可刷新颜色。求值失败时使用下面指定的后备颜色。
            // 直接设置预览字段，不触发用户选色事件，也不将求值结果写回原始表达式。
            fontPreview._textColor = EvaluatePreviewColor(nameof(ActionOverlayText.ForeColor), Color.Black);
            fontPreview._textOutlineColor = EvaluatePreviewColor(nameof(ActionOverlayText.OutlineColor), Color.Empty);
            fontPreview._backgroundColor = EvaluatePreviewColor(nameof(ActionOverlayText.BackColor), Color.Transparent);
            // 同步控件底色，再请求重绘示例文字、描边和背景。
            fontPreview.BackColor = fontPreview._backgroundColor;
            fontPreview.Invalidate();
        }

        /// <summary>将字体名称、字号和样式组合为摘要文本。</summary>
        private static string DescribeFont(Font font)
        {
            string description = font.Name + ", " + font.SizeInPoints.ToString(CultureInfo.InvariantCulture)
                + I18n.Translate("internal/ActionForm/textptunit", "pt");
            var styles = new List<string>();
            if (font.Bold) styles.Add(I18n.Translate("internal/ActionForm/textbold", "bold"));
            if (font.Italic) styles.Add(I18n.Translate("internal/ActionForm/textitalic", "italic"));
            if (font.Underline) styles.Add(I18n.Translate("internal/ActionForm/textunderline", "underline"));
            if (font.Strikeout) styles.Add(I18n.Translate("internal/ActionForm/textstrikeout", "strikeout"));
            return styles.Count == 0 ? description : description + " (" + string.Join(", ", styles) + ")";
        }

        /// <summary>重新载入动作草稿的值并更新悬浮窗预览。</summary>
        private void ReloadEditorValues()
        {
            isBinding = true;
            try { configurationEditor.ReloadValues(); }
            finally { isBinding = false; }
            UpdateOverlayTools();
            UpdateOverlayFontPreview();
        }
    }
}
