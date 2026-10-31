using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;
using System.ComponentModel;
using Triggernometry.UI.Forms;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;
using Triggernometry.Core;

namespace Triggernometry.UI.CustomControls
{
    public partial class ExpressionTextBox : UserControl
    {

        public enum SupportedExpressionTypeEnum
        {
            String, Numeric, Regex, Color
        }

        public bool ReadOnly
        {
            get => txtExpression.ReadOnly;
            set => txtExpression.ReadOnly = value;
        }

        public string Expression
        {
            get => txtExpression.Text;
            set => txtExpression.Text = value;
        }

        public override string Text
        {
            get => Expression;
            set => Expression = value;
        }

        private bool _IsPersistent = false;
        public bool IsPersistent
        {
            get => _IsPersistent;
            set
            {
                _IsPersistent = value;
                UpdateBackground();
            }
        }

        public bool AutocompleteAvailable { get; set; } = true;

        private bool _regexSupportsExpressions = true;
        [DefaultValue(true)]
        public bool RegexSupportsExpressions
        {
            get => _regexSupportsExpressions;
            set
            {
                if (_regexSupportsExpressions == value) return;
                _regexSupportsExpressions = value;
                ResetTooltip();
            }
        }

        private SupportedExpressionTypeEnum _ExpressionType;
        public SupportedExpressionTypeEnum ExpressionType
        {
            get => _ExpressionType;
            set
            {
                if (value != _ExpressionType)
                {
                    _ExpressionType = value;
                    ResetTooltip();
                }
            }
        }

        private void ResetTooltip()
        {
            switch (ExpressionType)
            {
                case SupportedExpressionTypeEnum.Numeric:
                    string temp = I18n.Translate("internal/ExpressionTextBox/numeric1", "This field supports numeric expressions; references to named regular expression groups will be expanded, after which the result will be evaluated as a mathematic expression.");
                    temp += Environment.NewLine;
                    temp += I18n.Translate("internal/ExpressionTextBox/numeric2", "The color of the field will also change depending on whether the numeric expression appears to be valid (green) or not (red).");
                    toolTipExpression.SetToolTip(pnlExpandToggle, temp);
                    break;
                case SupportedExpressionTypeEnum.String:
                    toolTipExpression.SetToolTip(pnlExpandToggle, I18n.Translate("internal/ExpressionTextBox/string", "This field supports string expressions; references to named regular expression groups will be expanded."));
                    break;
                case SupportedExpressionTypeEnum.Regex:
                    toolTipExpression.SetToolTip(pnlExpandToggle, I18n.Translate("internal/RegexTextBox", "This field supports regular expressions; the color of the field will change depending on whether the regular expression is valid (green) or not (red)."));
                    break;
            }
            UpdateBackground();
        }

        /// <summary> 
        /// For validating the expression with placeholder values based on ExpressionType. 
        /// Set background color based on the result.
        /// </summary>
        private static Context contextForValidation = new Context(null) { testByPlaceholder = true };

        private bool _triedToGetUiContext = false;
        private Context _uiContext;

        /// <summary> For autofilling. </summary>
        // to-do: replace the logic using the static RealPlugin instance
        internal Context UiContext
        {
            get
            {
                if (_uiContext != null)
                    return _uiContext;

                if (!_triedToGetUiContext)
                {
                    var form = this.FindForm();
                    if (form is TriggerForm tf)
                    {
                        _uiContext = tf.UiContext;
                    }
                    else if (form is ActionForm af)
                    {
                        _uiContext = af.UiContext;
                    }
                    _triedToGetUiContext = true;
                }

                return _uiContext ?? Context.Unbound;
            }
            set => _uiContext = value;
        }

        public delegate void EnterDelegate();
        public event EnterDelegate OnEnterKeyHit;
        public IButtonControl AcceptButton;
        public IButtonControl CancelButton;

        // unhide TextChanged on base class
        private EventHandler _TextChanged;
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new event EventHandler TextChanged
        {
            add
            {
                _TextChanged += value;
            }
            remove
            {
                _TextChanged -= value;
            }
        }

