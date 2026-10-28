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
using Triggernometry.UI.CustomControls;
using Triggernometry.Utilities;
using ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum;
using Autofill = Triggernometry.UI.CustomControls.ExpressionTextBox.AutofillTypeEnum;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Scalar variable operations
    /// </summary>
    [Action("Scalar variable operation", ActionAttribute.CategoryType.Variables)]
    [XmlRoot(ElementName = "ActionVariableScalar")]
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
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Operation type",
            ChoiceLabels = new[]
        {
            "Unset scalar variable",
            "Set scalar variable value with string expression",
            "Set scalar variable value with numeric expression",
            "Increment scalar variable value",
            "Copy scalar variable or expression to the clipboard",
            "Unset all scalar variables",
            "Unset scalar variables matching regular expression",
            "Unset all types of variables matching regular expression",
            "Query variable with JSONPath and store scalar value",
            "Query variable with JSONPath and store list value",
        })]
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
        [ActionSpecificProperty]
        [ExpressionEditor(2, Label = "Scalar variable name", Autofill = EditorAutofillType.Scalar)]
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name, "", omitWhitespace: false);
            set => Name = value;
        }

        /// <summary>
        /// Name of the target variable for some JSON operations
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(5, Label = "Target variable name")]
        public string TargetVar { get; set; } = "";

        [XmlAttribute("TargetVar")]
        public string Xml_TargetVar
        {
            get => XmlAttr.String(TargetVar, "", omitWhitespace: false);
            set => TargetVar = value;
        }

        /// <summary>
        /// Value expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(4, Label = "Expression")]
        public string Value { get; set; } = "";

        [XmlAttribute("Value")]
        public string Xml_Value
        {
            get => XmlAttr.String(Value, "", omitWhitespace: false);
            set => Value = value;
        }

        /// <summary>
        /// Indicates whether referenced target variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [PersistenceEditor(6, nameof(TargetVar), Label = "Target variable name")]
        public bool TargetPersistent { get; set; } = false;

        [XmlAttribute("TargetPersistent")]
        public string Xml_TargetPersistent
        {
            get => XmlAttr.Bool(TargetPersistent, false);
            set => TargetPersistent = XmlAttr.Bool(value);
        }

        /// <summary>
        /// Indicates whether referenced variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [PersistenceEditor(3, nameof(Name), Label = "Scalar variable name")]
        public bool Persistent { get; set; } = false;

        [XmlAttribute("Persistent")]
        public string Xml_Persistent
        {
            get => XmlAttr.Bool(Persistent, false);
            set => Persistent = XmlAttr.Bool(value);
        }

        #endregion


        public override Action CreateEditorRules(ConfigEditor editor)
        {
            var operation = editor.GetEditor(nameof(Operation));
            var name = editor.GetEditor(nameof(Name));
            var value = editor.GetEditor(nameof(Value));
            var target = editor.GetEditor(nameof(TargetVar));
            var persistent = editor.GetEditor(nameof(Persistent));
            var targetPersistent = editor.GetEditor(nameof(TargetPersistent));
            return () =>
            {
                var op = (OperationEnum)operation.GetValue();
                switch (op)
                {
                    case OperationEnum.UnsetAll:
                        editor.EnableOnly(operation); break;
                    case OperationEnum.Unset:
                    case OperationEnum.UnsetRegex:
                    case OperationEnum.UnsetRegexUniversal:
                        editor.EnableOnly(operation, name); break;
                    case OperationEnum.QueryJsonPath:
                    case OperationEnum.QueryJsonPathList:
                        editor.EnableOnly(operation, name, value, target); break;
                    default:
                        editor.EnableOnly(operation, name, value); break;
                }
                editor.SetEnabled(true, persistent);
                editor.SetEnabled(target.Enabled, targetPersistent);
                name.SetVariableNameMode(Autofill.Scalar, op == OperationEnum.UnsetRegex || op == OperationEnum.UnsetRegexUniversal);
                editor.SetExpressionType(op == OperationEnum.SetNumeric || op == OperationEnum.Increment ? ExpressionType.Numeric : ExpressionType.String, value);
                target.Expression.AutofillType = op == OperationEnum.QueryJsonPathList ? Autofill.List : Autofill.Scalar;
            };
        }

        #region Implementation

        internal override string DescribeImplementation()
        {
            string sPersist = I18n.TrlVarPersist(Persistent);
            string tPersist = I18n.TrlVarPersist(TargetPersistent);
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
                        Name, sPersist, Value, TargetVar, tPersist
                    );
                case OperationEnum.QueryJsonPathList:
                    return I18n.Translate(
                        "internal/Action/descscalarqueryjsonlist",
                        "query {1} variable ({0}) with JSON path ({2}) and store result to {4}list variable ({3})",
                        Name, sPersist, Value, TargetVar, tPersist
                    );
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            string varname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Name);
            string sPersist = I18n.TrlVarPersist(Persistent);
            string tPersist = I18n.TrlVarPersist(TargetPersistent);
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
            VariableStore vs = plug.GetVariableStore(Persistent);
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
                        vs.UnsetVariableRegex(vs.Scalar, varname);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarunsetregex",
                            "All {1}scalar variables matching ({0}) unset", varname, sPersist));
                        break;
                    }
                case OperationEnum.UnsetRegexUniversal:
                    {
                        Regex rx = new Regex(varname);
                        vs.UnsetVariableRegex(vs.Scalar, rx);
                        vs.UnsetVariableRegex(vs.List, rx);
                        vs.UnsetVariableRegex(vs.Table, rx);
                        vs.UnsetVariableRegex(vs.Dict, rx);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarunsetregexuniversal",
                            "All {1}variables matching ({0}) unset", varname, sPersist));
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
                        RealPlugin.ClipboardSetText(text);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarclipboard",
                            "Set text ({0}) to clipboard", text));
                        break;
                    }
                case OperationEnum.QueryJsonPath:
                    {
                        newval = vs.GetScalarVariable(varname, false).Value;
                        string tgtname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetVar);
                        VariableStore vs2 = plug.GetVariableStore(TargetPersistent);
                        string query = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Value);
                        JsonPath.JsonPathContext pc = new JsonPath.JsonPathContext();
                        object p = new Utilities.JsonParser().Parse(newval);
                        object[] result = pc.Select(p, query).ToArray();

                        VariableScalar x = new VariableScalar();
                        switch (result.Length)
                        {
                            case 0: x.Value = ""; break;
                            case 1: x.Value = Utilities.JsonParser.ToValueString(result[0]); break;
                            default: x.Value = JsonSerializer.Serialize(result); break;
                        }
                        x.LastChanger = changer;
                        x.LastChanged = DateTime.Now;

                        lock (vs2.Scalar) // verified
                        {
                            vs2.Scalar[tgtname] = x;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/scalarset",
                            "{2}Scalar variable ({0}) value set to ({1})", tgtname, x.Value, tPersist));
                    }
                    break;
                case OperationEnum.QueryJsonPathList:
                    {
                        newval = vs.GetScalarVariable(varname, false).Value;
                        string tgtname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetVar);
                        VariableStore vs2 = plug.GetVariableStore(TargetPersistent);
                        string query = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Value);
                        JsonPath.JsonPathContext pc = new JsonPath.JsonPathContext();
                        object p = new Utilities.JsonParser().Parse(newval);
                        object[] result = pc.Select(p, query).ToArray();

                        VariableList x = new VariableList();
                        x.LastChanger = changer;
                        x.LastChanged = DateTime.Now;
                        foreach (object value in result)
                        {
                            x.Push(new VariableScalar { Value = Utilities.JsonParser.ToValueString(value), LastChanged = x.LastChanged, LastChanger = changer }, changer);
                        }
                        lock (vs2.List) // verified
                        {
                            vs2.List[tgtname] = x;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listset",
                            "{2}List variable ({0}) value set with ({1}) items", tgtname, x.Size, tPersist));
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
            action.TargetVar = oldAction._VariableJsonTarget;
            action.Value = oldAction._VariableExpression;
            action.TargetPersistent = oldAction._VariableTargetPersist;
            action.Persistent = oldAction._VariablePersist;
            return action;
        }

        #endregion Old Action Converter
    }

}
