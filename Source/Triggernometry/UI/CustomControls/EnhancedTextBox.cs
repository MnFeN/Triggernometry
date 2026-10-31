using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Utils;
using SupportedExpressionTypeEnum = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum;

namespace Triggernometry.UI.CustomControls
{
    /// <summary>支持连续中文选词、括号选区和三击选行的文本框。</summary>
    public class EnhancedTextBox : System.Windows.Forms.TextBox
    {
        /// <summary>允许只读文本框使用 Ctrl+A 选择全部内容。</summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ReadOnly && ShortcutsEnabled && keyData == (Keys.Control | Keys.A))
            {
                SelectAll();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Enhanced Double-Click Selection

        private int clickCount = 0;
        private DateTime lastClickTime = DateTime.Now;
        /// <summary>普通文本按文本规则选词；表达式框可指定当前表达式类型。</summary>
        protected virtual SupportedExpressionTypeEnum SelectionExpressionType => SupportedExpressionTypeEnum.String;

        /// <summary>接管双击和三击选区，避免系统按中文分词规则拆分文本。</summary>
        protected override void WndProc(ref Message m)
        {
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_LBUTTONDBLCLK = 0x0203;

            if (m.Msg == WM_LBUTTONDBLCLK)
            {
                clickCount = 2;
                lastClickTime = DateTime.Now;

                int x = m.LParam.ToInt32() & 0xFFFF;
                int y = (m.LParam.ToInt32() >> 16) & 0xFFFF;
                Point clickPoint = new Point(x, y);

                int index = GetCharIndexFromPosition(clickPoint);
                if (index < 0)
                    return;

                Point charPosition = GetPositionFromCharIndex(index);
                if (clickPoint.X < charPosition.X && index > 0)
                {
                    index--;
                }

                HandleDoubleClick(index);
                return;
            }
            if (m.Msg == WM_LBUTTONDOWN)
            {
                TimeSpan interval = DateTime.Now - lastClickTime;
                if (interval.TotalMilliseconds > SystemInformation.DoubleClickTime || clickCount >= 3)
                {
                    clickCount = 0;
                }

                clickCount++;
                lastClickTime = DateTime.Now;
                if (clickCount == 3)
                {
                    if (Multiline)
                    {
                        int x = m.LParam.ToInt32() & 0xFFFF;
                        int y = (m.LParam.ToInt32() >> 16) & 0xFFFF;
                        int index = GetCharIndexFromPosition(new Point(x, y));
                        int lineIndex = GetLineFromCharIndex(index);
                        int lineStart = GetFirstCharIndexFromLine(lineIndex);
                        int lineEnd = GetFirstCharIndexFromLine(lineIndex + 1);
                        if (lineEnd == -1)
                            lineEnd = Text.Length; // last line

                        Select(lineStart, lineEnd - lineStart);
                    }
                    else
                    {
                        Select(0, Text.Length);
                    }
                    clickCount = 0;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        /// <summary>根据点击位置选择连续文本、运算符或成对括号。</summary>
        private void HandleDoubleClick(int index)
        {
            if (index < 0 || index >= Text.Length)
                return;

            char currentChar = Text[index];
            if (char.IsWhiteSpace(currentChar))
            {
                SelectAdjacentChars(index, c => char.IsWhiteSpace(c) && c != '\n' && c != '\r');
                return;
            }

            SupportedExpressionTypeEnum type = GetExpressionEnvironmentAt(index);

            if (_leftBracketChars.ContainsKey(currentChar) || _rightBracketChars.ContainsKey(currentChar) || currentChar == '$' || currentChar == '¤')
            {
                SelectEnclosedBrackets(index);
                return;
            }

            if (type == SupportedExpressionTypeEnum.Numeric)
            {
                // select the adjacent alphanumeric chars
                if (!MathParser.OperatorChar.Contains(currentChar))
                {
                    SelectAdjacentChars(index, c => !MathParser.OperatorChar.Contains(c) && !char.IsWhiteSpace(c) && c != '}');
                    // if the prev char is a unary operator, select it
                    int prevIdx = SelectionStart - 1;
                    if (prevIdx < 0)
                    {
                        return;
                    }
                    char prevChar = Text[prevIdx];
                    if (prevChar != '+' && prevChar != '-')
                    {
                        return;
                    }
                    int i = prevIdx - 1;
                    // scan the prev char before +/- to check if it is really a unary op
                    while (true)
                    {
                        if (i < 0 || Text[i] == '\n' || MathParser.OperatorChar.Contains(Text[i]))
                        {
                            Select(prevIdx, SelectionLength + 1); return;
                        }
                        else if (char.IsWhiteSpace(Text[i--]))
                            continue;
                        else
                            return;
                    }
                }
                // select the current op
                else
                {
                    // if it is the 2nd char of an op
                    if (index > 0 && !char.IsWhiteSpace(Text[index - 1]))
                    {
                        string potentialOperator = Text.Substring(index - 1, 2);
                        if (MathParser.OperatorOrder.Contains(potentialOperator))
                        {
                            Select(index - 1, 2);
                            return;
                        }
                    }
                    // if it is the 1st char of an op
                    if (index < Text.Length - 1 && !char.IsWhiteSpace(Text[index + 1]))
                    {
                        string potentialOperator = Text.Substring(index, 2);
                        if (MathParser.OperatorOrder.Contains(potentialOperator))
                        {
                            Select(index, 2);
                            return;
                        }
                    }
                    // if it is a single-char op
                    Select(index, 1);
                }
            }
            else if (type == SupportedExpressionTypeEnum.String || type == SupportedExpressionTypeEnum.Color
                  || type == SupportedExpressionTypeEnum.Regex) // To do: better logic for regexes
            {
                HashSet<char> separators = new HashSet<char>($"^$¤{{}}[]()<>,.:;=|/\\'\"，。？！、：；（）《》「」『』【】“”‘’…{ParserCommon.LINEBREAK}");
                if (type == SupportedExpressionTypeEnum.Regex)
                    separators.ExceptWith(".");
                if (!separators.Contains(currentChar))
                {
                    SelectAdjacentChars(index, c => !separators.Contains(c) && !char.IsWhiteSpace(c));
                }
                else
                {
                    Select(index, 1);
                }
            }
        }

        /// <summary>向两侧扩展选区，直到字符不再满足同一规则。</summary>
        private void SelectAdjacentChars(int index, Func<char, bool> predicate)
        {
            int start = index;
            int end = index;

            while (start > 0 && predicate(Text[start - 1]))
                start--;

            while (end < Text.Length - 1 && predicate(Text[end + 1]))
                end++;

            Select(start, end - start + 1);
        }

        /// <summary>识别当前位置的表达式类型，数值占位符内部使用数值选词规则。</summary>
        private SupportedExpressionTypeEnum GetExpressionEnvironmentAt(int position)
        {
            if (SelectionExpressionType == SupportedExpressionTypeEnum.Regex)
                return SupportedExpressionTypeEnum.Regex;

            if (position > Text.Length)
                return SupportedExpressionTypeEnum.String;

            string prevText = Text.Substring(0, position);
            int lBracketCount = 0;
            string afterLBracket = "";
            for (int i = prevText.Length - 1; i >= 0; i--)
            {
                if (prevText[i] == '}')
                    lBracketCount--;
                else if (prevText[i] == '{')
                    lBracketCount++;

                if (lBracketCount == 1)
                {
                    afterLBracket = prevText.Substring(i + 1);
                    break;
                }
            }

            if (lBracketCount != 1) // not in {...}
                return SelectionExpressionType;

            if (afterLBracket.StartsWith("n:") || afterLBracket.StartsWith("numeric:"))
                return SupportedExpressionTypeEnum.Numeric;
            else
                return SupportedExpressionTypeEnum.String;
        }

        private static readonly Dictionary<char, char> _leftBracketChars = new Dictionary<char, char> {
            { '(',  ')'  }, { '[',  ']'  }, { '{',  '}'  },
            { '（', '）' }, { '［', '］' }, { '｛', '｝' }, 
            { '【', '】' }, { '《', '》' }, { '<',  '>'  },
            { '“',  '”'  }, { '‘',  '’'  }, { '「', '」' },
            { '\"', '\"' }, { '\'', '\'' }, // too complicated to determine if a " or ' is left/right, so always consider it as left for now
        };

        private static readonly Dictionary<char, char> _rightBracketChars = _leftBracketChars.ToDictionary(pair => pair.Value, pair => pair.Key);

        /// <summary>选中配对括号及其内容，并保留占位符前缀。</summary>
        private void SelectEnclosedBrackets(int index)
        {
            char clicked = Text[index];
            char pair;
            int direction;
            if (_leftBracketChars.ContainsKey(clicked))
            {
                pair = _leftBracketChars[clicked];
                direction = 1;
            }
            else if (_rightBracketChars.ContainsKey(clicked))
            {
                pair = _rightBracketChars[clicked];
                direction = -1;
            }
            else if ((clicked == '$' || clicked == '¤') && index < Text.Length - 1 && Text[index + 1] == '{')
            {

                SelectEnclosedBrackets(index + 1); return;
            }
            else
            {
                Select(index, 1); return;
            }
           
            int count = 0; 
            int end = (direction == 1) ? Text.Length - 1 : 0;
            for (int i = index + direction; i >= 0 && i < Text.Length; i += direction)
            {
                char c = Text[i];
                if (c == pair)
                {
                    count++;
                }
                else if (c == clicked)
                {
                    count--;
                }
                if (count == 1)
                {
                    end = i;  break;
                }
            }

            int start = Math.Min(index, end);
            int length = Math.Abs(index - end) + 1;
            if (start >= 1 && Text[start] == '{' && (Text[start - 1] == '$' || Text[start - 1] == '¤'))
            {
                start--;  length++;
            }
            Select(start, length);
        }

        #endregion

        /// <summary>选中当前选区外的最近一层括号。</summary>
        public void SelectNextOuterBracket()
        {
            bool hasOuterLayer = false;
            int index;
            Dictionary<char, int> counts = _leftBracketChars.ToDictionary(pair => pair.Key, pair => 0);
            for (index = SelectionStart - 1; index >= 0; index--)
            {
                char c = Text[index];
                if (c == '\"' || c == '\'')
                {
                    continue;
                }
                if (_rightBracketChars.TryGetValue(c, out char pair))
                {
                    counts[pair]--;
                }
                else if (_leftBracketChars.ContainsKey(c))
                {
                    if (++counts[c] == 1)
                    {
                        hasOuterLayer = true;
                        break;
                    }
                }
            }
            if (hasOuterLayer)
            {
                SelectEnclosedBrackets(index);
            }
            else
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }
    }
}
