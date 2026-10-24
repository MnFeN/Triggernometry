namespace Triggernometry.UI.Forms
{
    partial class SimpleInputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.expEditedValue = new Triggernometry.UI.CustomControls.ExpressionTextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.buttons = new System.Windows.Forms.FlowLayoutPanel();
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.buttons.SuspendLayout();
            this.table.SuspendLayout();
            this.SuspendLayout();
            //
            // expEditedValue
            //
            this.expEditedValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.expEditedValue.Location = new System.Drawing.Point(18, 18);
            this.expEditedValue.Name = "expEditedValue";
            this.expEditedValue.Size = new System.Drawing.Size(444, 48);
            this.expEditedValue.TabIndex = 0;
            //
            // okButton
            //
            this.okButton.AutoSize = true;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(285, 13);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 24);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            //
            // cancelButton
            //
            this.cancelButton.AutoSize = true;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(366, 13);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 24);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            //
            // buttons
            //
            this.buttons.AutoSize = true;
            this.buttons.Controls.Add(this.cancelButton);
            this.buttons.Controls.Add(this.okButton);
            this.buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttons.Location = new System.Drawing.Point(18, 72);
            this.buttons.Name = "buttons";
            this.buttons.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.buttons.Size = new System.Drawing.Size(444, 40);
            this.buttons.TabIndex = 1;
            this.buttons.WrapContents = false;
            //
            // table
            //
            this.table.ColumnCount = 1;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.Controls.Add(this.expEditedValue, 0, 0);
            this.table.Controls.Add(this.buttons, 0, 1);
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.Name = "table";
            this.table.Padding = new System.Windows.Forms.Padding(15);
            this.table.RowCount = 2;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table.Size = new System.Drawing.Size(480, 130);
            this.table.TabIndex = 0;
            //
            // SimpleInputForm
            //
            this.AcceptButton = this.okButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(480, 130);
            this.Controls.Add(this.table);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(360, 160);
            this.Name = "SimpleInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Disposed += new System.EventHandler(this.SimpleInputForm_Disposed);
            this.Shown += new System.EventHandler(this.SimpleInputForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SimpleInputForm_KeyDown);
            this.buttons.ResumeLayout(false);
            this.buttons.PerformLayout();
            this.table.ResumeLayout(false);
            this.table.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Triggernometry.UI.CustomControls.ExpressionTextBox expEditedValue;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.FlowLayoutPanel buttons;
        private System.Windows.Forms.TableLayoutPanel table;
    }
}
