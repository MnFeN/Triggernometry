using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Triggernometry.Core.Actions;
using Triggernometry.Core.Conditions;
using Triggernometry.Core.Serialization;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;
using static Triggernometry.Core.ActionOld;
using static Triggernometry.Core.RealPlugin;

namespace Triggernometry.Core
{

    [XmlInclude(typeof(ActionActInteraction))]
    [XmlInclude(typeof(ActionBeep))]
    [XmlInclude(typeof(ActionDiscordWebhook))]
    [XmlInclude(typeof(ActionDiskOperation))]
    [XmlInclude(typeof(ActionExecuteScript))]
    [XmlInclude(typeof(ActionFolderOperation))]
    [XmlInclude(typeof(ActionJsonRequest))]
    [XmlInclude(typeof(ActionKeypress))]
    [XmlInclude(typeof(ActionLaunchProcess))]
    [XmlInclude(typeof(ActionLiveSplitControl))]
    [XmlInclude(typeof(ActionLogMessage))]
    [XmlInclude(typeof(ActionLoop))]
    [XmlInclude(typeof(ActionMessageBox))]
    [XmlInclude(typeof(ActionMouse))]
    [XmlInclude(typeof(ActionMutex))]
    [XmlInclude(typeof(ActionNamedCallback))]
    [XmlInclude(typeof(ActionObsControl))]
    [XmlInclude(typeof(ActionOverlayImage))]
    [XmlInclude(typeof(ActionOverlayText))]
    [XmlInclude(typeof(ActionPlaceholder))]
    [XmlInclude(typeof(ActionPlaySound))]
    [XmlInclude(typeof(ActionPlaySpeech))]
    [XmlInclude(typeof(ActionRepository))]
    [XmlInclude(typeof(ActionTriggerOperation))]
    [XmlInclude(typeof(ActionVariableDict))]
    [XmlInclude(typeof(ActionVariableList))]
    [XmlInclude(typeof(ActionVariableScalar))]
    [XmlInclude(typeof(ActionVariableTable))]
    [XmlInclude(typeof(ActionWindowMessage))]
    public abstract class ActionBase
    {

        #region Classes and enums

        /// <summary>
        /// This class attribute determines the category in which the action belongs into.
        /// The category in turn is used by the trigger action editor to assign it to the right menu.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
        public class ActionCategory : Attribute
        {

            public enum CategoryTypeEnum
            {
                /// <summary>
                /// Miscellaneous actions that don't really belong into any other group
                /// </summary>
                Miscellaneous,
                /// <summary>
                /// Audio actions (TTS, play sound, ..)
                /// </summary>
                Audio,
                /// <summary>
                /// Overlay actions (show image, show text, ..)
                /// </summary>
                Overlay,
                /// <summary>
                /// Communication actions (Discord webhook, JSON requests, ..)
                /// </summary>
                Networking,
                /// <summary>
                /// Variable actions (scalars, lists, ..)
                /// </summary>
                Variable,
                /// <summary>
                /// File actions (read file, ..)
                /// </summary>
                File,
                /// <summary>
                /// External application remote control actions (OBS, LiveSplit, ..)
                /// </summary>
                RemoteControl,
                /// <summary>
                /// Programming types (scripting, mutex, ..)
                /// </summary>
                Programming,
                /// <summary>
                /// Input types (keypress, mouse, ..)
                /// </summary>
                Input,
            }

            internal CategoryTypeEnum _categoryType;

            public ActionCategory(CategoryTypeEnum categoryType = CategoryTypeEnum.Miscellaneous)
            {
                _categoryType = categoryType;
            }

        }

