using System;
using System.Collections.Generic;
using Triggernometry.Core;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Evaluators;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;

namespace Triggernometry.Expressions.String.Parsers
{
    internal static class ColonParser
    {
        internal static string TryParse(string template, Context ctx)
        {
            ctx = ctx ?? Context.Unbound;
            var plug = ctx.Plugin; // can be null

            var colonPos = template.IndexOf(':');
            if (colonPos == -1) return null;

            var rawPrefix = template.Substring(0, colonPos);
            var prefixLower = rawPrefix.TrimEx().ToLowerInvariant();
            
            var rawBody = template.Substring(colonPos + 1);
            var body = rawBody.TrimEx();

            // First handle the cases that could not tolerate the general IndexMemberExpression format:
            switch (prefixLower)
            {
                // check if variable exists (combined the logic for all types of variable expressions)
                // evar  = ev, epvar  = epv;    elvar = el, eplvar = epl;
                // etvar = et, eptvar = ept;    edvar = ed, epdvar = epd;
                case "ev": case "evar":  case "epv": case "epvar":
                case "el": case "elvar": case "epl": case "eplvar":
                case "et": case "etvar": case "ept": case "eptvar":
                case "ed": case "edvar": case "epd": case "epdvar":
                    {
                        var isPersisent = prefixLower.StartsWith("ep");
                        var varType = prefixLower[isPersisent ? 2 : 1]; // 'v' 'l' 't' 'd'
                        var varStore = isPersisent ? plug?.cfg.PersistentVariables : plug?.sessionvars;
                        
                        switch (varType)
                        {
                            case 'v': return ContainsKeyResultWithLock(varStore?.Scalar, body);
                            case 'l': return ContainsKeyResultWithLock(varStore?.List, body);
                            case 't': return ContainsKeyResultWithLock(varStore?.Table, body);
                            case 'd': return ContainsKeyResultWithLock(varStore?.Dict, body);
                            default: return "0"; // should not be here
                        }
                    }
                // etext, eimage for Overlays (Auras);
                // ecallback for named callbacks;
                // estorage for script storage
                case "etext":
                    return plug?.sc?.textitems != null  
                        ? ContainsKeyResultWithLock(plug?.sc?.textitems, body) // new
                        : ContainsKeyResultWithLock(plug?.textauras, body);    // old
                case "eimage":
                    return plug?.sc?.imageitems != null
                        ? ContainsKeyResultWithLock(plug?.sc?.imageitems, body) // new
                        : ContainsKeyResultWithLock(plug?.imageauras, body);    // old
                case "ecallback":
                    return ContainsKeyResultWithLock(plug?.callbacksByName, body);
                case "estorage":
                    return ContainsKeyResultWithLock(plug?.scriptingStorage, body);

                case "env": // folder environment variables
                    Folder f = ctx.Trigger?.Parent;
                    while (f != null)
                    {
                        if (f.EnvironmentVariables.TryGetValue(body, out var value))
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
                case "string":  case "s":
                    return rawBody; // not trimmed

                case "numeric": case "n":
                    return I18n.ThingToString(MathParser.Parse(body));

                case "if":
                    return TernaryParser.Parse(body);

                // retrieve scalar variable value
                case "var":   case "v":
                case "pvar":  case "pv":
                case "!var":  case "!v":
                case "!pvar": case "!pv":
                    {
                        string varname = body;
                        ResolveVarAccess(prefixLower, ctx, out var store, out bool mustExist, out _);
                        lock (store.Scalar)
                        {
                            VariableScalar result = GetVariableWithCondition(store, store.Scalar, varname, mustExist);
                            return result.Value;
                        }
                    }
                case "sfunc":  // "StorageKey(arg1, arg2, ...)"
                    {
                        var methodExpr = new MemberExpression(body);
                        return plug?.InvokeStorageCallback(methodExpr.Name, methodExpr.Args).ToDataString() ?? "";
                    }
                case "func":
                case "f":
                    {
                        var funcResult = StringFunctionParser.TryParse(rawBody); // use untrimmed body
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

            var expr = new IndexMemberExpression(body);

            switch (prefixLower)
            {
                // retrieve list variable value
                case "lvar":   case "l":
                case "plvar":  case "pl":
                case "!lvar":  case "!l":
                case "!plvar": case "!pl":
                case "?lvar":  case "?l":
                    {
                        Func<VariableList, string> evaluator = ListEvaluator.BuildEvaluator(expr);
                        ResolveVarAccess(prefixLower, ctx, out var store, out bool mustExist, out bool isTemp);
                        if (isTemp)
                        {
                            var vl = VariableList.BuildTemp(expr.Name); // name is actually expression: "1, 2, 3"
                            return evaluator(vl);
                        }
                        lock (store.List)
                        {
                            VariableList vl = GetVariableWithCondition(store, store.List, expr.Name, mustExist);
                            return evaluator(vl);
                        }
                    }

                // retrieve dict variable value
                case "dvar":   case "d":
                case "pdvar":  case "pd":
                case "!dvar":  case "!d":
                case "!pdvar": case "!pd":
                case "?dvar":  case "?d":
                    {
                        Func<VariableDictionary, string> evaluator = DictEvaluator.BuildEvaluator(expr);
                        ResolveVarAccess(prefixLower, ctx, out var store, out bool mustExist, out bool isTemp);
                        if (isTemp)
                        {
                            var vd = VariableDictionary.BuildTemp(expr.Name); // name is actually expression: "a=1, b=2"
                            return evaluator(vd);
                        }
                        lock (store.Dict)
                        {
                            VariableDictionary vd = GetVariableWithCondition(store, store.Dict, expr.Name, mustExist);
                            return evaluator(vd);
                        }
                    }

                // retrieve table variable value
                case "tvar":   case "t":
                case "ptvar":  case "pt":
                case "!tvar":  case "!t":
                case "!ptvar": case "!pt":
                case "?tvar":  case "?t":
                    {
                        Func<VariableTable, string> evaluator = TableEvaluator.BuildEvaluator(expr);
                        ResolveVarAccess(prefixLower, ctx, out var store, out bool mustExist, out bool isTemp);
                        if (isTemp)
                        {
                            var vt = VariableTable.BuildTemp(expr.Name); // name is actually expression
                            return evaluator(vt);
                        }
                        lock (store.Table)
                        {
                            VariableTable vt = GetVariableWithCondition(store, store.Table, expr.Name, mustExist);
                            return evaluator(vt);
                        }
                    }

                // row-based table lookup
                case "tvarrl":   case "trl":
                case "ptvarrl":  case "ptrl":
                case "!tvarrl":  case "!trl":
                case "!ptvarrl": case "!ptrl":
                    {
                        // tvarrl:TableName[Header][ColIndex]
                        if (expr.Indexes.Length != 2)
                        {
                            throw new Exception($"Row-based table lookup expects exactly two indices [Header][ColIndex]: '{template}'");
                        }

                        string headerExpr = expr.Index1;
                        int idx = expr.Index2.Equals("last", StringComparison.OrdinalIgnoreCase) ? -1 : (int)MathParser.Parse(expr.Index2);

                        ResolveVarAccess(prefixLower, ctx, out var store, out bool mustExist, out _);
                        lock (store.Table)
                        {
                            VariableTable vt = GetVariableWithCondition(store, store.Table, expr.Name, mustExist);
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
                case "tvarcl":   case "tcl":
                case "ptvarcl":  case "ptcl":
                case "!tvarcl":  case "!tcl":
                case "!ptvarcl": case "!ptcl":
                    {
                        // tvarcl:TableName[Header][RowIndex]
                        if (expr.Indexes.Length != 2)
                        {
                            throw new Exception($"Column-based table lookup expects exactly two indices [Header][RowIndex]: '{template}'");
                        }

                        string headerExpr = expr.Index1;
                        int idx = expr.Index2.Equals("last", StringComparison.OrdinalIgnoreCase) ? -1 : (int)MathParser.Parse(expr.Index2);

                        ResolveVarAccess(prefixLower, ctx, out var store, out bool mustExist, out _);
                        lock (store.Table)
                        {
                            VariableTable vt = GetVariableWithCondition(store, store.Table, expr.Name, mustExist);
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
                case "tvardl":   case "tdl":
                case "ptvardl":  case "ptdl":
                case "!tvardl":  case "!tdl":
                case "!ptvardl": case "!ptdl":
                    {
                        // tvardl:TableName[ColHeader][RowHeader]
                        if (expr.Indexes.Length != 2)
                        {
                            throw new Exception($"Double-based table lookup expects exactly two indices [ColHeader][RowHeader]: '{template}'");
                        }

                        string colHeader = expr.Index1;
                        string rowHeader = expr.Index2;
                        ResolveVarAccess(prefixLower, ctx, out var store, out bool mustExist, out _);
                        lock (store.Table)
                        {
                            VariableTable vt = GetVariableWithCondition(store, store.Table, expr.Name, mustExist);
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

        private static string ContainsKeyResultWithLock<TValue>(IDictionary<string, TValue> dict, string key)
        {
            if (dict == null)
            {
                return "0";
            }
            lock (dict)
            {
                return dict.ContainsKey(key) ? "1" : "0";
            }
        }

        private static void ResolveVarAccess(string prefixLower, Context ctx, out VariableStore store, out bool mustExist, out bool isTemp)
        {
            mustExist = prefixLower.StartsWith("!");
            isTemp = prefixLower.StartsWith("?");
            int pPos = mustExist ? 1 : 0;
            var isPersistent = prefixLower.Length > pPos && prefixLower[pPos] == 'p';
            store = isPersistent ? ctx?.Plugin?.cfg.PersistentVariables : ctx?.Plugin?.sessionvars;
        }

        private static T GetVariableWithCondition<T>(VariableStore store, Dictionary<string, T> variables, string varName, bool mustExist)
            where T: class, new()
        {
            return !mustExist
                ? store.GetVariable(variables, varName, false)
                : store.GetVariable(variables, varName) ?? throw new Exception($"Variable '{varName}' does not exist.");
        }

    }
}
