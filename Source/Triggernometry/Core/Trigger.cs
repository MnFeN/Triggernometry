using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Triggernometry.Core.Conditions;
using Triggernometry.Core.Serialization;
using Triggernometry.Localization;

namespace Triggernometry.Core
{

    public class Trigger
    {

        #region Properties - Identity/Runtime

        [XmlIgnore]
        public Folder Parent { get; set; }

        [XmlIgnore]
        public Repository Repo { get; set; } = null;

        [XmlIgnore]
        public bool ZoneBlocked { get; set; } = false;

        [XmlIgnore]
        private DateTime LastFired { get; set; }

        [XmlIgnore]
        internal DateTime RefireDelayedUntil { get; set; }

        [XmlIgnore]
        public Repository.RestrictionEnum RepoRestrictions { get; internal set; } = Repository.RestrictionEnum.None;

        [XmlIgnore]
        public string LogName => $"{Name} ({(Repo != null ? "@" : "")}{Id})";

        [XmlIgnore]
        public string FullPath
        {
            get
            {
                string name = Name;
                Folder f = Parent;
                while (f != null)
                {
                    if (f.Parent != null)
                    {
                        name = f.Name + @"\" + name;
                    }
                    f = f.Parent;
                }
                return name;
            }
        }

        #endregion Properties - Identity/Runtime


        #region Properties - General Settings

        [XmlAttribute]
        public bool Enabled { get; set; }


        [XmlAttribute]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The name of the trigger.
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; } = "";

        /// <summary>
        /// Represents the cached regular expression used internally for pattern matching operations. <br />
        /// Null if the regular expression is invalid or empty.
        /// </summary>
        internal Regex regexCache;

        private string _regularExpression = "";

        /// <summary>
        /// The regular expression pattern used by this trigger to match log lines.
        /// </summary>
        [XmlIgnore]
        public string RegularExpression
        {
            get => _regularExpression;
            set
            {
                var newExpr = RealPlugin.UnserializeInvalidXmlCharacters(value);
                if (_regularExpression == newExpr) return;

                _regularExpression = newExpr;
                if (string.IsNullOrWhiteSpace(newExpr))
                {
                    regexCache = null;
                    return;
                }
                try
                {
                    regexCache = new Regex(_regularExpression);
                }
                catch
                {
                    regexCache = null;
                }
            }
        }

        [XmlAttribute("RegularExpression")]
        public string Xml_RegularExpression
        {
            get => XmlAttr.String(RegularExpression);
            set => RegularExpression = value;
        }

        /// <summary>
        /// Attempts to match the given input string against the trigger's regular expression.
        /// </summary>
        /// <returns>
        /// A <see cref="Match"/> object if the trigger has a non-empty valid regex and matches successfully; otherwise, <c>null</c>.
        /// </returns>
        public Match CheckMatch(string input)
        {
            var regex = regexCache; // snapshot
            if (regex == null || input == null)
                return null;

            var m = regex.Match(input);
            return m.Success ? m : null;
        }

        #endregion Properties - General Settings


        #region Properties - Actions/Conditions

        /// <summary>
        /// The list of actions executed when this trigger is fired.
        /// </summary>
        [XmlArray("Actions")]
        [XmlArrayItem("Action")]
        public List<ActionOld> Actions { get; set; } = new List<ActionOld>();

        public bool ShouldSerializeActions() => Actions != null && Actions.Count > 0;

        /// <summary>
        /// The condition group that determines whether this trigger should execute.
        /// </summary>
        [XmlIgnore]
        public ConditionGroup Condition { get; set; } = new ConditionGroup { Enabled = false };

        [XmlElement("Condition")]
        public ConditionGroup Xml_Condition
        {
            get => Condition.Children.Count == 0 && !Condition.Enabled ? null : Condition;
            set => Condition = value ?? new ConditionGroup { Enabled = false };
        }

        #endregion Properties - Actions/Conditions


        #region Properties - Scheduling

