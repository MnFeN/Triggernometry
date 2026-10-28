using System;
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
    /// Image overlay operations
    /// </summary>
    [Action("Image aura operation", ActionAttribute.CategoryType.Display)]
    [XmlRoot(ElementName = "ActionOverlayImage")]
    public class ActionOverlayImage : ActionBase
    {

        #region Properties

        // todo probably needs a custom property editor

        /// <summary>
        /// Image overlay operations
        /// </summary>
        public enum OperationEnum
        {
            /// <summary>
            /// Activate image overlay
            /// </summary>
            Activate,
            /// <summary>
            /// Deactive image overlay with matching name
            /// </summary>
            Deactivate,
            /// <summary>
            /// Deactive all image overlays
            /// </summary>
            DeactivateAll,
            /// <summary>
            /// Deactive all image overlays with name matching given regex
            /// </summary>
            DeactivateRegex,
            /// <summary>
            /// Deactive all image overlays from specified trigger
            /// </summary>
            DeactivateTrigger
        }

        /// <summary>
        /// Type of the image overlay operation
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Operation",
            ChoiceLabels = new[]
        {
            "Activate or modify the specified image aura",
            "Deactivate the specified image aura",
            "Deactivate all image auras",
            "Deactivate image auras by regular expression",
        })]
        public OperationEnum Operation { get; set; } = OperationEnum.Activate;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.Activate);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// Image sizing mode within overlay
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(4, Label = "Display method",
            ChoiceLabels = new[]
        {
            "Normal",
            "Stretched to fill",
            "Centered",
            "Zoomed to fill respecting Aspect Ratio",
        }, ChoiceValues = new[] { 0, 1, 3, 4 })]
        public PictureBoxSizeMode SizeMode { get; set; } = PictureBoxSizeMode.Normal;

        [XmlAttribute("SizeMode")]
        public string Xml_SizeMode
        {
            get => XmlAttr.Enum(SizeMode, PictureBoxSizeMode.Normal);
            set => SizeMode = XmlAttr.Enum<PictureBoxSizeMode>(value);
        }

        /// <summary>
        /// Name of the image overlay
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
        /// Image file name expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [FileEditor(3, EditorFileType.Image, Label = "Image to display")]
        public string Filename { get; set; } = "";

        [XmlAttribute("Filename")]
        public string Xml_Filename
        {
            get => XmlAttr.String(Filename, "", omitWhitespace: false);
            set => Filename = value;
        }

        /// <summary>
        /// Expression for initializing overlay X position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(5, EditorExpressionType.Numeric, Label = "X location expression",
            LabelQualifier = "Initial values")]
        public string X_0 { get; set; } = "";

        [XmlAttribute("X_0")]
        public string Xml_X_0
        {
            get => XmlAttr.String(X_0, "", omitWhitespace: false);
            set => X_0 = value;
        }

        /// <summary>
        /// Expression for initializing overlay Y position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(7, EditorExpressionType.Numeric, Label = "Y location expression",
            LabelQualifier = "Initial values")]
        public string Y_0 { get; set; } = "";

        [XmlAttribute("Y_0")]
        public string Xml_Y_0
        {
            get => XmlAttr.String(Y_0, "", omitWhitespace: false);
            set => Y_0 = value;
        }

        /// <summary>
        /// Expression for initializing overlay width
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(9, EditorExpressionType.Numeric, Label = "Width expression",
            LabelQualifier = "Initial values")]
        public string W_0 { get; set; } = "";

        [XmlAttribute("W_0")]
        public string Xml_W_0
        {
            get => XmlAttr.String(W_0, "", omitWhitespace: false);
            set => W_0 = value;
        }

        /// <summary>
        /// Expression for initializing overlay height
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(11, EditorExpressionType.Numeric, Label = "Height expression",
            LabelQualifier = "Initial values")]
        public string H_0 { get; set; } = "";

        [XmlAttribute("H_0")]
        public string Xml_H_0
        {
            get => XmlAttr.String(H_0, "", omitWhitespace: false);
            set => H_0 = value;
        }

        /// <summary>
        /// Expression for initializing overlay opacity
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(13, EditorExpressionType.Numeric, Label = "Opacity expression",
            LabelQualifier = "Initial values")]
        public string O_0 { get; set; } = "";

        [XmlAttribute("O_0")]
        public string Xml_O_0
        {
            get => XmlAttr.String(O_0, "", omitWhitespace: false);
            set => O_0 = value;
        }

        /// <summary>
        /// Expression for updating overlay X position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(6, EditorExpressionType.Numeric, Label = "X location expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string X_t { get; set; } = "";

        [XmlAttribute("X_t")]
        public string Xml_X_t
        {
            get => XmlAttr.String(X_t, "", omitWhitespace: false);
            set => X_t = value;
        }

        /// <summary>
        /// Expression for updating overlay Y position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(8, EditorExpressionType.Numeric, Label = "Y location expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string Y_t { get; set; } = "";

        [XmlAttribute("Y_t")]
        public string Xml_Y_t
        {
            get => XmlAttr.String(Y_t, "", omitWhitespace: false);
            set => Y_t = value;
        }

        /// <summary>
        /// Expression for updating overlay width
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(10, EditorExpressionType.Numeric, Label = "Width expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string W_t { get; set; } = "";

        [XmlAttribute("W_t")]
        public string Xml_W_t
        {
            get => XmlAttr.String(W_t, "", omitWhitespace: false);
            set => W_t = value;
        }

        /// <summary>
        /// Expression for updating overlay height
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(12, EditorExpressionType.Numeric, Label = "Height expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string H_t { get; set; } = "";

        [XmlAttribute("H_t")]
        public string Xml_H_t
        {
            get => XmlAttr.String(H_t, "", omitWhitespace: false);
            set => H_t = value;
        }

        /// <summary>
        /// Expression for updating overlay opacity
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(14, EditorExpressionType.Numeric, Label = "Opacity expression",
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
        [ExpressionEditor(15, EditorExpressionType.Numeric, Label = "Time-to-live expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string AliveCondition { get; set; } = "";

        [XmlAttribute("AliveCondition")]
        public string Xml_AliveCondition
        {
            get => XmlAttr.String(AliveCondition, "", omitWhitespace: false);
            set => AliveCondition = value;
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
                name.SetVariableNameMode(Autofill.Image, op == OperationEnum.DeactivateRegex);
            };
        }

        #region Implementation

        internal override string DescribeImplementation()
        {
            switch (Operation)
            {
                case OperationEnum.Activate:
                    return I18n.Translate("internal/Action/descimgauraact", "activate image overlay ({0}) with image ({1})", Name, Filename);
                case OperationEnum.Deactivate:
                    return I18n.Translate("internal/Action/descimgauradeact", "deactivate image overlay ({0})", Name);
                case OperationEnum.DeactivateAll:
                    return I18n.Translate("internal/Action/descimgauradeactall", "deactivate all image overlays");
                case OperationEnum.DeactivateRegex:
                    return I18n.Translate("internal/Action/descimgauradeactrex", "deactivate image overlays matching regular expression ({0})", Name);
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            plug.ImageAuraManagement(ctx, this);
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionOverlayImage(ActionOld oldAction)
        {
            var action = new ActionOverlayImage();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._AuraOp;
            action.Name = oldAction._AuraName;
            action.Filename = oldAction._AuraImage;
            action.SizeMode = oldAction._AuraImageMode;
            action.X_0 = oldAction._AuraXIniExpression;
            action.Y_0 = oldAction._AuraYIniExpression;
            action.W_0 = oldAction._AuraWIniExpression;
            action.H_0 = oldAction._AuraHIniExpression;
            action.O_0 = oldAction._AuraOIniExpression;
            action.X_t = oldAction._AuraXTickExpression;
            action.Y_t = oldAction._AuraYTickExpression;
            action.W_t = oldAction._AuraWTickExpression;
            action.H_t = oldAction._AuraHTickExpression;
            action.O_t = oldAction._AuraOTickExpression;
            action.AliveCondition = oldAction._AuraTTLTickExpression;
            return action;
        }

        #endregion Old Action Converter
    }

}
