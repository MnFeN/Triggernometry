using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core.Serialization;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Parsers;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;

namespace Triggernometry.Core.Actions
{

    /// <summary>
    /// Table variable operations
    /// </summary>
    [ActionCategory(ActionCategory.CategoryTypeEnum.Variable)]
    [XmlRoot(ElementName = "VariableTable")]
    public class ActionVariableTable : ActionBase
    {

        #region Properties

        /// <summary>
        /// Table variable operations
        /// </summary>
        public enum OperationEnum
        {
            Unset,
            Set,
            SetAll,
            SlicesSetAll,
            Resize,
            Build,
            SetLine,
            InsertLine,
            RemoveLine,
            Filter,
            FilterLine,
            Copy,
            Append,
            AppendH,
            SortLine,
            GetAllEntities,
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
        /// Table variable operation type
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
        /// Name of the table variable
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
        /// Name of the target table variable for some operations
        /// </summary>
        [XmlIgnore]
        [Action(order: 4)]
        public string TargetName { get; set; } = "";

        [XmlAttribute("TargetName")]
        public string Xml_TargetName
        {
            get => XmlAttr.String(TargetName);
            set => TargetName = value;
        }

        /// <summary>
        /// Value expression
        /// </summary>
        [XmlIgnore]
        [Action(order: 5)]
        public string Value { get; set; } = "";

        [XmlAttribute("Value")]
        public string Xml_Value
        {
            get => XmlAttr.String(Value);
            set => Value = value;
        }

        /// <summary>
        /// X (column) reference
        /// </summary>
        [XmlIgnore]
        [Action(order: 6)]
        public string X { get; set; } = "";

        [XmlAttribute("X")]
        public string Xml_X
        {
            get => XmlAttr.String(X);
            set => X = value;
        }

        /// <summary>
        /// Y (row) reference
        /// </summary>
        [XmlIgnore]
        [Action(order: 7)]
        public string Y { get; set; } = "";

        [XmlAttribute("Y")]
        public string Xml_Y
        {
            get => XmlAttr.String(Y);
            set => Y = value;
        }

        /// <summary>
        /// Indicates whether referenced variable is persistent or not
        /// </summary>
        [XmlIgnore]
        [Action(order: 8)] // todo need to couple this with variable on editor
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
        [Action(order: 9)] // todo need to couple this with variable on editor
        public bool TargetPersistent { get; set; } = false;

        [XmlAttribute("TargetPersistent")]
        public string Xml_TargetPersistent
        {
            get => XmlAttr.Bool(TargetPersistent, false);
            set => TargetPersistent = XmlAttr.Bool(value);
        }

        #endregion


        #region Implementation

