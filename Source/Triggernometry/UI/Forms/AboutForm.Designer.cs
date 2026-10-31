namespace Triggernometry.UI.Forms
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.tbcMain = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.txtGeneral = new System.Windows.Forms.TextBox();
            this.tabLicenses = new System.Windows.Forms.TabPage();
            this.txtLicenses = new System.Windows.Forms.TextBox();
            this.pnlButtonsSpacing = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.pnlContent.SuspendLayout();
            this.tbcMain.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabLicenses.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLogo
            // 
            this.pnlLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pnlLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlLogo.BackgroundImage")));
            this.pnlLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(434, 70);
            this.pnlLogo.TabIndex = 0;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.tbcMain);
            this.pnlContent.Controls.Add(this.pnlButtonsSpacing);
            this.pnlContent.Controls.Add(this.btnOk);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 70);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(10);
            this.pnlContent.Size = new System.Drawing.Size(434, 241);
            this.pnlContent.TabIndex = 1;
            // 
            // tbcMain
            // 
            this.tbcMain.Controls.Add(this.tabGeneral);
            this.tbcMain.Controls.Add(this.tabLicenses);
            this.tbcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcMain.Location = new System.Drawing.Point(10, 10);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.SelectedIndex = 0;
            this.tbcMain.Size = new System.Drawing.Size(414, 176);
            this.tbcMain.TabIndex = 3;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.txtGeneral);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(10);
            this.tabGeneral.Size = new System.Drawing.Size(406, 150);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // txtGeneral
            // 
            this.txtGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.txtGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGeneral.Location = new System.Drawing.Point(10, 10);
            this.txtGeneral.Multiline = true;
            this.txtGeneral.Name = "txtGeneral";
            this.txtGeneral.ReadOnly = true;
            this.txtGeneral.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtGeneral.Size = new System.Drawing.Size(386, 130);
            this.txtGeneral.TabIndex = 3;
            this.txtGeneral.Text = "Triggernometry - ACT trigger extension\r\nVersion [VERSIONNUMBER]\r\n\r\nBugs? Suggesti" +
    "ons? Join the Triggernometry Discord server:\r\nhttps://discord.gg/6f9MY55";
            this.txtGeneral.WordWrap = false;
            // 
            // tabLicenses
            // 
            this.tabLicenses.Controls.Add(this.txtLicenses);
            this.tabLicenses.Location = new System.Drawing.Point(4, 22);
            this.tabLicenses.Name = "tabLicenses";
            this.tabLicenses.Padding = new System.Windows.Forms.Padding(10);
            this.tabLicenses.Size = new System.Drawing.Size(406, 150);
            this.tabLicenses.TabIndex = 1;
            this.tabLicenses.Text = "Licenses";
            this.tabLicenses.UseVisualStyleBackColor = true;
            // 
            // txtLicenses
            // 
            this.txtLicenses.BackColor = System.Drawing.SystemColors.Control;
            this.txtLicenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLicenses.Location = new System.Drawing.Point(10, 10);
            this.txtLicenses.Multiline = true;
            this.txtLicenses.Name = "txtLicenses";
            this.txtLicenses.ReadOnly = true;
            this.txtLicenses.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLicenses.Size = new System.Drawing.Size(386, 130);
            this.txtLicenses.TabIndex = 4;
            this.txtLicenses.Text = resources.GetString("txtLicenses.Text");
            this.txtLicenses.WordWrap = false;
            // 
            // pnlButtonsSpacing
            // 
            this.pnlButtonsSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonsSpacing.Location = new System.Drawing.Point(10, 186);
            this.pnlButtonsSpacing.Name = "pnlButtonsSpacing";
            this.pnlButtonsSpacing.Size = new System.Drawing.Size(414, 10);
            this.pnlButtonsSpacing.TabIndex = 6;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOk.Location = new System.Drawing.Point(10, 196);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(414, 35);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // AboutForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOk;
            this.ClientSize = new System.Drawing.Size(434, 311);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(450, 350);
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " About";
            this.Shown += new System.EventHandler(this.AboutForm_Shown);
            this.pnlContent.ResumeLayout(false);
            this.tbcMain.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabLicenses.ResumeLayout(false);
            this.tabLicenses.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Panel pnlContent;
        internal System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TextBox txtGeneral;
        private System.Windows.Forms.TabPage tabLicenses;
        private System.Windows.Forms.TextBox txtLicenses;
        private System.Windows.Forms.Panel pnlButtonsSpacing;
    }
}