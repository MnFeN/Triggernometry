namespace Triggernometry.UI.Forms
{
    partial class BenchmarkForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BenchmarkForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlButtonsSpacing = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.workerBenchmark = new System.ComponentModel.BackgroundWorker();
            this.tlsMain = new System.Windows.Forms.ToolStrip();
            this.btnCopyToClipboard = new System.Windows.Forms.ToolStripButton();
            this.dgvResults = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRelEvalTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAvgEvalTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercOnRegexp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercOnFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercOnConditions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlButtons.SuspendLayout();
            this.tlsMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlButtonsSpacing
            // 
            this.pnlButtonsSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonsSpacing.Location = new System.Drawing.Point(10, 306);
            this.pnlButtonsSpacing.Name = "pnlButtonsSpacing";
            this.pnlButtonsSpacing.Size = new System.Drawing.Size(588, 10);
            this.pnlButtonsSpacing.TabIndex = 22;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Controls.Add(this.btnRun);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(10, 316);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(588, 35);
            this.pnlButtons.TabIndex = 24;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(438, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.btnRun.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRun.Image = ((System.Drawing.Image)(resources.GetObject("btnRun.Image")));
            this.btnRun.Location = new System.Drawing.Point(0, 0);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(150, 35);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // workerBenchmark
            // 
            this.workerBenchmark.WorkerReportsProgress = true;
            this.workerBenchmark.WorkerSupportsCancellation = true;
            this.workerBenchmark.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workerBenchmark_DoWork);
            // 
            // tlsMain
            // 
            this.tlsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCopyToClipboard});
            this.tlsMain.Location = new System.Drawing.Point(10, 10);
            this.tlsMain.Name = "tlsMain";
            this.tlsMain.Size = new System.Drawing.Size(588, 25);
            this.tlsMain.TabIndex = 25;
            // 
            // btnCopyToClipboard
            // 
            this.btnCopyToClipboard.Enabled = false;
            this.btnCopyToClipboard.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyToClipboard.Image")));
            this.btnCopyToClipboard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.Size = new System.Drawing.Size(122, 22);
            this.btnCopyToClipboard.Text = "Copy to clipboard";
            this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AllowUserToResizeColumns = false;
            this.dgvResults.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colRelEvalTime,
            this.colAvgEvalTime,
            this.colPercOnRegexp,
            this.colPercOnFolder,
            this.colPercOnConditions});
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvResults.Location = new System.Drawing.Point(10, 35);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.ShowCellErrors = false;
            this.dgvResults.ShowEditingIcon = false;
            this.dgvResults.ShowRowErrors = false;
            this.dgvResults.Size = new System.Drawing.Size(588, 271);
            this.dgvResults.TabIndex = 23;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.HeaderText = "Name";
            this.colName.MinimumWidth = 200;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colRelEvalTime
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colRelEvalTime.DefaultCellStyle = dataGridViewCellStyle2;
            this.colRelEvalTime.HeaderText = "Relative evaluation time";
            this.colRelEvalTime.Name = "colRelEvalTime";
            this.colRelEvalTime.ReadOnly = true;
            this.colRelEvalTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colRelEvalTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRelEvalTime.Width = 90;
            // 
            // colAvgEvalTime
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colAvgEvalTime.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAvgEvalTime.HeaderText = "Average evaluation time";
            this.colAvgEvalTime.Name = "colAvgEvalTime";
            this.colAvgEvalTime.ReadOnly = true;
            this.colAvgEvalTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colAvgEvalTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAvgEvalTime.Width = 90;
            // 
            // colPercOnRegexp
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colPercOnRegexp.DefaultCellStyle = dataGridViewCellStyle4;
            this.colPercOnRegexp.HeaderText = "% on regexp";
            this.colPercOnRegexp.Name = "colPercOnRegexp";
            this.colPercOnRegexp.ReadOnly = true;
            this.colPercOnRegexp.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colPercOnRegexp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPercOnRegexp.Width = 60;
            // 
            // colPercOnFolder
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colPercOnFolder.DefaultCellStyle = dataGridViewCellStyle5;
            this.colPercOnFolder.HeaderText = "% on folder";
            this.colPercOnFolder.Name = "colPercOnFolder";
            this.colPercOnFolder.ReadOnly = true;
            this.colPercOnFolder.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colPercOnFolder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPercOnFolder.Width = 60;
            // 
            // colPercOnConditions
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colPercOnConditions.DefaultCellStyle = dataGridViewCellStyle6;
            this.colPercOnConditions.HeaderText = "% on conditions";
            this.colPercOnConditions.Name = "colPercOnConditions";
            this.colPercOnConditions.ReadOnly = true;
            this.colPercOnConditions.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colPercOnConditions.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPercOnConditions.Width = 60;
            // 
            // BenchmarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(608, 361);
            this.Controls.Add(this.dgvResults);
            this.Controls.Add(this.pnlButtonsSpacing);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.tlsMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "BenchmarkForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Trigger benchmark";
            this.pnlButtons.ResumeLayout(false);
            this.tlsMain.ResumeLayout(false);
            this.tlsMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomControls.DataGridViewEx dgvResults;
        private System.Windows.Forms.Panel pnlButtonsSpacing;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnRun;
        internal System.ComponentModel.BackgroundWorker workerBenchmark;
        private System.Windows.Forms.ToolStrip tlsMain;
        private System.Windows.Forms.ToolStripButton btnCopyToClipboard;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRelEvalTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAvgEvalTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercOnRegexp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercOnFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercOnConditions;
    }
}