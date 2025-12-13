using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Triggernometry.Localization;
using static Triggernometry.Expressions.String.Utils.ParserCommon;

namespace Triggernometry.Expressions.String.Utils
{
    /// <summary>
    /// Helper functions for argument parsing and validation from user-input strings.
    /// </summary>
    public static class ArgHelper
    {
        private static readonly object _lockValidators = new object();

        private static readonly Dictionary<string, Func<int, bool>> _validators
            = new Dictionary<string, Func<int, bool>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Validates that the actual argument count meets the required rule. <br /><br />
        /// <paramref name="expectedCount"/> supports the following formats: <br />
        /// · Exact value ("2") <br />
        /// · Range ("1-3") <br />
        /// · Minimum (">= 2") <br />
        /// · Arithmetic progression ("1, 3, 5, ...") <br />
        /// · Their combinations ("1, 3-5, >=7") <br /><br />
        /// Throws <see cref="ArgumentException" /> if not valid.
        /// </summary>
        public static void CheckArgCount(string expectedCount, int givenCount, string funcName,
            string rawExpression = null, Func<int, bool> validatorOverride = null)
        {
            var validator = validatorOverride ?? GetArgCountValidator(expectedCount);
            if (!validator(givenCount))
                throw ErrorHelper.ArgumentCountError(funcName, expectedCount, givenCount, rawExpression);
        }

        /// <summary> Get or build the argument count validator function from the required count expression. </summary> 
        private static Func<int, bool> GetArgCountValidator(string requiredCount)
        {
            Func<int, bool> validator;
            lock (_lockValidators)
            {
                if (_validators.TryGetValue(requiredCount, out validator))
                    return validator;
            }
            // "0, 1, 3-5, >=7"  "1, 2, 4, 6, 8, 10, ..."
            var parts = requiredCount.Replace(" ", "").Split(',').ToList();
            List<Func<int, bool>> checks = new List<Func<int, bool>>();
            int? last1 = null;
            int? last2 = null;
            foreach (var part in parts)
            {
                // "1"
                if (part.All(c => char.IsDigit(c)))
                {
                    var exact = int.Parse(part);
                    checks.Add(i => i == exact);
                    last2 = last1;
                    last1 = exact;
                }
                // "3-5"
                else if (part.Contains('-'))
                {
                    var rangeParts = part.Split('-');
                    var min = int.Parse(rangeParts[0]);
                    var max = int.Parse(rangeParts[1]);
                    checks.Add(i => i >= min && i <= max);
                    last1 = null;
                }
                // ">=7"
                else if (part.StartsWith(">="))
                {
                    var min = int.Parse(part.Substring(2));
                    checks.Add(i => i >= min);
                    last1 = null;
                }
                // "..."
                else if (part == "..." || part == "…")
                {
                    var d = last1 - last2
                        ?? throw new ArgumentException($"Invalid argument count expression: '...' \nIn:{requiredCount}");
                    if (d <= 0)
                        throw new ArgumentException($"Invalid argument count expression: {last2}, {last1}, ... \nIn:{requiredCount}");
                    checks.Add(i => i > last1 && (i - last1) % d == 0);
                    last1 = null;
                }
                else
                    throw new ArgumentException($"Invalid argument count expression: {part} \nIn:{requiredCount}");
            }
            validator = i => checks.Any(f => f(i));
            lock (_lockValidators)
            {
                _validators[requiredCount] = validator;
            }
            return validator;
        }

        internal static Regex BuildSplitArgRegex(string separator, bool isCompiled = false)
        {
            string s = Regex.Escape(separator);
            char lb = LINEBREAK;
            return new Regex(
            //   (?<=^|{s}): after a separator or start-of-line
            //               (?:\\s|{lb})*\"[^\"]*\"(?:\\s|{lb})*: spaces? + " + text? + " + spaces?
            //                                                    (?:\\s|{lb})*'[^']*'(?:\\s|{lb})*: spaces? + ' + text? + ' + spaces?
            //                                                                                      [^{s}]*: any unquoted text
            //                                                                                              (?=$|{s}): before a separator or end-of-line
                $"(?<=^|{s})((?:\\s|{lb})*\"[^\"]*\"(?:\\s|{lb})*|(?:\\s|{lb})*'[^']*'(?:\\s|{lb})*|[^{s}]*)(?=$|{s})",
                isCompiled ? RegexOptions.Compiled : RegexOptions.None
            );
        }

        internal static Regex reSplitArgComma = BuildSplitArgRegex(",", true);
        internal static Regex reSplitArgEqual = BuildSplitArgRegex("=", true);
        internal static Regex reSplitArgPipe = BuildSplitArgRegex("|", true);

