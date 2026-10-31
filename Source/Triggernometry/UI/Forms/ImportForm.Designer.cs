namespace Triggernometry.UI.Forms
{
    partial class ImportForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportForm));
            this.pnlButtonsSpacing = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.grpImportSource = new System.Windows.Forms.GroupBox();
            this.tableImportSource = new System.Windows.Forms.TableLayoutPanel();
            this.radExistingActTriggers = new System.Windows.Forms.RadioButton();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.radLoadFromFileURI = new System.Windows.Forms.RadioButton();
            this.radImportFromText = new System.Windows.Forms.RadioButton();
            this.txtImportFile = new System.Windows.Forms.TextBox();
            this.pnlDirectImport = new System.Windows.Forms.Panel();
            this.pnlImportSourceText = new System.Windows.Forms.Panel();
            this.txtImportSource = new System.Windows.Forms.RichTextBox();
            this.ctxImportSource = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxPasteFromClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsDirectPaste = new System.Windows.Forms.ToolStrip();
            this.btnPaste = new System.Windows.Forms.ToolStripButton();
            this.tbcImportSteps = new System.Windows.Forms.TabControl();
            this.tabImportSource = new System.Windows.Forms.TabPage();
            this.tabImportPreview = new System.Windows.Forms.TabPage();
            this.trvImportPreview = new System.Windows.Forms.TreeView();
            this.ctxImportPreview = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.sepContextRemove = new System.Windows.Forms.ToolStripSeparator();
            this.ctxRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.lblWarning = new System.Windows.Forms.Label();
            this.tlsImport = new System.Windows.Forms.ToolStrip();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.sepToolbarRemove = new System.Windows.Forms.ToolStripSeparator();
            this.btnRemove = new System.Windows.Forms.ToolStripButton();
            this.dlgImportFile = new System.Windows.Forms.OpenFileDialog();
            this.pnlButtons.SuspendLayout();
            this.grpImportSource.SuspendLayout();
            this.tableImportSource.SuspendLayout();
            this.pnlDirectImport.SuspendLayout();
            this.pnlImportSourceText.SuspendLayout();
            this.ctxImportSource.SuspendLayout();
            this.tlsDirectPaste.SuspendLayout();
            this.tbcImportSteps.SuspendLayout();
            this.tabImportSource.SuspendLayout();
            this.tabImportPreview.SuspendLayout();
            this.ctxImportPreview.SuspendLayout();
            this.tlsImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtonsSpacing
            // 
            this.pnlButtonsSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonsSpacing.Location = new System.Drawing.Point(10, 306);
            this.pnlButtonsSpacing.Name = "pnlButtonsSpacing";
            this.pnlButtonsSpacing.Size = new System.Drawing.Size(564, 10);
            this.pnlButtonsSpacing.TabIndex = 13;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnNext);
            this.pnlButtons.Controls.Add(this.btnBack);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(10, 316);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(564, 35);
            this.pnlButtons.TabIndex = 14;
            // 
            // btnNext
            // 
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(414, 0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(150, 35);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnBack.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnBack.Location = new System.Drawing.Point(0, 0);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(150, 35);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Cancel";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // grpImportSource
            // 
            this.grpImportSource.AutoSize = true;
            this.grpImportSource.Controls.Add(this.tableImportSource);
            this.grpImportSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpImportSource.Location = new System.Drawing.Point(3, 3);
            this.grpImportSource.Name = "grpImportSource";
            this.grpImportSource.Padding = new System.Windows.Forms.Padding(10);
            this.grpImportSource.Size = new System.Drawing.Size(550, 261);
            this.grpImportSource.TabIndex = 15;
            this.grpImportSource.TabStop = false;
            this.grpImportSource.Text = " Specify import source ";
            // 
            // tableImportSource
            // 
            this.tableImportSource.AutoSize = true;
            this.tableImportSource.ColumnCount = 3;
            this.tableImportSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableImportSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableImportSource.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableImportSource.Controls.Add(this.radExistingActTriggers, 0, 4);
            this.tableImportSource.Controls.Add(this.btnBrowseFile, 2, 3);
            this.tableImportSource.Controls.Add(this.radLoadFromFileURI, 0, 2);
            this.tableImportSource.Controls.Add(this.radImportFromText, 0, 0);
            this.tableImportSource.Controls.Add(this.txtImportFile, 1, 3);
            this.tableImportSource.Controls.Add(this.pnlDirectImport, 1, 1);
            this.tableImportSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableImportSource.Location = new System.Drawing.Point(10, 23);
            this.tableImportSource.Name = "tableImportSource";
            this.tableImportSource.RowCount = 5;
            this.tableImportSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableImportSource.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableImportSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableImportSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableImportSource.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableImportSource.Size = new System.Drawing.Size(530, 228);
            this.tableImportSource.TabIndex = 0;
            // 
            // radExistingActTriggers
            // 
            this.radExistingActTriggers.AutoSize = true;
            this.tableImportSource.SetColumnSpan(this.radExistingActTriggers, 3);
            this.radExistingActTriggers.Dock = System.Windows.Forms.DockStyle.Top;
            this.radExistingActTriggers.Location = new System.Drawing.Point(3, 208);
            this.radExistingActTriggers.Name = "radExistingActTriggers";
            this.radExistingActTriggers.Size = new System.Drawing.Size(524, 17);
            this.radExistingActTriggers.TabIndex = 17;
            this.radExistingActTriggers.TabStop = true;
            this.radExistingActTriggers.Text = "Existing ACT triggers";
            this.radExistingActTriggers.UseVisualStyleBackColor = true;
            this.radExistingActTriggers.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // btnBrowseFile
            // 
            this.btnBrowseFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBrowseFile.Enabled = false;
            this.btnBrowseFile.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowseFile.Image")));
            this.btnBrowseFile.Location = new System.Drawing.Point(490, 179);
            this.btnBrowseFile.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(37, 26);
            this.btnBrowseFile.TabIndex = 14;
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.button3_Click);
            // 
            // radLoadFromFileURI
            // 
            this.radLoadFromFileURI.AutoSize = true;
            this.tableImportSource.SetColumnSpan(this.radLoadFromFileURI, 3);
            this.radLoadFromFileURI.Dock = System.Windows.Forms.DockStyle.Top;
            this.radLoadFromFileURI.Location = new System.Drawing.Point(3, 159);
            this.radLoadFromFileURI.Name = "radLoadFromFileURI";
            this.radLoadFromFileURI.Size = new System.Drawing.Size(524, 17);
            this.radLoadFromFileURI.TabIndex = 3;
            this.radLoadFromFileURI.TabStop = true;
            this.radLoadFromFileURI.Text = "Load from file or URI";
            this.radLoadFromFileURI.UseVisualStyleBackColor = true;
            this.radLoadFromFileURI.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radImportFromText
            // 
            this.radImportFromText.AutoSize = true;
            this.tableImportSource.SetColumnSpan(this.radImportFromText, 3);
            this.radImportFromText.Dock = System.Windows.Forms.DockStyle.Top;
            this.radImportFromText.Location = new System.Drawing.Point(3, 3);
            this.radImportFromText.Name = "radImportFromText";
            this.radImportFromText.Size = new System.Drawing.Size(524, 17);
            this.radImportFromText.TabIndex = 1;
            this.radImportFromText.TabStop = true;
            this.radImportFromText.Text = "Directly from text";
            this.radImportFromText.UseVisualStyleBackColor = true;
            this.radImportFromText.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // txtImportFile
            // 
            this.txtImportFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtImportFile.Enabled = false;
            this.txtImportFile.Location = new System.Drawing.Point(33, 182);
            this.txtImportFile.Name = "txtImportFile";
            this.txtImportFile.Size = new System.Drawing.Size(454, 20);
            this.txtImportFile.TabIndex = 15;
            this.txtImportFile.TextChanged += new System.EventHandler(this.txtImportFile_TextChanged);
            // 
            // pnlDirectImport
            // 
            this.tableImportSource.SetColumnSpan(this.pnlDirectImport, 2);
            this.pnlDirectImport.Controls.Add(this.pnlImportSourceText);
            this.pnlDirectImport.Controls.Add(this.tlsDirectPaste);
            this.pnlDirectImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDirectImport.Enabled = false;
            this.pnlDirectImport.Location = new System.Drawing.Point(33, 26);
            this.pnlDirectImport.Name = "pnlDirectImport";
            this.pnlDirectImport.Size = new System.Drawing.Size(494, 127);
            this.pnlDirectImport.TabIndex = 16;
            // 
            // pnlImportSourceText
            // 
            this.pnlImportSourceText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlImportSourceText.Controls.Add(this.txtImportSource);
            this.pnlImportSourceText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImportSourceText.Location = new System.Drawing.Point(0, 25);
            this.pnlImportSourceText.Name = "pnlImportSourceText";
            this.pnlImportSourceText.Size = new System.Drawing.Size(494, 102);
            this.pnlImportSourceText.TabIndex = 1;
            // 
            // txtImportSource
            // 
            this.txtImportSource.BackColor = System.Drawing.SystemColors.Window;
            this.txtImportSource.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtImportSource.ContextMenuStrip = this.ctxImportSource;
            this.txtImportSource.DetectUrls = false;
            this.txtImportSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtImportSource.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImportSource.Location = new System.Drawing.Point(0, 0);
            this.txtImportSource.Name = "txtImportSource";
            this.txtImportSource.Size = new System.Drawing.Size(492, 100);
            this.txtImportSource.TabIndex = 1;
            this.txtImportSource.Text = "";
            this.txtImportSource.WordWrap = false;
            this.txtImportSource.TextChanged += new System.EventHandler(this.txtImportSource_TextChanged);
            // 
            // ctxImportSource
            // 
            this.ctxImportSource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxPasteFromClipboard});
            this.ctxImportSource.Name = "ctxImportSource";
            this.ctxImportSource.Size = new System.Drawing.Size(185, 26);
            this.ctxImportSource.Opening += new System.ComponentModel.CancelEventHandler(this.ctxImportSource_Opening);
            // 
            // ctxPasteFromClipboard
            // 
            this.ctxPasteFromClipboard.Image = ((System.Drawing.Image)(resources.GetObject("ctxPasteFromClipboard.Image")));
            this.ctxPasteFromClipboard.Name = "ctxPasteFromClipboard";
            this.ctxPasteFromClipboard.Size = new System.Drawing.Size(184, 22);
            this.ctxPasteFromClipboard.Text = "Paste from clipboard";
            this.ctxPasteFromClipboard.Click += new System.EventHandler(this.pasteFromClipboardToolStripMenuItem_Click);
            // 
            // tlsDirectPaste
            // 
            this.tlsDirectPaste.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsDirectPaste.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPaste});
            this.tlsDirectPaste.Location = new System.Drawing.Point(0, 0);
            this.tlsDirectPaste.Name = "tlsDirectPaste";
            this.tlsDirectPaste.Size = new System.Drawing.Size(494, 25);
            this.tlsDirectPaste.TabIndex = 0;
            // 
            // btnPaste
            // 
            this.btnPaste.Image = ((System.Drawing.Image)(resources.GetObject("btnPaste.Image")));
            this.btnPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(137, 22);
            this.btnPaste.Text = "Paste from clipboard";
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // tbcImportSteps
            // 
            this.tbcImportSteps.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tbcImportSteps.Controls.Add(this.tabImportSource);
            this.tbcImportSteps.Controls.Add(this.tabImportPreview);
            this.tbcImportSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcImportSteps.Location = new System.Drawing.Point(10, 10);
            this.tbcImportSteps.Margin = new System.Windows.Forms.Padding(0);
            this.tbcImportSteps.Name = "tbcImportSteps";
            this.tbcImportSteps.Padding = new System.Drawing.Point(0, 0);
            this.tbcImportSteps.SelectedIndex = 0;
            this.tbcImportSteps.Size = new System.Drawing.Size(564, 296);
            this.tbcImportSteps.TabIndex = 16;
            this.tbcImportSteps.Selected += new System.Windows.Forms.TabControlEventHandler(this.tbcImportSteps_Selected);
            // 
            // tabImportSource
            // 
            this.tabImportSource.Controls.Add(this.grpImportSource);
            this.tabImportSource.Location = new System.Drawing.Point(4, 25);
            this.tabImportSource.Name = "tabImportSource";
            this.tabImportSource.Padding = new System.Windows.Forms.Padding(3);
            this.tabImportSource.Size = new System.Drawing.Size(556, 267);
            this.tabImportSource.TabIndex = 0;
            this.tabImportSource.Text = "Source";
            this.tabImportSource.UseVisualStyleBackColor = true;
            // 
            // tabImportPreview
            // 
            this.tabImportPreview.Controls.Add(this.trvImportPreview);
            this.tabImportPreview.Controls.Add(this.lblWarning);
            this.tabImportPreview.Controls.Add(this.tlsImport);
            this.tabImportPreview.Location = new System.Drawing.Point(4, 25);
            this.tabImportPreview.Margin = new System.Windows.Forms.Padding(0);
            this.tabImportPreview.Name = "tabImportPreview";
            this.tabImportPreview.Size = new System.Drawing.Size(556, 267);
            this.tabImportPreview.TabIndex = 1;
            this.tabImportPreview.Text = "Import";
            this.tabImportPreview.UseVisualStyleBackColor = true;
            // 
            // trvImportPreview
            // 
            this.trvImportPreview.AllowDrop = true;
            this.trvImportPreview.ContextMenuStrip = this.ctxImportPreview;
            this.trvImportPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvImportPreview.Location = new System.Drawing.Point(0, 88);
            this.trvImportPreview.Name = "trvImportPreview";
            this.trvImportPreview.ShowNodeToolTips = true;
            this.trvImportPreview.Size = new System.Drawing.Size(556, 179);
            this.trvImportPreview.TabIndex = 3;
            this.trvImportPreview.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvImportPreview_BeforeCollapse);
            this.trvImportPreview.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvImportPreview_BeforeExpand);
            this.trvImportPreview.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvImportPreview_AfterSelect);
            this.trvImportPreview.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvImportPreview_NodeMouseClick);
            this.trvImportPreview.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvImportPreview_NodeMouseDoubleClick);
            // 
            // ctxImportPreview
            // 
            this.ctxImportPreview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxEdit,
            this.sepContextRemove,
            this.ctxRemove});
            this.ctxImportPreview.Name = "ctxImportPreview";
            this.ctxImportPreview.Size = new System.Drawing.Size(118, 54);
            this.ctxImportPreview.Opening += new System.ComponentModel.CancelEventHandler(this.ctxImportPreview_Opening);
            // 
            // ctxEdit
            // 
            this.ctxEdit.Image = ((System.Drawing.Image)(resources.GetObject("ctxEdit.Image")));
            this.ctxEdit.Name = "ctxEdit";
            this.ctxEdit.Size = new System.Drawing.Size(117, 22);
            this.ctxEdit.Text = "Edit";
            this.ctxEdit.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // sepContextRemove
            // 
            this.sepContextRemove.Name = "sepContextRemove";
            this.sepContextRemove.Size = new System.Drawing.Size(114, 6);
            // 
            // ctxRemove
            // 
            this.ctxRemove.Image = ((System.Drawing.Image)(resources.GetObject("cxtRemove.Image")));
            this.ctxRemove.Name = "cxtRemove";
            this.ctxRemove.Size = new System.Drawing.Size(117, 22);
            this.ctxRemove.Text = "Remove";
            this.ctxRemove.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // lblWarning
            // 
            this.lblWarning.AutoEllipsis = true;
            this.lblWarning.BackColor = System.Drawing.SystemColors.Info;
            this.lblWarning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWarning.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWarning.Location = new System.Drawing.Point(0, 25);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(956, 63);
            this.lblWarning.TabIndex = 4;
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblWarning.Visible = false;
            // 
            // tlsImport
            // 
            this.tlsImport.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsImport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEdit,
            this.sepToolbarRemove,
            this.btnRemove});
            this.tlsImport.Location = new System.Drawing.Point(0, 0);
            this.tlsImport.Name = "tlsImport";
            this.tlsImport.Size = new System.Drawing.Size(556, 25);
            this.tlsImport.TabIndex = 5;
            // 
            // btnEdit
            // 
            this.btnEdit.Enabled = false;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(47, 22);
            this.btnEdit.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // sepToolbarRemove
            // 
            this.sepToolbarRemove.Name = "sepToolbarRemove";
            this.sepToolbarRemove.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(70, 22);
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // dlgImportFile
            // 
            this.dlgImportFile.FileName = "TriggernometryExport.xml";
            this.dlgImportFile.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            this.dlgImportFile.RestoreDirectory = true;
            this.dlgImportFile.Title = "Select file to import";
            // 
            // ImportForm
            // 
            this.AcceptButton = this.btnNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnBack;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.tbcImportSteps);
            this.Controls.Add(this.pnlButtonsSpacing);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(1000, 700);
            this.Name = "ImportForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import";
            this.pnlButtons.ResumeLayout(false);
            this.grpImportSource.ResumeLayout(false);
            this.grpImportSource.PerformLayout();
            this.tableImportSource.ResumeLayout(false);
            this.tableImportSource.PerformLayout();
            this.pnlDirectImport.ResumeLayout(false);
            this.pnlDirectImport.PerformLayout();
            this.pnlImportSourceText.ResumeLayout(false);
            this.ctxImportSource.ResumeLayout(false);
            this.tlsDirectPaste.ResumeLayout(false);
            this.tlsDirectPaste.PerformLayout();
            this.tbcImportSteps.ResumeLayout(false);
            this.tabImportSource.ResumeLayout(false);
            this.tabImportSource.PerformLayout();
            this.tabImportPreview.ResumeLayout(false);
            this.tabImportPreview.PerformLayout();
            this.ctxImportPreview.ResumeLayout(false);
            this.tlsImport.ResumeLayout(false);
            this.tlsImport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlButtonsSpacing;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnNext;
        internal System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.GroupBox grpImportSource;
        private System.Windows.Forms.TableLayoutPanel tableImportSource;
        private System.Windows.Forms.RadioButton radLoadFromFileURI;
        private System.Windows.Forms.RadioButton radImportFromText;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.TextBox txtImportFile;
        private System.Windows.Forms.RadioButton radExistingActTriggers;
        private System.Windows.Forms.Panel pnlDirectImport;
        private System.Windows.Forms.Panel pnlImportSourceText;
        private System.Windows.Forms.ToolStrip tlsDirectPaste;
        private System.Windows.Forms.RichTextBox txtImportSource;
        private System.Windows.Forms.ToolStripButton btnPaste;
        private System.Windows.Forms.TabControl tbcImportSteps;
        private System.Windows.Forms.TabPage tabImportSource;
        private System.Windows.Forms.TabPage tabImportPreview;
        private System.Windows.Forms.OpenFileDialog dlgImportFile;
        internal System.Windows.Forms.TreeView trvImportPreview;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.ToolStrip tlsImport;
        private System.Windows.Forms.ToolStripSeparator sepToolbarRemove;
        private System.Windows.Forms.ToolStripButton btnRemove;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ContextMenuStrip ctxImportPreview;
        private System.Windows.Forms.ToolStripMenuItem ctxEdit;
        private System.Windows.Forms.ToolStripSeparator sepContextRemove;
        private System.Windows.Forms.ToolStripMenuItem ctxRemove;
        private System.Windows.Forms.ContextMenuStrip ctxImportSource;
        private System.Windows.Forms.ToolStripMenuItem ctxPasteFromClipboard;
    }
}