        /// <summary>
        /// Defines the possible sources from which a trigger can be activated, such as ACT Log, Network, etc.
        /// </summary>
        public enum TriggerSourceEnum
        {
            /// <summary> Parsed ACT Log Lines. </summary>
            Log,
            /// <summary> FFXIV Network Log Lines. </summary>
            FFXIVNetwork,
            None,
            /// <summary> ACT Events. </summary>
            ACT,
            /// <summary> Triggernometry Endpoint. </summary>
            Endpoint
        }

        /// <summary>
        /// Specifies the source of the trigger's input, such as ACT Log, Network, etc.
        /// </summary>
        [XmlIgnore]
        public TriggerSourceEnum Source { get; set; } = TriggerSourceEnum.Log;

        [XmlAttribute("Source")]
        public string Xml_Source
        {
            get => XmlAttr.Enum(Source, TriggerSourceEnum.Log);
            set => Source = XmlAttr.Enum<TriggerSourceEnum>(value);
        }

        /// <summary> 
        /// A string tag used in trigger/folder actions to interrupt specific actions. Null if not set.
        /// </summary>
        [XmlIgnore]
        public string Tag { get; set; }

        [XmlAttribute("Tag")]
        public string Xml_Tag
        {
            get => XmlAttr.String(Tag);
            set => Tag = value;
        }

        public enum PrevActionsEnum
        {
            Keep,
            Interrupt
        }

        /// <summary>
        /// Specifies whether actions already queued from this trigger should keep executing when it refires.
        /// </summary>
        [XmlIgnore]
        public PrevActionsEnum PrevActions { get; set; } = PrevActionsEnum.Keep;

        [XmlAttribute("PrevActions")]
        public string Xml_PrevActions
        {
            get => XmlAttr.Enum(PrevActions, PrevActionsEnum.Keep);
            set => PrevActions = XmlAttr.Enum<PrevActionsEnum>(value);
        }

        public enum RefireEnum
        {
            Allow,
            Deny
        }

        /// <summary>
        /// Determines whether the trigger is allowed to refire when an action from it is already queued.
        /// </summary>
        [XmlIgnore]
        public RefireEnum PrevActionsRefire { get; set; } = RefireEnum.Allow;

        [XmlAttribute("PrevActionsRefire")]
        public string Xml_PrevActionsRefire
        {
            get => XmlAttr.Enum(PrevActionsRefire, RefireEnum.Allow);
            set => PrevActionsRefire = XmlAttr.Enum<RefireEnum>(value);
        }

        public enum SchedulingEnum
        {
            FromFire,
            FromLastAction,
            FromRefirePeriod
        }

        /// <summary>
        /// Specifies the scheduling mode used by the trigger.
        /// </summary>
        [XmlIgnore]
        public SchedulingEnum Scheduling { get; set; } = SchedulingEnum.FromFire;

        [XmlAttribute("Scheduling")]
        public string Xml_Scheduling
        {
            get => XmlAttr.Enum(Scheduling, SchedulingEnum.FromFire);
            set => Scheduling = XmlAttr.Enum<SchedulingEnum>(value);
        }

        /// <summary>
        /// Determines whether the trigger is allowed to refire during its refire period.
        /// </summary>
        [XmlIgnore]
        public RefireEnum PeriodRefire { get; set; } = RefireEnum.Allow;

        [XmlAttribute("PeriodRefire")]
        public string Xml_PeriodRefire
        {
            get => XmlAttr.Enum(PeriodRefire, RefireEnum.Allow);
            set => PeriodRefire = XmlAttr.Enum<RefireEnum>(value);
        }

        /// <summary>
        /// Specifies the expression that determines the refire period of the trigger.
        /// </summary>
        [XmlIgnore]
        public string RefirePeriodExpression { get; set; } = "0";

        [XmlAttribute("RefirePeriodExpression")]
        public string Xml_RefirePeriodExpression
        {
            get => XmlAttr.String(RefirePeriodExpression, "0");
            set => RefirePeriodExpression = value;
        }

        /// <summary>
        /// Specifies the mutex name that this trigger should capture when executed.
        /// </summary>
        [XmlIgnore]
        public string MutexToCapture { get; set; }

        [XmlAttribute("MutexToCapture")]
        public string Xml_MutexToCapture
        {
            get => XmlAttr.String(MutexToCapture);
            set => MutexToCapture = value;
        }

