namespace Triggernometry.UI.Forms
{
    partial class RepositoryListForm
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
            this.trvRepositories = new Triggernometry.UI.CustomControls.TreeViewEx();
            this.pnlButtonsSpacing = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.grpAvailRepos = new System.Windows.Forms.GroupBox();
            this.stsRepositoryLoading = new System.Windows.Forms.StatusStrip();
            this.prgProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpRepoDetails = new System.Windows.Forms.GroupBox();
            this.txtRepoDetails = new System.Windows.Forms.TextBox();
            this.pnlRepositoryDetailsSpacing = new System.Windows.Forms.Panel();
            this.pnlButtons.SuspendLayout();
            this.grpAvailRepos.SuspendLayout();
            this.stsRepositoryLoading.SuspendLayout();
            this.grpRepoDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // trvRepositories
            // 
            this.trvRepositories.CheckBoxes = true;
            this.trvRepositories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvRepositories.Enabled = false;
            this.trvRepositories.Location = new System.Drawing.Point(10, 23);
            this.trvRepositories.Name = "trvRepositories";
            this.trvRepositories.Size = new System.Drawing.Size(544, 258);
            this.trvRepositories.TabIndex = 0;
            this.trvRepositories.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.trvRepositories_AfterCheck);
            this.trvRepositories.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvRepositories_BeforeSelect);
            // 
            // pnlButtonsSpacing
            // 
            this.pnlButtonsSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonsSpacing.Location = new System.Drawing.Point(10, 406);
            this.pnlButtonsSpacing.Name = "pnlButtonsSpacing";
            this.pnlButtonsSpacing.Size = new System.Drawing.Size(564, 10);
            this.pnlButtonsSpacing.TabIndex = 18;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOk);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(10, 416);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(564, 35);
            this.pnlButtons.TabIndex = 19;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(414, 0);
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
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(0, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 35);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Add selected";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // grpAvailRepos
            // 
            this.grpAvailRepos.Controls.Add(this.trvRepositories);
            this.grpAvailRepos.Controls.Add(this.stsRepositoryLoading);
            this.grpAvailRepos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAvailRepos.Location = new System.Drawing.Point(10, 10);
            this.grpAvailRepos.Name = "grpAvailRepos";
            this.grpAvailRepos.Padding = new System.Windows.Forms.Padding(10);
            this.grpAvailRepos.Size = new System.Drawing.Size(564, 291);
            this.grpAvailRepos.TabIndex = 20;
            this.grpAvailRepos.TabStop = false;
            this.grpAvailRepos.Text = " Available repositories ";
            // 
            // stsRepositoryLoading
            // 
            this.stsRepositoryLoading.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prgProgress,
            this.lblStatus});
            this.stsRepositoryLoading.Location = new System.Drawing.Point(10, 259);
            this.stsRepositoryLoading.Name = "stsRepositoryLoading";
            this.stsRepositoryLoading.Size = new System.Drawing.Size(544, 22);
            this.stsRepositoryLoading.SizingGrip = false;
            this.stsRepositoryLoading.TabIndex = 1;
            this.stsRepositoryLoading.Text = "stsRepositoryLoading";
            this.stsRepositoryLoading.Visible = false;
            // 
            // prgProgress
            // 
            this.prgProgress.Name = "prgProgress";
            this.prgProgress.Size = new System.Drawing.Size(100, 16);
            this.prgProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(74, 17);
            this.lblStatus.Text = "Please wait...";
            // 
            // grpRepoDetails
            // 
            this.grpRepoDetails.Controls.Add(this.txtRepoDetails);
            this.grpRepoDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpRepoDetails.Location = new System.Drawing.Point(10, 311);
            this.grpRepoDetails.Name = "grpRepoDetails";
            this.grpRepoDetails.Padding = new System.Windows.Forms.Padding(10);
            this.grpRepoDetails.Size = new System.Drawing.Size(564, 95);
            this.grpRepoDetails.TabIndex = 21;
            this.grpRepoDetails.TabStop = false;
            this.grpRepoDetails.Text = " Repository details ";
            // 
            // txtRepoDetails
            // 
            this.txtRepoDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRepoDetails.Location = new System.Drawing.Point(10, 23);
            this.txtRepoDetails.Multiline = true;
            this.txtRepoDetails.Name = "txtRepoDetails";
            this.txtRepoDetails.ReadOnly = true;
            this.txtRepoDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRepoDetails.Size = new System.Drawing.Size(544, 62);
            this.txtRepoDetails.TabIndex = 0;
            // 
            // pnlRepositoryDetailsSpacing
            // 
            this.pnlRepositoryDetailsSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlRepositoryDetailsSpacing.Location = new System.Drawing.Point(10, 301);
            this.pnlRepositoryDetailsSpacing.Name = "pnlRepositoryDetailsSpacing";
            this.pnlRepositoryDetailsSpacing.Size = new System.Drawing.Size(564, 10);
            this.pnlRepositoryDetailsSpacing.TabIndex = 29;
            // 
            // RepositoryListForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 461);
            this.Controls.Add(this.grpAvailRepos);
            this.Controls.Add(this.pnlRepositoryDetailsSpacing);
            this.Controls.Add(this.grpRepoDetails);
            this.Controls.Add(this.pnlButtonsSpacing);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "RepositoryListForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.pnlButtons.ResumeLayout(false);
            this.grpAvailRepos.ResumeLayout(false);
            this.grpAvailRepos.PerformLayout();
            this.stsRepositoryLoading.ResumeLayout(false);
            this.stsRepositoryLoading.PerformLayout();
            this.grpRepoDetails.ResumeLayout(false);
            this.grpRepoDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Triggernometry.UI.CustomControls.TreeViewEx trvRepositories;
        private System.Windows.Forms.Panel pnlButtonsSpacing;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox grpAvailRepos;
        private System.Windows.Forms.GroupBox grpRepoDetails;
        private System.Windows.Forms.Panel pnlRepositoryDetailsSpacing;
        private System.Windows.Forms.TextBox txtRepoDetails;
        private System.Windows.Forms.StatusStrip stsRepositoryLoading;
        private System.Windows.Forms.ToolStripProgressBar prgProgress;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}