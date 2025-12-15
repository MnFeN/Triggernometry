using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Sound playback
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Audio)]
    [XmlRoot(ElementName = "PlaySound")]
    public class ActionPlaySound : ActionBase
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
        /// Sound file name expression
        /// </summary>
        [XmlIgnore]
        [Action(order: 2, specialtype: ActionAttribute.SpecialTypeEnum.AudioSelector)]
        public string Filename { get; set; } = "";

        [XmlAttribute("Filename")]
        public string Xml_Filename
        {
            get => XmlAttr.String(Filename);
            set => Filename = value;
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

        // todo remove?
        [XmlIgnore]
        [Action(order: 4)]
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
            return I18n.Translate("internal/Action/descplaysound", "play sound file ({0}) at volume ({1}) %", Filename, Volume);
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            ctx.soundhook(ctx, (ActionOld)this); // todo
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionPlaySound(ActionOld oldAction)
        {
            var action = new ActionPlaySound();
            oldAction.CopyCommonPropertiesTo(action);
            action.Filename = oldAction._PlaySoundFileExpression;
            action.Volume = oldAction._PlaySoundVolumeExpression;
            action.ExclusivePlayer = oldAction._PlaySoundExclusive;
            action.AudioTarget = oldAction._SoundRouting;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionPlaySound action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.PlaySound;
            oldAction._PlaySoundFileExpression = action.Filename;
            oldAction._PlaySoundVolumeExpression = action.Volume;
            oldAction._PlaySoundExclusive = action.ExclusivePlayer;
            oldAction._SoundRouting = action.AudioTarget;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