        /// <summary>
        /// This property attribute controls how the generic action property editor will treat and display a specific property
        /// </summary>
        [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
        public class ActionAttribute : Attribute
        {

            public enum SpecialTypeEnum
            {
                None,
                /// <summary>
                /// For GUID type, means that this guid is a reference to a trigger
                /// </summary>
                TriggerReference,
                /// <summary>
                /// For GUID type, means that this guid is a reference to a folder
                /// </summary>
                FolderReference,
                /// <summary>
                /// For GUID type, means that this guid is a reference to a remote repository
                /// </summary>
                RepoReference,
                /// <summary>
                /// For string type, means that this is a path to a generic file
                /// </summary>
                FileSelector,
                /// <summary>
                /// For string type, means that this is a path to an executable
                /// </summary>
                ExecutableSelector,
                /// <summary>
                /// For string type, means that this is a path to an audio file
                /// </summary>
                AudioSelector,
                /// <summary>
                /// For string type, means that this is a path to an image file
                /// </summary>
                ImageSelector,
                /// <summary>
                /// For string type, means that a keypress recorder should be provided
                /// </summary>
                KeypressRecorder
            }

            /// <summary>
            /// Display order number (smallest first)
            /// </summary>
            internal int _order;

            /// <summary>
            /// Type hint for the generic editor, can be different from the underlying datatype.
            /// For example, underlying data might be string, but we want to have an editor for numeric expression.
            /// If null, type is taken from underlying datatype.
            /// </summary>
            internal Type _typehint;

            /// <summary>
            /// Setting that applies special meaning to properties of specific types, namely System.Guid and System.String
            /// </summary>
            internal SpecialTypeEnum _specialtype;

            public ActionAttribute(int order = 0, Type typehint = null, SpecialTypeEnum specialtype = SpecialTypeEnum.None)
            {
                _order = order;
                _typehint = typehint;
                _specialtype = specialtype;
            }

        }

        /// <summary>
        /// Helper class for ComboBox/CheckedListBox for binding Enum values to a specific item
        /// </summary>
        public class EnumBinding
        {

            public string Text { get; set; }
            public PropertyInfo Prop { get; set; }
            public string EnumValueName { get; set; }

            public override string ToString()
            {
                return Text;
            }

        }

        /// <summary>
        /// When an action is queued for execution, it's wrapped in an ActionInstance that carries all the relevant data for execution.
        /// </summary>
        public class ActionInstance : IComparable
        {

            internal DateTime when { get; set; }
            internal long ordinal { get; set; }
            internal MutexInformation mutex { get; set; }
            internal ActionOld act { get; set; }
            internal Context ctx { get; set; }
            internal bool releaseMutex { get; set; } = false;

            public ActionInstance(DateTime when, long ordinal, MutexInformation mtx, ActionOld act, Context ctx, bool releaseMutex)
            {
                this.when = when;
                this.ordinal = ordinal;
                mutex = mtx;
                this.act = act;
                this.ctx = ctx;
                this.releaseMutex = releaseMutex;
            }

            public int CompareTo(object o)
            {
                ActionInstance b = (ActionInstance)o;
                int ex = when.CompareTo(b.when);
                if (ex != 0)
                {
                    return ex;
                }
                return ordinal.CompareTo(b.ordinal);
            }

            public void ActionFinished()
            {
                if (mutex != null && releaseMutex == true)
                {
                    mutex.Release(ctx);
                }
            }

        }

        #endregion

        #region General properties

        public class ActionBundle
        {
            [XmlArrayItem("Action")]
            public List<ActionOld> Actions { get; set; } = new List<ActionOld>();

            /// <summary>
            /// Serializes a list of <see cref="ActionOld"/> into an XML string. <br />
            /// Multiple <see cref="ActionOld"/>s are wrapped in a <see cref="ActionBundle"/>; <br />
            /// A single <see cref="ActionOld"/> is serialized as a single <see cref="ActionOld"/>. <br />
            /// Returns an empty string for null or empty input.
            /// </summary>
            internal static string ActionsToXml(List<ActionOld> actions)
            {
                if (actions == null || actions.Count == 0) return string.Empty;

                object toSerialize;
                XmlSerializer xs;

                if (actions.Count > 1)
                {
                    var ab = new ActionBundle();
                    ab.Actions.AddRange(actions);
                    ab.Actions.Sort((a, b) => a.OrderNumber.CompareTo(b.OrderNumber));
                    toSerialize = ab;
                    xs = new XmlSerializer(typeof(ActionBundle));
                }
                else // single Action
                {
                    toSerialize = actions[0];
                    xs = new XmlSerializer(typeof(ActionOld));
                }

                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                using (var sw = new StringWriter())
                {
                    xs.Serialize(sw, toSerialize, ns);
                    return sw.ToString();
                }
            }

            /// <summary>
            /// Deserializes XML containing either an <see cref="ActionBundle"/> or a single <see cref="ActionOld"/>, <br />
            /// and returns a <see cref="List{Action}"/> of <see cref="ActionOld"/> in its original order.
            /// </summary>
            internal static List<ActionOld> XmlToActions(string xmlData)
            {
                var result = new List<ActionOld>();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);
                using (var sr = new StringReader(xmlData))
                {
                    if (xmlDoc.DocumentElement.Name == "ActionBundle")
                    {
                        var bundleSerializer = new XmlSerializer(typeof(ActionBundle));
                        var bundle = (ActionBundle)bundleSerializer.Deserialize(sr);
                        if (bundle?.Actions != null)
                            result.AddRange(bundle.Actions);
                    }
                    else // single Action
                    {
                        var actionSerializer = new XmlSerializer(typeof(ActionOld));
                        var a = (ActionOld)actionSerializer.Deserialize(sr);
                        if (a != null)
                            result.Add(a);
                    }
                }
                return result;
            }

        }

