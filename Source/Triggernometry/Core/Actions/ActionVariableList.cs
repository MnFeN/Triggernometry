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

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// List variable operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Variable)]
    [XmlRoot(ElementName = "VariableList")]
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
            SortNumericAsc,
            SortNumericDesc,
            SortAlphaAsc,
            SortAlphaDesc,
            SortFfxivPartyAsc,
            SortFfxivPartyDesc,
            SortByKeys,
            UnsetAll,
            UnsetRegex,
        }

        /// <summary>
        /// Expression types
        /// </summary>
        public enum ExpressionTypeEnum
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
        [Action(order: 1)]
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
        [Action(order: 2)]
        public ExpressionTypeEnum ValueType { get; set; } = ExpressionTypeEnum.String;

        [XmlAttribute("ValueType")]
        public string Xml_ValueType
        {
            get => XmlAttr.Enum(ValueType, ExpressionTypeEnum.String);
            set => ValueType = XmlAttr.Enum<ExpressionTypeEnum>(value);
        }

        /// <summary>
        /// Name of the list variable
        /// </summary>
        [XmlIgnore]
        [Action(order: 3)]
        public string Name { get; set; } = "";

        [XmlAttribute("Name")]
        public string Xml_Name
        {
            get => XmlAttr.String(Name);
            set => Name = value;
        }

        /// <summary>
        /// Value expression
        /// </summary>
        [XmlIgnore]
        [Action(order: 4)]
        public string ValueExpression { get; set; } = "";

        [XmlAttribute("ValueExpression")]
        public string Xml_ValueExpression
        {
            get => XmlAttr.String(ValueExpression);
            set => ValueExpression = value;
        }

        /// <summary>
        /// Index inside the list variable (zero-based)
        /// </summary>
        [XmlIgnore]
        [Action(order: 5)]
        public string Index { get; set; } = "";

        [XmlAttribute("Index")]
        public string Xml_Index
        {
            get => XmlAttr.String(Index);
            set => Index = value;
        }

        /// <summary>
        /// Name of the target variable for some operations
        /// </summary>
        [XmlIgnore]
        [Action(order: 6)]
        public string Target { get; set; } = "";

        [XmlAttribute("Target")]
        public string Xml_Target
        {
            get => XmlAttr.String(Target);
            set => Target = value;
        }

        /// <summary>
        /// Indicates whether referenced variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [Action(order: 7)] // todo need to couple this with variable on editor
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
        [Action(order: 8)] // todo need to couple this with variable on editor
        public bool TargetPersistent { get; set; } = false;

        [XmlAttribute("TargetPersistent")]
        public string Xml_TargetPersistent
        {
            get => XmlAttr.Bool(TargetPersistent, false);
            set => TargetPersistent = XmlAttr.Bool(value);
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation(Context ctx)
        {
            string sPersistL = I18n.TrlVarPersist(Persistent);
            string tPersistL = I18n.TrlVarPersist(TargetPersistent);
            string exprTypeL = I18n.TrlExprType(ValueType == ExpressionTypeEnum.String);
            switch (Operation)
            {
                case OperationEnum.Unset:
                    return I18n.Translate("internal/Action/desclistunset", "unset {1}list variable ({0})", Name, sPersistL);
                case OperationEnum.Push:
                    return I18n.Translate(
                        "internal/Action/desclistpush",
                        "push the value from {3} expression ({2}) to the end of {1}list variable ({0})",
                        Name, sPersistL, ValueExpression, exprTypeL
                    );
                case OperationEnum.Insert:
                    return I18n.Translate(
                        "internal/Action/desclistinsert",
                        "insert the value from {3} expression ({2}) to index ({4}) on {1}list variable ({0})",
                        Name, sPersistL, ValueExpression, exprTypeL, Index
                    );
                case OperationEnum.Set:
                    return I18n.Translate(
                        "internal/Action/desclistset",
                        "set the value from {3} expression ({2}) to index ({4}) on {1}list variable ({0})",
                        Name, sPersistL, ValueExpression, exprTypeL, Index
                    );
                case OperationEnum.SetAll:
                    if (string.IsNullOrWhiteSpace(Index))
                    {
                        return I18n.Translate(
                            "internal/Action/desclistsetall",
                            "set all values on {1}list ({0}) to {3} expr ({2})",
                            Name, sPersistL, ValueExpression, exprTypeL
                        );
                    }
                    return I18n.Translate(
                        "internal/Action/desclistsetallresize",
                        "set all values on {1}list ({0}) to {3} expr ({2}) (resized to length ({4}))",
                        Name, sPersistL, ValueExpression, exprTypeL, Index
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
                        Name, sPersistL, Target, tPersistL, index
                    );
                case OperationEnum.PopToListInsert:
                    if (string.IsNullOrWhiteSpace(ValueExpression))
                    {
                        return I18n.Translate(
                            "internal/Action/desclistpoptolist",
                            "pop index ({2}) of {1}list variable ({0}) to the end of {5}list variable ({4})",
                            Name, sPersistL, Index, Target, tPersistL
                        );
                    }
                    return I18n.Translate(
                        "internal/Action/desclistpoptolistinsert",
                        "pop index ({2}) of {1}list variable ({0}) and insert to index ({5}) of {4}list variable ({3})",
                        Name, sPersistL, Index,
                        Target, tPersistL, ValueExpression
                    );
                case OperationEnum.PopToListSet:
                    return I18n.Translate(
                        "internal/Action/desclistpoptolistset",
                        "pop index ({2}) of {1}list variable ({0}) and set to index ({5}) of {4}list variable ({3})",
                        Name, sPersistL, Index,
                        Target, tPersistL, ValueExpression
                    );
                case OperationEnum.SortAlphaAsc:
                case OperationEnum.SortAlphaDesc:
                    string strOrder = I18n.TrlSortAscOrDesc(Operation == OperationEnum.SortAlphaAsc);
                    return I18n.Translate(
                        "internal/Action/desclistsortstring",
                        "sort {1}list variable ({0}) in an alphabetically {2} order",
                        Name, sPersistL, strOrder
                    );
                case OperationEnum.SortNumericAsc:
                case OperationEnum.SortNumericDesc:
                    string numOrder = I18n.TrlSortAscOrDesc(Operation == OperationEnum.SortNumericAsc);
                    return I18n.Translate(
                        "internal/Action/desclistsortnum",
                        "sort {1}list variable ({0}) in a numerically {2} order",
                        Name, sPersistL, numOrder
                    );
                case OperationEnum.SortFfxivPartyAsc:
                case OperationEnum.SortFfxivPartyDesc:
                    string jobOrder = I18n.TrlSortAscOrDesc(Operation == OperationEnum.SortFfxivPartyAsc);
                    return I18n.Translate(
                        "internal/Action/desclistsortffxiv",
                        "sort {1}list variable ({0}) in an FFXIV party job {2} order",
                        Name, sPersistL, jobOrder
                    );
                case OperationEnum.SortByKeys:
                    return I18n.Translate(
                        "internal/Action/desclistsortbykeys",
                        "sort {1}list variable ({0}) by keys ({2})",
                        Name, sPersistL, ValueExpression
                    );
                case OperationEnum.Copy:
                    return I18n.Translate(
                        "internal/Action/desclistcopy",
                        "copy {2}list variable ({0}) to {3}list variable ({1})",
                        Name, Target, sPersistL, tPersistL
                    );
                case OperationEnum.InsertList:
                    return I18n.Translate(
                        "internal/Action/desclistinsertlist",
                        "insert {3}list variable ({0}) into {4}list variable ({1}) at index ({2})",
                        Name, Target, Index, sPersistL, tPersistL
                    );
                case OperationEnum.Join:
                    return I18n.Translate(
                        "internal/Action/desclistjoin",
                        "join all values in {3}list variable ({0}) to {4}scalar variable ({1}) using {5} expression ({2}) as separator",
                        Name, Target, ValueExpression, sPersistL, tPersistL, exprTypeL
                    );
                case OperationEnum.Split:
                    return I18n.Translate(
                        "internal/Action/desclistsplit",
                        "split {3}scalar variable ({0}) into {4}list variable ({1}) using {5} expression ({2}) as separator",
                        Name, Target, ValueExpression, sPersistL, tPersistL, exprTypeL
                    );
                case OperationEnum.Build:
                    if (
                        ValueType == ExpressionTypeEnum.String
                        && !ValueExpression.StartsWith("$") && !ValueExpression.StartsWith("¡è{")
                    )
                    {
                        return I18n.Translate(
                            "internal/Action/desclistbuild",
                            "build {1}list variable ({0}) from string ({2}) separated by ({3})",
                            Target, tPersistL,
                            ValueExpression.Length == 0 ? "" : ValueExpression.Substring(1),
                            ValueExpression.Length == 0 ? "" : ValueExpression.Substring(0, 1)
                        );
                    }
                    return I18n.Translate(
                        "internal/Action/desclistbuildraw",
                        "build {1}list variable ({0}) from {3} expression ({2}) separated by its first character",
                        Target, tPersistL, ValueExpression, exprTypeL
                    );
                case OperationEnum.Filter:
                    return I18n.Translate(
                        "internal/Action/desclistfilter",
                        "Use expression ({4}) to filter {1}list ({0}) into {3}list ({2})",
                        Name, sPersistL, Target, tPersistL, ValueExpression
                    );
                case OperationEnum.UnsetAll:
                    return I18n.Translate( "internal/Action/desclistunsetall", "unset all {0}list variables", sPersistL);
                case OperationEnum.UnsetRegex:
                    return I18n.Translate(
                        "internal/Action/desclistunsetregex",
                        "unset {1}list variables matching regular expression ({0})",
                        Name, sPersistL
                    );
            }
            return "";
        }

        private string GetListExpressionValue(Context ctx, ExpressionTypeEnum typ, string expr)
        {
            switch (typ)
            {
                case ExpressionTypeEnum.Numeric:
                    return I18n.ThingToString(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, expr));
                case ExpressionTypeEnum.String:
                    return ctx.EvaluateStringExpression(ActionContextLogger, ctx, expr);
            }
            return "";
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai.ctx;
            string sourcename = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Name);
            VariableStore svs = Persistent ? ctx.Plugin.cfg.PersistentVariables : ctx.Plugin.sessionvars;
            VariableStore tvs = TargetPersistent ? ctx.Plugin.cfg.PersistentVariables : ctx.Plugin.sessionvars;
            string sPersist = I18n.TrlVarPersist(Persistent);
            string tPersist = I18n.TrlVarPersist(TargetPersistent);
            string changer;
            if (ctx.Trigger != null)
            {
                changer = I18n.Translate("internal/Action/changetagtrigaction", "Trigger '{0}' action '{1}'", ctx.Trigger.LogName, Describe(ctx));
            }
            else
            {
                changer = I18n.Translate("internal/Action/changetagtestmode", "Action '{0}' test mode", Describe(ctx));
            }
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
                        string value = GetListExpressionValue(ctx, ValueType, ValueExpression);
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
                        string value = GetListExpressionValue(ctx, ValueType, ValueExpression);
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
                        CheckInvalidDymanicExpr(ValueExpression, invalidExprs);

                        string value = GetListExpressionValue(ctx, ValueType, ValueExpression);
                        int index = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Index);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, true);
                            ctx.varName = (Persistent ? "plvar:" : "lvar:") + sourcename;   // ${_this}
                            ctx.listIndex = index;  // ${_idx}
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
                        CheckInvalidDymanicExpr(ValueExpression, invalidExprs);
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
                                string expr = GetListExpressionValue(ctx, ValueType, ValueExpression);
                                vlNew.Push(new VariableScalar() { Value = expr }, changer);
                            }
                            svs.List[sourcename] = vlNew;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listsetall",
                            "All values in {1}list variable ({0}) set to ({2})",
                            sourcename, sPersist, ValueExpression));
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
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Target);
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
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Target);
                        bool isInsert = Operation == OperationEnum.PopToListInsert;
                        bool popToEnd = isInsert && string.IsNullOrWhiteSpace(ValueExpression);

                        int rawSourceIndex = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Index);
                        int rawTargetIndex = 0;
                        if (!popToEnd)
                        {
                            rawTargetIndex = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, ValueExpression);
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
                case OperationEnum.SortAlphaAsc:
                    {
                        string order = I18n.TrlSortAscOrDesc(true);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            vl.SortAlphaAsc(changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listsortstr",
                            "{1}List variable ({0}) sorted in alphabetically {2} order", sourcename, sPersist, order));
                    }
                    break;
                case OperationEnum.SortAlphaDesc:
                    {
                        string order = I18n.TrlSortAscOrDesc(false);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            vl.SortAlphaDesc(changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listsortstr",
                            "{1}List variable ({0}) sorted in alphabetically {2} order", sourcename, sPersist, order));
                    }
                    break;
                case OperationEnum.SortNumericAsc:
                    {
                        string order = I18n.TrlSortAscOrDesc(true);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            vl.SortNumericAsc(changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listsortnum",
                            "{1}List variable ({0}) sorted in numerically {2} order", sourcename, sPersist, order));
                    }
                    break;
                case OperationEnum.SortNumericDesc:
                    {
                        string order = I18n.TrlSortAscOrDesc(false);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            vl.SortNumericDesc(changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listsortnum",
                            "{1}List variable ({0}) sorted in numerically {2} order", sourcename, sPersist, order));
                    }
                    break;
                case OperationEnum.SortFfxivPartyAsc:
                    {
                        string order = I18n.TrlSortAscOrDesc(true);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            vl.SortFfxivPartyAsc(ctx.Plugin.cfg, changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listsortffxiv",
                            "{1}List variable ({0}) sorted in FFXIV party {2} order", sourcename, sPersist, order));
                    }
                    break;
                case OperationEnum.SortFfxivPartyDesc:
                    {
                        string order = I18n.TrlSortAscOrDesc(false);
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            vl.SortFfxivPartyDesc(ctx.Plugin.cfg, changer);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listsortffxiv",
                            "{1}List variable ({0}) sorted in FFXIV party {2} order", sourcename, sPersist, order));
                    }
                    break;
                case OperationEnum.SortByKeys:
                    {
                        string[] invalidExprs = new[] { "${_row", "${_col", "${_key}", "${_val}" };
                        CheckInvalidDymanicExpr(ValueExpression, invalidExprs);

                        // parsing expressions like "n+:key1, s-:key2, s+:key3, ..."
                        ParseSortKeyFunctions(ValueExpression,
                            out List<bool> isNumeric, out List<bool> isAscending,
                            out List<string> keysExpr, out List<List<string>> values);
                        int keysCount = keysExpr.Count;

                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            ctx.varName = (Persistent ? "plvar:" : "lvar:") + Name;    // for ${_this}
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
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Target);
                        VariableList vl = null;
                        lock (svs.List)
                        {
                            vl = svs.GetListVariable(sourcename, false);
                        }
                        lock (tvs.List)
                        {
                            VariableList newvl = new VariableList();
                            foreach (Variable x in vl.Values)
                            {
                                newvl.Push(x.Duplicate(), changer);
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
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Target);
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
                        CheckInvalidDymanicExpr(ValueExpression, invalidExprs);
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Target);
                        VariableList vlResult = new VariableList();
                        lock (svs.List)
                        {
                            VariableList vl = svs.GetListVariable(sourcename, false);
                            ctx.varName = (Persistent ? "plvar:" : "lvar:") + Name;    // for ${_this}
                            for (int i = 0; i < vl.Size; i++)
                            {
                                ctx.listIndex = i + 1;  // for ${_idx}
                                double result = ctx.EvaluateNumericExpression(ActionContextLogger, ctx, ValueExpression);
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
                        string separator = GetListExpressionValue(ctx, ValueType, ValueExpression);
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Target);
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
                        string separator = GetListExpressionValue(ctx, ValueType, ValueExpression);
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Target);
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
                        string expr = GetListExpressionValue(ctx, ValueType, ValueExpression);
                        string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Target);
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
                        svs.UnsetVariableRegex(svs.List, Name);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/listunsetregex",
                            "All {1}list variables matching ({0}) unset", Name, sPersist));
                        break;
                    }
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionVariableList(ActionOld oldAction)
        {
            var action = new ActionVariableList();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._ListVariableOp;
            action.ValueType = (ExpressionTypeEnum)(int)oldAction._ListVariableExpressionType;
            action.Name = oldAction._ListVariableName;
            action.ValueExpression = oldAction._ListVariableExpression;
            action.Index = oldAction._ListVariableIndex;
            action.Target = oldAction._ListVariableTarget;
            action.Persistent = oldAction._ListSourcePersist;
            action.TargetPersistent = oldAction._ListTargetPersist;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionVariableList action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.ListVariable;
            oldAction._ListVariableOp = (ActionOld.ListVariableOpEnum)(int)action.Operation;
            oldAction._ListVariableExpressionType = (ActionOld.ListVariableExpTypeEnum)(int)action.ValueType;
            oldAction._ListVariableName = action.Name;
            oldAction._ListVariableExpression = action.ValueExpression;
            oldAction._ListVariableIndex = action.Index;
            oldAction._ListVariableTarget = action.Target;
            oldAction._ListSourcePersist = action.Persistent;
            oldAction._ListTargetPersist = action.TargetPersistent;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