        /// <summary>
        /// Determines whether the trigger should automatically fire after being saved.
        /// </summary>
        [XmlIgnore]
        public bool EditAutofire { get; set; } = false;

        [XmlAttribute("EditAutofire")]
        public string Xml_EditAutofire
        {
            get => XmlAttr.Bool(EditAutofire, false);
            set => EditAutofire = XmlAttr.Bool(value);
        }

        /// <summary>
        /// Determines whether the trigger should respect its conditions when auto-fired after being saved.
        /// </summary>
        [XmlIgnore]
        public bool EditAutofireAllowCondition { get; set; } = false;

        [XmlAttribute("EditAutofireAllowCondition")]
        public string Xml_EditAutofireAllowCondition
        {
            get => XmlAttr.Bool(EditAutofireAllowCondition, false);
            set => EditAutofireAllowCondition = XmlAttr.Bool(value);
        }

        /// <summary>
        /// Determines whether the actions in this trigger are executed sequentially.
        /// </summary>
        [XmlIgnore]
        public bool Sequential { get; set; } = false;

        [XmlAttribute("Sequential")]
        public string Xml_Sequential
        {
            get => XmlAttr.Bool(Sequential, false);
            set => Sequential = XmlAttr.Bool(value);
        }

        #endregion Properties - Scheduling


        #region Properties - Debugging

        /// <summary>
        /// The <see cref="RealPlugin.DebugLevelEnum"/> applied to this trigger.
        /// If set to <see cref="RealPlugin.DebugLevelEnum.Inherit"/>, the trigger uses the plugin's global debug level.
        /// </summary>
        [XmlIgnore]
        public RealPlugin.DebugLevelEnum DebugLevel { get; set; } = RealPlugin.DebugLevelEnum.Inherit;

        [XmlAttribute("DebugLevel")]
        public string Xml_DebugLevel
        {
            get => XmlAttr.Enum(DebugLevel, RealPlugin.DebugLevelEnum.Inherit);
            set => DebugLevel = XmlAttr.Enum<RealPlugin.DebugLevelEnum>(value);
        }

        /// <summary> 
        /// Simulated log lines used when force-firing the trigger, one entry per line. Null if not set.
        /// </summary>
        [XmlIgnore]
        public string TestInput { get; set; }

        [XmlAttribute("TestInput")]
        public string Xml_TestInput
        {
            get => XmlAttr.String(TestInput);
            set => TestInput = value;
        }

        #endregion Properties - Debugging


        #region Properties - Description

        /// <summary> 
        /// Description text associated with this trigger. 
        /// </summary>
        [XmlIgnore]
        public string Description { get; set; }

        [XmlAttribute("Description")]
        public string Xml_Description
        {
            get => XmlAttr.String(Description);
            set => Description = value;
        }

        /// <summary>
        /// Indicates whether this trigger serves as a readme or informational entry rather than an active trigger.
        /// </summary>
        [XmlIgnore]
        public bool IsReadme { get; set; } = false;

        [XmlAttribute("IsReadme")]
        public string Xml_IsReadme
        {
            get => XmlAttr.Bool(IsReadme, false);
            set => IsReadme = XmlAttr.Bool(value);
        }

        #endregion Properties - Description


        #region Old property conversions

        [Obsolete("Old XML alias, kept for backward compatibility.")]
        [XmlAttribute("AllowRefire")]
        public string Xml_AllowRefire
        {
            get => null;
            set => PrevActionsRefire = XmlAttr.Bool(value) ? RefireEnum.Allow : RefireEnum.Deny;
        }

        [Obsolete("Old XML alias, kept for backward compatibility.")]
        [XmlAttribute("RefireDelayed")]
        public string Xml_RefireDelayed
        {
            get => null;
            set => PeriodRefire = XmlAttr.Bool(value) ? RefireEnum.Deny : RefireEnum.Allow;
        }

        [Obsolete("Old XML alias, kept for backward compatibility.")]
        [XmlAttribute("RefireDelayExpression")]
        public string Xml_RefireDelayExpression
        {
            get => null;
            set => RefirePeriodExpression = value;
        }

        #endregion


        public Trigger()
        {
        }


        #region Logging

