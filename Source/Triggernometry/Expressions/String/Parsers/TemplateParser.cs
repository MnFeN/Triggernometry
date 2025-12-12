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
            public string RawExpression => NumIndex != null ? $"${NumIndex}" : $"${{{Expression}}}";

            public TemplateMatch(int start, int end, string expression, int? numIndex)
            {
                Start = start;
                End = end;
                Expression = expression;
                NumIndex = numIndex;
            }
        }

        private static string ParseTemplateMatch(TemplateMatch m, Context ctx, bool isNumeric = false)
        {
            if (m.NumIndex == null) // "${...}"
                return ParseSingleTemplate(m.Expression ?? "", ctx, isNumeric);

            else // "$n"
                return ctx?.GetNumGroup(m.NumIndex.Value) ?? "";
        }

        /// <summary>
        /// Find all non-nested template expressions <c>$n</c> or <c>${...}</c> in the given string.
        /// </summary>
        internal static List<TemplateMatch> FindNonNestedTemplates(string expr)
        {
            var results = new List<TemplateMatch>();
            if (string.IsNullOrEmpty(expr)) return results;

            int fullLength = expr.Length;

            for (int i = 0; i < fullLength - 1; i++)
            {
                // not '$'
                if (expr[i] != '$') continue;

                var next = expr[i + 1];

                // "$n"
                if (next >= '0' && next <= '9')
                {
                    AddNumGroupTemplate(results, next, i, out i);
                    continue;
                }
                
                // a single $
                if (next != '{') continue;

                // "${": scan for the next valid close '}'
                int validRightBracePos = -1;
                for (int j = i + 2; j < fullLength; j++)
                {
                    // found valid close '}'
                    if (expr[j] == '}')
                    {
                        validRightBracePos = j;
                        break;
                    }
                    
                    bool notInnerDollar = j >= fullLength - 1 || expr[j] != '$';
                    if (notInnerDollar) continue;

                    // nested: ${...${...}...} or ${...$n...}
                    //         i    j             i    j
                    var charAfterDollar = expr[j + 1];
                    if (charAfterDollar == '{') // inner ${
                    {
                        i = j - 1; // skip scanned part
                        break;
                    }
                    else if (charAfterDollar >= '0' && charAfterDollar <= '9') // inner $n
                    {
                        AddNumGroupTemplate(results, charAfterDollar, j, out i);
                        break;
                    }
                }

                // found valid "${...}"
                if (validRightBracePos != -1)
                    AddBraceTemplate(results, expr, i, validRightBracePos, out i);
            }
            return results;
        }

        /// <summary>
        /// Add a "$n" template match to the result list. <br />
        /// </summary>
        private static void AddNumGroupTemplate(List<TemplateMatch> templateMatches, char digitChar, int dollarPos, out int newScanPos)
        {
            int numGroupIdx = digitChar - '0';
            templateMatches.Add(new TemplateMatch(dollarPos, dollarPos + 1, null, numGroupIdx));
            newScanPos = dollarPos + 1;
        }

        /// <summary>
        /// Add a "${...}" template match to the result list. <br />
        /// </summary>
        private static void AddBraceTemplate(List<TemplateMatch> templateMatches, string expr, int dollarPos, int rBracePos, out int newScanPos)
        {
            var length = rBracePos - dollarPos - 2;
            string innerExpr = expr.Substring(dollarPos + 2, length); // without "${" / "}"
            templateMatches.Add(new TemplateMatch(dollarPos, rBracePos, innerExpr, null));
            newScanPos = rBracePos;
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
                var result = ParseTemplateMatch(template, ctx, isNumeric);
                result = Utils.ParserCommon.ReplaceLineBreak(result);
                sb.Append(result); // 和前面可以合并一个函数减少扫描

                start = template.End + 1;
            }

            // add final part after last template
            Append(text.AsSpan(start, text.Length - start));

            return sb.ToString();
        }

        internal static string ParseSingleTemplate(string rawExpr, Context ctx, bool isTestModeNumeric)
        {
            ctx = ctx ?? Context.Unbound;

            return KeywordParser.TryParse(rawExpr, ctx, isTestModeNumeric) // ${_xxx} single keyword
                ?? ColonParser.TryParse(rawExpr, ctx) // expressions start with "xxx:"
                ?? IndexPropParser.TryParse(rawExpr, ctx) // var, var.prop(arg), var[index], var[index1][index2].prop(arg), ...
                ?? throw new Exception($"模板字符串 {rawExpr} 无法识别为有效表达式，且不存在此名称的正则捕获组。");
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
