using System;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.UI.Forms;
using Triggernometry.Localization;
using Triggernometry.Core.Actions;

namespace Triggernometry.UI.CustomControls
{

    public class RichTextBoxHelper : ReadonlyRichTextBox // to do: change to CustomControls
    {
        private readonly TableLayoutPanel ParentTable;
        private readonly Func<string> helpKey;
        private static readonly System.ComponentModel.ComponentResourceManager resources =
            new System.ComponentModel.ComponentResourceManager(typeof(ActionForm));
        private string displayedKey;
        private object displayedLanguage;
        private Font displayedFont;
        private bool? displayedExpanded;

        internal static bool _Expanded = false;  // 所有帮助框共用展开状态。
        public bool Expanded
        {
            get { return _Expanded; }
            set
            {
                _Expanded = value;
                UpdateText();
            }
        }

        /// <summary>在布局末尾添加可展开的帮助区域。</summary>
        public RichTextBoxHelper(string name, Form parentForm, TableLayoutPanel table, int colIndex = 1, Func<string> helpKey = null)
        {
            Name = name;
            Tag = I18n.DoNotTranslate;
            Dock = DockStyle.Fill;
            Margin = new Padding(3, 15, 15, 7);
            ScrollBars = RichTextBoxScrollBars.Vertical;

            this.helpKey = helpKey;
            ParentTable = table;
            // 应用富文本样式前先继承编辑器字体。
            ResetFont();
            // 追加到最后一行，并占满该行剩余列。
            ParentTable.RowCount += 1;
            ParentTable.RowStyles.Add(new RowStyle());
            ParentTable.Controls.Add(this, colIndex, ParentTable.RowCount - 1);
            if (ParentTable.ColumnCount - colIndex > 1)
            {
                table.SetColumnSpan(this, ParentTable.ColumnCount - colIndex);
            }

            Cursor = Cursors.Help;
            Expanded = _Expanded;
        }

        /// <summary>按展开状态和当前帮助键更新文本及格式。</summary>
        internal void UpdateText()
        {
            string key = _Expanded ? helpKey?.Invoke() ?? "" : "rtbHelper";
            if (displayedExpanded == _Expanded && displayedKey == key &&
                ReferenceEquals(displayedLanguage, I18n.CurrentLanguage) && Equals(displayedFont, Font))
            {
                UpdateHeight();
                return;
            }
            Lock();
            try
            {
                Text = _Expanded
                    ? I18n.Translate("ActionForm/" + key, resources.GetString(key + ".Text") ?? "")
                    : I18n.Translate("ActionForm/rtbHelper", "[Show Help]");
                SetStyles();
                displayedKey = key;
                displayedLanguage = I18n.CurrentLanguage;
                displayedFont = Font;
                displayedExpanded = _Expanded;
                UpdateHeight();
            }
            finally { Unlock(); }
        }

        /// <summary>界面字号改变后重新应用帮助字体和着色。</summary>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (ParentTable != null)
                UpdateText();
        }

