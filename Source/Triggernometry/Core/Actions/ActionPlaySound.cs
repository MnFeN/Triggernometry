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

        /// <summary>跟踪当前专用播放器，停止、媒体出错或启动失败时解绑并移除。</summary>
        internal Action TrackPlayer(WindowsMediaPlayer player)
        {
            _WMPOCXEvents_PlayStateChangeEventHandler stateChanged = null;
            _WMPOCXEvents_MediaErrorEventHandler mediaError = null;

            void StopTracking()
            {
                lock (players)
                {
                    if (!players.Remove(player)) return;
                }
                // 在集合锁外解绑，避免 COM 调用阻塞其他播放器的登记。
                try { player.PlayStateChange -= stateChanged; }
                finally { player.MediaError -= mediaError; }
            }

            stateChanged = state =>
            {
                // 延迟到达的停止通知不能移除已经继续播放的实例。
                if ((WMPPlayState)state == WMPPlayState.wmppsStopped && player.playState == WMPPlayState.wmppsStopped)
                    StopTracking();
            };
            // MediaError 的参数是媒体对象，不是播放器；使用注册时绑定的实例。
            mediaError = _ => StopTracking();

            lock (players) players.Add(player);
            try
            {
                player.PlayStateChange += stateChanged;
                player.MediaError += mediaError;
            }
            catch
            {
                StopTracking();
                throw;
            }
            return StopTracking;
        }

        internal override string DescribeImplementation()
        {
            return I18n.Translate("internal/Action/descplaysound", "play sound file ({0}) at volume ({1}) %", Filename, Volume);
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            if (ctx.soundhook != null) 
                ctx.soundhook(ctx, this);
            else 
                ctx.Plugin.SoundPlaybackSmart(ctx, this);
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

        #endregion Old Action Converter
    }

}
