namespace Triggernometry.UI.CustomControls
{
    partial class RegisteredVariableViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegisteredVariableViewer));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnAddVariable = new System.Windows.Forms.ToolStripButton();
            this.btnAddComment = new System.Windows.Forms.ToolStripButton();
            this.btnExpandEdit = new System.Windows.Forms.ToolStripButton();
            this.btnApply = new System.Windows.Forms.ToolStripButton();
            this.btnUp = new System.Windows.Forms.ToolStripButton();
            this.btnDown = new System.Windows.Forms.ToolStripButton();
            this.btnTop = new System.Windows.Forms.ToolStripButton();
            this.btnBottom = new System.Windows.Forms.ToolStripButton();
            this.btnRemove = new System.Windows.Forms.ToolStripButton();
            this.btnApplyAll = new System.Windows.Forms.ToolStripButton();
            this.sepToolbar1 = new System.Windows.Forms.ToolStripSeparator();
            this.sepToolbar2 = new System.Windows.Forms.ToolStripSeparator();
            this.dgvVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoadWhenActive = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ctxVariables = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxApply = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxApplyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxAddVariable = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxAddComment = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxExpandEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxUp = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxDown = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTop = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.sepContext1 = new System.Windows.Forms.ToolStripSeparator();
            this.sepContext2 = new System.Windows.Forms.ToolStripSeparator();
            this.sepContext3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).BeginInit();
            this.ctxVariables.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddVariable,
            this.btnAddComment,
            this.btnExpandEdit,
            this.btnApply,
            this.sepToolbar1,
            this.btnUp,
            this.btnDown,
            this.btnTop,
            this.btnBottom,
            this.sepToolbar2,
            this.btnRemove,
            this.btnApplyAll});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(700, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // btnAddVariable
            // 
            this.btnAddVariable.Image = ((System.Drawing.Image)(resources.GetObject("btnAddVariable.Image")));
            this.btnAddVariable.Name = "btnAddVariable";
            this.btnAddVariable.Size = new System.Drawing.Size(90, 22);
            this.btnAddVariable.Text = "Add variable";
            this.btnAddVariable.Click += new System.EventHandler(this.AddVariable_Click);
            // 
            // btnAddComment
            // 
            this.btnAddComment.Image = ((System.Drawing.Image)(resources.GetObject("btnAddComment.Image")));
            this.btnAddComment.Name = "btnAddComment";
            this.btnAddComment.Size = new System.Drawing.Size(90, 22);
            this.btnAddComment.Text = "Add comment";
            this.btnAddComment.Click += new System.EventHandler(this.AddComment_Click);
            // 
            // btnExpandEdit
            // 
            this.btnExpandEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnExpandEdit.Image")));
            this.btnExpandEdit.Name = "btnExpandEdit";
            this.btnExpandEdit.Size = new System.Drawing.Size(90, 22);
            this.btnExpandEdit.Text = "Expand edit";
            this.btnExpandEdit.Click += new System.EventHandler(this.ExpandEdit_Click);
            // 
            // btnApply
            // 
            this.btnApply.Image = ((System.Drawing.Image)(resources.GetObject("btnApply.Image")));
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(90, 22);
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // btnUp
            // 
            this.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(23, 22);
            this.btnUp.Text = "Up";
            this.btnUp.Click += new System.EventHandler(this.Up_Click);
            // 
            // btnDown
            // 
            this.btnDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(23, 22);
            this.btnDown.Text = "Down";
            this.btnDown.Click += new System.EventHandler(this.Down_Click);
            // 
            // btnTop
            // 
            this.btnTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnTop.Image = ((System.Drawing.Image)(resources.GetObject("btnTop.Image")));
            this.btnTop.Name = "btnTop";
            this.btnTop.Size = new System.Drawing.Size(23, 22);
            this.btnTop.Text = "Top";
            this.btnTop.Click += new System.EventHandler(this.Top_Click);
            // 
            // btnBottom
            // 
            this.btnBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBottom.Image = ((System.Drawing.Image)(resources.GetObject("btnBottom.Image")));
            this.btnBottom.Name = "btnBottom";
            this.btnBottom.Size = new System.Drawing.Size(23, 22);
            this.btnBottom.Text = "Bottom";
            this.btnBottom.Click += new System.EventHandler(this.Bottom_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRemove.Image")));
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(90, 22);
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // btnApplyAll
            // 
            this.btnApplyAll.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnApplyAll.Image = ((System.Drawing.Image)(resources.GetObject("btnApplyAll.Image")));
            this.btnApplyAll.Name = "btnApplyAll";
            this.btnApplyAll.Size = new System.Drawing.Size(90, 22);
            this.btnApplyAll.Text = "Apply all";
            this.btnApplyAll.Click += new System.EventHandler(this.ApplyAll_Click);
            // 
            // ctxApply
            // 
            this.ctxApply.Image = ((System.Drawing.Image)(resources.GetObject("ctxApply.Image")));
            this.ctxApply.Name = "ctxApply";
            this.ctxApply.Size = new System.Drawing.Size(180, 22);
            this.ctxApply.Text = "Apply";
            this.ctxApply.ShortcutKeys = ((System.Windows.Forms.Keys)(System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q));
            this.ctxApply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // ctxApplyAll
            // 
            this.ctxApplyAll.Image = ((System.Drawing.Image)(resources.GetObject("ctxApplyAll.Image")));
            this.ctxApplyAll.Name = "ctxApplyAll";
            this.ctxApplyAll.Size = new System.Drawing.Size(180, 22);
            this.ctxApplyAll.Text = "Apply all";
            this.ctxApplyAll.Click += new System.EventHandler(this.ApplyAll_Click);
            // 
            // ctxAddVariable
            // 
            this.ctxAddVariable.Image = ((System.Drawing.Image)(resources.GetObject("ctxAddVariable.Image")));
            this.ctxAddVariable.Name = "ctxAddVariable";
            this.ctxAddVariable.Size = new System.Drawing.Size(180, 22);
            this.ctxAddVariable.Text = "Add variable";
            this.ctxAddVariable.Click += new System.EventHandler(this.AddVariable_Click);
            // 
            // ctxAddComment
            // 
            this.ctxAddComment.Image = ((System.Drawing.Image)(resources.GetObject("ctxAddComment.Image")));
            this.ctxAddComment.Name = "ctxAddComment";
            this.ctxAddComment.Size = new System.Drawing.Size(180, 22);
            this.ctxAddComment.Text = "Add comment";
            this.ctxAddComment.Click += new System.EventHandler(this.AddComment_Click);
            // 
            // ctxExpandEdit
            // 
            this.ctxExpandEdit.Image = ((System.Drawing.Image)(resources.GetObject("ctxExpandEdit.Image")));
            this.ctxExpandEdit.Name = "ctxExpandEdit";
            this.ctxExpandEdit.Size = new System.Drawing.Size(180, 22);
            this.ctxExpandEdit.Text = "Expand edit";
            this.ctxExpandEdit.Click += new System.EventHandler(this.ExpandEdit_Click);
            // 
            // ctxCopy
            // 
            this.ctxCopy.Image = ((System.Drawing.Image)(resources.GetObject("ctxCopy.Image")));
            this.ctxCopy.Name = "ctxCopy";
            this.ctxCopy.Size = new System.Drawing.Size(180, 22);
            this.ctxCopy.Text = "Copy";
            this.ctxCopy.ShortcutKeyDisplayString = "Ctrl+C";
            this.ctxCopy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // ctxPaste
            // 
            this.ctxPaste.Image = ((System.Drawing.Image)(resources.GetObject("ctxPaste.Image")));
            this.ctxPaste.Name = "ctxPaste";
            this.ctxPaste.Size = new System.Drawing.Size(180, 22);
            this.ctxPaste.Text = "Paste";
            this.ctxPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.ctxPaste.Click += new System.EventHandler(this.Paste_Click);
            // 
            // ctxUp
            // 
            this.ctxUp.Image = ((System.Drawing.Image)(resources.GetObject("ctxUp.Image")));
            this.ctxUp.Name = "ctxUp";
            this.ctxUp.Size = new System.Drawing.Size(180, 22);
            this.ctxUp.Text = "Move up";
            this.ctxUp.Click += new System.EventHandler(this.Up_Click);
            // 
            // ctxDown
            // 
            this.ctxDown.Image = ((System.Drawing.Image)(resources.GetObject("ctxDown.Image")));
            this.ctxDown.Name = "ctxDown";
            this.ctxDown.Size = new System.Drawing.Size(180, 22);
            this.ctxDown.Text = "Move down";
            this.ctxDown.Click += new System.EventHandler(this.Down_Click);
            // 
            // ctxTop
            // 
            this.ctxTop.Image = ((System.Drawing.Image)(resources.GetObject("ctxTop.Image")));
            this.ctxTop.Name = "ctxTop";
            this.ctxTop.Size = new System.Drawing.Size(180, 22);
            this.ctxTop.Text = "Move to top";
            this.ctxTop.Click += new System.EventHandler(this.Top_Click);
            // 
            // ctxBottom
            // 
            this.ctxBottom.Image = ((System.Drawing.Image)(resources.GetObject("ctxBottom.Image")));
            this.ctxBottom.Name = "ctxBottom";
            this.ctxBottom.Size = new System.Drawing.Size(180, 22);
            this.ctxBottom.Text = "Move to bottom";
            this.ctxBottom.Click += new System.EventHandler(this.Bottom_Click);
            // 
            // ctxRemove
            // 
            this.ctxRemove.Image = ((System.Drawing.Image)(resources.GetObject("ctxRemove.Image")));
            this.ctxRemove.Name = "ctxRemove";
            this.ctxRemove.Size = new System.Drawing.Size(180, 22);
            this.ctxRemove.Text = "Remove";
            this.ctxRemove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // sepToolbar1
            // 
            this.sepToolbar1.Name = "sepToolbar1";
            // 
            // sepToolbar2
            // 
            this.sepToolbar2.Name = "sepToolbar2";
            // 
            // sepContext1
            // 
            this.sepContext1.Name = "sepContext1";
            // 
            // sepContext2
            // 
            this.sepContext2.Name = "sepContext2";
            // 
            // sepContext3
            // 
            this.sepContext3.Name = "sepContext3";
            // 
            // dgvVariables
            // 
            this.dgvVariables.AllowUserToAddRows = false;
            this.dgvVariables.AllowUserToDeleteRows = false;
            this.dgvVariables.AllowUserToResizeRows = false;
            this.dgvVariables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVariables.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEnabled,
            this.colName,
            this.colValue,
            this.colLoadWhenActive});
            this.dgvVariables.ContextMenuStrip = this.ctxVariables;
            this.dgvVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVariables.Location = new System.Drawing.Point(0, 25);
            this.dgvVariables.Name = "dgvVariables";
            this.dgvVariables.RowHeadersVisible = false;
            this.dgvVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVariables.Size = new System.Drawing.Size(700, 315);
            this.dgvVariables.TabIndex = 1;
            this.dgvVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariables_CellDoubleClick);
            this.dgvVariables.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvVariables_CellFormatting);
            this.dgvVariables.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariables_CellValueChanged);
            this.dgvVariables.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvVariables_CurrentCellDirtyStateChanged);
            this.dgvVariables.SelectionChanged += new System.EventHandler(this.dgvVariables_SelectionChanged);
            this.dgvVariables.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvVariables_RowPostPaint);
            this.dgvVariables.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvVariables_MouseDown);
            this.dgvVariables.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvVariables_MouseUp);
            this.dgvVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvVariables_KeyDown);
            // 
            // colEnabled
            // 
            this.colEnabled.HeaderText = "";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colEnabled.Width = 28;
            this.colEnabled.MinimumWidth = 28;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colName.HeaderText = "Key";
            this.colName.Name = "colName";
            this.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colName.MinimumWidth = 80;
            this.colName.FillWeight = 35F;
            // 
            // colValue
            // 
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            this.colValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colValue.MinimumWidth = 80;
            this.colValue.FillWeight = 65F;
            // 
            // colLoadWhenActive
            // 
            this.colLoadWhenActive.HeaderText = "Activation policy";
            this.colLoadWhenActive.Name = "colLoadWhenActive";
            this.colLoadWhenActive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colLoadWhenActive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colLoadWhenActive.Width = 110;
            this.colLoadWhenActive.MinimumWidth = 80;
            // 
            // ctxVariables
            // 
            this.ctxVariables.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxApply,
            this.ctxApplyAll,
            this.sepContext1,
            this.ctxAddVariable,
            this.ctxAddComment,
            this.ctxExpandEdit,
            this.ctxCopy,
            this.ctxPaste,
            this.sepContext2,
            this.ctxUp,
            this.ctxDown,
            this.ctxTop,
            this.ctxBottom,
            this.sepContext3,
            this.ctxRemove});
            this.ctxVariables.Name = "ctxVariables";
            this.ctxVariables.Opening += new System.ComponentModel.CancelEventHandler(this.ctxVariables_Opening);
            // 
            // RegisteredVariableViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvVariables);
            this.Controls.Add(this.toolStrip);
            this.Name = "RegisteredVariableViewer";
            this.Size = new System.Drawing.Size(700, 340);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).EndInit();
            this.ctxVariables.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnAddVariable;
        private System.Windows.Forms.ToolStripButton btnAddComment;
        private System.Windows.Forms.ToolStripButton btnExpandEdit;
        private System.Windows.Forms.ToolStripButton btnApply;
        private System.Windows.Forms.ToolStripButton btnUp;
        private System.Windows.Forms.ToolStripButton btnDown;
        private System.Windows.Forms.ToolStripButton btnTop;
        private System.Windows.Forms.ToolStripButton btnBottom;
        private System.Windows.Forms.ToolStripButton btnRemove;
        private System.Windows.Forms.ToolStripButton btnApplyAll;
        private System.Windows.Forms.ToolStripSeparator sepToolbar1;
        private System.Windows.Forms.ToolStripSeparator sepToolbar2;
        private Triggernometry.UI.CustomControls.DataGridViewEx dgvVariables;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewComboBoxColumn colLoadWhenActive;
        private System.Windows.Forms.ContextMenuStrip ctxVariables;
        private System.Windows.Forms.ToolStripMenuItem ctxApply;
        private System.Windows.Forms.ToolStripMenuItem ctxApplyAll;
        private System.Windows.Forms.ToolStripMenuItem ctxAddVariable;
        private System.Windows.Forms.ToolStripMenuItem ctxAddComment;
        private System.Windows.Forms.ToolStripMenuItem ctxExpandEdit;
        private System.Windows.Forms.ToolStripMenuItem ctxCopy;
        private System.Windows.Forms.ToolStripMenuItem ctxPaste;
        private System.Windows.Forms.ToolStripMenuItem ctxUp;
        private System.Windows.Forms.ToolStripMenuItem ctxDown;
        private System.Windows.Forms.ToolStripMenuItem ctxTop;
        private System.Windows.Forms.ToolStripMenuItem ctxBottom;
        private System.Windows.Forms.ToolStripMenuItem ctxRemove;
        private System.Windows.Forms.ToolStripSeparator sepContext1;
        private System.Windows.Forms.ToolStripSeparator sepContext2;
        private System.Windows.Forms.ToolStripSeparator sepContext3;
    }
}
