using System;
using Triggernometry.Localization;

namespace Triggernometry.Expressions.String.Utils
{
    public static class ErrorHelper // todo I18n cleanup
    {
        public static ArgumentException ArgumentCountError(string functionName, string expectedCount, int givenCount)
        {
            return new ArgumentException(I18n.Translate("internal/Context/argumentCountError-",
                "In ({0}), expected {1} arguments but got {2}.",
                functionName, expectedCount, givenCount));
        }

        public static ArgumentException ArgumentCountError(string functionName, string expectedCount, int givenCount, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/argumentCountError",
                "In ({0}), expected {1} arguments but got {2}. Expr: ({3})",
                functionName, expectedCount, givenCount, totalExpression));
        }

        public static ArgumentException InvalidValueError(string functionName, string exprDesc, string exprValue)
        {
            return new ArgumentException(I18n.Translate("internal/Context/invalidValueError-",
                "In ({0}), {1} ({2}) is invalid.",
                functionName, exprDesc, exprValue));
        }

        public static ArgumentException InvalidValueError(string functionName, string exprDesc, string exprValue, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/invalidValueError",
                "In ({0}), {1} ({2}) is invalid. Expr: ({3})",
                functionName, exprDesc, exprValue, totalExpression));
        }

        public static ArgumentException ParseTypeError(string srcFormatDesc, string srcValue, string tgtFormatDesc)
        {
            return new ArgumentException(I18n.Translate("internal/Context/parseTypeError-",
                "{0} ({1}) could not be parsed into {2}.",
                srcFormatDesc, srcValue, tgtFormatDesc));
        }

        public static ArgumentException ParseTypeError(string srcFormatDesc, string srcValue, string tgtFormatDesc, string fullExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/parseTypeError",
                "{0} ({1}) could not be parsed into {2}. Expr: ({3})",
                srcFormatDesc, srcValue, tgtFormatDesc, fullExpression));
        }

        internal static Exception InfiniteRepeatError(string newStr, string oldStr)
        {
            return new ArgumentException(I18n.Translate("internal/Context/infiniteRepeatError-",
                "In the repeat function, new string ({0}) cannot contain old string ({1}) in loop mode.",
                newStr, oldStr));
        }

        internal static Exception InfiniteRepeatError(string newStr, string oldStr, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/infiniteRepeatError",
                "In the repeat function, new string ({0}) cannot contain old string ({1}) in loop mode. Expr: ({2})",
                newStr, oldStr, totalExpression));
        }

        internal static Exception ExtremumListZeroElementError(string varName)
        {
            return new ArgumentException(I18n.Translate("internal/Context/extremumlistzeroelementerror-",
                    "The variable ({0}) selected zero elements to get its extremum value.",
                    varName));
        }

        internal static Exception ExtremumListZeroElementError(string varName, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/extremumlistzeroelementerror",
                    "The variable ({0}) selected zero elements to get its extremum value. Expr: ({1})",
                    varName, totalExpression));
        }

        internal static Exception ExtremumParseTypeError(string funcName, string parseFormat)
        {
            return new ArgumentException(I18n.Translate("internal/Context/extremumparsetypeerror-",
                    "In the function ({0}), not all selected values could be parsed into {1}.",
                    funcName, parseFormat));
        }

        internal static Exception ExtremumParseTypeError(string funcName, string parseFormat, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/extremumparsetypeerror",
                    "In the function ({0}), not all selected values could be parsed into {1}. Expr: ({2})",
                    funcName, parseFormat, totalExpression));
        }
    }
}
