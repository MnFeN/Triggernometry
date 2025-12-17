using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Triggernometry.Core.Actions;
using Triggernometry.Core.Conditions;
using Triggernometry.Core.Serialization;
using Triggernometry.Expressions.Maths;
using Triggernometry.Localization;
using WMPLib;
using static Triggernometry.Core.RealPlugin;

namespace Triggernometry.Core
{
    [XmlType("Action")]
    public partial class ActionOld
    {

        public class ActionBundle
        {
            [XmlArrayItem("Action")]
            public List<ActionOld> Actions { get; set; } = new List<ActionOld>();

            /// <summary>
            /// Serializes a list of <see cref="ActionOld"/> into an XML string. <br />
            /// Multiple <see cref="ActionOld"/>s are wrapped in a <see cref="ActionBundle"/>; <br />
            /// A single <see cref="ActionOld"/> is serialized as a single <see cref="ActionOld"/>. <br />
            /// Returns an empty string for null or empty input.
            /// </summary>
            internal static string ActionsToXml(List<ActionOld> actions)
            {
                if (actions == null || actions.Count == 0) return string.Empty;

                object toSerialize;
                XmlSerializer xs;

                if (actions.Count > 1)
                {
                    var ab = new ActionBundle();
                    ab.Actions.AddRange(actions);
                    ab.Actions.Sort((a, b) => a.OrderNumber.CompareTo(b.OrderNumber));
                    toSerialize = ab;
                    xs = new XmlSerializer(typeof(ActionBundle));
                }
                else // single Action
                {
                    toSerialize = actions[0];
                    xs = new XmlSerializer(typeof(ActionOld));
                }

                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                using (var sw = new StringWriter())
                {
                    xs.Serialize(sw, toSerialize, ns);
                    return sw.ToString();
                }
            }

            /// <summary>
            /// Deserializes XML containing either an <see cref="ActionBundle"/> or a single <see cref="ActionOld"/>, <br />
            /// and returns a <see cref="List{T}"/> of <see cref="ActionOld"/> in its original order.
            /// </summary>
            internal static List<ActionOld> XmlToActions(string xmlData)
            {
                var result = new List<ActionOld>();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);
                using (var sr = new StringReader(xmlData))
                {
                    if (xmlDoc.DocumentElement.Name == "ActionBundle")
                    {
                        var bundleSerializer = new XmlSerializer(typeof(ActionBundle));
                        var bundle = (ActionBundle)bundleSerializer.Deserialize(sr);
                        if (bundle?.Actions != null)
                            result.AddRange(bundle.Actions);
                    }
                    else // single Action
                    {
                        var actionSerializer = new XmlSerializer(typeof(ActionOld));
                        var a = (ActionOld)actionSerializer.Deserialize(sr);
                        if (a != null)
                            result.Add(a);
                    }
                }
                return result;
            }

        }

        #region General properties

        internal ActionOld NextAction { get; set; } = null;

        /// <summary> 循环的父动作 </summary>
        internal ActionOld LoopAction { get; set; } = null;
        //internal Guid LoopContext { get; set; } = Guid.Empty;

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

        public void ActionContextLogger(object o, string msg)
        {
            AddToLog((Context)o, DebugLevelEnum.Verbose, msg);
        }

        // todo: 子类复用
        private string GetTargetWindowsDescription(string procid, string titleRegex)
        {
            procid = procid.Trim();
            if (string.IsNullOrWhiteSpace(titleRegex))
            {
                titleRegex = ".*";
            }
            int parsedProcId = 1;
            try { parsedProcId = (int)MathParser.Parse(procid); } 
            catch { }
            if (parsedProcId == 0)
            {
                return I18n.Translate("internal/Action/descwindowtargetsingle", "the first window whose title match ({0})", titleRegex);
            }
            else if (parsedProcId < 0)
            {
                return I18n.Translate("internal/Action/descwindowtargetall", "all windows whose titles match ({0})", titleRegex);
            }
            else
            {
                return I18n.Translate("internal/Action/descwindowtargetid", "windows in the process with id ({0}) whose titles match ({1})", procid, titleRegex);
            }
        }

