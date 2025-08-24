using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Triggernometry.FFXIV;
using Triggernometry.PluginBridges;
using Triggernometry.Utilities;
using Triggernometry.Variables;
using System.Threading;
/*
namespace Triggernometry.NewContext
{
    public partial class NewContext
    {

        internal Guid id = Guid.NewGuid();
        internal bool testByPlaceholder;
        internal RealPlugin plug;
        public Trigger trig { get; set; }
        internal Action.TriggerForceTypeEnum force;

        internal RealPlugin.ActionExecutionHook soundhook;
        internal RealPlugin.ActionExecutionHook ttshook;

        /// <summary> Regex matching: ${id} </summary>
        internal static Regex rex
            = new Regex(@"\$\{(?<id>[^${}]*)\}", RegexOptions.Compiled);
        /// <summary> Regex matching: ¤{id} </summary>
        internal static Regex rox
            = new Regex(@"¤\{[^${}]*\}", RegexOptions.Compiled);
        /// <summary> Regex matching: $num (e.g. $1, $20) </summary>
        internal static Regex rexNum
            = new Regex(@"\$(?<id>[0-9]+)", RegexOptions.Compiled);
        /// <summary> Regex matching: name[index] </summary>
        internal static Regex rexListIdx
            = new Regex(@"^(?<name>[^[]+)\[(?<index>[^[\]]*?)\]", RegexOptions.Compiled);
        /// <summary> Regex matching: name[column][row] </summary>
        internal static Regex rexTableIdx
            = new Regex(@"^(?<name>[^[]+)\[(?<column>[^[\]]*?)\]\[(?<row>[^[\]]*?)\] *$", RegexOptions.Compiled);
        /// <summary> Regex matching: name(arg)?:val </summary>
        internal static Regex rexFunc
            = new Regex(@"^(?<name>[^(:]+)(?:\((?<arg>[^)]*)\))? *:(?<val>.*)$", RegexOptions.Compiled);
        /// <summary> Regex matching: name.prop(arg)? </summary>
        internal static Regex rexMethod
            = new Regex(@"^(?<name>.+?)\.(?<prop>[^([.]+?)(?:\((?<arg>[^)]*)\))? *$", RegexOptions.Compiled);
        /// <summary> Regex matching: name?[index].prop </summary>
        internal static Regex rexListProp
            = new Regex(@"^(?<name>[^[]*)\[(?<index>[^[\]]*?)\]\.(?<prop>.+)$", RegexOptions.Compiled);
        /// <summary> Regex matching: name?[index].prop(arg)? </summary>
        internal static Regex rexListMethod
            = new Regex(@"^(?<name>[^[]*)\[(?<index>[^[\]]*?)\]\.(?<prop>[^([]+?)(?:\((?<arg>[^)]*)\))? *$", RegexOptions.Compiled);
        /// <summary> Regex matching: name?[index1][index2].prop(arg)? </summary>
        internal static Regex rexTableMethod
            = new Regex(@"^(?<name>[^[]*)\[(?<column>[^[\]]*)\]\[(?<row>[^[\]]*)\]\.(?<prop>[^([]+?)(?:\((?<arg>[^)]*)\))? *$", RegexOptions.Compiled);
        /// <summary> Regex matching: evar: / epvar: / elvar: / ecallback: / ... </summary>
        internal static Regex rexExistVar
            = new Regex(@"^e(?<persist>p?)(?<type>[vltd]|text|image|callback|storage)(?:v?ar)?:(?<name>.*)$", RegexOptions.Compiled);

        internal static Regex reHex8 = new Regex("^[0-9A-Fa-f]{1,8}$", RegexOptions.Compiled);

        internal Dictionary<string, string> namedgroups;
        internal List<string> numgroups;
        internal DateTime triggered;
        internal string zoneIdOverride = null;
        internal string contextResponse = "";
        internal int contextResponseCode = 0;
        internal dynamic contextJsonResponse;
        internal bool contextJsonParsed = false;

        internal List<int> ActionResults = new List<int>();
        internal Dictionary<Mutex, int> heldmutices = new Dictionary<Mutex, int>();

        internal int loopiterator { get; set; } = 0;
        internal Guid loopcontext { get; set; } = Guid.Empty;
        internal string varName { get; set; } = "";         // for ${_this} ${_row[i]} ${_col[i]}
        internal int listIndex { get; set; } = 0;           // for ${_idx}
        internal int tableColIndex { get; set; } = 0;       // for ${_col}
        internal int tableRowIndex { get; set; } = 0;       // for ${_row}
        internal string dictKey { get; set; } = "";         // for ${_key}
        internal string dictValue { get; set; } = "";       // for ${_val}

        public const double EORZEA_MULTIPLIER = 3600D / 175D;
        public const char LINEBREAK_PLACEHOLDER = '⏎';
        private static readonly CultureInfo InvClt = CultureInfo.InvariantCulture;
        private static readonly NumberStyles NSFloat = NumberStyles.Float;
        public Random rng = new Random();

        public NewContext()
        {
            namedgroups = new Dictionary<string, string>();
            numgroups = new List<string>();
        }

        public override string ToString()
        {
            return id.ToString() + " for " + (trig != null ? trig.LogName : "(no trigger)") + " at " + triggered.ToString();
        }

        internal NewContext Duplicate()
        {
            NewContext ctx = (NewContext)MemberwiseClone();
            return ctx;
        }

        internal void PushActionResult(int i)
        {
            lock (ActionResults)
            {
                ActionResults.Add(i);
            }
        }

        internal int PeekActionResult(bool previous, int i)
        {
            lock (ActionResults)
            {
                if (previous == true)
                {
                    if (ActionResults.Count > 0)
                    {
                        return ActionResults[ActionResults.Count - 1];
                    }
                    return 0;
                }
                else
                {
                    if (i < 1 || i > ActionResults.Count)
                    {
                        return 0;
                    }
                    return ActionResults[i - 1];
                }
            }
        }

        public delegate void LoggerCallback(object o, string message);

        public double EvaluateNumericExpression(LoggerDelegate logger, object o, string expr)
        {
            string exp = ExpandVariables(logger, o, true, expr ?? "");
            if (plug != null)
            {
                exp = plug.cfg.PerformSubstitution(exp, Configuration.Substitution.SubstitutionScopeEnum.loggerExpression);
            }
            return MathParser.Parse(exp);
        }

        public string EvaluateStringExpression(LoggerDelegate logger, object o, string expr)
        {
            string exp = ExpandVariables(logger, o, false, expr ?? "");
            if (plug != null)
            {
                exp = plug.cfg.PerformSubstitution(exp, Configuration.Substitution.SubstitutionScopeEnum.StringExpression);
            }
            return exp;
        }

        public delegate void LoggerDelegate(object o, string msg);

        public static TimeSpan GetEorzeanTime()
        {
            long epochTicks = DateTime.UtcNow.Ticks - (new DateTime(1970, 1, 1).Ticks);
            long eorzeaTicks = (long)Math.Round(epochTicks * EORZEA_MULTIPLIER);
            return new DateTime(eorzeaTicks).TimeOfDay;
        }

        private VariableScalar GetScalarVariable(VariableStore vs, string varname, bool createNew)
        {
            if (vs.Scalar.ContainsKey(varname))
            {
                return vs.Scalar[varname];
            }
            VariableScalar v = new VariableScalar();
            if (createNew)
            {
                vs.Scalar[varname] = v;
            }
            return v;
        }

        private VariableList GetListVariable(VariableStore vs, string varname, bool createNew)
        {
            if (vs.List.ContainsKey(varname))
            {
                return vs.List[varname];
            }
            VariableList vl = new VariableList();
            if (createNew)
            {
                vs.List[varname] = vl;
            }
            return vl;
        }

        private VariableTable GetTableVariable(VariableStore vs, string varname, bool createNew)
        {
            if (vs.Table.ContainsKey(varname))
            {
                return vs.Table[varname];
            }
            VariableTable vt = new VariableTable();
            if (createNew)
            {
                vs.Table[varname] = vt;
            }
            return vt;
        }

        private VariableDictionary GetDictVariable(VariableStore vs, string varname, bool createNew)
        {
            if (vs.Dict.ContainsKey(varname))
            {
                return vs.Dict[varname];
            }
            VariableDictionary vd = new VariableDictionary();
            if (createNew)
            {
                vs.Dict[varname] = vd;
            }
            return vd;
        }

        internal static Regex BuildSplitArgRegex(string separator, bool isCompiled = false)
        {
            string s = Regex.Escape(separator);
            char lb = LINEBREAK_PLACEHOLDER;
            return new Regex(
            //    (?<=^|{s}): after a separator or start-of-line
            //               (?:\\s|{lb})*\"[^\"]*\"(?:\\s|{lb})*: spaces? + " + text? + " + spaces?
            //                                                    (?:\\s|{lb})*'[^']*'(?:\\s|{lb})*: spaces? + ' + text? + ' + spaces?
            //                                                                                      [^{s}]*: any unquoted text
            //                                                                                              (?=$|{s}): before a separator or end-of-line
                $"(?<=^|{s})((?:\\s|{lb})*\"[^\"]*\"(?:\\s|{lb})*|(?:\\s|{lb})*'[^']*'(?:\\s|{lb})*|[^{s}]*)(?=$|{s})",
                isCompiled ? RegexOptions.Compiled : RegexOptions.None
            );
        }

        /// <summary>Trim all whitespace characters and the linebreak placeholders from both sides of the string.</summary>
        public static string Trim(string s) => reTrim.Match(s).Groups["string"].Value;
        /// <summary>Trim all whitespace characters and the linebreak placeholders from the left side of the string.</summary>
        public static string TrimL(string s) => reTrimL.Match(s).Groups["string"].Value;
        /// <summary>Trim all whitespace characters and the linebreak placeholders from the right side of the string.</summary>
        public static string TrimR(string s) => reTrimR.Match(s).Groups["string"].Value;

        internal static Regex reSplitArgComma = BuildSplitArgRegex(",", true);
        internal static Regex reSplitArgEqual = BuildSplitArgRegex("=", true);
        internal static Regex reSplitArgPipe = BuildSplitArgRegex("|", true);
        internal static Regex reTrim = new Regex($"^[\\s{LINEBREAK_PLACEHOLDER}]*(?<string>.*?)[\\s{LINEBREAK_PLACEHOLDER}]*$", RegexOptions.Compiled);
        internal static Regex reTrimL = new Regex($"^[\\s{LINEBREAK_PLACEHOLDER}]*(?<string>.*?)$", RegexOptions.Compiled);
        internal static Regex reTrimR = new Regex($"^(?<string>.*?)[\\s{LINEBREAK_PLACEHOLDER}]*$", RegexOptions.Compiled);

        /// <summary> 
        /// Split an expression with commas or other specified separators to a list of arguments. <br />
        /// If the argument contains separator, single/double quotes, it should be quoted like "xx,xx" 'xx,xx' 'xx"xx' "xx'xx".
        /// </summary>
        /// <param name="allowEmptyList"> Converts an empty string or spaces to an empty list, or a list with an empty string.</param>>
        public static string[] SplitArguments(string args, bool allowEmptyList = true, string separator = ",")
        {
            if (allowEmptyList && Trim(args ?? "") == "")
            {
                return new string[0];
            }
            var reSingleArg = (separator == ",") ? reSplitArgComma
                            : (separator == "=") ? reSplitArgEqual
                            : (separator == "|") ? reSplitArgPipe
                            : BuildSplitArgRegex(separator);

            args = UnescapeCustomExpr(args);
            var matches = reSingleArg.Matches(args);
            var result = new List<string>();
            foreach (Match match in matches)
            {
                string currentMatch = Trim(match.Value);
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

        /// <summary> Pick the argument with the given index from a string[]. </summary>
        /// <param name="args"> </param>
        /// <param name="index"> The selected index from the string[] (0-based).</param>
        /// <param name="defaultValue"> If the index is out of range, returns this value.</param>
        /// <param name="setEmptyToDefault"> If the selected argument is an empty string, consider it as the default value or not.</param>
        public static string GetArgument(string[] args, int index, string defaultValue, bool setEmptyToDefault = false)
        {
            if (index >= args.Length || (args[index] == "" && setEmptyToDefault))
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
                string[] sliceArgs = slice.Split(':').Select(s => Trim(s)).ToArray();
                if (sliceArgs.Length > 3) { throw ArgCountError(I18n.TranslateWord("slice"), "0-3", sliceArgs.Length, rawExpr); }

                string startStr = GetArgument(sliceArgs, 0, "", true);
                string endStr = GetArgument(sliceArgs, 1, "", true);
                string stepStr = GetArgument(sliceArgs, 2, "1", true);
                int start = 0; int end = 0; int step = 0;
                try
                {
                    if (sliceArgs.Length == 1 && Trim(startStr) != "")
                    {   // sliceArgs.Length = 1: push a single index
                        start = int.Parse(startStr, InvClt);
                        start = (start >= 0) ? (start - startIndex) : (start + totalLength);
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
                        if (step == 0) { throw InvalidValueError(I18n.TranslateWord("slice"), "step", "0", rawExpr); }
                        if (startStr != "")
                        {   // `start` value given: apply the negative-index and startIndex logics
                            start = int.Parse(startStr, InvClt);
                            start = (start >= 0) ? (start - startIndex) : (start + totalLength);
                        }
                        else
                        {   // `start` value not given: set init value based on the sign of `step`
                            start = (step > 0) ? int.MinValue : int.MaxValue;
                        }
                        if (endStr != "") // logic similar to `start`
                        {
                            end = int.Parse(endStr, InvClt);
                            end = (end >= 0) ? (end - startIndex) : (end + totalLength);
                        }
                        else
                        {
                            end = (step > 0) ? int.MaxValue : int.MinValue;
                        }
                    }
                }
                catch { throw ParseTypeError(I18n.TranslateWord("string"), slice, I18n.TranslateWord("slice"), rawExpr); }

                // fix the out-of-range early start value / late end value
                if (step > 0)
                {
                    start = (start < 0) ? 0 : start;
                    end = (end > totalLength) ? totalLength : end;
                }
                else
                {
                    start = (start > totalLength - 1) ? (totalLength - 1) : start;
                    end = (end < -1) ? -1 : end;
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

        /// <summary> Get the extremum double number from a list of strings. The list should not be empty. </summary>
        public static string GetExtremumNum(List<string> strings, bool isMin)
        {
            try
            {
                string extremum = strings[0];
                double parsedExtremum = double.Parse(extremum, NSFloat, InvClt);
                foreach (var str in strings)
                {
                    double parsedStr = double.Parse(str, NSFloat, InvClt);
                    if ((parsedStr > parsedExtremum) ^ isMin)
                    {
                        extremum = str;
                        parsedExtremum = parsedStr;
                    }
                }
                return extremum;
            }
            catch { return null; }
        }

        /// <summary> Get the extremum hex number from a list of strings. The list should not be empty.  </summary>
        public static string GetExtremumHex(List<string> strings, bool isMin)
        {
            try
            {
                string extremum = strings[0];
                Int64 parsedExtremum = Int64.Parse(extremum, NumberStyles.HexNumber, InvClt);
                foreach (var str in strings)
                {
                    Int64 parsedStr = Int64.Parse(str, NumberStyles.HexNumber, InvClt);
                    if ((parsedStr > parsedExtremum) ^ isMin)
                    {
                        extremum = str;
                        parsedExtremum = parsedStr;
                    }
                }
                return extremum;
            }
            catch { return null; }
        }

        /// <summary> Get the extremum string from a list of strings. The list should not be empty. </summary>
        public static string GetExtremumStr(List<string> strings, bool isMin)
        {
            try
            {
                string extremum = strings[0];
                foreach (var str in strings)
                {
                    if ((str.CompareTo(extremum) > 0) ^ isMin)
                    {
                        extremum = str;
                    }
                }
                return extremum;
            }
            catch { return null; }
        }

        private static void ExtremumInit(string[] args, string gprop, string expr, out string type, out bool isMin)
        {
            type = (GetArgument(args, 0, "n") + " ").Substring(0, 1);
            if (type != "n" && type != "s" && type != "h")
            {
                throw InvalidValueError(gprop, "type", type, expr);
            }
            isMin = gprop.StartsWith("min");
        }

        private string ExtremumGetResult(List<string> strings, string type, bool isMin,
            string varName, string funcName, string totalExpression)
        {
            if (strings.Count == 0) { throw ExtremumListZeroElementError(varName, totalExpression); }
            string result = null;
            switch (type)
            {
                case "n": result = GetExtremumNum(strings, isMin); type = I18n.TranslateWord("double"); break;
                case "h": result = GetExtremumHex(strings, isMin); type = I18n.TranslateWord("hex"); break;
                case "s": result = GetExtremumStr(strings, isMin); type = I18n.TranslateWord("string"); break;
            }
            if (result == null) { throw ExtremumParseTypeError(funcName, type, totalExpression); }
            return result;
        }

        private void ParseTernaryExpression(string input, out string condExpr, out string trueStr, out string falseStr)
        {
            falseStr = ExtractLastExpression(ref input, ':');
            trueStr = ExtractLastExpression(ref input, '?');
            condExpr = Trim(input);
        }

        private string ExtractLastExpression(ref string input, char sep)
        {
            input = TrimR(input);
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

            string afterSep = TrimL(input.Substring(sepIndex.Value + 1));
            int length = afterSep.Length;
            if (length >= 2 && afterSep[0] == afterSep[length - 1] && (afterSep[0] == '\"' || afterSep[0] == '\''))
            {
                afterSep = afterSep.Substring(1, length - 2);  // "..." / '...' => ...
            }

            input = input.Substring(0, sepIndex.Value);
            return afterSep;
        }

        public static string ToFullWidth(string input)
        {
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 32) // ' '
                {
                    array[i] = (char)0x3000; // '　'
                }
                else if (array[i] > 32 && array[i] < 127)
                {
                    array[i] = (char)(array[i] + 0xFEE0);
                }
            }
            return new string(array);
        }

        public static string ToHalfWidth(string input)
        {
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 0x3000)
                {
                    array[i] = (char)32;
                }
                else if (array[i] > 0xFF00 && array[i] < 0xFF5F)
                {
                    array[i] = (char)(array[i] - 0xFEE0);
                }
            }
            return new string(array);
        }

        [Obsolete("Use ToXivBlackChar")]
        public static string ToXivChar(string input, bool combineDigits)
            => ToXivBlackChar(input, combineDigits);

        /// <summary> Convert the letters and numbers in a string to the XIV-defined black box character.</summary>
        /// <param name="combineDigits">True if you want the numbers 10-31 in the string to be combined as a single XIV character.</param>
        public static string ToXivBlackChar(string input, bool combineDigits)
        {
            StringBuilder result = new StringBuilder();
            char[] array = input.ToCharArray();

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] >= 'a' && array[i] <= 'z')
                {
                    // Convert lowercase letters to special XIV capital characters
                    result.Append((char)(array[i] + 57360));
                }
                else if (array[i] >= 'A' && array[i] <= 'Z')
                {
                    // Convert uppercase letters to special XIV capital characters
                    result.Append((char)(array[i] + 57392));
                }
                else if (array[i] >= '0' && array[i] <= '9')
                {
                    // Convert digits to special XIV capital characters 10-31
                    if (combineDigits && i + 1 < array.Length && array[i + 1] >= '0' && array[i + 1] <= '9')
                    {
                        int num = 10 * (array[i] - '0') + (array[i + 1] - '0');
                        if (num >= 10 && num <= 31)
                        {
                            result.Append((char)(num + 57487));
                            i++;
                            continue;
                        }
                    }
                    // Convert digits to special XIV capital characters 0-9
                    result.Append((char)(array[i] + 57439));
                }
                else
                {
                    result.Append(array[i]);
                }
            }
            return result.ToString();
        }

        /// <summary> Convert the 0-9 numbers in a string to the XIV-defined white box character.</summary>
        public static string ToXivWhiteChar(string input)
        {
            StringBuilder result = new StringBuilder();
            char[] array = input.ToCharArray();

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] >= '0' && array[i] <= '9')
                {
                    // U+E0E0 - E0E9
                    result.Append((char)(array[i] + 57520));
                }
                else
                {
                    result.Append(array[i]);
                }
            }
            return result.ToString();
        }

        /// <summary> use "⏎" as a single-digit placeholder for linebreaks
        /// to parse linebreaks as regular characters.</summary>
        public static string ReplaceLineBreak(string str, string placeholder = null)
        {
            placeholder = placeholder ?? LINEBREAK_PLACEHOLDER.ToString();
            return str.Replace("\r\n", placeholder).Replace("\r", placeholder).Replace("\n", placeholder);
        }

        /// <summary> replace linebreaks with the placeholder when converting charcode to char </summary>
        private static char GetReplacedChar(int charcode)
        {
            return (charcode == 10 || charcode == 13) ? LINEBREAK_PLACEHOLDER : (char)charcode;
        }

        /// For arguments and regex in ${func:...}.
        /// __LB__ / '｛' => '{';   __RB__ / '｝' => '}';
        /// __FLB__ => '｛';        __FRB__ => '｝'; 
        /// __FLP__ => '（';        __FRP__ => '）';
        private static string UnescapeCustomExpr(string rawExpr)
        {
            StringBuilder sb = new StringBuilder(rawExpr);
            sb.Replace("__LB__", "{").Replace("__RB__", "}")
              .Replace("｛", "{").Replace("｝", "}")
              .Replace("__FLB__", "｛").Replace("__FRB__", "｝")
              .Replace("__LP__", "(").Replace("__RP__", ")")
              .Replace("__FLP__", "（").Replace("__FRP__", "）");
            return sb.ToString();
        }

        public static Exception ArgCountError(string functionName, string requiredArgCount, int givenArgCount, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/argCountError",
                "({0}) requires {1} arguments, {2} were given. Expr: ({3})",
                functionName, requiredArgCount, givenArgCount, totalExpression));
        }

        public static Exception InvalidValueError(string functionName, string exprDesc, string exprValue, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/invalidValueError",
                "In ({0}), {1} ({2}) is invalid. Expr: ({3})",
                functionName, exprDesc, exprValue, totalExpression));
        }

        public static Exception ParseTypeError(string srcFormatDesc, string srcValue, string tgtFormatDesc, string fullExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/parseTypeError",
                "{0} ({1}) could not be parsed into {2}. Expr: ({3})",
                srcFormatDesc, srcValue, tgtFormatDesc, fullExpression));
        }

        private static Exception ExtremumListZeroElementError(string varName, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/extremumlistzeroelementerror",
                    "The variable ({0}) selected zero elements to get its extremum value. Expr: ({1})",
                    varName, totalExpression));
        }

        private static Exception ExtremumParseTypeError(string funcName, string parseFormat, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/extremumparsetypeerror",
                    "In the function ({0}), not all selected values could be parsed into {1}. Expr: ({2})",
                    funcName, parseFormat, totalExpression));
        }

        private static Exception InfiniteRepeatError(string newStr, string oldStr, string totalExpression)
        {
            return new ArgumentException(I18n.Translate("internal/Context/infiniteRepeatError",
                "In the repeat function, new string ({0}) cannot contain old string ({1}) in loop mode. Expr: ({2})",
                newStr, oldStr, totalExpression));
        }

        private static Exception InfiniteClipboardError()
        {
            return new ArgumentException(I18n.Translate("internal/Context/infiniteClipboardError",
                "The current clipboard contains the expression ${{_clipboard}}, which would cause infinite loop"));
        }

    }

}
*/