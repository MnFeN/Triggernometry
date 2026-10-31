using System;
using System.Windows.Forms;
using Triggernometry.Core;

namespace Triggernometry.UI.CustomControls
{
    public partial class ExpressionTextBox
    {
        /// <summary>为表达式输入保留展开、模板快捷键和补全行为。</summary>
        public class TextBox : EnhancedTextBox
        {

            /// <summary>将可编辑状态下的回车交给输入框处理，避免触发表单默认按钮。</summary>
            protected override bool IsInputKey(Keys keyData)
            {
                if (!ReadOnly && (keyData & Keys.KeyCode) == Keys.Enter)
                    return true;
                return base.IsInputKey(keyData);
            }

            private ExpressionTextBox ExpTextBox => Parent as ExpressionTextBox;

            protected override SupportedExpressionTypeEnum SelectionExpressionType
                => ExpTextBox?.ExpressionType ?? SupportedExpressionTypeEnum.String;

            protected override void WndProc(ref Message m)
            {
                const int WM_PASTE = 0x302;
                if (m.Msg == WM_PASTE && !ReadOnly && !Multiline && Clipboard.GetText().Contains("\n"))
                {
                    ExpTextBox?.ToggleExpand();
                    BeginInvoke(new System.Action(() => Paste()));
                    return;
                }
                base.WndProc(ref m);
            }

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

            #endregion
        }
    }
}
