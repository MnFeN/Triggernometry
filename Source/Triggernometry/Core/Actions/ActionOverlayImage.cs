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
        /// Exoression for initializing overlay X position
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(5, EditorExpressionType.Numeric, Label = "X location expression",
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
        [ExpressionEditor(7, EditorExpressionType.Numeric, Label = "Y location expression",
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
        [ExpressionEditor(9, EditorExpressionType.Numeric, Label = "Width expression",
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
        [ExpressionEditor(11, EditorExpressionType.Numeric, Label = "Height expression",
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
        [ExpressionEditor(13, EditorExpressionType.Numeric, Label = "Opacity expression",
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
        [ExpressionEditor(6, EditorExpressionType.Numeric, Label = "X location expression",
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
        [ExpressionEditor(8, EditorExpressionType.Numeric, Label = "Y location expression",
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
        [ExpressionEditor(10, EditorExpressionType.Numeric, Label = "Width expression",
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
        [ExpressionEditor(12, EditorExpressionType.Numeric, Label = "Height expression",
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
        [ExpressionEditor(14, EditorExpressionType.Numeric, Label = "Opacity expression",
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
        [ExpressionEditor(15, EditorExpressionType.Numeric, Label = "Time-to-live expression",
            LabelQualifier = "Update tick (20 ms) expressions")]
        public string TTLTickExpression { get; set; } = "";

        [XmlAttribute("TTLTickExpression")]
        public string Xml_TTLTickExpression
        {
            get => XmlAttr.String(TTLTickExpression, "", omitWhitespace: false);
            set => TTLTickExpression = value;
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
            action.XIniExpression = oldAction._AuraXIniExpression;
            action.YIniExpression = oldAction._AuraYIniExpression;
            action.WIniExpression = oldAction._AuraWIniExpression;
            action.HIniExpression = oldAction._AuraHIniExpression;
            action.OIniExpression = oldAction._AuraOIniExpression;
            action.XTickExpression = oldAction._AuraXTickExpression;
            action.YTickExpression = oldAction._AuraYTickExpression;
            action.WTickExpression = oldAction._AuraWTickExpression;
            action.HTickExpression = oldAction._AuraHTickExpression;
            action.OTickExpression = oldAction._AuraOTickExpression;
            action.TTLTickExpression = oldAction._AuraTTLTickExpression;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionOverlayImage action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.Aura;
            oldAction._AuraOp = (ActionOld.AuraOpEnum)(int)action.Operation;
            oldAction._AuraName = action.Name;
            oldAction._AuraImage = action.Filename;
            oldAction._AuraImageMode = action.SizeMode;
            oldAction._AuraXIniExpression = action.XIniExpression;
            oldAction._AuraYIniExpression = action.YIniExpression;
            oldAction._AuraWIniExpression = action.WIniExpression;
            oldAction._AuraHIniExpression = action.HIniExpression;
            oldAction._AuraOIniExpression = action.OIniExpression;
            oldAction._AuraXTickExpression = action.XTickExpression;
            oldAction._AuraYTickExpression = action.YTickExpression;
            oldAction._AuraWTickExpression = action.WTickExpression;
            oldAction._AuraHTickExpression = action.HTickExpression;
            oldAction._AuraOTickExpression = action.OTickExpression;
            oldAction._AuraTTLTickExpression = action.TTLTickExpression;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
