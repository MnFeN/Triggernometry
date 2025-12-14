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
            /// <summary> The string ${...} or $n </summary>
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
            string result;
            if (m.NumIndex == null) // "${...}"
            {
                result = ParseSingleTemplate(m.Expression ?? "", ctx, isNumeric);
                if (result == null)
                {
                    var msg = $"文本模板 {m.RawExpression} 无法识别为有效表达式，且不存在此名称的正则捕获组。";
                    throw new FormatException(msg);
                }
            }
            else // "$n"
            {
                result = ctx?.GetNumGroup(m.NumIndex.Value);
                if (result == null && ctx?.Trigger != null)
                {
                    var msg = $"正则捕获组索引 {m.RawExpression} 超出范围。\n触发器：{ctx.Trigger?.LogName ?? "(null)"}";
                    ctx.Plugin?.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Warning, msg, ctx.Trigger);
                }
            }
            return result ?? string.Empty;
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
        private static void AddBraceTemplate(List<TemplateMatch> templateMatches, string fullExpr, int dollarPos, int rBracePos, out int newScanPos)
        {
            var length = rBracePos - dollarPos - 2;
            string innerExpr = fullExpr.Substring(dollarPos + 2, length); // without "${" / "}"
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
                sb.Append(result);

                start = template.End + 1;
            }

            // add final part after last template
            Append(text.AsSpan(start, text.Length - start));

            return sb.ToString();
        }

        /// <summary> Null if not matched. </summary>
        internal static string ParseSingleTemplate(string templateBody, Context ctx, bool isTestModeNumeric)
        {
            ctx = ctx ?? Context.Unbound;
            return KeywordParser.TryParse(templateBody, ctx, isTestModeNumeric) // ${_xxx} ${regexGroup} ${regexIdx}
                ?? ColonParser.TryParse(templateBody, ctx) // expressions start with "xxx:"
                ?? IndexMemberParser.TryParse(templateBody, ctx); // var, var.prop(arg), var[index], var[index1][index2].prop(arg), ...
        }

        /// <summary> Replace all valid currency-sign templates ¤n, ¤{...} to dollar-sign templates $n, ${...} in the given string. </summary>
        internal static string ReplaceCurrencyTemplates(string expr)
        {
            var sb = new StringBuilder(expr.Length);
            int n = expr.Length;
            for (int i = 0; i < n; i++)
            {
                // count all consecutive '¤'s and convert them together
                var currencyCount = CheckConsecutiveCurrencyCharCount(expr, i, out bool isValidCurrencyTemplate);
                if (currencyCount > 0)
                {
                    char c = isValidCurrencyTemplate ? '$' : '¤';
                    sb.Append(c, currencyCount);
                    i += currencyCount - 1;
                }
                else sb.Append(expr[i]);
            }
            return sb.ToString();
        }

        private static int CheckConsecutiveCurrencyCharCount(string expr, int idx, out bool isValidCurrencyTemplate)
        {
            isValidCurrencyTemplate = false;
            if (expr[idx] != '¤') return 0;

            // ¤{...} => ${...} valid; count = 1
            // ¤¤1 => $$1 valid; count = 2
            // ¤¤{var:regexIdx} => $${var:regexIdx} valid; count = 2
            // ¤¤rawText => ¤¤rawText invalid; count = 2

            // Note: ¤${template} is not a possible case,
            // because templates are all parsed before currency replacement,
            // so we treat '$' as normal char here.

            int count = 1;
            int pos = idx + 1;
            int length = expr.Length;
            while (pos < length && expr[pos] == '¤')
            {
                count++;
                pos++;
            }

            if (pos >= length) return count; // ...¤ till the end

            // check the char after consecutive '¤'s
            char next = expr[pos];
            isValidCurrencyTemplate = char.IsDigit(next) || next == '{';
            return count;
        }

    }
}
