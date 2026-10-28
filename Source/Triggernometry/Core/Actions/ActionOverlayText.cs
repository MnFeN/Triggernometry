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
        public string TextColor { get; set; } = "";

        [XmlAttribute("TextColor")]
        public string Xml_TextColor
        {
            get => XmlAttr.String(TextColor, "", omitWhitespace: false);
            set => TextColor = value;
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
        public string X_0 { get; set; } = "";

        [XmlAttribute("X_0")]
        public string Xml_X_0
        {
            get => XmlAttr.String(X_0, "", omitWhitespace: false);
            set => X_0 = value;
        }

        /// <summary>
        /// Exoression for initializing overlay Y position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(13, EditorExpressionType.Numeric, Label = "Y location expression",
            LabelQualifier = "Initial values")]
        public string Y_0 { get; set; } = "";

        [XmlAttribute("Y_0")]
        public string Xml_Y_0
        {
            get => XmlAttr.String(Y_0, "", omitWhitespace: false);
            set => Y_0 = value;
        }

        /// <summary>
        /// Exoression for initializing overlay width
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(15, EditorExpressionType.Numeric, Label = "Width expression",
            LabelQualifier = "Initial values")]
        public string W_0 { get; set; } = "";

        [XmlAttribute("W_0")]
        public string Xml_W_0
        {
            get => XmlAttr.String(W_0, "", omitWhitespace: false);
            set => W_0 = value;
        }

        /// <summary>
        /// Exoression for initializing overlay height
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(17, EditorExpressionType.Numeric, Label = "Height expression",
            LabelQualifier = "Initial values")]
        public string H_0 { get; set; } = "";

        [XmlAttribute("H_0")]
        public string Xml_H_0
        {
            get => XmlAttr.String(H_0, "", omitWhitespace: false);
            set => H_0 = value;
        }

        /// <summary>
        /// Exoression for initializing overlay opacity
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(19, EditorExpressionType.Numeric, Label = "Opacity expression",
            LabelQualifier = "Initial values")]
        public string O_0 { get; set; } = "";

        [XmlAttribute("O_0")]
        public string Xml_O_0
        {
            get => XmlAttr.String(O_0, "", omitWhitespace: false);
            set => O_0 = value;
        }

        /// <summary>
        /// Exoression for updating overlay X position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(12, EditorExpressionType.Numeric, Label = "X location expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string X_t { get; set; } = "";

        [XmlAttribute("X_t")]
        public string Xml_X_t
        {
            get => XmlAttr.String(X_t, "", omitWhitespace: false);
            set => X_t = value;
        }

        /// <summary>
        /// Exoression for updating overlay Y position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(14, EditorExpressionType.Numeric, Label = "Y location expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string Y_t { get; set; } = "";

        [XmlAttribute("Y_t")]
        public string Xml_Y_t
        {
            get => XmlAttr.String(Y_t, "", omitWhitespace: false);
            set => Y_t = value;
        }

        /// <summary>
        /// Exoression for updating overlay width
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(16, EditorExpressionType.Numeric, Label = "Width expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string W_t { get; set; } = "";

        [XmlAttribute("W_t")]
        public string Xml_W_t
        {
            get => XmlAttr.String(W_t, "", omitWhitespace: false);
            set => W_t = value;
        }

        /// <summary>
        /// Exoression for updating overlay height
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(18, EditorExpressionType.Numeric, Label = "Height expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string H_t { get; set; } = "";

        [XmlAttribute("H_t")]
        public string Xml_H_t
        {
            get => XmlAttr.String(H_t, "", omitWhitespace: false);
            set => H_t = value;
        }

        /// <summary>
        /// Exoression for updating overlay opacity
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(20, EditorExpressionType.Numeric, Label = "Opacity expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string O_t { get; set; } = "";

        [XmlAttribute("O_t")]
        public string Xml_O_t
        {
            get => XmlAttr.String(O_t, "", omitWhitespace: false);
            set => O_t = value;
        }

        /// <summary>
        /// Numeric lifetime check evaluated on each update; a negative result deactivates the overlay
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(21, EditorExpressionType.Numeric, Label = "Time-to-live expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string AliveCondition { get; set; } = "";

        [XmlAttribute("AliveCondition")]
        public string Xml_AliveCondition
        {
            get => XmlAttr.String(AliveCondition, "", omitWhitespace: false);
            set => AliveCondition = value;
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
            action.X_0 = oldAction._TextAuraXIniExpression;
            action.Y_0 = oldAction._TextAuraYIniExpression;
            action.W_0 = oldAction._TextAuraWIniExpression;
            action.H_0 = oldAction._TextAuraHIniExpression;
            action.O_0 = oldAction._TextAuraOIniExpression;
            action.X_t = oldAction._TextAuraXTickExpression;
            action.Y_t = oldAction._TextAuraYTickExpression;
            action.W_t = oldAction._TextAuraWTickExpression;
            action.H_t = oldAction._TextAuraHTickExpression;
            action.O_t = oldAction._TextAuraOTickExpression;
            action.AliveCondition = oldAction._TextAuraTTLTickExpression;
            action.Font = oldAction._TextAuraFontName;
            action.FontSize = oldAction._TextAuraFontSize.ToString(CultureInfo.InvariantCulture);
            action.Effect = (EffectEnum)(int)oldAction._TextAuraEffect;
            action.OutlineColor = oldAction._TextAuraOutlineClInt;
            action.TextColor = oldAction._TextAuraForegroundClInt;
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
            oldAction._TextAuraXIniExpression = action.X_0;
            oldAction._TextAuraYIniExpression = action.Y_0;
            oldAction._TextAuraWIniExpression = action.W_0;
            oldAction._TextAuraHIniExpression = action.H_0;
            oldAction._TextAuraOIniExpression = action.O_0;
            oldAction._TextAuraXTickExpression = action.X_t;
            oldAction._TextAuraYTickExpression = action.Y_t;
            oldAction._TextAuraWTickExpression = action.W_t;
            oldAction._TextAuraHTickExpression = action.H_t;
            oldAction._TextAuraOTickExpression = action.O_t;
            oldAction._TextAuraTTLTickExpression = action.AliveCondition;
            oldAction._TextAuraFontName = action.Font;
            oldAction._TextAuraFontSize = float.Parse(action.FontSize, CultureInfo.InvariantCulture);
            oldAction._TextAuraEffect = (ActionOld.TextAuraEffectEnum)(int)action.Effect;
            oldAction._TextAuraOutlineClInt = action.OutlineColor;
            oldAction._TextAuraForegroundClInt = action.TextColor;
            oldAction._TextAuraBackgroundClInt = action.BackColor;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
