
namespace Triggernometry.UI.CustomControls
{
    partial class PersistenceSwitch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PersistenceSwitch));
            this.pnlPersistentIcon = new System.Windows.Forms.Panel();
            this.toolTipPersistence = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // pnlPersistentIcon
            // 
            this.pnlPersistentIcon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlPersistentIcon.BackgroundImage")));
            this.pnlPersistentIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlPersistentIcon.Location = new System.Drawing.Point(18, 16);
            this.pnlPersistentIcon.Name = "pnlPersistentIcon";
            this.pnlPersistentIcon.Size = new System.Drawing.Size(66, 38);
            this.pnlPersistentIcon.TabIndex = 0;
            this.pnlPersistentIcon.Visible = false;
            // 
            // PersistenceSwitch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.pnlPersistentIcon);
            this.DoubleBuffered = true;
            this.Name = "PersistenceSwitch";
            this.Size = new System.Drawing.Size(225, 89);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPersistentIcon;
        private System.Windows.Forms.ToolTip toolTipPersistence;
    }
}
