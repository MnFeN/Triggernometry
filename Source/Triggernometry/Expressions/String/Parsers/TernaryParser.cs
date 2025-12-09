using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;

namespace Triggernometry.Expressions.String.Parsers
{
    internal static class TernaryParser
    {

        internal static string Parse(string ternaryExpr)
        {
            ParseTernaryExpression(ternaryExpr, out string condExpr, out string trueStr, out string falseStr);
            if (trueStr == null || falseStr == null)
            {
                throw new Exception(I18n.Translate("internal/Context/ternaryexpressionerror",
                    "Ternary expression ({0}) could not be parsed: \r\nCondition: ({1}); \r\nTrueExpr: ({2}); \r\nFalseExpr: ({3})",
                    ternaryExpr, condExpr, trueStr ?? "null", falseStr ?? "null"));
            }
            bool cond = !MathParser.IsZero(MathParser.Parse(condExpr));
            return cond ? trueStr : falseStr;
        }

        private static void ParseTernaryExpression(string input, out string condExpr, out string trueStr, out string falseStr)
        {
            falseStr = ExtractLastExpression(ref input, ':');
            trueStr = ExtractLastExpression(ref input, '?');
            condExpr = input.TrimEx();
        }

        private static string ExtractLastExpression(ref string input, char sep)
        {
            input = input.TrimRightEx();
            int lastIndex = input.Length - 1;
            char lastChar = input[lastIndex];

            int? sepIndex = null;
            if (lastChar == '\'' || lastChar == '\"')
            {
                int quoteIndex = input.LastIndexOf(lastChar, lastIndex - 1);
                if (quoteIndex != -1)
                {
                    sepIndex = input.LastIndexOf(sep, quoteIndex - 1);
                }
            }
            sepIndex = sepIndex ?? input.LastIndexOf(sep);
            if (sepIndex == -1) return null;

            string afterSep = input.Substring(sepIndex.Value + 1).TrimLeftEx();
            int length = afterSep.Length;
            if (length >= 2 && afterSep[0] == afterSep[length - 1] && (afterSep[0] == '\"' || afterSep[0] == '\''))
            {
                afterSep = afterSep.Substring(1, length - 2);  // "..." / '...' => ...
            }

            input = input.Substring(0, sepIndex.Value);
            return afterSep;
        }
    }
}
