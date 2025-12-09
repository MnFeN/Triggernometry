using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Triggernometry.Expressions.String.Utils
{
    public static class ParserCommon
    {
        internal static Regex reHex8 = new Regex("^[0-9A-Fa-f]{1,8}$", RegexOptions.Compiled);
        public const char LINEBREAK = '⏎';
        public const string LINEBREAK_STR = "⏎";
        public const int MAX_DEPTH = 10000;
        internal static readonly CultureInfo InvClt = CultureInfo.InvariantCulture;
        internal static readonly NumberStyles NSFloat = NumberStyles.Float;
        internal static readonly Random rng = new Random();

        /// <summary> use "⏎" as a single-digit placeholder for linebreaks
        /// to parse linebreaks as regular characters.</summary>
        public static string ReplaceLineBreak(string str, string placeholder = null)
        {
            if (str == null)
                return null;

            // fast path: no linebreaks
            if (str.IndexOfAny(new[] { '\r', '\n' }) < 0)
                return str;

            placeholder = placeholder ?? LINEBREAK.ToString();

            var sb = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                if (c == '\r')
                {
                    if (i + 1 < str.Length && str[i + 1] == '\n') //  \r\n
                        i++; // also skip '\n'

                    sb.Append(placeholder);
                }
                else if (c == '\n')
                    sb.Append(placeholder);
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary> replace linebreaks with the placeholder when converting charcode to char </summary>
        internal static char GetReplacedChar(int charcode)
        {
            return charcode == 10 || charcode == 13 ? LINEBREAK : (char)charcode;
        }

        /// For arguments and regex in ${func:...}.
        /// __LB__ / '｛' => '{';   __RB__ / '｝' => '}';
        /// __FLB__ => '｛';        __FRB__ => '｝'; 
        /// __FLP__ => '（';        __FRP__ => '）';
        internal static string UnescapeCustomExpr(string rawExpr)
        {
            StringBuilder sb = new StringBuilder(rawExpr);
            sb.Replace("__LB__", "{").Replace("__RB__", "}")
              .Replace("｛", "{").Replace("｝", "}")
              .Replace("__FLB__", "｛").Replace("__FRB__", "｝")
              .Replace("__LP__", "(").Replace("__RP__", ")")
              .Replace("__FLP__", "（").Replace("__FRP__", "）");
            return sb.ToString();
        }
    }
}
