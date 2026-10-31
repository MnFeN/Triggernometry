namespace Triggernometry.UI.Forms
{
    partial class TraySliderForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Timer tmrShow;
        private System.Windows.Forms.Timer tmrFade;
        private System.Windows.Forms.Timer tmrSlideIn;
        private System.Windows.Forms.Timer tmrFadeOut;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tmrShow = new System.Windows.Forms.Timer(this.components);
            this.tmrFade = new System.Windows.Forms.Timer(this.components);
            this.tmrSlideIn = new System.Windows.Forms.Timer(this.components);
            this.tmrFadeOut = new System.Windows.Forms.Timer(this.components);
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.rtbText = new System.Windows.Forms.RichTextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.tlpButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnExtraAction = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.tlpMain.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrShow
            // 
            this.tmrShow.Enabled = true;
            this.tmrShow.Interval = 250;
            this.tmrShow.Tick += new System.EventHandler(this.tmrShow_Tick);
            // 
            // tmrFade
            // 
            this.tmrFade.Interval = 15000;
            this.tmrFade.Tick += new System.EventHandler(this.tmrFade_Tick);
            // 
            // tmrSlideIn
            // 
            this.tmrSlideIn.Interval = 15;
            this.tmrSlideIn.Tick += new System.EventHandler(this.tmrSlideIn_Tick);
            // 
            // tmrFadeOut
            // 
            this.tmrFadeOut.Interval = 25;
            this.tmrFadeOut.Tick += new System.EventHandler(this.tmrFadeOut_Tick);
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSize = true;
            this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.rtbText, 0, 1);
            this.tlpMain.Controls.Add(this.lblTitle, 0, 0);
            this.tlpMain.Controls.Add(this.tlpButtons, 0, 2);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.ForeColor = System.Drawing.Color.Black;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(320, 280);
            this.tlpMain.TabIndex = 0;
            // 
            // rtbText
            // 
            this.rtbText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(250)))));
            this.rtbText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbText.DetectUrls = false;
            this.rtbText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbText.ForeColor = System.Drawing.Color.Black;
            this.rtbText.Location = new System.Drawing.Point(20, 41);
            this.rtbText.Margin = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.rtbText.Name = "rtbText";
            this.rtbText.ReadOnly = true;
            this.rtbText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbText.Size = new System.Drawing.Size(280, 188);
            this.rtbText.TabIndex = 0;
            this.rtbText.TabStop = false;
            this.rtbText.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor in" +
    "cididunt ut labore et dolore magna aliqua.";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(180)))), ((int)(((byte)(225)))));
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.lblTitle.Location = new System.Drawing.Point(3, 3);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(3);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lblTitle.Size = new System.Drawing.Size(314, 25);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlpButtons
            // 
            this.tlpButtons.AutoSize = true;
            this.tlpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpButtons.ColumnCount = 3;
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpButtons.Controls.Add(this.btnExtraAction, 2, 0);
            this.tlpButtons.Controls.Add(this.btnCancel, 1, 0);
            this.tlpButtons.Controls.Add(this.btnConfirm, 0, 0);
            this.tlpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButtons.Location = new System.Drawing.Point(3, 242);
            this.tlpButtons.Name = "tlpButtons";
            this.tlpButtons.RowCount = 1;
            this.tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButtons.Size = new System.Drawing.Size(314, 35);
            this.tlpButtons.TabIndex = 2;
            // 
            // btnExtraAction
            // 
            this.btnExtraAction.AutoSize = true;
            this.btnExtraAction.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExtraAction.BackColor = System.Drawing.Color.Gainsboro;
            this.btnExtraAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExtraAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExtraAction.ForeColor = System.Drawing.Color.Black;
            this.btnExtraAction.Location = new System.Drawing.Point(211, 3);
            this.btnExtraAction.Name = "btnExtraAction";
            this.btnExtraAction.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.btnExtraAction.Size = new System.Drawing.Size(100, 29);
            this.btnExtraAction.TabIndex = 0;
            this.btnExtraAction.TabStop = false;
            this.btnExtraAction.Text = "btnExtraAction";
            this.btnExtraAction.UseVisualStyleBackColor = false;
            this.btnExtraAction.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.BackColor = System.Drawing.Color.MistyRose;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(107, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.btnCancel.Size = new System.Drawing.Size(98, 29);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.Button_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.AutoSize = true;
            this.btnConfirm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnConfirm.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnConfirm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.ForeColor = System.Drawing.Color.Black;
            this.btnConfirm.Location = new System.Drawing.Point(3, 3);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.btnConfirm.Size = new System.Drawing.Size(98, 29);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.TabStop = false;
            this.btnConfirm.Text = "btnConfirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.Button_Click);
            // 
            // TraySliderForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(320, 280);
            this.ControlBox = false;
            this.Controls.Add(this.tlpMain);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TraySliderForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TraySlider";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TraySlider_FormClosing);
            this.MouseEnter += new System.EventHandler(this.TraySlider_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.TraySlider_MouseLeave);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.tlpButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpButtons;
        public System.Windows.Forms.Button btnConfirm;
        public System.Windows.Forms.Button btnExtraAction;
        public System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RichTextBox rtbText;
        public System.Windows.Forms.Label lblTitle;
    }
}