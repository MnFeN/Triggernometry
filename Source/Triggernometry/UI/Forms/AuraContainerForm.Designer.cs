namespace Triggernometry.UI.Forms
{
    partial class AuraContainerForm
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
            this.picOverlay = new Triggernometry.UI.CustomControls.ClickthroughPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picOverlay)).BeginInit();
            this.SuspendLayout();
            // 
            // picOverlay
            // 
            this.picOverlay.BackColor = System.Drawing.Color.Transparent;
            this.picOverlay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picOverlay.Location = new System.Drawing.Point(0, 0);
            this.picOverlay.Name = "picOverlay";
            this.picOverlay.Size = new System.Drawing.Size(173, 133);
            this.picOverlay.TabIndex = 0;
            this.picOverlay.TabStop = false;
            // 
            // AuraContainerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(173, 133);
            this.ControlBox = false;
            this.Controls.Add(this.picOverlay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AuraContainerForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Triggernometry Aura";
            ((System.ComponentModel.ISupportInitialize)(this.picOverlay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CustomControls.ClickthroughPictureBox picOverlay;
    }

}