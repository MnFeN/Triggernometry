using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Image overlay operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Overlay)]
    [XmlRoot(ElementName = "OverlayImage")]
    internal class ActionOverlayImage : ActionBase
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
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name);
            set => Name = value;
        }

        /// <summary>
        /// Image file name expression
        /// </summary>
        [XmlIgnore]
        public string Filename { get; set; } = "";

        [XmlAttribute("Filename")]
        public string Xml_Filename
        {
            get => XmlAttr.String(Filename);
            set => Filename = value;
        }

        /// <summary>
        /// Exoression for initializing overlay X position
        /// </summary>
        [XmlIgnore]
        public string XIniExpression { get; set; } = "";

        [XmlAttribute("XIniExpression")]
        public string Xml_XIniExpression
        {
            get => XmlAttr.String(XIniExpression);
            set => XIniExpression = value;
        }

        /// <summary>
        /// Exoression for initializing overlay Y position
        /// </summary>
        [XmlIgnore]
        public string YIniExpression { get; set; } = "";

        [XmlAttribute("YIniExpression")]
        public string Xml_YIniExpression
        {
            get => XmlAttr.String(YIniExpression);
            set => YIniExpression = value;
        }

        /// <summary>
        /// Exoression for initializing overlay width
        /// </summary>
        [XmlIgnore]
        public string WIniExpression { get; set; } = "";

        [XmlAttribute("WIniExpression")]
        public string Xml_WIniExpression
        {
            get => XmlAttr.String(WIniExpression);
            set => WIniExpression = value;
        }

        /// <summary>
        /// Exoression for initializing overlay height
        /// </summary>
        [XmlIgnore]
        public string HIniExpression { get; set; } = "";

        [XmlAttribute("HIniExpression")]
        public string Xml_HIniExpression
        {
            get => XmlAttr.String(HIniExpression);
            set => HIniExpression = value;
        }

        /// <summary>
        /// Exoression for initializing overlay opacity
        /// </summary>
        [XmlIgnore]
        public string OIniExpression { get; set; } = "";

        [XmlAttribute("OIniExpression")]
        public string Xml_OIniExpression
        {
            get => XmlAttr.String(OIniExpression);
            set => OIniExpression = value;
        }

        /// <summary>
        /// Exoression for updating overlay X position
        /// </summary>
        [XmlIgnore]
        public string XTickExpression { get; set; } = "";

        [XmlAttribute("XTickExpression")]
        public string Xml_XTickExpression
        {
            get => XmlAttr.String(XTickExpression);
            set => XTickExpression = value;
        }

        /// <summary>
        /// Exoression for updating overlay Y position
        /// </summary>
        [XmlIgnore]
        public string YTickExpression { get; set; } = "";

        [XmlAttribute("YTickExpression")]
        public string Xml_YTickExpression
        {
            get => XmlAttr.String(YTickExpression);
            set => YTickExpression = value;
        }

        /// <summary>
        /// Exoression for updating overlay width
        /// </summary>
        [XmlIgnore]
        public string WTickExpression { get; set; } = "";

        [XmlAttribute("WTickExpression")]
        public string Xml_WTickExpression
        {
            get => XmlAttr.String(WTickExpression);
            set => WTickExpression = value;
        }

        /// <summary>
        /// Exoression for updating overlay height
        /// </summary>
        [XmlIgnore]
        public string HTickExpression { get; set; } = "";

        [XmlAttribute("HTickExpression")]
        public string Xml_HTickExpression
        {
            get => XmlAttr.String(HTickExpression);
            set => HTickExpression = value;
        }

        /// <summary>
        /// Exoression for updating overlay opacity
        /// </summary>
        [XmlIgnore]
        public string OTickExpression { get; set; } = "";

        [XmlAttribute("OTickExpression")]
        public string Xml_OTickExpression
        {
            get => XmlAttr.String(OTickExpression);
            set => OTickExpression = value;
        }

        /// <summary>
        /// Exoression for checking overlay life cycle
        /// </summary>
        [XmlIgnore]
        public string TTLTickExpression { get; set; } = "";

        [XmlAttribute("TTLTickExpression")]
        public string Xml_TTLTickExpression
        {
            get => XmlAttr.String(TTLTickExpression);
            set => TTLTickExpression = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation(Context ctx)
        {
            switch (Operation)
            {
                case OperationEnum.Activate:
                    return I18n.Translate("internal/Action/descimgoverlayact", "activate image overlay ({0}) with image ({1})", Name, Filename);
                case OperationEnum.Deactivate:
                    return I18n.Translate("internal/Action/descimgoverlaydeact", "deactivate image overlay ({0})", Name);
                case OperationEnum.DeactivateAll:
                    return I18n.Translate("internal/Action/descimgoverlaydeactall", "deactivate all image overlays");
                case OperationEnum.DeactivateRegex:
                    return I18n.Translate("internal/Action/descimgoverlaydeactrex", "deactivate image overlays matching regular expression ({0})", Name);
            }
            return "";
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            ctx.Plugin.ImageAuraManagement(ctx, null); // todo supposed to be a reference to this action
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
