using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Triggernometry.Core.Actions;
using Triggernometry.Localization;
using static Triggernometry.Core.ActionBase;

namespace Triggernometry.Core
{

    public partial class RealPlugin
    {

        internal Thread ActionQueueThread = null;
        internal List<ActionInstance> ActionQueue = new List<ActionInstance>();
        // Cancellation must also account for actions already removed from the queue to execute.
        internal HashSet<ActionInstance> ExecutingActions = new HashSet<ActionInstance>();
        internal AutoResetEvent ActionUpdateEvent = null;
        private long curOrdinal = 0;
        internal object QueueProcessingLock = new object();
        internal bool QueueProcessing = true;


        private void InitActionQueue()
        {
            ActionQueueThread = new Thread(new ThreadStart(ActionThreadProc));
            ActionQueueThread.Name = "ActionQueueThread";
            ActionQueueThread.Start();
        }

        private void DeinitActionQueue()
        {
            if (ActionQueueThread != null)
            {
                if (ActionQueueThread.Join(5000) == false)
                {
                    ActionQueueThread.Abort();
                }
                ActionQueueThread = null;
            }
            ActionUpdateEvent?.Dispose();
            ActionUpdateEvent = null;
        }

        /// <summary>
        /// Remove queued actions from the global action queue based on the specified filter. <br/>
        /// If <paramref name="filter"/> is <c>null</c>, all queued actions are cleared. <br/>
        /// If any actions are removed, the action queue is re-sorted and the update event is signaled.
        /// </summary>
        /// <param name="filter">
        /// Select which queued actions to remove. <br/>
        /// If <c>null</c>, all actions will be removed.
        /// </param>
        /// <returns>The number of queued actions removed from the queue.</returns>

        internal int CancelQueuedActions(Func<ActionInstance, bool> filter = null)
        {
            int removedCount = 0;
            lock (ActionQueue)
            {
                var toRemove = ActionQueue.Where(qa => filter == null || filter(qa)).ToList();
                removedCount = toRemove.Count;
                foreach (var qa in toRemove)
                    ActionQueue.Remove(qa);

                foreach (var group in toRemove.Where(qa => qa.mutex != null)
                    .GroupBy(qa => new { qa.mutex, qa.ctx.id }))
                {
                    // Transfer the release to a surviving action, or finish the canceled chain.
                    var last = ExecutingActions.Concat(ActionQueue).LastOrDefault(qa => qa.mutex == group.Key.mutex && qa.ctx.id == group.Key.id);
                    if (last != null)
                    {
                        if (group.Any(qa => qa.releaseMutex)) last.releaseMutex = true;
                    }
                    else if (group.Any(qa => qa.releaseMutex || qa.ctx.Trigger.Sequential))
                        group.Key.mutex.Release(group.First().ctx);
                }
                if (removedCount > 0)
                    ActionUpdateEvent.Set();
            }
            return removedCount;
        }

        /// <summary>Builds execution-local continuations and queues the first action or parallel batch.</summary>
        internal ActionInstance QueueActions(Context ctx, DateTime startingFrom, IEnumerable<ActionBase> actions, bool sequential,
            MutexInformation mtx, Context.LoggerDelegate logger, ActionInstance loopParent = null)
        {
            var instances = new List<ActionInstance>();
            foreach (ActionBase action in actions.OrderBy(a => a.OrderNumber))
            {
                if (!action.Enabled) continue;
                if (!sequential || instances.Count == 0)
                    startingFrom = startingFrom.AddMilliseconds(ctx.EvaluateNumericExpression(logger, this, action.ExecutionDelayExpression));
                var instance = new ActionInstance(startingFrom, 0, mtx, action, ctx, false) { LoopParent = loopParent };
                if (sequential && instances.Count > 0) instances[instances.Count - 1].NextAction = instance;
                instances.Add(instance);
            }
            if (instances.Count == 0) return null;

            ActionInstance lastQueued = null;
            lock (ActionQueue)
            {
                try
                {
                    foreach (ActionInstance instance in instances)
                    {
                        if (QueueAction(instance)) lastQueued = instance;
                        if (sequential) break;
                    }
                }
                finally
                {
                    if (lastQueued != null)
                    {
                        // Transfer ownership before the worker can consume any of the new actions.
                        lastQueued.releaseMutex = loopParent?.releaseMutex ?? true;
                        if (loopParent != null) loopParent.releaseMutex = false;
                        if (loopParent != null) instances[sequential ? instances.Count - 1 : instances.IndexOf(lastQueued)].IsLoopTail = true;
                    }
                }
            }
            return lastQueued;
        }

