using System;
using System.Collections.Generic;
using System.Linq;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Localization;
using static Triggernometry.Expressions.String.Utils.ArgHelper;

namespace Triggernometry.Expressions.String.Evaluators
{
    internal static class DictEvaluator
    {
        internal static Func<VariableDictionary, string> BuildEvaluator(IndexMethodExpression expr)
        {
            // invalid (only name)
            if (expr.Indexes.Length == 0 && !expr.Method.HasValue)
                throw new NotImplementedException("dict");

            // dvar:Name[Key]
            if (expr.Indexes.Length > 0)
            {
                string key = expr.Index;
                return vd => vd.GetValue(key).ToString();
            }

            // dvar:Name.Method(Args)
            var methodName = expr.Method.Name.ToLowerInvariant();
            var args = expr.Method.Args ?? Array.Empty<string>();
            int argCount = args.Length;

            switch (methodName)
            {
                case "size":
                case "length":
                    CheckArgCount("0", argCount, methodName, expr.RawExpression);
                    return vd => vd.Size.ToString();

                case "ekey":
                case "evalue":
                    CheckArgCount("1", argCount, methodName, expr.RawExpression);
                    {
                        string queryStr = args[0];
                        bool checkKey = methodName == "ekey";
                        return vd =>
                        {
                            bool exist = checkKey ? vd.ContainsKey(queryStr) : vd.ContainsValue(queryStr);
                            return exist ? "1" : "0";
                        };
                    }

                case "ifekey":
                case "ifevalue":
                    CheckArgCount("3", argCount, methodName, expr.RawExpression);
                    {
                        string queryStr = args[0];
                        string trueStr = args[1];
                        string falseStr = args[2];
                        bool checkKey = methodName == "ifekey";
                        return vd =>
                        {
                            bool exist = checkKey ? vd.ContainsKey(queryStr) : vd.ContainsValue(queryStr);
                            return exist ? trueStr : falseStr;
                        };
                    }

                case "count": // count(value)
                    CheckArgCount("1", argCount, methodName, expr.RawExpression);
                    {
                        string value = args[0];
                        return vd => vd.Count(value).ToString();
                    }

                case "keyof":
                    CheckArgCount("1", argCount, methodName, expr.RawExpression);
                    {
                        string value = args[0];
                        return vd => vd.KeyOf(value);
                    }

                case "keysof":
                    CheckArgCount("1-2", argCount, methodName, expr.RawExpression);
                    {
                        string value = args[0];
                        string joiner = GetArgument(args, 1, ",");
                        return vd => vd.KeysOf(value, joiner);
                    }

                case "joinkeys":
                    CheckArgCount("0-1", argCount, methodName, expr.RawExpression);
                    {
                        string joiner = GetArgument(args, 0, ",");
                        return vd => vd.JoinKeys(joiner);
                    }

                case "joinvalues":
                    {
                        string joiner = GetArgument(args, 0, ",");
                        if (argCount <= 1)
                        {
                            // joinvalues(joiner = ",")
                            return vd => vd.JoinValues(joiner);
                        }
                        else
                        {
                            // joinvalues(joiner, params keys)
                            string[] keys = args.Skip(1).ToArray();
                            return vd => vd.JoinValues(joiner, keys);
                        }
                    }

                case "joinall":
                    {
                        string kvjoiner = GetArgument(args, 0, "=");
                        string pairjoiner = GetArgument(args, 1, ",");

                        if (argCount <= 2)
                        {
                            // joinall(kvjoiner = "=", pairjoiner = ",")
                            return vd => vd.JoinAll(kvjoiner, pairjoiner);
                        }
                        else
                        {
                            // joinall(kvjoiner, pairjoiner, params keys)
                            string[] keys = args.Skip(2).ToArray();
                            return vd => vd.JoinAll(kvjoiner, pairjoiner, keys);
                        }
                    }

                case "sumkeys":
                case "sum": // sum values
                    CheckArgCount("0", argCount, methodName, expr.RawExpression);
                    {
                        bool sumKeys = methodName == "sumkeys";
                        return vd =>
                        {
                            double sum = sumKeys ? vd.SumKeys() : vd.Sum();
                            return I18n.ThingToString(sum);
                        };
                    }

                case "minkey":
                case "maxkey":
                    CheckArgCount("0-1", argCount, methodName, expr.RawExpression);
                    {
                        bool isMin = methodName.StartsWith("min", StringComparison.OrdinalIgnoreCase);
                        var valueType = GetArgument(args, 0);
                        var extremum = ExtremumEvaluator.BuildEvaluator(valueType, isMin, expr);
                        return vd => extremum(vd.Values.Keys);
                    }

                case "min":  // dvar:dict.min(type = "n")  num = "n" / str = "s" / hex = "h"
                case "max":
                    CheckArgCount("0-1", argCount, methodName, expr.RawExpression);
                    {
                        bool isMin = methodName.StartsWith("min", StringComparison.OrdinalIgnoreCase);
                        var valueType = GetArgument(args, 0);
                        var extremum = ExtremumEvaluator.BuildEvaluator(valueType, isMin, expr);
                        return vd => extremum(vd.Values.Values.Select(v => v.ToString()));
                    }

                default:
                    throw new Exception($"Unknown dict method '{methodName}' in expression '{expr.RawExpression}'.");
            }
        }

    }
}
