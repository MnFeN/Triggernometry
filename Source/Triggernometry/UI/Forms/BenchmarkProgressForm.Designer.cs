namespace Triggernometry.UI.Forms
{
    partial class BenchmarkProgressForm
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
            this.pnlBottomSpacing = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.prgBenchmark = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // pnlBottomSpacing
            // 
            this.pnlBottomSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomSpacing.Location = new System.Drawing.Point(10, 36);
            this.pnlBottomSpacing.Name = "pnlBottomSpacing";
            this.pnlBottomSpacing.Size = new System.Drawing.Size(264, 10);
            this.pnlBottomSpacing.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(10, 46);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(264, 35);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // prgBenchmark
            // 
            this.prgBenchmark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prgBenchmark.Location = new System.Drawing.Point(10, 10);
            this.prgBenchmark.Name = "prgBenchmark";
            this.prgBenchmark.Size = new System.Drawing.Size(264, 26);
            this.prgBenchmark.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgBenchmark.TabIndex = 9;
            // 
            // BenchmarkProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 91);
            this.Controls.Add(this.prgBenchmark);
            this.Controls.Add(this.pnlBottomSpacing);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 130);
            this.Name = "BenchmarkProgressForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Benchmark in progress";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottomSpacing;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.ProgressBar prgBenchmark;
    }
}