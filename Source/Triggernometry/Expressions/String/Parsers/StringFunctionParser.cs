using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;
using static Triggernometry.Expressions.String.Utils.ArgHelper;
using static Triggernometry.Expressions.String.Utils.ErrorHelper;
using static Triggernometry.Expressions.String.Utils.ParserCommon;

namespace Triggernometry.Expressions.String.Parsers
{
    internal static class StringFunctionParser
    {
        /// <summary> Regex matching: name(arg)?:val </summary>
        internal static Regex rexFunc
            = new Regex(@"^(?<name>[^(:]+)(?:\((?<arg>[^)]*)\))? *:(?<val>.*)$", RegexOptions.Compiled);

        internal static string TryParse(string operand, string rawExpression)
        {
            Match funcMatch = rexFunc.Match(operand);
            if (!funcMatch.Success)
                throw new Exception($"Invalid function expression syntax: '{operand}'");

            var funcNameLower = funcMatch.Groups["name"].Value.ToLower().TrimEx();
            var sourceString = funcMatch.Groups["val"].Value;
            var args = SplitArguments(funcMatch.Groups["arg"].Value);

            void CheckArgCountLocal(string argCountRule)
            {
                CheckArgCount(argCountRule, args.Length, funcNameLower, rawExpression);
            }

            switch (funcNameLower)
            {
                case "toupper":
                    CheckArgCountLocal("0");
                    return sourceString.ToUpper();

                case "tolower":
                    CheckArgCountLocal("0");
                    return sourceString.ToLower();

                case "tofullwidth":
                    CheckArgCountLocal("0");
                    return sourceString.ToFullWidth();

                case "tohalfwidth":
                    CheckArgCountLocal("0");
                    return sourceString.ToHalfWidth();

                case "tosimpcn":
                    CheckArgCountLocal("0");
                    return sourceString.ToSimplifiedChinese();
                case "totradcn":
                    CheckArgCountLocal("0");
                    return sourceString.ToTraditionalChinese();

                case "toxivchar": // old name
                case "toblackchar":
                    CheckArgCountLocal("0-1");
                    var combineDigits = GetArgument(args, 0)?.ParseData<bool>() ?? false;
                    return sourceString.ToXivBlackBoxChar(combineDigits);

                case "towhitechar":
                    CheckArgCountLocal("0");
                    return sourceString.ToXivWhiteBoxChar();

                case "length":
                    CheckArgCountLocal("0");
                    return sourceString.Length.ToString();

                case "hex2dec":    // hex2dec()
                case "hex2float":  // hex2float()
                case "hex2double": // hex2double()
                    CheckArgCountLocal("0");
                    {
                        sourceString = sourceString.TrimEx();
                        if (!long.TryParse(sourceString, NumberStyles.HexNumber, InvClt, out var dataAsLong))
                        {
                            throw ParseTypeError(funcNameLower, sourceString, I18n.TranslateWord("hex"), rawExpression);
                        }
                        switch (funcNameLower)
                        {
                            case "hex2dec":
                                return dataAsLong.ToString();

                            case "hex2float":
                                return "" + BitConverter.ToSingle(BitConverter.GetBytes((int)dataAsLong), 0);

                            case "hex2double":
                                return "" + BitConverter.ToDouble(BitConverter.GetBytes(dataAsLong), 0);
                        }
                        return null; // unreachable
                    }

                case "parsedmg": // parse the hex damage in ACT loglines to dec value
                    CheckArgCountLocal("0");
                    {
                        sourceString = sourceString.TrimEx();
                        if (!reHex8.IsMatch(sourceString))
                        {
                            throw InvalidValueError(funcNameLower, "funcval", sourceString, rawExpression);
                        }
                        return MathParser.ParseDamage(sourceString).ToString(InvClt);
                    }

                case "float2hex":
                    CheckArgCountLocal("0");
                    {
                        if (!float.TryParse(sourceString, NSFloat, InvClt, out float floatValue))
                        {
                            throw ParseTypeError(I18n.TranslateWord("string"), sourceString, I18n.TranslateWord("float"), rawExpression);
                        }
                        byte[] bytesArray = BitConverter.GetBytes(floatValue);
                        Array.Reverse(bytesArray, 0, bytesArray.Length);
                        return BitConverter.ToString(bytesArray).Replace("-", "");
                    }

                case "double2hex":
                    CheckArgCountLocal("0");
                    {
                        if (!double.TryParse(sourceString, NSFloat, InvClt, out double doubleValue))
                        {
                            throw ParseTypeError(I18n.TranslateWord("string"), sourceString, I18n.TranslateWord("double"), rawExpression);
                        }
                        long bytesArray = BitConverter.DoubleToInt64Bits(doubleValue);
                        return bytesArray.ToString("X");
                    }

                case "dec2hex": // dec2hex()
                case "dec2hex2": // dec2hex2()
                case "dec2hex4": // dec2hex4()
                case "dec2hex8": // dec2hex8()
                    CheckArgCountLocal("0");
                    {
                        if (!long.TryParse(sourceString, NSFloat, InvClt, out var result))
                        {
                            throw ParseTypeError(I18n.TranslateWord("string"), sourceString, I18n.TranslateWord("int"), rawExpression);
                        }
                        string format = funcNameLower.Substring(6).ToUpper(); // "X" "X2" "X4" "X8"
                        return result.ToString(format);
                    }

                case "ord": // chars => charcodes separated by separator
                    CheckArgCountLocal("0-1");
                    {
                        string separator = GetArgument(args, 0, ",");
                        List<int> charcodes = new List<int>();
                        for (int idx = 0; idx < sourceString.Length; idx++)
                        {
                            if (char.IsHighSurrogate(sourceString[idx]) && idx + 1 < sourceString.Length && char.IsLowSurrogate(sourceString[idx + 1]))
                            {
                                charcodes.Add(char.ConvertToUtf32(sourceString[idx++], sourceString[idx]));
                            }
                            else
                            {
                                charcodes.Add(sourceString[idx]);
                            }
                        }
                        return string.Join(separator, charcodes);
                    }

                case "chr": // charcodes separated by separator => chars
                    CheckArgCountLocal("0-1");
                    {
                        string separator = GetArgument(args, 0, ",");
                        string[] rawCharcodes = SplitArguments(sourceString, separator: separator);
                        List<string> chars = new List<string>();
                        for (int idx = 0; idx < rawCharcodes.Length; idx++)
                        {
                            if (int.TryParse(rawCharcodes[idx], out int charcode))
                            {
                                chars.Add(char.ConvertFromUtf32(charcode));
                            }
                            else
                            {
                                throw ParseTypeError($"#{idx}" + I18n.TranslateWord("string"), rawCharcodes[idx], I18n.TranslateWord("int"), rawExpression);
                            }
                        }
                        return string.Join("", chars);
                    }

                case "padleft":
                case "padright":
                    CheckArgCountLocal("2");
                    {
                        char paddingChar = args[0].Length == 1 ? args[0][0] : GetReplacedChar(int.Parse(args[0], InvClt));
                        int length = int.Parse(args[1], InvClt);

                        if (funcNameLower == "padleft")
                            return sourceString.PadLeft(length, paddingChar);
                        else
                            return sourceString.PadRight(length, paddingChar);
                    }

                case "repeat": // repeat(times, joiner = "")
                    CheckArgCountLocal("1-2");
                    {
                        if (!int.TryParse(args[0], NSFloat, InvClt, out int times))
                        {
                            throw ParseTypeError(I18n.TranslateWord("times"), args[0], I18n.TranslateWord("int"), rawExpression);
                        }
                        string joiner = GetArgument(args, 1, "");
                        if (times == 0)
                        {
                            return "";
                        }
                        else
                        {
                            if (times < 0)
                            {
                                times = -times;
                                sourceString = new string(sourceString.Reverse().ToArray());
                            }
                            StringBuilder sb = new StringBuilder(sourceString);
                            string repeatedUnit = joiner + sourceString;
                            for (int repeatCount = 1; repeatCount < times; repeatCount++)
                            {
                                sb.Append(repeatedUnit);
                            }
                            return sb.ToString();
                        }
                    }

                case "replace": // replace(oldStr, newStr = "", isLooped = false)
                    CheckArgCountLocal("1-3");
                    {
                        string oldStr = args[0];
                        if (oldStr == "") { throw InvalidValueError(funcNameLower, "oldString", oldStr, rawExpression); }

                        string newStr = GetArgument(args, 1, "");
                        if (newStr == oldStr) { }

                        string isLoopedStr = GetArgument(args, 2, "false");
                        if (!bool.TryParse(isLoopedStr, out bool isLooped))
                        {
                            throw ParseTypeError("isLooped", isLoopedStr, I18n.TranslateWord("bool"), rawExpression);
                        }
                        if (newStr.Contains(oldStr) && isLooped)
                        {
                            throw InfiniteRepeatError(newStr, oldStr, rawExpression);
                        }

                        var result = sourceString.Replace(oldStr, newStr);
                        while (result.Contains(oldStr) && isLooped)
                        {
                            result = result.Replace(oldStr, newStr);
                        }
                        return result;
                    }

                case "dictreplace": // dictreplace(old1 = new1, old2 = new2, ...)
                    {
                        var result = sourceString;
                        foreach (string pair in args)
                        {
                            string[] kv = SplitArguments(pair, separator: "=");
                            result = result.Replace(kv[0], kv[1]);
                        }
                        return result;
                    }

                case "substring": // substring(startindex, length) or substring(startindex)
                    CheckArgCountLocal("1-2");
                    {
                        if (!int.TryParse(args[0], NSFloat, InvClt, out int startIndex))
                        {
                            throw ParseTypeError(I18n.TranslateWord("startindex"), args[0], I18n.TranslateWord("int"), rawExpression);
                        }
                        if (startIndex < 0)
                        {
                            startIndex += sourceString.Length;
                        }
                        switch (args.Length)
                        {
                            case 1:
                                return sourceString.Substring(startIndex);

                            case 2:
                                if (!int.TryParse(args[1], NSFloat, InvClt, out int length))
                                {
                                    throw ParseTypeError(I18n.TranslateWord("length"), args[1], I18n.TranslateWord("int"), rawExpression);
                                }
                                return sourceString.Substring(startIndex, length);
                        }
                        return null; // unreachable
                    }
                case "slice":  // slice(slices = ":") 
                    CheckArgCountLocal("0-1");
                    {
                        string slicesStr = GetArgument(args, 0, ":");
                        var indices = GetSliceIndices(slicesStr, sourceString.Length, rawExpression, startIndex: 0);
                        StringBuilder sb = new StringBuilder();
                        foreach (int index in indices)
                        {
                            sb.Append(sourceString[index]);
                        }
                        return sb.ToString();

                    }
                case "pick": // pick(index, splitter = ",")
                    CheckArgCountLocal("1-2");
                    {
                        string separator = GetArgument(args, 1, ",");
                        string[] strArray = SplitArguments(sourceString, separator: separator);
                        if (!int.TryParse(args[0], NSFloat, InvClt, out int index))
                            throw ParseTypeError(I18n.TranslateWord("index"), args[0], I18n.TranslateWord("int"), rawExpression);

                        int normIndex = index < 0 ? index + strArray.Length : index;
                        return normIndex >= 0 && normIndex < strArray.Length
                            ? strArray[normIndex] : "";
                    }

                case "args": // for testing the argument splitting: ${f:args(...):}
                    return "(" + string.Join(")\n(", args) + ")";

                case "i":
                case "indexof":      // indexof(stringtosearch)
                case "lastindexof":  // lastindexof(stringtosearch)
                    CheckArgCountLocal("1");
                    {
                        int index = funcNameLower.StartsWith("i") ? sourceString.IndexOf(args[0]) : sourceString.LastIndexOf(args[0]);
                        return I18n.ThingToString(index);
                    }

                case "indicesof":
                    CheckArgCountLocal("1-3");
                    {
                        string targetStr = args[0];
                        int subLength = targetStr.Length;
                        int totalLength = sourceString.Length;
                        string joiner = GetArgument(args, 1, defaultValue: ",");
                        string slicesStr = GetArgument(args, 2, defaultValue: ":");
                        List<int> indices = GetSliceIndices(slicesStr, totalLength - subLength + 1, rawExpression, startIndex: 0);
                        StringBuilder sb = new StringBuilder();
                        foreach (int idx in indices)
                        {
                            if (sourceString.Substring(idx, subLength) == targetStr)
                            {
                                if (sb.Length > 0)
                                    sb.Append(joiner);
                                sb.Append(idx);
                            }
                        }
                        return sb.ToString();

                    }
                case "compare": // compare(stringtocompare) or compare(stringtocompare, ignorecase)
                    CheckArgCountLocal("1-2");
                    string ignoreCaseStr = GetArgument(args, 1, "true");
                    if (!bool.TryParse(ignoreCaseStr, out bool ignoreCase))
                    {
                        throw ParseTypeError("ignoreCase", ignoreCaseStr, I18n.TranslateWord("bool"), rawExpression);
                    }
                    return string.Compare(sourceString, args[0], ignoreCase).ToString();

                case "versioncompare": // ${f:versioncompare(1.2.0.0):1.1.8.0} = -1
                    CheckArgCountLocal("1");
                    Version srcVersion = Version.TryParse(sourceString, out Version v)
                        ? v : throw ParseTypeError(I18n.TranslateWord("string"), sourceString, I18n.TranslateWord("version"), rawExpression);
                    Version tgtVersion = Version.TryParse(args[0], out v)
                        ? v : throw ParseTypeError(I18n.TranslateWord("string"), args[0], I18n.TranslateWord("version"), rawExpression);
                    return I18n.ThingToString(srcVersion.CompareTo(tgtVersion));

                case "contain":
                    CheckArgCountLocal("1");
                    return sourceString.Contains(args[0]) ? "1" : "0";

                case "startwith":
                    CheckArgCountLocal("1");
                    return sourceString.StartsWith(args[0]) ? "1" : "0";

                case "endwith":
                    CheckArgCountLocal("1");
                    return sourceString.EndsWith(args[0]) ? "1" : "0";

                case "equal":
                    CheckArgCountLocal("1");
                    return args[0] == sourceString ? "1" : "0";

                case "ifcontain":
                    CheckArgCountLocal("3");
                    return sourceString.Contains(args[0]) ? args[1] : args[2];

                case "ifstartwith":
                    CheckArgCountLocal("3");
                    return sourceString.StartsWith(args[0]) ? args[1] : args[2];

                case "ifendwith":
                    CheckArgCountLocal("3");
                    return sourceString.EndsWith(args[0]) ? args[1] : args[2];

                case "ifequal":
                    CheckArgCountLocal("3");
                    return args[0] == sourceString ? args[1] : args[2];

                case "match": // func:match(str):regex
                    CheckArgCountLocal("1");
                    {
                        Match match = new Regex(UnescapeCustomExpr(sourceString)).Match(args[0]);
                        return match.Success ? "1" : "0";
                    }

                case "capture": // func:capture(str, group):regex
                    CheckArgCountLocal("2");
                    {
                        Match match = new Regex(UnescapeCustomExpr(sourceString)).Match(args[0]);
                        if (int.TryParse(args[1], NSFloat, InvClt, out int groupNumber)
                            && groupNumber >= 0 && groupNumber < match.Groups.Count)
                        {
                            return match.Success ? match.Groups[groupNumber].Value : "";
                        }
                        return match.Success ? match.Groups[args[1]].Value : "";
                    }

                case "ifmatch": // func:ifmatch(str, successStr, failStr):regex
                    CheckArgCountLocal("3");
                    {
                        Match match = new Regex(UnescapeCustomExpr(sourceString)).Match(args[0]);
                        return match.Success ? args[1] : args[2];
                    }

                case "trim":        // trim() or trim(charcode/char, charcode/char, ...)
                case "trimleft":    // trimleft() or trimleft(charcode/char, charcode/char, ...)
                case "trimright":   // trimright() or trimright(charcode/char, charcode/char, ...)
                    string trimChars = "";
                    if (args.Length > 0)
                    {
                        foreach (string arg in args)
                        {
                            // length == 1: char    length != 1: charcode
                            if (arg.Length == 1)
                            {
                                trimChars += arg;
                            }
                            else if (arg.Length == 0)
                            {
                                throw InvalidValueError(funcNameLower, I18n.TranslateWord("char") + "/" + I18n.TranslateWord("charcode"), arg, rawExpression);
                            }
                            else if (arg.Length > 1)
                            {
                                if (!int.TryParse(arg, NSFloat, InvClt, out int charcode))
                                {
                                    throw ParseTypeError(I18n.TranslateWord("charcode"), arg, I18n.TranslateWord("int"), rawExpression);
                                }
                                trimChars += GetReplacedChar(charcode).ToString();
                            }
                        }
                    }
                    char[] trimCharsArray = trimChars.ToCharArray();

                    switch (funcNameLower)
                    {
                        case "trim":
                            return args.Length == 0 ? sourceString.TrimEx() : sourceString.Trim(trimCharsArray);

                        case "trimleft":
                            return args.Length == 0 ? sourceString.TrimLeftEx() : sourceString.TrimStart(trimCharsArray);

                        case "trimright":
                            return args.Length == 0 ? sourceString.TrimRightEx() : sourceString.TrimEnd(trimCharsArray);
                    }
                    return null; // unreachable

                case "format": // format(type,formatstring)
                    CheckArgCountLocal("2");
                    {
                        Type type = Type.GetType(args[0]);
                        object converted = Convert.ChangeType(sourceString, type, InvClt);
                        return string.Format("{0:" + args[1] + "}", converted);
                    }

                case "utctime": // utctime(formatstring)
                case "localtime": // localtime(formatstring)
                    CheckArgCountLocal("0-1");
                    {
                        var ts = long.Parse(sourceString, InvClt);
                        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(ts);
                        if (funcNameLower == "localtime")
                        {
                            dt = dt.ToLocalTime();
                        }
                        string format = GetArgument(args, 0, "");
                        return dt.ToString(format);
                    }
                default:
                    throw new Exception($"Unknown string function '{funcNameLower}' in expression '{rawExpression}'.");
            }
        }
    }
}