        internal string GetDescription(Context ctx)
            => ConvertToNewAction().Describe();

        internal List<WindowsMediaPlayer> players = new List<WindowsMediaPlayer>();

        public ActionOld()
        {
        }

        private DebugLevelEnum GetDebugLevel(Context ctx)
        {
            if (DebugLevel == DebugLevelEnum.Inherit)
            {
                return ctx?.Trigger?.GetDebugLevel(Instance) ?? DebugLevelEnum.Verbose;
            }
            else
            {
                return DebugLevel;
            }
        }

        internal void AddToLog(Context ctx, DebugLevelEnum level, string message)
        {
            DebugLevelEnum dx = GetDebugLevel(ctx);
            if (level > dx)
            {
                return;
            }
            Instance.UnfilteredAddToLog(level, message, this);
        }

        private void ExecutionImplementation(QueuedAction qa, Context ctx)
        {
            try
            {
                if ((ctx.forceType & TriggerForceTypeEnum.SkipConditions) == 0 && !ctx.testByPlaceholder &&
                    Condition?.Enabled == true && Condition.CheckCondition(ctx, ActionContextLogger, ctx) == false)
                {
                    ctx.PushActionResult(0);
                    AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/actionnotfired", "Action #{0} on trigger '{1}' not fired, condition not met", OrderNumber, ctx.Trigger?.LogName ?? "(null)"));
                }
                else
                {
                    ctx.PushActionResult(1);
                    AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/executingaction", "Executing action '{0}' in thread {1}", GetDescription(ctx), Thread.CurrentThread.ManagedThreadId));

                    if (ActionType == ActionTypeEnum.Loop)
                    {
                        ExecuteLoopAction(ref ctx, qa, out bool shouldReturn); // ctx might be changed here
                        if (shouldReturn) return; // need to check: mutex should be released here?
                    }
                    else
                    {
                        ExecutionCore(qa, ctx);
                    }
                }

                if (LoopAction != null)
                {
                    DateTime dt = DateTime.Now.AddMilliseconds(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, LoopAction._LoopDelayExpression));
                    Instance.QueueAction(ctx, ctx.Trigger, qa?.mutex, LoopAction, dt, false);
                }
                else if (NextAction != null)
                {
                    DateTime dt = DateTime.Now.AddMilliseconds(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, NextAction.ExecutionDelayExpression));
                    Instance.QueueAction(ctx, ctx.Trigger, qa?.mutex, NextAction, dt, false);
                }
                else if (qa?.mutex != null)
                {
                    qa.mutex.Release(ctx);
                    qa.mutex = null;
                }
            }
            catch (Exception ex)
            {
                if (ActionType == ActionTypeEnum.NamedCallback)
                {
                    if (ex is TargetInvocationException tiex && tiex.InnerException != null)
                        ex = tiex.InnerException;
                }
                string triggerPath = qa?.ctx?.Trigger?.FullPath ?? "(null)";
                string actionDesc = "";
                try { actionDesc = GetDescription(ctx); } catch { }
                actionDesc = actionDesc.Length > 300 ? actionDesc.Substring(0, 297) + "..." : actionDesc;
                bool showDetail = true; // _ActionType == ActionTypeEnum.ExecuteScript || _ActionType == ActionTypeEnum.NamedCallback || plug.cfg.DeveloperMode;
                string detail = showDetail ? ex.FullMessage() : ""; // inner and stack
                
                AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/exception",
                    "Action exception: {0}  \nIn action: {1}  \nIn trigger: {2}{3}",
                    ex.Message, actionDesc, triggerPath, detail));
            }
        }

        private void ExecuteLoopAction(ref Context ctx, QueuedAction qa, out bool shouldReturn)
        {
            shouldReturn = false;
            var loopActionIdOld = ctx.loopActionId;

            if (loopActionIdOld == Id)
                ctx.loopIterator += (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, _LoopIncrExpression);

            if (!LoopCondition.Enabled || LoopCondition.CheckCondition(ctx, ActionContextLogger, ctx) != true)
                return;

            bool continuing = false;
            if (loopActionIdOld != Id)
            {
                continuing = loopActionIdOld == Guid.Empty;
                ctx = ctx.Duplicate();
                if (loopActionIdOld != Guid.Empty && loopActionIdOld != Id)
                {
                    ctx.id = Guid.NewGuid();
                }
                ctx.loopActionId = Id;
                ctx.loopIterator = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, _LoopInitExpression);
            }
            else
            {
                continuing = true;
            }
            DateTime curTime = DateTime.Now;
            ActionOld lastAction = Instance.QueueActions(ctx, curTime, LoopActions, ctx.Trigger.Sequential, qa?.mutex, ActionContextLogger);
            lastAction.LoopAction = this;

            shouldReturn = continuing == true;
        }

        private void ExecutionCore(QueuedAction qa, Context ctx)
        {
            var ai = new ActionBase.ActionInstance(
                qa?.when ?? default,
                qa?.ordinal ?? default,
                qa?.mutex,
                this,
                ctx,
                qa?.releaseMutex ?? default);
            var newAction = ConvertToNewAction();
            newAction.ExecuteImplementation(ai);
        }
        
        internal void Mywmp_PlayStateChange(int NewState)
        {
            if ((WMPPlayState)NewState != WMPPlayState.wmppsStopped)
            {
                return;
            }
            WindowsMediaPlayer wmp = null;
            lock (players) // verified
            {
                do
                {
                    wmp = null;
                    foreach (WindowsMediaPlayer x in players)
                    {
                        if (x.playState == WMPPlayState.wmppsStopped)
                        {
                            wmp = x;
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

        internal void Execute(QueuedAction qa, Context ctx)
        {
            if (Asynchronous == true)
            {
                CancellationToken? ct = ctx.Plugin?.GetCancellationToken();
                Task.Run(() =>
                {
                    ct?.ThrowIfCancellationRequested();
                    ExecutionImplementation(qa, ctx);
                    qa?.ActionFinished();
                });
            }
            else
            {
                ExecutionImplementation(qa, ctx);
                qa?.ActionFinished();
            }
        }

        public void CopySettingsTo(ActionOld a)
        {
            a.ParentTrigger = ParentTrigger;
            a.Id = Id;
            a.ActionType = ActionType;
            a.OrderNumber = OrderNumber;
            a.Asynchronous = Asynchronous;
            a.Enabled = Enabled;
            a.Tag = Tag;
            a.ExecutionDelayExpression = ExecutionDelayExpression;
            a._LaunchProcessCmdlineExpression = _LaunchProcessCmdlineExpression;
            a._LaunchProcessPathExpression = _LaunchProcessPathExpression;
            a._LaunchProcessWindowStyle = _LaunchProcessWindowStyle;
            a._LaunchProcessWorkingDirExpression = _LaunchProcessWorkingDirExpression;
            a._PlaySoundExclusive = _PlaySoundExclusive;
            a._PlaySoundFileExpression = _PlaySoundFileExpression;
            a._PlaySoundVolumeExpression = _PlaySoundVolumeExpression;
            a.RefireInterrupt = RefireInterrupt;
            a.RefireRequeue = RefireRequeue;
            a._SystemBeepFreqExpression = _SystemBeepFreqExpression;
            a._SystemBeepLengthExpression = _SystemBeepLengthExpression;
            a._UseTTSExclusive = _UseTTSExclusive;
            a._UseTTSRateExpression = _UseTTSRateExpression;
            a._UseTTSTextExpression = _UseTTSTextExpression;
            a._UseTTSVolumeExpression = _UseTTSVolumeExpression;
            a._ExecScriptAssembliesExpression = _ExecScriptAssembliesExpression;
            a._ExecScriptExpression = _ExecScriptExpression;
            a._MessageBoxIconType = _MessageBoxIconType;
            a._MessageBoxText = _MessageBoxText;
            a._VariableOp = _VariableOp;
            a._VariableName = _VariableName;
            a._VariableJsonTarget = _VariableJsonTarget;
            a.DebugLevel = DebugLevel;
            a._VariableExpression = _VariableExpression;
            a._TriggerId = _TriggerId;
            a._TriggerOp = _TriggerOp;
            a._TriggerText = _TriggerText;
            a._TriggerZone = _TriggerZone;
            a._TriggerTagRegex = _TriggerTagRegex;
            a._TriggerForceType = _TriggerForceType;
            a._AuraOp = _AuraOp;
            a._AuraName = _AuraName;
            a._AuraImage = _AuraImage;
            a._AuraImageMode = _AuraImageMode;
            a._AuraXIniExpression = _AuraXIniExpression;
            a._AuraYIniExpression = _AuraYIniExpression;
            a._AuraWIniExpression = _AuraWIniExpression;
            a._AuraHIniExpression = _AuraHIniExpression;
            a._AuraOIniExpression = _AuraOIniExpression;
            a._AuraXTickExpression = _AuraXTickExpression;
            a._AuraYTickExpression = _AuraYTickExpression;
            a._AuraWTickExpression = _AuraWTickExpression;
            a._AuraHTickExpression = _AuraHTickExpression;
            a._AuraOTickExpression = _AuraOTickExpression;
            a._AuraTTLTickExpression = _AuraTTLTickExpression;
            a._FolderOp = _FolderOp;
            a._FolderId = _FolderId;
            a._DiscordWebhookMessage = _DiscordWebhookMessage;
            a._DiscordWebhookURL = _DiscordWebhookURL;
            a._TextAuraOp = _TextAuraOp;
            a._TextAuraName = _TextAuraName;
            a._TextAuraExpression = _TextAuraExpression;
            a._TextAuraAlignment = _TextAuraAlignment;
            a._TextAuraXIniExpression = _TextAuraXIniExpression;
            a._TextAuraYIniExpression = _TextAuraYIniExpression;
            a._TextAuraWIniExpression = _TextAuraWIniExpression;
            a._TextAuraHIniExpression = _TextAuraHIniExpression;
            a._TextAuraOIniExpression = _TextAuraOIniExpression;
            a._TextAuraXTickExpression = _TextAuraXTickExpression;
            a._TextAuraYTickExpression = _TextAuraYTickExpression;
            a._TextAuraWTickExpression = _TextAuraWTickExpression;
            a._TextAuraHTickExpression = _TextAuraHTickExpression;
            a._TextAuraOTickExpression = _TextAuraOTickExpression;
            a._TextAuraTTLTickExpression = _TextAuraTTLTickExpression;
            a._TextAuraFontName = _TextAuraFontName;
            a._TextAuraFontSize = _TextAuraFontSize;
            a._TextAuraEffect = _TextAuraEffect;
            a._TextAuraOutlineClInt = _TextAuraOutlineClInt;
            a._TextAuraForegroundClInt = _TextAuraForegroundClInt;
            a._TextAuraBackgroundClInt = _TextAuraBackgroundClInt;
            a._LogMessageText = _LogMessageText;
            a._LogLevel = _LogLevel;
            a._DiscordTts = _DiscordTts;
            a._ListVariableExpression = _ListVariableExpression;
            a._ListVariableExpressionType = _ListVariableExpressionType;
            a._ListVariableIndex = _ListVariableIndex;
            a._ListVariableName = _ListVariableName;
            a._ListVariableOp = _ListVariableOp;
            a._ListVariableTarget = _ListVariableTarget;
            a._OBSControlType = _OBSControlType;
            a._OBSEndPoint = _OBSEndPoint;
            a._OBSPassword = _OBSPassword;
            a._OBSSceneName = _OBSSceneName;
            a._OBSSourceName = _OBSSourceName;
            a._OBSJSONPayload = _OBSJSONPayload;
            a._LSControlType = _LSControlType;
            a._LSCustomPayload = _LSCustomPayload;
            a._LogProcess = _LogProcess;
            a._LogProcessACT = _LogProcessACT;
            a._LogMessageTarget = _LogMessageTarget;
            a._JsonOperationType = _JsonOperationType;
            a._JsonCacheRequest = _JsonCacheRequest;
            a._JsonEndpointExpression = _JsonEndpointExpression;
            a._JsonHeaderExpression = _JsonHeaderExpression;
            a._JsonFiringExpression = _JsonFiringExpression;
            a._JsonPayloadExpression = _JsonPayloadExpression;
            a.Condition = (ConditionGroup)Condition?.Duplicate();
            a._KeyPressExpression = _KeyPressExpression;
            a._KeypressType = _KeypressType;
            a._KeyPressCode = _KeyPressCode;
            a._KeyPressWindow = _KeyPressWindow;
            a._KeyPressProcId = _KeyPressProcId;
            a._WmsgProcId = _WmsgProcId;
            a._WmsgCode = _WmsgCode;
            a._WmsgTitle = _WmsgTitle;
            a._WmsgLparam = _WmsgLparam;
            a._WmsgWparam = _WmsgWparam;
            a._DiskFileOp = _DiskFileOp;
            a._DiskFileOpVar = _DiskFileOpVar;
            a._DiskFileOpName = _DiskFileOpName;
            a._TableVariableExpression = _TableVariableExpression;
            a._TableVariableExpressionType = _TableVariableExpressionType;
            a._TableVariableName = _TableVariableName;
            a._TableVariableOp = _TableVariableOp;
            a._TableVariableTarget = _TableVariableTarget;
            a._TableVariableX = _TableVariableX;
            a._TableVariableY = _TableVariableY;
            a._DictVariableName = _DictVariableName;
            a._DictVariableTarget = _DictVariableTarget;
            a._DictSourcePersist = _DictSourcePersist;
            a._DictTargetPersist = _DictTargetPersist;
            a._DictVariableKey = _DictVariableKey;
            a._DictVariableValue = _DictVariableValue;
            a._DictVariableKeyType = _DictVariableKeyType;
            a._DictVariableValueType = _DictVariableValueType;
            a._DictVariableOp = _DictVariableOp;
            a._DictVariableLength = _DictVariableLength;
            a._MutexOpType = _MutexOpType;
            a._MutexName = _MutexName;
            a.Description = Description;
            a.DescriptionOverride = DescriptionOverride;
            a.DescBgColor = DescBgColor;
            a.DescTextColor = DescTextColor;
            a._NamedCallbackParam = _NamedCallbackParam;
            a._NamedCallbackName = _NamedCallbackName;
            a._MouseOpType = _MouseOpType;
            a._MouseCoordType = _MouseCoordType;
            a._MouseX = _MouseX;
            a._MouseY = _MouseY;
            a._ListSourcePersist = _ListSourcePersist;
            a._ListTargetPersist = _ListTargetPersist;
            a._TableSourcePersist = _TableSourcePersist;
            a._TableTargetPersist = _TableTargetPersist;
            a._DiskPersist = _DiskPersist;
            a._VariablePersist = _VariablePersist;
            a._VariableTargetPersist = _VariableTargetPersist;
            a.LoopCondition = (ConditionGroup)LoopCondition?.Duplicate();
            a.LoopActions.Clear();
            foreach (var loopAction in LoopActions.OrderBy(x => x.OrderNumber))
            {
                var copy = new ActionOld();
                loopAction.CopySettingsTo(copy);
                a.LoopActions.Add(copy);
            }
            a._LoopDelayExpression = _LoopDelayExpression;
            a._LoopIncrExpression = _LoopIncrExpression;
            a._LoopInitExpression = _LoopInitExpression;
            a._ActOpBoolParam = _ActOpBoolParam;
            a._ActOpStringParam = _ActOpStringParam;
            a._ActOpType = _ActOpType;
            a._RepositoryId = _RepositoryId;
            a._RepositoryOp = _RepositoryOp;
            a._JsonResultVariable = _JsonResultVariable;
            a._JsonResultVariablePersist = _JsonResultVariablePersist;
            a._TriggerZoneType = _TriggerZoneType;
            a._SoundRouting = _SoundRouting;
            a._TTSRouting = _TTSRouting;
        }

        /// <summary> Set text to the clipboard using the UI thread. If text is empty, clear the clipboard. </summary>
        public static void ClipboardSetText(string text)
        {
            Instance.ui.Invoke(new System.Action(() =>
            {
                if (string.IsNullOrEmpty(text))
                    Clipboard.Clear();
                else
                    Clipboard.SetText(text);
            }));
        }

        /// <summary> Get the clipboard text using the UI thread. </summary>
        public static string ClipboardGetText()
            => (string)Instance.ui.Invoke(new Func<string>(Clipboard.GetText));

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

        public ActionOld Copy()
        {
            var a = new ActionOld();
            CopySettingsTo(a);
            return a;
        }

        internal void CopyCommonPropertiesTo(ActionBase action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            action.Enabled = Enabled;
            action.Id = Id;
            action.ParentTrigger = ParentTrigger;
            action.OrderNumber = OrderNumber;
            action.Condition = (ConditionGroup)Condition?.Duplicate();
            action.Tag = Tag;
            action.RefireInterrupt = RefireInterrupt;
            action.RefireRequeue = RefireRequeue;
            action.ExecutionDelayExpression = ExecutionDelayExpression;
            action.Asynchronous = Asynchronous;
            action.DebugLevel = DebugLevel;
            action.Description = Description;
            action.DescBgColor = DescBgColor;
            action.DescTextColor = DescTextColor;
            action.DescriptionOverride = DescriptionOverride;
        }

        internal ActionBase ConvertToNewAction()
        {
            switch (ActionType)
            {
                case ActionTypeEnum.ActInteraction:
                    return (ActionActInteraction)this;
                case ActionTypeEnum.SystemBeep:
                    return (ActionBeep)this;
                case ActionTypeEnum.DiscordWebhook:
                    return (ActionDiscordWebhook)this;
                case ActionTypeEnum.DiskFile:
                    return (ActionDiskOperation)this;
                case ActionTypeEnum.ExecuteScript:
                    return (ActionExecuteScript)this;
                case ActionTypeEnum.Folder:
                    return (ActionFolderOperation)this;
                case ActionTypeEnum.GenericJson:
                    return (ActionJsonRequest)this;
                case ActionTypeEnum.KeyPress:
                    return (ActionKeypress)this;
                case ActionTypeEnum.LaunchProcess:
                    return (ActionLaunchProcess)this;
                case ActionTypeEnum.LiveSplitControl:
                    return (ActionLiveSplitControl)this;
                case ActionTypeEnum.LogMessage:
                    return (ActionLogMessage)this;
                case ActionTypeEnum.Loop:
                    return (ActionLoop)this;
                case ActionTypeEnum.MessageBox:
                    return (ActionMessageBox)this;
                case ActionTypeEnum.Mouse:
                    return (ActionMouse)this;
                case ActionTypeEnum.Mutex:
                    return (ActionMutex)this;
                case ActionTypeEnum.NamedCallback:
                    return (ActionNamedCallback)this;
                case ActionTypeEnum.ObsControl:
                    return (ActionObsControl)this;
                case ActionTypeEnum.Aura:
                    return (ActionOverlayImage)this;
                case ActionTypeEnum.TextAura:
                    return (ActionOverlayText)this;
                case ActionTypeEnum.Placeholder:
                    return (ActionPlaceholder)this;
                case ActionTypeEnum.PlaySound:
                    return (ActionPlaySound)this;
                case ActionTypeEnum.UseTTS:
                    return (ActionPlaySpeech)this;
                case ActionTypeEnum.Repository:
                    return (ActionRepository)this;
                case ActionTypeEnum.Trigger:
                    return (ActionTriggerOperation)this;
                case ActionTypeEnum.DictVariable:
                    return (ActionVariableDict)this;
                case ActionTypeEnum.ListVariable:
                    return (ActionVariableList)this;
                case ActionTypeEnum.Variable:
                    return (ActionVariableScalar)this;
                case ActionTypeEnum.TableVariable:
                    return (ActionVariableTable)this;
                case ActionTypeEnum.WindowMessage:
                    return (ActionWindowMessage)this;

                default:
                    throw new NotSupportedException($"Unknown ActionType: {ActionType}");
            }
        }

    }

}
