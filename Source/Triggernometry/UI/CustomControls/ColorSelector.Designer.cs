namespace Triggernometry.UI.CustomControls
{
    partial class ColorSelector
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorSelector));
            this.btnColorMenu = new System.Windows.Forms.Button();
            this.ctxColors = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxSetTextColor = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxSetTextOutline = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxSetBackground = new System.Windows.Forms.ToolStripMenuItem();
            this.sepTransparentBackground = new System.Windows.Forms.ToolStripSeparator();
            this.ctxSetBackgroundTransparent = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.ctxColors.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnColorMenu
            // 
            this.btnColorMenu.ContextMenuStrip = this.ctxColors;
            this.btnColorMenu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnColorMenu.Image = ((System.Drawing.Image)(resources.GetObject("btnColorMenu.Image")));
            this.btnColorMenu.Location = new System.Drawing.Point(178, 0);
            this.btnColorMenu.Name = "btnColorMenu";
            this.btnColorMenu.Size = new System.Drawing.Size(29, 33);
            this.btnColorMenu.TabIndex = 0;
            this.btnColorMenu.UseVisualStyleBackColor = true;
            this.btnColorMenu.Click += new System.EventHandler(this.btnColorMenu_Click);
            // 
            // ctxColors
            // 
            this.ctxColors.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxSetTextColor,
            this.ctxSetTextOutline,
            this.ctxSetBackground,
            this.sepTransparentBackground,
            this.ctxSetBackgroundTransparent});
            this.ctxColors.Name = "ctxColors";
            this.ctxColors.Size = new System.Drawing.Size(235, 120);
            this.ctxColors.Opening += new System.ComponentModel.CancelEventHandler(this.ctxColors_Opening);
            // 
            // ctxSetTextColor
            // 
            this.ctxSetTextColor.Name = "ctxSetTextColor";
            this.ctxSetTextColor.Size = new System.Drawing.Size(234, 22);
            this.ctxSetTextColor.Text = "Set text color";
            this.ctxSetTextColor.Click += new System.EventHandler(this.setTextColorToolStripMenuItem_Click);
            // 
            // ctxSetTextOutline
            // 
            this.ctxSetTextOutline.Name = "ctxSetTextOutline";
            this.ctxSetTextOutline.Size = new System.Drawing.Size(234, 22);
            this.ctxSetTextOutline.Text = "Set text outline color";
            this.ctxSetTextOutline.Click += new System.EventHandler(this.setTextOutlineColorToolStripMenuItem_Click);
            // 
            // ctxSetBackground
            // 
            this.ctxSetBackground.Name = "ctxSetBackground";
            this.ctxSetBackground.Size = new System.Drawing.Size(234, 22);
            this.ctxSetBackground.Text = "Set background color";
            this.ctxSetBackground.Click += new System.EventHandler(this.setBackgroundColorToolStripMenuItem_Click);
            // 
            // sepTransparentBackground
            // 
            this.sepTransparentBackground.Name = "sepTransparentBackground";
            this.sepTransparentBackground.Size = new System.Drawing.Size(231, 6);
            // 
            // ctxSetBackgroundTransparent
            // 
            this.ctxSetBackgroundTransparent.Name = "ctxSetBackgroundTransparent";
            this.ctxSetBackgroundTransparent.Size = new System.Drawing.Size(234, 22);
            this.ctxSetBackgroundTransparent.Text = "Set background as transparent";
            this.ctxSetBackgroundTransparent.Click += new System.EventHandler(this.setBackgroundAsTransparentToolStripMenuItem_Click);
            // 
            // ColorSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContextMenuStrip = this.ctxColors;
            this.Controls.Add(this.btnColorMenu);
            this.Name = "ColorSelector";
            this.Size = new System.Drawing.Size(207, 33);
            this.Click += new System.EventHandler(this.btnColorMenu_Click);
            this.ctxColors.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnColorMenu;
        private System.Windows.Forms.ColorDialog dlgColor;
        private System.Windows.Forms.ContextMenuStrip ctxColors;
        private System.Windows.Forms.ToolStripMenuItem ctxSetTextColor;
        private System.Windows.Forms.ToolStripMenuItem ctxSetTextOutline;
        private System.Windows.Forms.ToolStripMenuItem ctxSetBackground;
        private System.Windows.Forms.ToolStripSeparator sepTransparentBackground;
        private System.Windows.Forms.ToolStripMenuItem ctxSetBackgroundTransparent;
    }
}
