using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.UI.CustomControls
{
    public partial class ExpressionTextBox
    {
        public class TextBox : System.Windows.Forms.TextBox
        {

            /// <summary>将可编辑状态下的回车交给输入框处理，避免触发表单默认按钮。</summary>
            protected override bool IsInputKey(Keys keyData)
            {
                if (!ReadOnly && (keyData & Keys.KeyCode) == Keys.Enter)
                    return true;
                return base.IsInputKey(keyData);
            }

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
            private ExpressionTextBox ExpTextBox => Parent as ExpressionTextBox;

            protected override void WndProc(ref Message m)
            {
                const int WM_LBUTTONDOWN = 0x0201;
                const int WM_LBUTTONDBLCLK = 0x0203;
                const int WM_PASTE = 0x302;

                if (m.Msg == WM_LBUTTONDBLCLK)
                {
                    clickCount = 2;
                    lastClickTime = DateTime.Now;

                    int x = m.LParam.ToInt32() & 0xFFFF;
                    int y = (m.LParam.ToInt32() >> 16) & 0xFFFF;
                    Point clickPoint = new Point(x, y);

                    int index = GetCharIndexFromPosition(clickPoint);
                    if (index < 0) return;

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
                            if (lineEnd == -1) lineEnd = Text.Length; // last line

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
                if (m.Msg == WM_PASTE && !ReadOnly)
                {
                    string clipboardText = Clipboard.GetText();
                    if (!Multiline && clipboardText.Contains("\n"))
                    {
                        ExpTextBox?.ToggleExpand();
                        this.BeginInvoke(new System.Action(() => this.Paste()));
                        return;
                    }
                }
                base.WndProc(ref m);
            }

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
                            else if (char.IsWhiteSpace(Text[i--])) continue;
                            else return;
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

            private SupportedExpressionTypeEnum GetExpressionEnvironmentAt(int position)
            {
                if (ExpTextBox?._ExpressionType == SupportedExpressionTypeEnum.Regex)
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
                    return ExpTextBox?._ExpressionType ?? SupportedExpressionTypeEnum.String;

                if (afterLBracket.StartsWith("n:") || afterLBracket.StartsWith("numeric:"))
                    return SupportedExpressionTypeEnum.Numeric;
                else
                    return SupportedExpressionTypeEnum.String;
            }

            private static Dictionary<char, char> _leftBracketChars = new Dictionary<char, char> {
                { '(',  ')'  }, { '[',  ']'  }, { '{',  '}'  },
                { '（', '）' }, { '［', '］' }, { '｛', '｝' }, 
                { '【', '】' }, { '《', '》' }, { '<',  '>'  },
                { '“',  '”'  }, { '‘',  '’'  }, { '「', '」' },
                { '\"', '\"' }, { '\'', '\'' }, // too complicated to determine if a " or ' is left/right, so always consider it as left for now
            };

            private static Dictionary<char, char> _rightBracketChars = _leftBracketChars.ToDictionary(pair => pair.Value, pair => pair.Key);

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

            #region Shortcuts

            private Keys prevKeyWithCtrlShift = Keys.None;

            protected override void OnKeyUp(KeyEventArgs e)
            {
                if (!e.Control || !e.Shift)
                {
                    prevKeyWithCtrlShift = Keys.None;
                }
                base.OnKeyUp(e);
            }

            protected override void OnKeyDown(KeyEventArgs e)
            {
                if (e.Control && e.Shift && !e.Alt && RealPlugin.Instance.Config.EnableShortcutTemplates)
                {
                    if (ReadOnly && e.KeyCode != Keys.A)
                    {
                        base.OnKeyDown(e);
                        return;
                    }
                    bool handled = true;
                    bool shouldWrap = SelectionLength != 0 && RealPlugin.Instance.Config.WrapTextWhenSelected;
                    bool useAbbrev = RealPlugin.Instance.Config.UseAbbrevInTemplates;
                    switch (e.KeyCode)
                    {
                        // Ctrl + Shift + 4: ${}
                        case Keys.D4:
                            if (shouldWrap)
                                Paste($"${{{SelectedText}}}"); 
                            else
                                InsertStringOnBothSides("${", "}");
                            break;
                        // Ctrl + Shift + V/L/T/D: ${v/l/t/d:};
                        // Ctrl + Shift + P, V/L/T/D: ${pv/pl/pt/pd:};
                        case Keys.P:
                            break;
                        case Keys.V:
                        case Keys.L:
                        case Keys.T:
                        case Keys.D:
                            string varType = e.KeyCode.ToString().ToLower();
                            varType = useAbbrev ? varType : varType.Trim('v') + "var";
                            if (prevKeyWithCtrlShift == Keys.P)
                                varType = "p" + varType;
                            if (shouldWrap)
                                Paste($"${{{varType}:{SelectedText}}}");
                            else
                                InsertStringOnBothSides($"${{{varType}:", "}");
                            break;
                        // Ctrl + Shift + N: ${n:}
                        case Keys.N:
                            string numeric = useAbbrev ? "n" : "numeric";
                            if (shouldWrap)
                                Paste($"${{{numeric}: {SelectedText.TrimStart()}}}");
                            else
                                InsertStringOnBothSides($"${{{numeric}: ", "}");
                            break;
                        // Ctrl + Shift + F: ${f::}
                        case Keys.F:
                            string func = useAbbrev ? "f" : "func";
                            if (shouldWrap)
                                InsertStringOnBothSides($"${{{func}:", $":{SelectedText}}}");
                            else
                                InsertStringOnBothSides($"${{{func}:", ":}");
                            break;
                        // Ctrl + Shift + E: ${_entity[].}
                        case Keys.E:
                            string entity = useAbbrev ? "entity" : "ffxiventity";
                            if (shouldWrap)
                                InsertStringOnBothSides($"${{_{entity}[{SelectedText}].", "}");
                            else
                                InsertStringOnBothSides($"${{_{entity}[", "].}");
                            break;
                        // Ctrl + Shift + M: ${_me.id}
                        case Keys.M:
                            Paste(useAbbrev ? "${_me.id}" : "${_ffxiventity[${_ffxivplayer}].id}");
                            SelectionStart -= 3;
                            SelectionLength = 2; // select "id"
                            break;
                        // Ctrl + Shift + I: ${if: ? : }
                        case Keys.I:
                            if (shouldWrap)
                                InsertStringOnBothSides("${if: " + SelectedText.Trim() + " ? ", " :  }");
                            else
                                InsertStringOnBothSides("${if: ", " ?  :  }");
                            break;
                        // Ctrl + Shift + A: Select the next outer layer of brackets
                        case Keys.A:
                            SelectNextOuterBracket();
                            ExpTextBox?.HideAutocomplete();
                            break;
                        // Ctrl + Shift + C (in regex textboxes): named capture group (?<name>xxx)
                        case Keys.C:
                            if (ExpTextBox?.ExpressionType == SupportedExpressionTypeEnum.Regex)
                            {
                                if (shouldWrap)
                                    InsertStringOnBothSides("(?<", $">{SelectedText})");
                                else
                                    InsertStringOnBothSides("(?<", ">)");
                            }
                            else
                            {
                                handled = false;
                            }
                            break;
                        default:
                            handled = false;
                            break;
                    }
                    prevKeyWithCtrlShift = e.KeyCode;
                    if (handled)
                    {
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                        return;
                    }
                }
                base.OnKeyDown(e);
            }

            public void InsertStringOnBothSides(string newTextBeforeCursor, string newTextAfterCursor = "")
            {
                Paste(newTextBeforeCursor + newTextAfterCursor);
                SelectionStart -= newTextAfterCursor.Length;
                ExpTextBox?.ProcessAutocomplete();
            }

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
            #endregion

        }
    }
}
