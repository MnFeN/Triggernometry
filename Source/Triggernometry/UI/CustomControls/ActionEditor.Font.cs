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
        private (string Family, string Size, ActionOverlayText.EffectEnum Effect)? previewFontRequest;
        private string previewTextColor, previewOutlineColor, previewBackColor;

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
                    expressions[nameof(ActionOverlayText.TextColor)].Expression = ExpressionTextBox.ColorToString(e.NewColor, Color.Black);
                    break;
                case ColorSelector.ColorChangedType.TextOutlineColor:
                    expressions[nameof(ActionOverlayText.OutlineColor)].Expression = ExpressionTextBox.ColorToString(e.NewColor, Color.Empty);
                    break;
                case ColorSelector.ColorChangedType.BackgroundColor:
                    expressions[nameof(ActionOverlayText.BackColor)].Expression = ExpressionTextBox.ColorToString(e.NewColor, Color.Transparent);
                    break;
            }
        }

        /// <summary>仅重新计算改动的颜色表达式，输入未完成时使用后备颜色。</summary>
        private Color UpdatePreviewColor(string field, Color fallback, Color current, ref string previous, ref bool changed, bool force)
        {
            string expression = expressions[field].Expression;
            if (!force && expression == previous)
                return current;
            previous = expression;
            changed = true;
            var context = UiContext ?? Context.Unbound;
            try { return ExpressionTextBox.ParseColor(context.EvaluateStringExpression(null, context, expression), fallback); }
            catch { return fallback; }
        }

        /// <summary>按改动字段刷新预览，不因编辑无关参数反复求值或创建字体。</summary>
        private void UpdateOverlayFontPreview(bool force = false)
        {
            if (fontPreview == null)
                return;
            var effect = (ActionOverlayText.EffectEnum)configurationEditor.GetEditor(nameof(ActionOverlayText.Effect)).GetValue();
            var request = (
                expressions[nameof(ActionOverlayText.Font)].Expression, 
                expressions[nameof(ActionOverlayText.FontSize)].Expression, 
                effect);
            bool changed = force || previewFontRequest != request;
            if (changed)
            {
                // 缓存用户请求，而非系统后备字体的名称：不存在的字体也只尝试创建一次。
                previewFontRequest = request;
                var context = UiContext ?? Context.Unbound;
                try
                {
                    float size = Math.Max(1, (float)context.EvaluateNumericExpression(null, context, request.Item2));
                    var current = fontPreview.SampleFont;
                    if (current.Name != request.Item1 || current.SizeInPoints != size || current.Style != RealPlugin.GetFontStyle(effect))
                        fontPreview.SampleFont = RealPlugin.CreateFontFromDefinition(request.Item1, size, effect);
                    fontSummary.Text = DescribeFont(fontPreview.SampleFont);
                    fontPreview.Height = Math.Max(48, Math.Min(160, fontPreview.SampleFont.Height + 12));
                }
                catch
                {
                    // 保留上一次有效字体；下次相关输入变动或重新载入时再尝试。
                }
            }
            fontPreview._textColor = UpdatePreviewColor(nameof(ActionOverlayText.TextColor), Color.Black,
                fontPreview._textColor, ref previewTextColor, ref changed, force);
            fontPreview._textOutlineColor = UpdatePreviewColor(nameof(ActionOverlayText.OutlineColor), Color.Empty,
                fontPreview._textOutlineColor, ref previewOutlineColor, ref changed, force);
            fontPreview._backgroundColor = UpdatePreviewColor(nameof(ActionOverlayText.BackColor), Color.Transparent,
                fontPreview._backgroundColor, ref previewBackColor, ref changed, force);
            if (!changed)
                return;
            fontPreview.BackColor = fontPreview._backgroundColor;
            fontPreview.Invalidate();
        }

        /// <summary>将字体名称、字号和样式组合为摘要文本。</summary>
        private static string DescribeFont(Font font)
        {
            string description = font.Name + ", " + font.SizeInPoints.ToString(CultureInfo.InvariantCulture)
                + I18n.Translate("internal/ActionForm/textptunit", "pt");
            var styles = new List<string>();
            if (font.Bold)
                styles.Add(I18n.Translate("internal/ActionForm/textbold", "bold"));
            if (font.Italic)
                styles.Add(I18n.Translate("internal/ActionForm/textitalic", "italic"));
            if (font.Underline)
                styles.Add(I18n.Translate("internal/ActionForm/textunderline", "underline"));
            if (font.Strikeout)
                styles.Add(I18n.Translate("internal/ActionForm/textstrikeout", "strikeout"));
            return styles.Count == 0 ? description : description + " (" + string.Join(", ", styles) + ")";
        }

        /// <summary>重新载入动作草稿的值并更新悬浮窗预览。</summary>
        private void ReloadEditorValues()
        {
            isBinding = true;
            try { configurationEditor.ReloadValues(); }
            finally { isBinding = false; }
            UpdateOverlayTools();
            UpdateOverlayFontPreview(true);
        }
    }
}
