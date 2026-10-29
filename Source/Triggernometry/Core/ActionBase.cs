using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Triggernometry.Core.Actions;
using Triggernometry.Core.Conditions;
using Triggernometry.Core.Variables;
using Triggernometry.Core.Serialization;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;
using Triggernometry.Utilities;
using static Triggernometry.Core.RealPlugin;

namespace Triggernometry.Core
{

    [XmlInclude(typeof(ActionBeep))]
    [XmlInclude(typeof(ActionPlaySound))]
    [XmlInclude(typeof(ActionPlaySpeech))]
    [XmlInclude(typeof(ActionMessageBox))]
    [XmlInclude(typeof(ActionOverlayText))]
    [XmlInclude(typeof(ActionOverlayImage))]
    [XmlInclude(typeof(ActionVariableScalar))]
    [XmlInclude(typeof(ActionVariableList))]
    [XmlInclude(typeof(ActionVariableTable))]
    [XmlInclude(typeof(ActionVariableDict))]
    [XmlInclude(typeof(ActionLogMessage))]
    [XmlInclude(typeof(ActionNamedCallback))]
    [XmlInclude(typeof(ActionExecuteScript))]
    [XmlInclude(typeof(ActionLoop))]
    [XmlInclude(typeof(ActionMutex))]
    [XmlInclude(typeof(ActionTriggerOperation))]
    [XmlInclude(typeof(ActionFolderOperation))]
    [XmlInclude(typeof(ActionRepository))]
    [XmlInclude(typeof(ActionMouse))]
    [XmlInclude(typeof(ActionKeypress))]
    [XmlInclude(typeof(ActionWindowMessage))]
    [XmlInclude(typeof(ActionLaunchProcess))]
    [XmlInclude(typeof(ActionDiskOperation))]
    [XmlInclude(typeof(ActionClipboard))]
    [XmlInclude(typeof(ActionActInteraction))]
    [XmlInclude(typeof(ActionJsonRequest))]
    [XmlInclude(typeof(ActionDiscordWebhook))]
    [XmlInclude(typeof(ActionObsControl))]
    [XmlInclude(typeof(ActionLiveSplitControl))]
    [XmlInclude(typeof(ActionPlaceholder))]
    public abstract class ActionBase : IEditorRulesProvider
    {

        /// <summary>创建动作参数的控件联动规则。</summary>
        public virtual Action CreateEditorRules(ConfigEditor editor) => null;

        /// <summary>通用动作的 UI 选项联动规则</summary>
        internal Action CreateGeneralEditorRules(ConfigEditor editor)
        {
            var uninterruptible = editor.GetEditor(nameof(Uninterruptible));
            var replaceQueued = editor.GetEditor(nameof(ReplaceQueued));
            return () =>
            {
                bool isUninterruptible = (bool)uninterruptible.GetValue();
                if (isUninterruptible) replaceQueued.SetValue(false);
                editor.SetEnabled(!isUninterruptible, replaceQueued);
            };
        }

        #region Classes and enums

        /// <summary>声明动作类型的显示名称和分类。</summary>
        [AttributeUsage(AttributeTargets.Class, Inherited = false)]
        public sealed class ActionAttribute : Attribute
        {
            /// <summary>动作选择器中的分类，供菜单与滚轮切换使用。</summary>
            public enum CategoryType
            {
                Audio,
                Display,
                Variables,
                Programming,
                TriggerManagement,
                SystemInteraction,
                ExternalInteraction,
                Placeholder,
            }

            public string Name { get; }
            public CategoryType Category { get; }
            public string TranslationKey => "ActionForm/cbxActionType[" + Name + "]";

            /// <summary>设置动作类型的显示名称和分类。</summary>
            public ActionAttribute(string name, CategoryType category)
            {
                Name = name;
                Category = category;
            }
        }

        /// <summary>Copied automatically by <see cref="CopySpecificPropertiesTo"/> via reflection.</summary>
        [AttributeUsage(AttributeTargets.Property, Inherited = true)]
        public sealed class ActionSpecificPropertyAttribute : Attribute { }

        #endregion

        #region General properties