        internal ActionOld NextAction { get; set; } = null;

        /// <summary> 循环的父动作 </summary>
        internal ActionOld LoopAction { get; set; } = null;
        //internal Guid LoopContext { get; set; } = Guid.Empty;

        /// <summary>
        /// for queue controlling
        /// </summary>
        internal Guid Id { get; set; } = Guid.NewGuid();

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
        public string ExecutionDelayExpression { get; set; } = "0";

        [XmlAttribute("ExecutionDelayExpression")]
        public string Xml_ExecutionDelayExpression
        {
            get => XmlAttr.String(ExecutionDelayExpression, "0");
            set => ExecutionDelayExpression = value;
        }



        [XmlIgnore]
        public bool Asynchronous { get; set; } = true;

        [XmlAttribute("Asynchronous")]
        public string Xml_Asynchronous
        {
            get => XmlAttr.Bool(Asynchronous, true);
            set => Asynchronous = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        public DebugLevelEnum DebugLevel { get; set; } = DebugLevelEnum.Inherit;

        [XmlAttribute("DebugLevel")]
        public string Xml_DebugLevel
        {
            get => XmlAttr.Enum(DebugLevel, DebugLevelEnum.Inherit);
            set => DebugLevel = XmlAttr.Enum<DebugLevelEnum>(value);
        }



        [XmlIgnore]
        public bool RefireInterrupt { get; set; } = false;

        [XmlAttribute("RefireInterrupt")]
        public string Xml_RefireInterrupt
        {
            get => XmlAttr.Bool(RefireInterrupt, false);
            set => RefireInterrupt = XmlAttr.Bool(value);
        }



        [XmlIgnore]
        public bool RefireRequeue { get; set; } = true;

        [XmlAttribute("RefireRequeue")]
        public string Xml_RefireRequeue
        {
            get => XmlAttr.Bool(RefireRequeue, true);
            set => RefireRequeue = XmlAttr.Bool(value);
        }



        [XmlAttribute]
        public int OrderNumber;



        [XmlIgnore]
        public string Description { get; set; } = "";

        [XmlAttribute("Description")]
        public string Xml_Description
        {
            get => XmlAttr.String(Description);
            set => Description = value;
        }



        [XmlIgnore]
        public string DescBgColor { get; set; } = "";

        [XmlAttribute("DescBgColor")]
        public string Xml_DescBgColor
        {
            get => XmlAttr.String(DescBgColor);
            set => DescBgColor = value;
        }



        [XmlIgnore]
        public string DescTextColor { get; set; } = "";

        [XmlAttribute("DescTextColor")]
        public string Xml_DescTextColor
        {
            get => XmlAttr.String(DescTextColor);
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
        public string Tag { get; set; }

        [XmlAttribute("Tag")]
        public string Xml_Tag
        {
            get => XmlAttr.String(Tag);
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

        internal abstract void ExecuteImplementation(ActionInstance ai);
        public void Execute(ActionInstance ai)
        {            
            if (Enabled == false)
            {
                LastExecutionResult = false;
                return;
            }
            Context ctx = ai.ctx;
            if ((ctx.forceType & ActionOld.TriggerForceTypeEnum.SkipConditions) == 0 && ctx.testByPlaceholder == false)
            {
                if (Condition != null && Condition.Enabled == true)
                {
                    if (Condition.CheckCondition(ctx, ActionContextLogger, ctx) == false)
                    {
                        AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/actionnotfired", "Action #{0} on trigger '{1}' not fired, condition not met", OrderNumber, ctx.Trigger?.LogName ?? "(null)"));
                        LastExecutionResult = false;
                        return;
                    }
                }
            }
            if (Asynchronous == true)
            {
                Task t;
                if (ctx.Plugin != null)
                {
                    CancellationToken ct = ctx.Plugin.GetCancellationToken();
                    t = new Task(() =>
                    {
                        ct.ThrowIfCancellationRequested();
                        AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/executingaction", "Executing action '{0}' in thread {1}", Describe(), Thread.CurrentThread.ManagedThreadId));
                        ExecuteImplementation(ai);
                        ai?.ActionFinished();
                    });
                }
                else
                {
                    t = new Task(() =>
                    {                        
                        AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/executingaction", "Executing action '{0}' in thread {1}", Describe(), Thread.CurrentThread.ManagedThreadId));
                        ExecuteImplementation(ai);
                        ai?.ActionFinished();
                    });
                }
                t.Start();
            }
            else
            {                
                AddToLog(ctx, DebugLevelEnum.Verbose, I18n.Translate("internal/Action/executingaction", "Executing action '{0}' in thread {1}", Describe(), Thread.CurrentThread.ManagedThreadId));
                ExecuteImplementation(ai);
                ai?.ActionFinished();
            }
            LastExecutionResult = true;
        }

        internal DebugLevelEnum GetDebugLevel(Context ctx)
        {
            if (DebugLevel == DebugLevelEnum.Inherit)
            {
                return ctx.Trigger?.GetDebugLevel(Instance) ?? DebugLevelEnum.Verbose;
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
            Instance.UnfilteredAddToLog(level, message, ParentTrigger); // to-do: should pass Action instance as well
        }

        // todo should get rid of this maybe

        #endregion

        #region Obsoletes, under construction, etc

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
                            ? indices.OrderBy(i => Convert.ToDouble(values[k][i]))
                            : indices.OrderByDescending(i => Convert.ToDouble(values[k][i]));
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
                            ? sortedIndices.ThenBy(i => Convert.ToDouble(values[k][i]))
                            : sortedIndices.ThenByDescending(i => Convert.ToDouble(values[k][i]));
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

        protected Tuple<int, string> SendJson(Context ctx, ActionOld.HTTPMethodEnum method, string url, string json, IEnumerable<string> headers, bool expectNoContent)
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
                    case HTTPMethodEnum.POST:
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        break;
                    case HTTPMethodEnum.GET:
                        httpWebRequest.Method = "GET";
                        break;
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode != HttpStatusCode.NoContent && expectNoContent == true)
                {
                    AddToLog(ctx, DebugLevelEnum.Error, I18n.Translate("internal/Action/jsonpostunexpectedresponse", "Unexpected response code: {0}", httpResponse.StatusCode));
                }
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return new Tuple<int, string>((int)httpResponse.StatusCode, streamReader.ReadToEnd());
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

        #region Action-specific property management

        /// <summary>
        /// Builds a list of all properties on the action that have ActionAttribute set to them
        /// </summary>
        /// <returns>Tuple containing the PropertyInfo, its displayorder, and suggested editor type</returns>
        private List<(PropertyInfo prop, ActionAttribute attr)> GetProperties()
        {
            PropertyInfo[] props = GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            List<(PropertyInfo prop, ActionAttribute attr)> results = new List<(PropertyInfo prop, ActionAttribute attr)>();
            foreach (PropertyInfo pi in props)
            {
                ActionAttribute aa = pi.GetCustomAttributes<ActionAttribute>().FirstOrDefault();
                if (aa == null)
                {
                    continue;
                }
                // attribute can hint at what kind of editor to provide for this property
                // (which can be different from the underlying data type)                
                if (aa._typehint == null)
                {
                    // determine editor type from property type instead
                    object val = pi.GetValue(this);
                    aa._typehint = val.GetType();
                    if (aa._typehint.IsEnum)
                    {
                        aa._typehint = typeof(Enum);
                    }
                }
                results.Add((
                    prop: pi,
                    attr: aa
                ));
            }
            results.Sort((a, b) => a.attr._order.CompareTo(b.attr._order));
            return results;
        }

        /// <summary>
        /// Updates underlying property value when expression textbox contents change
        /// </summary>
        /// <param name="sender">ExpressionTextBox</param>
        /// <param name="e">Unused</param>
        private void Etb_TextChanged(object sender, EventArgs e)
        {
            ExpressionTextBox etb = (ExpressionTextBox)sender;
            PropertyInfo pi = (PropertyInfo)etb.Tag;
            pi.SetValue(this, etb.Text);
        }

        /// <summary>
        /// Updates underlying property value when checkbox state changes
        /// </summary>
        /// <param name="sender">CheckBox</param>
        /// <param name="e">Unused</param>
        private void Cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            PropertyInfo pi = (PropertyInfo)cb.Tag;
            if (pi.PropertyType == typeof(bool))
            {
                pi.SetValue(this, cb.Checked);
            }
            else
            {
                pi.SetValue(this, cb.Checked.ToString());
            }
        }

        /// <summary>
        /// Updates underlying enum value when combobox state changes
        /// </summary>
        /// <param name="sender">ComboBox</param>
        /// <param name="e">Unused</param>
        private void Cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            EnumBinding eb = (EnumBinding)cb.SelectedItem;
            PropertyInfo pi = eb.Prop;
            pi.SetValue(this, Enum.Parse(pi.PropertyType, eb.EnumValueName));
        }

        /// <summary>
        /// Updates underlying enum value when checkedlistbox state changes
        /// </summary>
        /// <param name="sender">CheckedListBox</param>
        /// <param name="e">Unused</param>        
        private void Clb_ItemCheck(object sender, ItemCheckEventArgs e)
        {            
            CheckedListBox clb = (CheckedListBox)sender;
            int newval = 0;
            PropertyInfo pi = (PropertyInfo)clb.Tag;            
            for (int i = 0; i < clb.Items.Count; i++)            
            {
                if (i == e.Index)
                {
                    if (e.NewValue == CheckState.Unchecked)
                    {
                        continue;
                    }
                }
                else if (clb.GetItemChecked(i) == false)
                {
                    continue;
                }
                EnumBinding eb = (EnumBinding)clb.Items[i];
                int thisval = (int)Enum.Parse(pi.PropertyType, eb.EnumValueName);
                newval |= thisval;
            }
            pi.SetValue(this, newval);
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            (PropertyInfo prop, ActionAttribute attr, ExpressionTextBox target) prop = ((PropertyInfo prop, ActionAttribute attr, ExpressionTextBox target))b.Tag;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                string curvalue = (string)prop.prop.GetValue(this);
                ofd.FileName = curvalue;
                switch (prop.attr._specialtype)
                {
                    case ActionAttribute.SpecialTypeEnum.FileSelector:
                        ofd.Title = "Select file"; // todo i18n
                        ofd.Filter = "All files (*.*)|*.*";
                        break;
                    case ActionAttribute.SpecialTypeEnum.ExecutableSelector:
                        ofd.Title = "Select executable"; // todo i18n
                        ofd.Filter = "Executables (*.exe)|*.exe|All files (*.*)|*.*";
                        break;
                    case ActionAttribute.SpecialTypeEnum.AudioSelector:
                        ofd.Title = "Select audio file"; // todo i18n
                        ofd.Filter = "Sound files (*.wav, *.mp3)|*.wav;*.mp3|All files (*.*)|*.*";
                        break;
                    case ActionAttribute.SpecialTypeEnum.ImageSelector:
                        ofd.Title = "Select image file"; // todo i18n
                        ofd.Filter = "Image files (*.gif, *.bmp, *.png, *.jpg, *.jpeg)|*.gif;*.bmp;*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
                        break;
                }
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    prop.target.Text = ofd.FileName;
                }
            }
        }

