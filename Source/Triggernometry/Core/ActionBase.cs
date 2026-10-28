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

        /// <summary>
        /// When an action is queued for execution, it's wrapped in an ActionInstance that carries all the relevant data for execution.
        /// </summary>
        public class ActionInstance : IComparable<ActionInstance>
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

            /// <summary>
            /// The effective tag text used by the action,
            /// with expressions evaluated when the action is queued. <br />
            /// If this action’s <see cref="ActionBase.Tag"/> is not specified, or evaluates to a whitespace-string,
            /// the <see cref="Trigger.Tag"/> from the parent trigger will be used instead. <br />
            /// Empty if no tag is specified anywhere.
            /// </summary>
            internal string ParsedTag { get; set; } = "";

            public ActionInstance(DateTime executeAt, long queueSequence, MutexInfo mutextx, ActionBase action, Context ctx, bool shouldReleaseMutex)
            {
                ExecuteAt = executeAt;
                QueueSequence = queueSequence;
                Mutex = mutextx;
                Action = action;
                Context = ctx;
                ShouldReleaseMutex = shouldReleaseMutex;
            }

            /// <summary>
            /// Invoked by <see cref="List.BinarySearch"/> in <see cref="RealPlugin.QueueAction"/>
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
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

            public void ActionFinished()
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
        public string ExecutionDelayExpression { get; set; } = "0";

        [XmlAttribute("ExecutionDelayExpression")]
        public string Xml_ExecutionDelayExpression
        {
            get => XmlAttr.String(ExecutionDelayExpression, "0", omitWhitespace: false);
            set => ExecutionDelayExpression = value;
        }



        [XmlIgnore]
        [CheckBoxEditor(4, Label = "Execute asynchronously without blocking other actions from executing")]
        public bool Asynchronous { get; set; } = false;

        [XmlAttribute("Asynchronous")]
        public string Xml_Asynchronous
        {
            get => XmlAttr.Bool(Asynchronous, false);
            set => Asynchronous = XmlAttr.Bool(value);
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
        public bool RefireInterrupt { get; set; } = false;

        [XmlAttribute("RefireInterrupt")]
        public string Xml_RefireInterrupt
        {
            get => XmlAttr.Bool(RefireInterrupt, false);
            set => RefireInterrupt = XmlAttr.Bool(value);
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



        [XmlIgnore]
        public string Description { get; set; } = "";

        [XmlAttribute("Description")]
        public string Xml_Description
        {
            get => XmlAttr.String(Description, "", omitWhitespace: false);
            set => Description = value;
        }



        [XmlIgnore]
        public string DescBgColor { get; set; } = "";

        [XmlAttribute("DescBgColor")]
        public string Xml_DescBgColor
        {
            get => XmlAttr.String(DescBgColor, "", omitWhitespace: false);
            set => DescBgColor = value;
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
        public bool DescriptionOverride { get; set; } = false;

        [XmlAttribute("DescriptionOverride")]
        public string Xml_DescriptionOverride
        {
            get => XmlAttr.Bool(DescriptionOverride, false);
            set => DescriptionOverride = XmlAttr.Bool(value);
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
                if (DescriptionOverride)
                {
                    return Description ?? "";
                }
                string temp = I18n.TrlAsync(Asynchronous);
                if (!string.IsNullOrWhiteSpace(ExecutionDelayExpression) && ExecutionDelayExpression.Trim() != "0")
                {
                    string delay = double.TryParse(ExecutionDelayExpression.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out _) ? ExecutionDelayExpression : $"({ExecutionDelayExpression})";
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
            get
            {
                return _LastExecutionResult;
            }
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

        /// <summary>Executes a queued instance through the same lifecycle as direct UI tests.</summary>
        public void Execute(ActionInstance ai) => Execute(ai, ai.Context);

        internal void Execute(ActionInstance ai, Context ctx)
        {
            if (ai != null)
            {
                lock (ctx.Plugin.ActionQueue) ctx.Plugin.ExecutingActions.Add(ai);
            }
            CancellationToken? ct = Asynchronous ? ctx.Plugin?.GetCancellationToken() : null;
            void ExecuteCore()
            {
                try
                {
                    ct?.ThrowIfCancellationRequested();
                    ExecutionImplementation(ai ?? new ActionInstance(default, 0, null, this, ctx, false), ctx);
                }
                finally
                {
                    ai?.ActionFinished();
                }
            }
            if (Asynchronous) Task.Run((Action)ExecuteCore);
            else ExecuteCore();
        }

        /// <summary>检查动作条件、执行业务并安排后续动作。</summary>
        private void ExecutionImplementation(ActionInstance ai, Context ctx)
        {
            try
            {
                if ((ctx.forceType & ActionTriggerOperation.ForceEnum.SkipConditions) == 0 && !ctx.testByPlaceholder &&
                    Condition?.Enabled == true && Condition.CheckCondition(ctx, ActionContextLogger, ctx) == false)
                {
                    ctx.PushActionResult(0);
                    AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/actionnotfired", "Action #{0} on trigger '{1}' not fired, condition not met", IndexInTrigger, ctx.Trigger?.LogName ?? "(null)"));
                }
                else
                {
                    ctx.PushActionResult(1);
                    AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/executingaction", "Executing action '{0}' in thread {1}", Describe(), Thread.CurrentThread.ManagedThreadId));

                    if (this is ActionLoop loop)
                    {
                        if (loop.QueueIteration(ai)) return;
                        // A nested loop restores its caller's iterator; a root loop keeps its final value.
                        if (ai.LoopParent == null && ai.LoopContext != null) ctx = ai.LoopContext;
                    }
                    else
                    {
                        ExecuteImplementation(ctx);
                    }
                }

                ActionInstance next = ai.NextAction;
                if (next != null)
                {
                    next.Context = ctx;
                    next.ExecuteAt = DateTime.Now.AddMilliseconds(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, next.Action.ExecutionDelayExpression));
                }
                else if (ai.IsLoopTail)
                {
                    ActionInstance parent = ai.LoopParent;
                    DateTime when = DateTime.Now.AddMilliseconds(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, ((ActionLoop)parent.Action).DelayExpression));
                    next = new ActionInstance(when, 0, ai.Mutex, parent.Action, parent.Context, false)
                    {
                        NextAction = parent.NextAction,
                        LoopParent = parent.LoopParent,
                        LoopContext = parent.LoopContext,
                        IsLoopTail = parent.IsLoopTail
                    };
                }
                if (next != null)
                {
                    lock (ctx.Plugin.ActionQueue)
                    {
                        // Only a successfully queued successor takes over the mutex release.
                        next.ShouldReleaseMutex = ai.ShouldReleaseMutex;
                        if (ctx.Plugin.QueueAction(next)) ai.ShouldReleaseMutex = false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (this is ActionNamedCallback)
                {
                    if (ex is TargetInvocationException tiex && tiex.InnerException != null)
                        ex = tiex.InnerException;
                }
                string triggerPath = ai?.Context?.Trigger?.FullPath ?? "(null)";
                string actionDesc = "";
                try { actionDesc = Describe(); } catch { }
                actionDesc = actionDesc.Length > 300 ? actionDesc.Substring(0, 297) + "..." : actionDesc;
                string detail = ex.FullMessage();

                AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/exception",
                    "Action exception: {0}  \nIn action: {1}  \nIn trigger: {2}{3}",
                    ex.Message, actionDesc, triggerPath, detail));
            }
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

        public void ActionContextLogger(object o, string msg)
        {
            AddToLog((Context)o, DebugLevelEnum.Verbose, msg);
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

        /// <summary>Converts this action's settings to the legacy model through its concrete converter.</summary>
        internal ActionOld ConvertToOldAction()
        {
            switch (this)
            {
                case ActionBeep value:
                    return (ActionOld)value;
                case ActionPlaySound value:
                    return (ActionOld)value;
                case ActionPlaySpeech value:
                    return (ActionOld)value;
                case ActionMessageBox value:
                    return (ActionOld)value;
                case ActionOverlayText value:
                    return (ActionOld)value;
                case ActionOverlayImage value:
                    return (ActionOld)value;
                case ActionVariableScalar value:
                    return (ActionOld)value;
                case ActionVariableList value:
                    return (ActionOld)value;
                case ActionVariableTable value:
                    return (ActionOld)value;
                case ActionVariableDict value:
                    return (ActionOld)value;
                case ActionLogMessage value:
                    return (ActionOld)value;
                case ActionNamedCallback value:
                    return (ActionOld)value;
                case ActionExecuteScript value:
                    return (ActionOld)value;
                case ActionLoop value:
                    return (ActionOld)value;
                case ActionMutex value:
                    return (ActionOld)value;
                case ActionTriggerOperation value:
                    return (ActionOld)value;
                case ActionFolderOperation value:
                    return (ActionOld)value;
                case ActionRepository value:
                    return (ActionOld)value;
                case ActionMouse value:
                    return (ActionOld)value;
                case ActionKeypress value:
                    return (ActionOld)value;
                case ActionWindowMessage value:
                    return (ActionOld)value;
                case ActionLaunchProcess value:
                    return (ActionOld)value;
                case ActionDiskOperation value:
                    return (ActionOld)value;
                case ActionActInteraction value:
                    return (ActionOld)value;
                case ActionJsonRequest value:
                    return (ActionOld)value;
                case ActionDiscordWebhook value:
                    return (ActionOld)value;
                case ActionObsControl value:
                    return (ActionOld)value;
                case ActionLiveSplitControl value:
                    return (ActionOld)value;
                case ActionPlaceholder value:
                    return (ActionOld)value;
                default:
                    throw new NotSupportedException(GetType().FullName);
            }
        }

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
            action.RefireInterrupt = RefireInterrupt;
            action.PreventDuplicateQueue = PreventDuplicateQueue;
            action.ExecutionDelayExpression = ExecutionDelayExpression;
            action.Asynchronous = Asynchronous;
            action.DebugLevel = DebugLevel;
            action.Description = Description;
            action.DescBgColor = DescBgColor;
            action.DescTextColor = DescTextColor;
            action.DescriptionOverride = DescriptionOverride;
        }

        internal void CopyCommonPropertiesTo(ActionOld oldAction)
        {
            if (oldAction == null)
                throw new ArgumentNullException(nameof(oldAction));

            oldAction.Enabled = Enabled;
            oldAction.Id = Id;
            oldAction.ParentTrigger = ParentTrigger;
            oldAction.Condition = (ConditionGroup)Condition?.Duplicate();
            oldAction.Tag = Tag;
            oldAction.RefireInterrupt = RefireInterrupt;
            oldAction.RefireRequeue = !PreventDuplicateQueue;
            oldAction.ExecutionDelayExpression = ExecutionDelayExpression;
            oldAction.Asynchronous = Asynchronous;
            oldAction.DebugLevel = DebugLevel;
            oldAction.Description = Description;
            oldAction.DescBgColor = DescBgColor;
            oldAction.DescTextColor = DescTextColor;
            oldAction.DescriptionOverride = DescriptionOverride;
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
