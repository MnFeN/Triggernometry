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
            ui?.ClearLogCounts();
        }

        /// <summary>应用调低后的记录上限，每个级别只保留最新的日志。</summary>
        internal void TrimLogs()
        {
            foreach (var pair in log)
            {
                lock (pair.Value)
                    TrimLogQueue(pair.Key, pair.Value);
            }
        }

        /// <summary>在调用方持有队列锁时，按该级别上限移除最旧日志。</summary>
        private void TrimLogQueue(DebugLevelEnum level, Queue<InternalLog> queue)
        {
            int limit = Math.Max(1000, Config?.GetLogLimit(level) ?? 30000);
            while (queue.Count > limit)
                queue.Dequeue();
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
            var queue = log[level];
            lock (queue)
            {
                // Preserve enqueue order across level queues when timestamps are equal.
                il.Sequence = Interlocked.Increment(ref logSequence);
                queue.Enqueue(il);
                TrimLogQueue(level, queue);
            }
            // 记录入队后再通知 UI，确保点击计数时能够查询到对应日志。
            if (level == DebugLevelEnum.Error || level == DebugLevelEnum.Warning)
            {
                ui?.IncrementLogCount(level);
            }
        }

        public void FilteredAddToLog(DebugLevelEnum level, string msg, Trigger trig = null)
            => FilteredAddToLog(level, msg, trig, (ActionBase)null);

        public void FilteredAddToLog(DebugLevelEnum level, string msg, ActionBase action)
            => FilteredAddToLog(level, msg, action?.ParentTrigger, action);

        public void FilteredAddToLog(DebugLevelEnum level, string msg, Trigger trig, ActionBase action)
        {
            if (Config != null && level > Config.DebugLevel)
            {
                return;
            }
            UnfilteredAddToLog(level, msg, trig, action);
        }

    }
}
