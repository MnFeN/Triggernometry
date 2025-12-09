using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Localization;
using static Triggernometry.Expressions.String.Utils.ErrorHelper;
using static Triggernometry.Expressions.String.Utils.ParserCommon;

namespace Triggernometry.Expressions.String.Evaluators
{
    internal static class ExtremumEvaluator
    {
        /// <summary>
        /// Build an extremum evaluator for a list of string values. <br />
        /// · <paramref name="valueType"/> (actually only check the first char): <br />
        /// -- n: numeric <br />
        /// -- s: string <br />
        /// -- h: hex numeric <br />
        /// -- Null/Empty: numeric as default.  <br />
        /// · <paramref name="expr"/>: provides method name, variable name, and raw expression text for error reporting.
        ///</summary>
        internal static Func<IEnumerable<string>, string> BuildEvaluator(string valueType, bool isMin, IndexMethodExpression expr)
        {
            // type: "n(umeric)" / "s(tring)" / "h(ex)"
            var typeChar = !string.IsNullOrEmpty(valueType) ? valueType[0] : 'n';
            switch (typeChar)
            {
                case 'n':
                    return source => SafeGetExtremum(source, ParseDouble, isMin, I18n.TranslateWord("double"), expr);
                case 'h':
                    return source => SafeGetExtremum(source, ParseHex, isMin, I18n.TranslateWord("hex"), expr);
                case 's':
                    return source => SafeGetExtremum(source, s => s, isMin, I18n.TranslateWord("string"), expr);
                default: throw InvalidValueError(expr.Method.Name, "type", valueType, expr.RawExpression);
            }
        }

        private static string SafeGetExtremum<T>(IEnumerable<string> source, Func<string, T> parser, bool isMin,
            string typeDescription, IndexMethodExpression expr) where T : IComparable<T>
        {
            var srcList = source as List<string> ?? source.ToList();
            if (source == null || srcList.Count == 0)
            {
                throw ExtremumListZeroElementError(expr.Name, expr.RawExpression);
            }
            try
            {
                return GetExtremum(srcList, parser, isMin);
            }
            catch
            {
                throw ExtremumParseTypeError(expr.Method.Name, typeDescription, expr.RawExpression);
            }
        }

        private static string GetExtremum<T>(List<string> source, Func<string, T> parser, bool isMin) where T : IComparable<T>
        {
            string extremum = source[0];
            T extremumValue = parser(source[0]);

            for (int i = 1; i < source.Count; i++)
            {
                string current = source[i];
                T currentValue = parser(current);

                if ((currentValue.CompareTo(extremumValue) > 0) ^ isMin)
                {
                    extremum = current;
                    extremumValue = currentValue;
                }
            }
            return extremum;
        }

        private static double ParseDouble(string value)
        {
            return double.Parse(value, NSFloat, InvClt);
        }

        private static long ParseHex(string value)
        {
            return long.Parse(value, NumberStyles.HexNumber, InvClt);
        }


    }
}
