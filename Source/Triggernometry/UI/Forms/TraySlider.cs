using System;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.Localization;

namespace Triggernometry.UI.Forms
{
    internal partial class TraySliderForm : Form
    {
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        internal Action OnClick1 { get; set; }
        internal Action OnClick2 { get; set; }
        internal Action OnClick3 { get; set; }

        private SystemSound _sound;

        private const int SW_SHOWNOACTIVATE = 4;
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOACTIVATE = 16u;

        private bool _activated;
        private bool _choiceClicked;
        private bool _hasMouse;
        private bool _forceShow;

        protected override bool ShowWithoutActivation => true;

        internal TraySliderForm(int buttonCount, int durationMs, bool forceShow)
        {
            if (buttonCount > 3 || buttonCount < 0)
                throw new ArgumentOutOfRangeException(nameof(buttonCount), "Button count must be 0-3.");

            InitializeComponent();

            tmrFade.Interval = durationMs;
            _forceShow = forceShow;

            Button1.Text = I18n.Translate("internal/TraySlider/Button1", "Confirm");
            Button2.Text = I18n.Translate("internal/TraySlider/Button2", "Cancel");

            ApplyDefaultFont();
            ConfigureButtons(buttonCount);
            BindMouseEvents(this);

            _ = Handle; // Force handle creation to avoid issues with showing the form before it's fully initialized.
        }

        internal void ShowTraySlider(string message, string title = "")
        {
            lblTitle.Text = title ?? "";
            rtbText.Text = message ?? "";
            rtbText.SelectionStart = 0;
            rtbText.SelectionLength = 0;
            _activated = true;
            _sound?.Play();
        }

        private void ApplyDefaultFont()
        {
            var btnfont = new Font(SystemFonts.MessageBoxFont.FontFamily, 9f, FontStyle.Regular);
            var textFont = new Font(SystemFonts.MessageBoxFont.FontFamily, 10f, FontStyle.Regular);
            var titleFont = new Font(SystemFonts.MessageBoxFont.FontFamily, 10f, FontStyle.Bold);

            lblTitle.Font = titleFont;
            rtbText.Font = textFont;
            Button1.Font = btnfont;
            Button2.Font = btnfont;
            Button3.Font = btnfont;
        }

        private void ConfigureButtons(int buttonCount)
        {
            if (buttonCount > 3 || buttonCount < 0)
                throw new ArgumentOutOfRangeException(nameof(buttonCount), "Button count must be 0-3.");

            var buttons = new[] { Button1, Button2, Button3 };

            tlpButtons.SuspendLayout();

            tlpButtons.Controls.Clear();
            tlpButtons.ColumnStyles.Clear();
            tlpButtons.ColumnCount = buttonCount;

            for (int i = 0; i < buttons.Length; i++)
                buttons[i].Visible = i < buttonCount;

            for (int i = 0; i < buttonCount; i++)
            {
                tlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / buttonCount));
                buttons[i].Dock = DockStyle.Fill;
                tlpButtons.Controls.Add(buttons[i], i, 0);
            }