        internal override string DescribeImplementation()
        {
            string sPersistT = I18n.TrlVarPersist(Persistent);
            string tPersistT = I18n.TrlVarPersist(TargetPersistent);
            string exprTypeT = I18n.TrlExprType(ValueType == ExpressionTypeEnum.String);
            switch (Operation)
            {
                case OperationEnum.Set:
                    return I18n.Translate(
                        "internal/Action/desctableset",
                        "set {1}table variable ({0}) value at ({2},{3}) with {5} expression ({4})",
                        Name, sPersistT, X, Y, Value, exprTypeT
                    );                    
                case OperationEnum.SetAll:
                    {
                        string temp = I18n.Translate(
                            "internal/Action/desctablesetall",
                            "set all values in {1}table ({0}) to {3} expr ({2})",
                            Name, sPersistT, Value, exprTypeT
                        );
                        bool givenX = !string.IsNullOrWhiteSpace(X);
                        bool givenY = !string.IsNullOrWhiteSpace(Y);
                        if (givenX && givenY)
                        {
                            temp += I18n.Translate(
                                "internal/Action/desctablesetallresizeXY",
                                " (resized to width ({0}) height ({1}))",
                                X, Y
                            );
                        }
                        else if (givenX && !givenY)
                        {
                            temp += I18n.Translate(
                                "internal/Action/desctablesetallresizeX",
                                " (resized to width ({0}))",
                                X
                            );
                        }
                        else if (!givenX && givenY)
                        {
                            temp += I18n.Translate(
                                "internal/Action/desctablesetallresizeY",
                                " (resized to height ({0}))",
                                Y
                            );
                        }
                        return temp;
                    }                    
                case OperationEnum.SlicesSetAll:
                    return I18n.Translate(
                        "internal/Action/desctableslicessetall",
                        "set all values in column(s) ({4}) and row(s) ({5}) of {1}table ({0}) to {3} expr ({2})",
                        Name, sPersistT, Value, exprTypeT, X, Y
                    );
                case OperationEnum.Resize:
                    {
                        string temp = I18n.Translate(
                            "internal/Action/desctableresizeprefix",
                            "resize {1}table variable ({0}) to",
                            Name, sPersistT
                        );
                        bool givenCol = !string.IsNullOrWhiteSpace(X);
                        bool givenRow = !string.IsNullOrWhiteSpace(Y);
                        if (!givenCol && !givenRow)
                        {
                            temp += I18n.Translate("internal/Action/desctableresizeunchanged", " (unchanged)");
                        }
                        if (givenCol)
                        {
                            temp += I18n.Translate("internal/Action/desctableresizecol", " width ({0})", X);
                        }
                        if (givenRow)
                        {
                            temp += I18n.Translate("internal/Action/desctableresizerow", " height ({0})", Y);
                        }
                        return temp;
                    }
                case OperationEnum.Unset:
                    return I18n.Translate(
                        "internal/Action/desctableunset",
                        "unset {1}table variable ({0})",
                        Name, sPersistT
                    );
                case OperationEnum.UnsetAll:
                    return I18n.Translate(
                        "internal/Action/desctableunsetall",
                        "unset {0}all table variables",
                        sPersistT
                    );
                case OperationEnum.UnsetRegex:
                    return I18n.Translate(
                        "internal/Action/desctableunsetregex",
                        "unset {1}table variables matching regular expression ({0})",
                        Name, sPersistT
                    );
                case OperationEnum.Copy:
                    return I18n.Translate(
                        "internal/Action/desctablecopy",
                        "copy {2}table variable ({0}) to {3}table variable ({1})",
                        Name, TargetName, sPersistT, tPersistT
                    );
                case OperationEnum.Append:
                    return I18n.Translate("internal/Action/desctableappend",
                        "vertically append {2}table variable ({0}) to {3}table variable ({1})",
                        Name, TargetName, sPersistT, tPersistT
                    );
                case OperationEnum.AppendH:
                    return I18n.Translate(
                        "internal/Action/desctableappendh",
                        "horizontally append {2}table variable ({0}) to {3}table variable ({1})",
                        Name, TargetName, sPersistT, tPersistT
                    );
                case OperationEnum.Build:
                    int dollarIndex = Value.IndexOf("$");
                    int crcIndex = Value.IndexOf("¡è{");
                    if (
                        ValueType == ExpressionTypeEnum.String
                        && dollarIndex != 0 && dollarIndex != 1 && crcIndex != 0 && crcIndex != 1
                    )
                    {
                        return I18n.Translate(
                            "internal/Action/desctablebuild",
                            "build {1}table variable ({0}) from string ({2}) separated by ({3}) ({4})",
                            TargetName, tPersistT,
                            Value.Length < 2 ? "" : Value.Substring(2),
                            Value.Length < 1 ? "" : Value.Substring(0, 1),
                            Value.Length < 2 ? "" : Value.Substring(1, 1)
                        );
                    }
                    return I18n.Translate(
                        "internal/Action/desctablebuildraw",
                        "build {1}table variable ({0}) from {3} expression ({2}) separated by its first 2 characters",
                        TargetName, tPersistT, Value, exprTypeT
                    );
                case OperationEnum.Filter:
                    return I18n.Translate(
                        "internal/Action/desctablefilter",
                        "Use expression ({4}) to filter {1}table ({0}) into {3}list ({2})",
                        Name, sPersistT, TargetName, tPersistT, Value
                    );
                case OperationEnum.FilterLine:
                    {
                        bool isCol = !string.IsNullOrWhiteSpace(X);
                        string lineType = I18n.TrlTableColOrRow(isCol);
                        return I18n.Translate(
                            "internal/Action/desctablefilterline",
                            "Use expression ({4}) to filter the {5}s in {1}table ({0}) into {3}table ({2})",
                            Name, sPersistT, TargetName, tPersistT, isCol ? X : Y, lineType
                        );
                    }
                case OperationEnum.SetLine:
                    {
                        string lineType = I18n.TrlTableColOrRow(!string.IsNullOrWhiteSpace(X));
                        string index = !string.IsNullOrWhiteSpace(X) ? X : Y;
                        if (
                            ValueType == ExpressionTypeEnum.String
                            && !Value.StartsWith("$") && !Value.StartsWith("¡è{")
                        )
                        {
                            return I18n.Translate(
                                "internal/Action/desctablesetline",
                                "set {1}table ({0}) {2} #({3}) values from string ({4}) separated by ({5})",
                                Name, sPersistT, lineType, index,
                                Value.Length < 1 ? "" : Value.Substring(1),
                                Value.Length < 1 ? "" : Value.Substring(0, 1)
                            );
                        }
                        return I18n.Translate(
                            "internal/Action/desctablesetlineraw",
                            "set {1}table ({0}) {2} #({3}) values from {5} expression ({4}) separated by its first character",
                            Name, sPersistT, lineType, index, Value, exprTypeT
                        );
                    }
                case OperationEnum.InsertLine:
                    {
                        string lineType = I18n.TrlTableColOrRow(!string.IsNullOrWhiteSpace(X));
                        string index = !string.IsNullOrWhiteSpace(X) ? X : Y;
                        if (
                            ValueType == ExpressionTypeEnum.String
                            && !Value.StartsWith("$") && !Value.StartsWith("¡è{")
                        )
                        {
                            return I18n.Translate(
                                "internal/Action/desctableinsertline",
                                "at {1}table ({0}) {3} #({2}), insert values from string ({4}) separated by ({5})",
                                Name, sPersistT, lineType, index,
                                Value.Length < 1 ? "" : Value.Substring(1),
                                Value.Length < 1 ? "" : Value.Substring(0, 1)
                            );
                        }
                        return I18n.Translate(
                            "internal/Action/desctableinsertlineraw",
                            "at {1}table ({0}) {3} #({2}), insert values from {5} expression ({4}) separated by its first character",
                            Name, sPersistT, lineType, index, Value, exprTypeT
                        );
                    }
                case OperationEnum.RemoveLine:
                    {
                        string lineType = I18n.TrlTableColOrRow(!string.IsNullOrWhiteSpace(X));
                        string index = !string.IsNullOrWhiteSpace(X) ? X : Y;
                        return I18n.Translate(
                            "internal/Action/desctableremoveline",
                            "removed {2} #({3}) from {1}table ({0})",
                            Name, sPersistT, lineType, index
                        );
                    }
                case OperationEnum.SortLine:
                    {
                        bool isCol = !string.IsNullOrWhiteSpace(X);
                        string lineType = I18n.TrlTableColOrRow(isCol);
                        return I18n.Translate(
                            "internal/Action/desctablesortline",
                            "sort the {2}s of {1}table variable ({0}) by keys ({3})",
                            Name, sPersistT, lineType, isCol ? X : Y
                        );
                    }
                case OperationEnum.GetAllEntities:
                    {
                        bool hasFilter = !string.IsNullOrWhiteSpace(Y);
                        bool hasSpecifiedProps = !string.IsNullOrWhiteSpace(X);
                        string keySuffix = (hasFilter ? "1" : "0") + (hasSpecifiedProps ? "1" : "0");
                        string key = $"internal/Action/desctablegetallentities{keySuffix}"; //...00, ...01, ...10, ...11
                        string trl = "";
                        switch (keySuffix)
                        {
                            case "11": trl = "Store ({3}) properties of FFXIV entities matching ({2}) in {1}table ({0})"; break;
                            case "10": trl = "Store all properties of FFXIV entities matching ({2}) in {1}table ({0})"; break;
                            case "01": trl = "Store ({3}) properties of all FFXIV entities in {1}table ({0})"; break;
                            case "00": trl = "Store all properties of all FFXIV entities in {1}table ({0})"; break;
                        }
                        return I18n.Translate(key, trl, Name, sPersistT, Y, X);
                    }
                default:
                    return NotImplementedEnumMessage(Operation);
            }
        }

