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
        // UI 写入、队列线程读取；状态改变后通过 ActionUpdateEvent 唤醒等待。
        internal volatile bool QueueProcessing = true;


        private void InitActionQueue()
        {
            ActionQueueThread = new Thread(new ThreadStart(ActionThreadProc))
            {
                Name = "ActionQueueThread"
            };
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

        /// <summary>取消符合筛选条件的执行实例及其后续动作；空筛选表示全部可打断动作。</summary>
        internal int CancelQueuedActions(Func<ActionInstance, bool> filter = null, bool queuedOnly = false, bool requiresTag = false)
        {
            lock (ActionQueue)
            {
                List<ActionInstance> toRemove;
                int cancelledCount;
                if (queuedOnly)
                {
                    toRemove = RemoveQueuedActions(filter);
                    cancelledCount = toRemove.Count;
                    if (cancelledCount == 0)
                        return 0;
                }
                else
                {
                    cancelledCount = CancelActionChains(filter, requiresTag);
                    if (cancelledCount == 0)
                        return 0;
                    toRemove = RemoveCancelledActionsAndQueueNext();
                }

                TransferOrReleaseCancelledActionMutexes(toRemove);
                ActionUpdateEvent.Set();
                return cancelledCount;
            }
        }

        /// <summary>仅移除匹配的可打断排队项，返回已移除的实例；不检查执行中或后续动作。调用方须持有队列锁。</summary>
        private List<ActionInstance> RemoveQueuedActions(Func<ActionInstance, bool> filter)
        {
            var toRemove = ActionQueue.Where(action => !action.Action.Uninterruptible && (filter == null || filter(action))).ToList();
            if (toRemove.Count == 0)
                return toRemove;
            var removedActions = new HashSet<ActionInstance>(toRemove);
            ActionQueue.RemoveAll(removedActions.Contains);
            return toRemove;
        }

        /// <summary>遍历排队、执行中及其后续动作，记录取消请求并返回新标记为取消的实例数。调用方须持有队列锁。</summary>
        private int CancelActionChains(Func<ActionInstance, bool> filter, bool requiresTag)
        {
            int cancelledCount = 0;
            var pending = new Stack<ActionInstance>();
            var requests = new List<(Func<ActionInstance, bool> Filter, bool RequiresTag)> { (filter, requiresTag) };
            void CancelInstance(ActionInstance action)
            {
                if (action.Cancel(filter, requiresTag, requests))
                    cancelledCount++;
                if (action.NextAction != null)
                    pending.Push(action.NextAction);
                if (action.LoopParent != null)
                    pending.Push(action.LoopParent);
            }
            // 队列与执行集合本身没有重复项，仅为后续动作图去重。
            foreach (var action in ActionQueue)
                CancelInstance(action);
            foreach (var action in ExecutingActions)
                CancelInstance(action);
            if (pending.Count > 0)
            {
                var visited = new HashSet<ActionInstance>();
                while (pending.Count > 0)
                {
                    var action = pending.Pop();
                    if (visited.Add(action))
                        CancelInstance(action);
                }
            }
            return cancelledCount;
        }

        /// <summary>移除已取消的排队项，续排未取消或不可打断的后续动作，并返回已移除的实例。调用方须持有队列锁。</summary>
        private List<ActionInstance> RemoveCancelledActionsAndQueueNext()
        {
            var toRemove = ActionQueue.Where(action => action.IsCancelled).ToList();
            ActionQueue.RemoveAll(action => action.IsCancelled);
            foreach (var action in toRemove)
                action.QueueNext(action.Context);
            return toRemove;
        }

        /// <summary>将已移除动作的互斥锁释放责任移交给剩余动作，或在动作链结束时释放。调用方须持有队列锁。</summary>
        private void TransferOrReleaseCancelledActionMutexes(List<ActionInstance> toRemove)
        {
            foreach (var group in toRemove.Where(qa => qa.Mutex != null)
                .GroupBy(qa => new { qa.Mutex, qa.Context.id }))
            {
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
        }

        /// <summary>Builds execution-local continuations and queues the first action or parallel batch.</summary>
        internal ActionInstance QueueActions(Context ctx, DateTime startingFrom, IEnumerable<ActionBase> actions, bool sequential,
            MutexInfo mtx, Context.LoggerDelegate logger, ActionInstance loopParent = null)
        {
            var instances = new List<ActionInstance>();
            foreach (ActionBase action in actions)
            {
                if (!action.Enabled)
                    continue;
                if (!sequential || instances.Count == 0)
                    startingFrom = startingFrom.AddMilliseconds(ctx.EvaluateNumericExpression(logger, this, action.ExecDelay));
                var instance = new ActionInstance(startingFrom, 0, mtx, action, ctx, false) { LoopParent = loopParent };
                if (sequential && instances.Count > 0)
                    instances[instances.Count - 1].NextAction = instance;
                instances.Add(instance);
            }
            if (instances.Count == 0)
                return null;

            ActionInstance lastQueued = null;
            lock (ActionQueue)
            {
                if (loopParent?.IsCancelled == true)
                    return null;
                foreach (ActionInstance action in instances)
                    action.InheritCancellation(loopParent);
                try
                {
                    foreach (ActionInstance action in instances)
                    {
                        if (!action.PrepareToQueue())
                            continue;
                        if (sequential && action != instances[0])
                            action.ExecuteAt = DateTime.Now.AddMilliseconds(ctx.EvaluateNumericExpression(logger, this, action.Action.ExecDelay));
                        if (QueueAction(action))
                            lastQueued = action;
                        if (sequential)
                            break;
                    }
                }
                finally
                {
                    if (lastQueued != null)
                    {
                        // Transfer ownership before the worker can consume any of the new actions.
                        lastQueued.ShouldReleaseMutex = loopParent?.ShouldReleaseMutex ?? true;
                        if (loopParent != null)
                        {
                            loopParent.ShouldReleaseMutex = false;
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
            if (!QueueAction(instance, trig) && releaseMutex && mutex != null)
                mutex.Release(ctx);
        }

        /// <summary>Queues an instance without releasing a rejected continuation's mutex.</summary>
        internal bool QueueAction(ActionInstance instance, Trigger trig = null)
        {
            Context ctx = instance.Context;
            ActionBase action = instance.Action;
            trig = trig ?? ctx.Trigger;
            lock (ActionQueue)
            {
                if (!instance.PrepareToQueue())
                    return false;
                var logLevel = action.GetDebugLevel(ctx);
                bool shouldLogVerbose = logLevel >= DebugLevelEnum.Verbose;
                bool shouldLogInfo = logLevel >= DebugLevelEnum.Info;
                bool canReplaceQueued = action.ReplaceQueued && !action.Uninterruptible;
                bool removedQueuedDuplicate = false;
                if (canReplaceQueued)
                {
                    int removedCount = CancelQueuedActions(a => a.Action.Id == action.Id, queuedOnly: true);
                    removedQueuedDuplicate = removedCount > 0;
                    if (removedQueuedDuplicate && shouldLogVerbose)
                    {
                        action.AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/actionqueuerem", "Removed {0} instance(s) of trigger '{1}' action '{2}' from queue", removedCount, trig?.LogName ?? "(null)", action.Describe()));
                    }
                }
                // 禁止重复排队同时考虑本次移除的实例与仍受保护的实例。
                bool shouldRejectDuplicate = action.PreventDuplicateQueue &&
                    (removedQueuedDuplicate || ActionQueue.Any(a => a.Action.Id == action.Id));
                if (shouldRejectDuplicate)
                {
                    if (shouldLogVerbose)
                        action.AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Plugin/actionqueuefail", "Trigger '{0}' action '{1}' not queued, refire requeue disabled", trig?.LogName ?? "(null)", action.Describe()));
                    return false;
                }
                // already locked by ActionQueue
                instance.QueueSequence = curOrdinal++;
                if (shouldLogInfo)
                    action.AddToLog(ctx, DebugLevelEnum.Info, I18n.Translate("internal/Plugin/actionqueued", "Queuing trigger '{0}' action '{1}' to {2} slot {3}", trig?.LogName ?? "(null)", action.Describe(), FormatDateTime(instance.ExecuteAt), instance.QueueSequence));
                instance.ParseTag();
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
                if (ExitEvent.WaitOne(100))
                    return;
            RunStartupTriggers();

            int timeout = Timeout.Infinite;
            WaitHandle[] waitHandles = { ExitEvent, ActionUpdateEvent };
            while (true)
            {
                int waitResult = WaitHandle.WaitAny(waitHandles, timeout);
                if (waitResult == 0)
                    return;

                ExecuteNextQueuedAction();

                // Both queue changes and completed actions require a new wait deadline.
                timeout = GetActionQueueTimeout();
            }
        }

        /// <summary>Fires the configured startup trigger or the direct children of its folder.</summary>
        private void RunStartupTriggers()
        {
            if (Config.StartupTriggerId == Guid.Empty)
                return;

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
                    if (folder == null)
                        return;
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
                if (ActionQueue.Count == 0)
                    return;
                if (!QueueProcessing)
                    return;
                instance = ActionQueue[0];
                // 唤醒可能来自队列更新或超长延迟的分段等待（等待时间超过 int 上限，~25 天）
                // 出队前必须确认已到期
                if (instance.ExecuteAt > DateTime.Now)
                    return;
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
                if (!QueueProcessing)
                    return Timeout.Infinite;
                if (ActionQueue.Count == 0)
                    return Timeout.Infinite;
                double remainingMilliseconds = (ActionQueue[0].ExecuteAt - DateTime.Now).TotalMilliseconds;
                return (int)Math.Min(int.MaxValue, Math.Max(0, Math.Ceiling(remainingMilliseconds)));
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
                        MutexTicket first = AcquireQueue[0];
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
                    if (autoget == false)
                    {
                        ownername = OwnerContext != null ? OwnerContext.ToString() : null;
                    }
                }
                while (m.ResetEvent.WaitOne(5000) == false)
                {
                    if (ctx.Plugin != null)
                    {
                        ctx.Plugin.FilteredAddToLog(DebugLevelEnum.Warning, I18n.Translate("internal/Plugin/mutexdelayed", "Context '{0}' has been waiting for mutex '{1}' on {2} for {3} ms, current owner is '{4}'", ctx.ToString(), Name, m.GetHashCode(), (DateTime.Now - start).TotalMilliseconds, ownername), ctx.Trigger);
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
                    MutexTicket m = AcquireQueue[0];
                    System.Diagnostics.Debug.WriteLine("### {0} - Waking up next context in queue : {1}", Name, m.Context.ToString());
                    m.ResetEvent.Set();
                }
            }

        }

        internal MutexInfo GetMutex(string name)
        {
            lock (mutexes)
            {
                if (!mutexes.TryGetValue(name, out MutexInfo mutex))
                {
                    mutex = new MutexInfo(name);
                    mutexes.Add(name, mutex);
                }
                return mutex;
            }
        }

        #endregion
    }
}