            tlpButtons.ResumeLayout(true);
        }

        internal void SetLevel(TraySliderLevel level)
        {
            Color titleBgColor;
            switch (level)
            {
                case TraySliderLevel.Info:
                    titleBgColor = Color.FromArgb(135, 180, 225);
                    _sound = null;
                    break;
                case TraySliderLevel.Warning:
                    titleBgColor = Color.FromArgb(235, 200, 135);
                    _sound = SystemSounds.Exclamation;
                    break;
                case TraySliderLevel.Error:
                    titleBgColor = Color.FromArgb(225, 150, 150);
                    _sound = SystemSounds.Asterisk;
                    break;
                default:
                    return;
            }
            var formBgColor = Color.FromArgb(
                (titleBgColor.R + 7 * 255) / 8,
                (titleBgColor.G + 7 * 255) / 8,
                (titleBgColor.B + 7 * 255) / 8
            );
            var min = Math.Min(titleBgColor.R, Math.Min(titleBgColor.G, titleBgColor.B));
            var titleForeColor = Color.FromArgb(
                (titleBgColor.R - min) / 2,
                (titleBgColor.G - min) / 2,
                (titleBgColor.B - min) / 2
            );

            BackColor = formBgColor;
            tlpMain.BackColor = formBgColor;
            rtbText.BackColor = formBgColor;
            lblTitle.BackColor = titleBgColor;
            lblTitle.ForeColor = titleForeColor;
        }

        private void BindMouseEvents(Control control)
        {
            control.MouseEnter += TraySlider_MouseEnter;
            control.MouseLeave += TraySlider_MouseLeave;

            foreach (Control child in control.Controls)
                BindMouseEvents(child);
        }

        private void tmrShow_Tick(object sender, EventArgs e)
        {
            if (!_activated)
                return;

            if (IsForegroundFullScreen() && !_forceShow)
                return;

            _activated = false;
            _choiceClicked = false;

            Button1.Enabled = true;
            Button2.Enabled = true;
            Button3.Enabled = true;

            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;

            Location = new Point(workingArea.Right - Width, workingArea.Bottom);
            _targetTop = workingArea.Bottom - Height;

            Opacity = 0.0;
            ShowInactiveTopmost(this);

            _slideStartTop = Top;
            _slideStartTick = Environment.TickCount;
            tmrSlideIn.Start();

            tmrFade.Start();
        }

        private int _targetTop;
        private int _slideStartTop;
        private int _slideStartTick;
        private const int SlideDurationMs = 600;
        private void tmrSlideIn_Tick(object sender, EventArgs e)
        {
            double t = (Environment.TickCount - _slideStartTick) / (double)SlideDurationMs;

            if (t >= 1.0)
            {
                Top = _targetTop;
                Opacity = 1.0;
                tmrSlideIn.Stop();
                return;
            }

            if (t < 0.0)
                t = 0.0;

            double eased = t * t * (3.0 - 2.0 * t);
            Top = _slideStartTop + (int)Math.Round((_targetTop - _slideStartTop) * eased);

            double fadeInT = Math.Min(1.0, t / 0.25);
            Opacity = fadeInT;
        }

        private void tmrFade_Tick(object sender, EventArgs e)
        {
            if (!_hasMouse && (!IsForegroundFullScreen() || _forceShow))
                FadeOut();
        }

        private int _fadeOutStep;
        private void FadeOut()
        {
            Button1.Enabled = false;
            Button2.Enabled = false;
            Button3.Enabled = false;

            tmrFade.Stop();
            tmrSlideIn.Stop();

            _fadeOutStep = 0;
            tmrFadeOut.Start();
        }

        private void tmrFadeOut_Tick(object sender, EventArgs e)
        {
            _fadeOutStep++;

            Opacity = Math.Max(0.0, 1.0 - _fadeOutStep * 0.1);

            if (Opacity > 0.0)
                return;

            tmrFadeOut.Stop();
            Hide();

            Button1.Enabled = true;
            Button2.Enabled = true;
            Button3.Enabled = true;

            if (_choiceClicked)
                Dispose();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            _choiceClicked = true;
            FadeOut();

            try
            {
                if (sender == Button1)
                    OnClick1?.Invoke();
                else if (sender == Button2)
                    OnClick2?.Invoke();
                else if (sender == Button3)
                    OnClick3?.Invoke();
            }
            catch (Exception ex)
            {
                RealPlugin.Instance.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Error, ex.Message);
            }
        }

        private void TraySlider_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void TraySlider_MouseEnter(object sender, EventArgs e)
        {
            _hasMouse = true;
        }

        private void TraySlider_MouseLeave(object sender, EventArgs e)
        {
            _hasMouse = false;
        }

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(HandleRef hWnd, ref RECT rect);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private static void ShowInactiveTopmost(Form form)
        {
            ShowWindow(form.Handle, SW_SHOWNOACTIVATE);
            SetWindowPos(form.Handle, HWND_TOPMOST, form.Left, form.Top, form.Width, form.Height, SWP_NOACTIVATE);
        }

        private static bool IsForegroundFullScreen()
        {
            Screen screen = Screen.PrimaryScreen;

            RECT rect = new RECT();
            GetWindowRect(new HandleRef(null, GetForegroundWindow()), ref rect);

            Rectangle foreground = new Rectangle(
                rect.left,
                rect.top,
                rect.right - rect.left,
                rect.bottom - rect.top);

            return foreground.Contains(screen.Bounds);
        }
    }

    public enum TraySliderLevel
    {
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// Wrapper class for thread-safe showing a tray slider notification.
    /// </summary>
    public class TraySlider
    {
        public string Message { get; set; }
        public string Title { get; set; } = "";
        public int ButtonCount { get; set; } = 0;
        public int DurationMs { get; set; } = 15000;
        public bool ForceShow { get; set; } = true;
        public TraySliderLevel Level { get; set; } = TraySliderLevel.Info;

        public string Button1Text { get; set; }
        public string Button2Text { get; set; }
        public string Button3Text { get; set; }

        public Action OnClick1 { get; set; }
        public Action OnClick2 { get; set; }
        public Action OnClick3 { get; set; }

        public TraySlider(int buttonCount, string message, string title = "", TraySliderLevel level = TraySliderLevel.Info, int durationMs = 15000, bool forceShow = true)
        {
            if (buttonCount > 3)
                buttonCount = 3;
            if (buttonCount < 0)
                buttonCount = 0;
            Message = message;
            Title = title;
            Level = level;
            ButtonCount = buttonCount;
            DurationMs = durationMs;
            ForceShow = forceShow;
        }

        /// <summary> Create a <see cref="TraySliderLevel.Info"/> level <see cref="TraySlider"/> instance.</summary>
        public static TraySlider Info(int buttonCount, string message, string title = "", int durationMs = 15000, bool forceShow = true)
            => new TraySlider(buttonCount, message, title, TraySliderLevel.Info, durationMs, forceShow);

        /// <summary> Create a <see cref="TraySliderLevel.Warning"/> level <see cref="TraySlider"/> instance.</summary>
        public static TraySlider Warning(int buttonCount, string message, string title = "", int durationMs = 15000, bool forceShow = true)
            => new TraySlider(buttonCount, message, title, TraySliderLevel.Warning, durationMs, forceShow);

        /// <summary> Create a <see cref="TraySliderLevel.Error"/> level <see cref="TraySlider"/> instance.</summary>
        public static TraySlider Error(int buttonCount, string message, string title = "", int durationMs = 15000, bool forceShow = true)
            => new TraySlider(buttonCount, message, title, TraySliderLevel.Error, durationMs, forceShow);

        /// <summary> Thread-safe create and show a <see cref="TraySliderForm"/> from the <see cref="TraySlider"/> info. </summary>
        public void Show()
        {
            var mainForm = Application.OpenForms
                .Cast<Form>()
                .OrderByDescending(f => f.GetType().FullName == "Advanced_Combat_Tracker.FormActMain")
                .FirstOrDefault()
                ?? throw new InvalidOperationException("No UI form was created.");

            Action show = () =>
            {
                var form = new TraySliderForm(ButtonCount, DurationMs, ForceShow)
                {
                    OnClick1 = OnClick1,
                    OnClick2 = OnClick2,
                    OnClick3 = OnClick3,
                };

                form.SetLevel(Level);

                if (Button1Text != null)
                    form.Button1.Text = Button1Text;
                if (Button2Text != null)
                    form.Button2.Text = Button2Text;
                if (Button3Text != null)
                    form.Button3.Text = Button3Text;

                form.ShowTraySlider(Message, Title);
            };

            if (mainForm.InvokeRequired)
                mainForm.BeginInvoke(show);
            else
                show();
        }

        internal static void CallbackInfo(object _, string s)
        { 
            var (title, message) = ParseTitleAndMessage(s);
            Info(1, message, title).Show();
        }

        internal static void CallbackWarning(object _, string s)
        {
            var (title, message) = ParseTitleAndMessage(s);
            Warning(1, message, title).Show();
        }

        internal static void CallbackError(object _, string s)
        {
            var (title, message) = ParseTitleAndMessage(s);
            Error(1, message, title).Show();
        }

        private static (string, string) ParseTitleAndMessage(string s)
        {
            var parts = (s ?? "").Split(new[] { "\r\n", "\r", "\n" }, 2, StringSplitOptions.None);
            var title = parts.Length > 1 ? parts[0] : "";
            var message = parts.Length > 1 ? parts[1] : parts[0];
            return (title, message);
        }

    }
}