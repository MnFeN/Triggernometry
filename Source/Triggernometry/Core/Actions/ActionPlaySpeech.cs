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

        internal override string DescribeImplementation(Context ctx)
        {
            return I18n.Translate("internal/Action/desctts", "say ({0}) at volume ({1}) %, using speed ({2})", Message, Volume, Rate);
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            ctx.ttshook(ctx, null); // todo
        }

        #endregion

    }

}