        /// <summary> 
        /// Split an expression with commas or other specified separators to a list of arguments. <br />
        /// If the argument contains separator, single/double quotes, it should be quoted like "xx,xx", 'xx,xx', 'xx"xx', "xx'xx".
        /// </summary>
        /// <param name="allowEmptyList"> Converts an empty string or spaces to an empty list, or a list with an empty string.</param>>
        public static string[] SplitArguments(string args, bool allowEmptyList = true, string separator = ",")
        {
            if (allowEmptyList && args.TrimEx() == "")
            {
                return Array.Empty<string>();
            }
            var reSingleArg = separator == "," ? reSplitArgComma
                            : separator == "=" ? reSplitArgEqual
                            : separator == "|" ? reSplitArgPipe
                            : BuildSplitArgRegex(separator);

            args = UnescapeCustomExpr(args);
            var matches = reSingleArg.Matches(args);
            var result = new List<string>();
            foreach (Match match in matches)
            {
                string currentMatch = match.Value.TrimEx();
                int length = currentMatch.Length;
                if (length >= 2
                    && currentMatch[0] == currentMatch[length - 1]
                    && (currentMatch[0] == '\"' || currentMatch[0] == '\'')  // quoted string: "..." '...'
                    )
                {
                    currentMatch = currentMatch.Substring(1, length - 2); // remove " / '
                }
                result.Add(currentMatch);
            }
            return result.ToArray();
        }

        /// <summary> 
        /// Pick the argument with the given index from a <see cref="string[]" />. <br />
        /// · <paramref name="args"/>: The source <see cref="string[]" />. <br />
        /// · <paramref name="index"/>: The selected index (0-based). <br />
        /// · <paramref name="defaultValue"/>: If the index is out of range, returns this value. <br />
        /// · <paramref name="setEmptyToDefault"/>: If the selected argument is empty, consider it as the default value or not.
        /// </summary>
        public static string GetArgument(string[] args, int index, string defaultValue = null, bool setEmptyToDefault = false)
        {
            if (index >= args.Length || args[index] == "" && setEmptyToDefault)
                return defaultValue;
            else
                return args[index];
        }

        /// <summary> Parse the slices expression to a list of indices (starts from 0). </summary>
        /// <param name="slicesStr"> String expression of slices.</param>
        /// <param name="totalLength"> The length of the string, list, or table row/column.</param>
        /// <param name="rawExpr"> The raw expression before parsed. Used in error messages.</param>
        /// <param name="startIndex"> The start index in the given expression. 0 for strings; 1 for other variables. (Triggernometry definition)</param>
        /// <returns>A list of selected indices.</returns>
        public static List<int> GetSliceIndices(string slicesStr, int totalLength, string rawExpr, int startIndex)
        {
            if (totalLength <= 0) { return new List<int>(); }

            // optimize for default input
            string checkDefault = slicesStr.Replace(" ", "");
            if (checkDefault == "" || checkDefault == ":" || checkDefault == "::")
            {
                return Enumerable.Range(0, totalLength).ToList();
            }

            List<int> indices = new List<int>();
            string[] slices = SplitArguments(slicesStr, allowEmptyList: false);
            foreach (string slice in slices)
            {   // parse slice string to int start/end/step
                string[] sliceArgs = slice.Split(':').Select(s => s.TrimEx()).ToArray();
                CheckArgCount("0-3", sliceArgs.Length, I18n.TranslateWord("slice"));

                string startStr = GetArgument(sliceArgs, 0, "", true);
                string endStr = GetArgument(sliceArgs, 1, "", true);
                string stepStr = GetArgument(sliceArgs, 2, "1", true);
                int start = 0; int end = 0; int step = 0;
                try
                {
                    if (sliceArgs.Length == 1 && startStr.TrimEx() != "")
                    {   // sliceArgs.Length = 1: push a single index
                        start = int.Parse(startStr, InvClt);
                        start = start >= 0 ? start - startIndex : start + totalLength;
                        if (start >= 0 && start < totalLength)
                        {
                            indices.Add(start);
                        }
                        continue;
                    }
                    else
                    {   // sliceArgs.Length = 3:  a:b:c, a:b:, a::c, :b:c, a::, :b:, ::c, ::
                        // sliceArgs.Length = 2:  a:b, a:, :b, :
                        // sliceArgs.Length = 0:  "" (= ":")
                        step = int.Parse(stepStr, InvClt);
                        if (step == 0) { throw ErrorHelper.InvalidValueError(I18n.TranslateWord("slice"), "step", "0"); }
                        if (startStr != "")
                        {   // `start` value given: apply the negative-index and startIndex logics
                            start = int.Parse(startStr, InvClt);
                            start = start >= 0 ? start - startIndex : start + totalLength;
                        }
                        else
                        {   // `start` value not given: set init value based on the sign of `step`
                            start = step > 0 ? int.MinValue : int.MaxValue;
                        }
                        if (endStr != "") // logic similar to `start`
                        {
                            end = int.Parse(endStr, InvClt);
                            end = end >= 0 ? end - startIndex : end + totalLength;
                        }
                        else
                        {
                            end = step > 0 ? int.MaxValue : int.MinValue;
                        }
                    }
                }
                catch 
                { 
                    throw ErrorHelper.ParseTypeError(I18n.TranslateWord("string"), slice, I18n.TranslateWord("slice")); 
                }

                // fix the out-of-range early start value / late end value
                if (step > 0)
                {
                    start = start < 0 ? 0 : start;
                    end = end > totalLength ? totalLength : end;
                }
                else
                {
                    start = start > totalLength - 1 ? totalLength - 1 : start;
                    end = end < -1 ? -1 : end;
                }

                // get indices
                int sign = Math.Sign(step);
                int index = start;
                while (sign * index >= sign * start && sign * index < sign * end)
                {
                    indices.Add(index);
                    index += step;
                }
            }
            return indices;
        }

