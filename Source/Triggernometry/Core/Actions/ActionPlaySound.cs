using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using WMPLib;
using Triggernometry.Utilities;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Sound playback
    /// </summary>
    [Action("Play sound file", ActionAttribute.CategoryType.Audio)]
    [XmlRoot(ElementName = "ActionPlaySound")]
    public class ActionPlaySound : ActionBase
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
            "Windows Media Player API",
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
        /// Sound file name expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [FileEditor(1, EditorFileType.Audio, Label = "Sound file to play")]
        public string Filename { get; set; } = "";

        [XmlAttribute("Filename")]
        public string Xml_Filename
        {
            get => XmlAttr.String(Filename, "", omitWhitespace: false);
            set => Filename = value;
        }

        /// <summary>
        /// Volume expression (0 - 100)
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, EditorExpressionType.Numeric, Label = "Sound volume (0 ... 100 %)")]
        public string Volume { get; set; } = "100";

        [XmlAttribute("Volume")]
        public string Xml_Volume
        {
            get => XmlAttr.String(Volume, "100", omitWhitespace: false);
            set => Volume = value;
        }

        // todo remove?
        [XmlIgnore]
        [ActionSpecificProperty]
        [CheckBoxEditor(3, Label = "Use shared sound player")]
        public bool UseSharedPlayer { get; set; } = false;

        [XmlAttribute("UseSharedPlayer")]
        public string Xml_UseSharedPlayer
        {
            get => XmlAttr.Bool(UseSharedPlayer, false);
            set => UseSharedPlayer = XmlAttr.Bool(value);
        }

        #endregion


        #region Implementation

        // Each sound action owns the players used for overlapping playback.
        internal readonly List<WindowsMediaPlayer> players = new List<WindowsMediaPlayer>();

        internal void Mywmp_PlayStateChange(int NewState)
        {
            if ((WMPPlayState)NewState != WMPPlayState.wmppsStopped)
            {
                return;
            }
            WindowsMediaPlayer wmp;
            lock (players) // verified
            {
                do
                {
                    wmp = null;
                    foreach (WindowsMediaPlayer player in players)
                    {
                        if (player.playState == WMPPlayState.wmppsStopped)
                        {
                            wmp = player;
                            break;
                        }
                    }
                    if (wmp != null)
                    {
                        players.Remove(wmp);
                    }
                } while (wmp != null);
            }
        }

        internal void Mywmp_MediaError(object pMediaObject)
        {
            WindowsMediaPlayer wmp = (WindowsMediaPlayer)pMediaObject;
            lock (players) // verified
            {
                players.Remove(wmp);
            }
        }

        internal override string DescribeImplementation()
        {
            return I18n.Translate("internal/Action/descplaysound", "play sound file ({0}) at volume ({1}) %", Filename, Volume);
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            ctx.soundhook(ctx, this);
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
            action.UseSharedPlayer = !oldAction._PlaySoundExclusive;
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
            oldAction._PlaySoundExclusive = !action.UseSharedPlayer;
            oldAction._SoundRouting = action.AudioTarget;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
