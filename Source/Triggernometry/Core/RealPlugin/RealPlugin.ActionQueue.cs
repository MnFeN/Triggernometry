using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.UI.CustomControls;
using Triggernometry.Localization;
using Triggernometry.Utilities;
using Triggernometry.Core.Variables;
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

        internal ActionBase QueueActions(Context ctx, DateTime startingFrom, IEnumerable<ActionBase> actions, bool sequential, MutexInformation mtx, Context.LoggerDelegate logger)
        {
            ActionBase lastAction = null;
            IEnumerable<ActionBase> sortedActions = actions.OrderBy(a => a.OrderNumber);
            if (sequential == false)
            {
                // Reuse the sorted result for both the final-action lookup and enqueueing.
                sortedActions = sortedActions.ToArray();
                var finalAction = sortedActions.LastOrDefault(a => a.Enabled);
                foreach (ActionBase action in sortedActions)
                {
                    if (action.Enabled == true)
                    {
                        startingFrom = startingFrom.AddMilliseconds(ctx.EvaluateNumericExpression(logger, this, action.ExecutionDelayExpression));
                        QueueAction(ctx, ctx.Trigger, mtx, action, startingFrom, finalAction == action);
                        lastAction = action;
                    }
                }
            }
            else
            {
                // Sequential execution only needs one traversal to build the action chain.
                ActionBase prev = null;
                ActionBase first = null;
                foreach (ActionBase action in sortedActions)
                {
                    if (action.Enabled == false)
                    {
                        continue;
                    }
                    lastAction = action;
                    if (prev != null)
                    {
                        prev.NextAction = action;
                    }
                    else
                    {
                        first = action;
                        startingFrom = startingFrom.AddMilliseconds(ctx.EvaluateNumericExpression(logger, this, action.ExecutionDelayExpression));
                    }
                    prev = action;
                }
                // The new tail must not retain a successor from an earlier queue operation.
                if (prev != null) prev.NextAction = null;
                if (first != null)
                {
                    QueueAction(ctx, ctx.Trigger, mtx, first, startingFrom, false);
                }
            }
            return lastAction;
        }

        public void QueueAction(Context ctx, Trigger trig, MutexInformation mutex, ActionBase action, DateTime when, bool releaseMutex)
        {
            lock (ActionQueue) // verified
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
                            if (releaseMutex == true && mutex != null)
                            {
                                mutex.Release(ctx);
                            }
                            return;
                        }
                    }
                }
                long newOrdinal;
                lock (this)
                {
                    newOrdinal = curOrdinal;
                    curOrdinal++;
                }
                action.AddToLog(ctx, DebugLevelEnum.Info, I18n.Translate("internal/Plugin/actionqueued", "Queuing trigger '{0}' action '{1}' to {2} slot {3}", trig.LogName, action.Describe(), FormatDateTime(when), newOrdinal));
                var queuedAction = new ActionInstance(when, newOrdinal, mutex, action, ctx, releaseMutex);
                var actionTag = ctx.EvaluateStringExpression(null, ctx, action.Tag);
                if (!string.IsNullOrWhiteSpace(actionTag))
                {
                    queuedAction.ParsedTag = actionTag;
                }
                else
                {
                    var triggerTag = ctx.EvaluateStringExpression(null, ctx, ctx?.Trigger?.Tag);
                    if (!string.IsNullOrWhiteSpace(triggerTag))
                        queuedAction.ParsedTag = triggerTag;
                }
                int index = ActionQueue.BinarySearch(queuedAction);
                ActionQueue.Insert(index < 0 ? ~index : index, queuedAction);
                ActionUpdateEvent.Set();
            }
        }

        /// <summary>Converts legacy callers once before entering the typed action queue.</summary>
        public void QueueAction(Context ctx, Trigger trig, MutexInformation mutex, ActionOld actionOld, DateTime when, bool releaseMutex)
            => QueueAction(ctx, trig, mutex, actionOld.ConvertToNewAction(), when, releaseMutex);

        internal bool ReadyForOperation()
        {
            return mainform?.IsHandleCreated == true && ActInitedHook?.Invoke() == true;
        }

        internal void ActionThreadProc()
        {
            int timeout = Timeout.Infinite;
            WaitHandle[] wh = new WaitHandle[2];
            wh[0] = ExitEvent;
            wh[1] = ActionUpdateEvent;
            if (ReadyForOperation() == false)
            {
                //FilteredAddToLog(DebugLevelEnum.Warning, I18n.Translate("internal/Plugin/invalidwindowhandle", "No valid window handle yet, waiting for it to be created"));
                do
                {
                    Thread.Sleep(100);
                } while (ReadyForOperation() == false);
            }
            if (cfg.StartupTriggerId != Guid.Empty)
            {
                if (cfg.StartupTriggerType == Configuration.StartupTriggerTypeEnum.Trigger)
                {
                    Trigger t = GetTriggerById(cfg.StartupTriggerId, null);
                    if (t != null)
                    {
                        LogEvent le = new LogEvent();
                        le.Text = "";
                        le.ZoneName = "";
                        le.Timestamp = DateTime.Now;
                        TestTrigger(t, le, ActionOld.TriggerForceTypeEnum.SkipAll);
                    }
                }
                if (cfg.StartupTriggerType == Configuration.StartupTriggerTypeEnum.Folder)
                {
                    Folder f = GetFolderById(cfg.StartupTriggerId, null);
                    if (f != null)
                    {
                        LogEvent le = new LogEvent();
                        le.Text = "";
                        le.ZoneName = "";
                        le.Timestamp = DateTime.Now;
                        foreach (Trigger tx in f.Triggers)
                        {
                            TestTrigger(tx, le, ActionOld.TriggerForceTypeEnum.SkipAll);
                        }
                    }
                }
            }
            while (true)
            {
                switch (WaitHandle.WaitAny(wh, timeout))
                {
                    case WaitHandle.WaitTimeout:
                        {
                            ActionInstance tp = null;
                            lock (ActionQueue) // verified
                            {
                                if (ActionQueue.Count > 0)
                                {
                                    tp = ActionQueue[0];
                                    ActionQueue.RemoveAt(0);
                                    ExecutingActions.Add(tp);
                                }
                            }
                            if (tp != null)
                            {
                                tp.act.Execute(tp, tp.ctx);
                                goto case 1;
                            }
                            else
                            {
                                timeout = Timeout.Infinite;
                                continue;
                            }
                        }
                    case 0:
                        {
                            return;
                        }
                    case 1:
                        {
                            lock (ActionQueue) // verified
                            {
                                lock (QueueProcessingLock) // verified
                                {
                                    if (QueueProcessing == false)
                                    {
                                        timeout = Timeout.Infinite;
                                        break;
                                    }
                                }
                                if (ActionQueue.Count > 0)
                                {
                                    timeout = (int)Math.Ceiling((ActionQueue[0].when - DateTime.Now).TotalMilliseconds);
                                    if (timeout < 0)
                                    {
                                        timeout = 0;
                                    }
                                }
                                else
                                {
                                    timeout = Timeout.Infinite;
                                }
                            }
                        }
                        break;
                }
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
                if (ev != null)
                {
                    ev.Dispose();
                    ev = null;
                }
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
