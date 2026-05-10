using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using Triggernometry.Localization;

namespace Triggernometry.UI.Forms
{
    public partial class TraySlider : Form
    {
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private const int SW_SHOWNOACTIVATE = 4;
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOACTIVATE = 16u;

        private bool _activated;
        private bool _choiceClicked;
        private bool _hasMouse;
        

        private bool ForceShow { get; set; } = true;

        protected override bool ShowWithoutActivation => true;

        public TraySlider(int buttonCount, int durationMs = 15000, bool forceShow = true)
        {
            if (buttonCount > 3)
                throw new ArgumentOutOfRangeException(nameof(buttonCount), "Button count must be 0-3.");

            InitializeComponent();

            tmrFade.Interval = durationMs;
            ForceShow = forceShow;

            Button1.Text = I18n.Translate("internal/TraySlider/Button1", "Confirm");
            Button2.Text = I18n.Translate("internal/TraySlider/Button2", "Cancel");

            ApplyDefaultFont();
            ConfigureButtons(buttonCount);
            BindMouseEvents(this);

            var handle = Handle;
        }

        public void ShowTraySlider(string message, string title = "")
        {
            lblTitle.Text = title ?? "";
            rtbText.Text = message ?? "";
            rtbText.SelectionStart = 0;
            rtbText.SelectionLength = 0;
            _activated = true;
        }

        private void ApplyDefaultFont()
        {
            var textFont = new Font(SystemFonts.MessageBoxFont.FontFamily, 11f, FontStyle.Regular);
            var titleFont = new Font(SystemFonts.MessageBoxFont.FontFamily, 11f, FontStyle.Bold);

            lblTitle.Font = titleFont;
            rtbText.Font = textFont;
            Button1.Font = textFont;
            Button2.Font = textFont;
            Button3.Font = textFont;
        }

        private void ConfigureButtons(int buttonCount)
        {
            if (buttonCount > 3)
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

            if (IsForegroundFullScreen() && !ForceShow)
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
            if (!_hasMouse && (!IsForegroundFullScreen() || ForceShow))
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
}