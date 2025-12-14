using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Text overlay operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Overlay)]
    [XmlRoot(ElementName = "OverlayText")]
    internal class ActionOverlayText : ActionBase
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
        public enum TextAlignmentEnum
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
        public TextAlignmentEnum Alignment { get; set; } = TextAlignmentEnum.MiddleCenter;

        [XmlAttribute("Alignment")]
        public string Xml_Alignment
        {
            get => XmlAttr.Enum(Alignment, TextAlignmentEnum.MiddleCenter);
            set => Alignment = XmlAttr.Enum<TextAlignmentEnum>(value);
        }

        /// <summary>
        /// Text display effect
        /// </summary>
        [XmlIgnore]
        public EffectEnum Effect { get; set; } = EffectEnum.None;

        [XmlAttribute("Effect")]
        public string Xml_Effect
        {
            get => XmlAttr.Enum(Effect, EffectEnum.None);
            set => Effect = XmlAttr.Enum<EffectEnum>(value);
        }

        /// <summary>
        /// Font size
        /// </summary>
        [XmlIgnore]
        public float FontSize { get; set; } = 10.0f;

        [XmlAttribute("FontSize")]
        public string Xml_FontSize
        {
            get => XmlAttr.Float(FontSize, 10.0f);
            set => FontSize = XmlAttr.Float(value);
        }

        /// <summary>
        /// Color of the text
        /// </summary>
        [XmlIgnore]
        public string ForeColor { get; set; } = "";

        [XmlAttribute("ForeColor")]
        public string Xml_ForeColor
        {
            get => XmlAttr.String(ForeColor);
            set => ForeColor = value;
        }

        /// <summary>
        /// Background color of the overlay
        /// </summary>
        [XmlIgnore]
        public string BackColor { get; set; } = "";

        [XmlAttribute("BackColor")]
        public string Xml_BackColor
        {
            get => XmlAttr.String(BackColor);
            set => BackColor = value;
        }

        /// <summary>
        /// Color of the text outline
        /// </summary>
        [XmlIgnore]
        public string OutlineColor { get; set; } = "";

        [XmlAttribute("OutlineColor")]
        public string Xml_OutlineColor
        {
            get => XmlAttr.String(OutlineColor);
            set => OutlineColor = value;
        }

        /// <summary>
        /// Name of the text overlay
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
        /// Text expression
        /// </summary>
        [XmlIgnore]
        public string Text { get; set; } = "";

        [XmlAttribute("Text")]
        public string Xml_Text
        {
            get => XmlAttr.String(Text);
            set => Text = value;
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

        /// <summary>
        /// Name of the font to use
        /// </summary>
        [XmlIgnore]
        public string Font { get; set; } = "";

        [XmlAttribute("Font")]
        public string Xml_Font
        {
            get => XmlAttr.String(Font);
            set => Font = value;
        }

        #endregion


        #region Implementation

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
            }
            return "";
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            ctx.Plugin.TextAuraManagement(ctx, null); // todo supposed to be a reference to this action
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
            action.Alignment = (TextAlignmentEnum)(int)oldAction._TextAuraAlignment;
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
            action.FontSize = oldAction._TextAuraFontSize;
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
            oldAction._TextAuraFontSize = action.FontSize;
            oldAction._TextAuraEffect = (ActionOld.TextAuraEffectEnum)(int)action.Effect;
            oldAction._TextAuraOutlineClInt = action.OutlineColor;
            oldAction._TextAuraForegroundClInt = action.ForeColor;
            oldAction._TextAuraBackgroundClInt = action.BackColor;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