        /// <summary>
        /// Parse a raw argument string into several typed values.<br />
        /// · <paramref name="rawData" />: Source text containing the raw arguments to split and convert.<br />
        /// · <typeparamref name="T1" />...: Target types for arguments.<br />
        /// · <paramref name="defaults" />: Optional (index, value) pairs used when a given argument is missing.
        /// </summary>
        public static (T1, T2) ParseArgs<T1, T2>(this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults),
                GetOrDefault<T2>(rawArgs, 1, defaults)
            );
        }

        /// <summary>
        /// Parse a raw argument string into several typed values.<br />
        /// · <paramref name="rawData" />: Source text containing the raw arguments to split and convert.<br />
        /// · <typeparamref name="T1" />...: Target types for arguments.<br />
        /// · <paramref name="defaults" />: Optional (index, value) pairs used when a given argument is missing.
        /// </summary>
        public static (T1, T2, T3) ParseArgs<T1, T2, T3>(this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults),
                GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults)
            );
        }

        /// <summary>
        /// Parse a raw argument string into several typed values.<br />
        /// · <paramref name="rawData" />: Source text containing the raw arguments to split and convert.<br />
        /// · <typeparamref name="T1" />...: Target types for arguments.<br />
        /// · <paramref name="defaults" />: Optional (index, value) pairs used when a given argument is missing.
        /// </summary>
        public static (T1, T2, T3, T4) ParseArgs<T1, T2, T3, T4>(this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5) ParseArgs<T1, T2, T3, T4, T5>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5, T6) ParseArgs<T1, T2, T3, T4, T5, T6>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults), GetOrDefault<T6>(rawArgs, 5, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5, T6, T7) ParseArgs<T1, T2, T3, T4, T5, T6, T7>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults), GetOrDefault<T6>(rawArgs, 5, defaults),
                GetOrDefault<T7>(rawArgs, 6, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5, T6, T7, T8) ParseArgs<T1, T2, T3, T4, T5, T6, T7, T8>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults), GetOrDefault<T6>(rawArgs, 5, defaults),
                GetOrDefault<T7>(rawArgs, 6, defaults), GetOrDefault<T8>(rawArgs, 7, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) ParseArgs
            <T1, T2, T3, T4, T5, T6, T7, T8, T9>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults), GetOrDefault<T6>(rawArgs, 5, defaults),
                GetOrDefault<T7>(rawArgs, 6, defaults), GetOrDefault<T8>(rawArgs, 7, defaults),
                GetOrDefault<T9>(rawArgs, 8, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) ParseArgs
            <T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults), GetOrDefault<T6>(rawArgs, 5, defaults),
                GetOrDefault<T7>(rawArgs, 6, defaults), GetOrDefault<T8>(rawArgs, 7, defaults),
                GetOrDefault<T9>(rawArgs, 8, defaults), GetOrDefault<T10>(rawArgs, 9, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) ParseArgs
            <T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults), GetOrDefault<T6>(rawArgs, 5, defaults),
                GetOrDefault<T7>(rawArgs, 6, defaults), GetOrDefault<T8>(rawArgs, 7, defaults),
                GetOrDefault<T9>(rawArgs, 8, defaults), GetOrDefault<T10>(rawArgs, 9, defaults),
                GetOrDefault<T11>(rawArgs, 10, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) ParseArgs
            <T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults), GetOrDefault<T6>(rawArgs, 5, defaults),
                GetOrDefault<T7>(rawArgs, 6, defaults), GetOrDefault<T8>(rawArgs, 7, defaults),
                GetOrDefault<T9>(rawArgs, 8, defaults), GetOrDefault<T10>(rawArgs, 9, defaults),
                GetOrDefault<T11>(rawArgs, 10, defaults), GetOrDefault<T12>(rawArgs, 11, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) ParseArgs
            <T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults), GetOrDefault<T6>(rawArgs, 5, defaults),
                GetOrDefault<T7>(rawArgs, 6, defaults), GetOrDefault<T8>(rawArgs, 7, defaults),
                GetOrDefault<T9>(rawArgs, 8, defaults), GetOrDefault<T10>(rawArgs, 9, defaults),
                GetOrDefault<T11>(rawArgs, 10, defaults), GetOrDefault<T12>(rawArgs, 11, defaults),
                GetOrDefault<T13>(rawArgs, 12, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) ParseArgs
            <T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults), GetOrDefault<T6>(rawArgs, 5, defaults),
                GetOrDefault<T7>(rawArgs, 6, defaults), GetOrDefault<T8>(rawArgs, 7, defaults),
                GetOrDefault<T9>(rawArgs, 8, defaults), GetOrDefault<T10>(rawArgs, 9, defaults),
                GetOrDefault<T11>(rawArgs, 10, defaults), GetOrDefault<T12>(rawArgs, 11, defaults),
                GetOrDefault<T13>(rawArgs, 12, defaults), GetOrDefault<T14>(rawArgs, 13, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) ParseArgs
            <T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults), GetOrDefault<T6>(rawArgs, 5, defaults),
                GetOrDefault<T7>(rawArgs, 6, defaults), GetOrDefault<T8>(rawArgs, 7, defaults),
                GetOrDefault<T9>(rawArgs, 8, defaults), GetOrDefault<T10>(rawArgs, 9, defaults),
                GetOrDefault<T11>(rawArgs, 10, defaults), GetOrDefault<T12>(rawArgs, 11, defaults),
                GetOrDefault<T13>(rawArgs, 12, defaults), GetOrDefault<T14>(rawArgs, 13, defaults),
                GetOrDefault<T15>(rawArgs, 14, defaults)
            );
        }

        /// <summary> Parse a raw argument string into several typed values. <br />Check <see cref="ParseArgs"/> for details. </summary>
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) ParseArgs
            <T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
            (this string rawData, params (int idx, object val)[] defaults)
        {
            var rawArgs = SplitArguments(rawData);
            return (
                GetOrDefault<T1>(rawArgs, 0, defaults), GetOrDefault<T2>(rawArgs, 1, defaults),
                GetOrDefault<T3>(rawArgs, 2, defaults), GetOrDefault<T4>(rawArgs, 3, defaults),
                GetOrDefault<T5>(rawArgs, 4, defaults), GetOrDefault<T6>(rawArgs, 5, defaults),
                GetOrDefault<T7>(rawArgs, 6, defaults), GetOrDefault<T8>(rawArgs, 7, defaults),
                GetOrDefault<T9>(rawArgs, 8, defaults), GetOrDefault<T10>(rawArgs, 9, defaults),
                GetOrDefault<T11>(rawArgs, 10, defaults), GetOrDefault<T12>(rawArgs, 11, defaults),
                GetOrDefault<T13>(rawArgs, 12, defaults), GetOrDefault<T14>(rawArgs, 13, defaults),
                GetOrDefault<T15>(rawArgs, 14, defaults), GetOrDefault<T16>(rawArgs, 15, defaults)
            );
        }

        private static T GetOrDefault<T>(string[] rawArgs, int index, (int idx, object val)[] defaults)
        {
            if (index >= 0 && index < rawArgs.Length)
            {
                return rawArgs[index].ParseData<T>();
            }
            for (int i = 0; i < defaults.Length; i++)
            {
                if (defaults[i].idx != index) continue;
                var value = defaults[i].val;
                if (value == null)
                {
                    if (!typeof(T).IsValueType || Nullable.GetUnderlyingType(typeof(T)) != null)
                        return default;
                    throw new ArgumentException($"默认参数 #{index} 的类型 {typeof(T).Name} 不可为 null。");
                }
                try
                {
                    return (T)value;
                }
                catch { }
                // int 0 => uint/byte
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
                }
                catch { }
                // fallback
                return value.ToDataString().ParseData<T>();
            }
            throw new ArgumentException($"缺少 {typeof(T).Name} 参数 #{index} 且未提供默认值。提供的参数：{string.Join(", ", rawArgs)}");
        }

    }
}
