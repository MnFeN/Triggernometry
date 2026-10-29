using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Triggernometry.Core
{

    public partial class RealPlugin
    {

        internal bool DisableLogging = false;
        private long logSequence;

        internal Dictionary<DebugLevelEnum, Queue<InternalLog>> log = Enum.GetValues(typeof(DebugLevelEnum))
            .Cast<DebugLevelEnum>()
            .ToDictionary(level => level, level => new Queue<InternalLog>());

        public enum DebugLevelEnum
        {
            None,
            Error,
            Warning,
            Custom,
            Custom2,
            Info,
            Verbose,
            Inherit
        }

        internal void ClearLog()
        {
            foreach (var queue in log.Values)
            {
                lock (queue) queue.Clear();
            }
            ui?.ClearErrorCount();
        }

        internal void UnfilteredAddToLog(DebugLevelEnum level, string msg, Trigger trig = null)
            => UnfilteredAddToLog(level, msg, trig, (ActionBase)null);

        internal void UnfilteredAddToLog(DebugLevelEnum level, string msg, ActionBase action)
            => UnfilteredAddToLog(level, msg, action?.ParentTrigger, action);

        internal void UnfilteredAddToLog(DebugLevelEnum level, string msg, Trigger trig, ActionBase action)
        {
            InternalLog il = new InternalLog() 
            { 
                Timestamp = DateTime.Now, 
                Level = level, 
                Message = msg,
                SourceTrigger = trig,
                SourceAction = action
            };
            if (DisableLogging == false && System.Diagnostics.Debugger.IsAttached == true)
            {
                System.Diagnostics.Debug.WriteLine(il.ToString());
            }
            if (level == DebugLevelEnum.Error)
            {
                ui?.IncrementErrorCount();
            }
            var queue = log[level];
            lock (queue)
            {
                // Preserve enqueue order across level queues when timestamps are equal.
                il.Sequence = Interlocked.Increment(ref logSequence);
                queue.Enqueue(il);
                if (queue.Count > 30000)
                {
                    queue.Dequeue();
                }
            }
        }

        public void FilteredAddToLog(DebugLevelEnum level, string msg, Trigger trig = null)
            => FilteredAddToLog(level, msg, trig, (ActionBase)null);

        public void FilteredAddToLog(DebugLevelEnum level, string msg, ActionBase action)
            => FilteredAddToLog(level, msg, action?.ParentTrigger, action);

        public void FilteredAddToLog(DebugLevelEnum level, string msg, Trigger trig, ActionBase action)
        {
            if (cfg != null && level > cfg.DebugLevel)
            {
                return;
            }
            UnfilteredAddToLog(level, msg, trig, action);
        }

    }
}
