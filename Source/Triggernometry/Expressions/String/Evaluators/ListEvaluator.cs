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
        internal static Func<VariableList, string> BuildEvaluator(IndexMethodExpression expr)
        {
            // invalid (only name)
            if (expr.Indexes.Length == 0 && !expr.Method.HasValue)
                throw new NotImplementedException("list");

            // lvar:Name[Index]
            if (expr.Indexes.Length > 0)
            {
                var idx = expr.Index.Equals("last", StringComparison.OrdinalIgnoreCase) ? -1 : (int)MathParser.Parse(expr.Index);
                return vl => vl.Peek(idx).ToString();
            }

            // lvar:Name.Method(Args)
            var methodName = expr.Method.Name.ToLowerInvariant();
            var args = expr.Method.Args;
            int argCount = args.Length;
            switch (methodName)
            {
                case "size":
                case "length":
                    return vl => vl.Size.ToString();

                case "indexof":
                case "i":
                    CheckArgCount("1", argCount, methodName, expr.RawExpression);
                    return vl => vl.IndexOf(args[0]).ToString();

                case "lastindexof":
                    CheckArgCount("1", argCount, methodName, expr.RawExpression);
                    return vl => vl.LastIndexOf(args[0]).ToString();

                case "indicesof":
                    CheckArgCount("1-3", argCount, methodName, expr.RawExpression);
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
                    CheckArgCount("0-1", argCount, methodName, expr.RawExpression);
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
                    CheckArgCount("0-2", argCount, methodName, expr.RawExpression);
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
                    CheckArgCount("0-2", argCount, methodName, expr.RawExpression);
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
                    CheckArgCount("1-2", argCount, methodName, expr.RawExpression);
                    {
                        string slicesStr = GetArgument(args, 1, ":");
                        return vl =>
                        {
                            var indices = GetSliceIndices(slicesStr, vl.Size, expr.RawExpression, startIndex: 1);
                            return vl.Count(args[0], indices).ToString();
                        };
                    }

                case "contain":
                    CheckArgCount("1-2", argCount, methodName, expr.RawExpression);
                    {
                        string slicesStr = GetArgument(args, 1, ":");
                        return vl =>
                        {
                            var indices = GetSliceIndices(slicesStr, vl.Size, expr.RawExpression, startIndex: 1);
                            return indices.Any(idx => vl.Values[idx].ToString() == args[0]) ? "1" : "0";
                        };
                    }

                case "ifcontain":
                    CheckArgCount("3", argCount, methodName, expr.RawExpression);
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
