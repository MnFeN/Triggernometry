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
            Configuration cfg = RealPlugin.Instance.Config;
            if (!cfg.UiFontDefault)
            {
                fontOverride = RealPlugin.CreateFontFromDefinition(cfg.UiFontName, cfg.UiFontSize, cfg.UiFontEffect);
                RealPlugin.ApplyFontOverrideToForm(this, fontOverride);
            }
            expEditedValue.ExpressionType = exprType;
            expEditedValue.Text = defaultValue;
            expEditedValue.txtExpression.MinimumSize = new Size(200, 0);
            if (multiline)
            {
                expEditedValue.txtExpression.Dock = DockStyle.Fill;
                expEditedValue.txtExpression.AcceptsReturn = true;
                expEditedValue.txtExpression.AcceptsTab = true;
                expEditedValue.txtExpression.MaxLength = 0;
                expEditedValue.txtExpression.WordWrap = false;
                expEditedValue.ToggleExpand();
            }

            // Enter inserts a newline in the expanded editor; Ctrl+Enter accepts either mode.
            if (multiline) AcceptButton = null;
        }

        private void SimpleInputForm_Shown(object sender, EventArgs e)
        {
            expEditedValue.txtExpression.Focus();
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

        /// <summary>创建输入窗口并返回结果，取消返回 null；窗口在取值后自动释放。</summary>
        public static string GetInput(string title, ExpressionTextBox.SupportedExpressionTypeEnum exprType,
            string defaultValue = "", bool multiline = false, IWin32Window owner = null)
        {
            using (var form = new SimpleInputForm(title, exprType, defaultValue, multiline))
                return form.GetInput(owner);
        }

        /// <summary>显示当前窗口并返回输入，取消返回 null；由调用方负责释放窗口。</summary>
        public string GetInput(IWin32Window owner = null)
        {
            return ShowDialog(owner) == DialogResult.OK ? expEditedValue.Text : null;
        }
    }
}