        internal override void ExecuteImplementation(ActionInstance ai)
        {
            Context ctx = ai?.ctx ?? Context.Unbound;
            RealPlugin plug = ctx.Plugin;

            string sourcename = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Name);
            string targetname = ctx.EvaluateStringExpression(ActionContextLogger, ctx, TargetName);
            VariableStore svs = plug.GetVariableStore(Persistent);
            VariableStore tvs = plug.GetVariableStore(TargetPersistent);
            string sPersist = I18n.TrlVarPersist(Persistent);
            string tPersist = I18n.TrlVarPersist(TargetPersistent);
            string expr;
            string ParseExpr()
            {
                if (ValueType == ExpressionTypeEnum.String)
                    return ctx.EvaluateStringExpression(ActionContextLogger, ctx, Value);
                else
                    return I18n.ThingToString(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Value));
            }

            string vtchanger;
            if (ctx.Trigger != null)
            {
                vtchanger = I18n.Translate("internal/Action/changetagtrigaction", "Trigger '{0}' action '{1}'", ctx.Trigger.LogName, Describe());
            }
            else
            {
                vtchanger = I18n.Translate("internal/Action/changetagtestmode", "Action '{0}' test mode", Describe());
            }

            switch (Operation)
            {
                case OperationEnum.UnsetAll:
                    {
                        svs.UnsetAllVariables(svs.Table);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tableunsetall",
                            "All {0}table variables unset", sPersist));
                        break;
                    }
                case OperationEnum.UnsetRegex:
                    {
                        svs.UnsetVariableRegex(svs.Table, Name);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tableunsetregex",
                            "All {1}table variables matching ({0}) unset", Name, sPersist));
                        break;
                    }
                case OperationEnum.Resize:
                    {
                        int w = string.IsNullOrWhiteSpace(X) ? int.MinValue : (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, X);
                        int h = string.IsNullOrWhiteSpace(Y) ? int.MinValue : (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Y);
                        lock (svs.Table) // verified
                        {
                            VariableTable vt = svs.GetTableVariable(sourcename, true);
                            w = w == int.MinValue ? vt.Width : w;
                            h = h == int.MinValue ? vt.Height : h;
                            vt.Resize(w, h, vtchanger);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tableresize",
                            "{3}Table variable ({0}) resized to ({1},{2})", sourcename, w, h, sPersist));
                        break;
                    }
                case OperationEnum.Unset:
                    {
                        svs.UnsetVariable(svs.Table, sourcename);
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tableunset",
                            "{1}Table variable ({0}) unset", sourcename, sPersist));
                        break;
                    }
                case OperationEnum.Copy:
                    {
                        VariableTable vt = null;
                        lock (svs.Table) // verified
                        {
                            if (svs.Table.ContainsKey(sourcename) == true)
                            {
                                vt = (VariableTable)svs.Table[sourcename].Duplicate();
                                vt.LastChanged = DateTime.Now;
                                vt.LastChanger = vtchanger;
                            }
                        }
                        if (vt != null)
                        {
                            lock (tvs.Table)
                            {
                                tvs.Table[targetname] = vt;
                            }
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tablecopy",
                                "{2}Table ({0}) copied to {3}table ({1})",
                                sourcename, targetname, sPersist, tPersist));
                        }
                        else
                        {
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/tablecopynotexist",
                                "{2}Table variable ({0}) couldn't be copied to {3}table ({1}) since it doesn't exist",
                                sourcename, targetname, sPersist, tPersist));
                        }
                        break;
                    }
                case OperationEnum.Append:
                case OperationEnum.AppendH:
                    {
                        VariableTable tableToAppend;
                        lock (svs.Table) // verified
                        {
                            tableToAppend = svs.Table.TryGetValue(sourcename, out VariableTable svt)
                                ? (VariableTable)svt.Duplicate()
                                : new VariableTable();
                        }
                        lock (tvs.Table)
                        {
                            if (!tvs.Table.ContainsKey(targetname))
                            {
                                tvs.Table.Add(targetname, new VariableTable());
                            }
                            VariableTable tvt = tvs.Table[targetname];
                            if (Operation == OperationEnum.Append)
                            {
                                tvt.AppendVertical(tableToAppend, vtchanger);
                            }
                            else
                            {
                                tvt.AppendHorizontal(tableToAppend, vtchanger);
                            }
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tableappend",
                            "{2}Table variable ({0}) appended to {3} table ({1})",
                            sourcename, targetname, sPersist, tPersist));
                        break;
                    }
                case OperationEnum.Set:
                    {
                        string[] invalidExprs = new[] { "${_idx}", "${_key}", "${_val}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);

                        int x = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, X);
                        int y = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Y);

                        lock (svs.Table) // verified
                        {
                            VariableTable vt = svs.GetTableVariable(sourcename, true);
                            int mx = Math.Max(x, vt.Width);
                            int my = Math.Max(y, vt.Height);
                            if (mx != vt.Width || my != vt.Height)
                            {
                                vt.Resize(mx, my);
                            }
                            ctx.varName = (Persistent ? "ptvar:" : "tvar:") + Name;
                            ctx.tableColIndex = x;          // for ${_col}
                            ctx.tableRowIndex = y;          // for ${_row}
                            expr = ParseExpr();
                            vt.Set(x, y, expr, vtchanger);
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tableset",
                            "{4}Table variable ({0}) column ({1}) row ({2}) set to ({3})",
                            sourcename, x, y, expr, sPersist));
                        break;
                    }
                case OperationEnum.SetAll:
                    {
                        string[] invalidExprs = new[] { "${_idx}", "${_key}", "${_val}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);
                        int newWidth = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, X);
                        int newHeight = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Y);
                        VariableTable vtNew = new VariableTable { LastChanger = vtchanger, LastChanged = DateTime.Now };
                        lock (svs.Table)
                        {
                            VariableTable vt = svs.GetTableVariable(sourcename, false);
                            newWidth = newWidth <= 0 ? vt.Width : newWidth;
                            newHeight = newHeight <= 0 ? vt.Height : newHeight;
                            ctx.varName = (Persistent ? "ptvar:" : "tvar:") + Name;
                            for (int y = 1; y <= newHeight; y++)     // x/y index starts from 1
                            {
                                ctx.tableRowIndex = y;          // for ${_row}
                                vtNew.Rows.Add(new VariableTable.VariableTableRow());
                                for (int x = 1; x <= newWidth; x++)
                                {
                                    ctx.tableColIndex = x;      // for ${_col}
                                    expr = ParseExpr();         // evaluate the expression for every grid
                                    vtNew.Rows[y - 1].Values.Add(new VariableScalar() { Value = expr, LastChanger = vtchanger, LastChanged = DateTime.Now });
                                }
                            }
                            svs.Table[sourcename] = vtNew;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tablesetall",
                            "All values in {1}table variable ({0}) set to ({2})",
                            sourcename, sPersist, Value));
                    }
                    break;
                case OperationEnum.SlicesSetAll:
                    {
                        string[] invalidExprs = new[] { "${_idx}", "${_key}", "${_val}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);
                        string colSlicesStr = ctx.EvaluateStringExpression(ActionContextLogger, ctx, X);
                        string rowSlicesStr = ctx.EvaluateStringExpression(ActionContextLogger, ctx, Y);
                        VariableTable vtNew;
                        lock (svs.Table)
                        {
                            VariableTable vt = svs.GetTableVariable(sourcename, false);
                            vtNew = (VariableTable)vt.Duplicate();
                            // index starts from 0
                            List<int> colIndices = ArgHelper.GetSliceIndices(colSlicesStr, vt.Width, X, startIndex: 1);
                            List<int> rowIndices = ArgHelper.GetSliceIndices(rowSlicesStr, vt.Height, Y, startIndex: 1);
                            ctx.varName = (Persistent ? "ptvar:" : "tvar:") + Name;
                            foreach (int rowIndex in rowIndices)
                            {
                                ctx.tableRowIndex = rowIndex + 1;       // for ${_row}
                                foreach (int colIndex in colIndices)
                                {
                                    ctx.tableColIndex = colIndex + 1;   // for ${_col}
                                    expr = ParseExpr();                 // evaluate the expression for every grid
                                    vtNew.Rows[rowIndex].Values[colIndex] = new VariableScalar() { Value = expr, LastChanger = vtchanger, LastChanged = DateTime.Now };
                                }
                            }
                            svs.Table[sourcename] = vtNew;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tableslicessetall",
                            "All values in column ({3}) row ({4}) of {1}table variable ({0}) set to ({2})",
                            sourcename, sPersist, Value, colSlicesStr, rowSlicesStr));
                    }
                    break;
                case OperationEnum.Build:
                    {   // Using the first 2 characters in the expression as the separator to split the remaining part into a new table
                        // e.g. expr = ",|1,2,3|4,5,6|7,8,9"
                        VariableTable vt = new VariableTable();
                        expr = ParseExpr();
                        if (expr.Length > 1)
                        {
                            if (expr[1] == '\n' || expr.Substring(1).StartsWith("\r\n"))
                                expr = ParserCommon.ReplaceLineBreak(expr);
                            char colSeparator = expr[0];
                            char rowSeparator = expr[1];
                            string splitval = expr.Substring(2);
                            vt = VariableTable.Build(splitval, colSeparator, rowSeparator, vtchanger);
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tablebuild",
                                "{1}Table variable ({0}) built from expression ({2}) splitted by ({3}) ({4})",
                                targetname, tPersist, splitval, colSeparator, rowSeparator));
                        }
                        else
                        {
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Warning, I18n.Translate("internal/Action/tablebuildfail",
                                "{1}Table variable ({0}) cannot be built since expression ({2}) length < 2",
                                targetname, tPersist, expr));
                        }

                        lock (tvs.Table)
                        {
                            tvs.Table[targetname] = vt;
                        }
                    }
                    break;
                case OperationEnum.Filter:
                    {
                        VariableList vlResult = new VariableList();
                        string[] invalidExprs = new[] { "${_idx}", "${_key}", "${_val}" };
                        CheckInvalidDymanicExpr(Value, invalidExprs);
                        lock (svs.Table)
                        {
                            VariableTable vt = svs.GetTableVariable(sourcename, false);
                            ctx.varName = (Persistent ? "ptvar:" : "tvar:") + Name;  // for ${_this}

                            for (int rowIndex = 0; rowIndex < vt.Height; rowIndex++)
                            {
                                ctx.tableRowIndex = rowIndex + 1;       // for ${_row}
                                for (int colIndex = 0; colIndex < vt.Width; colIndex++)
                                {
                                    ctx.tableColIndex = colIndex + 1;   // for ${_col}
                                    double result = ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Value);
                                    if (!MathParser.IsZero(result))
                                    {
                                        vlResult.Values.Add(vt.Rows[rowIndex].Values[colIndex].Duplicate());
                                    }
                                }
                            }
                        }
                        vlResult.LastChanger = vtchanger;
                        vlResult.LastChanged = DateTime.Now;
                        lock (tvs.List)
                        {
                            tvs.List[targetname] = vlResult;
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tablefilter",
                                "Filtered {4} elements from {1}table ({0}) into {3}list ({2})",
                                sourcename, sPersist, targetname, tPersist, vlResult.Size));
                        }
                    }
                    break;
                case OperationEnum.FilterLine:
                    {
                        bool isCol = !string.IsNullOrWhiteSpace(X);
                        string rawExpr = isCol ? X : Y;
                        string[] invalidExprs = isCol
                            ? new[] { "${_this}", "${_row", "${_idx}", "${_key}", "${_val}" }
                            : new[] { "${_this}", "${_col", "${_idx}", "${_key}", "${_val}" };
                        CheckInvalidDymanicExpr(rawExpr, invalidExprs);
                        VariableTable vtResult = new VariableTable();
                        lock (svs.Table)
                        {
                            VariableTable vt = svs.GetTableVariable(sourcename, false);
                            ctx.varName = (Persistent ? "ptvar:" : "tvar:") + Name;  // for ${_this}
                            if (isCol)
                            {
                                for (int rowIndex = 0; rowIndex < vt.Height; rowIndex++)
                                {
                                    vtResult.Rows.Add(new VariableTable.VariableTableRow());
                                }
                                for (int colIndex = 0; colIndex < vt.Width; colIndex++)
                                {
                                    ctx.tableColIndex = colIndex + 1;          // for ${_col}
                                    double result = ctx.EvaluateNumericExpression(ActionContextLogger, ctx, rawExpr);
                                    if (!MathParser.IsZero(result))
                                    {
                                        for (int rowIndex = 0; rowIndex < vt.Height; rowIndex++)
                                        {
                                            vtResult.Rows[rowIndex].Values.Add(vt.Rows[rowIndex].Values[colIndex].Duplicate());
                                        }
                                    }
                                }
                                if (vtResult.Width == 0) { vtResult.Rows.Clear(); }
                            }
                            else // is row
                            {
                                for (int rowIndex = 0; rowIndex < vt.Height; rowIndex++)
                                {
                                    ctx.tableRowIndex = rowIndex + 1;      // for ${_row}
                                    double result = ctx.EvaluateNumericExpression(ActionContextLogger, ctx, rawExpr);
                                    if (!MathParser.IsZero(result))
                                    {
                                        var newRow = new VariableTable.VariableTableRow();
                                        for (int colIndex = 0; colIndex < vt.Width; colIndex++)
                                        {
                                            newRow.Values.Add(vt.Rows[rowIndex].Values[colIndex].Duplicate());
                                        }
                                        vtResult.Rows.Add(newRow);
                                    }
                                }
                            }
                        }
                        vtResult.LastChanger = vtchanger;
                        vtResult.LastChanged = DateTime.Now;
                        lock (tvs.Table)
                        {
                            tvs.Table[targetname] = vtResult;
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tablefilterline",
                                "Filtered {4} {5}s from {1}table ({0}) into {3}table ({2})",
                                sourcename, sPersist, targetname, tPersist,
                                isCol ? vtResult.Width : vtResult.Height, I18n.TrlTableColOrRow(isCol)));
                        }
                    }
                    break;
                case OperationEnum.SetLine:
                case OperationEnum.InsertLine:
                    {
                        expr = ParseExpr();
                        string separator = expr.Length > 0 ? expr.Substring(0, 1) : "";
                        string splitval = expr.Length > 0 ? expr.Substring(1) : "";
                        string[] newValues = separator.Length > 0 ? splitval.Split(separator[0]) : new string[0];
                        bool isRow = string.IsNullOrWhiteSpace(X);
                        string lineType = I18n.TrlTableColOrRow(!isRow);
                        int rawIndex = (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, isRow ? Y : X);

                        lock (svs.Table) // verified
                        {
                            VariableTable vt = svs.GetTableVariable(sourcename, true);
                            int tableLength = isRow ? vt.Height : vt.Width;

                            // index start from 0
                            int index = rawIndex < 0 ? rawIndex + tableLength : rawIndex - 1;
                            if (index < 0)
                                break;

                            if (Operation == OperationEnum.SetLine)
                            {
                                if (isRow)
                                    vt.SetRow(index, newValues, vtchanger);
                                else
                                    vt.SetColumn(index, newValues, vtchanger);

                                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tablesetline",
                                    "{1}Table ({0}) {3} #({2}) set to ({4})",
                                    sourcename, sPersist, index, lineType, splitval));
                            }
                            else // InsertLine
                            {
                                if (isRow)
                                    vt.InsertRow(index, newValues, vtchanger);
                                else
                                    vt.InsertColumn(index, newValues, vtchanger);

                                AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tableinsertline",
                                    "Inserted ({4}) to {1}Table ({0}) {3} #({2})",
                                    sourcename, sPersist, index, lineType, splitval));
                            }
                        }
                    }
                    break;
                case OperationEnum.RemoveLine:
                    {
                        bool isRow = string.IsNullOrWhiteSpace(X);
                        string lineType = I18n.TrlTableColOrRow(!isRow);

                        lock (svs.Table) // verified
                        {
                            VariableTable vt = svs.GetTableVariable(sourcename, true);
                            int tableLength = isRow ? vt.Height : vt.Width;
                            int rawIndex = isRow ? (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, Y)
                                                   : (int)ctx.EvaluateNumericExpression(ActionContextLogger, ctx, X);
                            // index start from 0
                            int index = rawIndex < 0 ? rawIndex + tableLength : rawIndex - 1;

                            if (isRow) { vt.RemoveRow(index, vtchanger); }
                            else { vt.RemoveColumn(index, vtchanger); }

                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tableremoveline",
                                "Removed {3} #({2}) from {1}table ({0})",
                                sourcename, sPersist, index, lineType));
                        }
                    }
                    break;
                case OperationEnum.SortLine:
                    {
                        bool isCol = !string.IsNullOrWhiteSpace(X);
                        string lineType = I18n.TrlTableColOrRow(isCol);
                        string rawExpr = isCol ? X : Y;
                        string[] invalidExprs = isCol
                            ? new[] { "${_this}", "${_row", "${_idx}", "${_key}", "${_val}" }
                            : new[] { "${_this}", "${_col", "${_idx}", "${_key}", "${_val}" };
                        CheckInvalidDymanicExpr(rawExpr, invalidExprs);

                        // parsing expressions like "n+:key1, s-:key2, s+:key3, ..."
                        ParseSortKeyFunctions(rawExpr, out List<bool> isNumeric, out List<bool> isAscending,
                            out List<string> keysExpr, out List<List<string>> values);
                        int keysCount = keysExpr.Count;

                        lock (svs.Table)
                        {
                            VariableTable vt = svs.GetTableVariable(sourcename, false);
                            ctx.varName = (Persistent ? "ptvar:" : "tvar:") + Name; // for ${_row[i]}
                            if (isCol)
                            {
                                // Iterate through the columns and evaluate the key expression in the current context
                                for (int colIndex = 0; colIndex < vt.Width; colIndex++)
                                {
                                    ctx.tableColIndex = colIndex + 1;  // for ${_col}
                                    for (int keyIndex = 0; keyIndex < keysCount; keyIndex++)
                                    {
                                        string keyValue = isNumeric[keyIndex]
                                            ? I18n.ThingToString(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, keysExpr[keyIndex]))
                                            : ctx.EvaluateStringExpression(ActionContextLogger, ctx, keysExpr[keyIndex]);
                                        values[keyIndex].Add(keyValue);
                                    }
                                }

                                IOrderedEnumerable<int> sortedIndices = ApplySorting(vt.Width, isNumeric, isAscending, values);
                                foreach (var row in vt.Rows)
                                {
                                    var sortedValue = sortedIndices.Select(i => row.Values[i]).ToList();
                                    row.Values = sortedValue;
                                }
                            }
                            else // is row
                            {
                                // Iterate through the rows and evaluate the key expression in the current context
                                for (int rowIndex = 0; rowIndex < vt.Height; rowIndex++)
                                {
                                    ctx.tableRowIndex = rowIndex + 1;  // for ${_row}
                                    for (int keyIndex = 0; keyIndex < keysCount; keyIndex++)
                                    {
                                        string keyValue = isNumeric[keyIndex]
                                            ? I18n.ThingToString(ctx.EvaluateNumericExpression(ActionContextLogger, ctx, keysExpr[keyIndex]))
                                            : ctx.EvaluateStringExpression(ActionContextLogger, ctx, keysExpr[keyIndex]);
                                        values[keyIndex].Add(keyValue);
                                    }
                                }

                                IOrderedEnumerable<int> sortedIndices = ApplySorting(vt.Height, isNumeric, isAscending, values);
                                var sortedRows = sortedIndices.Select(i => vt.Rows[i]).ToList();
                                vt.Rows = sortedRows;
                            }

                            vt.LastChanger = vtchanger;
                            vt.LastChanged = DateTime.Now;
                        }

                        for (int i = 0; i < keysCount; i++)
                        {   // logging each sorting keys
                            AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tablesortline",
                                "Sorting {2}s of {1}table ({0}): function ({3}/{4}, {6}) = ({5}). Keys: ({7})",
                                sourcename, sPersist, lineType, i + 1, keysCount, keysExpr[i],
                                (isNumeric[i] ? "n" : "s") + (isAscending[i] ? "+" : "-"),
                                string.Join(", ", values[i])));
                        }
                    }
                    break;
                case OperationEnum.GetAllEntities:
                    {
                        var entities = string.IsNullOrWhiteSpace(Y)
                            ? FFXIV.Entity.GetEntities()
                            : XivEntityParser.GetEntitiesByCondition(
                                ctx.EvaluateStringExpression(ActionContextLogger, ctx, Y),
                                false);

                        var propNames = string.IsNullOrWhiteSpace(X)
                            ? FFXIV.Entity.RecommendedEntityPropNames.Select(x => x.ToLower()).Concat(FFXIV.Job.LegalJobPropNames).OrderBy(s => s)
                            : (IEnumerable<string>)ArgHelper.SplitArguments(ctx.EvaluateStringExpression(ActionContextLogger, ctx, X), false);
                        if (string.IsNullOrWhiteSpace(X))
                        {
                            var specialKeys = new List<string> { "id", "name", "x", "y", "z", "h", "bnpcid" };
                            propNames = specialKeys.Concat(propNames.Except(specialKeys));
                        }

                        VariableTable vt = new VariableTable { LastChanger = vtchanger, LastChanged = DateTime.Now };
                        var headerRow = new VariableTable.VariableTableRow
                        {
                            Values = propNames.Select(prop => (Variable)new VariableScalar(prop)).ToList()
                        };
                        vt.Rows.Add(headerRow);

                        foreach (var entity in entities)
                        {
                            if (entity.ID == 0) continue;
                            var row = new VariableTable.VariableTableRow
                            {
                                Values = propNames.Select(prop => XivEntityParser.EvaluateEntityMembers(entity, prop))
                                                  .Select(result => (Variable)new VariableScalar(result))
                                                  .ToList()
                            };
                            vt.Rows.Add(row);
                        }
                        lock (svs.Table)
                        {
                            svs.Table[sourcename] = vt;
                        }
                        AddToLog(ctx, RealPlugin.DebugLevelEnum.Verbose, I18n.Translate("internal/Action/tablegetallentities",
                            "Saved {2} entities into {1}table variable ({0})",
                            sourcename, sPersist, vt.Rows.Count - 1));
                    }
                    break;
                default:
                    throw NotImplementedEnumException(Operation);
            }
        }

        #endregion

        #region Old Action Converter

        // (this)ActionOld
        public static explicit operator ActionVariableTable(ActionOld oldAction)
        {
            var action = new ActionVariableTable();
            oldAction.CopyCommonPropertiesTo(action);
            action.Operation = (OperationEnum)(int)oldAction._TableVariableOp;
            action.ValueType = (ExpressionTypeEnum)(int)oldAction._TableVariableExpressionType;
            action.Name = oldAction._TableVariableName;
            action.TargetName = oldAction._TableVariableTarget;
            action.Value = oldAction._TableVariableExpression;
            action.X = oldAction._TableVariableX;
            action.Y = oldAction._TableVariableY;
            action.Persistent = oldAction._TableSourcePersist;
            action.TargetPersistent = oldAction._TableTargetPersist;
            return action;
        }

        // (ActionOld)this
        public static explicit operator ActionOld(ActionVariableTable action)
        {
            var oldAction = new ActionOld();
            action.CopyCommonPropertiesTo(oldAction);
            oldAction.ActionType = ActionOld.ActionTypeEnum.TableVariable;
            oldAction._TableVariableOp = (ActionOld.TableVariableOpEnum)(int)action.Operation;
            oldAction._TableVariableExpressionType = (ActionOld.TableVariableExpTypeEnum)(int)action.ValueType;
            oldAction._TableVariableName = action.Name;
            oldAction._TableVariableTarget = action.TargetName;
            oldAction._TableVariableExpression = action.Value;
            oldAction._TableVariableX = action.X;
            oldAction._TableVariableY = action.Y;
            oldAction._TableSourcePersist = action.Persistent;
            oldAction._TableTargetPersist = action.TargetPersistent;
            return oldAction;
        }

        #endregion Old Action Converter
    }

}