        public void QueueAction(Context ctx, Trigger trig, MutexInformation mutex, ActionBase action, DateTime when, bool releaseMutex)
        {
            var instance = new ActionInstance(when, 0, mutex, action, ctx, releaseMutex);
            if (!QueueAction(instance, trig) && releaseMutex && mutex != null) mutex.Release(ctx);
        }

        /// <summary>Queues an instance without releasing a rejected continuation's mutex.</summary>
        internal bool QueueAction(ActionInstance instance, Trigger trig = null)
        {
            Context ctx = instance.ctx;
            ActionBase action = instance.act;
            trig = trig ?? ctx.Trigger;
            lock (ActionQueue)
            {
                if (action.RefireRequeue == false || action.RefireInterrupt == true)
                {
                    if (ActionQueue.Any(actionInstance => actionInstance.act.Id == action.Id))
                    {
                        if (action.RefireInterrupt == true)
                        {
                            int removedCount = ActionQueue.RemoveAll(actionInstance => actionInstance.act.Id == action.Id);
                            if (removedCount > 0)
                            {
                                action.AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/actionqueuerem", "Removed {0} instance(s) of trigger '{1}' action '{2}' from queue", removedCount, trig.LogName, action.Describe()));
                            }
                        }
                        if (action.RefireRequeue == false)
                        {
                            action.AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/actionqueuefail", "Trigger '{0}' action '{1}' not queued, refire requeue disabled", trig.LogName, action.Describe()));
                            return false;
                        }
                    }
                }
                lock (this) instance.ordinal = curOrdinal++;
                action.AddToLog(ctx, DebugLevelEnum.Info, I18n.Translate("internal/Plugin/actionqueued", "Queuing trigger '{0}' action '{1}' to {2} slot {3}", trig.LogName, action.Describe(), FormatDateTime(instance.when), instance.ordinal));
                Context tagContext = instance.LoopContext ?? ctx;
                var actionTag = tagContext.EvaluateStringExpression(null, tagContext, action.Tag);
                if (!string.IsNullOrWhiteSpace(actionTag))
                {
                    instance.ParsedTag = actionTag;
                }
                else
                {
                    var triggerTag = tagContext.EvaluateStringExpression(null, tagContext, tagContext.Trigger?.Tag);
                    if (!string.IsNullOrWhiteSpace(triggerTag)) instance.ParsedTag = triggerTag;
                }
                int index = ActionQueue.BinarySearch(instance);
                ActionQueue.Insert(index < 0 ? ~index : index, instance);
                ActionUpdateEvent.Set();
                return true;
            }
        }
        /// <summary>Converts legacy callers once before entering the typed action queue.</summary>
        [Obsolete("Use the typed action queue instead.")]
        public void QueueAction(Context ctx, Trigger trig, MutexInformation mutex, ActionOld actionOld, DateTime when, bool releaseMutex)
            => QueueAction(ctx, trig, mutex, actionOld.ConvertToNewAction(), when, releaseMutex);

        internal bool ReadyForOperation()
        {
            return mainform?.IsHandleCreated == true && ActInitedHook?.Invoke() == true;
        }

        /// <summary>Runs startup triggers, then dispatches actions until the plugin exits.</summary>
        internal void ActionThreadProc()
        {
            while (!ReadyForOperation()) Thread.Sleep(100);
            RunStartupTriggers();

            int timeout = Timeout.Infinite;
            WaitHandle[] waitHandles = { ExitEvent, ActionUpdateEvent };
            while (true)
            {
                int waitResult = WaitHandle.WaitAny(waitHandles, timeout);
                if (waitResult == 0) return;

                if (waitResult == WaitHandle.WaitTimeout) ExecuteNextQueuedAction();

                // Both queue changes and completed actions require a new wait deadline.
                timeout = GetActionQueueTimeout();
            }
        }

