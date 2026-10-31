namespace Triggernometry.UI.CustomControls
{
    partial class ExpressionTextBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpressionTextBox));
            this.pnlExpandToggle = new System.Windows.Forms.Panel();
            this.txtExpression = new Triggernometry.UI.CustomControls.ExpressionTextBox.TextBox();
            this.toolTipExpression = new System.Windows.Forms.ToolTip(this.components);
            this.pnlExpandAlternateIcon = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlExpandToggle
            // 
            this.pnlExpandToggle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlExpandToggle.BackgroundImage")));
            this.pnlExpandToggle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlExpandToggle.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlExpandToggle.Location = new System.Drawing.Point(0, 0);
            this.pnlExpandToggle.Name = "pnlExpandToggle";
            this.pnlExpandToggle.Size = new System.Drawing.Size(24, 106);
            this.pnlExpandToggle.TabIndex = 0;
            this.pnlExpandToggle.Click += new System.EventHandler(this.pnlExpandToggle_Click);
            this.pnlExpandToggle.DoubleClick += new System.EventHandler(this.pnlExpandToggle_DoubleClick);
            // 
            // txtExpression
            // 
            this.txtExpression.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtExpression.Location = new System.Drawing.Point(24, 0);
            this.txtExpression.Name = "txtExpression";
            this.txtExpression.Size = new System.Drawing.Size(407, 20);
            this.txtExpression.TabIndex = 1;
            this.txtExpression.WordWrap = false;
            // 
            // pnlExpandAlternateIcon
            // 
            this.pnlExpandAlternateIcon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlExpandAlternateIcon.BackgroundImage")));
            this.pnlExpandAlternateIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlExpandAlternateIcon.Location = new System.Drawing.Point(43, 35);
            this.pnlExpandAlternateIcon.Name = "pnlExpandAlternateIcon";
            this.pnlExpandAlternateIcon.Size = new System.Drawing.Size(41, 28);
            this.pnlExpandAlternateIcon.TabIndex = 0;
            this.pnlExpandAlternateIcon.Visible = false;
            // 
            // ExpressionTextBox
            // 
            this.AutoSize = true;
            this.Controls.Add(this.pnlExpandAlternateIcon);
            this.Controls.Add(this.txtExpression);
            this.Controls.Add(this.pnlExpandToggle);
            this.DoubleBuffered = true;
            this.Name = "ExpressionTextBox";
            this.Size = new System.Drawing.Size(431, 106);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTipExpression;
        private System.Windows.Forms.Panel pnlExpandAlternateIcon;
        internal System.Windows.Forms.Panel pnlExpandToggle;
        internal TextBox txtExpression;
    }
}
