using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;
using Triggernometry.Expressions.String.Parsers;
using static Triggernometry.Expressions.String.Utils.ParserCommon;

namespace Triggernometry.Expressions.String.Utils
{
    public static class StringExtensions
    {

        /// <summary>
        /// Check whether the character is considered whitespace in this parser. <br />
        /// · All Unicode whitespace characters: <see cref="char.IsWhiteSpace(char)" />. <br />
        /// · The custom linebreak placeholder <see cref="LINEBREAK" />.
        /// </summary>
        public static bool IsWhiteSpaceEx(this char c)
        {
            return char.IsWhiteSpace(c) || c == LINEBREAK;
        }

        /// <summary>
        /// Trim all characters considered whitespace in this parser from both sides. <br />
        /// · Includes Unicode whitespace (<see cref="char.IsWhiteSpace(char)"/>). <br />
        /// · Includes the custom linebreak placeholder (<see cref="LINEBREAK"/>).
        /// </summary>
        public static string TrimEx(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            int start = 0;
            int end = s.Length - 1;

            while (start <= end && s[start].IsWhiteSpaceEx())
                start++;

            while (end >= start && s[end].IsWhiteSpaceEx())
                end--;

            return s.Substring(start, end - start + 1);
        }

        /// <summary>
        /// Trim all characters considered whitespace in this parser from the left side. <br />
        /// · Includes Unicode whitespace (<see cref="char.IsWhiteSpace(char)"/>). <br />
        /// · Includes the custom linebreak placeholder (<see cref="LINEBREAK"/>).
        /// </summary>
        public static string TrimLeftEx(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            int start = 0;
            while (start < s.Length && s[start].IsWhiteSpaceEx())
                start++;

            return start == 0 ? s : s.Substring(start);
        }

        /// <summary>
        /// Trim all characters considered whitespace in this parser from the right side. <br />
        /// · Includes Unicode whitespace (<see cref="char.IsWhiteSpace(char)"/>). <br />
        /// · Includes the custom linebreak placeholder (<see cref="LINEBREAK"/>).
        /// </summary>
        public static string TrimRightEx(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            int end = s.Length - 1;
            while (end >= 0 && s[end].IsWhiteSpaceEx())
                end--;

            return end == s.Length - 1 ? s : s.Substring(0, end + 1);
        }

        /// <summary>
        /// Convert ASCII characters in the string to their full-width forms. <br />
        /// · Space (0x20) becomes full-width space (0x3000). <br />
        /// · Visible ASCII (0x21–0x7E) is shifted to the full-width Unicode block.
        /// </summary>
        public static string ToFullWidth(this string input)
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

        /// <summary>
        /// Convert full-width characters in the string to their half-width ASCII forms. <br />
        /// · Full-width space (0x3000) becomes ASCII space (0x20). <br />
        /// · Full-width ASCII range (0xFF01–0xFF5E) is converted back to normal ASCII.
        /// </summary>
        public static string ToHalfWidth(this string input)
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

        /// <summary>
        /// Convert the string to Simplified Chinese. <br />
        /// · Segments are split by the linebreak placeholder (<see cref="LINEBREAK"/>). <br />
        /// · Each segment is converted with <see cref="VisualBasic.Strings.StrConv"/>.
        /// </summary>
        public static string ToSimplifiedChinese(this string input)
        {
            var conv = VbStrConv.SimplifiedChinese;
            var lines = input.Split(LINEBREAK)
                             .Select(s => Strings.StrConv(s, conv));
            return string.Join(LINEBREAK_STR, lines);
        }

        /// <summary>
        /// Convert the string to Traditional Chinese. <br />
        /// · Segments are split by the linebreak placeholder (<see cref="LINEBREAK"/>). <br />
        /// · Each segment is converted with <see cref="VisualBasic.Strings.StrConv"/>.
        /// </summary>
        public static string ToTraditionalChinese(this string input)
        {
            var conv = VbStrConv.TraditionalChinese;
            var lines = input.Split(LINEBREAK)
                             .Select(s => Strings.StrConv(s, conv));
            return string.Join(LINEBREAK_STR, lines);
        }

        /// <summary>
        /// Convert letters and digits in the string into XIV-defined black box characters. <br />
        /// · Letters map to the corresponding XIV private-use characters. <br />
        /// · Digits 0–9 map to individual XIV-number glyphs. <br />
        /// · When <paramref name="combineDigits"/> is true, numbers 10–31 are combined into a single XIV glyph.
        /// </summary>
        public static string ToXivBlackBoxChar(this string input, bool combineDigits = false)
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

        /// <summary>
        /// Convert digits 0–9 in the string into XIV-defined white box characters. <br />
        /// · Maps ASCII digits to the U+E0E0–U+E0E9 range (XIV private-use block).
        /// </summary>
        public static string ToXivWhiteBoxChar(this string input)
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

    }

}
