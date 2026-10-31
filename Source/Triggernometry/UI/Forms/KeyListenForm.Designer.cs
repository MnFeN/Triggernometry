namespace Triggernometry.UI.Forms
{
    partial class KeyListenForm
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
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlButtonsSpacing = new System.Windows.Forms.Panel();
            this.pnlKeypressHint = new System.Windows.Forms.Panel();
            this.lblKeypressInfo = new System.Windows.Forms.Label();
            this.lblKeypress = new System.Windows.Forms.Label();
            this.pnlButtons.SuspendLayout();
            this.pnlKeypressHint.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOk);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(10, 175);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(464, 35);
            this.pnlButtons.TabIndex = 13;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(314, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 35);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOk.Location = new System.Drawing.Point(0, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 35);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // pnlButtonsSpacing
            // 
            this.pnlButtonsSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonsSpacing.Location = new System.Drawing.Point(10, 165);
            this.pnlButtonsSpacing.Name = "pnlButtonsSpacing";
            this.pnlButtonsSpacing.Size = new System.Drawing.Size(464, 10);
            this.pnlButtonsSpacing.TabIndex = 14;
            // 
            // pnlKeypressHint
            // 
            this.pnlKeypressHint.BackColor = System.Drawing.SystemColors.Control;
            this.pnlKeypressHint.Controls.Add(this.lblKeypressInfo);
            this.pnlKeypressHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKeypressHint.Location = new System.Drawing.Point(10, 10);
            this.pnlKeypressHint.Name = "pnlKeypressHint";
            this.pnlKeypressHint.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlKeypressHint.Size = new System.Drawing.Size(464, 51);
            this.pnlKeypressHint.TabIndex = 18;
            // 
            // lblKeypressInfo
            // 
            this.lblKeypressInfo.AutoEllipsis = true;
            this.lblKeypressInfo.BackColor = System.Drawing.SystemColors.Info;
            this.lblKeypressInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblKeypressInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblKeypressInfo.Location = new System.Drawing.Point(0, 0);
            this.lblKeypressInfo.Name = "lblKeypressInfo";
            this.lblKeypressInfo.Size = new System.Drawing.Size(464, 41);
            this.lblKeypressInfo.TabIndex = 0;
            this.lblKeypressInfo.Text = "Press a key or a combination of keys, and press OK when you are satisfied with th" +
    "e result.";
            this.lblKeypressInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblKeypress
            // 
            this.lblKeypress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblKeypress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblKeypress.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeypress.Location = new System.Drawing.Point(10, 61);
            this.lblKeypress.Name = "lblKeypress";
            this.lblKeypress.Size = new System.Drawing.Size(464, 104);
            this.lblKeypress.TabIndex = 19;
            this.lblKeypress.Text = "(no keypress recorded)";
            this.lblKeypress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KeyListenForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(484, 220);
            this.Controls.Add(this.lblKeypress);
            this.Controls.Add(this.pnlButtonsSpacing);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlKeypressHint);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KeyListenForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Listening to keypresses";
            this.pnlButtons.ResumeLayout(false);
            this.pnlKeypressHint.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel pnlButtonsSpacing;
        private System.Windows.Forms.Panel pnlKeypressHint;
        private System.Windows.Forms.Label lblKeypressInfo;
        private System.Windows.Forms.Label lblKeypress;
    }
}