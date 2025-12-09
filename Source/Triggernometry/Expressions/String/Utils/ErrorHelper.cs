using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triggernometry.Localization;

namespace Triggernometry.Expressions.String.Utils
{
    public static class ErrorHelper
    {
        public static ArgumentException ArgumentCountError(string functionName, string expectedCount, int givenCount, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/argumentCountError",
                "In ({0}), expected {1} arguments but got {2}. Expr: ({3})",
                functionName, expectedCount, givenCount, totalExpression));
        }

        public static ArgumentException InvalidValueError(string functionName, string exprDesc, string exprValue, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/invalidValueError",
                "In ({0}), {1} ({2}) is invalid. Expr: ({3})",
                functionName, exprDesc, exprValue, totalExpression));
        }

        public static ArgumentException ParseTypeError(string srcFormatDesc, string srcValue, string tgtFormatDesc, string fullExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/parseTypeError",
                "{0} ({1}) could not be parsed into {2}. Expr: ({3})",
                srcFormatDesc, srcValue, tgtFormatDesc, fullExpression));
        }

        internal static Exception InfiniteRepeatError(string newStr, string oldStr, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/infiniteRepeatError",
                "In the repeat function, new string ({0}) cannot contain old string ({1}) in loop mode. Expr: ({2})",
                newStr, oldStr, totalExpression));
        }

        internal static Exception ExtremumListZeroElementError(string varName, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/extremumlistzeroelementerror",
                    "The variable ({0}) selected zero elements to get its extremum value. Expr: ({1})",
                    varName, totalExpression));
        }

        internal static Exception ExtremumParseTypeError(string funcName, string parseFormat, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/extremumparsetypeerror",
                    "In the function ({0}), not all selected values could be parsed into {1}. Expr: ({2})",
                    funcName, parseFormat, totalExpression));
        }
    }
}