        /// <summary>
        /// Returns the effective debug level for this trigger, resolving inheritance from the plugin if needed.
        /// </summary>
        internal RealPlugin.DebugLevelEnum GetDebugLevel(RealPlugin p)
        {
            if (DebugLevel == RealPlugin.DebugLevelEnum.Inherit)
            {
                if (p?.cfg != null)
                {
                    return p.cfg.DebugLevel;
                }
                else
                {
                    return RealPlugin.DebugLevelEnum.Verbose;
                }
            }
            return DebugLevel;
        }

        /// <summary>
        /// Adds a log entry for this trigger if the specified debug level is within the effective debug level threshold.
        /// </summary>
        internal void AddToLog(RealPlugin.DebugLevelEnum level, string message)
        {
            RealPlugin.DebugLevelEnum configLevel = GetDebugLevel(RealPlugin.Instance);
            if (level > configLevel)
            {
                return;
            }
            RealPlugin.Instance.UnfilteredAddToLog(level, message, this);
        }

        public void TriggerContextLogger(object _, string msg)
        {
            AddToLog(RealPlugin.DebugLevelEnum.Verbose, msg);
        }

        #endregion Logging


        #region Firing Pipeline

        /// <summary>
        /// Executes trigger firing pipeline and enqueues actions when allowed. <br />
        /// · <see langword="true"/> if actions are queued, or a deferred fire is successfully scheduled (mutex capture). <br />
        /// · <see langword="false"/> if blocked (repository/condition/refire rules) or an exception occurs.
        /// </summary>
        public bool Fire(Context ctx, RealPlugin.MutexInformation mtx = null)
		{
            try
            {
                if (TryBlockByRepository())
                    return false;

                if (TryScheduleDeferredFireWithMutexCapture(ctx, mtx))
                    return true;

                if (TryBlockByCondition(ctx))
                    return false;

                DateTime prevLastFired = UpdateFireTiming(ctx);
                DateTime scheduleBaseTime = ComputeScheduleBaseTime(ctx, prevLastFired);

                if (!ApplyPrevActionsPolicy(ctx))
                    return false;

                QueueActions(ctx, scheduleBaseTime, mtx);
                return true;
            }
            catch (Exception ex)
            {
                AddToLog(RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Trigger/firingexception", "Trigger '{0}' didn't fire due to exception: {1}", LogName, ex.ToString()));
            }
            return false;
        }

        /// <summary> <see langword="true"/> if this fire should be blocked.</summary>
        private bool TryBlockByRepository()
        {
            if (Repo == null || RepoRestrictions == Repository.RestrictionEnum.None)
                return false;

            var restrictionNames = Enum.GetValues(typeof(Repository.RestrictionEnum))
                .Cast<Repository.RestrictionEnum>()
                .Where(flag => flag != Repository.RestrictionEnum.None && RepoRestrictions.HasFlag(flag))
                .Select(flag => I18n.Translate($"internal/Repository/restriction/{flag}", flag.ToString()))
                .ToArray();

            AddToLog(RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Trigger/restricted",
                "Trigger '{1}' was not executed because the following repository permission(s) are disabled: \n[{0}] \nYou can enable these permissions for the remote repository, or disable this trigger to prevent this error.",
                string.Join(", ", restrictionNames), FullPath));
            return true;
        }

        /// <summary>
        /// Schedules a deferred fire when this trigger needs to capture a mutex by name. <br />
        /// · <see langword="true"/> if a background task is scheduled and the caller should stop further processing. <br />
        /// · <see langword="false"/> if mutex capture is not applicable (mtx already present or no MutexToCapture configured).
        /// </summary>
        private bool TryScheduleDeferredFireWithMutexCapture(Context ctx, RealPlugin.MutexInformation mtx)
        {
            if (mtx != null)
                return false;

            if (string.IsNullOrWhiteSpace(MutexToCapture))
                return false;

            string mn = ctx.EvaluateStringExpression(TriggerContextLogger, ctx.Plugin, MutexToCapture);
            RealPlugin.MutexInformation mi = ctx.Plugin.GetMutex(mn);
            RealPlugin.MutexTicket m = mi.QueueForAcquisition(ctx);

            _ = Task.Run(() =>
            {
                try
                {
                    DeferredFire(ctx, mi, m);
                }
                catch (Exception ex)
                {
                    AddToLog(RealPlugin.DebugLevelEnum.Error, I18n.Translate(
                        "internal/Trigger/firingexception",
                        "Trigger '{0}' didn't fire due to exception: {1}",
                        LogName, ex.ToString()));
                }
            });

            // Once scheduled, we must stop the current pipeline because DeferredFire will re-enter the trigger later with the acquired mutex.
            return true;
        }