        public class ActionBundle
        {
            internal static string ActionsToXml(List<ActionBase> actions) => ActionXmlCollection.ToXml(actions);
            internal static List<ActionBase> XmlToActions(string text) => ActionXmlCollection.FromXml(text);
        }

        /// <summary>
        /// for queue controlling
        /// </summary>
        internal Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>The one-based position within the trigger, including loop ancestors (for example, 5-3).</summary>
        [XmlIgnore]
        internal string IndexInTrigger { get; private set; } = "0";

        [XmlIgnore]
        public bool Enabled { get; set; } = true;

        [XmlAttribute("Enabled")]
        public string Xml_Enabled
        {
            get => XmlAttr.Bool(Enabled, true);
            set => Enabled = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        public Trigger ParentTrigger { get; set; } = null;




        [XmlIgnore]
        [ExpressionEditor(3, EditorExpressionType.Numeric, Label = "Execution delay from last action (ms)")]
        public string ExecDelay { get; set; } = "0";

        [XmlAttribute("ExecDelay")]
        public string Xml_ExecDelay
        {
            get => XmlAttr.String(ExecDelay, "0", omitWhitespace: false);
            set => ExecDelay = value;
        }



        [XmlIgnore]
        [CheckBoxEditor(4, Label = "Execute asynchronously without blocking other actions from executing")]
        public bool Async { get; set; } = false;

        [XmlAttribute("Async")]
        public string Xml_Async
        {
            get => XmlAttr.Bool(Async, false);
            set => Async = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        [EnumChoiceEditor(1, Label = "Logging filtering level",
            ChoiceLabels = new[]
        {
            "Nothing",
            "Errors only",
            "Errors and warnings",
            "Above custom",
            "Above custom 2",
            "All informational messages",
            "Verbose debug",
            "(inherit from trigger)",
        })]
        public DebugLevelEnum DebugLevel { get; set; } = DebugLevelEnum.Inherit;

        [XmlAttribute("DebugLevel")]
        public string Xml_DebugLevel
        {
            get => XmlAttr.Enum(DebugLevel, DebugLevelEnum.Inherit);
            set => DebugLevel = XmlAttr.Enum<DebugLevelEnum>(value);
        }



        [XmlIgnore]
        [CheckBoxEditor(5, Label = "Interrupt previously queued instances of this action")]
        public bool ReplaceQueued { get; set; } = false;

        [XmlAttribute("ReplaceQueued")]
        public string Xml_ReplaceQueued
        {
            get => XmlAttr.Bool(ReplaceQueued, false);
            set => ReplaceQueued = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        [CheckBoxEditor(6, Label = "Prevent the same action from being queued again")]
        public bool PreventDuplicateQueue { get; set; } = false;

        [XmlAttribute("PreventDuplicateQueue")]
        public string Xml_PreventDuplicateQueue
        {
            get => XmlAttr.Bool(PreventDuplicateQueue, false);
            set => PreventDuplicateQueue = XmlAttr.Bool(value);
        }



        /// <summary>
        /// 保留已排队的动作，并忽略全局取消请求；<br />
        /// 不保护尚未入队的后续动作或循环子动作。
        /// </summary>
        [XmlIgnore]
        [CheckBoxEditor(7, Label = "Never interrupt this action")]
        public bool Uninterruptible { get; set; } = false;

        [XmlAttribute("Uninterruptible")]
        public string Xml_Uninterruptible
        {
            get => XmlAttr.Bool(Uninterruptible, false);
            set => Uninterruptible = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        public string Desc { get; set; } = "";

        [XmlAttribute("Desc")]
        public string Xml_Desc
        {
            get => XmlAttr.String(Desc, "", omitWhitespace: false);
            set => Desc = value;
        }



        [XmlIgnore]
        public string DescBackColor { get; set; } = "";

        [XmlAttribute("DescBackColor")]
        public string Xml_DescBackColor
        {
            get => XmlAttr.String(DescBackColor, "", omitWhitespace: false);
            set => DescBackColor = value;
        }



        [XmlIgnore]
        public string DescTextColor { get; set; } = "";

        [XmlAttribute("DescTextColor")]
        public string Xml_DescTextColor
        {
            get => XmlAttr.String(DescTextColor, "", omitWhitespace: false);
            set => DescTextColor = value;
        }



        [XmlIgnore]
        public bool DescOverride { get; set; } = false;

        [XmlAttribute("DescOverride")]
        public string Xml_DescOverride
        {
            get => XmlAttr.Bool(DescOverride, false);
            set => DescOverride = XmlAttr.Bool(value);
        }



        /// <summary>A string tag used in trigger/folder actions to interrupt specific actions.</summary>
        [XmlIgnore]
        [ExpressionEditor(2, Label = "Tag")]
        public string Tag { get; set; }

        [XmlAttribute("Tag")]
        public string Xml_Tag
        {
            get => XmlAttr.String(Tag, null, omitWhitespace: false);
            set => Tag = value;
        }



        [XmlIgnore]
        public ConditionGroup Condition = new ConditionGroup { Enabled = false };

        [XmlElement("Condition")]
        public ConditionGroup Xml_Condition
        {
            get => Condition.Children.Count == 0 && !Condition.Enabled ? null : Condition;
            set => Condition = value;
        }

        #endregion

        #region Describe/Implementation

        /// <summary>
        /// Returns the specific description text for this action type.  
        /// Implemented by each subclass.
        /// </summary>
        internal abstract string DescribeImplementation();

        /// <summary>
        /// Builds a full description of this action, including common details  
        /// like async mode, delay, and condition, plus the subclass-specific part.
        /// </summary>
        public string Describe()
        {
            try
            {
                if (DescOverride)
                {
                    return Desc ?? "";
                }
                string temp = I18n.TrlAsync(Async);
                if (!string.IsNullOrWhiteSpace(ExecDelay) && ExecDelay.Trim() != "0")
                {
                    string delay = double.TryParse(ExecDelay.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out _) ? ExecDelay : $"({ExecDelay})";
                    temp += I18n.Translate("internal/Action/descafterdelay", "after {0} ms, ", delay);  // included comma in translations (comma symbols are language-dependent)
                }
                if (Condition?.Enabled == true)
                {
                    temp += I18n.Translate("internal/Action/descassumingcondition", "assuming condition is met, ");
                }
                temp += DescribeImplementation();
                return !string.IsNullOrWhiteSpace(temp) ? char.ToUpperInvariant(temp[0]) + temp.Substring(1) : string.Empty;
            }
            catch (Exception ex)
            {
                return "Failed to describe action: " + ex.Message;
            }
        }

        #endregion

        #region Scheduling and execution

        /// <summary>
        /// True = action was executed (enabled & conditions met), false = action was not executed (disabled or conditions not met)
        /// </summary>
        internal bool LastExecutionResult
        {
            get => _LastExecutionResult;
            set
            {
                _LastExecutionResult = value;
                LastExecutionTime = DateTime.Now;
                ExecutionCount++;
            }
        }
        private bool _LastExecutionResult { get; set; } = false;
        internal DateTime LastExecutionTime { get; set; } = DateTime.MinValue;
        internal int ExecutionCount { get; set; } = 0;

        /// <summary>在指定上下文中执行具体动作的业务逻辑。</summary>
        internal abstract void ExecuteImplementation(Context ctx);

        /// <summary>
        /// 直接执行一个动作，忽略动作延迟属性。
        /// </summary>
        /// <remarks>
        /// 同步动作：等待动作结束；<br/>
        /// 异步动作：提交到后台执行并直接返回。<br/><br/>
        /// 两种模式的业务异常均被捕获并记录日志。<br/><br/>
        /// 循环动作仅表示启动完成，不等待子动作和后续迭代。
        /// </remarks>
        public void Execute(Context context = null)
        {
            Task task = ExecuteAsync(context);
            if (!Async)
            {
                try { task.GetAwaiter().GetResult(); }
                catch
                {
                    // 业务异常已由执行路径记录，普通调用保留旧版不向外抛出的行为。
                }
            }
        }

        /// <summary>
        /// 返回表示本次动作执行完成的任务，忽略动作延迟属性。
        /// </summary>
        /// <remarks>
        /// 同步动作：业务仍在调用线程同步执行，返回时任务已结束；<br/>
        /// 异步动作：业务在后台执行，调用方可 await 返回任务。<br/><br/>
        /// 两种模式的业务异常均直接记录日志并保存于返回任务。<br/>
        /// 直接调用不会抛出异常；await / GetAwaiter().GetResult() 时会重新抛出。<br/><br/>
        /// 循环动作仅表示启动完成，不包含子动作和后续迭代。
        /// </remarks>
        public async Task ExecuteAsync(Context context = null)
            => await InternalExecute(context ?? new Context(ParentTrigger)).ConfigureAwait(false);

        /// <summary>
        /// 统一处理条件、日志、异常和异步调度。<br />
        /// <paramref name="executeOverride"/> ：用于循环等特殊动作，替代默认业务逻辑。
        /// </summary>
        internal Task InternalExecute(Context ctx, Action executeOverride = null)
        {
            void ExecuteCore()
            {
                try
                {
                    var shouldCheckCondition = !ctx.forceType.HasFlag(ActionTriggerOperation.ForceEnum.SkipConditions) && !ctx.testByPlaceholder;
                    var conditionFailed = shouldCheckCondition && Condition?.Enabled == true && !Condition.Check(ctx, ExpressionLogger, ctx);

                    var shouldLogVerbose = GetDebugLevel(ctx) >= DebugLevelEnum.Verbose;
                    if (conditionFailed)
                    {
                        ctx.PushActionResult(0);
                        if (shouldLogVerbose)
                            AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/actionnotfired", "Action #{0} on trigger '{1}' not fired, condition not met", IndexInTrigger, ctx.Trigger?.LogName ?? "(null)"));
                        return;
                    }
                    ctx.PushActionResult(1);
                    // 过滤掉的日志无需生成动作描述和翻译文本。
                    if (shouldLogVerbose)
                        AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/executingaction", "Executing action '{0}' in thread {1}", Describe(), Thread.CurrentThread.ManagedThreadId));

                    if (executeOverride != null) 
                        executeOverride();
                    else 
                        ExecuteImplementation(ctx);
                }
                catch (Exception ex)
                {
                    LogExecutionException(ctx, ex);
                    // 通过任务告知队列执行失败，避免继续排入后续动作。
                    throw;
                    // To-do: 需要重新考虑动作失败后应该如何处理，包括顺序和非顺序的情况。
                    // 现在的顺序模式下出错会导致后续动作不再执行，这似乎并非预期设计。
                }
            }

            if (Async)
            { 
                var cancelToken = Uninterruptible ? CancellationToken.None : ctx.Plugin?.GetCancellationToken() ?? CancellationToken.None;
                return Task.Run(ExecuteCore, cancelToken);
            }
            else ExecuteCore();
            return Task.CompletedTask;
        }

        /// <summary>统一记录直接执行和队列执行中的异常，并保留回调的原始异常。</summary>
        internal void LogExecutionException(Context ctx, Exception ex)
        {
            if (GetDebugLevel(ctx) < DebugLevelEnum.Error) return;
            if (this is ActionNamedCallback && ex is TargetInvocationException tiex && tiex.InnerException != null)
                ex = tiex.InnerException;
            string triggerPath = ctx?.Trigger?.FullPath ?? "(null)";
            string actionDesc = "";
            try { actionDesc = Describe(); } catch { }
            actionDesc = actionDesc.Length > 300 ? actionDesc.Substring(0, 297) + "..." : actionDesc;
            string detail = ex.FullMessage();

            AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/exception",
                "Action exception: {0}  \nIn action: {1}  \nIn trigger: {2}{3}",
                ex.Message, actionDesc, triggerPath, detail));
        }

        internal DebugLevelEnum GetDebugLevel(Context ctx)
        {
            if (DebugLevel == DebugLevelEnum.Inherit)
            {
                return ctx?.Trigger?.GetDebugLevel(Instance) ?? DebugLevelEnum.Verbose;
            }
            return DebugLevel;
        }

        internal void AddToLog(Context ctx, DebugLevelEnum level, string message)
        {
            DebugLevelEnum dx = GetDebugLevel(ctx);
            if (level > dx)
            {
                return;
            }
            Instance.UnfilteredAddToLog(level, message, this);
        }

        #endregion

        #region Action helpers

        private Context.LoggerDelegate _expressionLogger;

        /// <summary>复用仅绑定当前动作的日志委托，执行上下文仍由调用方传入。</summary>
        internal Context.LoggerDelegate ExpressionLogger
            => _expressionLogger ?? (_expressionLogger = ActionContextLogger);

        public void ActionContextLogger(object o, string msg)
        {
            AddToLog((Context)o, DebugLevelEnum.Verbose, msg);
        }

        /// <summary>使用当前动作的日志回调解析数值表达式。</summary>
        protected double EvalNumeric(string expression, Context ctx)
            => ctx.EvaluateNumericExpression(ExpressionLogger, ctx, expression);

        /// <summary>使用当前动作的日志回调解析文本表达式。</summary>
        protected string EvalString(string expression, Context ctx)
            => ctx.EvaluateStringExpression(ExpressionLogger, ctx, expression);

        /// <summary>生成变量修改来源文本，区分触发器执行和独立执行。</summary>
        protected string DescribeVariableChanger(Context ctx)
        {
            // #18：集中保留原有翻译键及文本，各动作仍在原来的时机生成来源。
            return ctx.Trigger != null
                ? I18n.Translate("internal/Action/changetagtrigaction", "Trigger '{0}' action '{1}'", ctx.Trigger.LogName, Describe())
                : I18n.Translate("internal/Action/changetagtestmode", "Action '{0}' test mode", Describe());
        }

        /// <summary>解析变量名称并取得对应存储区，不读取或创建变量。</summary>
        protected (string Name, VariableStore Store) ResolveVariable(Context ctx, string expression, bool persistent)
        {
            RealPlugin plug = ctx.Plugin;
            return (EvalString(expression, ctx), plug.GetVariableStore(persistent));
        }

        protected IOrderedEnumerable<int> ApplySorting(int elementCount, List<bool> isNumeric, List<bool> isAscending, List<List<string>> values)
        {
            // Create an enumeration of indices representing the initial order
            IEnumerable<int> indices = Enumerable.Range(0, elementCount);
            IOrderedEnumerable<int> sortedIndices = null;

            // Iterate through the sorting key functions
            for (int keyIndex = 0; keyIndex < values.Count; keyIndex++)
            {
                int k = keyIndex; // local variable for lambda expression
                // 4 sorting rules: numeric/string × ascending/descending
                if (keyIndex == 0)
                {
                    if (isNumeric[k])
                    {
                        sortedIndices = isAscending[k]
                            ? indices.OrderBy(i => Convert.ToDouble(values[k][i], CultureInfo.InvariantCulture))
                            : indices.OrderByDescending(i => Convert.ToDouble(values[k][i], CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        sortedIndices = isAscending[k]
                            ? indices.OrderBy(i => values[k][i])
                            : indices.OrderByDescending(i => values[k][i]);
                    }
                }
                else
                {
                    if (isNumeric[k])
                    {
                        sortedIndices = isAscending[k]
                            ? sortedIndices.ThenBy(i => Convert.ToDouble(values[k][i], CultureInfo.InvariantCulture))
                            : sortedIndices.ThenByDescending(i => Convert.ToDouble(values[k][i], CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        sortedIndices = isAscending[k]
                            ? sortedIndices.ThenBy(i => values[k][i])
                            : sortedIndices.ThenByDescending(i => values[k][i]);
                    }
                }
            }
            return sortedIndices;
        }

        protected static void CheckInvalidDymanicExpr(string expr, string[] invalidExprs)
        {
            foreach (string word in invalidExprs)
            {
                if (expr.Contains(word))
                    throw new ArgumentException(I18n.Translate("internal/Action/dynamicexprerror",
                        "The dynamic expression ({0}) is invalid in the current action. Expression: ({1})",
                        word, expr));
            }
        }

        protected void ParseSortKeyFunctions(string rawExpr,
            out List<bool> isNumeric, out List<bool> isAscending,
            out List<string> keysExpr, out List<List<string>> values)
        {   // parsing expressions like "n+:key1, s-:key2, s+:key3, ..."
            string[] rawKeys = ArgHelper.SplitArguments(rawExpr, allowEmptyList: true);

            isNumeric = new List<bool>();       // numeric / string options
            isAscending = new List<bool>();     // ascending / descending options
            keysExpr = new List<string>();      // expression of the keys
            values = new List<List<string>>();  // each sublist contains the evaluated results of one key

            Regex regexSortKeyExpr = new Regex("^ *(?<type>[NnSs]) *(?<order>[-+]?) *:(?<key>.+)$");
            foreach (string rawKey in rawKeys)
            {
                Match keyMatch = regexSortKeyExpr.Match(rawKey);
                if (keyMatch.Success)
                {
                    isNumeric.Add(keyMatch.Groups["type"].Value.ToLower() == "n");
                    isAscending.Add(keyMatch.Groups["order"].Value != "-");
                    keysExpr.Add(keyMatch.Groups["key"].Value);
                    values.Add(new List<string>());
                }
                else
                {
                    throw new ArgumentException(I18n.Translate("internal/Action/sortkeyexprerror",
                        "The sorting key functions ({0}) could not be parsed.", rawKey));
                }
            }
        }

        protected Tuple<int, string> SendJson(Context ctx, ActionJsonRequest.MethodEnum method, string url, string json, IEnumerable<string> headers, bool expectNoContent)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                if (headers != null && headers.Count() > 0)
                {
                    foreach (string hdr in headers)
                    {
                        var sepIndex = hdr.IndexOf(':');
                        if (sepIndex > 0)
                        {
                            var key = hdr.Substring(0, sepIndex).Trim();
                            var value = hdr.Substring(sepIndex + 1).Trim();
                            switch (key.ToLower())
                            {
                                case "content-type":
                                    httpWebRequest.ContentType = value;
                                    break;
                                case "user-agent":
                                    httpWebRequest.UserAgent = value;
                                    break;
                                case "accept":
                                    httpWebRequest.Accept = value;
                                    break;
                                case "referer":
                                    httpWebRequest.Referer = value;
                                    break;
                                case "host":
                                    httpWebRequest.Host = value;
                                    break;
                                default:
                                    httpWebRequest.Headers.Add(key, value);
                                    break;
                            }
                        }
                    }
                }
                switch (method)
                {
                    case ActionJsonRequest.MethodEnum.POST:
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        break;
                    case ActionJsonRequest.MethodEnum.GET:
                        httpWebRequest.Method = "GET";
                        break;
                }
                HttpWebResponse httpResponse;
                try
                {
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                }
                catch (WebException ex) when (ex.Response is HttpWebResponse)
                {
                    // HTTP errors still carry a response that expressions may need to inspect.
                    httpResponse = (HttpWebResponse)ex.Response;
                }
                using (httpResponse)
                {
                    if (httpResponse.StatusCode != HttpStatusCode.NoContent && expectNoContent == true)
                    {
                        AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/jsonpostunexpectedresponse", "Unexpected response code: {0}", httpResponse.StatusCode));
                    }
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        return new Tuple<int, string>((int)httpResponse.StatusCode, streamReader.ReadToEnd());
                    }
                }
            }
            catch (Exception ex)
            {
                AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/jsonpostexception", "Couldn't send message due to exception: {0}", ex.Message));
                return new Tuple<int, string>(-1, "");
            }
        }

        protected static ArgumentException InvalidEnumException(string enumName, string enumValue)
        {
            return new ArgumentException(
                I18n.Translate(
                    "internal/Action/invalidEnumType",
                    "{0} = {1} is not a known enum type.\n\n" +
                    "This may be because your Triggernometry plugin is not up to date, or the data you are trying to import is corrupted.",
                    enumName, enumValue)
                );
        }

        /// <summary> "Not implemented for [ActionType] enum [EnumName]." </summary>
        protected string NotImplementedEnumMessage<T>(T @enum) where T : Enum
            => $"Not implemented for {GetType().Name} enum {@enum}.";

        /// <summary> "Not implemented for [ActionType] enum [EnumName]." </summary>
        protected Exception NotImplementedEnumException<T>(T @enum) where T : Enum
            => new NotImplementedException(NotImplementedEnumMessage(@enum));

        #endregion

        /// <summary>Enumerates actions and their loop descendants in tree order.</summary>
        internal static IEnumerable<ActionBase> RecursiveGetActions(IEnumerable<ActionBase> actions)
        {
            foreach (ActionBase action in actions)
            {
                yield return action;
                if (action is ActionLoop loop)
                {
                    foreach (ActionBase child in RecursiveGetActions(loop.LoopActions))
                    {
                        yield return child;
                    }
                }
            }
        }

        /// <summary>Restores trigger ownership and condition parent links for an action subtree.</summary>
        /// <param name="parentPosition">An empty string numbers the complete trigger list; null preserves existing positions.</param>
        internal static void RebuildParentage(IEnumerable<ActionBase> actions, Trigger trigger, string parentPosition = null)
        {
            int position = 0;
            foreach (ActionBase action in actions)
            {
                if (parentPosition != null)
                {
                    string index = (++position).ToString(CultureInfo.InvariantCulture);
                    action.IndexInTrigger = parentPosition.Length == 0 ? index : parentPosition + "-" + index;
                }
                action.ParentTrigger = trigger;
                if (action.Condition != null) ConditionGroup.RebuildParentage(action.Condition);
                if (action is ActionLoop loop)
                {
                    if (loop.LoopCondition != null) ConditionGroup.RebuildParentage(loop.LoopCondition);
                    RebuildParentage(loop.LoopActions, trigger, parentPosition == null ? null : action.IndexInTrigger);
                }
            }
        }

        public ActionBase Copy()
        {
            var clone = (ActionBase)Activator.CreateInstance(GetType());
            CopyCommonPropertiesTo(clone);
            CopySpecificPropertiesTo(clone);
            return clone;
        }

        /// <summary>
        /// Copies all generic, non-attribute-based settings from this action into another <see cref="ActionBase"/> instance.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is <c>null</c>.</exception>
        internal void CopyCommonPropertiesTo(ActionBase action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            action.Enabled = Enabled;
            action.Id = Id;
            action.ParentTrigger = ParentTrigger;
            action.IndexInTrigger = IndexInTrigger;
            action.Condition = (ConditionGroup)Condition?.Duplicate();
            action.Tag = Tag;
            action.ReplaceQueued = ReplaceQueued;
            action.PreventDuplicateQueue = PreventDuplicateQueue;
            action.Uninterruptible = Uninterruptible;
            action.ExecDelay = ExecDelay;
            action.Async = Async;
            action.DebugLevel = DebugLevel;
            action.Desc = Desc;
            action.DescBackColor = DescBackColor;
            action.DescTextColor = DescTextColor;
            action.DescOverride = DescOverride;
        }

        /// <summary>
        /// Thread-safe cache storing compiled property-copy delegates for each <see cref="ActionBase"/> subclass type.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Action<ActionBase, ActionBase>> _copyCache
            = new ConcurrentDictionary<Type, Action<ActionBase, ActionBase>>();

        /// <summary>
        /// Copies all subclass-specific properties with <see cref="ActionSpecificPropertyAttribute"/> from this instance into <paramref name="clone"/>.  <br />
        /// Builds and caches an optimized compiled delegate per type.
        /// </summary>
        internal virtual void CopySpecificPropertiesTo(ActionBase clone)
        {
            var type = GetType();

            var copier = _copyCache.GetOrAdd(type, t =>
            {
                var src = Expression.Parameter(typeof(ActionBase), "src");
                var dst = Expression.Parameter(typeof(ActionBase), "dst");
                var srcCast = Expression.Convert(src, t);
                var dstCast = Expression.Convert(dst, t);

                var assigns = new List<Expression>();
                var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

                foreach (var prop in t.GetProperties(flags))
                {
                    if (prop.GetCustomAttribute<ActionSpecificPropertyAttribute>() == null)
                        continue;

                    // Build dst.Prop = src.Prop;
                    var srcAccess = Expression.Property(srcCast, prop);
                    var dstAccess = Expression.Property(dstCast, prop);
                    assigns.Add(Expression.Assign(dstAccess, srcAccess));
                }

                // Support actions without specific settings, such as placeholders.
                assigns.Add(Expression.Empty());
                var body = Expression.Block(assigns);
                var lambda = Expression.Lambda<Action<ActionBase, ActionBase>>(body, src, dst);
                return lambda.Compile();
            });

            copier(this, clone);
        }
    }

}
