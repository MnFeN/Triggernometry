using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Core.Variables;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Scalar variable operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Variable)]
    [XmlRoot(ElementName = "VariableScalar")]
    public class ActionVariableScalar : ActionBase
    {

        #region Properties

        /// <summary>
        /// Scalar variable operations
        /// </summary>
        public enum OperationEnum
        {
            /// <summary>
            /// Unset scalar variable
            /// </summary>
            Unset,
            /// <summary>
            /// Set scalar variable value according to string expression
            /// </summary>
            SetString,
            /// <summary>
            /// Set scalar variable value according to numeric expression
            /// </summary>
            SetNumeric,
            /// <summary>
            /// Increment scalar variable by value if defined, 1 otherwise
            /// </summary>
            Increment,
            /// <summary>
            /// Copy value from scalar variable to clipboard
            /// </summary>
            Clipboard,
            /// <summary>
            /// Unset all scalar variables
            /// </summary>
            UnsetAll,
            /// <summary>
            /// Unset all scalar variables with names matching given regex
            /// </summary>
            UnsetRegex,
            // todo should not be here
            UnsetRegexUniversal,
            /// <summary>
            /// Query scalar variable with JSONPath, and store result in scalar variable
            /// </summary>
            QueryJsonPath,
            /// <summary>
            /// Query scalar variable with JSONPath, and store result in list variable
            /// </summary>
            QueryJsonPathList
        }

        /// <summary>
        /// Scalar variable operation type
        /// </summary>
        [XmlIgnore]
        [Action(order: 1)]
        public OperationEnum Operation { get; set; } = OperationEnum.Unset;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.Unset);
            set => Operation = XmlAttr.Enum<OperationEnum>(value);
        }

        /// <summary>
        /// Name of the scalar variable
        /// </summary>
        [XmlIgnore]
        [Action(order: 2)]
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name);
            set => Name = value;
        }

        /// <summary>
        /// Name of the target variable for some JSON operations
        /// </summary>
        [XmlIgnore]
        [Action(order: 3)]
        public string JsonTargetName { get; set; } = "";

        [XmlAttribute("JsonTargetName")]
        public string Xml_JsonTargetName
        {
            get => XmlAttr.String(JsonTargetName);
            set => JsonTargetName = value;
        }

        /// <summary>
        /// Value expression
        /// </summary>
        [XmlIgnore]
        [Action(order: 4)]
        public string Value { get; set; } = "";

        [XmlAttribute("Value")]
        public string Xml_Value
        {
            get => XmlAttr.String(Value);
            set => Value = value;
        }

        /// <summary>
        /// Indicates whether referenced target variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [Action(order: 5)] // todo need to couple this with variable on editor
        public bool JsonTargetPersistent { get; set; } = false;

        [XmlAttribute("JsonTargetPersistent")]
        public string Xml_JsonTargetPersistent
        {
            get => XmlAttr.Bool(JsonTargetPersistent, false);
            set => JsonTargetPersistent = XmlAttr.Bool(value);
        }

        /// <summary>
        /// Indicates whether referenced variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [Action(order: 6)] // todo need to couple this with variable on editor
        public bool Persistent { get; set; } = false;

        [XmlAttribute("Persistent")]
        public string Xml_Persistent
        {
            get => XmlAttr.Bool(Persistent, false);
            set => Persistent = XmlAttr.Bool(value);
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            string sPersist = I18n.TrlVarPersist(Persistent);
            string tPersist = I18n.TrlVarPersist(JsonTargetPersistent);
            switch (Operation)
            {
                case OperationEnum.SetNumeric:
                case OperationEnum.SetString:
                    string exprType = I18n.TrlExprType(Operation == OperationEnum.SetString);
                    return I18n.Translate(
                        "internal/Action/descscalarset",
                        "set {1}scalar variable ({0}) value with {3} expression ({2})",
                        Name, sPersist, Value, exprType
                    );                    
                case OperationEnum.Increment:
                    string value = string.IsNullOrWhiteSpace(Value) ? "1" : Value;
                    return I18n.Translate(
                        "internal/Action/descscalarincrement",
                        "increment the value of {1}scalar variable ({0}) by ({2})",
                        Name, sPersist, value
                    );                    
                case OperationEnum.Clipboard:
                    bool isName = !string.IsNullOrWhiteSpace(Name);
                    if (isName)
                    {
                        return I18n.Translate(
                            "internal/Action/descscalarclipboardvar", 
                            "Copy {1}scalar variable ({0}) value to clipboard",
                            Name, sPersist
                        );
                    }
                    return I18n.Translate(
                        "internal/Action/descscalarclipboardexpr",
                        "Copy string expression ({0}) to clipboard",
                        Value
                    );
                case OperationEnum.Unset:
                    return I18n.Translate(
                        "internal/Action/descscalarunset",
                        "unset {1}scalar variable ({0})",
                        Name, sPersist
                    );
                case OperationEnum.UnsetAll:
                    return I18n.Translate(
                        "internal/Action/descscalarunsetall",
                        "unset all {0}scalar variables",
                        sPersist
                    );
                case OperationEnum.UnsetRegex:
                    return I18n.Translate(
                        "internal/Action/descscalarunsetregex",
                        "unset {1}scalar variables matching regular expression ({0})",
                        Name, sPersist
                    );
                case OperationEnum.UnsetRegexUniversal:
                    return I18n.Translate(
                        "internal/Action/descscalarunsetregexuniversal", 
                        "unset all types of {1}variables matching regular expression ({0})", 
                        Name, sPersist
                    );
                case OperationEnum.QueryJsonPath:
                    return I18n.Translate(
                        "internal/Action/descscalarqueryjson",
                        "query {1} variable ({0}) with JSON path ({2}) and store result to {4}scalar variable ({3})",
                        Name, sPersist, Value, JsonTargetName, tPersist
                    );
                case OperationEnum.QueryJsonPathList:
                    return I18n.Translate(
                        "internal/Action/descscalarqueryjsonlist",
                        "query {1} variable ({0}) with JSON path ({2}) and store result to {4}list variable ({3})",
                        Name, sPersist, Value, JsonTargetName, tPersist
                    );
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;

            string varname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Name);
            string sPersist = I18n.TrlVarPersist(Persistent);
            string tPersist = I18n.TrlVarPersist(JsonTargetPersistent);
            string changer;
            if (ctx.Trigger != null)
            {
                changer = I18n.Translate("internal/Action/changetagtrigaction", "Trigger '{0}' action '{1}'", ctx.Trigger.LogName, Describe());
            }
            else
            {
                changer = I18n.Translate("internal/Action/changetagtestmode", "Action '{0}' test mode", Describe());
            }
            string newval;
            VariableStore vs = ctx.Plugin.GetVariableStore(Persistent);
            switch (Operation)
            {
                case OperationEnum.UnsetAll:
                    {
                        vs.UnsetAllVariables(vs.Scalar);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarunsetall",
                            "All {0}scalar variables unset", sPersist));
                        break;
                    }
                case OperationEnum.UnsetRegex:
                    {
                        vs.UnsetVariableRegex(vs.Scalar, Name);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarunsetregex",
                            "All {1}scalar variables matching ({0}) unset", Name, sPersist));
                        break;
                    }
                case OperationEnum.UnsetRegexUniversal:
                    {
                        Regex rx = new Regex(Name);
                        vs.UnsetVariableRegex(vs.Scalar, rx);
                        vs.UnsetVariableRegex(vs.List, rx);
                        vs.UnsetVariableRegex(vs.Table, rx);
                        vs.UnsetVariableRegex(vs.Dict, rx);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarunsetregexuniversal",
                            "All {1}variables matching ({0}) unset", Name, sPersist));
                        break;
                    }
                case OperationEnum.Unset:
                    {
                        vs.UnsetVariable(vs.Scalar, varname);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarunset",
                            "{1}Scalar variable ({0}) unset", varname, sPersist));
                        break;
                    }
                case OperationEnum.SetString:
                case OperationEnum.SetNumeric:
                    {
                        if (Operation == OperationEnum.SetString)
                        {
                            newval = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Value);
                        }
                        else
                        {
                            newval = I18n.ThingToString(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Value));
                        }

                        VariableScalar x = new VariableScalar();
                        x.Value = newval;
                        x.LastChanger = changer;
                        x.LastChanged = DateTime.Now;
                        lock (vs.Scalar) // verified
                        {
                            vs.Scalar[varname] = x;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarset",
                            "{2}Scalar variable ({0}) value set to ({1})", varname, newval, sPersist));
                        break;
                    }
                case OperationEnum.Increment:
                    {
                        double original = 0;
                        double increment = string.IsNullOrWhiteSpace(Value)
                            ? 1 : ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Value);
                        VariableScalar x = new VariableScalar { LastChanger = changer, LastChanged = DateTime.Now };
                        lock (vs.Scalar)
                        {
                            if (vs.Scalar.TryGetValue(varname, out VariableScalar originalVar))
                            {
                                if (!string.IsNullOrWhiteSpace(originalVar.Value))
                                {
                                    original = ctx.EvaluateNumericExpression(ActionContextLogger, ctx, originalVar.Value);
                                }
                            }
                            x.Value = I18n.ThingToString(original + increment);
                            vs.Scalar[varname] = x;
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarset",
                                "{2}Scalar variable ({0}) value set to ({1})", varname, x.Value, sPersist));
                        }
                        break;
                    }
                case OperationEnum.Clipboard:
                    {
                        bool isName = !string.IsNullOrWhiteSpace(Name);
                        string text = "";
                        if (isName)
                            lock (vs.Scalar)
                            {
                                text = vs.GetScalarVariable(varname, false).Value;
                            }
                        else
                        {
                            text = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Value);
                        }
                        ActionOld.ClipboardSetText(text); // todo
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarclipboard",
                            "Set text ({0}) to clipboard", text));
                        break;
                    }
                case OperationEnum.QueryJsonPath:
                    {
                        newval = "";
                        lock (vs.Scalar) // verified
                        {
                            if (vs.Scalar.ContainsKey(varname) == true)
                            {
                                newval = vs.Scalar[varname].Value;
                            }
                        }
                        string tgtname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, JsonTargetName);
                        VariableStore vs2 = ctx.Plugin.GetVariableStore(JsonTargetPersistent);
                        string query = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Value);
                        JsonPath.JsonPathContext pc = new JsonPath.JsonPathContext();
                        Dictionary<string, object> p = new Utilities.JsonParser().Parse(newval);
                        IEnumerable<object> result = pc.Select(p, query);

                        VariableScalar x = new VariableScalar();
                        switch (result.Count())
                        {
                            case 0: x.Value = ""; break;
                            case 1: x.Value = result.First().ToString(); break;
                            default: x.Value = JsonSerializer.Serialize(result.ToArray()); break;
                        }
                        x.LastChanger = changer;
                        x.LastChanged = DateTime.Now;

                        lock (vs2.Scalar) // verified
                        {
                            vs2.Scalar[tgtname] = x;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarset",
                            "{2}Scalar variable ({0}) value set to ({1})", tgtname, newval, tPersist));
                    }
                    break;
                case OperationEnum.QueryJsonPathList:
                    {
                        newval = "";
                        lock (vs.Scalar) // verified
                        {
                            if (vs.Scalar.ContainsKey(varname) == true)
                            {
                                newval = vs.Scalar[varname].Value;
                            }
                        }
                        string tgtname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, JsonTargetName);
                        VariableStore vs2 = ctx.Plugin.GetVariableStore(JsonTargetPersistent);
                        string query = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Value);
                        JsonPath.JsonPathContext pc = new JsonPath.JsonPathContext();
                        Dictionary<string, object> p = new Utilities.JsonParser().Parse(newval);
                        IEnumerable<object> result = pc.Select(p, query);

                        VariableList x = new VariableList();
                        x.LastChanger = changer;
                        x.LastChanged = DateTime.Now;
                        switch (result.Count())
                        {
                            case 0: break;
                            case 1: x.Push(new VariableScalar() { Value = result.First().ToString(), LastChanged = x.LastChanged, LastChanger = changer }, changer); break;
                            default:
                                foreach (object o in result)
                                {
                                    if (o is object[])
                                    {
                                        x.Push(new VariableScalar() { Value = JsonSerializer.Serialize((object[])o), LastChanged = x.LastChanged, LastChanger = changer }, changer);
                                    }
                                    else if (o is Dictionary<string, object>)
                                    {
                                        x.Push(new VariableScalar() { Value = JsonSerializer.Serialize((Dictionary<string, object>)o), LastChanged = x.LastChanged, LastChanger = changer }, changer);
                                    }
                                    else
                                    {
                                        x.Push(new VariableScalar() { Value = o.ToString(), LastChanged = x.LastChanged, LastChanger = changer }, changer);
                                    }
                                }
                                break;
                        }
                        lock (vs2.List) // verified
                        {
                            vs2.List[tgtname] = x;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listset",
                            "{2}List variable ({0}) value set to ({1})", tgtname, newval, tPersist));
                    }
                    break;
                default:
                    throw NotImplementedEnumException(Operation);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionVariableScalar(ActionOld oldAction)
        {
            var action = new ActionVariableScalar();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._VariableOp;
            action.Name = oldAction._VariableName;
            action.JsonTargetName = oldAction._VariableJsonTarget;
            action.Value = oldAction._VariableExpression;
            action.JsonTargetPersistent = oldAction._VariableTargetPersist;
            action.Persistent = oldAction._VariablePersist;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionVariableScalar action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.Variable;
            oldAction._VariableOp = (ActionOld.VariableOpEnum)(int)action.Operation;
            oldAction._VariableName = action.Name;
            oldAction._VariableJsonTarget = action.JsonTargetName;
            oldAction._VariableExpression = action.Value;
            oldAction._VariableTargetPersist = action.JsonTargetPersistent;
            oldAction._VariablePersist = action.Persistent;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
