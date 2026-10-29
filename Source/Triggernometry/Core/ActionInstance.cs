using System;
using System.Threading.Tasks;
using Triggernometry.Core.Actions;
using static Triggernometry.Core.RealPlugin;

namespace Triggernometry.Core
{
    /// <summary>保存一次动作执行的调度状态，并负责执行及收尾。</summary>
    internal class ActionInstance : IComparable<ActionInstance>
    {

        internal DateTime ExecuteAt { get; set; }
        internal long QueueSequence { get; set; }
        internal MutexInfo Mutex { get; set; }
        internal ActionBase Action { get; set; }
        internal Context Context { get; set; }
        internal bool ShouldReleaseMutex { get; set; } = false;

        // Continuations belong to this execution, never to the shared action definition.
        internal ActionInstance NextAction { get; set; }
        internal ActionInstance LoopParent { get; set; }
        internal Context LoopContext { get; set; }
        internal bool IsLoopTail { get; set; }
        private bool isIterationQueued;

        /// <summary>
        /// The effective tag text used by the action,
        /// with expressions evaluated when the action is queued. <br />
        /// If this action’s <see cref="ActionBase.Tag"/> is not specified, or evaluates to a whitespace-string,
        /// the <see cref="Trigger.Tag"/> from the parent trigger will be used instead. <br />
        /// Empty if no tag is specified anywhere.
        /// </summary>
        internal string ParsedTag { get; set; } = "";

        /// <summary>创建执行实例，并记录上下文和互斥锁释放责任。</summary>
        internal ActionInstance(DateTime executeAt, long queueSequence, MutexInfo mutex, ActionBase action, Context ctx, bool shouldReleaseMutex)
        {
            ExecuteAt = executeAt;
            QueueSequence = queueSequence;
            Mutex = mutex;
            Action = action;
            Context = ctx;
            ShouldReleaseMutex = shouldReleaseMutex;
        }

        /// <summary>等待动作实际执行结束，再推进队列并释放本次执行持有的资源。</summary>
        internal async Task Dispatch()
        {
            Action executeOverride = null;
            // 仅循环绑定实例方法，普通动作不创建委托或闭包。
            if (Action is ActionLoop) executeOverride = ExecuteLoop;
            try
            {
                await Action.InternalExecute(Context, executeOverride).ConfigureAwait(false);
                if (!isIterationQueued) QueueNext(Context);
            }
            catch
            {
                // 动作异常已由 InternalExecute 记录；取消或失败时均不再推进后续动作。
            }
            finally 
            { 
                ActionFinished(); 
            }
        }

        /// <summary>推进本次循环，结束时选择后续动作使用的上下文。</summary>
        internal void ExecuteLoop()
        {
            isIterationQueued = QueueIteration((ActionLoop)Action);
            // 根循环传出最终迭代器；嵌套循环保留调用者的迭代器。
            if (!isIterationQueued && LoopParent == null && LoopContext != null)
                Context = LoopContext;
        }

        /// <summary>初始化或递增迭代器，满足循环条件时排入本轮子动作。</summary>
        private bool QueueIteration(ActionLoop loop)
        {
            if (!loop.LoopCondition.Enabled) return false;

            Context loopContext = LoopContext;
            if (loopContext == null)
            {
                loopContext = Context.Duplicate();
                loopContext.loopActionId = loop.Id;
                loopContext.loopIterator = (int)Context.EvaluateNumericExpression(loop.ActionContextLogger, Context, loop.Start);
            }
            else
            {
                loopContext.loopIterator += (int)loopContext.EvaluateNumericExpression(loop.ActionContextLogger, loopContext, loop.Step);
            }
            // 首次判断失败时不替换调用者的上下文。
            if (!loop.LoopCondition.Check(loopContext, loop.ActionContextLogger, loopContext)) return false;
            LoopContext = loopContext;

            return loopContext.Plugin.QueueActions(loopContext, DateTime.Now, loop.LoopActions,
                loopContext.Trigger?.Sequential ?? true, Mutex, loop.ActionContextLogger, this) != null;
        }

        /// <summary>安排下一个顺序动作或下一轮循环，并转交互斥锁的释放责任。</summary>
        private void QueueNext(Context ctx)
        {
            ActionBase action = Action;
            try
            {
                ActionInstance nextAction = NextAction;
                if (nextAction != null)
                {
                    nextAction.Context = ctx;
                    nextAction.ExecuteAt = DateTime.Now.AddMilliseconds(ctx.EvaluateNumericExpression(action.ActionContextLogger, ctx, nextAction.Action.ExecDelay));
                }
                else if (IsLoopTail)
                {
                    ActionInstance parent = LoopParent;
                    DateTime when = DateTime.Now.AddMilliseconds(ctx.EvaluateNumericExpression(action.ActionContextLogger, ctx, ((ActionLoop)parent.Action).LoopDelay));
                    nextAction = new ActionInstance(when, 0, Mutex, parent.Action, parent.Context, false)
                    {
                        NextAction = parent.NextAction,
                        LoopParent = parent.LoopParent,
                        LoopContext = parent.LoopContext,
                        IsLoopTail = parent.IsLoopTail
                    };
                }
                if (nextAction != null)
                {
                    lock (ctx.Plugin.ActionQueue)
                    {
                        // Only a successfully queued successor takes over the mutex release.
                        nextAction.ShouldReleaseMutex = ShouldReleaseMutex;
                        if (ctx.Plugin.QueueAction(nextAction)) ShouldReleaseMutex = false;
                    }
                }
            }
            catch (Exception ex)
            {
                action.LogExecutionException(ctx, ex);
            }
        }

        /// <summary>按执行时间和入队顺序比较，供动作队列二分插入时自动反射使用。</summary>
        public int CompareTo(ActionInstance other)
        {
            if (other == null) return 1;
            int execTimeComparison = ExecuteAt.CompareTo(other.ExecuteAt);
            if (execTimeComparison != 0)
            {
                return execTimeComparison;
            }
            return QueueSequence.CompareTo(other.QueueSequence);
        }

        /// <summary>移除执行记录，并按本次调用的释放责任归还互斥锁。</summary>
        private void ActionFinished()
        {
            lock (Context.Plugin.ActionQueue)
            {
                Context.Plugin.ExecutingActions.Remove(this);
                if (Mutex != null && ShouldReleaseMutex)
                {
                    Mutex.Release(Context);
                    Mutex = null;
                }
            }
        }

    }
}