        /// <summary>Fires the configured startup trigger or the direct children of its folder.</summary>
        private void RunStartupTriggers()
        {
            if (cfg.StartupTriggerId == Guid.Empty) return;

            var logEvent = new LogEvent { Text = "", ZoneName = "", Timestamp = DateTime.Now };
            switch (cfg.StartupTriggerType)
            {
                case Configuration.StartupTriggerTypeEnum.Trigger:
                    Trigger trigger = GetTriggerById(cfg.StartupTriggerId, null);
                    if (trigger != null)
                        TestTrigger(trigger, logEvent, ActionTriggerOperation.ForceEnum.SkipAll);
                    break;

                case Configuration.StartupTriggerTypeEnum.Folder:
                    Folder folder = GetFolderById(cfg.StartupTriggerId, null);
                    if (folder == null) return;
                    foreach (Trigger child in folder.Triggers)
                        TestTrigger(child, logEvent, ActionTriggerOperation.ForceEnum.SkipAll);
                    break;
            }
        }

        /// <summary>Moves the first queued action to the executing set before dispatching it outside the lock.</summary>
        private void ExecuteNextQueuedAction()
        {
            ActionInstance instance;
            lock (ActionQueue)
            {
                if (ActionQueue.Count == 0) return;
                instance = ActionQueue[0];
                ActionQueue.RemoveAt(0);
                ExecutingActions.Add(instance);
            }
            instance.act.Execute(instance, instance.ctx);
        }

        /// <summary>Returns the next wait duration, or an indefinite wait while paused or empty.</summary>
        private int GetActionQueueTimeout()
        {
            lock (ActionQueue)
            {
                lock (QueueProcessingLock)
                {
                    if (!QueueProcessing) return Timeout.Infinite;
                }
                if (ActionQueue.Count == 0) return Timeout.Infinite;
                return Math.Max(0, (int)Math.Ceiling((ActionQueue[0].when - DateTime.Now).TotalMilliseconds));
            }
        }


        #region Mutex

        internal Dictionary<string, MutexInformation> mutexes = new Dictionary<string, MutexInformation>();

        internal class MutexTicket : IDisposable
        {

            internal Context ctx { get; set; }
            internal ManualResetEvent ev { get; set; }

            internal MutexTicket(Context c)
            {
                ctx = c;
                ev = new ManualResetEvent(false);
            }

            public void Dispose()
            {
                ev?.Dispose();
                ev = null;
            }

        }

        public class MutexInformation
        {

            private string name { get; set; }
            internal int refCount { get; set; }
            internal Context heldBy { get; set; }
            internal List<MutexTicket> acquireQueue { get; set; }

            internal MutexInformation(string name)
            {
                this.name = name;
                refCount = 0;
                heldBy = null;
                acquireQueue = new List<MutexTicket>();
            }

            internal MutexTicket QueueForAcquisition(Context ctx)
            {
                System.Diagnostics.Debug.WriteLine("### {0} - Queuing acquisition for context: {1}", name, ctx.ToString());
                MutexTicket m = new MutexTicket(ctx);
                lock (this)
                {
                    acquireQueue.Add(m);
                }
                System.Diagnostics.Debug.WriteLine("### {0} - Queued acquisition {1} for context: {2}", name, m.GetHashCode(), ctx.ToString());
                return m;
            }

            internal void Acquire(Context ctx)
            {
                System.Diagnostics.Debug.WriteLine("### {0} - Acquiring for context: {1}", name, ctx.ToString());
                using (MutexTicket m = QueueForAcquisition(ctx))
                {
                    Acquire(ctx, m);
                    System.Diagnostics.Debug.WriteLine("### {0} - Acquired {1} for context: {2}", name, m.GetHashCode(), ctx.ToString());
                }
            }