        private Control GetGenericPropertyEditor()
        {
            var props = GetProperties();
            List<(PropertyInfo prop, ActionAttribute attr, object ctrl)> propeditors = new List<(PropertyInfo prop, ActionAttribute attr, object ctrl)>();
            foreach (var prop in props)
            {
                Control temp = GetPropertyEditor(prop);
                if (temp != null)
                {
                    propeditors.Add((prop.prop, prop.attr, ctrl: temp));
                    continue;
                }
                if (prop.attr._typehint == typeof(string))
                {
                    switch (prop.attr._specialtype)
                    {
                        case ActionAttribute.SpecialTypeEnum.FileSelector:
                        case ActionAttribute.SpecialTypeEnum.ExecutableSelector:
                        case ActionAttribute.SpecialTypeEnum.AudioSelector:
                        case ActionAttribute.SpecialTypeEnum.ImageSelector:                        
                            {
                                // show file selector
                                ExpressionTextBox etb = new ExpressionTextBox();
                                etb.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                                etb.Expression = (string)prop.prop.GetValue(this);
                                etb.TextChanged += Etb_TextChanged;
                                etb.Tag = prop.prop;
                                Button browseBtn = new Button();
                                browseBtn.Dock = DockStyle.Fill;
                                browseBtn.Click += BrowseBtn_Click;
                                browseBtn.Tag = (prop.prop, prop.attr, target: etb);
                                propeditors.Add((prop.prop, prop.attr, ctrl: new object[] { etb, browseBtn }));
                                break;
                            }
                        case ActionAttribute.SpecialTypeEnum.KeypressRecorder:
                            {
                                // todo show keypress recorder
                                break;
                            }
                        default:
                            {
                                // show string expression field
                                ExpressionTextBox etb = new ExpressionTextBox();
                                etb.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                                etb.Expression = (string)prop.prop.GetValue(this);
                                etb.TextChanged += Etb_TextChanged;
                                etb.Tag = prop.prop;
                                propeditors.Add((prop.prop, prop.attr, ctrl: etb));
                                break;
                            }
                    }
                }
                else if (prop.attr._typehint == typeof(int) || prop.attr._typehint == typeof(uint) || prop.attr._typehint == typeof(float))
                {
                    // show numeric expression field
                    ExpressionTextBox etb = new ExpressionTextBox();
                    etb.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Numeric;
                    etb.Expression = (string)prop.prop.GetValue(this);
                    etb.TextChanged += Etb_TextChanged;
                    etb.Tag = prop.prop;
                    propeditors.Add((prop.prop, prop.attr, ctrl: etb));
                }
                else if (prop.attr._typehint == typeof(Regex))
                {
                    // show regex expression field
                    ExpressionTextBox etb = new ExpressionTextBox();
                    etb.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Regex;
                    etb.Expression = (string)prop.prop.GetValue(this);
                    etb.TextChanged += Etb_TextChanged;
                    etb.Tag = prop.prop;
                    propeditors.Add((prop.prop, prop.attr, ctrl: etb));
                }
                else if (prop.attr._typehint == typeof(bool))
                {
                    // show checkbox
                    CheckBox cb = new CheckBox();
                    cb.Text = "";
                    cb.CheckAlign = ContentAlignment.MiddleRight;
                    if (prop.prop.PropertyType == typeof(bool))
                    {
                        cb.Checked = (bool)prop.prop.GetValue(this);
                    }
                    else
                    {
                        cb.Checked = bool.Parse(prop.prop.GetValue(this).ToString());
                    }
                    cb.CheckedChanged += Cb_CheckedChanged;
                    cb.Tag = prop.prop;
                    propeditors.Add((prop.prop, prop.attr, ctrl: cb));
                }
                else if (prop.attr._typehint == typeof(Enum))
                {
                    if (prop.prop.PropertyType.IsDefined(typeof(FlagsAttribute), true) == true)
                    {
                        // for flags enums, show checkedlistbox
                        CheckedListBox clb = new CheckedListBox();
                        clb.CheckOnClick = true;                        
                        string[] names = Enum.GetNames(prop.prop.PropertyType);
                        int curval = (int)prop.prop.GetValue(this);
                        foreach (string name in names)
                        {
                            // for example, "Internal/Enum/ActionTriggerOperation/ForceEnum/SkipConditions"
                            int thisval = (int)Enum.Parse(prop.prop.PropertyType, name);
                            if (thisval != 0 && (thisval & thisval - 1) == 0)
                            {
                                // show only single bit values
                                string trkey = "Internal/Enum/" + prop.prop.PropertyType.DeclaringType.Name + "/" + prop.prop.PropertyType.Name + "/" + name;
                                EnumBinding eb = new EnumBinding() { Text = trkey, Prop = prop.prop, EnumValueName = name };
                                clb.Items.Add(eb);
                                if ((thisval & curval) != 0)
                                {
                                    clb.SetItemChecked(clb.Items.Count - 1, true);
                                }
                            }
                        }
                        clb.Tag = prop.prop;
                        clb.ItemCheck += Clb_ItemCheck;
                        clb.Height = clb.GetItemHeight(0) * 4;
                        propeditors.Add((prop.prop, prop.attr, ctrl: clb));
                    }
                    else
                    {
                        // for regular enums, show combobox
                        ComboBox cb = new ComboBox();
                        cb.DropDownStyle = ComboBoxStyle.DropDownList;
                        string[] names = Enum.GetNames(prop.prop.PropertyType);
                        string curval = prop.prop.GetValue(this).ToString();
                        foreach (string name in names)
                        {
                            // for example, "Internal/Enum/ActionActInteraction/OperationEnum/SetCombatState"
                            string trkey = "Internal/Enum/" + prop.prop.PropertyType.DeclaringType.Name + "/" + prop.prop.PropertyType.Name + "/" + name;
                            EnumBinding eb = new EnumBinding() { Text = trkey, Prop = prop.prop, EnumValueName = name };
                            cb.Items.Add(eb);
                            if (curval == name)
                            {
                                cb.SelectedItem = eb;
                            }
                        }
                        cb.SelectedIndexChanged += Cb_SelectedIndexChanged;
                        propeditors.Add((prop.prop, prop.attr, ctrl: cb));
                    }
                }
                else if (prop.attr._typehint == typeof(Guid))
                {
                    // this is a reference to something, check specialtype for what it is
                    switch (prop.attr._specialtype)
                    {
                        case ActionAttribute.SpecialTypeEnum.TriggerReference:
                            // todo show trigger selector
                            break;
                        case ActionAttribute.SpecialTypeEnum.FolderReference:
                            // todo show folder selector
                            break;
                        case ActionAttribute.SpecialTypeEnum.RepoReference:
                            // todo show repository selector
                            break;
                    }
                }
            }
            if (propeditors.Count == 0)
            {
                // no properties to edit
                return null;
            }
            // the generic property editor is a TableLayoutPanel, where
            // - first column is AutoSize for Labels
            // - second column is 100 % for content
            // - third optional column is 50px for a button or some such, use ColumnSpan 2 on second column if not needed
            TableLayoutPanel tlp = new TableLayoutPanel();
            tlp.ColumnCount = 3;
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50.0f));
            tlp.RowCount = propeditors.Count;
            for (int i = 0; i < propeditors.Count; i++)
            {                
                tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                Label l = new Label();
                // for example, "Internal/Property/ActionActInteraction/Operation"
                string trkey = "Internal/Property/" + GetType().Name + "/" + Regex.Replace(propeditors[i].prop.Name, "[^a-zA-Z0-9]", "");
                l.Text = trkey;
                l.Dock = DockStyle.Fill;
                l.TextAlign = ContentAlignment.MiddleLeft;
                l.MinimumSize = new Size(150, 0);
                tlp.Controls.Add(l, 0, i);
                (PropertyInfo prop, ActionAttribute attr, object ctrl) pe = propeditors[i];
                if (pe.ctrl is object[])
                {
                    int col = 1;
                    object[] ctrls = (object[])pe.ctrl;
                    foreach (object o in ctrls)
                    {
                        Control ctrl = (Control)o;
                        ctrl.Dock = DockStyle.Top;
                        tlp.Controls.Add(ctrl, col, i);
                        tlp.SetColumnSpan(ctrl, 1);
                        col++;
                    }
                }
                else
                {
                    Control ctrl = (Control)pe.ctrl;
                    ctrl.Dock = DockStyle.Top;
                    tlp.Controls.Add(ctrl, 1, i);
                    tlp.SetColumnSpan(ctrl, 2);
                }
            }
            tlp.Dock = DockStyle.Top;
            tlp.AutoSize = true;
            tlp.BackColor = SystemColors.Highlight;
            return tlp;
        }

        protected virtual Control GetPropertyEditor((PropertyInfo prop, ActionAttribute attr) prop)
        {
            return null;
        }

        /// <summary>
        /// Creates a property editor control for the action. Actions can override this to provide their own entirely custom property editor.
        /// Alternatively, if controls want to use the generic property editor but provide a custom editor only for one parameter, override the PropertyInfo overload instead.
        /// </summary>
        /// <returns>Property editor for the action</returns>
        internal virtual Control GetPropertyEditor()
        {
            return GetGenericPropertyEditor();
        }

        #endregion


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
            action.OrderNumber = OrderNumber;
            action.Condition = (ConditionGroup)Condition?.Duplicate();
            action.Tag = Tag;
            action.RefireInterrupt = RefireInterrupt;
            action.RefireRequeue = RefireRequeue;
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
            oldAction.OrderNumber = OrderNumber;
            oldAction.Condition = (ConditionGroup)Condition?.Duplicate();
            oldAction.Tag = Tag;
            oldAction.RefireInterrupt = RefireInterrupt;
            oldAction.RefireRequeue = RefireRequeue;
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
        /// Copies all subclass-specific properties with <see cref="ActionAttribute"/> from this instance into <paramref name="clone"/>.  <br />
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
                    if (prop.GetCustomAttribute<ActionAttribute>() == null)
                        continue;

                    // Build dst.Prop = src.Prop;
                    var srcAccess = Expression.Property(srcCast, prop);
                    var dstAccess = Expression.Property(dstCast, prop);
                    assigns.Add(Expression.Assign(dstAccess, srcAccess));
                }

                var body = Expression.Block(assigns);
                var lambda = Expression.Lambda<Action<ActionBase, ActionBase>>(body, src, dst);
                return lambda.Compile();
            });

            copier(this, clone);
        }
    }

}
