using System;
using System.Linq;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Localization;
using static Triggernometry.Expressions.String.Utils.ArgHelper;
using static Triggernometry.Expressions.String.Utils.ParserCommon;

namespace Triggernometry.Expressions.String.Evaluators
{
    internal static class ListEvaluator
    {
        internal static Func<VariableList, string> BuildEvaluator(IndexMemberExpression expr)
        {
            // invalid (only name)
            if (expr.Indexes.Length == 0 && !expr.Member.HasValue)
                throw new Exception($"Listt variable must include an index or property in expression '{expr.RawExpression}'.");

            // lvar:Name[Index]
            if (expr.Indexes.Length > 0)
            {
                var idx = expr.Index.Equals("last", StringComparison.OrdinalIgnoreCase) ? -1 : (int)MathParser.Parse(expr.Index);
                return vl => vl.Peek(idx).ToString();
            }

            // lvar:Name.Method(Args)
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
                    return vl => vl.Size.ToString();

                case "indexof":
                case "i":
                    CheckArgCountLocal("1");
                    return vl => vl.IndexOf(args[0]).ToString();

                case "lastindexof":
                    CheckArgCountLocal("1");
                    return vl => vl.LastIndexOf(args[0]).ToString();

                case "indicesof":
                    CheckArgCountLocal("1-3");
                    {
                        string joiner = GetArgument(args, 1, ",");
                        string slicesStr = GetArgument(args, 2, ":");
                        return vl =>
                        {
                            var indices = GetSliceIndices(slicesStr, vl.Size, expr.RawExpression, startIndex: 1);
                            return vl.IndicesOf(args[0], joiner, indices);
                        };
                    }

                case "sum":    // lvar:list.sum(slices = ":")
                    CheckArgCountLocal("0-1");
                    {
                        string slicesStr = GetArgument(args, 0, ":");
                        return vl =>
                        {
                            var indices = GetSliceIndices(slicesStr, vl.Size, expr.RawExpression, startIndex: 1);
                            return I18n.ThingToString(vl.Sum(indices));
                        };
                    }

                case "min":    // lvar:list.min(type = "n", slices = ":")  num = "n" / str = "s" / hex = "h"
                case "max":
                    CheckArgCountLocal("0-2");
                    {
                        bool isMin = methodName.StartsWith("min", StringComparison.OrdinalIgnoreCase);
                        var valueType = GetArgument(args, 0);
                        string slicesStr = GetArgument(args, 1, ":");
                        var extremum = ExtremumEvaluator.BuildEvaluator(valueType, isMin, expr);
                        return vl =>
                        {
                            var indices = GetSliceIndices(slicesStr, vl.Size, expr.RawExpression, startIndex: 1);
                            var strings = indices.Select(i => vl.Values[i].ToString());
                            return extremum(strings);
                        };
                    }

                case "join":        // lvar:list.join(joiner = ",", slices = ":")
                case "randjoin":    // lvar:list.randjoin(joiner = ",", slices = ":")
                    CheckArgCountLocal("0-2");
                    {
                        string joiner = GetArgument(args, 0, ",");
                        string slicesStr = GetArgument(args, 1, ":");

                        return vl =>
                        {
                            var indices = GetSliceIndices(slicesStr, vl.Size, expr.RawExpression, startIndex: 1);

                            if (methodName == "randjoin")
                                indices = indices.OrderBy(_ => rng.Next()).ToList();

                            return vl.Join(joiner, indices);
                        };
                    }

                case "count":    // count(targetStr, slices = ":")
                    CheckArgCountLocal("1-2");
                    {
                        string slicesStr = GetArgument(args, 1, ":");
                        return vl =>
                        {
                            var indices = GetSliceIndices(slicesStr, vl.Size, expr.RawExpression, startIndex: 1);
                            return vl.Count(args[0], indices).ToString();
                        };
                    }

                case "contain":
                    CheckArgCountLocal("1-2");
                    {
                        string slicesStr = GetArgument(args, 1, ":");
                        return vl =>
                        {
                            var indices = GetSliceIndices(slicesStr, vl.Size, expr.RawExpression, startIndex: 1);
                            return indices.Any(idx => vl.Values[idx].ToString() == args[0]) ? "1" : "0";
                        };
                    }

                case "ifcontain":
                    CheckArgCountLocal("3");
                    {
                        string target = args[0];
                        string trueStr = args[1];
                        string falseStr = args[2];

                        return vl => vl.Values.Any(v => v.ToString() == target) ? trueStr : falseStr;
                    }
                default:
                    throw new Exception($"Unknown list method '{methodName}' in expression '{expr.RawExpression}'.");
            }
        }

    }
}
