using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;
using Triggernometry.UI.CustomControls;
using Triggernometry.Utilities;
using Autofill = Triggernometry.UI.CustomControls.ExpressionTextBox.AutofillTypeEnum;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// List variable operations
    /// </summary>
    [Action("List variable operation", ActionAttribute.CategoryType.Variables)]
    [XmlRoot(ElementName = "ActionVariableList")]
    public class ActionVariableList : ActionBase
    {

        #region Properties

        /// <summary>
        /// List variable operations
        /// </summary>
        public enum OperationEnum
        {
            Unset,
            Push,
            Insert,
            Set,
            SetAll,
            Remove,
            PopFirst, // should be "Pop"
            PopToListInsert,
            PopToListSet,
            Build,
            Filter,
            Join,
            Split,
            Copy,
            InsertList,
            SortAsc,
            SortDesc,
            SortByKeys,
            UnsetAll,
            UnsetRegex,
        }

        /// <summary>
        /// Expression types
        /// </summary>
        public enum ExpressionType
        {
            /// <summary>
            /// String expression
            /// </summary>
            String,
            /// <summary>
            /// Numeric expression
            /// </summary>
            Numeric
        }

        /// <summary>
        /// List variable operation type
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(1, Label = "Operation type",
            ChoiceLabels = new[]
        {
            "Unset list variable",
            "Push value to the end of the list variable",
            "Insert value to the given index of the list variable",
            "Set value at the given index of the list variable",
            "Set all values on the list variable to the expression",
            "Remove value at the given index of the list variable",
            "Pop the given index from list variable into a scalar variable",
            "Pop and insert a value from a list into another list",
            "Pop and set a value from a list into another list",
            "Build a list variable from the expression separated by its first character",
            "Filter elements into another list",
            "Join all values in the list variable into a scalar variable (separator in expression)",
            "Split a scalar variable into a list variable (separator in expression)",
            "Copy whole list variable to another list variable",
            "Insert list variable into another list variable at the given index",
            "Sort list in ascending order",
            "Sort list in descending order",
            "Sort list by the key functions",
            "Unset all list variables",
            "Unset list variables matching regular expression",
        })]
        public OperationEnum Operation { get; set; } = OperationEnum.Unset;

        [XmlAttribute("Operation")]
        public string Xml_Operation
        {
            get => XmlAttr.Enum(Operation, OperationEnum.Unset);
            set
            {
                // convert old actions
                if (value == "PopLast")
                {
                    Operation = OperationEnum.PopFirst;
                    Index = "-1";
                    return;
                }

                Operation = XmlAttr.Enum<OperationEnum>(value);
            }
        }

        /// <summary>
        /// Type of the value expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [EnumChoiceEditor(4, Label = "Expression type",
            ChoiceLabels = new[]
        {
            "String",
            "Numeric",
        })]
        public ExpressionType ValueType { get; set; } = ExpressionType.String;

        [XmlAttribute("ValueType")]
        public string Xml_ValueType
        {
            get => XmlAttr.Enum(ValueType, ExpressionType.String);
            set => ValueType = XmlAttr.Enum<ExpressionType>(value);
        }

        /// <summary>
        /// Name of the list variable
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(2, Label = "Source variable name", Autofill = EditorAutofillType.List)]
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name, "", omitWhitespace: false);
            set => Name = value;
        }

        /// <summary>
        /// Value expression
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(5, Label = "Expression")]
        public string Value { get; set; } = "";

        [XmlAttribute("Value")]
        public string Xml_Value
        {
            get => XmlAttr.String(Value, "", omitWhitespace: false);
            set => Value = value;
        }

        /// <summary>
        /// Index inside the list variable (zero-based)
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(6, Label = "List index number")]
        public string Index { get; set; } = "";

        [XmlAttribute("Index")]
        public string Xml_Index
        {
            get => XmlAttr.String(Index, "", omitWhitespace: false);
            set => Index = value;
        }

        /// <summary>
        /// Name of the target variable for some operations
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [ExpressionEditor(7, Label = "Target variable name", Autofill = EditorAutofillType.List)]
        public string TargetVar { get; set; } = "";

        [XmlAttribute("TargetVar")]
        public string Xml_TargetVar
        {
            get => XmlAttr.String(TargetVar, "", omitWhitespace: false);
            set => TargetVar = value;
        }

        /// <summary>
        /// Indicates whether referenced variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [PersistenceEditor(3, nameof(Name), Label = "Source variable name")]
        public bool Persistent { get; set; } = false;

        [XmlAttribute("Persistent")]
        public string Xml_Persistent
        {
            get => XmlAttr.Bool(Persistent, false);
            set => Persistent = XmlAttr.Bool(value);
        }

        /// <summary>
        /// Indicates whether referenced target variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [ActionSpecificProperty]
        [PersistenceEditor(8, nameof(TargetVar), Label = "Target variable name")]
        public bool TargetPersistent { get; set; } = false;

        [XmlAttribute("TargetPersistent")]
        public string Xml_TargetPersistent
        {
            get => XmlAttr.Bool(TargetPersistent, false);
            set => TargetPersistent = XmlAttr.Bool(value);
        }

        #endregion


        public override Action CreateEditorRules(ConfigEditor editor)
        {
            var operation = editor.GetEditor(nameof(Operation));
            var name = editor.GetEditor(nameof(Name));
            var value = editor.GetEditor(nameof(Value));
            string valueLabel = value.Label.Text;
            string targetIndexLabel = I18n.Translate(value.Field.TranslationKey + "/TargetIndex", "Target Index");
            var valueType = editor.GetEditor(nameof(ValueType));
            var index = editor.GetEditor(nameof(Index));
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
                    case OperationEnum.Push: 
                        editor.EnableOnly(operation, name, value, valueType); break;
                    case OperationEnum.Insert:
                    case OperationEnum.Set:
                    case OperationEnum.SetAll: 
                        editor.EnableOnly(operation, name, value, valueType, index); break;
                    case OperationEnum.Remove: 
                        editor.EnableOnly(operation, name, index); break;
                    case OperationEnum.PopFirst:
                    case OperationEnum.InsertList: 
                        editor.EnableOnly(operation, name, index, target); break;
                    case OperationEnum.PopToListInsert:
                    case OperationEnum.PopToListSet: 
                        editor.EnableOnly(operation, name, value, index, target); break;
                    case OperationEnum.Build: 
                        editor.EnableOnly(operation, value, valueType, target); break;
                    case OperationEnum.Filter: 
                        editor.EnableOnly(operation, name, value, target); break;
                    case OperationEnum.Join:
                    case OperationEnum.Split: 
                        editor.EnableOnly(operation, name, value, valueType, target); break;
                    case OperationEnum.Copy: 
                        editor.EnableOnly(operation, name, target); break;
                    case OperationEnum.SortAsc:
                    case OperationEnum.SortDesc:
                        editor.EnableOnly(operation, name, valueType); break;
                    case OperationEnum.SortByKeys: 
                        editor.EnableOnly(operation, name, value); break;
                    default: 
                        editor.EnableOnly(operation, name); break;
                }
                bool targetIndex = op == OperationEnum.PopToListInsert || op == OperationEnum.PopToListSet;
                if (targetIndex || op == OperationEnum.Filter) valueType.SetValue(ExpressionType.Numeric);
                else if (op == OperationEnum.SortByKeys) valueType.SetValue(ExpressionType.String);
                editor.SetExpressionType((ExpressionType)valueType.GetValue()
                    == ExpressionType.Numeric ? ExpressionTextBox.SupportedExpressionTypeEnum.Numeric : ExpressionTextBox.SupportedExpressionTypeEnum.String, value);
                editor.SetExpressionType(ExpressionTextBox.SupportedExpressionTypeEnum.Numeric, index);
                name.SetVariableNameMode(op == OperationEnum.Split ? Autofill.Scalar : Autofill.List, op == OperationEnum.UnsetRegex);
                target.Expression.AutofillType = op == OperationEnum.PopFirst || op == OperationEnum.Join ? Autofill.Scalar : Autofill.List;
                editor.SetEnabled(name.Enabled || op == OperationEnum.UnsetAll, persistent);
                editor.SetEnabled(target.Enabled, targetPersistent);
                value.Label.Text = targetIndex ? targetIndexLabel : valueLabel;
            };
        }

        #region Implementation

        internal override string DescribeImplementation()
        {
            string sPersistL = I18n.TrlVarPersist(Persistent);
            string tPersistL = I18n.TrlVarPersist(TargetPersistent);
            string exprTypeL = I18n.TrlExprType(ValueType == ExpressionType.String);
            switch (Operation)
            {
                case OperationEnum.Unset:
                    return I18n.Translate("internal/Action/desclistunset", "unset {1}list variable ({0})", Name, sPersistL);
                case OperationEnum.Push:
                    return I18n.Translate(
                        "internal/Action/desclistpush",
                        "push the value from {3} expression ({2}) to the end of {1}list variable ({0})",
                        Name, sPersistL, Value, exprTypeL
                    );
                case OperationEnum.Insert:
                    return I18n.Translate(
                        "internal/Action/desclistinsert",
                        "insert the value from {3} expression ({2}) to index ({4}) on {1}list variable ({0})",
                        Name, sPersistL, Value, exprTypeL, Index
                    );
                case OperationEnum.Set:
                    return I18n.Translate(
                        "internal/Action/desclistset",
                        "set the value from {3} expression ({2}) to index ({4}) on {1}list variable ({0})",
                        Name, sPersistL, Value, exprTypeL, Index
                    );
                case OperationEnum.SetAll:
                    if (string.IsNullOrWhiteSpace(Index))
                    {
                        return I18n.Translate(
                            "internal/Action/desclistsetall",
                            "set all values on {1}list ({0}) to {3} expr ({2})",
                            Name, sPersistL, Value, exprTypeL
                        );
                    }
                    return I18n.Translate(
                        "internal/Action/desclistsetallresize",
                        "set all values on {1}list ({0}) to {3} expr ({2}) (resized to length ({4}))",
                        Name, sPersistL, Value, exprTypeL, Index
                    );
                case OperationEnum.Remove:
                    return I18n.Translate(
                        "internal/Action/desclistremove",
                        "remove the value at index ({0}) on {2}list variable ({1})",
                        Index, Name, sPersistL
                    );
                case OperationEnum.PopFirst: // the action was updated to "Pop" but the name was unchanged
                    string index = string.IsNullOrWhiteSpace(Index) ? "1" : Index;
                    return I18n.Translate(
                        "internal/Action/desclistpop",
                        "pop index ({4}) of {1}list variable ({0}) into {3}scalar variable ({2})",
                        Name, sPersistL, TargetVar, tPersistL, index
                    );
                case OperationEnum.PopToListInsert:
                    if (string.IsNullOrWhiteSpace(Value))
                    {
                        return I18n.Translate(
                            "internal/Action/desclistpoptolist",
                            "pop index ({2}) of {1}list variable ({0}) to the end of {5}list variable ({4})",
                            Name, sPersistL, Index, TargetVar, tPersistL
                        );
                    }
                    return I18n.Translate(
                        "internal/Action/desclistpoptolistinsert",
                        "pop index ({2}) of {1}list variable ({0}) and insert to index ({5}) of {4}list variable ({3})",
                        Name, sPersistL, Index,
                        TargetVar, tPersistL, Value
                    );
                case OperationEnum.PopToListSet:
                    return I18n.Translate(
                        "internal/Action/desclistpoptolistset",
                        "pop index ({2}) of {1}list variable ({0}) and set to index ({5}) of {4}list variable ({3})",
                        Name, sPersistL, Index,
                        TargetVar, tPersistL, Value
                    );
                case OperationEnum.SortAsc:
                case OperationEnum.SortDesc:
                    {
                        bool isNumeric = ValueType == ExpressionType.Numeric;
                        string order = I18n.TrlSortAscOrDesc(Operation == OperationEnum.SortAsc);
                        return I18n.Translate(
                            isNumeric ? "internal/Action/desclistsortnum" : "internal/Action/desclistsortstring",
                            isNumeric ? "sort {1}list variable ({0}) in a numerically {2} order"
                                : "sort {1}list variable ({0}) in an alphabetically {2} order",
                            Name, sPersistL, order);
                    }
                case OperationEnum.SortByKeys:
                    return I18n.Translate(
                        "internal/Action/desclistsortbykeys",
                        "sort {1}list variable ({0}) by keys ({2})",
                        Name, sPersistL, Value
                    );
                case OperationEnum.Copy:
                    return I18n.Translate(
                        "internal/Action/desclistcopy",
                        "copy {2}list variable ({0}) to {3}list variable ({1})",
                        Name, TargetVar, sPersistL, tPersistL
                    );
                case OperationEnum.InsertList:
                    return I18n.Translate(
                        "internal/Action/desclistinsertlist",
                        "insert {3}list variable ({0}) into {4}list variable ({1}) at index ({2})",
                        Name, TargetVar, Index, sPersistL, tPersistL
                    );
                case OperationEnum.Join:
                    return I18n.Translate(
                        "internal/Action/desclistjoin",
                        "join all values in {3}list variable ({0}) to {4}scalar variable ({1}) using {5} expression ({2}) as separator",
                        Name, TargetVar, Value, sPersistL, tPersistL, exprTypeL
                    );
                case OperationEnum.Split:
                    return I18n.Translate(
                        "internal/Action/desclistsplit",
                        "split {3}scalar variable ({0}) into {4}list variable ({1}) using {5} expression ({2}) as separator",
                        Name, TargetVar, Value, sPersistL, tPersistL, exprTypeL
                    );
                case OperationEnum.Build:
                    if (
                        ValueType == ExpressionType.String
                        && !Value.StartsWith("$") && !Value.StartsWith("¤{")
                    )
                    {
                        return I18n.Translate(
                            "internal/Action/desclistbuild",
                            "build {1}list variable ({0}) from string ({2}) separated by ({3})",
                            TargetVar, tPersistL,
                            Value.Length == 0 ? "" : Value.Substring(1),
                            Value.Length == 0 ? "" : Value.Substring(0, 1)
                        );
                    }
                    return I18n.Translate(
                        "internal/Action/desclistbuildraw",
                        "build {1}list variable ({0}) from {3} expression ({2}) separated by its first character",
                        TargetVar, tPersistL, Value, exprTypeL
                    );
                case OperationEnum.Filter:
                    return I18n.Translate(
                        "internal/Action/desclistfilter",
                        "Use expression ({4}) to filter {1}list ({0}) into {3}list ({2})",
                        Name, sPersistL, TargetVar, tPersistL, Value
                    );
                case OperationEnum.UnsetAll:
                    return I18n.Translate( "internal/Action/desclistunsetall", "unset all {0}list variables", sPersistL);
                case OperationEnum.UnsetRegex:
                    return I18n.Translate(
                        "internal/Action/desclistunsetregex",
                        "unset {1}list variables matching regular expression ({0})",
                        Name, sPersistL
                    );
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }

        private string GetListExpressionValue(Context ctx, ExpressionType typ, string expr)
        {
            switch (typ)
            {
                case ExpressionType.Numeric:
                    return I18n.ThingToString(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, expr));
                case ExpressionType.String:
                    return ctx.EvaluateStringExpression(ActionContextLogger, ctx, expr);
            }
            return "";
        }

        internal override void ExecuteImplementation(Context ctx)
        {
            RealPlugin plug = ctx.Plugin;

            string sourcename = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Name);
            VariableStore svs = plug.GetVariableStore(Persistent);
            VariableStore tvs = plug.GetVariableStore(TargetPersistent);
            string sPersist = I18n.TrlVarPersist(Persistent);
            string tPersist = I18n.TrlVarPersist(TargetPersistent);
            string changer = DescribeVariableChanger(ctx);
            switch (Operation)
            {
                case OperationEnum.Unset:
                    {
                        svs.UnsetVariable(svs.List, sourcename);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listunset",
                            "{1}List variable ({0}) unset", sourcename, sPersist));
                    }
                    break;
                case OperationEnum.Push:
                    {
                        string value = GetListExpressionValue(ctx, ValueType, Value);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, true);
                            vl.Push(new VariableScalar() { Value = value }, changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listpush",
                            "Value ({0}) pushed to the end of {2}list variable ({1})",
                            value, sourcename, sPersist));
                    }
                    break;
                case OperationEnum.Insert:
                    {
                        string value = GetListExpressionValue(ctx, ValueType, Value);
                        int index = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Index);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, true);
                            vl.Insert(index, value, changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listindexinsert",
                            "Value ({0}) inserted to index ({1}) of {3}list variable ({2})",
                            value, index, sourcename, sPersist));
                    }
                    break;
                case OperationEnum.Set:
                    {
                        string[] invalidExprs = new[] { "${_row", "${_col", "${_key}", "${_val}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);

                        string value;
                        int index = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Index);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, true);
                            ctx.varName = (Persistent ? "plvar:" : "lvar:") + sourcename;   // ${_this}
                            ctx.listIndex = index;  // ${_idx}
                            // Establish the target before evaluating expressions that read its old value.
                            value = GetListExpressionValue(ctx, ValueType, Value);
                            vl.Set(index, value, changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listindexset",
                            "Value ({0}) set to index ({1}) of {3}list variable ({2})",
                            value, index, sourcename, sPersist));
                    }
                    break;
                case OperationEnum.SetAll:
                    {
                        string[] invalidExprs = new[] { "${_row", "${_col", "${_key}", "${_val}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);
                        int newLength = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Index);
                        // create a new list to avoid setting values base on previously edited ones
                        VariableList vlNew = new VariableList();
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            newLength = newLength <= 0 ? vl.Size : newLength;
                            ctx.varName = (Persistent ? "plvar:" : "lvar:") + sourcename;   // ${_this}
                            for (int i = 1; i <= newLength; i++)   // index starts from 1
                            {
                                ctx.listIndex = i;  // ${_idx}
                                string expr = GetListExpressionValue(ctx, ValueType, Value);
                                vlNew.Push(new VariableScalar() { Value = expr }, changer);
                            }
                            svs.List[sourcename] = vlNew;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listsetall",
                            "All values in {1}list variable ({0}) set to ({2})",
                            sourcename, sPersist, Value));
                    }
                    break;
                case OperationEnum.Remove:
                    {
                        int index = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Index);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            vl.Remove(index, changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listindexunset",
                            "Value removed from index ({0}) of {2}list variable ({1})",
                            index, sourcename, sPersist));
                    }
                    break;
                case OperationEnum.PopFirst:
                    {
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetVar);
                        int rawIndex = string.IsNullOrWhiteSpace(Index) ? 1 : (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Index);

                        string newval = "";
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            newval = vl.Pop(rawIndex, changer).ToString();
                        }
                        VariableScalar x = new VariableScalar();
                        x.Value = newval;
                        x.LastChanger = changer;
                        x.LastChanged = DateTime.Now;
                        lock (tvs.Scalar) // verified
                        {
                            tvs.Scalar[targetname] = x;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listpop",
                            "Value ({4}) popped from index ({5}) of {1}list ({0}) into {3}scalar variable ({2})",
                            sourcename, sPersist, targetname, tPersist, newval, rawIndex));
                    }
                    break;
                case OperationEnum.PopToListInsert:
                case OperationEnum.PopToListSet:
                    {
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetVar);
                        bool isInsert = Operation == OperationEnum.PopToListInsert;
                        bool popToEnd = isInsert && string.IsNullOrWhiteSpace(Value);

                        int rawSourceIndex = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Index);
                        int rawTargetIndex = 0;
                        if (!popToEnd)
                        {
                            rawTargetIndex = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Value);
                        }

                        VariableScalar popped;
                        lock (svs.List)
                        {
                            VariableList svl = svs.GetListVariable(sourcename, false);
                            popped = (VariableScalar)svl.Pop(rawSourceIndex, changer);
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listpoptolist",
                                "pop value ({3}) from {1}list variable ({0}) index ({2})",
                                sourcename, sPersist, rawSourceIndex, targetname, tPersist));
                        }
                        lock (tvs.List)
                        {
                            VariableList tvl = tvs.GetListVariable(targetname, true);
                            if (popToEnd)
                            {
                                tvl.Values.Add(popped);
                                tvl.LastChanged = DateTime.Now;
                                tvl.LastChanger = changer;
                                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listindexset",
                                    "Value ({0}) set to index ({1}) of {3}list variable ({2})",
                                    popped.Value, tvl.Size, targetname, tPersist));
                            }
                            else if (isInsert)
                            {
                                tvl.Insert(rawTargetIndex, popped, changer);
                                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listindexinsert",
                                    "Value ({0}) inserted to index ({1}) of {3}list variable ({2})",
                                    popped.Value, rawTargetIndex, targetname, tPersist));
                            }
                            else
                            {
                                tvl.Set(rawTargetIndex, popped, changer);
                                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listindexset",
                                    "Value ({0}) set to index ({1}) of {3}list variable ({2})",
                                    popped.Value, rawTargetIndex, targetname, tPersist));
                            }
                        }
                    }
                    break;
                case OperationEnum.SortAsc:
                case OperationEnum.SortDesc:
                    {
                        // #38：普通排序统一按值类型和操作方向调用。
                        bool isNumeric = ValueType == ExpressionType.Numeric;
                        bool isAscending = Operation == OperationEnum.SortAsc;
                        string order = I18n.TrlSortAscOrDesc(isAscending);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            vl.Sort(isNumeric, isAscending, changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate(
                            isNumeric ? "internal/Action/listsortnum" : "internal/Action/listsortstr",
                            isNumeric ? "{1}List variable ({0}) sorted in numerically {2} order"
                                : "{1}List variable ({0}) sorted in alphabetically {2} order",
                            sourcename, sPersist, order));
                    }
                    break;
                case OperationEnum.SortByKeys:
                    {
                        string[] invalidExprs = new[] { "${_row", "${_col", "${_key}", "${_val}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);

                        // parsing expressions like "n+:key1, s-:key2, s+:key3, ..."
                        ParseSortKeyFunctions(Value,
                            out List<bool> isNumeric, out List<bool> isAscending,
                            out List<string> keysExpr, out List<List<string>> values);
                        int keysCount = keysExpr.Count;

                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            ctx.varName = (Persistent ? "plvar:" : "lvar:") + sourcename;    // for ${_this}
                                                                                                            // Iterate through the list and evaluate the key expression in the current context
                            for (int listIndex = 0; listIndex < vl.Size; listIndex++)
                            {
                                ctx.listIndex = listIndex + 1;  // for ${_idx}
                                for (int keyIndex = 0; keyIndex < keysCount; keyIndex++)
                                {
                                    string keyValue = isNumeric[keyIndex]
                                        ? I18n.ThingToString(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, keysExpr[keyIndex]))
                                        : ctx.EvaluateStringExpression(ActionContextLogger, ctx, keysExpr[keyIndex]);
                                    values[keyIndex].Add(keyValue);
                                }
                            }
                            IOrderedEnumerable<int> sortedIndices = ApplySorting(vl.Size, isNumeric, isAscending, values);
                            var sortedList = sortedIndices.Select(i => vl.Values[i]).ToList();
                            vl.Values = sortedList;
                            vl.LastChanger = changer;
                            vl.LastChanged = DateTime.Now;
                        }

                        for (int i = 0; i < keysCount; i++)
                        {   // logging each sorting keys
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listsortbykeys",
                                "Sorting {1}list ({0}): function ({2}/{3}, {5}) = ({4}). Keys: ({6})",
                                sourcename, sPersist, i + 1, keysCount, keysExpr[i],
                                (isNumeric[i] ? "n" : "s") + (isAscending[i] ? "+" : "-"),
                                string.Join(", ", values[i])));
                        }
                    }
                    break;
                case OperationEnum.Copy:
                    {
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetVar);
                        VariableList vl = null;
                        lock (svs.List)
                        {
                            vl = svs.GetListVariable(sourcename, false);
                        }
                        lock (tvs.List)
                        {
                            VariableList newvl = vl.Size == 0 ? new VariableList() : (VariableList)vl.Duplicate();
                            if (newvl.Size > 0)
                            {
                                newvl.LastChanger = changer;
                                newvl.LastChanged = DateTime.Now;
                            }
                            tvs.List[targetname] = newvl;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listcopy",
                            "{2}List variable ({0}) copied to {3}list variable ({1})",
                            sourcename, targetname, sPersist, tPersist));
                    }
                    break;
                case OperationEnum.InsertList:
                    {
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetVar);
                        int rawIndex = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Index);
                        VariableList vl = null;
                        VariableList vlcopy = null;
                        lock (svs.List)
                        {
                            vl = svs.GetListVariable(sourcename, false);
                            if (svs == tvs)
                            {
                                VariableList newvl = tvs.GetListVariable(targetname, true);
                                newvl.InsertList(rawIndex, vl, changer);
                            }
                            else
                            {
                                vlcopy = (VariableList)vl.Duplicate();
                            }
                        }
                        if (svs != tvs)
                        {
                            lock (tvs.List)
                            {
                                VariableList newvl = tvs.GetListVariable(targetname, true);
                                newvl.InsertList(rawIndex, vlcopy, changer);
                            }
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listinsertlist",
                            "{3}List variable ({0}) inserted to {4}list variable ({1}) at index ({2})",
                            sourcename, targetname, rawIndex, sPersist, tPersist));
                    }
                    break;
                case OperationEnum.Filter:
                    {
                        string[] invalidExprs = new[] { "${_row", "${_col", "${_key}", "${_val}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetVar);
                        VariableList vlResult = new VariableList();
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            ctx.varName = (Persistent ? "plvar:" : "lvar:") + sourcename;    // for ${_this}
                            for (int i = 0; i < vl.Size; i++)
                            {
                                ctx.listIndex = i + 1;  // for ${_idx}
                                double result = ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Value);
                                if (!MathParser.IsZero(result))
                                {
                                    vlResult.Values.Add(vl.Values[i].Duplicate());
                                }
                            }
                        }
                        vlResult.LastChanger = changer;
                        vlResult.LastChanged = DateTime.Now;
                        lock (tvs.List)
                        {
                            tvs.List[targetname] = vlResult;
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listfilter",
                                "Filtered {4} elements from {1}list ({0}) into {3}list ({2})",
                                sourcename, sPersist, targetname, tPersist, vlResult.Size));
                        }
                    }
                    break;
                case OperationEnum.Join:
                    {
                        string separator = GetListExpressionValue(ctx, ValueType, Value);
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetVar);
                        string newval = "";
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            newval = vl.Join(separator);
                        }
                        VariableScalar x = new VariableScalar
                        {
                            Value = newval,
                            LastChanger = changer,
                            LastChanged = DateTime.Now
                        };
                        lock (tvs.Scalar) // verified
                        {
                            tvs.Scalar[targetname] = x;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listscalarjoin",
                            "{3}List variable ({0}) joined to {4}scalar variable ({1}) with separator ({2})",
                            sourcename, targetname, separator, sPersist, tPersist));
                    }
                    break;
                case OperationEnum.Split:
                    {
                        string separator = GetListExpressionValue(ctx, ValueType, Value);
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetVar);
                        string splitval = "";
                        lock (svs.Scalar) // verified
                        {
                            if (svs.Scalar.ContainsKey(sourcename))
                            {
                                splitval = svs.Scalar[sourcename].Value;
                            }
                        }
                        string[] vals = splitval.Split(new string[] { separator }, StringSplitOptions.None);

                        VariableList vl = new VariableList();
                        foreach (string x in vals)
                        {
                            vl.Push(new VariableScalar() { Value = x }, changer);
                        }
                        lock (tvs.List)
                        {
                            tvs.List[targetname] = vl;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listsplit",
                            "{3}Scalar variable ({0}) split into {4}list variable ({1}) with separator ({2})",
                            sourcename, targetname, separator, sPersist, tPersist));
                    }
                    break;
                case OperationEnum.Build:
                    {   // Using the first character in the expression to split the remaining part into a new list
                        // e.g. expr = ",1,2,3,4,5,6,7,8"
                        string expr = GetListExpressionValue(ctx, ValueType, Value);
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetVar);
                        VariableList vl = new VariableList();
                        if (expr.Length > 0)
                        {
                            if (expr[0] == '\n' || expr.StartsWith("\r\n"))
                                expr = ParserCommon.ReplaceLineBreak(expr);
                            char separator = expr[0];
                            string splitval = expr.Substring(1);
                            vl = VariableList.Build(splitval, separator, changer);
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listbuild",
                                "{1}List variable ({0}) built from expression ({2}) splitted by ({3})",
                                targetname, tPersist, splitval, separator));
                        }
                        else
                        {
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/listbuildfail",
                                "{1}List variable ({0}) cannot be built because expression ({2}) length < 1",
                                targetname, tPersist, expr));
                        }

                        lock (tvs.List)
                        {
                            tvs.List[targetname] = vl;
                        }
                    }
                    break;
                case OperationEnum.UnsetAll:
                    {
                        svs.UnsetAllVariables(svs.List);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listunsetall",
                            "All {0}list variables unset", sPersist));
                    }
                    break;
                case OperationEnum.UnsetRegex:
                    {
                        svs.UnsetVariableRegex(svs.List, sourcename);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listunsetregex",
                            "All {1}list variables matching ({0}) unset", sourcename, sPersist));
                        break;
                    }
                default:
                    throw NotImplementedEnumException(Operation);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionVariableList(ActionOld oldAction)
        {
            var action = new ActionVariableList();
            oldAction.CopyCommonPropertiesTo(action);
            action.ValueType = (ExpressionType)(int)oldAction._ListVariableExpressionType;
            // #38：旧排序类型决定比较方式，不能沿用旧配置中排序并未使用的表达式类型。
            switch (oldAction._ListVariableOp)
            {
                case ActionOld.ListVariableOpEnum.SortNumericAsc:
                case ActionOld.ListVariableOpEnum.SortAlphaAsc:
                    action.Operation = OperationEnum.SortAsc;
                    action.ValueType = oldAction._ListVariableOp == ActionOld.ListVariableOpEnum.SortNumericAsc
                        ? ExpressionType.Numeric : ExpressionType.String;
                    break;
                case ActionOld.ListVariableOpEnum.SortNumericDesc:
                case ActionOld.ListVariableOpEnum.SortAlphaDesc:
                    action.Operation = OperationEnum.SortDesc;
                    action.ValueType = oldAction._ListVariableOp == ActionOld.ListVariableOpEnum.SortNumericDesc
                        ? ExpressionType.Numeric : ExpressionType.String;
                    break;
                default:
                    action.Operation = (OperationEnum)Enum.Parse(typeof(OperationEnum), oldAction._ListVariableOp.ToString());
                    break;
            }
            action.Name = oldAction._ListVariableName;
            action.Value = oldAction._ListVariableExpression;
            action.Index = oldAction._ListVariableIndex;
            action.TargetVar = oldAction._ListVariableTarget;
            action.Persistent = oldAction._ListSourcePersist;
            action.TargetPersistent = oldAction._ListTargetPersist;
            return action;
        }

        #endregion Old Action Converter
    }

}