            internal void Acquire(Context ctx, MutexTicket m)
            {
                DateTime start = DateTime.Now;
                string ownername = "";
                bool autoget = false;
                System.Diagnostics.Debug.WriteLine("### {0} - Acquisition {1} pending stage 1 for context: {2}", name, m.GetHashCode(), ctx.ToString());
                lock (this)
                {
                    if (heldBy == null)
                    {
                        MutexTicket first = acquireQueue.ElementAt(0);
                        if (first == m)
                        {
                            m.ev.Set();
                            refCount++;
                            heldBy = ctx;
                            autoget = true;
                        }
                    }
                    else if (heldBy.id == ctx.id)
                    {
                        m.ev.Set();
                        refCount++;
                        autoget = true;
                    }
                    else
                    {
                    }
                    if (autoget == false)
                    {
                        ownername = heldBy != null ? heldBy.ToString() : null;
                    }
                }
                while (m.ev.WaitOne(5000) == false)
                {
                    if (ctx.Plugin != null)
                    {
                        ctx.Plugin.FilteredAddToLog(DebugLevelEnum.Warning, I18n.Translate("internal/Plugin/mutexdelayed", "Context '{0}' has been waiting for mutex '{1}' on {2} for {3} ms, current owner is '{4}'", ctx.ToString(), name, m.GetHashCode(), (DateTime.Now - start).TotalMilliseconds, ownername));
                    }
                }
                System.Diagnostics.Debug.WriteLine("### {0} - Acquisition {1} pending stage 2 for context: {2}", name, m.GetHashCode(), ctx.ToString());
                lock (this)
                {
                    System.Diagnostics.Debug.WriteLine("### {0} - Acquisition {1} pending stage 3 for context: {2}", name, m.GetHashCode(), ctx.ToString());
                    acquireQueue.Remove(m);
                    if (autoget == false)
                    {
                        if (heldBy != null)
                        {
                            throw new InvalidOperationException(I18n.Translate("internal/Plugin/invalidacquiremutex", "Tried to acquire mutex '{0}' belonging to context '{1}' on context '{2}'", name, heldBy.ToString(), ctx.ToString()));
                        }
                        heldBy = ctx;
                        refCount++;
                        System.Diagnostics.Debug.WriteLine("### {0} - New acquisition {1} for context: {2}", name, m.GetHashCode(), ctx.ToString());
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("### {0} - Autoget acquisition {1} for context: {2}", name, m.GetHashCode(), ctx.ToString());
                    }
                }
                System.Diagnostics.Debug.WriteLine("### {0} - Acquisition {1} pending stage 4 for context: {2}", name, m.GetHashCode(), ctx.ToString());
            }

            internal void Release(Context ctx)
            {
                System.Diagnostics.Debug.WriteLine("### {0} - Releasing for context: {1}", name, ctx.ToString());
                lock (this)
                {
                    if (heldBy == null || heldBy.id != ctx.id)
                    {
                        throw new InvalidOperationException(I18n.Translate("internal/Plugin/releaseunownedmutex", "Tried to release unowned mutex '{0}' from context '{1}'", name, ctx.ToString()));
                    }
                    refCount--;
                    if (refCount == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("### {0} - Fully released by context: {1}", name, ctx.ToString());
                        heldBy = null;
                        WakeupNext();
                    }
                }
                System.Diagnostics.Debug.WriteLine("### {0} - Released for context: {1}", name, ctx.ToString());
            }

            internal void ForceRelease()
            {
                System.Diagnostics.Debug.WriteLine("### {0} - Releasing by force", name);
                lock (this)
                {
                    refCount = 0;
                    heldBy = null;
                    WakeupNext();
                }
                System.Diagnostics.Debug.WriteLine("### {0} - Released by force", name);
            }

            private void WakeupNext()
            {
                if (acquireQueue.Count > 0)
                {
                    MutexTicket m = acquireQueue.ElementAt(0);
                    System.Diagnostics.Debug.WriteLine("### {0} - Waking up next context in queue : {1}", name, m.ctx.ToString());
                    m.ev.Set();
                }
            }

        }

        internal MutexInformation GetMutex(string name)
        {
            MutexInformation mi = null;
            lock (mutexes)
            {
                if (mutexes.ContainsKey(name) == false)
                {
                    mutexes[name] = new MutexInformation(name);
                }
                mi = mutexes[name];
            }
            return mi;
        }

        #endregion
    }
}
