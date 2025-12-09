using System;
using Triggernometry.Core;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Evaluators;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;

namespace Triggernometry.Expressions.String.Parsers
{
    internal static class ColonExpressionParser
    {
        // 【【要不要 trim part】】
        internal static string TryParse(string rawExpr, Context ctx)
        {
            ctx = ctx ?? Context.Unbound;
            var plug = ctx.Plugin; // can be null

            var colonPos = rawExpr.IndexOf(':');
            if (colonPos == -1) return null;

            var operation = rawExpr.Substring(0, colonPos).TrimEx();
            var operationToLower = operation.ToLowerInvariant();
            var operand = rawExpr.Substring(colonPos + 1);

            // 先处理冒号后面的部分不需要使用通用 VarAccessExpression 解析属性、索引等的：
            switch (operationToLower)
            {
                // check if variable exists (combined the logic for all types of variable expressions)
                // evar  = ev, epvar  = epv;    elvar = el, eplvar = epl;
                // etvar = et, eptvar = ept;    edvar = ed, epdvar = epd;
                case "ev": case "evar":  case "epv": case "epvar":
                case "el": case "elvar": case "epl": case "eplvar":
                case "et": case "etvar": case "ept": case "eptvar":
                case "ed": case "edvar": case "epd": case "epdvar":
                    {
                        var isPersisent = operation.StartsWith("ep");
                        var varType = operation[isPersisent ? 2 : 1]; // 'v' 'l' 't' 'd'
                        var varStore = isPersisent ? plug?.cfg.PersistentVariables : plug?.sessionvars;
                        switch (varType)
                        {
                            case 'v': return varStore?.Scalar.ContainsKey(operand) ?? false ? "1" : "0";
                            case 'l': return varStore?.List  .ContainsKey(operand) ?? false ? "1" : "0";
                            case 't': return varStore?.Table .ContainsKey(operand) ?? false ? "1" : "0";
                            case 'd': return varStore?.Dict  .ContainsKey(operand) ?? false ? "1" : "0";
                            default: return "0"; // should not be here
                        }
                    }
                // etext, eimage for Overlays (Auras);
                // ecallback for named callbacks;
                // estorage for script storage
                case "etext":
                    return plug?.sc?.textitems.ContainsKey(operand)  // new
                        ?? plug?.textauras.ContainsKey(operand)      // old
                        ?? false ? "1" : "0";
                case "eimage":
                    return plug?.sc?.imageitems.ContainsKey(operand) // new
                        ?? plug?.imageauras.ContainsKey(operand)     // old
                        ?? false ? "1" : "0";
                case "ecallback":
                    return plug?.callbacksByName.ContainsKey(operand) ?? false ? "1" : "0";
                case "estorage":
                    return plug?.scriptingStorage.ContainsKey(operand) ?? false ? "1" : "0";

                case "env": // folder environment variables
                    Folder f = ctx.Trigger?.Parent;
                    while (f != null)
                    {
                        if (f.EnvironmentVariables.TryGetValue(operand, out var value))
                        {
                            return value;
                        }
                        f = f.Parent;
                    }
                    return "";

                // The previous logic EvaluateStringExpression(logger, o, operand) would do nothing,
                // since the inner nested expressions were already evaluated.
                // The logger would also not log when invoking EvaluateStringExpression again,
                // since the inner expressions would not change.
                // Same for the numeric case.
                case "string":
                case "s":
                    return operand;

                case "numeric":
                case "n":
                    return I18n.ThingToString(MathParser.Parse(operand));

                case "if":
                    return TernaryParser.Parse(operand);

                // retrieve scalar variable value
                case "var":
                case "v":
                case "pvar":
                case "pv":
                    {
                        var isPersistent = operation.StartsWith("p");
                        var store = isPersistent ? plug.cfg.PersistentVariables : plug.sessionvars;
                        var varname = operand;
                        lock (store.Scalar)
                        {
                            return store.GetScalarVariable(varname).Value;
                        }
                    }
                case "sfunc":  // "StorageKey(arg1, arg2, ...)"
                    {
                        var methodExpr = new MethodExpression(operand);
                        return plug?.InvokeStorageCallback(methodExpr.Name, methodExpr.Args).ToDataString() ?? "";
                    }
                case "func":
                case "f":
                    {
                        var funcResult = StringFunctionParser.TryParse(operand);
                        if (funcResult != null)
                        {
                            return funcResult;
                        }
                        break;
                    }
            }

            // The remaining cases follow the general format:
            // operator:operand = operator:Name[Index].Prop(Args)
            // (Index or Prop must be present)

            var expr = new IndexMethodExpression(operand);

            switch (operationToLower)
            {
                // retrieve list variable value
                case "lvar":
                case "l":
                case "plvar":
                case "pl":
                case "?lvar":
                case "?l":
                    {
                        Func<VariableList, string> evaluator = ListEvaluator.BuildEvaluator(expr);

                        if (operation.StartsWith("?"))
                        {
                            var vl = VariableList.BuildTemp(expr.Name); // name is actually expression: "1, 2, 3"
                            return evaluator(vl);
                        }

                        VariableStore store = rawExpr.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;
                        lock (store.List)
                        {
                            var vl = store.GetListVariable(expr.Name);
                            return evaluator(vl);
                        }
                    }

                // retrieve dict variable value
                case "dvar":
                case "d":
                case "pdvar":
                case "pd":
                case "?dvar":
                case "?d":
                    {
                        Func<VariableDictionary, string> evaluator = DictEvaluator.BuildEvaluator(expr);

                        if (operation.StartsWith("?"))
                        {
                            var vd = VariableDictionary.BuildTemp(expr.Name); // name is actually expression: "a=1, b=2"
                            return evaluator(vd);
                        }

                        VariableStore store = rawExpr.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;
                        lock (store.Dict)
                        {
                            var vd = store.GetDictVariable(expr.Name);
                            return evaluator(vd);
                        }
                    }

                // retrieve table variable value
                case "tvar":
                case "t":
                case "ptvar":
                case "pt":
                case "?tvar":
                case "?t":
                    {
                        Func<VariableTable, string> evaluator = TableEvaluator.BuildEvaluator(expr);

                        if (operation.StartsWith("?"))
                        {
                            var vt = VariableTable.BuildTemp(expr.Name); // name is actually expression
                            return evaluator(vt);
                        }

                        VariableStore store = rawExpr.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;
                        lock (store.Table)
                        {
                            VariableTable vt = store.GetTableVariable(expr.Name);
                            return evaluator(vt);
                        }
                    }

                // row-based table lookup
                case "tvarrl":
                case "trl":
                case "ptvarrl":
                case "ptrl":
                    {
                        VariableStore store = rawExpr.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;

                        // tvarrl:TableName[Header][ColIndex]
                        if (expr.Indexes.Length != 2)
                        {
                            throw new Exception($"Row-based table lookup expects exactly two indices [Header][ColIndex]: '{rawExpr}'");
                        }

                        string headerExpr = expr.Index1;
                        int idx = expr.Index2.Equals("last", StringComparison.OrdinalIgnoreCase) ? -1 : (int)MathParser.Parse(expr.Index2);

                        lock (store.Table)
                        {
                            VariableTable vt = store.GetTableVariable(expr.Name);
                            int rowIdxFrom1 = vt.SeekRow(headerExpr);
                            if (rowIdxFrom1 > 0)
                            {
                                int colIdxFrom1 = idx + (idx >= 0 ? 1 : 0);
                                return vt.Peek(colIdxFrom1, rowIdxFrom1).ToString();
                            }
                        }
                        // not found:
                        return "";
                    }

                // column-based table lookup
                case "tvarcl":
                case "tcl":
                case "ptvarcl":
                case "ptcl":
                    {
                        VariableStore store = rawExpr.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;

                        // tvarcl:TableName[Header][RowIndex]
                        if (expr.Indexes.Length != 2)
                        {
                            throw new Exception($"Column-based table lookup expects exactly two indices [Header][RowIndex]: '{rawExpr}'");
                        }

                        string headerExpr = expr.Index1;
                        int idx = expr.Index2.Equals("last", StringComparison.OrdinalIgnoreCase) ? -1 : (int)MathParser.Parse(expr.Index2);

                        lock (store.Table)
                        {
                            VariableTable vt = store.GetTableVariable(expr.Name);
                            int colIdxFrom1 = vt.SeekColumn(headerExpr);
                            if (colIdxFrom1 > 0)
                            {
                                int rowIdxFrom1 = idx + (idx >= 0 ? 1 : 0);
                                return vt.Peek(colIdxFrom1, rowIdxFrom1).ToString();
                            }
                        }
                        // not found:
                        return "";
                    }

                // double-based lookup based on col/row names
                case "tvardl":
                case "tdl":
                case "ptvardl":
                case "ptdl":
                    {
                        VariableStore store = rawExpr.StartsWith("p") ? plug.cfg.PersistentVariables : plug.sessionvars;

                        // tvardl:TableName[ColHeader][RowHeader]
                        if (expr.Indexes.Length != 2)
                        {
                            throw new Exception($"Double-based table lookup expects exactly two indices [ColHeader][RowHeader]: '{rawExpr}'");
                        }

                        string colHeader = expr.Index1;
                        string rowHeader = expr.Index2;

                        lock (store.Table)
                        {
                            VariableTable vt = store.GetTableVariable(expr.Name);

                            // index starts from 1; 0 if not found
                            int colIdxFrom1 = vt.SeekColumn(colHeader);
                            int rowIdxFrom1 = vt.SeekRow(rowHeader);
                            if (colIdxFrom1 > 0 && rowIdxFrom1 > 0)
                            {
                                return vt.Peek(colIdxFrom1, rowIdxFrom1).ToString();
                            }
                        }

                        // not found:
                        return "";
                    }
            }

            return null;
        }
    }
}
