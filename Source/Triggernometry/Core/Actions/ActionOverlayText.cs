using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;
using Triggernometry.Utilities;
using Autofill = Triggernometry.UI.CustomControls.ExpressionTextBox.AutofillTypeEnum;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Text overlay operations
    /// </summary>
    [Action("Text aura operation", ActionAttribute.CategoryType.Display)]
    [XmlRoot(ElementName = "ActionOverlayText")]
    public class ActionOverlayText : ActionBase
    {

        #region Properties

        // todo probably needs a custom property editor

        /// <summary>
        /// Text overlay operations
        /// </summary>
        public enum OperationEnum
        {
            /// <summary>
            /// Activate text overlay
            /// </summary>
            Activate,
            /// <summary>
            /// Deactive text overlay with matching name
            /// </summary>
            Deactivate,
            /// <summary>
            /// Deactive all text overlays
            /// </summary>
            DeactivateAll,
            /// <summary>
            /// Deactive all text overlays with name matching given regex
            /// </summary>
            DeactivateRegex,
            /// <summary>
            /// Deactive all text overlays from specified trigger
            /// </summary>
            DeactivateTrigger
        }

        /// <summary>
        /// Text alignment within area
        /// </summary>
        public enum TextAlignment
        {
            TopLeft,
            TopCenter,
            TopRight,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            BottomLeft,
            BottomCenter,
            BottomRight
        }

        /// <summary>
        /// Text effect flags
        /// </summary>
        [Flags]
        public enum EffectEnum
        {
            None = 0,
            Bold = 1,
            Italic = 2,
            Underline = 4,
            Strikeout = 8,
            Outline = 16
        }

        /// <summary>
        /// Type of the text overlay operation
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Operation",
            ChoiceLabels = new[]
        {
            "Activate or modify the specified text aura",
            "Deactivate the specified text aura",
            "Deactivate all text auras",
            "Deactivate text auras by regular expression",
        })]
        public OperationEnum Operation { get; set; } = OperationEnum.Activate;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.Activate);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// Alignment of text within overlay area
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(7, Label = "Text alignment",
            ChoiceLabels = new[]
        {
            "Top left",
            "Top center",
            "Top right",
            "Middle left",
            "Middle center",
            "Middle right",
            "Bottom left",
            "Bottom center",
            "Bottom right",
        })]
        public TextAlignment Alignment { get; set; } = TextAlignment.MiddleCenter;

        [XmlAttribute("Alignment")]
        public string Xml_Alignment
        {
            get => XmlAttr.Enum(Alignment, TextAlignment.MiddleCenter);
            set => Alignment = XmlAttr.Enum<TextAlignment>(value);
        }

        /// <summary>
        /// Text display effect
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumFlagsEditor(6, Label = "Text effect")]
        public EffectEnum Effect { get; set; } = EffectEnum.None;

        [XmlAttribute("Effect")]
        public string Xml_Effect
        {
            get => XmlAttr.Enum(Effect, EffectEnum.None);
            set => Effect = XmlAttr.Enum<EffectEnum>(value);
        }

        /// <summary>
        /// Numeric expression for the font size, evaluated when the overlay is activated
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(5, EditorExpressionType.Numeric, Label = "Font size")]
        public string FontSize { get; set; } = "10";

        [XmlAttribute("FontSize")]
        public string Xml_FontSize
        {
            get => XmlAttr.String(FontSize, "10", omitWhitespace: false);
            set => FontSize = value;
        }

        /// <summary>
        /// Color of the text
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ColorEditor(8, Label = "Text color")]
        public string ForeColor { get; set; } = "";

        [XmlAttribute("ForeColor")]
        public string Xml_ForeColor
        {
            get => XmlAttr.String(ForeColor, "", omitWhitespace: false);
            set => ForeColor = value;
        }

        /// <summary>
        /// Background color of the overlay
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ColorEditor(9, Label = "Background color")]
        public string BackColor { get; set; } = "";

        [XmlAttribute("BackColor")]
        public string Xml_BackColor
        {
            get => XmlAttr.String(BackColor, "", omitWhitespace: false);
            set => BackColor = value;
        }

        /// <summary>
        /// Color of the text outline
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ColorEditor(10, Label = "Outline color")]
        public string OutlineColor { get; set; } = "";

        [XmlAttribute("OutlineColor")]
        public string Xml_OutlineColor
        {
            get => XmlAttr.String(OutlineColor, "", omitWhitespace: false);
            set => OutlineColor = value;
        }

        /// <summary>
        /// Name of the text overlay
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, Label = "Unique identifier")]
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name, "", omitWhitespace: false);
            set => Name = value;
        }

        /// <summary>
        /// Text expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(3, Label = "Text to display")]
        public string Text { get; set; } = "";

        [XmlAttribute("Text")]
        public string Xml_Text
        {
            get => XmlAttr.String(Text, "", omitWhitespace: false);
            set => Text = value;
        }

        /// <summary>
        /// Exoression for initializing overlay X position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(11, EditorExpressionType.Numeric, Label = "X location expression",
            LabelQualifier = "Initial values")]
        public string XIniExpression { get; set; } = "";

        [XmlAttribute("XIniExpression")]
        public string Xml_XIniExpression
        {
            get => XmlAttr.String(XIniExpression, "", omitWhitespace: false);
            set => XIniExpression = value;
        }

        /// <summary>
        /// Exoression for initializing overlay Y position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(13, EditorExpressionType.Numeric, Label = "Y location expression",
            LabelQualifier = "Initial values")]
        public string YIniExpression { get; set; } = "";

        [XmlAttribute("YIniExpression")]
        public string Xml_YIniExpression
        {
            get => XmlAttr.String(YIniExpression, "", omitWhitespace: false);
            set => YIniExpression = value;
        }

        /// <summary>
        /// Exoression for initializing overlay width
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(15, EditorExpressionType.Numeric, Label = "Width expression",
            LabelQualifier = "Initial values")]
        public string WIniExpression { get; set; } = "";

        [XmlAttribute("WIniExpression")]
        public string Xml_WIniExpression
        {
            get => XmlAttr.String(WIniExpression, "", omitWhitespace: false);
            set => WIniExpression = value;
        }

        /// <summary>
        /// Exoression for initializing overlay height
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(17, EditorExpressionType.Numeric, Label = "Height expression",
            LabelQualifier = "Initial values")]
        public string HIniExpression { get; set; } = "";

        [XmlAttribute("HIniExpression")]
        public string Xml_HIniExpression
        {
            get => XmlAttr.String(HIniExpression, "", omitWhitespace: false);
            set => HIniExpression = value;
        }

        /// <summary>
        /// Exoression for initializing overlay opacity
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(19, EditorExpressionType.Numeric, Label = "Opacity expression",
            LabelQualifier = "Initial values")]
        public string OIniExpression { get; set; } = "";

        [XmlAttribute("OIniExpression")]
        public string Xml_OIniExpression
        {
            get => XmlAttr.String(OIniExpression, "", omitWhitespace: false);
            set => OIniExpression = value;
        }

        /// <summary>
        /// Exoression for updating overlay X position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(12, EditorExpressionType.Numeric, Label = "X location expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string XTickExpression { get; set; } = "";

        [XmlAttribute("XTickExpression")]
        public string Xml_XTickExpression
        {
            get => XmlAttr.String(XTickExpression, "", omitWhitespace: false);
            set => XTickExpression = value;
        }

        /// <summary>
        /// Exoression for updating overlay Y position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(14, EditorExpressionType.Numeric, Label = "Y location expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string YTickExpression { get; set; } = "";

        [XmlAttribute("YTickExpression")]
        public string Xml_YTickExpression
        {
            get => XmlAttr.String(YTickExpression, "", omitWhitespace: false);
            set => YTickExpression = value;
        }

        /// <summary>
        /// Exoression for updating overlay width
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(16, EditorExpressionType.Numeric, Label = "Width expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string WTickExpression { get; set; } = "";

        [XmlAttribute("WTickExpression")]
        public string Xml_WTickExpression
        {
            get => XmlAttr.String(WTickExpression, "", omitWhitespace: false);
            set => WTickExpression = value;
        }

        /// <summary>
        /// Exoression for updating overlay height
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(18, EditorExpressionType.Numeric, Label = "Height expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string HTickExpression { get; set; } = "";

        [XmlAttribute("HTickExpression")]
        public string Xml_HTickExpression
        {
            get => XmlAttr.String(HTickExpression, "", omitWhitespace: false);
            set => HTickExpression = value;
        }

        /// <summary>
        /// Exoression for updating overlay opacity
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(20, EditorExpressionType.Numeric, Label = "Opacity expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string OTickExpression { get; set; } = "";

        [XmlAttribute("OTickExpression")]
        public string Xml_OTickExpression
        {
            get => XmlAttr.String(OTickExpression, "", omitWhitespace: false);
            set => OTickExpression = value;
        }

        /// <summary>
        /// Exoression for checking overlay life cycle
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(21, EditorExpressionType.Numeric, Label = "Time-to-live expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string TTLTickExpression { get; set; } = "";

        [XmlAttribute("TTLTickExpression")]
        public string Xml_TTLTickExpression
        {
            get => XmlAttr.String(TTLTickExpression, "", omitWhitespace: false);
            set => TTLTickExpression = value;
        }

        /// <summary>
        /// Name of the font to use
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [FontEditor(4, Label = "Font")]
        public string Font { get; set; } = "";

        [XmlAttribute("Font")]
        public string Xml_Font
        {
            get => XmlAttr.String(Font, "", omitWhitespace: false);
            set => Font = value;
        }

        #endregion


        public override Action CreateEditorRules(ConfigEditor editor)
        {
            var operation = editor.GetEditor(nameof(Operation));
            var name = editor.GetEditor(nameof(Name));
            return () =>
            {
                var op = (OperationEnum)operation.GetValue();
                if (op == OperationEnum.Activate)
                    foreach (var field in editor.Editors) editor.SetEnabled(true, field);
                else if (op == OperationEnum.DeactivateAll) editor.EnableOnly(operation);
                else editor.EnableOnly(operation, name);
                name.SetVariableNameMode(Autofill.Text, op == OperationEnum.DeactivateRegex);
            };
        }

        #region Implementation

        /// <summary>Evaluates the font size for one activation or preview, with a minimum of one point.</summary>
        internal float EvaluateFontSize(Context ctx)
        {
            var size = ctx.EvaluateNumericExpression(ActionContextLogger, ctx, FontSize);
            return Math.Max(1, (float)size);
        }

        internal override string DescribeImplementation()
        {
            switch (Operation)
            {
                case OperationEnum.Activate:
                    return I18n.Translate("internal/Action/desctextauraact", "activate text overlay ({0}) with expression ({1})", Name, Text);
                case OperationEnum.Deactivate:
                    return I18n.Translate("internal/Action/desctextauradeact", "deactivate text overlay ({0})", Name);
                case OperationEnum.DeactivateAll:
                    return I18n.Translate("internal/Action/desctextauradeactall", "deactivate all text overlays");
                case OperationEnum.DeactivateRegex:
                    return I18n.Translate("internal/Action/desctextauradeactrex", "deactivate text overlays matching regular expression ({0})", Name);
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            plug.TextAuraManagement(ctx, this);
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionOverlayText(ActionOld oldAction)
        {
            var action = new ActionOverlayText();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._TextAuraOp;
            action.Name = oldAction._TextAuraName;
            action.Text = oldAction._TextAuraExpression;
            action.Alignment = (TextAlignment)(int)oldAction._TextAuraAlignment;
            action.XIniExpression = oldAction._TextAuraXIniExpression;
            action.YIniExpression = oldAction._TextAuraYIniExpression;
            action.WIniExpression = oldAction._TextAuraWIniExpression;
            action.HIniExpression = oldAction._TextAuraHIniExpression;
            action.OIniExpression = oldAction._TextAuraOIniExpression;
            action.XTickExpression = oldAction._TextAuraXTickExpression;
            action.YTickExpression = oldAction._TextAuraYTickExpression;
            action.WTickExpression = oldAction._TextAuraWTickExpression;
            action.HTickExpression = oldAction._TextAuraHTickExpression;
            action.OTickExpression = oldAction._TextAuraOTickExpression;
            action.TTLTickExpression = oldAction._TextAuraTTLTickExpression;
            action.Font = oldAction._TextAuraFontName;
            action.FontSize = oldAction._TextAuraFontSize.ToString(CultureInfo.InvariantCulture);
            action.Effect = (EffectEnum)(int)oldAction._TextAuraEffect;
            action.OutlineColor = oldAction._TextAuraOutlineClInt;
            action.ForeColor = oldAction._TextAuraForegroundClInt;
            action.BackColor = oldAction._TextAuraBackgroundClInt;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionOverlayText action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.TextAura;
            oldAction._TextAuraOp = (ActionOld.AuraOpEnum)(int)action.Operation;
            oldAction._TextAuraName = action.Name;
            oldAction._TextAuraExpression = action.Text;
            oldAction._TextAuraAlignment = (ActionOld.TextAuraAlignmentEnum)(int)action.Alignment;
            oldAction._TextAuraXIniExpression = action.XIniExpression;
            oldAction._TextAuraYIniExpression = action.YIniExpression;
            oldAction._TextAuraWIniExpression = action.WIniExpression;
            oldAction._TextAuraHIniExpression = action.HIniExpression;
            oldAction._TextAuraOIniExpression = action.OIniExpression;
            oldAction._TextAuraXTickExpression = action.XTickExpression;
            oldAction._TextAuraYTickExpression = action.YTickExpression;
            oldAction._TextAuraWTickExpression = action.WTickExpression;
            oldAction._TextAuraHTickExpression = action.HTickExpression;
            oldAction._TextAuraOTickExpression = action.OTickExpression;
            oldAction._TextAuraTTLTickExpression = action.TTLTickExpression;
            oldAction._TextAuraFontName = action.Font;
            oldAction._TextAuraFontSize = float.Parse(action.FontSize, CultureInfo.InvariantCulture);
            oldAction._TextAuraEffect = (ActionOld.TextAuraEffectEnum)(int)action.Effect;
            oldAction._TextAuraOutlineClInt = action.OutlineColor;
            oldAction._TextAuraForegroundClInt = action.ForeColor;
            oldAction._TextAuraBackgroundClInt = action.BackColor;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
