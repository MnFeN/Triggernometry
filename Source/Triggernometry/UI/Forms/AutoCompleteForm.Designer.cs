
namespace Triggernometry.UI.Forms
{
    partial class AutoCompleteForm
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
            this.lstSuggestions = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstSuggestions
            // 
            this.lstSuggestions.BackColor = System.Drawing.SystemColors.Info;
            this.lstSuggestions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstSuggestions.FormattingEnabled = true;
            this.lstSuggestions.ItemHeight = 16;
            this.lstSuggestions.Location = new System.Drawing.Point(0, 0);
            this.lstSuggestions.Margin = new System.Windows.Forms.Padding(4);
            this.lstSuggestions.Name = "lstSuggestions";
            this.lstSuggestions.Size = new System.Drawing.Size(100, 66);
            this.lstSuggestions.TabIndex = 0;
            // 
            // AutoCompleteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(242, 138);
            this.Controls.Add(this.lstSuggestions);
            this.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AutoCompleteForm";
            this.Opacity = 0.8D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AutoCompleteForm";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ListBox lstSuggestions;
    }
}