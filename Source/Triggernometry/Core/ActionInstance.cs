using System;
using System.Collections.Generic;
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
        private volatile bool isCancelled;
        /// <summary>线程间可见的取消状态；true 表示实例已取消，读取无需获取队列锁。</summary>
        internal bool IsCancelled
        {
            get => isCancelled;
            private set => isCancelled = value;
        }
        private bool isIterationQueued;

        /// <summary>
        /// 已收到的取消筛选条件，供尚未入队的顺序动作及本次循环的后续实例继续检查。<br />
        /// 任意条件匹配即取消；Filter 为 null 表示取消全部可打断动作，列表为 null 表示未保留请求。<br />
        /// RequiresTag 表示条件依赖已求值的标签，其他条件可立即判断。<br />
        /// 同次取消覆盖的实例可共享此列表，读写时须持有队列锁。
        /// </summary>
        private List<(Func<ActionInstance, bool> Filter, bool RequiresTag)> cancellationFilters;

        /// <summary>
        /// The effective tag text used by the action,
        /// with expressions evaluated when the action is queued. <br />
        /// If this action’s <see cref="ActionBase.Tag"/> is not specified, or evaluates to a whitespace-string,
        /// the <see cref="Trigger.Tag"/> from the parent trigger will be used instead. <br />
        /// 尚未求值时为 null；已求值但未指定标签时为空字符串。
        /// </summary>
        internal string ParsedTag { get; private set; }

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

        // 调用方持有队列锁；循环新实例沿用本次循环收到的取消请求。
        internal void InheritCancellation(ActionInstance parent)
            => cancellationFilters = parent?.cancellationFilters;

        /// <summary>
        /// 保留后续执行所需的取消请求，并尝试立即取消当前实例。<br />
        /// 返回 true 表示本次调用首次将实例标记为已取消；false 表示未改变取消状态。
        /// </summary>
        internal bool Cancel(Func<ActionInstance, bool> filter, bool requiresTag, List<(Func<ActionInstance, bool> Filter, bool RequiresTag)> requests)
        {
            if (Action is ActionLoop || (requiresTag && ParsedTag == null))
            {
                if (cancellationFilters == null)
                    cancellationFilters = requests;
                else
                {
                    var request = (filter, requiresTag);
                    if (!cancellationFilters.Contains(request))
                        cancellationFilters.Add(request);
                }
            }
            // 尚未入队的顺序动作保留筛选条件，等到正常入队时再求值标签。
            if (IsCancelled)
                return false;
            if (Action.Uninterruptible)
                return false;
            if (filter != null)
            {
                if (requiresTag && ParsedTag == null)
                    return false;
                if (!filter(this))
                    return false;
            }
            IsCancelled = true;
            return true;
        }

        internal void ParseTag()
        {
            if (ParsedTag != null)
                return;
            Context tagContext = LoopContext ?? Context;
            var tag = tagContext.EvaluateStringExpression(null, tagContext, Action.Tag);
            if (string.IsNullOrWhiteSpace(tag))
                tag = tagContext.EvaluateStringExpression(null, tagContext, tagContext.Trigger?.Tag);
            ParsedTag = string.IsNullOrWhiteSpace(tag) ? "" : tag;
        }

        /// <summary>
        /// 入队前应用已记录的取消请求，必要时求值标签。<br />
        /// 返回 true 表示实例尚未取消，可以继续尝试入队；false 表示实例已取消，不应入队。
        /// </summary>
        internal bool PrepareToQueue()
        {
            if (IsCancelled)
                return false;
            if (Action.Uninterruptible || cancellationFilters == null)
                return true;

            // 先检查不依赖标签的请求，命中时无需求值标签。
            foreach (var item in cancellationFilters)
            {
                if (item.RequiresTag)
                    continue;
                if (item.Filter == null || item.Filter(this))
                {
                    IsCancelled = true;
                    return false;
                }
            }

            // 再检查依赖标签的请求；ParseTag 会复用已经求值的标签。
            foreach (var item in cancellationFilters)
            {
                if (!item.RequiresTag)
                    continue;
                ParseTag();
                if (item.Filter == null || item.Filter(this))
                {
                    IsCancelled = true;
                    return false;
                }
            }
            return true;
        }

        /// <summary>等待动作实际执行结束，再推进队列并释放本次执行持有的资源。</summary>
        internal async Task Dispatch()
        {
            Action executeOverride = null;
            // 仅循环绑定实例方法，普通动作不创建委托或闭包。
            if (Action is ActionLoop)
                executeOverride = ExecuteLoop;
            try
            {
                await Action.InternalExecute(Context, executeOverride, this).ConfigureAwait(false);
                if (!isIterationQueued)
                    QueueNext(Context);
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
            if (!loop.LoopCondition.Enabled)
                return false;

            Context loopContext = LoopContext;
            if (loopContext == null)
            {
                loopContext = Context.Duplicate();
                loopContext.loopActionId = loop.Id;
                loopContext.loopIterator = (int)Context.EvaluateNumericExpression(loop.ExpressionLogger, Context, loop.Start);
            }
            else
            {
                loopContext.loopIterator += (int)loopContext.EvaluateNumericExpression(loop.ExpressionLogger, loopContext, loop.Step);
            }
            // 首次判断失败时不替换调用者的上下文。
            if (!loop.LoopCondition.Check(loopContext, loop.ExpressionLogger, loopContext))
                return false;
            LoopContext = loopContext;

            return loopContext.Plugin.QueueActions(loopContext, DateTime.Now, loop.LoopActions,
                loopContext.Trigger?.Sequential ?? true, Mutex, loop.ExpressionLogger, this) != null;
        }

        /// <summary>安排下一个顺序动作或下一轮循环，并转交互斥锁的释放责任。</summary>
        internal void QueueNext(Context ctx)
        {
            ActionBase action = Action;
            try
            {
                lock (ctx.Plugin.ActionQueue)
                {
                    ActionInstance current = this;
                    while (true)
                    {
                        ActionInstance nextAction = current.NextAction;
                        bool nextIteration = false;
                        if (nextAction == null && current.IsLoopTail)
                        {
                            ActionInstance parent = current.LoopParent;
                            if (parent.IsCancelled)
                            {
                                current = parent;
                                continue;
                            }
                            nextIteration = true;
                            nextAction = new ActionInstance(DateTime.Now, 0, Mutex, parent.Action, parent.Context, false)
                            {
                                NextAction = parent.NextAction,
                                LoopParent = parent.LoopParent,
                                LoopContext = parent.LoopContext,
                                IsLoopTail = parent.IsLoopTail
                            };
                            nextAction.InheritCancellation(parent);
                        }
                        if (nextAction == null)
                            return;
                        if (!nextIteration)
                            nextAction.Context = ctx;
                        if (!nextAction.PrepareToQueue())
                        {
                            current = nextAction;
                            continue;
                        }
                        string delay = nextIteration ? ((ActionLoop)nextAction.Action).LoopDelay : nextAction.Action.ExecDelay;
                        nextAction.ExecuteAt = DateTime.Now.AddMilliseconds(ctx.EvaluateNumericExpression(action.ExpressionLogger, ctx, delay));
                        // Only a successfully queued successor takes over the mutex release.
                        nextAction.ShouldReleaseMutex = ShouldReleaseMutex;
                        if (ctx.Plugin.QueueAction(nextAction))
                            ShouldReleaseMutex = false;
                        return;
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
            if (other == null)
                return 1;
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
