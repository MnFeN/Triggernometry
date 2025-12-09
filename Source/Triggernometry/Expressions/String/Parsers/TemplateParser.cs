using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Triggernometry.Core;

namespace Triggernometry.Expressions.String.Parsers
{
    internal static class TemplateParser
    {
        internal readonly struct TemplateMatch
        {
            /// <summary> Index of the first template char in the string. </summary>
            public readonly int Start;
            /// <summary> Index of the last template char in the string. </summary>
            public readonly int End;
            /// <summary> The expression "..." inside "${...}" </summary>
            public readonly string Expression;
            /// <summary> The group index n of $n. </summary>
            public readonly int? NumIndex;

            public TemplateMatch(int start, int end, string expression, int? numIndex)
            {
                Start = start;
                End = end;
                Expression = expression;
                NumIndex = numIndex;
            }
        }

        private static string EvaluateTemplateMatch(TemplateMatch m, Context ctx, bool isNumeric = false)
        {
            if (m.NumIndex == null) // "${...}"
                return EvaluateSingleTemplate(m.Expression ?? "", ctx, isNumeric);

            else // "$n"
                return ctx?.GetNumGroup(m.NumIndex.Value) ?? "";
        }

        /// <summary>
        /// Find all non-nested template expressions <c>$n</c> or <c>${...}</c> in the given string.
        /// </summary>
        internal static List<TemplateMatch> FindTemplates(string expr)
        {
            var result = new List<TemplateMatch>();
            if (string.IsNullOrEmpty(expr)) return result;

            int fullLength = expr.Length;

            for (int i = 0; i < fullLength - 1; i++)
            {
                // not '$'
                if (expr[i] != '$')
                    continue;

                var next = expr[i + 1];

                // "$n"
                if (next >= '0' && next <= '9')
                {
                    var start = i;
                    var end = i + 1;
                    var num = next - '0';
                    result.Add(new TemplateMatch(start, end, null, num));
                    i = end;
                }
                // "${"
                else if (next == '{')
                {
                    // scan for the next valid close '}'
                    int j = i + 2;
                    bool isValid = false;

                    // Find the next '}'
                    while (j < fullLength)
                    {
                        // valid close '}'
                        if (expr[j] == '}')
                        {
                            isValid = true;
                            break;
                        }
                        // inner start: ${...${...}...}
                        //                   ^
                        if (j < fullLength - 1 && expr[j] == '$' && expr[j + 1] == '{')
                        {
                            i = j - 1; // skip scanned part
                            break;
                        }
                        j++;
                    }
                    // invalid: continue to find the next '${'
                    if (!isValid) continue;

                    // "${...}"
                    var start = i;
                    var end = j;
                    // inner expression "..."
                    var innerExpr = expr.Substring(start + 2, end - start - 2);
                    result.Add(new TemplateMatch(start, end, innerExpr, null));
                    i = j;
                }
                // else: a single '$', continue;
            }
            return result;
        }

        internal static string ReplaceTemplates(string text, List<TemplateMatch> templates, Context ctx, bool isNumeric)
        {
            if (templates == null || templates.Count == 0)
                return text;

            var sb = new StringBuilder(text.Length);

            // avoid Substring
            void Append(ReadOnlySpan<char> span)
            {
                for (int i = 0; i < span.Length; i++)
                    sb.Append(span[i]);
            }

            int start = 0;

            for (int i = 0; i < templates.Count; i++)
            {
                var template = templates[i];

                // add original part before template
                Append(text.AsSpan(start, template.Start - start));

                // add evaluated template
                var result = EvaluateTemplateMatch(template, ctx, isNumeric);
                result = Utils.ParserCommon.ReplaceLineBreak(result);
                sb.Append(result); // 和前面可以合并一个函数减少扫描

                start = template.End + 1;
            }

            // add final part after last template
            Append(text.AsSpan(start, text.Length - start));

            return sb.ToString();
        }

        internal static string EvaluateSingleTemplate(string rawExpr, Context ctx, bool isTestModeNumeric)
        {
            ctx = ctx ?? Context.Unbound;
            var plug = ctx.Plugin;

            return KeywordParser.TryParse(rawExpr, ctx, isTestModeNumeric) // ${_xxx} single keyword
                ?? ColonExpressionParser.TryParse(rawExpr, ctx) // expressions start with "xxx:"
                ?? IndexPropExpressionParser.TryParse(rawExpr, ctx) // var, var.prop(arg), var[index], var[index1][index2].prop(arg), ...
                ?? "";
        }

        /// <summary>
        /// ¤1, ¤{...} => $1, ${...}
        /// </summary>
        internal static string ReplacePlaceholderTemplates(string expr)
        {
            var sb = new StringBuilder(expr.Length);
            int n = expr.Length;
            for (int i = 0; i < n; i++)
            {
                char c = expr[i];
                if (c == '¤' && i + 1 < n)
                {
                    char next = expr[i + 1];
                    if (char.IsDigit(next) || next == '¤' || next == '$' || next == '{')
                    {
                        sb.Append('$');
                        continue;
                    }
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

    }
}