        internal void DeferredFire(Context ctx, RealPlugin.MutexInformation mi, RealPlugin.MutexTicket m)
        {
            using (m)
            {
                mi.Acquire(ctx, m);
                if (Fire(ctx, mi) == false)
                {
                    mi.Release(ctx);
                }
            }
        }

        /// <summary> <see langword="true"/> if this fire should be blocked.</summary>
        private bool TryBlockByCondition(Context ctx)
        {
            if ((ctx.forceType & ActionOld.TriggerForceTypeEnum.SkipConditions) != 0)
                return false;

            if (Condition?.Enabled != true)
                return false;

            if (Condition.CheckCondition(ctx, TriggerContextLogger, ctx.Plugin) == true)
                return false;

            AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate("internal/Trigger/trignotfired", "Trigger '{0}' not fired, condition not met", LogName));
            return true;
        }

        /// <summary>
        /// Updates LastFired and refire delay window for this trigger. <br />
        /// · Returns the previous LastFired value, used as a scheduling base when Scheduling is FromRefirePeriod. <br />
        /// · Updates RefireDelayedUntil based on PeriodRefire and RefirePeriodExpression.
        /// </summary>
        private DateTime UpdateFireTiming(Context ctx)
        {
            DateTime prevLastFired = LastFired;
            LastFired = DateTime.Now;

            if (PeriodRefire == RefireEnum.Deny)
            {
                RefireDelayedUntil = LastFired.AddMilliseconds(ctx.EvaluateNumericExpression(TriggerContextLogger, ctx.Plugin, RefirePeriodExpression));
                AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate("internal/Trigger/delayingrefire", "Delaying trigger '{0}' refire to {1}", LogName, RefireDelayedUntil));
            }
            else
            {
                RefireDelayedUntil = DateTime.MinValue;
            }

            return prevLastFired;
        }

        /// <summary>
        /// Computes the base timestamp used to schedule queued actions. <br />
        /// · FromLastAction: uses the latest queued action time for this trigger. <br />
        /// · FromRefirePeriod: uses previous fire time plus refire period, clamped to LastFired.
        /// </summary>
        private DateTime ComputeScheduleBaseTime(Context ctx, DateTime previousLastFired)
        {
            DateTime scheduleBaseTime = DateTime.Now;
            if (Scheduling == SchedulingEnum.FromLastAction)
            {
                // get the last queued action as curTime
                lock (ctx.Plugin.ActionQueue)
                {
                    var lastQueuedAction = GetSelfQueuedActions(ctx).OrderByDescending(ax => ax.when).FirstOrDefault();
                    if (lastQueuedAction != null)
                    {
                        scheduleBaseTime = lastQueuedAction.when;
                        AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate("internal/Trigger/lastactionfound", "Last action for trigger '{0}' found at {1}", LogName, scheduleBaseTime));
                    }
                }
            }
            else if (Scheduling == SchedulingEnum.FromRefirePeriod)
            {
                var refirePeriod = ctx.EvaluateNumericExpression(TriggerContextLogger, ctx.Plugin, RefirePeriodExpression);
                scheduleBaseTime = previousLastFired.AddMilliseconds(refirePeriod);
                if (scheduleBaseTime < LastFired)
                {
                    scheduleBaseTime = LastFired;
                    AddToLog(RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Trigger/beforelastfired", "Current time is before last fired for trigger '{0}'", LogName));
                }
            }
            return scheduleBaseTime;
        }

        /// <summary>
        /// Applies previous-actions policy before enqueuing new actions. <br />
        /// · Interrupt: removes existing queued actions for this trigger (optionally denies refire). <br />
        /// · Refire deny: blocks if any action for this trigger is still in queue. <br />
        /// Returns <see langword="false"/> when refire is denied by the policy.
        /// </summary>
        private bool ApplyPrevActionsPolicy(Context ctx)
        {
            if (PrevActions == PrevActionsEnum.Interrupt)
            {
                int removedCount = RemoveSelfQueuedActions(ctx);
                if (removedCount > 0)
                {
                    if (PrevActionsRefire == RefireEnum.Deny)
                    {
                        AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate(
                            "internal/Trigger/removefromqueuenorefire",
                            "Removed {0} instance(s) of trigger '{1}' actions from queue, refire denied",
                            removedCount, LogName));
                        return false;
                    }

                    AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate(
                        "internal/Trigger/removefromqueue",
                        "Removed {0} instance(s) of trigger '{1}' actions from queue",
                        removedCount, LogName));
                }

                return true;
            }

            if (PrevActionsRefire == RefireEnum.Deny)
            {
                int queuedCount = CountSelfQueuedActions(ctx);
                if (queuedCount > 0)
                {
                    AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate(
                        "internal/Trigger/refiredenied",
                        "{0} instance(s) of trigger '{1}' actions in queue, refire denied",
                        queuedCount, LogName));
                    return false;
                }
            }

            return true;
        }

        private int RemoveSelfQueuedActions(Context ctx)
        {
            int removedCount = 0;
            lock (ctx.Plugin.ActionQueue)
            {
                foreach (RealPlugin.QueuedAction qa in GetSelfQueuedActions(ctx))
                {
                    ctx.Plugin.ActionQueue.Remove(qa);
                    removedCount++;
                }
            }
            return removedCount;
        }

        private int CountSelfQueuedActions(Context ctx)
        {
            lock (ctx.Plugin.ActionQueue)
                return GetSelfQueuedActions(ctx).Count;
        }

        private List<RealPlugin.QueuedAction> GetSelfQueuedActions(Context ctx)
            => ctx.Plugin.ActionQueue.Where(qa => qa.ctx.Trigger.Id == Id).ToList();

        public void QueueActions(Context ctx, DateTime curtime, RealPlugin.MutexInformation mtx)
        {
            //System.Diagnostics.Debug.WriteLine("### queuing actions for " + ctx.ToString());
            RealPlugin.Instance.QueueActions(ctx, curtime, Actions, Sequential, mtx, TriggerContextLogger);
        }

        #endregion Firing Pipeline

        #region Utils

        internal bool PassesZoneRestriction(string zone)
        {
            return Parent.PassesZoneRestriction(zone);
        }

        public void CopySettingsTo(Trigger t)
        {
            t.Enabled = Enabled;
            t.Name = Name;
            t.Id = Id;
            t.RegularExpression = RegularExpression;

            // ─── Properties - Actions/Conditions ────────────────
            t.Actions.Clear();
            foreach (var oldAction in Actions.OrderBy(x => x.OrderNumber))
            {
                var newAction = new ActionOld();
                oldAction.CopySettingsTo(newAction);
                t.Actions.Add(newAction);
            }
            t.Condition = (ConditionGroup)Condition.Duplicate();

            // ─── Properties - Scheduling ────────────────────────
            t.Source = Source;
            t.Tag = Tag;
            t.Sequential = Sequential;
            t.PrevActions = PrevActions;
            t.PrevActionsRefire = PrevActionsRefire;
            t.Scheduling = Scheduling;
            t.PeriodRefire = PeriodRefire;
            t.RefirePeriodExpression = RefirePeriodExpression;
            t.MutexToCapture = MutexToCapture;
            t.EditAutofire = EditAutofire;
            t.EditAutofireAllowCondition = EditAutofireAllowCondition;

            // ─── Properties - Debugging ─────────────────────────
            t.DebugLevel = DebugLevel;
            t.TestInput = TestInput;

            // ─── Properties - Description ───────────────────────
            t.Description = Description;
            t.IsReadme = IsReadme;
        }

        internal void SetActionsParent()
        {
            Actions.ForEach(a => a.ParentTrigger = this);
        }

        #endregion Utils
    }

}
