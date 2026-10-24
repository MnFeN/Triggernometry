using System;
using System.Drawing;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.UI.CustomControls;
using Triggernometry.Localization;

namespace Triggernometry.UI.Forms
{
    public partial class SimpleInputForm : Form
    {
        private Font fontOverride;

        public bool ReadOnly
        {
            get => expEditedValue.ReadOnly;
            set
            {
                expEditedValue.ReadOnly = value;
                okButton.Text = value ? I18n.Translate("internal/SimpleInputForm/close", "Close")
                    : I18n.Translate("internal/SimpleInputForm/ok", "OK");
                cancelButton.Visible = !value;
            }
        }

        public SimpleInputForm()
        {
            InitializeComponent();
        }

        public SimpleInputForm(string title, ExpressionTextBox.SupportedExpressionTypeEnum exprType, string defaultValue = "", bool multiline = false) : this()
        {
            Text = title;
            MinimumSize = multiline ? new Size(480, 300) : new Size(360, 160);
            ClientSize = multiline ? new Size(900, 560) : new Size(480, 130);
            okButton.Text = I18n.Translate("internal/SimpleInputForm/ok", "OK");
            cancelButton.Text = I18n.Translate("internal/SimpleInputForm/cancel", "Cancel");
            Configuration cfg = RealPlugin.Instance.cfg;
            if (!cfg.UiFontDefault)
            {
                fontOverride = RealPlugin.CreateFontFromDefinition(cfg.UiFontName, cfg.UiFontSize, cfg.UiFontEffect);
                RealPlugin.ApplyFontOverrideToForm(this, fontOverride);
            }
            expEditedValue.ExpressionType = exprType;
            expEditedValue.Text = defaultValue;
            expEditedValue.textBox1.MinimumSize = new Size(200, 0);
            if (multiline)
            {
                expEditedValue.textBox1.Dock = DockStyle.Fill;
                expEditedValue.textBox1.AcceptsReturn = true;
                expEditedValue.textBox1.AcceptsTab = true;
                expEditedValue.textBox1.MaxLength = 0;
                expEditedValue.textBox1.WordWrap = false;
                expEditedValue.ToggleExpand();
            }

            // Enter inserts a newline in the expanded editor; Ctrl+Enter accepts either mode.
            if (multiline) AcceptButton = null;
        }

        private void SimpleInputForm_Shown(object sender, EventArgs e)
        {
            expEditedValue.textBox1.Focus();
            Activate();
        }

        private void SimpleInputForm_Disposed(object sender, EventArgs e)
        {
            fontOverride?.Dispose();
        }

        private void SimpleInputForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != (Keys.Control | Keys.Enter)) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        public string GetInput(IWin32Window owner = null)
        {
            return ShowDialog(owner) == DialogResult.OK ? expEditedValue.Text : null;
        }
    }
}