        private const int WM_SETCURSOR = 0x0020;
        private const int WM_LBUTTONUP = 0x202;

        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr hCursor);
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONUP)
            {
                Expanded = !Expanded;
            }
            else if (m.Msg == WM_SETCURSOR)
            {
                SetCursor(Cursors.Help.Handle);
                return;
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        /// <summary>根据可用空间和文本长度调整帮助区高度。</summary>
        internal void UpdateHeight()
        {
            int lineHeight = (int)(Font.Height * 1.5);
            int height = lineHeight;
            if (_Expanded && ParentTable.Parent is ScrollableControl viewport)
            {
                const int minimumHeight = 200;
                // 使用未滚动时的位置计算空间，避免滚动反过来改变帮助区高度。
                int top = ParentTable.Top - viewport.AutoScrollPosition.Y + Top;
                int availableHeight = viewport.ClientSize.Height - top - Margin.Bottom - ParentTable.Padding.Bottom;
                int maximumHeight = Math.Max(minimumHeight, availableHeight);
                int textHeight = (GetLineFromCharIndex(TextLength) + 1) * lineHeight;
                height = Math.Min(maximumHeight, Math.Max(minimumHeight, textHeight));
            }
            if (Height != height)
                Height = height;
        }

        internal static Color[] expressionColors = {
                Color.FromArgb(0, 85, 221),
                Color.FromArgb(140, 0, 0)
            };
        internal static Color monospaceColor = Color.FromArgb(32, 64, 144);
        internal static Color stringColor = Color.FromArgb(34, 153, 0);
        internal static Color separatorColor = Color.FromArgb(204, 102, 0);

        /// <summary>先计算文本样式，再按连续区间提交给 RichEdit，避免逐字符发送格式消息。</summary>
        public void SetStyles()
        {
            SelectAll();
            SelectionFont = Font;
            if (!_Expanded)
            {
                SelectionColor = Color.FromArgb(160, 160, 160);
                Select(0, 0);
                return;
            }
            string raw = Text;
            var colors = new Color[raw.Length];
            var monospace = new bool[raw.Length];
            var remove = new bool[raw.Length];
            int depth = 0;
            for (int i = 0; i < raw.Length; i++)
            {
                if (i + 1 < raw.Length && raw[i] == '$' && raw[i + 1] == '{')
                    depth++;
                colors[i] = depth > 0 ? expressionColors[(depth - 1) % expressionColors.Length] : Color.Black;
                if (depth > 0 && raw[i] == '}')
                    depth--;
            }
            // 反引号隐藏；其中的表达式仍保留原有颜色，其余文字采用等宽字体颜色。
            int start = 0;
            while (start < raw.Length)
            {
                int open = raw.IndexOf('`', start);
                int close = open < 0 ? -1 : raw.IndexOf('`', open + 1);
                if (close < 0)
                    break;
                for (int i = open; i <= close; i++)
                {
                    monospace[i] = true;
                    if (colors[i] == Color.Black)
                        colors[i] = monospaceColor;
                }
                remove[open] = remove[close] = true;
                start = close + 1;
            }
            var text = new StringBuilder(raw.Length);
            int length = 0;
            for (int i = 0; i < raw.Length; i++)
            {
                if (remove[i])
                    continue;
                text.Append(raw[i]);
                colors[length] = colors[i];
                monospace[length++] = monospace[i];
            }
            string formatted = text.ToString();
            ApplyQuotedColors(formatted, '\'', colors);
            ApplyQuotedColors(formatted, '"', colors);
            for (int i = 0; i < length; i++)
            {
                char ch = formatted[i];
                if (colors[i] == Color.Black || ch == ',' && colors[i] == stringColor)
                    continue;
                if (ch == ',' || ch == '|' || ch == '=' || ch == ';' || ch == ':' || ch == ParserCommon.LINEBREAK)
                    colors[i] = separatorColor;
            }
            Text = formatted;
            SelectAll();
            SelectionFont = Font;
            SelectionColor = Color.Black;
            using (var codeFont = new Font("Consolas", Font.Size))
            {
                for (start = 0; start < length;)
                {
                    int end = start + 1;
                    while (end < length && colors[end] == colors[start] && monospace[end] == monospace[start]) end++;
                    Select(start, end - start);
                    SelectionColor = colors[start];
                    if (monospace[start])
                        SelectionFont = codeFont;
                    start = end;
                }
            }
            Select(0, 0);
        }

        /// <summary>沿用引号优先级：包含表达式的片段只为两端引号着色。</summary>
        private static void ApplyQuotedColors(string text, char quote, Color[] colors)
        {
            // 不使用 RichEdit.Find：它在文本末尾会回绕，可能将未配对引号误认为成对标记。
            int start = 0;
            while (start < text.Length)
            {
                int open = text.IndexOf(quote, start);
                int close = open < 0 ? -1 : text.IndexOf(quote, open + 1);
                if (close < 0)
                    break;
                int dollar = text.IndexOf('$', open, close - open + 1);
                if (dollar < 0)
                    for (int i = open; i <= close; i++) colors[i] = stringColor;
                colors[open] = colors[close] = stringColor;
                start = close + 1;
            }
        }

    }

    public class ReadonlyRichTextBox : RichTextBox
    {
        public bool AllowWheelThrough { get; set; } = false;

        public ReadonlyRichTextBox() : base()
        {
            ReadOnly = true;
            TabStop = false;
            BorderStyle = BorderStyle.None;

            if (I18n.CurrentLanguage != null)
            {
                if (I18n.CurrentLanguage.LanguageName.Contains("CN")) // will switch to a more general implementation
                {
                    Font = new Font("Microsoft YaHei", Font.Size);
                }
            }

            DoubleBuffered = true;
        }

        private const int WM_SETFOCUS = 0x7;
        private const int WM_SETREDRAW = 0x000B;
        private const int WM_SETCURSOR = 0x0020;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_MOUSEWHEEL = 0x020A;

        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        protected override void WndProc(ref Message m)
        {
            HideCaret(Handle);
            if (m.Msg == WM_SETFOCUS || m.Msg == WM_KEYDOWN || m.Msg == WM_KEYUP ||
                m.Msg == WM_LBUTTONDOWN || m.Msg == WM_LBUTTONUP || m.Msg == WM_MOUSEMOVE || m.Msg == WM_LBUTTONDBLCLK ||
                m.Msg == WM_RBUTTONDOWN || m.Msg == WM_RBUTTONUP || m.Msg == WM_RBUTTONDBLCLK ||
                m.Msg == WM_SETCURSOR)
            {
                return;
            }
            else if (m.Msg == WM_MOUSEWHEEL && AllowWheelThrough)
            {
                SendMessage(Parent.Handle, m.Msg, m.WParam, m.LParam);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        public void Lock()
        {
            SendMessage(Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
            ReadOnly = false;
        }

        public void Unlock()
        {
            SendMessage(Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            ReadOnly = true;
            Refresh();
        }

        /// <summary>从父控件链中取得不透明背景色。</summary>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                // 原生 RichEdit 不支持透明背景，需要沿父控件链寻找不透明颜色。
                Control background = Parent;
                while (background != null && background.BackColor.A != 255) background = background.Parent;
                BackColor = background?.BackColor ?? SystemColors.Control;
            }
        }

    }

}
