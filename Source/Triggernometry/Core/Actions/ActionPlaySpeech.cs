using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Text-to-speech
    /// </summary>
    [Action("Use text-to-speech", ActionAttribute.CategoryType.Audio)]
    [XmlRoot(ElementName = "ActionPlaySpeech")]
    public class ActionPlaySpeech : ActionBase
    {

        #region Properties

        /// <summary>
        /// Audio target where speech will be directed (None means default)
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(4, Label = "Audio output method",
            ChoiceLabels = new[]
        {
            "Default",
            "Windows Speech API",
            "ACT",
        })]
        public Configuration.AudioRoutingMethodEnum AudioTarget { get; set; } = Configuration.AudioRoutingMethodEnum.None;

        [XmlAttribute("AudioTarget")]
        public string Xml_AudioTarget
        {
            get => XmlAttr.Enum(AudioTarget, Configuration.AudioRoutingMethodEnum.None);
            set => AudioTarget = XmlAttr.Enum<Configuration.AudioRoutingMethodEnum>(value);
        }

        /// <summary>
        /// Message expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(1, Label = "Text to say")]
        public string Message { get; set; } = "";

        [XmlAttribute("Message")]
        public string Xml_Message
        {
            get => XmlAttr.String(Message, "", omitWhitespace: false);
            set => Message = value;
        }

        /// <summary>
        /// Volume expression (0 - 100)
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, EditorExpressionType.Numeric, Label = "Speech volume (0 ... 100 %)")]
        public string Volume { get; set; } = "100";

        [XmlAttribute("Volume")]
        public string Xml_Volume
        {
            get => XmlAttr.String(Volume, "100", omitWhitespace: false);
            set => Volume = value;
        }

        /// <summary>
        /// Rate (speech speed) expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(3, EditorExpressionType.Numeric, Label = "Speech speed rate (-10 ... +10)")]
        public string Rate { get; set; } = "0";

        [XmlAttribute("Rate")]
        public string Xml_Rate
        {
            get => XmlAttr.String(Rate, "0", omitWhitespace: false);
            set => Rate = value;
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            return I18n.Translate("internal/Action/desctts", "say ({0}) at volume ({1}) %, using speed ({2})", Message, Volume, Rate);
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            ctx.ttshook(ctx, this);

            /* to-do: cactbot-like 文本显示
            if (plug.cfg.GenerateTTSEventLog)
            {
                plug.LogLineQueuer(this._UseTTSTextExpression, "", LogEvent.SourceEnum.ACT);
            }
            */
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionPlaySpeech(ActionOld oldAction)
        {
            var action = new ActionPlaySpeech();
            oldAction.CopyCommonPropertiesTo(action);
            action.Message = oldAction._UseTTSTextExpression;
            action.Volume = oldAction._UseTTSVolumeExpression;
            action.Rate = oldAction._UseTTSRateExpression;
            action.AudioTarget = oldAction._TTSRouting;
            return action;
        }

        #endregion Old Action Converter
    }

}