        public ExpressionTextBox()
        {
            InitializeComponent();
            ResetTooltip();
            txtExpression.TextChanged += txtExpression_TextChanged;
            txtExpression.KeyPress += txtExpression_KeyPress;
            txtExpression.KeyDown += txtExpression_KeyDown;
            txtExpression.MaxLength = 10000000; // for scripts
            txtExpression.GotFocus += ReplaceIncompleteLineBreaksInClipboard;
            Disposed += ExpressionTextBox_Disposed;
            Leave += ExpressionTextBox_Leave;
            LostFocus += ExpressionTextBox_LostFocus;
            acfDebounceTimer.Interval = 100; // debounce timer for autocomplete
            acfDebounceTimer.Tick += (sender, e) => ProcessAutocomplete();
        }

        public ExpressionTextBox(Context uiContext) : this()
        {
            UiContext = uiContext;
        }

        private void txtExpression_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtExpression.ReadOnly) return;
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                e.Handled = true;
                if (AutocompleteActive() == true)
                {
                    ApplyAutoComplete();
                }
                else if (OnEnterKeyHit != null)
                {
                    OnEnterKeyHit();
                }
                else
                {
                    if (!txtExpression.Multiline) // switch to multiline mode
                    {
                        ToggleExpand();
                    }
                    else // input linebreaks (+ indent), instead of closing the form
                    {
                        // Delete selected text, if any
                        if (txtExpression.SelectionLength > 0)
                            txtExpression.Paste("");

                        // Get the start index of the current line
                        int currentPosition = txtExpression.SelectionStart;
                        int currentLineIndex = txtExpression.GetLineFromCharIndex(currentPosition);
                        int lineStartIndex = txtExpression.GetFirstCharIndexFromLine(currentLineIndex);
                        if (lineStartIndex < 0) lineStartIndex = 0;

                        // Get indentation (leading spaces or full-width spaces) and add to the new line
                        int length = Math.Max(0, currentPosition - lineStartIndex);
                        string currentLineText = txtExpression.Text.Substring(lineStartIndex, length);
                        string indent = new string(currentLineText.TakeWhile(c => char.IsWhiteSpace(c)).ToArray());
                        txtExpression.Paste(Environment.NewLine + indent);
                    }
                }
            }
            else if (e.KeyChar == Convert.ToChar(Keys.Escape))
            {
                HideAutocomplete();
            }
            else if (char.IsControl(e.KeyChar) == true)
            {
                HideAutocomplete();
            }
        }

        private static Regex reCaptureGroups = new Regex(@"\$\{(?<capture>[\p{L}\d_]+?)\}");
        /// <summary> Check if the pure alphanumeric ${...} expressions are all capture groups or special variables (like _since) </summary>
        internal static bool CheckValidBasicExpression(string expression)
        {
            var matches = reCaptureGroups.Matches(expression);
            return matches.Cast<Match>().All(
                m => CurrentRegexGroupsAndPrefixes.Contains(m.Groups["capture"].Value)
                );
        }

        internal static Color BgRed    = Color.FromArgb(255, 225, 225);        // invalid expression
        internal static Color BgYellow = Color.FromArgb(255, 240, 210);        // capture group not found
        internal static Color BgGreen  = Color.FromArgb(225, 255, 225);        // correct
        internal static Color BgBlue   = Color.FromArgb(210, 240, 255);        // persistent variable
        
        private void UpdateBackground()
        {
            if (!Enabled && !IsPersistent)
            {
                txtExpression.BackColor = SystemColors.Window;
                txtExpression.ForeColor = SystemColors.WindowText;
                return;
            }
            if (ExpressionType == SupportedExpressionTypeEnum.Numeric)
            {
                if (txtExpression.Text.Length == 0)
                {
                    txtExpression.BackColor = SystemColors.Window;
                    return;
                }
                try
                {
                    contextForValidation.EvaluateNumericExpression(null, null, txtExpression.Text);
                    txtExpression.BackColor = BgGreen;
                    if (!CheckValidBasicExpression(Expression))
                    {
                        txtExpression.BackColor = BgYellow;
                    }
                }
                catch (Exception)
                {
                    txtExpression.BackColor = BgRed;
                }
            }
            else if (ExpressionType == SupportedExpressionTypeEnum.String)
            {
                if (!CheckValidBasicExpression(Expression))
                {
                    txtExpression.BackColor = BgYellow;
                }
                else if (IsPersistent)
                {
                    txtExpression.BackColor = BgBlue;
                }
                else if (txtExpression.BackColor != SystemColors.Window)
                {
                    txtExpression.BackColor = SystemColors.Window;
                }
            }
            else if (ExpressionType == SupportedExpressionTypeEnum.Regex)
            {
                if (txtExpression.Text.Length == 0)
                {
                    txtExpression.BackColor = IsPersistent ? BgBlue : SystemColors.Window;
                    return;
                }
                try
                {
                    string pattern = RegexSupportsExpressions
                        ? contextForValidation.ExpandVariables(null, null, false, txtExpression.Text)
                        : txtExpression.Text;
                    Regex rex = new Regex(pattern);
                    txtExpression.BackColor = IsPersistent ? BgBlue : BgGreen;
                }
                catch (Exception)
                {
                    txtExpression.BackColor = BgRed;
                }
            }
            else if (ExpressionType == SupportedExpressionTypeEnum.Color)
            {
                if (string.IsNullOrWhiteSpace(txtExpression.Text))
                {
                    txtExpression.BackColor = SystemColors.Window;
                    txtExpression.ForeColor = SystemColors.WindowText;
                }
                else
                {
                    Color color = Color.Empty;
                    try
                    {
                        string rawColor = contextForValidation.ExpandVariables(null, null, false, txtExpression.Text);
                        color = ParseColor(rawColor, Color.Empty);
                    }
                    catch { color = Color.Empty; }

                    if (color == Color.Empty)
                    {
                        txtExpression.BackColor = SystemColors.Window;
                        txtExpression.ForeColor = SystemColors.WindowText;
                    }
                    else
                    {
                        txtExpression.BackColor = color != Color.Transparent ? color : SystemColors.Window;
                        double brightness = 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
                        txtExpression.ForeColor = (brightness > 128) ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
                    }
                }
            }
        }

        private void txtExpression_TextChanged(object sender, EventArgs e)
        {
            UpdateBackground();
            if (txtExpression.Multiline) { MultiLineAdjustHeight(); }
            if (RealPlugin.Instance.Config.AutoComplete == true)
            {
                if (ExpressionType != SupportedExpressionTypeEnum.Regex && txtExpression.Focused)
                {
                    acfDebounceTimer.Stop();
                    acfDebounceTimer.Start();
                }
            }
            _TextChanged?.Invoke(this, EventArgs.Empty);
        }

        private void pnlExpandToggle_Click(object sender, EventArgs e)
        {
            ToggleExpand();
        }

        private void pnlExpandToggle_DoubleClick(object sender, EventArgs e)
        {
            ToggleExpand();
        }

        internal void ToggleExpand()
        {
            if (txtExpression.Multiline == true)
            {
                txtExpression.Multiline = false;
                txtExpression.MinimumSize = new Size(txtExpression.MinimumSize.Width, 0);
                txtExpression.ScrollBars = ScrollBars.None;
                (pnlExpandAlternateIcon.BackgroundImage, pnlExpandToggle.BackgroundImage) = (pnlExpandToggle.BackgroundImage, pnlExpandAlternateIcon.BackgroundImage);
            }
            else
            {
                txtExpression.Multiline = true;
                txtExpression.MinimumSize = new Size(txtExpression.MinimumSize.Width, 80);
                txtExpression.MaximumSize = new Size(txtExpression.MaximumSize.Width, txtExpression.Dock == DockStyle.Fill ? 0 : 400);
                txtExpression.ScrollBars = ScrollBars.Both;
                MultiLineAdjustHeight();
                (pnlExpandAlternateIcon.BackgroundImage, pnlExpandToggle.BackgroundImage) = (pnlExpandToggle.BackgroundImage, pnlExpandAlternateIcon.BackgroundImage);
            }
        }

        private void MultiLineAdjustHeight()
        {
            // A docked editor follows its window size rather than its text height.
            if (txtExpression.Dock == DockStyle.Fill) return;
            using (Graphics g = txtExpression.CreateGraphics())
            {
                SizeF size = g.MeasureString(txtExpression.Text + "\n\n1", txtExpression.Font); // add 2 more lines
                txtExpression.Height = (int)size.Height;
            }
        }

        // Prevent pasting a hidden \n (usually from ACT loglines) into the txtbox
        // Did not use WinProc since there are some regular TextBoxes in the forms besides ExpTxtBox.
        public static void ReplaceIncompleteLineBreaksInClipboard(object sender, EventArgs e)
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    string clipboardText = Clipboard.GetText();
                    Regex rexIncompleteLinebreaks = new Regex(@"\r(?!\n)|(?<!\r)\n");
                    if (rexIncompleteLinebreaks.IsMatch(clipboardText))
                    {
                        string replacedText = clipboardText.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
                        Clipboard.SetText(replacedText);
                    }
                }
            }
            catch { }
        }

        #region Color
        private static Regex regexHexColor = new Regex(@"^#? *(?<rgb>[\dA-Fa-f]{3}|[\dA-Fa-f]{6})$");
        private static Regex regexNumColor = new Regex(@"^(?<r>\d+(?:\.\d+)?) *, *(?<g>\d+(?:\.\d+)?) *, *(?<b>\d+(?:\.\d+)?)$");

        /// <summary>
        /// Parse a user-input raw color string to a <see cref="Color" />. <br /><br />
        /// If parsing fails, return the given default color. <br /><br />
        /// Input could be: <br /><br />
        /// · <see cref="Color" /> names: white <br />
        /// · RGB: 192, 0, 18 <br />
        /// · Hex value: #acf / #aaccff / acf / aaccff <br />
        /// </summary>
        /// <param name="defaultColor">The default color returned if the string is invalid.</param>
        /// <returns>The representing <see cref="Color" /></returns>
        public static Color ParseColor(string rawColor, Color defaultColor)
            => TryParseColor(rawColor) ?? defaultColor;

        /// <summary>
        /// Parse a user-input raw color string to a <see cref="Color" />. <br /><br />
        /// Input could be: <br /><br />
        /// · Color names: white <br />
        /// · RGB: 192, 0, 18 <br />
        /// · Hex value: #acf / #aaccff / acf / aaccff <br />
        /// </summary>
        /// <param name="defaultColor">The default <see cref="Color" /> returned if the string is invalid.</param>
        /// <returns>The representing <see cref="Color" /></returns>
        public static Color? TryParseColor(string rawColor)
        {
            rawColor = rawColor.Trim();

            Color namedColor = Color.FromName(rawColor);
            if (namedColor.IsKnownColor)
            {   // "white"
                return namedColor;
            }

            int r, g, b;
            Match hexMatch = regexHexColor.Match(rawColor);
            if (hexMatch.Success)
            {
                string rgb = hexMatch.Groups["rgb"].Value;
                if (rgb.Length == 3)
                {   // "#acf" or "acf"
                    rgb = string.Concat(rgb[0], rgb[0], rgb[1], rgb[1], rgb[2], rgb[2]);
                }
                // "#aaccff" or "aaccff"
                r = Convert.ToInt32(rgb.Substring(0, 2), 16);
                g = Convert.ToInt32(rgb.Substring(2, 2), 16);
                b = Convert.ToInt32(rgb.Substring(4, 2), 16);
            }
            else
            {   // "192, 0, 18"
                Match numMatch = regexNumColor.Match(rawColor);
                if (numMatch.Success)
                {
                    r = (int)Math.Round(double.Parse(numMatch.Groups["r"].Value, CultureInfo.InvariantCulture));
                    g = (int)Math.Round(double.Parse(numMatch.Groups["g"].Value, CultureInfo.InvariantCulture));
                    b = (int)Math.Round(double.Parse(numMatch.Groups["b"].Value, CultureInfo.InvariantCulture));
                }
                else return null;
            }

            return Color.FromArgb(r < 0 ? 0 : r > 255 ? 255 : r,
                                  g < 0 ? 0 : g > 255 ? 255 : g,
                                  b < 0 ? 0 : b > 255 ? 255 : b);
        }

        public static string ColorToString(Color color, Color? defaultColor = null)
        {
            if (color == Color.Empty || color == defaultColor)
            {
                return "";
            }
            else if (color.IsKnownColor)
            {
                return color.Name;
            }
            else
            {
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }
        }
        #endregion

    }
}
