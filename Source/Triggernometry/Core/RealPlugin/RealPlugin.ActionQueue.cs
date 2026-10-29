using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Triggernometry.Core.Actions;
using Triggernometry.Localization;

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

        /// <summary>取消允许打断且符合筛选条件的排队动作，并转交或释放互斥锁；空筛选表示全部可打断动作。</summary>
        internal int CancelQueuedActions(Func<ActionInstance, bool> filter = null)
        {
            int removedCount = 0;
            lock (ActionQueue)
            {
                var toRemove = ActionQueue.Where(qa => !qa.Action.Uninterruptible && (filter == null || filter(qa))).ToList();
                removedCount = toRemove.Count;
                foreach (var qa in toRemove)
                    ActionQueue.Remove(qa);

                foreach (var group in toRemove.Where(qa => qa.Mutex != null)
                    .GroupBy(qa => new { qa.Mutex, qa.Context.id }))
                {
                    // Transfer the release to a surviving action, or finish the canceled chain.
                    var last = ExecutingActions.Concat(ActionQueue)
                        .LastOrDefault(action => action.Mutex == group.Key.Mutex && action.Context.id == group.Key.id);
                    if (last != null)
                    {
                        if (group.Any(action => action.ShouldReleaseMutex))
                            last.ShouldReleaseMutex = true;
                    }
                    else if (group.Any(action => action.ShouldReleaseMutex || action.Context.Trigger.Sequential))
                        group.Key.Mutex.Release(group.First().Context);
                }
                if (removedCount > 0)
                    ActionUpdateEvent.Set();
            }
            return removedCount;
        }

        /// <summary>Builds execution-local continuations and queues the first action or parallel batch.</summary>
        internal ActionInstance QueueActions(Context ctx, DateTime startingFrom, IEnumerable<ActionBase> actions, bool sequential,
            MutexInfo mtx, Context.LoggerDelegate logger, ActionInstance loopParent = null)
        {
            var instances = new List<ActionInstance>();
            foreach (ActionBase action in actions)
            {
                if (!action.Enabled) continue;
                if (!sequential || instances.Count == 0)
                    startingFrom = startingFrom.AddMilliseconds(ctx.EvaluateNumericExpression(logger, this, action.ExecDelay));
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
                    foreach (ActionInstance action in instances)
                    {
                        if (QueueAction(action)) lastQueued = action;
                        if (sequential) break;
                    }
                }
                finally
                {
                    if (lastQueued != null)
                    {
                        // Transfer ownership before the worker can consume any of the new actions.
                        lastQueued.ShouldReleaseMutex = loopParent?.ShouldReleaseMutex ?? true;
                        if (loopParent != null) 
                            loopParent.ShouldReleaseMutex = false;
                        if (loopParent != null)
                        {
                            var index = sequential ? instances.Count - 1 : instances.IndexOf(lastQueued);
                            instances[index].IsLoopTail = true;
                        }
                            
                    }
                }
            }
            return lastQueued;
        }

        public void QueueAction(Context ctx, Trigger trig, MutexInfo mutex, ActionBase action, DateTime when, bool releaseMutex)
        {
            var instance = new ActionInstance(when, 0, mutex, action, ctx, releaseMutex);
            if (!QueueAction(instance, trig) && releaseMutex && mutex != null) mutex.Release(ctx);
        }

        /// <summary>Queues an instance without releasing a rejected continuation's mutex.</summary>
        internal bool QueueAction(ActionInstance instance, Trigger trig = null)
        {
            Context ctx = instance.Context;
            ActionBase action = instance.Action;
            trig = trig ?? ctx.Trigger;
            lock (ActionQueue)
            {
                var logLevel = action.GetDebugLevel(ctx);
                bool shouldLogVerbose = logLevel >= DebugLevelEnum.Verbose;
                bool shouldLogInfo = logLevel >= DebugLevelEnum.Info;
                bool canReplaceQueued = action.ReplaceQueued && !action.Uninterruptible;
                bool shouldCheckDuplicates = action.PreventDuplicateQueue || canReplaceQueued;
                bool hasQueuedDuplicate = shouldCheckDuplicates && ActionQueue.Any(a => a.Action.Id == action.Id);

                if (hasQueuedDuplicate)
                {
                    if (canReplaceQueued)
                    {
                        int removedCount = CancelQueuedActions(a => a.Action.Id == action.Id);
                        if (removedCount > 0 && shouldLogVerbose)
                        {
                            action.AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/actionqueuerem", "Removed {0} instance(s) of trigger '{1}' action '{2}' from queue", removedCount, trig?.LogName ?? "(null)", action.Describe()));
                        }
                    }
                    if (action.PreventDuplicateQueue)
                    {
                        if (shouldLogVerbose)
                            action.AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/actionqueuefail", "Trigger '{0}' action '{1}' not queued, refire requeue disabled", trig?.LogName ?? "(null)", action.Describe()));
                        return false;
                    }
                }
                // already locked by ActionQueue
                instance.QueueSequence = curOrdinal++;
                if (shouldLogInfo)
                    action.AddToLog(ctx, DebugLevelEnum.Info, I18n.Translate("internal/Plugin/actionqueued", "Queuing trigger '{0}' action '{1}' to {2} slot {3}", trig?.LogName ?? "(null)", action.Describe(), FormatDateTime(instance.ExecuteAt), instance.QueueSequence));
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
        public void QueueAction(Context ctx, Trigger trig, MutexInfo mutex, ActionOld actionOld, DateTime when, bool releaseMutex)
            => QueueAction(ctx, trig, mutex, actionOld.ConvertToNewAction(), when, releaseMutex);

        internal bool ReadyForOperation()
        {
            return mainform?.IsHandleCreated == true && ActInitedHook?.Invoke() == true;
        }

        /// <summary>Runs startup triggers, then dispatches actions until the plugin exits.</summary>
        internal void ActionThreadProc()
        {
            // 初始化尚未完成时，也必须能响应插件退出。
            while (!ReadyForOperation())
                if (ExitEvent.WaitOne(100)) return;
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
            if (Config.StartupTriggerId == Guid.Empty) return;

            var logEvent = new LogEvent { Text = "", ZoneName = "", Timestamp = DateTime.Now };
            switch (Config.StartupTriggerType)
            {
                case Configuration.StartupTriggerTypeEnum.Trigger:
                    Trigger trigger = GetTriggerById(Config.StartupTriggerId, null);
                    if (trigger != null)
                        TestTrigger(trigger, logEvent, ActionTriggerOperation.ForceEnum.SkipAll);
                    break;

                case Configuration.StartupTriggerTypeEnum.Folder:
                    Folder folder = GetFolderById(Config.StartupTriggerId, null);
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
            // 实例在任务完成后自行续排和收尾，队列线程继续处理其他动作。
            _ = instance.Dispatch();
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
                return Math.Max(0, (int)Math.Ceiling((ActionQueue[0].ExecuteAt - DateTime.Now).TotalMilliseconds));
            }
        }


        #region Mutex

        internal Dictionary<string, MutexInfo> mutexes = new Dictionary<string, MutexInfo>();

        internal class MutexTicket : IDisposable
        {

            internal Context Context { get; set; }
            internal ManualResetEvent ResetEvent { get; set; }

            internal MutexTicket(Context c)
            {
                Context = c;
                ResetEvent = new ManualResetEvent(false);
            }

            public void Dispose()
            {
                ResetEvent?.Dispose();
                ResetEvent = null;
            }

        }

        public class MutexInfo
        {

            private string Name { get; set; }
            internal int RefCount { get; set; } = 0;
            internal Context OwnerContext { get; set; } = null;
            internal List<MutexTicket> AcquireQueue { get; set; } = new List<MutexTicket>();

            internal MutexInfo(string name)
            {
                Name = name;
            }

            internal MutexTicket QueueForAcquisition(Context ctx)
            {
                // System.Diagnostics.Debug.WriteLine("### {0} - Queuing acquisition for context: {1}", Name, ctx.ToString());
                MutexTicket m = new MutexTicket(ctx);
                lock (this)
                {
                    AcquireQueue.Add(m);
                }
                // System.Diagnostics.Debug.WriteLine("### {0} - Queued acquisition {1} for context: {2}", Name, m.GetHashCode(), ctx.ToString());
                return m;
            }

            internal void Acquire(Context ctx)
            {
                // System.Diagnostics.Debug.WriteLine("### {0} - Acquiring for context: {1}", Name, ctx.ToString());
                using (MutexTicket m = QueueForAcquisition(ctx))
                {
                    Acquire(ctx, m);
                    // System.Diagnostics.Debug.WriteLine("### {0} - Acquired {1} for context: {2}", Name, m.GetHashCode(), ctx.ToString());
                }
            }

            internal void Acquire(Context ctx, MutexTicket m)
            {
                DateTime start = DateTime.Now;
                string ownername = "";
                bool autoget = false;
                System.Diagnostics.Debug.WriteLine("### {0} - Acquisition {1} pending stage 1 for context: {2}", Name, m.GetHashCode(), ctx.ToString());
                lock (this)
                {
                    if (OwnerContext == null)
                    {
                        MutexTicket first = AcquireQueue.ElementAt(0);
                        if (first == m)
                        {
                            m.ResetEvent.Set();
                            RefCount++;
                            OwnerContext = ctx;
                            autoget = true;
                        }
                    }
                    else if (OwnerContext.id == ctx.id)
                    {
                        m.ResetEvent.Set();
                        RefCount++;
                        autoget = true;
                    }
                    else
                    {
                    }
                    if (autoget == false)
                    {
                        ownername = OwnerContext != null ? OwnerContext.ToString() : null;
                    }
                }
                while (m.ResetEvent.WaitOne(5000) == false)
                {
                    if (ctx.Plugin != null)
                    {
                        ctx.Plugin.FilteredAddToLog(DebugLevelEnum.Warning, I18n.Translate("internal/Plugin/mutexdelayed", "Context '{0}' has been waiting for mutex '{1}' on {2} for {3} ms, current owner is '{4}'", ctx.ToString(), Name, m.GetHashCode(), (DateTime.Now - start).TotalMilliseconds, ownername));
                    }
                }
                System.Diagnostics.Debug.WriteLine("### {0} - Acquisition {1} pending stage 2 for context: {2}", Name, m.GetHashCode(), ctx.ToString());
                lock (this)
                {
                    System.Diagnostics.Debug.WriteLine("### {0} - Acquisition {1} pending stage 3 for context: {2}", Name, m.GetHashCode(), ctx.ToString());
                    AcquireQueue.Remove(m);
                    if (autoget == false)
                    {
                        if (OwnerContext != null)
                        {
                            throw new InvalidOperationException(I18n.Translate("internal/Plugin/invalidacquiremutex", "Tried to acquire mutex '{0}' belonging to context '{1}' on context '{2}'", Name, OwnerContext.ToString(), ctx.ToString()));
                        }
                        OwnerContext = ctx;
                        RefCount++;
                        System.Diagnostics.Debug.WriteLine("### {0} - New acquisition {1} for context: {2}", Name, m.GetHashCode(), ctx.ToString());
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("### {0} - Autoget acquisition {1} for context: {2}", Name, m.GetHashCode(), ctx.ToString());
                    }
                }
                System.Diagnostics.Debug.WriteLine("### {0} - Acquisition {1} pending stage 4 for context: {2}", Name, m.GetHashCode(), ctx.ToString());
            }

            internal void Release(Context ctx)
            {
                // System.Diagnostics.Debug.WriteLine("### {0} - Releasing for context: {1}", Name, ctx.ToString());
                lock (this)
                {
                    if (OwnerContext == null || OwnerContext.id != ctx.id)
                    {
                        throw new InvalidOperationException(I18n.Translate("internal/Plugin/releaseunownedmutex", "Tried to release unowned mutex '{0}' from context '{1}'", Name, ctx.ToString()));
                    }
                    RefCount--;
                    if (RefCount == 0)
                    {
                        System.Diagnostics.Debug.WriteLine("### {0} - Fully released by context: {1}", Name, ctx.ToString());
                        OwnerContext = null;
                        WakeupNext();
                    }
                }
                // System.Diagnostics.Debug.WriteLine("### {0} - Released for context: {1}", Name, ctx.ToString());
            }

            internal void ForceRelease()
            {
                // System.Diagnostics.Debug.WriteLine("### {0} - Releasing by force", Name);
                lock (this)
                {
                    RefCount = 0;
                    OwnerContext = null;
                    WakeupNext();
                }
                // System.Diagnostics.Debug.WriteLine("### {0} - Released by force", Name);
            }

            private void WakeupNext()
            {
                if (AcquireQueue.Count > 0)
                {
                    MutexTicket m = AcquireQueue.ElementAt(0);
                    System.Diagnostics.Debug.WriteLine("### {0} - Waking up next context in queue : {1}", Name, m.Context.ToString());
                    m.ResetEvent.Set();
                }
            }

        }

        internal MutexInfo GetMutex(string name)
        {
            MutexInfo mi = null;
            lock (mutexes)
            {
                if (mutexes.ContainsKey(name) == false)
                {
                    mutexes[name] = new MutexInfo(name);
                }
                mi = mutexes[name];
            }
            return mi;
        }

        #endregion
    }
}
