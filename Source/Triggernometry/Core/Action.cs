using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Triggernometry.Common.Audio;
using Triggernometry.Core.Actions;
using Triggernometry.Core.Conditions;
using Triggernometry.Core.Serialization;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Parsers;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;
using Triggernometry.PluginBridges.ExternalTools;
using Triggernometry.Utilities;
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

        private static readonly CultureInfo InvClt = CultureInfo.InvariantCulture;
        private static readonly NumberStyles NSFloat = NumberStyles.Float;

        /// <returns>
        /// <c>true</c> : connected <br /> 
        /// <c>false</c>: failed <br /> 
        /// <c>null</c> : not running
        /// </returns>
        internal bool? ObsConnector(Context ctx, string endpoint, string password)
        {
            lock (Instance._obs)
            {
                if (Instance._obs.IsConnected == true)
                {
                    return true;
                }
                var state = Instance._obs.CheckRunningState();
                if (state != ObsController.ObsRunningState.Running)
                { 
                    if (state == ObsController.ObsRunningState.NotRunningFirstlyFound)
                        AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/obsnotrunning",
                            "OBS is not running and the OBS action cannot be performed."));
                    return null;
                }
                try
                {
                    Instance._obs.Connect(endpoint, password);
                    AddToLog(ctx, DebugLevelEnum.Info, I18n.Translate("internal/Action/obsconnectok", 
                        "OBS WebSocket connected successfully"));
                    return true;
                }
                catch (Exception ex)
                {
                    AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/obsconnecterror", 
                        "Error connecting to OBS WebSocket: {0}", ex.Message));
                    return false;
                }
            }
        }

        internal bool LiveSplitConnector(Context ctx)
        {
            lock (Instance._livesplit)
            {
                if (Instance._livesplit.IsConnected == true)
                {
                    return true;
                }
                try
                {
                    Instance._livesplit.Connect();
                    AddToLog(ctx, DebugLevelEnum.Info, I18n.Translate("internal/Action/lsconnectok", "LiveSplit connected successfully"));
                    return true;
                }
                catch (Exception ex)
                {
                    AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/lsconnecterror", "Error connecting to LiveSplit: {0}", ex.Message));
                }
            }
            return false;
        }

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

        private ActionBase.ActionInstance ConvertActionInstanceTemp(QueuedAction qa, Context ctx)
            => new ActionBase.ActionInstance(
                qa?.when ?? default, 
                qa?.ordinal ?? default, 
                qa?.mutex, 
                this, 
                ctx, 
                qa?.releaseMutex ?? default);

        private void ExecutionCore(QueuedAction qa, Context ctx)
        {
            switch (ActionType)
            {
                #region Implementation - ACT Interaction
                case ActionTypeEnum.ActInteraction:
                    {
                        switch (_ActOpType)
                        {
                            case ActInteractionTypeEnum.SetCombatState:
                                Instance.SetCombatStateHook(_ActOpBoolParam);
                                break;
                            case ActInteractionTypeEnum.LogAllNetwork:
                                PluginBridges.BridgeFFXIV.LogAllNetwork(_ActOpBoolParam);
                                break;
                            case ActInteractionTypeEnum.UseDeucalion:
                                PluginBridges.BridgeFFXIV.UseDeucalion(_ActOpBoolParam);
                                break;
                        }
                    }
                    break;
                #endregion
                #region Implementation - Beep
                case ActionTypeEnum.SystemBeep:
                    {
                        var ai = ConvertActionInstanceTemp(qa, ctx);
                        var newAction = (ActionBeep)this;
                        newAction.ExecuteImplementation(ai);
                    }
                    break;
                #endregion
                #region Implementation - Dict variable
                case ActionTypeEnum.DictVariable:
                    {
                        var ai = ConvertActionInstanceTemp(qa, ctx);
                        var newAction = (ActionVariableDict)this;
                        newAction.ExecuteImplementation(ai);
                    }
                    break;
                #endregion
                #region Implementation - Discord webhook
                case ActionTypeEnum.DiscordWebhook:
                    {
                        string msg = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _DiscordWebhookMessage);
                        string url = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _DiscordWebhookURL);
                        if (_DiscordTts == true)
                        {
                            if (msg.Length > 1970)
                            {
                                msg = msg.Substring(0, 1970);
                                AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/warndiscordtrunc", "Discord message too long, capping to {0}", msg.Length));
                            }
                            var wh = new JavaScriptSerializer().Serialize(new { content = msg, tts = true });
                            SendJson(ctx, HTTPMethodEnum.POST, url, wh, null, true);
                        }
                        else
                        {
                            if (msg.Length > 1980)
                            {
                                msg = msg.Substring(0, 1980);
                                AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/warndiscordtrunc", "Discord message too long, capping to {0}", msg.Length));
                            }
                            var wh = new JavaScriptSerializer().Serialize(new { content = msg });
                            SendJson(ctx, HTTPMethodEnum.POST, url, wh, null, true);
                        }
                    }
                    break;
                #endregion
                #region Implementation - Disk operation
                case ActionTypeEnum.DiskFile:
                    {
                        var ai = ConvertActionInstanceTemp(qa, ctx);
                        var newAction = (ActionDiskOperation)this;
                        newAction.ExecuteImplementation(ai);
                    }
                    break;
                #endregion
                #region Implementation - Execute script
                case ActionTypeEnum.ExecuteScript:
                    {
                        string scp = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _ExecScriptExpression);
                        string assy = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _ExecScriptAssembliesExpression);
                        while (Instance.scriptingInited == false)
                        {
                            Thread.Sleep(10);
                        }
                        if (Instance?.scripting.Ready == true)
                        {
                            Instance.scripting.Evaluate(scp, assy, ctx);
                        }
                        else
                        {
                            AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/scriptinifailed", "Action #{0} on trigger '{1}' not fired, scripting not available", OrderNumber, ctx.Trigger?.LogName ?? "(null)"));
                        }
                    }
                    break;
                #endregion
                #region Implementation - Folder operation
                case ActionTypeEnum.Folder:
                    {
                        Folder f = Instance.GetFolderById(_FolderId, ctx.Trigger?.Repo);
                        if (f != null)
                        {
                            switch (_FolderOp)
                            {
                                case FolderOpEnum.DisableFolder:
                                    {
                                        f.Enabled = false;

                                        Instance.ui.Invoke((System.Action)(() =>
                                        {
                                            bool isLocal = ctx.Trigger?.Repo == null;
                                            TreeNode tn = Instance.LocateNodeHostingFolder(Instance.ui.treeView1.Nodes[isLocal ? 0 : 1], f);

                                            if (tn != null)
                                            {
                                                tn.Checked = false;
                                            }
                                            else
                                            {
                                                AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/notreenodefolderwithid", "Didn't find a tree node for folder ({0}) with id ({1})", f.Name, f.Id));
                                            }
                                            AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/disabledfolderwithid", "Disabled folder ({0}) with id ({1})", f.Name, f.Id));
                                        }));
                                    }
                                    break;
                                case FolderOpEnum.EnableFolder:
                                    {
                                        f.Enabled = true;

                                        Instance.ui.Invoke((System.Action)(() =>
                                        {
                                            bool isLocal = ctx.Trigger?.Repo == null;
                                            TreeNode tn = Instance.LocateNodeHostingFolder(Instance.ui.treeView1.Nodes[isLocal ? 0 : 1], f);

                                            if (tn != null)
                                            {
                                                tn.Checked = true;
                                            }
                                            else
                                            {
                                                AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/notreenodefolderwithid", "Didn't find a tree node for folder ({0}) with id ({1})", f.Name, f.Id));
                                            }
                                            AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/enabledfolderwithid", "Enabled folder ({0}) with id ({1})", f.Name, f.Id));
                                        }));
                                    }
                                    break;
                                case FolderOpEnum.CancelFolder:
                                    {
                                        var triggersInFolder = new HashSet<Trigger>(f.RecursiveGetTriggers());
                                        int removed = Instance.CancelQueuedActions(
                                            _qa => _qa?.ctx?.Trigger != null && triggersInFolder.Contains(_qa.ctx.Trigger)
                                        );
                                        AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/cancelfolder",
                                            "Cancelled {1} queued action(s) from {2} triggers in folder ({0})",
                                            f.Name, removed, triggersInFolder.Count));
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/nofolderwithid",
                                "Folder operation failed: In trigger ({1}), the specified folder id ({0}) does not exist.", _FolderId, ParentTrigger?.FullPath ?? "null"));
                        }
                    }
                    break;
                #endregion
                #region Implementation - Image aura
                case ActionTypeEnum.Aura:
                    {
                        var ai = ConvertActionInstanceTemp(qa, ctx);
                        var newAction = (ActionOverlayImage)this;
                        newAction.ExecuteImplementation(ai);
                    }
                    break;
                #endregion
                #region Implementation - JSON
                case ActionTypeEnum.GenericJson:
                    {
                        var ai = ConvertActionInstanceTemp(qa, ctx);
                        var newAction = (ActionJsonRequest)this;
                        newAction.ExecuteImplementation(ai);
                    }
                    break;
                #endregion
                #region Implementation - Keypress
                case ActionTypeEnum.KeyPress:
                    {
                        switch (_KeypressType)
                        {
                            case KeypressTypeEnum.SendKeys:
                                {
                                    string ks = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _KeyPressExpression);
                                    SendKeys.SendWait(ks);
                                }
                                break;
                            case KeypressTypeEnum.WindowMessage:
                                {
                                    int procid = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, _KeyPressProcId);
                                    string window = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _KeyPressWindow);
                                    int keycode = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, _KeyPressCode);
                                    WindowsUtils.SendKeycode(procid, window, keycode);
                                }
                                break;
                            case KeypressTypeEnum.WindowMessageCombo:
                                {
                                    int procid = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, _KeyPressProcId);
                                    string window = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _KeyPressWindow);
                                    int[] keycodes = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _KeyPressCode)
                                        .Split(',').Select(kx => Convert.ToInt32(kx.Trim())).ToArray();
                                    WindowsUtils.SendKeycodes(procid, window, keycodes);
                                }
                                break;
                        }
                    }
                    break;
                #endregion
                #region Implementation - Launch process
                case ActionTypeEnum.LaunchProcess:
                    {
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                        psi.Arguments = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _LaunchProcessCmdlineExpression);
                        psi.WindowStyle = _LaunchProcessWindowStyle;
                        psi.WorkingDirectory = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _LaunchProcessWorkingDirExpression);
                        psi.FileName = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _LaunchProcessPathExpression);
                        p.StartInfo = psi;
                        p.Start();
                        if (Asynchronous == false)
                        {
                            AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/waitingprocexit", "Waiting for process to exit"));
                            p.WaitForExit();
                        }
                    }
                    break;
                #endregion
                #region Implementation - List variable
                case ActionTypeEnum.ListVariable:
                    {
                        var ai = ConvertActionInstanceTemp(qa, ctx);
                        var newAction = (ActionVariableList)this;
                        newAction.ExecuteImplementation(ai);
                    }
                    break;
                #endregion
                #region Implementation - Log message
                case ActionTypeEnum.LogMessage:
                    {
                        string message = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _LogMessageText);

                        if (_LogProcess)
                        {
                            string zone = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Instance.currentZone);
                            Instance.LogLineQueuer(message, zone, _LogMessageTarget);
                        }
                        else
                        {
                            DebugLevelEnum debugLevel = DebugLevelEnum.Error;
                            switch (_LogLevel)
                            {
                                case LogMessageEnum.Custom: debugLevel = DebugLevelEnum.Custom; break;
                                case LogMessageEnum.Custom2: debugLevel = DebugLevelEnum.Custom2; break;
                                case LogMessageEnum.Error: debugLevel = DebugLevelEnum.Error; break;
                                case LogMessageEnum.Info: debugLevel = DebugLevelEnum.Info; break;
                                case LogMessageEnum.Verbose: debugLevel = DebugLevelEnum.Verbose; break;
                                case LogMessageEnum.Warning: debugLevel = DebugLevelEnum.Warning; break;
                            }
                            AddToLog(ctx, debugLevel, message);
                        }
                        if (_LogProcessACT)
                        {
                            Instance.ACTEncounterLogHook(message);
                        }
                    }
                    break;
                #endregion
                #region Implementation - Message box
                case ActionTypeEnum.MessageBox:
                    {
                        Form activeForm = Form.ActiveForm;
                        if (activeForm != null)
                        {
                            MessageBox.Show(activeForm, ctx.EvaluateStringExpression(ActionContextLogger, ctx, _MessageBoxText), "", MessageBoxButtons.OK, (MessageBoxIcon)_MessageBoxIconType);
                        }
                        else
                        {
                            MessageBox.Show(ctx.EvaluateStringExpression(ActionContextLogger, ctx, _MessageBoxText), "", MessageBoxButtons.OK, (MessageBoxIcon)_MessageBoxIconType);
                        }
                    }
                    break;
                #endregion
                #region Implementation - Mutex
                case ActionTypeEnum.Mutex:
                    {
                        string mn = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _MutexName);
                        switch (_MutexOpType)
                        {
                            case MutexOpEnum.Acquire:
                                {
                                    MutexInformation mi = Instance.GetMutex(mn);
                                    mi.Acquire(ctx);
                                }
                                break;
                            case MutexOpEnum.Release:
                                {
                                    MutexInformation mi = Instance.GetMutex(mn);
                                    mi.Release(ctx);
                                }
                                break;
                        }
                    }
                    break;
                #endregion
                #region Implementation - OBS
                case ActionTypeEnum.ObsControl:
                    ObsController obsController = Instance._obs;
                    if (obsController != null)
                    {
                        string endpoint = "";
                        if (!string.IsNullOrWhiteSpace(_OBSEndPoint))
                        {
                            endpoint = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _OBSEndPoint);
                        }
                        else
                        {
                            var constants = Instance.cfg.Constants;
                            if (constants.TryGetValue("OBSWebsocketEndpoint", out var e) && constants.TryGetValue("OBSWebsocketPort", out var p))
                                endpoint = $"ws://{e}:{p}";
                        }

                        string password = "";
                        if (!string.IsNullOrWhiteSpace(_OBSPassword))
                        {
                            password = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _OBSPassword);
                        }
                        else
                        {
                            var constants = Instance.cfg.Constants;
                            if (constants.TryGetValue("OBSWebsocketPassword", out var pw))
                                password = pw.ToString();
                        }

                        lock (obsController)
                        {
                            if (ObsConnector(ctx, endpoint, password) != true)
                                return; // already complaint about errors
                            try
                            {
                                switch (_OBSControlType)
                                {
                                    case ObsControlTypeEnum.StartStreaming:
                                        obsController.StartStreaming();
                                        break;
                                    case ObsControlTypeEnum.StopStreaming:
                                        obsController.StopStreaming();
                                        break;
                                    case ObsControlTypeEnum.ToggleStreaming:
                                        obsController.ToggleStreaming();
                                        break;
                                    case ObsControlTypeEnum.StartRecording:
                                        obsController.StartRecording();
                                        break;
                                    case ObsControlTypeEnum.StopRecording:
                                        obsController.StopRecording();
                                        break;
                                    case ObsControlTypeEnum.ToggleRecording:
                                        obsController.ToggleRecording();
                                        break;
                                    case ObsControlTypeEnum.RestartRecording:
                                        obsController.RestartRecording();
                                        break;
                                    case ObsControlTypeEnum.RestartRecordingIfActive:
                                        obsController.RestartRecordingIfActive();
                                        break;
                                    case ObsControlTypeEnum.ResumeRecording:
                                        obsController.ResumeRecording();
                                        break;
                                    case ObsControlTypeEnum.PauseRecording:
                                        obsController.PauseRecording();
                                        break;
                                    case ObsControlTypeEnum.ToggleRecordPause:
                                        obsController.ToggleRecordPause();
                                        break;
                                    case ObsControlTypeEnum.StartReplayBuffer:
                                        obsController.StartReplayBuffer();
                                        break;
                                    case ObsControlTypeEnum.StopReplayBuffer:
                                        obsController.StopReplayBuffer();
                                        break;
                                    case ObsControlTypeEnum.ToggleReplayBuffer:
                                        obsController.ToggleReplayBuffer();
                                        break;
                                    case ObsControlTypeEnum.SaveReplayBuffer:
                                        obsController.SaveReplayBuffer();
                                        break;
                                    case ObsControlTypeEnum.SetScene:
                                        {
                                            string scn = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _OBSSceneName);
                                            obsController.SetCurrentScene(scn);
                                        }
                                        break;
                                    case ObsControlTypeEnum.ShowSource:
                                        {
                                            string scn = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _OBSSceneName);
                                            string src = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _OBSSourceName);
                                            obsController.ShowHideSource(scn, src, true);
                                        }
                                        break;
                                    case ObsControlTypeEnum.HideSource:
                                        {
                                            string scn = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _OBSSceneName);
                                            string src = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _OBSSourceName);
                                            obsController.ShowHideSource(scn, src, false);
                                        }
                                        break;
                                    case ObsControlTypeEnum.JSONPayload:
                                        {
                                            string json = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _OBSJSONPayload);
                                            obsController.JSONPayload(json);
                                        }
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/obscontrolexception", "Can't execute OBS control action due to exception: {0}" + ex.Message));
                            }
                        }
                    }
                    break;
                #endregion
                #region Implementation - LiveSplit
                case ActionTypeEnum.LiveSplitControl:
                    LiveSplitController livesplitController = Instance._livesplit;
                    if (livesplitController != null)
                    {
                        lock (livesplitController)
                        {
                            if (LiveSplitConnector(ctx) == true)
                            {
                                try
                                {
                                    switch (_LSControlType)
                                    {
                                        case LiveSplitControlTypeEnum.StartOrSplit:
                                            livesplitController.StartOrSplit();
                                            break;
                                        case LiveSplitControlTypeEnum.Start:
                                            livesplitController.Start();
                                            break;
                                        case LiveSplitControlTypeEnum.Split:
                                            livesplitController.Split();
                                            break;
                                        case LiveSplitControlTypeEnum.UndoSplit:
                                            livesplitController.UndoSplit();
                                            break;
                                        case LiveSplitControlTypeEnum.SkipSplit:
                                            livesplitController.SkipSplit();
                                            break;
                                        case LiveSplitControlTypeEnum.Reset:
                                            livesplitController.Reset();
                                            break;
                                        case LiveSplitControlTypeEnum.Pause:
                                            livesplitController.Pause();
                                            break;
                                        case LiveSplitControlTypeEnum.Resume:
                                            livesplitController.Resume();
                                            break;
                                        case LiveSplitControlTypeEnum.CustomPayload:
                                            string lscommand = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _LSCustomPayload);
                                            livesplitController.SendCommand(lscommand);
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/lscontrolexception", "Can't execute LiveSplit control action due to exception: " + ex.Message));
                                }
                            }
                            else
                            {
                                AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/lscontrolerror", "Can't execute LiveSplit control action due to error"));
                            }
                        }
                    }
                    break;
                #endregion
                #region Implementation - Play sound
                case ActionTypeEnum.PlaySound:
                    {
                        var ai = ConvertActionInstanceTemp(qa, ctx);
                        var newAction = (ActionPlaySound)this;
                        newAction.ExecuteImplementation(ai);
                    }
                    break;
                #endregion
                #region Implementation - Placeholder
                case ActionTypeEnum.Placeholder:
                    break;
                #endregion
                #region Implementation - Play speech
                case ActionTypeEnum.UseTTS:
                    {
                        var ai = ConvertActionInstanceTemp(qa, ctx);
                        var newAction = (ActionPlaySpeech)this;
                        newAction.ExecuteImplementation(ai);
                    }
                    break;
                #endregion
                #region Implementation - Repository
                case ActionTypeEnum.Repository:
                    {
                        Repository r = null;
                        switch (_RepositoryOp)
                        {
                            case RepositoryOpEnum.UpdateSelf:
                                r = ctx.Trigger?.Repo;
                                break;
                            case RepositoryOpEnum.UpdateRepo:
                                r = Instance.GetRepositoryById(_RepositoryId);
                                break;
                            case RepositoryOpEnum.UpdateAll:
                                _ = Instance.UpdateAllRepositoriesAsync(false);
                                break;
                        }
                        if (r != null)
                        {
                            _ = Instance.UpdateSingleRepositoryAsync(r);
                        }
                    }
                    break;
                #endregion
                #region Implementation - Scalar variable
                case ActionTypeEnum.Variable:
                    {
                        var ai = ConvertActionInstanceTemp(qa, ctx);
                        var newAction = (ActionVariableScalar)this;
                        newAction.ExecuteImplementation(ai);
                    }
                    break;
                #endregion
                #region Implementation - Table variable
                case ActionTypeEnum.TableVariable:
                    {
                        var ai = ConvertActionInstanceTemp(qa, ctx);
                        var newAction = (ActionVariableTable)this;
                        newAction.ExecuteImplementation(ai);
                    }
                    break;
                #endregion
                #region Implementation - Text aura
                case ActionTypeEnum.TextAura:
                    {
                        var ai = ConvertActionInstanceTemp(qa, ctx);
                        var newAction = (ActionOverlayText)this;
                        newAction.ExecuteImplementation(ai);
                    }
                    break;
                #endregion
                #region Implementation - Trigger operation
                case ActionTypeEnum.Trigger:
                    {
                        Trigger t = Instance.GetTriggerById(_TriggerId, ctx.Trigger?.Repo);
                        if (t == null && _TriggerOp != TriggerOpEnum.CancelAllTrigger) // 优化：cancel all 时不要查询 t
                        {
                            AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/notriggerwithid",
                                    "Trigger operation failed: In trigger ({1}), the specified trigger id ({0}) does not exist.", _TriggerId, ParentTrigger?.FullPath ?? "null"));
                            break;
                        }
                        switch (_TriggerOp)
                        {
                            case TriggerOpEnum.CancelAllTrigger:
                                {
                                    // Specified Tag Regex
                                    if (!string.IsNullOrWhiteSpace(_TriggerTagRegex))
                                    {
                                        var tag = ctx.EvaluateStringExpression(ActionContextLogger, null, _TriggerTagRegex);
                                        var regex = new Regex(tag);
                                        var removedCount = Instance.CancelQueuedActions(
                                            _qa => regex.IsMatch(_qa?.ParsedTag ?? "")
                                        );
                                        Instance.UnfilteredAddToLog(DebugLevelEnum.Info, I18n.Translate(
                                            "internal/Action/trigcanceltag",
                                            "{0} queued action(s) with tags matching '{1}' cancelled",
                                            removedCount, tag));
                                    }
                                    else
                                    {
                                        var removedCount = Instance.CancelQueuedActions();
                                        Instance.UnfilteredAddToLog(DebugLevelEnum.Info, I18n.Translate(
                                            "internal/Action/trigcancelall",
                                            "All {0} queued action(s) cancelled",
                                            removedCount));
                                    }
                                }
                                break;
                            case TriggerOpEnum.CancelTrigger:
                                {
                                    bool trigFilter(QueuedAction _qa) => _qa?.ctx?.Trigger == t;

                                    if (!string.IsNullOrWhiteSpace(_TriggerTagRegex))
                                    {
                                        var tag = ctx.EvaluateStringExpression(ActionContextLogger, null, _TriggerTagRegex);
                                        var regex = new Regex(tag);
                                        var removedCount = Instance.CancelQueuedActions(
                                            _qa => trigFilter(_qa) && regex.IsMatch(_qa?.ParsedTag ?? "")
                                        );
                                        Instance.UnfilteredAddToLog(DebugLevelEnum.Info, I18n.Translate(
                                            "internal/Action/trigcanceltrigtag",
                                            "{0} queued action(s) from trigger '{1}' with tags matching '{2}' cancelled",
                                            removedCount, t.LogName, tag));
                                    }
                                    else
                                    {
                                        var removedCount = Instance.CancelQueuedActions(queuedAction => trigFilter(queuedAction));
                                        Instance.UnfilteredAddToLog(DebugLevelEnum.Info, I18n.Translate(
                                            "internal/Action/trigcanceltrig",
                                            "{0} queued action(s) from trigger '{1}' cancelled",
                                            removedCount, t.LogName));
                                    }
                                }
                                break;
                            case TriggerOpEnum.FireTrigger:
                                {
                                    LogEvent le = new LogEvent();
                                    le.Text = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _TriggerText);
                                    le.ZoneName = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _TriggerZone);
                                    if (_TriggerZoneType == TriggerZoneTypeEnum.ZoneIdFFXIV && le.ZoneName.Trim().Length > 0)
                                    {
                                        le.ZoneId = le.ZoneName;
                                    }
                                    le.Timestamp = DateTime.Now;
                                    if (ctx.zoneIdOverride != null)
                                    {
                                        le.TestMode = true;
                                        le.ZoneId = ctx.zoneIdOverride;
                                    }
                                    Instance.TestTrigger(t, le, _TriggerForceType);
                                }
                                break;
                            case TriggerOpEnum.EnableTrigger:
                                {
                                    t.Enabled = true;

                                    Instance.ui.Invoke((System.Action)(() =>
                                    {
                                        bool isLocal = ctx.Trigger == null || ctx.Trigger.Repo == null;
                                        TreeNode tn = Instance.LocateNodeHostingTrigger(Instance.ui.treeView1.Nodes[isLocal ? 0 : 1], t);

                                        if (tn != null)
                                        {
                                            AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/trigenable", "Trigger '{0}' enabled", t.LogName));
                                            tn.Checked = true;
                                        }
                                        else
                                        {
                                            AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/notreenodetrigenable", "Could not find tree node to modify for enabling trigger {0}", t.LogName));
                                        }
                                    }));
                                }
                                break;
                            case TriggerOpEnum.DisableTrigger:
                                {
                                    t.Enabled = false;

                                    Instance.ui.Invoke((System.Action)(() =>
                                    {
                                        bool isLocal = ctx.Trigger == null || ctx.Trigger.Repo == null;
                                        TreeNode tn = Instance.LocateNodeHostingTrigger(Instance.ui.treeView1.Nodes[isLocal ? 0 : 1], t);

                                        if (tn != null)
                                        {
                                            AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/trigdisable", "Trigger '{0}' disabled", t.LogName));
                                            tn.Checked = false;
                                        }
                                        else
                                        {
                                            AddToLog(ctx, DebugLevelEnum.Warning, I18n.Translate("internal/Action/notreenodetrigdisable", "Could not find tree node to modify for disabling trigger {0}", t.LogName));
                                        }
                                    }));
                                }
                                break;
                        }
                    }
                    break;
                #endregion
                #region Implementation - Window message
                case ActionTypeEnum.WindowMessage:
                    {
                        int procid = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, _WmsgProcId);
                        string window = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _WmsgTitle);
                        int code = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, _WmsgCode);
                        IntPtr wparam = (IntPtr)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, _WmsgWparam);
                        IntPtr lparam = (IntPtr)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, _WmsgLparam);
                        WindowsUtils.SendMessageToWindow(procid, window, (ushort)code, wparam, lparam);
                    }
                    break;
                #endregion
                #region Implementation - Mouse
                case ActionTypeEnum.Mouse:
                    {
                        int mousex = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, _MouseX);
                        int mousey = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, _MouseY);
                        WindowsUtils.MouseEventFlags flags = 0;
                        switch (_MouseCoordType)
                        {
                            case MouseCoordEnum.Absolute:
                                flags |= WindowsUtils.MouseEventFlags.ABSOLUTE;
                                break;
                            case MouseCoordEnum.Relative:
                                break;
                        }
                        switch (_MouseOpType)
                        {
                            case MouseOpEnum.Move:
                                WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MOVE, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                                break;
                            case MouseOpEnum.LeftClick:
                                Task.Run(() =>
                                {
                                    WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MOVE, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                                    Thread.Sleep(10);
                                    WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.LEFTDOWN, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                                    Thread.Sleep(10);
                                    WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.LEFTUP, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                                });
                                break;
                            case MouseOpEnum.MiddleClick:
                                Task.Run(() =>
                                {
                                    WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MOVE, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                                    Thread.Sleep(10);
                                    WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MIDDLEDOWN, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                                    Thread.Sleep(10);
                                    WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MIDDLEUP, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                                });
                                break;
                            case MouseOpEnum.RightClick:
                                Task.Run(() =>
                                {
                                    WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.MOVE, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                                    Thread.Sleep(10);
                                    WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.RIGHTDOWN, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                                    Thread.Sleep(10);
                                    WindowsUtils.SendMouse(flags | WindowsUtils.MouseEventFlags.RIGHTUP, WindowsUtils.MouseEventDataXButtons.NONE, mousex, mousey);
                                });
                                break;
                        }
                    }
                    break;
                #endregion
                #region Implementation - Named callback
                case ActionTypeEnum.NamedCallback:
                    {
                        string cbname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _NamedCallbackName);
                        string cbparm = ctx.EvaluateStringExpression(ActionContextLogger, ctx, _NamedCallbackParam);
                        AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/callbackinvoke", "Invoking named callback ({0}) with parameter ({1})", cbname, cbparm));
                        Instance.InvokeNamedCallback(cbname, cbparm);
                    }
                    break;
                #endregion
                #region Implementation - Loop
                case ActionTypeEnum.Loop:
                    throw new Exception("Loop actions are not handled here");
                    #endregion
            }
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

        private Tuple<int, string> SendJson(Context ctx, ActionOld.HTTPMethodEnum method, string url, string json, IEnumerable<string> headers, bool expectNoContent)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                if (headers != null && headers.Count() > 0)
                {
                    foreach (string hdr in headers)
                    {
                        var sepIndex = hdr.IndexOf(':');
                        if (sepIndex > 0)
                        {
                            var key = hdr.Substring(0, sepIndex).Trim();
                            var value = hdr.Substring(sepIndex + 1).Trim();
                            switch (key.ToLower())
                            {
                                case "content-type":
                                    httpWebRequest.ContentType = value;
                                    break;
                                case "user-agent":
                                    httpWebRequest.UserAgent = value;
                                    break;
                                case "accept":
                                    httpWebRequest.Accept = value;
                                    break;
                                case "referer":
                                    httpWebRequest.Referer = value;
                                    break;
                                case "host":
                                    httpWebRequest.Host = value;
                                    break;
                                default:
                                    httpWebRequest.Headers.Add(key, value);
                                    break;
                            }
                        }
                    }
                }
                switch (method)
                {
                    case HTTPMethodEnum.POST:
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        break;
                    case HTTPMethodEnum.GET:
                        httpWebRequest.Method = "GET";
                        break;
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode != HttpStatusCode.NoContent && expectNoContent == true)
                {
                    AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/jsonpostunexpectedresponse", "Unexpected response code: {0}", httpResponse.StatusCode));
                }
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return new Tuple<int, string>((int)httpResponse.StatusCode, streamReader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/jsonpostexception", "Couldn't send message due to exception: {0}", ex.Message));
                return new Tuple<int, string>(-1, "");
            }
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
