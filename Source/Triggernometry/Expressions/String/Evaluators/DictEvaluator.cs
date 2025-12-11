using System;
using System.Linq;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Localization;
using static Triggernometry.Expressions.String.Utils.ArgHelper;

namespace Triggernometry.Expressions.String.Evaluators
{
    internal static class DictEvaluator
    {
        internal static Func<VariableDictionary, string> BuildEvaluator(IndexMemberExpression expr)
        {
            // invalid (only name)
            if (expr.Indexes.Length == 0 && !expr.Member.HasValue)
                throw new NotImplementedException("dict");

            // dvar:Name[Key]
            if (expr.Indexes.Length > 0)
            {
                string key = expr.Index;
                return vd => vd.GetValue(key).ToString();
            }

            // dvar:Name.Method(Args)
            var methodName = expr.Member.Name.ToLowerInvariant();
            var args = expr.Member.Args;

            void CheckArgCountLocal(string argCountRule)
            {
                CheckArgCount(argCountRule, args.Length, methodName, expr.RawExpression);
            }

            switch (methodName)
            {
                case "size":
                case "length":
                    CheckArgCountLocal("0");
                    return vd => vd.Size.ToString();

                case "ekey":
                case "evalue":
                    CheckArgCountLocal("1");
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
                    CheckArgCountLocal("3");
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
                    CheckArgCountLocal("1");
                    {
                        string value = args[0];
                        return vd => vd.Count(value).ToString();
                    }

                case "keyof":
                    CheckArgCountLocal("1");
                    {
                        string value = args[0];
                        return vd => vd.KeyOf(value);
                    }

                case "keysof":
                    CheckArgCountLocal("1-2");
                    {
                        string value = args[0];
                        string joiner = GetArgument(args, 1, ",");
                        return vd => vd.KeysOf(value, joiner);
                    }

                case "joinkeys":
                    CheckArgCountLocal("0-1");
                    {
                        string joiner = GetArgument(args, 0, ",");
                        return vd => vd.JoinKeys(joiner);
                    }

                case "joinvalues":
                    {
                        string joiner = GetArgument(args, 0, ",");
                        if (args.Length <= 1)
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

                        if (args.Length <= 2)
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
                    CheckArgCountLocal("0");
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
                    CheckArgCountLocal("0-1");
                    {
                        bool isMin = methodName.StartsWith("min", StringComparison.OrdinalIgnoreCase);
                        var valueType = GetArgument(args, 0);
                        var extremum = ExtremumEvaluator.BuildEvaluator(valueType, isMin, expr);
                        return vd => extremum(vd.Values.Keys);
                    }

                case "min":  // dvar:dict.min(type = "n")  num = "n" / str = "s" / hex = "h"
                case "max":
                    CheckArgCountLocal("0-1");
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
