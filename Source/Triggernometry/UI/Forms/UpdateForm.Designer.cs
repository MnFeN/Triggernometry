namespace Triggernometry.UI.Forms
{
    partial class UpdateForm
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
            this.grpPatchDetails = new System.Windows.Forms.GroupBox();
            this.txtPatchDetails = new System.Windows.Forms.TextBox();
            this.pnlButtonsSpacing = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.tableVersions = new System.Windows.Forms.TableLayoutPanel();
            this.capYourVersion = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.capLatestVersion = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.txtYourVersion = new System.Windows.Forms.TextBox();
            this.txtLatestVersion = new System.Windows.Forms.TextBox();
            this.pnlVersionDetailsSpacing = new System.Windows.Forms.Panel();
            this.grpUpdateDetails = new System.Windows.Forms.GroupBox();
            this.txtUpdateDetails = new System.Windows.Forms.TextBox();
            this.pnlPatchDetailsSpacing = new System.Windows.Forms.Panel();
            this.stsUpdateProgress = new System.Windows.Forms.StatusStrip();
            this.prgProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpPatchDetails.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableVersions.SuspendLayout();
            this.grpUpdateDetails.SuspendLayout();
            this.stsUpdateProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPatchDetails
            // 
            this.grpPatchDetails.Controls.Add(this.txtPatchDetails);
            this.grpPatchDetails.Controls.Add(this.stsUpdateProgress);
            this.grpPatchDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPatchDetails.Location = new System.Drawing.Point(10, 253);
            this.grpPatchDetails.Name = "grpPatchDetails";
            this.grpPatchDetails.Padding = new System.Windows.Forms.Padding(10);
            this.grpPatchDetails.Size = new System.Drawing.Size(741, 193);
            this.grpPatchDetails.TabIndex = 24;
            this.grpPatchDetails.TabStop = false;
            this.grpPatchDetails.Text = " Update log ";
            // 
            // txtPatchDetails
            // 
            this.txtPatchDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPatchDetails.Location = new System.Drawing.Point(10, 23);
            this.txtPatchDetails.Multiline = true;
            this.txtPatchDetails.Name = "txtPatchDetails";
            this.txtPatchDetails.ReadOnly = true;
            this.txtPatchDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPatchDetails.Size = new System.Drawing.Size(721, 138);
            this.txtPatchDetails.TabIndex = 0;
            // 
            // pnlButtonsSpacing
            // 
            this.pnlButtonsSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonsSpacing.Location = new System.Drawing.Point(10, 446);
            this.pnlButtonsSpacing.Name = "pnlButtonsSpacing";
            this.pnlButtonsSpacing.Size = new System.Drawing.Size(741, 10);
            this.pnlButtonsSpacing.TabIndex = 22;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnUpdate);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(10, 456);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(741, 35);
            this.pnlButtons.TabIndex = 23;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(591, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 35);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(0, 0);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(150, 35);
            this.btnUpdate.TabIndex = 0;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // tableVersions
            // 
            this.tableVersions.AutoSize = true;
            this.tableVersions.ColumnCount = 2;
            this.tableVersions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableVersions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableVersions.Controls.Add(this.txtLatestVersion, 1, 1);
            this.tableVersions.Controls.Add(this.capLatestVersion, 1, 0);
            this.tableVersions.Controls.Add(this.capYourVersion, 0, 0);
            this.tableVersions.Controls.Add(this.txtYourVersion, 0, 1);
            this.tableVersions.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableVersions.Location = new System.Drawing.Point(10, 10);
            this.tableVersions.Name = "tableVersions";
            this.tableVersions.RowCount = 2;
            this.tableVersions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableVersions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableVersions.Size = new System.Drawing.Size(741, 73);
            this.tableVersions.TabIndex = 25;
            // 
            // capYourVersion
            // 
            this.capYourVersion.Caption = "Currently installed";
            this.capYourVersion.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capYourVersion.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capYourVersion.Location = new System.Drawing.Point(3, 3);
            this.capYourVersion.Name = "capYourVersion";
            this.capYourVersion.Size = new System.Drawing.Size(364, 30);
            this.capYourVersion.TabIndex = 0;
            // 
            // capLatestVersion
            // 
            this.capLatestVersion.Caption = "Latest version";
            this.capLatestVersion.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capLatestVersion.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capLatestVersion.Location = new System.Drawing.Point(373, 3);
            this.capLatestVersion.Name = "capLatestVersion";
            this.capLatestVersion.Size = new System.Drawing.Size(365, 30);
            this.capLatestVersion.TabIndex = 1;
            // 
            // txtYourVersion
            // 
            this.txtYourVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtYourVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtYourVersion.Location = new System.Drawing.Point(3, 39);
            this.txtYourVersion.Name = "txtYourVersion";
            this.txtYourVersion.ReadOnly = true;
            this.txtYourVersion.Size = new System.Drawing.Size(364, 31);
            this.txtYourVersion.TabIndex = 2;
            // 
            // txtLatestVersion
            // 
            this.txtLatestVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLatestVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLatestVersion.Location = new System.Drawing.Point(373, 39);
            this.txtLatestVersion.Name = "txtLatestVersion";
            this.txtLatestVersion.ReadOnly = true;
            this.txtLatestVersion.Size = new System.Drawing.Size(365, 31);
            this.txtLatestVersion.TabIndex = 3;
            // 
            // pnlVersionDetailsSpacing
            // 
            this.pnlVersionDetailsSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlVersionDetailsSpacing.Location = new System.Drawing.Point(10, 83);
            this.pnlVersionDetailsSpacing.Name = "pnlVersionDetailsSpacing";
            this.pnlVersionDetailsSpacing.Size = new System.Drawing.Size(741, 10);
            this.pnlVersionDetailsSpacing.TabIndex = 26;
            // 
            // grpUpdateDetails
            // 
            this.grpUpdateDetails.Controls.Add(this.txtUpdateDetails);
            this.grpUpdateDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpUpdateDetails.Location = new System.Drawing.Point(10, 93);
            this.grpUpdateDetails.Name = "grpUpdateDetails";
            this.grpUpdateDetails.Padding = new System.Windows.Forms.Padding(10);
            this.grpUpdateDetails.Size = new System.Drawing.Size(741, 150);
            this.grpUpdateDetails.TabIndex = 27;
            this.grpUpdateDetails.TabStop = false;
            this.grpUpdateDetails.Text = " Update details ";
            // 
            // txtUpdateDetails
            // 
            this.txtUpdateDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUpdateDetails.Location = new System.Drawing.Point(10, 23);
            this.txtUpdateDetails.Multiline = true;
            this.txtUpdateDetails.Name = "txtUpdateDetails";
            this.txtUpdateDetails.ReadOnly = true;
            this.txtUpdateDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUpdateDetails.Size = new System.Drawing.Size(721, 117);
            this.txtUpdateDetails.TabIndex = 0;
            // 
            // pnlPatchDetailsSpacing
            // 
            this.pnlPatchDetailsSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPatchDetailsSpacing.Location = new System.Drawing.Point(10, 243);
            this.pnlPatchDetailsSpacing.Name = "pnlPatchDetailsSpacing";
            this.pnlPatchDetailsSpacing.Size = new System.Drawing.Size(741, 10);
            this.pnlPatchDetailsSpacing.TabIndex = 28;
            // 
            // stsUpdateProgress
            // 
            this.stsUpdateProgress.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prgProgress,
            this.lblStatus});
            this.stsUpdateProgress.Location = new System.Drawing.Point(10, 161);
            this.stsUpdateProgress.Name = "stsUpdateProgress";
            this.stsUpdateProgress.Size = new System.Drawing.Size(721, 22);
            this.stsUpdateProgress.SizingGrip = false;
            this.stsUpdateProgress.TabIndex = 1;
            this.stsUpdateProgress.Text = "stsUpdateProgress";
            // 
            // prgProgress
            // 
            this.prgProgress.Name = "prgProgress";
            this.prgProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(74, 17);
            this.lblStatus.Text = "Please wait...";
            // 
            // UpdateForm
            // 
            this.AcceptButton = this.btnUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 501);
            this.Controls.Add(this.grpPatchDetails);
            this.Controls.Add(this.pnlPatchDetailsSpacing);
            this.Controls.Add(this.grpUpdateDetails);
            this.Controls.Add(this.pnlVersionDetailsSpacing);
            this.Controls.Add(this.tableVersions);
            this.Controls.Add(this.pnlButtonsSpacing);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "UpdateForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Triggernometry Updater";
            this.TopMost = true;
            this.grpPatchDetails.ResumeLayout(false);
            this.grpPatchDetails.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.tableVersions.ResumeLayout(false);
            this.tableVersions.PerformLayout();
            this.grpUpdateDetails.ResumeLayout(false);
            this.grpUpdateDetails.PerformLayout();
            this.stsUpdateProgress.ResumeLayout(false);
            this.stsUpdateProgress.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPatchDetails;
        private System.Windows.Forms.TextBox txtPatchDetails;
        private System.Windows.Forms.Panel pnlButtonsSpacing;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.TableLayoutPanel tableVersions;
        private CustomControls.PrettyCaption capYourVersion;
        private System.Windows.Forms.StatusStrip stsUpdateProgress;
        private System.Windows.Forms.TextBox txtLatestVersion;
        private CustomControls.PrettyCaption capLatestVersion;
        private System.Windows.Forms.TextBox txtYourVersion;
        private System.Windows.Forms.Panel pnlVersionDetailsSpacing;
        private System.Windows.Forms.GroupBox grpUpdateDetails;
        private System.Windows.Forms.TextBox txtUpdateDetails;
        private System.Windows.Forms.Panel pnlPatchDetailsSpacing;
        private System.Windows.Forms.ToolStripProgressBar prgProgress;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}