using System;
using System.Drawing;
using System.Windows.Forms;
using Triggernometry.UI.CustomControls;
using Triggernometry.Localization;

namespace Triggernometry.UI.Forms
{
    public partial class SimpleInputForm : Form
    {
        private ExpressionTextBox expEditedValue;
        private Button okButton;
        private Button cancelButton;
        private TableLayoutPanel table;

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

        public SimpleInputForm(string title, ExpressionTextBox.SupportedExpressionTypeEnum exprType, string defaultValue = "", bool multiline = false)
        {
            Text = title;
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = true;
            MinimizeBox = false;
            MinimumSize = multiline ? new Size(480, 300) : new Size(360, 160);
            ClientSize = multiline ? new Size(900, 560) : new Size(480, 130);
            KeyPreview = true;
            KeyDown += SimpleInputForm_KeyDown;

            expEditedValue = new ExpressionTextBox
            {
                Dock = DockStyle.Fill,
                ExpressionType = exprType,
                Text = defaultValue
            };
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

            okButton = new Button
            {
                Text = I18n.Translate("internal/SimpleInputForm/ok", "OK"),
                AutoSize = true,
                DialogResult = DialogResult.OK
            };
            cancelButton = new Button
            {
                Text = I18n.Translate("internal/SimpleInputForm/cancel", "Cancel"),
                AutoSize = true,
                DialogResult = DialogResult.Cancel
            };
            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
                WrapContents = false,
                Padding = new Padding(0, 10, 0, 0)
            };
            buttons.Controls.Add(cancelButton);
            buttons.Controls.Add(okButton);
            // Enter inserts a newline in the expanded editor; Ctrl+Enter accepts either mode.
            if (!multiline) AcceptButton = okButton;
            CancelButton = cancelButton;

            table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15),
                ColumnCount = 1,
                RowCount = 2
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.Controls.Add(expEditedValue, 0, 0);
            table.Controls.Add(buttons, 0, 1);
            Controls.Add(table);

            Shown += (_, __) =>
            {
                expEditedValue.textBox1.Focus();
                Activate();
            };
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
