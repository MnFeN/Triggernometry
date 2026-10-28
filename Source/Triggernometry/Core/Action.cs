using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Actions;
using Triggernometry.Core.Conditions;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;
using static Triggernometry.Core.RealPlugin;

namespace Triggernometry.Core
{
    /// <summary>Stores legacy action XML settings and converts them to concrete runtime actions.</summary>
    [XmlType("Action")]
    public class ActionOld
    {

        #region General properties



        /// <summary>
        /// for queue controlling
        /// </summary>
        internal Guid Id { get; set; } = Guid.NewGuid();

        [XmlIgnore]
        public bool Enabled { get; set; } = true;

        [XmlAttribute("Enabled")]
        public string Xml_Enabled
        {
            get => XmlAttr.Bool(Enabled, true);
            set => Enabled = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        public Trigger ParentTrigger { get; set; } = null;



        [XmlIgnore]
        public ActionTypeEnum ActionType = ActionTypeEnum.SystemBeep;

        [XmlAttribute("ActionType")]
        public string Xml_ActionType
        {
            get => XmlAttr.Enum(ActionType, ActionTypeEnum.SystemBeep);
            set
            {
                if (XmlAttr.TryEnum(value, out ActionType)) return;

                if (value == "EndEncounter") // convert old actions
                {
                    ActionType = ActionTypeEnum.ActInteraction;
                    _ActOpBoolParam = false;
                    return;
                }

                throw InvalidEnumException("ActionTypeEnum", value);
            }
        }



        [XmlIgnore]
        public string ExecutionDelayExpression { get; set; } = "0";

        [XmlAttribute("ExecutionDelayExpression")]
        public string Xml_ExecutionDelayExpression
        {
            get => XmlAttr.String(ExecutionDelayExpression, "0");
            set => ExecutionDelayExpression = value;
        }



        [XmlIgnore]
        public bool Asynchronous { get; set; } = true;

        [XmlAttribute("Asynchronous")]
        public string Xml_Asynchronous
        {
            get => XmlAttr.Bool(Asynchronous, true);
            set => Asynchronous = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        public DebugLevelEnum DebugLevel { get; set; } = DebugLevelEnum.Inherit;

        [XmlAttribute("DebugLevel")]
        public string Xml_DebugLevel
        {
            get => XmlAttr.Enum(DebugLevel, DebugLevelEnum.Inherit);
            set => DebugLevel = XmlAttr.Enum<DebugLevelEnum>(value);
        }



        [XmlIgnore]
        public bool RefireInterrupt { get; set; } = false;

        [XmlAttribute("RefireInterrupt")]
        public string Xml_RefireInterrupt
        {
            get => XmlAttr.Bool(RefireInterrupt, false);
            set => RefireInterrupt = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        public bool RefireRequeue { get; set; } = true;

        [XmlAttribute("RefireRequeue")]
        public string Xml_RefireRequeue
        {
            get => XmlAttr.Bool(RefireRequeue, true);
            set => RefireRequeue = XmlAttr.Bool(value);
        }



        [XmlAttribute]
        public int OrderNumber;



        [XmlIgnore]
        public string Description { get; set; } = "";

        [XmlAttribute("Description")]
        public string Xml_Description
        {
            get => XmlAttr.String(Description);
            set => Description = value;
        }



        [XmlIgnore]
        public string DescBgColor { get; set; } = "";

        [XmlAttribute("DescBgColor")]
        public string Xml_DescBgColor
        {
            get => XmlAttr.String(DescBgColor);
            set => DescBgColor = value;
        }



        [XmlIgnore]
        public string DescTextColor { get; set; } = "";

        [XmlAttribute("DescTextColor")]
        public string Xml_DescTextColor
        {
            get => XmlAttr.String(DescTextColor);
            set => DescTextColor = value;
        }



        [XmlIgnore]
        public bool DescriptionOverride { get; set; } = false;

        [XmlAttribute("DescriptionOverride")]
        public string Xml_DescriptionOverride
        {
            get => XmlAttr.Bool(DescriptionOverride, false);
            set => DescriptionOverride = XmlAttr.Bool(value);
        }



        /// <summary>A string tag used in trigger/folder actions to interrupt specific actions.</summary>
        [XmlIgnore]
        public string Tag { get; set; }

        [XmlAttribute("Tag")]
        public string Xml_Tag
        {
            get => XmlAttr.String(Tag);
            set => Tag = value;
        }



        [XmlIgnore]
        public ConditionGroup Condition = new ConditionGroup { Enabled = false };

        [XmlElement("Condition")]
        public ConditionGroup Xml_Condition
        {
            get => Condition.Children.Count == 0 && !Condition.Enabled ? null : Condition;
            set => Condition = value ?? new ConditionGroup { Enabled = false };
        }

        #endregion

        public static ArgumentException InvalidEnumException(string enumName, string enumValue)
        { 
            return new ArgumentException(
                I18n.Translate(
                    "internal/Action/invalidEnumType",
                    "{0} = {1} is not a known enum type.\n\n" +
                    "This may be because your Triggernometry plugin is not up to date, or the data you are trying to import is corrupted.",
                    enumName, enumValue)
                );
        }

        internal void CopyCommonPropertiesTo(ActionBase action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            action.Enabled = Enabled;
            action.Id = Id;
            action.ParentTrigger = ParentTrigger;
            action.Condition = (ConditionGroup)Condition?.Duplicate();
            action.Tag = Tag;
            action.ReplaceQueued = RefireInterrupt;
            // Legacy XML uses the opposite (allow requeue) logic.
            action.PreventDuplicateQueue = !RefireRequeue;
            action.ExecDelay = ExecutionDelayExpression;
            // 旧版未填写 Asynchronous 时默认为 true；显式保留旧行为。
            action.Async = Asynchronous;
            action.DebugLevel = DebugLevel;
            action.Desc = Description;
            action.DescBackColor = DescBgColor;
            action.DescTextColor = DescTextColor;
            action.DescOverride = DescriptionOverride;
        }

        internal ActionBase ConvertToNewAction()
        {
            switch (ActionType)
            {
                case ActionTypeEnum.SystemBeep:
                    return (ActionBeep)this;
                case ActionTypeEnum.PlaySound:
                    return (ActionPlaySound)this;
                case ActionTypeEnum.UseTTS:
                    return (ActionPlaySpeech)this;
                case ActionTypeEnum.MessageBox:
                    return (ActionMessageBox)this;
                case ActionTypeEnum.TextAura:
                    return (ActionOverlayText)this;
                case ActionTypeEnum.Aura:
                    return (ActionOverlayImage)this;
                case ActionTypeEnum.Variable:
                    return (ActionVariableScalar)this;
                case ActionTypeEnum.ListVariable:
                    if (_ListVariableOp == ListVariableOpEnum.SortFfxivPartyAsc || _ListVariableOp == ListVariableOpEnum.SortFfxivPartyDesc)
                    {
                        var placeholder = (ActionPlaceholder)this;
                        placeholder.DescOverride = true;
                        placeholder.Desc = "FFXIV job sorting is no longer supported. Please implement job sorting yourself.";
                        return placeholder;
                    }
                    return (ActionVariableList)this;
                case ActionTypeEnum.TableVariable:
                    return (ActionVariableTable)this;
                case ActionTypeEnum.DictVariable:
                    return (ActionVariableDict)this;
                case ActionTypeEnum.LogMessage:
                    return (ActionLogMessage)this;
                case ActionTypeEnum.NamedCallback:
                    return (ActionNamedCallback)this;
                case ActionTypeEnum.ExecuteScript:
                    return (ActionExecuteScript)this;
                case ActionTypeEnum.Loop:
                    return (ActionLoop)this;
                case ActionTypeEnum.Mutex:
                    return (ActionMutex)this;
                case ActionTypeEnum.Trigger:
                    return (ActionTriggerOperation)this;
                case ActionTypeEnum.Folder:
                    return (ActionFolderOperation)this;
                case ActionTypeEnum.Repository:
                    return (ActionRepository)this;
                case ActionTypeEnum.Mouse:
                    return (ActionMouse)this;
                case ActionTypeEnum.KeyPress:
                    return (ActionKeypress)this;
                case ActionTypeEnum.WindowMessage:
                    return (ActionWindowMessage)this;
                case ActionTypeEnum.LaunchProcess:
                    return (ActionLaunchProcess)this;
                case ActionTypeEnum.DiskFile:
                    return (ActionDiskOperation)this;
                case ActionTypeEnum.ActInteraction:
                    return (ActionActInteraction)this;
                case ActionTypeEnum.GenericJson:
                    return (ActionJsonRequest)this;
                case ActionTypeEnum.DiscordWebhook:
                    return (ActionDiscordWebhook)this;
                case ActionTypeEnum.ObsControl:
                    return (ActionObsControl)this;
                case ActionTypeEnum.LiveSplitControl:
                    return (ActionLiveSplitControl)this;
                case ActionTypeEnum.Placeholder:
                    return (ActionPlaceholder)this;

                default:
                    throw new NotSupportedException(I18n.Translate("internal/Action/unknowntype", "Unknown ActionType: {0}", ActionType));
            }
        }

        #region Legacy action settings

        #region Enums

        public enum ActionTypeEnum
        {
            // 旧动作类型及编号仅用于配置和脚本兼容，新增动作不再扩展此枚举。
            SystemBeep = 0,
            PlaySound = 1,
            UseTTS = 2,
            MessageBox = 7,
            TextAura = 9,
            Aura = 10,
            Variable = 3,
            ListVariable = 4,
            TableVariable = 5,
            DictVariable = 6,
            LogMessage = 8,
            NamedCallback = 13,
            ExecuteScript = 17,
            Loop = 19,
            Mutex = 18,
            Trigger = 25,
            Folder = 26,
            Repository = 27,
            Mouse = 11,
            KeyPress = 12,
            WindowMessage = 14,
            LaunchProcess = 16,
            DiskFile = 15,
            ActInteraction = 24,
            GenericJson = 20,
            DiscordWebhook = 21,
            ObsControl = 23,
            LiveSplitControl = 22,
            Placeholder = 28,
            // Old actions:
            // EndEncounter => ActInteraction
        }

        public enum VariableOpEnum
        {
            Unset,
            SetString,
            SetNumeric,
            Increment,
            Clipboard,
            UnsetAll,
            UnsetRegex,
            UnsetRegexUniversal,
            QueryJsonPath,
            QueryJsonPathList
        }

        public enum TableVariableOpEnum
        {
            Unset,
            Set,
            SetAll,
            SlicesSetAll,
            Resize,
            Build,
            SetLine,
            InsertLine,
            RemoveLine,
            Filter,
            FilterLine,
            Copy,
            Append,
            AppendH,
            SortLine,
            GetAllEntities,
            UnsetAll,
            UnsetRegex,
        }

        public enum TableVariableExpTypeEnum
        {
            String,
            Numeric
        }

        public enum DictVariableOpEnum
        {
            Unset,
            Set,
            Remove,
            SetAll,
            Build,
            Filter,
            Merge,
            MergeHard,
            GetEntity,
            UnsetAll,
            UnsetRegex,
            [Obsolete] GetEntityByName, // => GetEntity
            [Obsolete] GetEntityById, // => GetEntity
        }

        public enum DictVariableExpTypeEnum
        {
            String,
            Numeric
        }

        public enum DiskFileOpEnum
        {
            ReadIntoVariable,
            ReadIntoListVariable,
            ReadCSVIntoTableVariable
        }

        public enum TriggerOpEnum
        {
            FireTrigger,
            CancelTrigger,
            EnableTrigger,
            DisableTrigger,
            CancelAllTrigger
        }

        public enum FolderOpEnum
        {
            EnableFolder,
            DisableFolder,
            CancelFolder,
        }

        public enum AuraOpEnum
        {
            ActivateAura,
            DeactivateAura,
            DeactivateAllAura,
            DeactivateAuraRegex,
            DeactivateAuraTrigger
        }

        public enum MessageBoxIconTypeEnum
        {
            None = 0,
            Error = 16,
            Question = 32,
            Warning = 48,
            Information = 64
        }

        public enum TextAuraAlignmentEnum
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

        public enum ListVariableOpEnum
        {
            Unset,
            Push,
            Insert,
            Set,
            SetAll,
            Remove,
            PopFirst,
            PopToListInsert,
            PopToListSet,
            Build,
            Filter,
            Join,
            Split,
            Copy,
            InsertList,
            SortNumericAsc,
            SortNumericDesc,
            SortAlphaAsc,
            SortAlphaDesc,
            SortFfxivPartyAsc,
            SortFfxivPartyDesc,
            SortByKeys,
            UnsetAll,
            UnsetRegex,
            // Old
            // PopLast => PopFirst
        }

        public enum ListVariableExpTypeEnum
        {
            String,
            Numeric
        }

        public enum ObsControlTypeEnum
        {
            StartStreaming,
            StopStreaming,
            ToggleStreaming,
            StartRecording,
            StopRecording,
            ToggleRecording,
            RestartRecording,
            RestartRecordingIfActive,
            ResumeRecording,
            PauseRecording,
            ToggleRecordPause,
            StartReplayBuffer,
            StopReplayBuffer,
            ToggleReplayBuffer,
            SaveReplayBuffer,
            SetScene,
            ShowSource,
            HideSource,
            JSONPayload
        }

        public enum LiveSplitControlTypeEnum
        {
            StartOrSplit,
            Start,
            Split,
            UndoSplit,
            SkipSplit,
            Reset,
            Pause,
            Resume,
            CustomPayload
        }

        public enum KeypressTypeEnum
        {
            SendKeys,
            WindowMessage,
            WindowMessageCombo
        }

        public enum MutexOpEnum
        {
            Release,
            Acquire
        }

        public enum LogMessageEnum
        {
            Error,
            Warning,
            Custom,
            Custom2,
            Info,
            Verbose,
        }

        [Flags]
        public enum TriggerForceTypeEnum
        {
            NoSkip = 0,
            SkipRegexp = 1,
            SkipConditions = 2,
            SkipRefire = 4,
            SkipParent = 8,
            SkipActive = 16,
            SkipExceptConditions = SkipRegexp | SkipRefire | SkipParent | SkipActive,
            SkipAll = SkipRegexp | SkipConditions | SkipRefire | SkipParent | SkipActive
        }

        [Flags]
        public enum TextAuraEffectEnum
        {
            None = 0,
            Bold = 1,
            Italic = 2,
            Underline = 4,
            Strikeout = 8,
            Outline = 16
        }

        public enum MouseOpEnum
        {
            Move,
            LeftClick,
            MiddleClick,
            RightClick
        }

        public enum MouseCoordEnum
        {
            Absolute,
            Relative
        }

        public enum HTTPMethodEnum
        {
            POST,
            GET
        }

        public enum RepositoryOpEnum
        {
            UpdateSelf,
            UpdateRepo,
            UpdateAll
        }

        public enum TriggerZoneTypeEnum
        {
            ZoneName,
            ZoneIdFFXIV
        }

        public enum ActInteractionTypeEnum
        {
            SetCombatState,
            LogAllNetwork,
            UseDeucalion,
        }

        #endregion

        #region Action specific properties - ACT Interaction operation

        internal ActInteractionTypeEnum _ActOpType { get; set; } = ActInteractionTypeEnum.SetCombatState;
        [XmlAttribute]
        public string ActOpType
        {
            get => XmlAttr.Enum(_ActOpType, ActInteractionTypeEnum.SetCombatState);
            set => _ActOpType = XmlAttr.Enum<ActInteractionTypeEnum>(value);
        }

        internal bool _ActOpBoolParam { get; set; } = false;
        [XmlAttribute]
        public string ActOpBoolParam
        {
            get => XmlAttr.Bool(_ActOpBoolParam, false);
            set => _ActOpBoolParam = XmlAttr.Bool(value);
        }

        internal string _ActOpStringParam = "";
        [XmlAttribute]
        public string ActOpStringParam
        {
            get => XmlAttr.String(_ActOpStringParam, "", omitWhitespace: false);
            set => _ActOpStringParam = value;
        }

        #endregion
        #region Action specific properties - Beep

        internal string _SystemBeepFreqExpression = "1046.5"; // freq(C6)
        [XmlAttribute]
        public string SystemBeepFreqExpression
        {
            get => XmlAttr.String(_SystemBeepFreqExpression, "1046.5", omitWhitespace: false);
            set => _SystemBeepFreqExpression = value;
        }

        internal string _SystemBeepLengthExpression = "100";
        [XmlAttribute]
        public string SystemBeepLengthExpression
        {
            get => XmlAttr.String(_SystemBeepLengthExpression, "100", omitWhitespace: false);
            set => _SystemBeepLengthExpression = value;
        }

        #endregion
        #region Action specific properties - Dict variable

        internal DictVariableOpEnum _DictVariableOp { get; set; } = DictVariableOpEnum.Unset;
        [XmlAttribute]
        public string DictVariableOp
        {
            get => XmlAttr.Enum(_DictVariableOp, DictVariableOpEnum.Unset);
            set
            {
                _DictVariableOp = XmlAttr.Enum<DictVariableOpEnum>(value);
#pragma warning disable CS0612 // obsolete
                if (_DictVariableOp == DictVariableOpEnum.GetEntityByName ||
                    _DictVariableOp == DictVariableOpEnum.GetEntityById)
#pragma warning restore CS0612
                {
                    _DictVariableOp = DictVariableOpEnum.GetEntity;
                }
            }
        }

        internal DictVariableExpTypeEnum _DictVariableKeyType { get; set; } = DictVariableExpTypeEnum.String;
        [XmlAttribute]
        public string DictVariableKeyType
        {
            get => XmlAttr.Enum(_DictVariableKeyType, DictVariableExpTypeEnum.String);
            set => _DictVariableKeyType = XmlAttr.Enum<DictVariableExpTypeEnum>(value);
        }

        internal DictVariableExpTypeEnum _DictVariableValueType { get; set; } = DictVariableExpTypeEnum.String;
        [XmlAttribute]
        public string DictVariableValueType
        {
            get => XmlAttr.Enum(_DictVariableValueType, DictVariableExpTypeEnum.String);
            set => _DictVariableValueType = XmlAttr.Enum<DictVariableExpTypeEnum>(value);
        }

        internal string _DictVariableName = "";
        [XmlAttribute]
        public string DictVariableName
        {
            get => XmlAttr.String(_DictVariableName, "", omitWhitespace: false);
            set => _DictVariableName = value;
        }

        internal string _DictVariableTarget = "";
        [XmlAttribute]
        public string DictVariableTarget
        {
            get => XmlAttr.String(_DictVariableTarget, "", omitWhitespace: false);
            set => _DictVariableTarget = value;
        }

        internal string _DictVariableLength = "";
        [XmlAttribute]
        public string DictVariableLength
        {
            get => XmlAttr.String(_DictVariableLength, "", omitWhitespace: false);
            set => _DictVariableLength = value;
        }

        internal string _DictVariableKey = "";
        [XmlAttribute]
        public string DictVariableKey
        {
            get => XmlAttr.String(_DictVariableKey, "", omitWhitespace: false);
            set => _DictVariableKey = value;
        }

        internal string _DictVariableValue = "";
        [XmlAttribute]
        public string DictVariableValue
        {
            get => XmlAttr.String(_DictVariableValue, "", omitWhitespace: false);
            set => _DictVariableValue = value;
        }

        internal bool _DictSourcePersist { get; set; } = false;
        [XmlAttribute]
        public string DictSourcePersist
        {
            get => XmlAttr.Bool(_DictSourcePersist, false);
            set => _DictSourcePersist = XmlAttr.Bool(value);
        }

        internal bool _DictTargetPersist { get; set; } = false;
        [XmlAttribute]
        public string DictTargetPersist
        {
            get => XmlAttr.Bool(_DictTargetPersist, false);
            set => _DictTargetPersist = XmlAttr.Bool(value);
        }

        #endregion
        #region Action specific properties - Discord webhook

        internal string _DiscordWebhookURL = "";
        [XmlAttribute]
        public string DiscordWebhookURL
        {
            get => XmlAttr.String(_DiscordWebhookURL, "", omitWhitespace: false);
            set => _DiscordWebhookURL = value;
        }

        internal string _DiscordWebhookMessage = "";
        [XmlAttribute]
        public string DiscordWebhookMessage
        {
            get => XmlAttr.String(_DiscordWebhookMessage, "", omitWhitespace: false);
            set => _DiscordWebhookMessage = value;
        }

        internal bool _DiscordTts { get; set; } = false;
        [XmlAttribute]
        public string DiscordTts
        {
            get => XmlAttr.Bool(_DiscordTts, false);
            set => _DiscordTts = XmlAttr.Bool(value);
        }

        #endregion
        #region Action specific properties - Disk operation

        internal DiskFileOpEnum _DiskFileOp { get; set; } = DiskFileOpEnum.ReadIntoVariable;
        [XmlAttribute]
        public string DiskFileOp
        {
            get => XmlAttr.Enum(_DiskFileOp, DiskFileOpEnum.ReadIntoVariable);
            set => _DiskFileOp = XmlAttr.Enum<DiskFileOpEnum>(value);
        }

        internal string _DiskFileOpName;
        [XmlAttribute]
        public string DiskFileOpName
        {
            get => XmlAttr.String(_DiskFileOpName, "", omitWhitespace: false);
            set => _DiskFileOpName = value;
        }
        internal string _DiskFileOpVar;
        [XmlAttribute]
        public string DiskFileOpVar
        {
            get => XmlAttr.String(_DiskFileOpVar, "", omitWhitespace: false);
            set => _DiskFileOpVar = value;
        }

        internal bool _DiskFileCache { get; set; } = false;
        [XmlAttribute]
        public string DiskFileCache
        {
            get => XmlAttr.Bool(_DiskFileCache, false);
            set => _DiskFileCache = XmlAttr.Bool(value);
        }

        internal bool _DiskPersist { get; set; } = false;
        [XmlAttribute]
        public string DiskPersist
        {
            get => XmlAttr.Bool(_DiskPersist, false);
            set => _DiskPersist = XmlAttr.Bool(value);
        }

        #endregion
        #region Action specific properties - Execute script

        internal string _ExecScriptAssembliesExpression = "";
        [XmlAttribute]
        public string ExecScriptAssembliesExpression
        {
            get => XmlAttr.String(_ExecScriptAssembliesExpression, "", omitWhitespace: false);
            set => _ExecScriptAssembliesExpression = value;
        }

        internal string _ExecScriptExpression = "";
        [XmlAttribute]
        public string ExecScriptExpression
        {
            get => XmlAttr.String(_ExecScriptExpression, "", omitWhitespace: false);
            set => _ExecScriptExpression = value;
        }

        #endregion
        #region Action specific properties - Folder operation

        internal FolderOpEnum _FolderOp { get; set; } = FolderOpEnum.EnableFolder;
        [XmlAttribute]
        public string FolderOp
        {
            get => XmlAttr.Enum(_FolderOp, FolderOpEnum.EnableFolder);
            set => _FolderOp = XmlAttr.Enum<FolderOpEnum>(value);
        }

        internal Guid _FolderId { get; set; } = Guid.Empty;
        [XmlAttribute]
        public string FolderId
        {
            get => XmlAttr.Guid(_FolderId, Guid.Empty);
            set => _FolderId = XmlAttr.Guid(value);
        }

        #endregion
        #region Action specific properties - Image aura

        internal AuraOpEnum _AuraOp { get; set; } = AuraOpEnum.ActivateAura;
        [XmlAttribute]
        public string AuraOp
        {
            get => XmlAttr.Enum(_AuraOp, AuraOpEnum.ActivateAura);
            set => _AuraOp = XmlAttr.Enum<AuraOpEnum>(value);
        }

        internal PictureBoxSizeMode _AuraImageMode { get; set; } = PictureBoxSizeMode.Normal;
        [XmlAttribute]
        public string AuraImageMode
        {
            get => XmlAttr.Enum(_AuraImageMode, PictureBoxSizeMode.Normal);
            set => _AuraImageMode = XmlAttr.Enum<PictureBoxSizeMode>(value);
        }

        internal string _AuraName = "";
        [XmlAttribute]
        public string AuraName
        {
            get => XmlAttr.String(_AuraName, "", omitWhitespace: false);
            set => _AuraName = value;
        }

        internal string _AuraImage = "";
        [XmlAttribute]
        public string AuraImage
        {
            get => XmlAttr.String(_AuraImage, "", omitWhitespace: false);
            set => _AuraImage = value;
        }

        internal string _AuraXIniExpression = "";
        [XmlAttribute]
        public string AuraXIniExpression
        {
            get => XmlAttr.String(_AuraXIniExpression, "", omitWhitespace: false);
            set => _AuraXIniExpression = value;
        }

        internal string _AuraYIniExpression = "";
        [XmlAttribute]
        public string AuraYIniExpression
        {
            get => XmlAttr.String(_AuraYIniExpression, "", omitWhitespace: false);
            set => _AuraYIniExpression = value;
        }

        internal string _AuraWIniExpression = "";
        [XmlAttribute]
        public string AuraWIniExpression
        {
            get => XmlAttr.String(_AuraWIniExpression, "", omitWhitespace: false);
            set => _AuraWIniExpression = value;
        }

        internal string _AuraHIniExpression = "";
        [XmlAttribute]
        public string AuraHIniExpression
        {
            get => XmlAttr.String(_AuraHIniExpression, "", omitWhitespace: false);
            set => _AuraHIniExpression = value;
        }

        internal string _AuraOIniExpression = "";
        [XmlAttribute]
        public string AuraOIniExpression
        {
            get => XmlAttr.String(_AuraOIniExpression, "", omitWhitespace: false);
            set => _AuraOIniExpression = value;
        }

        internal string _AuraXTickExpression = "";
        [XmlAttribute]
        public string AuraXTickExpression
        {
            get => XmlAttr.String(_AuraXTickExpression, "", omitWhitespace: false);
            set => _AuraXTickExpression = value;
        }

        internal string _AuraYTickExpression = "";
        [XmlAttribute]
        public string AuraYTickExpression
        {
            get => XmlAttr.String(_AuraYTickExpression, "", omitWhitespace: false);
            set => _AuraYTickExpression = value;
        }

        internal string _AuraWTickExpression = "";
        [XmlAttribute]
        public string AuraWTickExpression
        {
            get => XmlAttr.String(_AuraWTickExpression, "", omitWhitespace: false);
            set => _AuraWTickExpression = value;
        }

        internal string _AuraHTickExpression = "";
        [XmlAttribute]
        public string AuraHTickExpression
        {
            get => XmlAttr.String(_AuraHTickExpression, "", omitWhitespace: false);
            set => _AuraHTickExpression = value;
        }

        internal string _AuraOTickExpression = "";
        [XmlAttribute]
        public string AuraOTickExpression
        {
            get => XmlAttr.String(_AuraOTickExpression, "", omitWhitespace: false);
            set => _AuraOTickExpression = value;
        }

        internal string _AuraTTLTickExpression = "";
        [XmlAttribute]
        public string AuraTTLTickExpression
        {
            get => XmlAttr.String(_AuraTTLTickExpression, "", omitWhitespace: false);
            set => _AuraTTLTickExpression = value;
        }

        #endregion
        #region Action specific properties - JSON

        internal HTTPMethodEnum _JsonOperationType { get; set; } = HTTPMethodEnum.POST;
        [XmlAttribute]
        public string JsonOperationType
        {
            get => XmlAttr.Enum(_JsonOperationType, HTTPMethodEnum.POST);
            set => _JsonOperationType = XmlAttr.Enum<HTTPMethodEnum>(value);
        }

        internal bool _JsonCacheRequest { get; set; } = false;
        [XmlAttribute]
        public string JsonCacheRequest
        {
            get => XmlAttr.Bool(_JsonCacheRequest, false);
            set => _JsonCacheRequest = XmlAttr.Bool(value);
        }

        internal string _JsonResultVariable = "";
        [XmlAttribute]
        public string JsonResultVariable
        {
            get => XmlAttr.String(_JsonResultVariable, "", omitWhitespace: false);
            set => _JsonResultVariable = value;
        }

        internal string _JsonEndpointExpression = "";
        [XmlAttribute]
        public string JsonEndpointExpression
        {
            get => XmlAttr.String(_JsonEndpointExpression, "", omitWhitespace: false);
            set => _JsonEndpointExpression = value;
        }

        internal string _JsonPayloadExpression = "";
        [XmlAttribute]
        public string JsonPayloadExpression
        {
            get => XmlAttr.String(_JsonPayloadExpression, "", omitWhitespace: false);
            set => _JsonPayloadExpression = value;
        }

        internal string _JsonHeaderExpression = "";
        [XmlAttribute]
        public string JsonHeaderExpression
        {
            get => XmlAttr.String(_JsonHeaderExpression, "", omitWhitespace: false);
            set => _JsonHeaderExpression = value;
        }

        internal string _JsonFiringExpression = "";
        [XmlAttribute]
        public string JsonFiringExpression
        {
            get => XmlAttr.String(_JsonFiringExpression, "", omitWhitespace: false);
            set => _JsonFiringExpression = value;
        }

        internal bool _JsonResultVariablePersist { get; set; } = false;
        [XmlAttribute]
        public string JsonResultVariablePersist
        {
            get => XmlAttr.Bool(_JsonResultVariablePersist, false);
            set => _JsonResultVariablePersist = XmlAttr.Bool(value);
        }

        #endregion
        #region Action specific properties - Keypress

        internal KeypressTypeEnum _KeypressType { get; set; } = KeypressTypeEnum.SendKeys;
        [XmlAttribute]
        public string KeypressType
        {
            get => XmlAttr.Enum(_KeypressType, KeypressTypeEnum.SendKeys);
            set => _KeypressType = XmlAttr.Enum<KeypressTypeEnum>(value);
        }

        internal string _KeyPressExpression = "";
        [XmlAttribute]
        public string KeyPressExpression
        {
            get => XmlAttr.String(_KeyPressExpression, "", omitWhitespace: false);
            set => _KeyPressExpression = value;
        }

        internal string _KeyPressCode = "";
        [XmlAttribute]
        public string KeyPressCode
        {
            get => XmlAttr.String(_KeyPressCode, "", omitWhitespace: false);
            set => _KeyPressCode = value;
        }

        internal string _KeyPressWindow = "";
        [XmlAttribute]
        public string KeyPressWindow
        {
            get => XmlAttr.String(_KeyPressWindow, "", omitWhitespace: false);
            set => _KeyPressWindow = value;
        }

        internal string _KeyPressProcId = "";
        [XmlAttribute]
        public string KeyPressProcId
        {
            get => XmlAttr.String(_KeyPressProcId, "", omitWhitespace: false);
            set => _KeyPressProcId = value;
        }

        #endregion
        #region Action specific properties - Launch process

        internal System.Diagnostics.ProcessWindowStyle _LaunchProcessWindowStyle { get; set; } = System.Diagnostics.ProcessWindowStyle.Normal;
        [XmlAttribute]
        public string LaunchProcessWindowStyle
        {
            get => XmlAttr.Enum(_LaunchProcessWindowStyle, System.Diagnostics.ProcessWindowStyle.Normal);
            set => _LaunchProcessWindowStyle = XmlAttr.Enum<System.Diagnostics.ProcessWindowStyle>(value);
        }

        internal string _LaunchProcessPathExpression = "";
        [XmlAttribute]
        public string LaunchProcessPathExpression
        {
            get => XmlAttr.String(_LaunchProcessPathExpression, "", omitWhitespace: false);
            set => _LaunchProcessPathExpression = value;
        }

        internal string _LaunchProcessCmdlineExpression = "";
        [XmlAttribute]
        public string LaunchProcessCmdlineExpression
        {
            get => XmlAttr.String(_LaunchProcessCmdlineExpression, "", omitWhitespace: false);
            set => _LaunchProcessCmdlineExpression = value;
        }

        internal string _LaunchProcessWorkingDirExpression = "";
        [XmlAttribute]
        public string LaunchProcessWorkingDirExpression
        {
            get => XmlAttr.String(_LaunchProcessWorkingDirExpression, "", omitWhitespace: false);
            set => _LaunchProcessWorkingDirExpression = value;
        }

        #endregion
        #region Action specific properties - List variable

        internal ListVariableOpEnum _ListVariableOp { get; set; } = ListVariableOpEnum.Unset;
        [XmlAttribute]
        public string ListVariableOp
        {
            get => XmlAttr.Enum(_ListVariableOp, ListVariableOpEnum.Unset);
            set
            {
                if (XmlAttr.TryEnum(value, out ListVariableOpEnum type) || string.IsNullOrEmpty(value))
                {
                    _ListVariableOp = type;
                }
                // convert old actions
                else if (value == "PopLast")
                {
                    _ListVariableOp = ListVariableOpEnum.PopFirst;
                    _ListVariableIndex = "-1";
                }
                else throw InvalidEnumException("ListVariableOpEnum", value);
            }
        }

        internal ListVariableExpTypeEnum _ListVariableExpressionType { get; set; } = ListVariableExpTypeEnum.String;
        [XmlAttribute]
        public string ListVariableExpressionType
        {
            get => XmlAttr.Enum(_ListVariableExpressionType, ListVariableExpTypeEnum.String);
            set => _ListVariableExpressionType = XmlAttr.Enum<ListVariableExpTypeEnum>(value);
        }

        internal string _ListVariableName = "";
        [XmlAttribute]
        public string ListVariableName
        {
            get => XmlAttr.String(_ListVariableName, "", omitWhitespace: false);
            set => _ListVariableName = value;
        }

        internal string _ListVariableExpression = "";
        [XmlAttribute]
        public string ListVariableExpression
        {
            get => XmlAttr.String(_ListVariableExpression, "", omitWhitespace: false);
            set => _ListVariableExpression = value;
        }

        internal string _ListVariableIndex = "";
        [XmlAttribute]
        public string ListVariableIndex
        {
            get => XmlAttr.String(_ListVariableIndex, "", omitWhitespace: false);
            set => _ListVariableIndex = value;
        }

        internal string _ListVariableTarget = "";
        [XmlAttribute]
        public string ListVariableTarget
        {
            get => XmlAttr.String(_ListVariableTarget, "", omitWhitespace: false);
            set => _ListVariableTarget = value;
        }

        internal bool _ListSourcePersist { get; set; } = false;
        [XmlAttribute]
        public string ListSourcePersist
        {
            get => XmlAttr.Bool(_ListSourcePersist, false);
            set => _ListSourcePersist = XmlAttr.Bool(value);
        }

        internal bool _ListTargetPersist { get; set; } = false;
        [XmlAttribute]
        public string ListTargetPersist
        {
            get => XmlAttr.Bool(_ListTargetPersist, false);
            set => _ListTargetPersist = XmlAttr.Bool(value);
        }

        #endregion
        #region Action specific properties - Log message

        internal LogEvent.SourceEnum _LogMessageTarget { get; set; } = LogEvent.SourceEnum.Log;
        [XmlAttribute]
        public string LogMessageTarget
        {
            get => XmlAttr.Enum(_LogMessageTarget, LogEvent.SourceEnum.Log);
            set => _LogMessageTarget = XmlAttr.Enum<LogEvent.SourceEnum>(value);
        }

        internal string _LogMessageText = "";
        [XmlAttribute]
        public string LogMessageText
        {
            get => XmlAttr.String(_LogMessageText, "", omitWhitespace: false);
            set => _LogMessageText = value;
        }

        internal bool _LogProcess { get; set; } = false;
        [XmlAttribute]
        public string LogProcess
        {
            get => XmlAttr.Bool(_LogProcess, false);
            set => _LogProcess = XmlAttr.Bool(value);
        }

        internal bool _LogProcessACT { get; set; } = false;
        [XmlAttribute]
        public string LogProcessACT
        {
            get => XmlAttr.Bool(_LogProcessACT, false);
            set => _LogProcessACT = XmlAttr.Bool(value);
        }

        internal LogMessageEnum _LogLevel { get; set; } = LogMessageEnum.Error;
        [XmlAttribute]
        public string LogLevel
        {
            get => XmlAttr.Enum(_LogLevel, LogMessageEnum.Error);
            set
            {
                _LogLevel = XmlAttr.Enum<LogMessageEnum>(value);
                if ((int)_LogLevel == -1)
                {
                    _LogLevel = LogMessageEnum.Error;
                }
            }
        }

        #endregion
        #region Action specific properties - Loop

        public bool ShouldSerializeLoopCondition()
        {
            return LoopCondition.Children.Count > 0;
        }

        public ConditionGroup LoopCondition = new ConditionGroup();

        public bool ShouldSerializeLoopActions()
        {
            return LoopActions.Count > 0;
        }

        // 子动作在读取时已转换并完成旧版排序，不再转回旧模型。
        [XmlIgnore]
        public List<ActionBase> LoopActions = new List<ActionBase>();

        [XmlElement("LoopActions")]
        public ActionXmlCollection Xml_LoopActions
        {
            get => new ActionXmlCollection(LoopActions);
            set => LoopActions = value.Actions;
        }

        public bool ShouldSerializeXml_LoopActions() => ShouldSerializeLoopActions();

        internal string _LoopDelayExpression = "";
        [XmlAttribute]
        public string LoopDelayExpression
        {
            get => XmlAttr.String(_LoopDelayExpression, "", omitWhitespace: false);
            set => _LoopDelayExpression = value;
        }

        internal string _LoopInitExpression = "0";
        [XmlAttribute]
        public string LoopInitExpression
        {
            get => XmlAttr.String(_LoopInitExpression, "0", omitWhitespace: false);
            set => _LoopInitExpression = value;
        }

        internal string _LoopIncrExpression = "1";
        [XmlAttribute]
        public string LoopIncrExpression
        {
            get => XmlAttr.String(_LoopIncrExpression, "1", omitWhitespace: false);
            set => _LoopIncrExpression = value;
        }

        #endregion
        #region Action specific properties - Message box

        internal MessageBoxIconTypeEnum _MessageBoxIconType { get; set; } = MessageBoxIconTypeEnum.None;
        [XmlAttribute]
        public string MessageBoxIconType
        {
            get => XmlAttr.Enum(_MessageBoxIconType, MessageBoxIconTypeEnum.None);
            set => _MessageBoxIconType = XmlAttr.Enum<MessageBoxIconTypeEnum>(value);
        }

        internal string _MessageBoxText = "";
        [XmlAttribute]
        public string MessageBoxText
        {
            get => XmlAttr.String(_MessageBoxText, "", omitWhitespace: false);
            set => _MessageBoxText = value;
        }

        #endregion
        #region Action specific properties - Mouse

        internal MouseOpEnum _MouseOpType { get; set; } = MouseOpEnum.Move;
        [XmlAttribute]
        public string MouseOpType
        {
            get => XmlAttr.Enum(_MouseOpType, MouseOpEnum.Move);
            set => _MouseOpType = XmlAttr.Enum<MouseOpEnum>(value);
        }

        internal MouseCoordEnum _MouseCoordType { get; set; } = MouseCoordEnum.Absolute;
        [XmlAttribute]
        public string MouseCoordType
        {
            get => XmlAttr.Enum(_MouseCoordType, MouseCoordEnum.Absolute);
            set => _MouseCoordType = XmlAttr.Enum<MouseCoordEnum>(value);
        }

        internal string _MouseX = "0";
        [XmlAttribute]
        public string MouseX
        {
            get => XmlAttr.String(_MouseX, false, "0", "");
            set => _MouseX = value;
        }

        internal string _MouseY = "0";
        [XmlAttribute]
        public string MouseY
        {
            get => XmlAttr.String(_MouseY, false, "0", "");
            set => _MouseY = value;
        }

        #endregion
        #region Action specific properties - Mutex

        internal MutexOpEnum _MutexOpType { get; set; } = MutexOpEnum.Release;
        [XmlAttribute]
        public string MutexOpType
        {
            get => XmlAttr.Enum(_MutexOpType, MutexOpEnum.Release);
            set => _MutexOpType = XmlAttr.Enum<MutexOpEnum>(value);
        }

        internal string _MutexName = "";
        [XmlAttribute]
        public string MutexName
        {
            get => XmlAttr.String(_MutexName, "", omitWhitespace: false);
            set => _MutexName = value;
        }

        #endregion
        #region Action specific properties - Named callback

        internal string _NamedCallbackName = "";
        [XmlAttribute]
        public string NamedCallbackName
        {
            get => XmlAttr.String(_NamedCallbackName, "", omitWhitespace: false);
            set => _NamedCallbackName = value;
        }

        internal string _NamedCallbackParam = "";
        [XmlAttribute]
        public string NamedCallbackParam
        {
            get => XmlAttr.String(_NamedCallbackParam, "", omitWhitespace: false);
            set => _NamedCallbackParam = value;
        }

        #endregion
        #region Action specific properties - OBS

        internal ObsControlTypeEnum _OBSControlType { get; set; } = ObsControlTypeEnum.StartStreaming;
        [XmlAttribute]
        public string OBSControlType
        {
            get => XmlAttr.Enum(_OBSControlType, ObsControlTypeEnum.StartStreaming);
            set => _OBSControlType = XmlAttr.Enum<ObsControlTypeEnum>(value);
        }

        internal string _OBSEndPoint = "";
        [XmlAttribute]
        public string OBSEndPoint
        {
            get => XmlAttr.String(_OBSEndPoint, "");
            set => _OBSEndPoint = value;
        }

        internal string _OBSPassword = "";
        [XmlAttribute]
        public string OBSPassword
        {
            get => XmlAttr.String(_OBSPassword, "");
            set => _OBSPassword = value;
        }

        internal string _OBSSceneName = "";
        [XmlAttribute]
        public string OBSSceneName
        {
            get => XmlAttr.String(_OBSSceneName, "", omitWhitespace: false);
            set => _OBSSceneName = value;
        }

        internal string _OBSSourceName = "";
        [XmlAttribute]
        public string OBSSourceName
        {
            get => XmlAttr.String(_OBSSourceName, "", omitWhitespace: false);
            set => _OBSSourceName = value;
        }

        internal string _OBSJSONPayload = "";
        [XmlAttribute]
        public string OBSJSONPayload
        {
            get => XmlAttr.String(_OBSJSONPayload, "", omitWhitespace: false);
            set => _OBSJSONPayload = value;
        }

        #endregion
        #region Action specific properties - LiveSplit
        internal LiveSplitControlTypeEnum _LSControlType { get; set; } = LiveSplitControlTypeEnum.StartOrSplit;
        [XmlAttribute]
        public string LiveSplitControlType
        {
            get => XmlAttr.Enum(_LSControlType, LiveSplitControlTypeEnum.StartOrSplit);
            set => _LSControlType = XmlAttr.Enum<LiveSplitControlTypeEnum>(value);
        }
        internal string _LSCustomPayload = "";
        [XmlAttribute]
        public string LiveSplitCustomPayload
        {
            get => _LSControlType == LiveSplitControlTypeEnum.CustomPayload ? _LSCustomPayload : null;
            set => _LSCustomPayload = value;
        }
        #endregion
        #region Action specific properties - Placeholder
        #endregion
        #region Action specific properties - Play sound

        internal string _PlaySoundFileExpression = "";
        [XmlAttribute]
        public string PlaySoundFileExpression
        {
            get => XmlAttr.String(_PlaySoundFileExpression, "", omitWhitespace: false);
            set => _PlaySoundFileExpression = value;
        }

        internal string _PlaySoundVolumeExpression = "100";
        [XmlAttribute]
        public string PlaySoundVolumeExpression
        {
            get => XmlAttr.String(_PlaySoundVolumeExpression, "100", omitWhitespace: false);
            set => _PlaySoundVolumeExpression = value;
        }

        internal bool _PlaySoundExclusive { get; set; } = true;
        [XmlAttribute]
        public string PlaySoundExclusive
        {
            get => XmlAttr.Bool(_PlaySoundExclusive, true);
            set => _PlaySoundExclusive = XmlAttr.Bool(value);
        }

        internal Configuration.AudioRoutingMethodEnum _SoundRouting = Configuration.AudioRoutingMethodEnum.None;
        [XmlAttribute]
        public string SoundRouting
        {
            get => XmlAttr.Enum(_SoundRouting, Configuration.AudioRoutingMethodEnum.None);
            set => _SoundRouting = XmlAttr.Enum<Configuration.AudioRoutingMethodEnum>(value);
        }

        [XmlAttribute]
        public string PlaySoundMyself // old version compatibility
        {
            get => null;
            set
            {
                if (XmlAttr.Bool(value)) _SoundRouting = Configuration.AudioRoutingMethodEnum.Triggernometry;
            }
        }

        #endregion
        #region Action specific properties - Play speech

        internal Configuration.AudioRoutingMethodEnum _TTSRouting = Configuration.AudioRoutingMethodEnum.None;
        [XmlAttribute]
        public string TTSRouting
        {
            get => XmlAttr.Enum(_TTSRouting, Configuration.AudioRoutingMethodEnum.None);
            set => _TTSRouting = XmlAttr.Enum<Configuration.AudioRoutingMethodEnum>(value);
        }

        [XmlAttribute]
        public string PlaySpeechMyself // old version compatibility
        {
            get => null;
            set
            {
                if (XmlAttr.Bool(value)) _TTSRouting = Configuration.AudioRoutingMethodEnum.Triggernometry;
            }
        }

        internal string _UseTTSTextExpression = "";
        [XmlAttribute]
        public string UseTTSTextExpression
        {
            get => XmlAttr.String(_UseTTSTextExpression, "", omitWhitespace: false);
            set => _UseTTSTextExpression = value;
        }

        internal string _UseTTSVolumeExpression = "100";
        [XmlAttribute]
        public string UseTTSVolumeExpression
        {
            get => XmlAttr.String(_UseTTSVolumeExpression, "100", omitWhitespace: false);
            set => _UseTTSVolumeExpression = value;
        }

        internal string _UseTTSRateExpression = "0";
        [XmlAttribute]
        public string UseTTSRateExpression
        {
            get => XmlAttr.String(_UseTTSRateExpression, "0", omitWhitespace: false);
            set => _UseTTSRateExpression = value;
        }

        #endregion
        #region Action specific properties - Repository

        internal RepositoryOpEnum _RepositoryOp { get; set; } = RepositoryOpEnum.UpdateSelf;
        [XmlAttribute]
        public string RepositoryOp
        {
            get => XmlAttr.Enum(_RepositoryOp, RepositoryOpEnum.UpdateSelf);
            set => _RepositoryOp = XmlAttr.Enum<RepositoryOpEnum>(value);
        }

        internal Guid _RepositoryId { get; set; } = Guid.Empty;
        [XmlAttribute]
        public string RepositoryId
        {
            get => XmlAttr.Guid(_RepositoryId, Guid.Empty);
            set => _RepositoryId = XmlAttr.Guid(value);
        }

        #endregion
        #region Action specific properties - Scalar variable

        internal VariableOpEnum _VariableOp { get; set; } = VariableOpEnum.Unset;
        [XmlAttribute]
        public string VariableOp
        {
            get => XmlAttr.Enum(_VariableOp, VariableOpEnum.Unset);
            set => _VariableOp = XmlAttr.Enum<VariableOpEnum>(value);
        }

        internal string _VariableName = "";
        [XmlAttribute]
        public string VariableName
        {
            get => XmlAttr.String(_VariableName, "", omitWhitespace: false);
            set => _VariableName = value;
        }

        internal string _VariableJsonTarget = "";
        [XmlAttribute]
        public string VariableJsonTarget
        {
            get => XmlAttr.String(_VariableJsonTarget, "", omitWhitespace: false);
            set => _VariableJsonTarget = value;
        }

        internal string _VariableExpression = "";
        [XmlAttribute]
        public string VariableExpression
        {
            get => XmlAttr.String(_VariableExpression, "", omitWhitespace: false);
            set => _VariableExpression = value;
        }

        internal bool _VariableTargetPersist { get; set; } = false;
        [XmlAttribute]
        public string VariableTargetPersist
        {
            get => XmlAttr.Bool(_VariableTargetPersist, false);
            set => _VariableTargetPersist = XmlAttr.Bool(value);
        }

        internal bool _VariablePersist { get; set; } = false;
        [XmlAttribute]
        public string VariablePersist
        {
            get => XmlAttr.Bool(_VariablePersist, false);
            set => _VariablePersist = XmlAttr.Bool(value);
        }

        #endregion
        #region Action specific properties - Table variable

        internal TableVariableOpEnum _TableVariableOp { get; set; } = TableVariableOpEnum.Unset;
        [XmlAttribute]
        public string TableVariableOp
        {
            get => XmlAttr.Enum(_TableVariableOp, TableVariableOpEnum.Unset);
            set => _TableVariableOp = XmlAttr.Enum<TableVariableOpEnum>(value);
        }

        internal TableVariableExpTypeEnum _TableVariableExpressionType { get; set; } = TableVariableExpTypeEnum.String;
        [XmlAttribute]
        public string TableVariableExpressionType
        {
            get => XmlAttr.Enum(_TableVariableExpressionType, TableVariableExpTypeEnum.String);
            set => _TableVariableExpressionType = XmlAttr.Enum<TableVariableExpTypeEnum>(value);
        }

        internal string _TableVariableName = "";
        [XmlAttribute]
        public string TableVariableName
        {
            get => XmlAttr.String(_TableVariableName, "", omitWhitespace: false);
            set => _TableVariableName = value;
        }

        internal string _TableVariableTarget = "";
        [XmlAttribute]
        public string TableVariableTarget
        {
            get => XmlAttr.String(_TableVariableTarget, "", omitWhitespace: false);
            set => _TableVariableTarget = value;
        }

        internal string _TableVariableExpression = "";
        [XmlAttribute]
        public string TableVariableExpression
        {
            get => XmlAttr.String(_TableVariableExpression, "", omitWhitespace: false);
            set => _TableVariableExpression = value;
        }

        internal string _TableVariableX = "";
        [XmlAttribute]
        public string TableVariableX
        {
            get => XmlAttr.String(_TableVariableX, "", omitWhitespace: false);
            set => _TableVariableX = value;
        }

        internal string _TableVariableY = "";
        [XmlAttribute]
        public string TableVariableY
        {
            get => XmlAttr.String(_TableVariableY, "", omitWhitespace: false);
            set => _TableVariableY = value;
        }

        internal bool _TableSourcePersist { get; set; } = false;
        [XmlAttribute]
        public string TableSourcePersist
        {
            get => XmlAttr.Bool(_TableSourcePersist, false);
            set => _TableSourcePersist = XmlAttr.Bool(value);
        }

        internal bool _TableTargetPersist { get; set; } = false;
        [XmlAttribute]
        public string TableTargetPersist
        {
            get => XmlAttr.Bool(_TableTargetPersist, false);
            set => _TableTargetPersist = XmlAttr.Bool(value);
        }

        #endregion
        #region Action specific properties - Text aura

        internal AuraOpEnum _TextAuraOp { get; set; } = AuraOpEnum.ActivateAura;
        [XmlAttribute]
        public string TextAuraOp
        {
            get => XmlAttr.Enum(_TextAuraOp, AuraOpEnum.ActivateAura);
            set => _TextAuraOp = XmlAttr.Enum<AuraOpEnum>(value);
        }

        internal TextAuraAlignmentEnum _TextAuraAlignment { get; set; } = TextAuraAlignmentEnum.MiddleCenter;
        [XmlAttribute]
        public string TextAuraAlignment
        {
            get => XmlAttr.Enum(_TextAuraAlignment, TextAuraAlignmentEnum.MiddleCenter);
            set => _TextAuraAlignment = XmlAttr.Enum<TextAuraAlignmentEnum>(value);
        }

        internal TextAuraEffectEnum _TextAuraEffect { get; set; } = TextAuraEffectEnum.None;
        [XmlAttribute]
        public string TextAuraEffect
        {
            get => XmlAttr.Enum(_TextAuraEffect, TextAuraEffectEnum.None);
            set
            {
                TextAuraEffectEnum tea = 0;
                string[] ex = value.Split(' ');
                foreach (string exx in ex)
                {
                    tea |= XmlAttr.Enum<TextAuraEffectEnum>(exx.Replace(",", ""));
                }
                _TextAuraEffect = tea;
            }
        }

        internal float _TextAuraFontSize { get; set; } = 10.0f;
        [XmlAttribute]
        public string TextAuraFontSize
        {
            get => ActionType == ActionTypeEnum.TextAura ? I18n.ThingToString(_TextAuraFontSize) : null;
            set => _TextAuraFontSize = XmlAttr.Float(value);
        }

        internal string _TextAuraForegroundClInt = "";
        [XmlAttribute]
        public string TextAuraForeground
        {
            get => XmlAttr.String(_TextAuraForegroundClInt, "");
            set => _TextAuraForegroundClInt = value;
        }

        internal string _TextAuraBackgroundClInt = "";
        [XmlAttribute]
        public string TextAuraBackground
        {
            get => XmlAttr.String(_TextAuraBackgroundClInt, "");
            set => _TextAuraBackgroundClInt = value;
        }

        internal string _TextAuraOutlineClInt = "";
        [XmlAttribute]
        public string TextAuraOutline
        {
            get => XmlAttr.String(_TextAuraOutlineClInt, "");
            set => _TextAuraOutlineClInt = value;
        }

        internal string _TextAuraName = "";
        [XmlAttribute]
        public string TextAuraName
        {
            get => XmlAttr.String(_TextAuraName, "", omitWhitespace: false);
            set => _TextAuraName = value;
        }

        internal string _TextAuraExpression = "";
        [XmlAttribute]
        public string TextAuraExpression
        {
            get => XmlAttr.String(_TextAuraExpression, "", omitWhitespace: false);
            set => _TextAuraExpression = value;
        }

        internal string _TextAuraXIniExpression = "";
        [XmlAttribute]
        public string TextAuraXIniExpression
        {
            get => XmlAttr.String(_TextAuraXIniExpression, "", omitWhitespace: false);
            set => _TextAuraXIniExpression = value;
        }

        internal string _TextAuraYIniExpression = "";
        [XmlAttribute]
        public string TextAuraYIniExpression
        {
            get => XmlAttr.String(_TextAuraYIniExpression, "", omitWhitespace: false);
            set => _TextAuraYIniExpression = value;
        }

        internal string _TextAuraWIniExpression = "";
        [XmlAttribute]
        public string TextAuraWIniExpression
        {
            get => XmlAttr.String(_TextAuraWIniExpression, "", omitWhitespace: false);
            set => _TextAuraWIniExpression = value;
        }

        internal string _TextAuraHIniExpression = "";
        [XmlAttribute]
        public string TextAuraHIniExpression
        {
            get => XmlAttr.String(_TextAuraHIniExpression, "", omitWhitespace: false);
            set => _TextAuraHIniExpression = value;
        }

        internal string _TextAuraOIniExpression = "";
        [XmlAttribute]
        public string TextAuraOIniExpression
        {
            get => XmlAttr.String(_TextAuraOIniExpression, "", omitWhitespace: false);
            set => _TextAuraOIniExpression = value;
        }

        internal string _TextAuraXTickExpression = "";
        [XmlAttribute]
        public string TextAuraXTickExpression
        {
            get => XmlAttr.String(_TextAuraXTickExpression, "", omitWhitespace: false);
            set => _TextAuraXTickExpression = value;
        }

        internal string _TextAuraYTickExpression = "";
        [XmlAttribute]
        public string TextAuraYTickExpression
        {
            get => XmlAttr.String(_TextAuraYTickExpression, "", omitWhitespace: false);
            set => _TextAuraYTickExpression = value;
        }

        internal string _TextAuraWTickExpression = "";
        [XmlAttribute]
        public string TextAuraWTickExpression
        {
            get => XmlAttr.String(_TextAuraWTickExpression, "", omitWhitespace: false);
            set => _TextAuraWTickExpression = value;
        }

        internal string _TextAuraHTickExpression = "";
        [XmlAttribute]
        public string TextAuraHTickExpression
        {
            get => XmlAttr.String(_TextAuraHTickExpression, "", omitWhitespace: false);
            set => _TextAuraHTickExpression = value;
        }

        internal string _TextAuraOTickExpression = "";
        [XmlAttribute]
        public string TextAuraOTickExpression
        {
            get => XmlAttr.String(_TextAuraOTickExpression, "", omitWhitespace: false);
            set => _TextAuraOTickExpression = value;
        }

        internal string _TextAuraTTLTickExpression = "";
        [XmlAttribute]
        public string TextAuraTTLTickExpression
        {
            get => XmlAttr.String(_TextAuraTTLTickExpression, "", omitWhitespace: false);
            set => _TextAuraTTLTickExpression = value;
        }

        internal string _TextAuraFontName = "";
        [XmlAttribute]
        public string TextAuraFontName
        {
            get => ActionType == ActionTypeEnum.TextAura ? XmlAttr.String(_TextAuraFontName, "", omitWhitespace: false) : null;
            set => _TextAuraFontName = value;
        }

        #endregion
        #region Action specific properties - Trigger operation

        internal TriggerZoneTypeEnum _TriggerZoneType { get; set; } = TriggerZoneTypeEnum.ZoneName;
        [XmlAttribute]
        public string TriggerZoneType
        {
            get => XmlAttr.Enum(_TriggerZoneType, TriggerZoneTypeEnum.ZoneName);
            set => _TriggerZoneType = XmlAttr.Enum<TriggerZoneTypeEnum>(value);
        }

        internal TriggerOpEnum _TriggerOp { get; set; } = TriggerOpEnum.FireTrigger;
        [XmlAttribute]
        public string TriggerOp
        {
            get => XmlAttr.Enum(_TriggerOp, TriggerOpEnum.FireTrigger);
            set => _TriggerOp = XmlAttr.Enum<TriggerOpEnum>(value);
        }

        internal Guid _TriggerId { get; set; } = Guid.Empty;
        [XmlAttribute]
        public string TriggerId
        {
            get => XmlAttr.Guid(_TriggerId, Guid.Empty);
            set => _TriggerId = XmlAttr.Guid(value);
        }

        internal string _TriggerText = "";
        [XmlAttribute]
        public string TriggerText
        {
            get => XmlAttr.String(_TriggerText, "", omitWhitespace: false);
            set => _TriggerText = value;
        }

        internal string _TriggerZone = "";
        [XmlAttribute]
        public string TriggerZone
        {
            get => XmlAttr.String(_TriggerZone, "", omitWhitespace: false);
            set => _TriggerZone = value;
        }

        internal string _TriggerTagRegex = "";
        [XmlAttribute]
        public string TriggerTagRegex
        {
            get => XmlAttr.String(_TriggerTagRegex, "");
            set => _TriggerTagRegex = value;
        }

        [XmlIgnore]
        internal TriggerForceTypeEnum _TriggerForceType { get; set; } = TriggerForceTypeEnum.NoSkip;
        [XmlAttribute]
        public string TriggerForce
        {
            get
            {
                List<string> ex = new List<string>();
                if (_TriggerForceType == TriggerForceTypeEnum.SkipAll)
                {
                    ex.Add("true");
                }
                else
                {
                    if ((_TriggerForceType & TriggerForceTypeEnum.SkipRegexp) != 0)
                    {
                        ex.Add("regexp");
                    }
                    if ((_TriggerForceType & TriggerForceTypeEnum.SkipConditions) != 0)
                    {
                        ex.Add("conditions");
                    }
                    if ((_TriggerForceType & TriggerForceTypeEnum.SkipRefire) != 0)
                    {
                        ex.Add("refire");
                    }
                    if ((_TriggerForceType & TriggerForceTypeEnum.SkipParent) != 0)
                    {
                        ex.Add("parent");
                    }
                    if ((_TriggerForceType & TriggerForceTypeEnum.SkipActive) != 0)
                    {
                        ex.Add("active");
                    }
                }
                return XmlAttr.String(string.Join(",", ex), "");
            }
            set
            {
                string[] exx = value != null ? value.Split(",".ToCharArray()) : new string[] { "" };
                TriggerForceTypeEnum newval = TriggerForceTypeEnum.NoSkip;
                foreach (string ex in exx)
                {
                    if (string.Compare(ex, "true", true) == 0)
                    {
                        newval = TriggerForceTypeEnum.SkipAll;
                        break;
                    }
                    else if (string.Compare(ex, "false", true) == 0)
                    {
                        newval = TriggerForceTypeEnum.NoSkip;
                        break;
                    }
                    if (string.Compare(ex, "regexp", true) == 0)
                    {
                        newval |= TriggerForceTypeEnum.SkipRegexp;
                    }
                    if (string.Compare(ex, "conditions", true) == 0)
                    {
                        newval |= TriggerForceTypeEnum.SkipConditions;
                    }
                    if (string.Compare(ex, "refire", true) == 0)
                    {
                        newval |= TriggerForceTypeEnum.SkipRefire;
                    }
                    if (string.Compare(ex, "parent", true) == 0)
                    {
                        newval |= TriggerForceTypeEnum.SkipParent;
                    }
                    if (string.Compare(ex, "active", true) == 0)
                    {
                        newval |= TriggerForceTypeEnum.SkipActive;
                    }
                }
                _TriggerForceType = newval;
            }
        }

        #endregion
        #region Action specific properties - Window message

        internal string _WmsgProcId = "";
        [XmlAttribute]
        public string WmsgProcId
        {
            get => XmlAttr.String(_WmsgProcId, "", omitWhitespace: false);
            set => _WmsgProcId = value;
        }

        internal string _WmsgTitle = "";
        [XmlAttribute]
        public string WmsgTitle
        {
            get => XmlAttr.String(_WmsgTitle, "", omitWhitespace: false);
            set => _WmsgTitle = value;
        }

        internal string _WmsgCode = "";
        [XmlAttribute]
        public string WmsgCode
        {
            get => XmlAttr.String(_WmsgCode, "", omitWhitespace: false);
            set => _WmsgCode = value;
        }

        internal string _WmsgWparam = "";
        [XmlAttribute]
        public string WmsgWparam
        {
            get => XmlAttr.String(_WmsgWparam, "", omitWhitespace: false);
            set => _WmsgWparam = value;
        }

        internal string _WmsgLparam = "";
        [XmlAttribute]
        public string WmsgLparam
        {
            get => XmlAttr.String(_WmsgLparam, "", omitWhitespace: false);
            set => _WmsgLparam = value;
        }

        #endregion

        #endregion

    }

}
