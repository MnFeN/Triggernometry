using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Triggernometry
{

    public class Trigger
    {

        #region Properties - Basic

        [XmlIgnore]
        public Folder Parent { get; set; }

        [XmlIgnore]
        public bool ZoneBlocked { get; set; } = false;

        [XmlAttribute]
        public bool Enabled { get; set; }

        [XmlAttribute]
        public Guid Id { get; set; } = Guid.NewGuid();

        [XmlIgnore]
        public Repository Repo { get; set; } = null;

        private DateTime LastFired { get; set; }
        internal DateTime RefireDelayedUntil { get; set; }

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

        private Repository.RestrictionEnum _repoRestrictions = Repository.RestrictionEnum.None;
        [XmlIgnore]
        public Repository.RestrictionEnum RepoRestrictions
        {
            get => _repoRestrictions;
            internal set
            {
                _repoRestrictions = value;
            }
        }

        #endregion Properties - Basic


        #region Properties - General Settings

        /// <summary>
        /// The name of the trigger.
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; } = "";

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
                if (_regularExpression != newExpr)
                {
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
        }

        [XmlAttribute("RegularExpression")]
        public string Xml_RegularExpression
        {
            get => string.IsNullOrWhiteSpace(RegularExpression) ? null : RegularExpression;
            set => RegularExpression = value;
        }

        #endregion Properties - General Settings


        #region Properties - Actions/Conditions

        /// <summary>
        /// The list of actions executed when this trigger is fired.
        /// </summary>
        public List<Action> Actions { get; set; } = new List<Action>();

        /// <summary>
        /// The condition group that determines whether this trigger should execute.
        /// </summary>
        [XmlIgnore]
        public ConditionGroup Condition { get; set; } = new ConditionGroup { Enabled = false };

        [XmlElement("Condition")]
        public ConditionGroup Xml_Condition
        {
            get => Condition.Children.Count == 0 && !Condition.Enabled ? null : Condition;
            set => Condition = value;
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
            get => Source != TriggerSourceEnum.Log ? Source.ToString() : null;
            set => Source = Enum.TryParse(value, out TriggerSourceEnum e) ? e : TriggerSourceEnum.Log;
        }

        /// <summary> 
        /// A string tag used in trigger/folder actions to interrupt specific actions. 
        /// </summary>
        [XmlIgnore]
        public string Tag { get; set; }

        [XmlAttribute("Tag")]
        public string Xml_Tag
        {
            get => string.IsNullOrWhiteSpace(Tag) ? null : Tag;
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
            get => PrevActions != PrevActionsEnum.Keep ? PrevActions.ToString() : null;
            set => PrevActions = Enum.TryParse(value, out PrevActionsEnum e) ? e : PrevActionsEnum.Keep;
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
            get => PrevActionsRefire != RefireEnum.Allow ? PrevActionsRefire.ToString() : null;
            set => PrevActionsRefire = Enum.TryParse(value, out RefireEnum e) ? e : RefireEnum.Allow;
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
            get => Scheduling != SchedulingEnum.FromFire ? Scheduling.ToString() : null;
            set => Scheduling = Enum.TryParse(value, out SchedulingEnum e) ? e : SchedulingEnum.FromFire;
        }

        /// <summary>
        /// Determines whether the trigger is allowed to refire during its refire period.
        /// </summary>
        [XmlIgnore]
        public RefireEnum PeriodRefire { get; set; } = RefireEnum.Allow;

        [XmlAttribute("PeriodRefire")]
        public string Xml_PeriodRefire
        {
            get => PeriodRefire != RefireEnum.Allow ? PeriodRefire.ToString() : null;
            set => PeriodRefire = Enum.TryParse(value, out RefireEnum e) ? e : RefireEnum.Allow;
        }

        /// <summary>
        /// Specifies the expression that determines the refire period of the trigger.
        /// </summary>
        [XmlIgnore]
        public string RefirePeriodExpression { get; set; } = "0";

        [XmlAttribute("RefirePeriodExpression")]
        public string Xml_RefirePeriodExpression
        {
            get => string.IsNullOrWhiteSpace(RefirePeriodExpression) || RefirePeriodExpression == "0"
                ? null
                : RefirePeriodExpression;
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
            get => string.IsNullOrWhiteSpace(MutexToCapture) ? null : MutexToCapture;
            set => MutexToCapture = value;
        }

        /// <summary>
        /// Determines whether the trigger should automatically fire after being saved.
        /// </summary>
        [XmlIgnore]
        public bool EditAutofire { get; set; }

        [XmlAttribute("EditAutofire")]
        public string Xml_EditAutofire
        {
            get => EditAutofire ? bool.TrueString : null;
            set => EditAutofire = bool.TryParse(value, out var result) && result;
        }

        /// <summary>
        /// Determines whether the trigger should respect its conditions when auto-fired after being saved.
        /// </summary>
        [XmlIgnore]
        public bool EditAutofireAllowCondition { get; set; }

        [XmlAttribute("EditAutofireAllowCondition")]
        public string Xml_EditAutofireAllowCondition
        {
            get => EditAutofireAllowCondition ? bool.TrueString : null;
            set => EditAutofireAllowCondition = bool.TryParse(value, out var result) && result;
        }

        /// <summary>
        /// Determines whether the actions in this trigger are executed sequentially.
        /// </summary>
        [XmlIgnore]
        public bool Sequential { get; set; } = false;

        [XmlAttribute("Sequential")]
        public string Xml_Sequential
        {
            get => Sequential ? bool.TrueString : null;
            set => Sequential = bool.TryParse(value, out var result) && result;
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
            get => DebugLevel != RealPlugin.DebugLevelEnum.Inherit ? DebugLevel.ToString() : null;
            set => DebugLevel = Enum.TryParse(value, out RealPlugin.DebugLevelEnum e) ? e : RealPlugin.DebugLevelEnum.Inherit;
        }

        /// <summary> 
        /// Simulated log lines used when force-firing the trigger, one entry per line.
        /// </summary>
        [XmlIgnore]
        public string TestInput { get; set; }

        [XmlAttribute("TestInput")]
        public string Xml_TestInput
        {
            get => string.IsNullOrWhiteSpace(TestInput) ? null : TestInput;
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
            get => string.IsNullOrWhiteSpace(Description) ? null : Description;
            set => Description = value;
        }

        /// <summary>
        /// Specifies that this trigger's description serves as the readme for repositories.
        /// </summary>
        /// <summary>
        /// Indicates whether this trigger serves as a readme or informational entry rather than an active trigger.
        /// </summary>
        [XmlIgnore]
        public bool IsReadme { get; set; } = false;

        [XmlAttribute("IsReadme")]
        public string Xml_IsReadme
        {
            get => IsReadme ? IsReadme.ToString() : null;
            set => IsReadme = bool.TryParse(value, out var result) && result;
        }

        #endregion Properties - Description


        #region Old condition to new condition converter
        private EventList<Condition> _Conditions = new EventList<Triggernometry.Condition>();
        public EventList<Condition> Conditions
        {
            get
            {
                return (_Conditions?.Count ?? 0) == 0 ? null : _Conditions;
            }
            set
            {
                _Conditions = value;
                if (_Conditions != null)
                {
                    _Conditions.ItemAdded += _Conditions_ItemAdded;
                }
            }
        }

        private void _Conditions_ItemAdded(object sender, EventListArgs<Condition> e)
        {
            if (Condition == null)
            {
                Condition = new ConditionGroup();
                Condition.Grouping = ConditionGroup.CndGroupingEnum.And;
                Condition.Enabled = true;
            }
            Condition cx = e.Item;
            Condition.AddChild(cx.ConvertToConditionSingle());
            _Conditions.Remove(e.Item);
        }

        public class EventListArgs<T> : EventArgs
        {
            public EventListArgs(T item, int index)
            {
                Item = item;
                Index = index;
            }

            public T Item { get; }
            public int Index { get; }
        }

        public class EventList<T> : IList<T>
        {
            private readonly List<T> _list;

            public EventList()
            {
                _list = new List<T>();
            }

            public EventList(IEnumerable<T> collection)
            {
                _list = new List<T>(collection);
            }

            public EventList(int capacity)
            {
                _list = new List<T>(capacity);
            }

            public event EventHandler<EventListArgs<T>> ItemAdded;
            public event EventHandler<EventListArgs<T>> ItemRemoved;

            private void RaiseEvent(EventHandler<EventListArgs<T>> eventHandler, T item, int index)
            {
                var eh = eventHandler;
                eh?.Invoke(this, new EventListArgs<T>(item, index));
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(T item)
            {
                var index = _list.Count;
                _list.Add(item);
                RaiseEvent(ItemAdded, item, index);
            }

            public void Clear()
            {
                for (var index = 0; index < _list.Count; index++)
                {
                    var item = _list[index];
                    RaiseEvent(ItemRemoved, item, index);
                }

                _list.Clear();
            }

            public bool Contains(T item)
            {
                return _list.Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                _list.CopyTo(array, arrayIndex);
            }

            public bool Remove(T item)
            {
                var index = _list.IndexOf(item);

                if (_list.Remove(item))
                {
                    RaiseEvent(ItemRemoved, item, index);
                    return true;
                }

                return false;
            }

            public int Count => _list.Count;
            public bool IsReadOnly => false;

            public int IndexOf(T item)
            {
                return _list.IndexOf(item);
            }

            public void Insert(int index, T item)
            {
                _list.Insert(index, item);
                RaiseEvent(ItemRemoved, item, index);
            }

            public void RemoveAt(int index)
            {
                var item = _list[index];
                _list.RemoveAt(index);
                RaiseEvent(ItemRemoved, item, index);
            }

            public T this[int index]
            {
                get { return _list[index]; }
                set { _list[index] = value; }
            }
        }
        #endregion


        #region Old property conversions

        [Obsolete("Old XML alias, kept for backward compatibility.")]
        [XmlAttribute("AllowRefire")]
        public string Xml_AllowRefire
        {
            get => null; // Legacy field, not serialized in new versions
            set => PrevActionsRefire = bool.TryParse(value, out var result) && result ? RefireEnum.Allow : RefireEnum.Deny;
        }

        [Obsolete("Old XML alias, kept for backward compatibility.")]
        [XmlAttribute("RefireDelayed")]
        public string Xml_RefireDelayed
        {
            get => null;
            set => PeriodRefire = bool.TryParse(value, out var result) && result ? RefireEnum.Deny : RefireEnum.Allow;
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

        /// <summary>
        /// Attempts to match the given input string against the trigger's regular expression.
        /// </summary>
        /// <returns>
        /// A <see cref="Match"/> object if the trigger has a non-empty valid regex and matches successfully; otherwise, <c>null</c>.
        /// </returns>
        public Match CheckMatch(string input)
        {
			if (regexCache == null)
			{
				return null;
			}		
            try
            {
                Match m = regexCache.Match(input);
                if (m.Success == true)
                {
                    return m;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

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
            RealPlugin.DebugLevelEnum dx = GetDebugLevel(RealPlugin.plug);
            if (level > dx)
            {
                return;
            }
            RealPlugin.plug.UnfilteredAddToLog(level, message, this);
        }

        internal void DeferredFire(RealPlugin p, Context ctx, RealPlugin.MutexInformation mi, RealPlugin.MutexTicket m)
        {
            using (m)
            {
                mi.Acquire(ctx, m);
                if (Fire(p, ctx, mi) == false)
                {
                    mi.Release(ctx);
                }
            }
        }

        internal bool PassesZoneRestriction(string zone)
        {
            return Parent.PassesZoneRestriction(zone);
        }

        public void QueueActions(Context ctx, DateTime curtime, RealPlugin.MutexInformation mtx)
        {
            //System.Diagnostics.Debug.WriteLine("### queuing actions for " + ctx.ToString());
            RealPlugin.plug.QueueActions(ctx, curtime, Actions, Sequential, mtx, TriggerContextLogger);
        }

        public bool Fire(RealPlugin p, Context ctx, RealPlugin.MutexInformation mtx)
		{
            if (Repo != null && RepoRestrictions != Repository.RestrictionEnum.None)
            {
                var restrictionNames = Enum.GetValues(typeof(Repository.RestrictionEnum))
                        .Cast<Repository.RestrictionEnum>()
                        .Where(flag => flag != Repository.RestrictionEnum.None && RepoRestrictions.HasFlag(flag))
                        .Select(flag => I18n.Translate($"internal/Repository/restriction/{flag}", flag.ToString()))
                        .ToList();
                AddToLog(RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Trigger/restricted",
                    "Trigger '{1}' was not executed because the following repository permission(s) are disabled: \n[{0}] \nYou can enable these permissions for the remote repository, or disable this trigger to prevent this error.",
                    string.Join(", ", restrictionNames), FullPath));
                return false;
            }
            try
            {
                if (mtx == null && !string.IsNullOrWhiteSpace(MutexToCapture))
                {
                    string mn = ctx.EvaluateStringExpression(TriggerContextLogger, p, MutexToCapture);
                    RealPlugin.MutexInformation mi = ctx.plug.GetMutex(mn);
                    RealPlugin.MutexTicket m = mi.QueueForAcquisition(ctx);
                    _ = Task.Run(() =>
                    {
                        using (m)
                        {
                            DeferredFire(ctx.plug, ctx, mi, m);
                        }
                    });
                    return true;
                }
                if ((ctx.force & Action.TriggerForceTypeEnum.SkipConditions) == 0)
                {
                    if (Condition != null && Condition.Enabled == true)
                    {
                        if (Condition.CheckCondition(ctx, TriggerContextLogger, ctx.plug) == false)
                        {
                            AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate("internal/Trigger/trignotfired", "Trigger '{0}' not fired, condition not met", LogName));
                            return false;
                        }
                    }
                }
                DateTime prevLastFired = LastFired;
                LastFired = DateTime.Now;
                if (PeriodRefire == RefireEnum.Deny)
                {
                    RefireDelayedUntil = LastFired.AddMilliseconds(ctx.EvaluateNumericExpression(TriggerContextLogger, p, RefirePeriodExpression));
                    AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate("internal/Trigger/delayingrefire", "Delaying trigger '{0}' refire to {1}", LogName, RefireDelayedUntil));
                }
                else
                {
                    RefireDelayedUntil = DateTime.MinValue;
                }
                DateTime curtime = DateTime.Now;
                if (Scheduling == SchedulingEnum.FromLastAction)
                {
                    // get the last queued action as curTime
                    lock (ctx.plug.ActionQueue)
                    {
                        var ixy = from ax in ctx.plug.ActionQueue
                                  where ax.ctx.trig.Id == Id
                                  orderby ax.when descending
                                  select ax;
                        if (ixy.Count() > 0)
                        {
                            curtime = ixy.ElementAt(0).when;
                            AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate("internal/Trigger/lastactionfound", "Last action for trigger '{0}' found at {1}", LogName, curtime));
                        }
                    }
                }
                else if (Scheduling == SchedulingEnum.FromRefirePeriod)
                {
                    curtime = prevLastFired.AddMilliseconds(ctx.EvaluateNumericExpression(TriggerContextLogger, p, RefirePeriodExpression));
                    if (curtime < LastFired)
                    {
                        curtime = LastFired;
                        AddToLog(RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Trigger/beforelastfired", "Current time is before last fired for trigger '{0}'", LogName));
                    }
                }
                if (PrevActions == PrevActionsEnum.Interrupt)
                {
                    int exx = 0;
                    lock (ctx.plug.ActionQueue)
                    {
                        var ixy = from ax in ctx.plug.ActionQueue
                                  where ax.ctx.trig.Id == Id
                                  select ax;
                        if (ixy.Count() > 0)
                        {
                            List<RealPlugin.QueuedAction> rems = new List<RealPlugin.QueuedAction>();
                            rems.AddRange(ixy);
                            foreach (RealPlugin.QueuedAction qa in rems)
                            {
                                ctx.plug.ActionQueue.Remove(qa);
                                exx++;
                            }
                        }
                    }
                    if (exx > 0)
                    {
                        if (PrevActionsRefire == RefireEnum.Deny)
                        {
                            AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate("internal/Trigger/removefromqueuenorefire", "Removed {0} instance(s) of trigger '{1}' actions from queue, refire denied", exx, LogName));
                            return false;
                        }
                        else
                        {
                            AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate("internal/Trigger/removefromqueue", "Removed {0} instance(s) of trigger '{1}' actions from queue", exx, LogName));
                        }
                    }
                }
                else if (PrevActionsRefire == RefireEnum.Deny)
                {
                    int exx = 0;
                    lock (ctx.plug.ActionQueue)
                    {
                        var ixy = from ax in ctx.plug.ActionQueue
                                  where ax.ctx.trig.Id == Id
                                  select ax;
                        exx = ixy.Count();
                    }
                    if (exx > 0)
                    {
                        AddToLog(RealPlugin.DebugLevelEnum.Info, I18n.Translate("internal/Trigger/refiredenied", "{0} instance(s) of trigger '{1}' actions in queue, refire denied", exx, LogName));
                        return false;
                    }
                }
                QueueActions(ctx, curtime, mtx);
                return true;
            }
            catch (Exception ex)
            {
                AddToLog(RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/Trigger/firingexception", "Trigger '{0}' didn't fire due to exception: {1}", LogName, ex.ToString()));
            }
            return false;
        }

        public void TriggerContextLogger(object o, string msg)
        {
            AddToLog(RealPlugin.DebugLevelEnum.Verbose, msg);
        }

        public void CopySettingsTo(Trigger t)
        {
            t.Enabled = Enabled;
            t.Name = Name;
            t.Id = Id;
            t.RegularExpression = RegularExpression;

            // ─── Properties - Actions/Conditions ────────────────
            t.Actions.Clear();
            foreach (var a in Actions.OrderBy(x => x.OrderNumber))
            {
                var newAction = new Action();
                a.CopySettingsTo(newAction);
                newAction.ParentTrigger = t;
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

    }

}
