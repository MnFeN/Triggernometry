using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Text-to-speech
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Audio)]
    [XmlRoot(ElementName = "PlaySpeech")]
    public class ActionPlaySpeech : ActionBase
    {

        #region Properties

        /// <summary>
        /// Audio target where speech will be directed (None means default)
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
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
        [Action(order: 2)]
        public string Message { get; set; } = "";

        [XmlAttribute("Message")]
        public string Xml_Message
        {
            get => XmlAttr.String(Message);
            set => Message = value;
        }

        /// <summary>
        /// Volume expression (0 - 100)
        /// </summary>
        [XmlIgnore]
        [Action(order: 3, typeof(float))]
        public string Volume { get; set; } = "100";

        [XmlAttribute("Volume")]
        public string Xml_Volume
        {
            get => XmlAttr.String(Volume, "100");
            set => Volume = value;
        }

        /// <summary>
        /// Rate (speech speed) expression
        /// </summary>
        [XmlIgnore]
        [Action(order: 4, typeof(int))]
        public string Rate { get; set; } = "0";

        [XmlAttribute("Rate")]
        public string Xml_Rate
        {
            get => XmlAttr.String(Rate, "0");
            set => Rate = value;
        }

        // todo remove?
        [XmlIgnore]
        [Action(order: 5)]
        public bool ExclusivePlayer { get; set; } = true;

        [XmlAttribute("ExclusivePlayer")]
        public string Xml_ExclusivePlayer
        {
            get => XmlAttr.Bool(ExclusivePlayer, true);
            set => ExclusivePlayer = XmlAttr.Bool(value);
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            return I18n.Translate("internal/Action/desctts", "say ({0}) at volume ({1}) %, using speed ({2})", Message, Volume, Rate);
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;

            ctx.ttshook(ctx, (ActionOld)this); // todo

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
            action.ExclusivePlayer = oldAction._UseTTSExclusive;
            action.AudioTarget = oldAction._TTSRouting;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionPlaySpeech action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.UseTTS;
            oldAction._UseTTSTextExpression = action.Message;
            oldAction._UseTTSVolumeExpression = action.Volume;
            oldAction._UseTTSRateExpression = action.Rate;
            oldAction._UseTTSExclusive = action.ExclusivePlayer;
            oldAction._TTSRouting = action.AudioTarget;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
