namespace Triggernometry.UI.CustomControls
{
    partial class ConditionViewer
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
            if (disposing)
            {
                ClearReverseLookup();
                components?.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionViewer));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddGroup = new System.Windows.Forms.ToolStripButton();
            this.btnAddCondition = new System.Windows.Forms.ToolStripButton();
            this.btnAddComment = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.pnlEditorSingle = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.expRight = new Triggernometry.UI.CustomControls.ExpressionTextBox();
            this.cbxExpRType = new System.Windows.Forms.ComboBox();
            this.lblLeftExpression = new System.Windows.Forms.Label();
            this.cbxExpLType = new System.Windows.Forms.ComboBox();
            this.expLeft = new Triggernometry.UI.CustomControls.ExpressionTextBox();
            this.cbxOpType = new System.Windows.Forms.ComboBox();
            this.cbxComparison = new System.Windows.Forms.ComboBox();
            this.operatorToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblRightExpression = new System.Windows.Forms.Label();
            this.lblComparisonDescription = new System.Windows.Forms.Label();
            this.pnlEditorGroup = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbxGroupingType = new System.Windows.Forms.ComboBox();
            this.lblGroupingType = new System.Windows.Forms.Label();
            this.pnlEditorComment = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTree = new System.Windows.Forms.Panel();
            this.trvNodes = new Triggernometry.UI.CustomControls.TreeViewEx();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxAddGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxAddCondition = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxAddComment = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.ctxCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxPasteOver = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ctxTest = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxExpandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxCollapseAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ctxRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tableLayoutPanelMain.SuspendLayout();
            this.pnlTree.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlEditorSingle.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlEditorGroup.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlEditorComment.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "function");
            this.imageList1.Images.SetKeyName(1, "group_and");
            this.imageList1.Images.SetKeyName(2, "group_or");
            this.imageList1.Images.SetKeyName(3, "group_not");
            this.imageList1.Images.SetKeyName(4, "group_xor");
            this.imageList1.Images.SetKeyName(5, "comment");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddCondition,
            this.btnAddGroup,
            this.btnAddComment,
            this.btnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(839, 25);
            this.toolStrip1.TabStop = false;
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Image = ((System.Drawing.Image)(resources.GetObject("btnAddGroup.Image")));
            this.btnAddGroup.Margin = new System.Windows.Forms.Padding(0, 1, 8, 2);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(162, 22);
            this.btnAddGroup.Text = "Add group";
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // btnAddCondition
            // 
            this.btnAddCondition.Image = ((System.Drawing.Image)(resources.GetObject("btnAddCondition.Image")));
            this.btnAddCondition.Margin = new System.Windows.Forms.Padding(0, 1, 8, 2);
            this.btnAddCondition.Name = "btnAddCondition";
            this.btnAddCondition.Size = new System.Drawing.Size(162, 22);
            this.btnAddCondition.Text = "Add condition";
            this.btnAddCondition.Click += new System.EventHandler(this.btnAddCondition_Click);
            //
            // btnAddComment
            //
            this.btnAddComment.Image = ((System.Drawing.Image)(resources.GetObject("btnAddComment.Image")));
            this.btnAddComment.Margin = new System.Windows.Forms.Padding(0, 1, 8, 2);
            this.btnAddComment.Name = "btnAddComment";
            this.btnAddComment.Size = new System.Drawing.Size(162, 22);
            this.btnAddComment.Text = "Add comment";
            this.btnAddComment.Click += new System.EventHandler(this.btnAddComment_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0, 1, 8, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 22);
            this.btnDelete.Text = "Remove";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnlEditorSingle
            // 
            this.pnlEditorSingle.AutoSize = true;
            this.pnlEditorSingle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlEditorSingle.Controls.Add(this.tableLayoutPanel1);
            this.pnlEditorSingle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEditorSingle.Location = new System.Drawing.Point(0, 56);
            this.pnlEditorSingle.Name = "pnlEditorSingle";
            this.pnlEditorSingle.Size = new System.Drawing.Size(200, 220);
            this.pnlEditorSingle.TabStop = false;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.cbxExpLType, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLeftExpression, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.expLeft, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbxComparison, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbxOpType, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblComparisonDescription, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbxExpRType, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblRightExpression, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.expRight, 3, 2);
            this.tableLayoutPanel1.SetColumnSpan(this.cbxExpLType, 2);
            this.tableLayoutPanel1.SetColumnSpan(this.cbxExpRType, 2);
            this.tableLayoutPanel1.SetColumnSpan(this.lblComparisonDescription, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(831, 81);
            this.tableLayoutPanel1.TabStop = false;
            // 
            // expRight
            // 
            this.expRight.AutoSize = true;
            this.expRight.Dock = System.Windows.Forms.DockStyle.Top;
            this.expRight.Expression = "";
            this.expRight.ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum.String;
            this.expRight.Location = new System.Drawing.Point(3, 197);
            this.expRight.Name = "expRight";
            this.expRight.Size = new System.Drawing.Size(194, 20);
            this.expRight.TabIndex = 5;
            // 
            // cbxExpRType
            // 
            this.cbxExpRType.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxExpRType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxExpRType.FormattingEnabled = true;
            this.cbxExpRType.Items.AddRange(new object[] {
            "String",
            "Numeric"});
            this.cbxExpRType.Location = new System.Drawing.Point(3, 170);
            this.cbxExpRType.Name = "cbxExpRType";
            this.cbxExpRType.Size = new System.Drawing.Size(194, 21);
            this.cbxExpRType.TabIndex = 4;
            this.cbxExpRType.SelectedIndexChanged += new System.EventHandler(this.cbxExpRType_SelectedIndexChanged);
            //
            // lblLeftExpression
            //
            this.lblLeftExpression.AutoSize = true;
            this.lblLeftExpression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLeftExpression.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLeftExpression.Name = "lblLeftExpression";
            this.lblLeftExpression.Text = "L";
            // 
            // cbxExpLType
            // 
            this.cbxExpLType.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxExpLType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxExpLType.FormattingEnabled = true;
            this.cbxExpLType.Items.AddRange(new object[] {
            "String",
            "Numeric"});
            this.cbxExpLType.Location = new System.Drawing.Point(3, 32);
            this.cbxExpLType.Name = "cbxExpLType";
            this.cbxExpLType.Size = new System.Drawing.Size(194, 21);
            this.cbxExpLType.TabIndex = 0;
            this.cbxExpLType.SelectedIndexChanged += new System.EventHandler(this.cbxExpLType_SelectedIndexChanged);
            // 
            // expLeft
            // 
            this.expLeft.AutoSize = true;
            this.expLeft.Dock = System.Windows.Forms.DockStyle.Top;
            this.expLeft.Expression = "";
            this.expLeft.ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum.String;
            this.expLeft.Location = new System.Drawing.Point(3, 59);
            this.expLeft.Name = "expLeft";
            this.expLeft.Size = new System.Drawing.Size(194, 20);
            this.expLeft.TabIndex = 1;
            //
            // cbxComparison
            //
            this.cbxComparison.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxComparison.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxComparison.FormattingEnabled = true;
            this.cbxComparison.Name = "cbxComparison";
            this.cbxComparison.Size = new System.Drawing.Size(160, 21);
            this.cbxComparison.TabIndex = 2;
            this.cbxComparison.Items.AddRange(new object[] { "Numeric", "Text", "Regex", "List" });
            this.cbxComparison.SelectedIndexChanged += new System.EventHandler(this.cbxComparison_SelectedIndexChanged);
            //
            // cbxOpType
            //
            this.cbxOpType.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxOpType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOpType.FormattingEnabled = true;
            this.cbxOpType.Name = "cbxOpType";
            this.cbxOpType.Size = new System.Drawing.Size(160, 21);
            this.cbxOpType.TabIndex = 3;
            this.cbxOpType.SelectedIndexChanged += new System.EventHandler(this.cbxOpType_SelectedIndexChanged);
            //
            // lblRightExpression
            //
            this.lblRightExpression.AutoSize = true;
            this.lblRightExpression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRightExpression.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblRightExpression.Name = "lblRightExpression";
            this.lblRightExpression.Text = "R";
            //
            // lblComparisonDescription
            //
            this.lblComparisonDescription.AutoSize = true;
            this.lblComparisonDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblComparisonDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblComparisonDescription.Name = "lblComparisonDescription";
            // 
            // pnlEditorGroup
            // 
            this.pnlEditorGroup.AutoSize = true;
            this.pnlEditorGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlEditorGroup.Controls.Add(this.tableLayoutPanel2);
            this.pnlEditorGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEditorGroup.Location = new System.Drawing.Point(0, 0);
            this.pnlEditorGroup.Name = "pnlEditorGroup";
            this.pnlEditorGroup.Size = new System.Drawing.Size(200, 56);
            this.pnlEditorGroup.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.cbxGroupingType, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblGroupingType, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(200, 56);
            this.tableLayoutPanel2.TabStop = false;
            // 
            // cbxGroupingType
            // 
            this.cbxGroupingType.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxGroupingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxGroupingType.FormattingEnabled = true;
            this.cbxGroupingType.Items.AddRange(new object[] {
            "All conditions must be true (AND)",
            "At least one condition must be true (OR)",
            "Only one condition must be true (XOR)",
            "None of the conditions may be true (NOT)"});
            this.cbxGroupingType.Location = new System.Drawing.Point(3, 32);
            this.cbxGroupingType.Name = "cbxGroupingType";
            this.cbxGroupingType.Size = new System.Drawing.Size(194, 21);
            this.cbxGroupingType.TabIndex = 0;
            this.cbxGroupingType.SelectedIndexChanged += new System.EventHandler(this.cbxGroupingType_SelectedIndexChanged);
            // 
            // lblGroupingType
            // 
            this.lblGroupingType.AutoSize = true;
            this.lblGroupingType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGroupingType.Location = new System.Drawing.Point(3, 0);
            this.lblGroupingType.Name = "lblGroupingType";
            this.lblGroupingType.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lblGroupingType.Size = new System.Drawing.Size(194, 29);
            this.lblGroupingType.TabStop = false;
            this.lblGroupingType.Text = "Grouping type";
            // 
            // pnlEditorComment
            //
            this.pnlEditorComment.Controls.Add(this.tableLayoutPanel3);
            this.pnlEditorComment.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEditorComment.AutoSize = true;
            this.pnlEditorComment.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlEditorComment.Location = new System.Drawing.Point(0, 0);
            this.pnlEditorComment.Name = "pnlEditorComment";
            this.pnlEditorComment.Size = new System.Drawing.Size(200, 546);
            this.pnlEditorComment.TabStop = false;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.lblComment, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtComment, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(200, 546);
            this.tableLayoutPanel3.TabStop = false;
            //
            // lblComment
            //
            this.lblComment.AutoSize = true;
            this.lblComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblComment.Location = new System.Drawing.Point(3, 0);
            this.lblComment.Name = "lblComment";
            this.lblComment.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.lblComment.Size = new System.Drawing.Size(194, 29);
            this.lblComment.TabStop = false;
            this.lblComment.Text = "Comment text";
            //
            // txtComment
            //
            this.txtComment.AcceptsReturn = true;
            this.txtComment.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtComment.Location = new System.Drawing.Point(3, 32);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtComment.Size = new System.Drawing.Size(194, 80);
            this.txtComment.TabIndex = 0;
            this.txtComment.TextChanged += new System.EventHandler(this.txtComment_TextChanged);
            //
            // tableLayoutPanelMain
            //
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Controls.Add(this.toolStrip1, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pnlTree, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanelMain.AutoSize = true;
            this.tableLayoutPanelMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(839, 601);
            this.tableLayoutPanelMain.TabStop = false;
            //
            // pnlTree
            //
            this.pnlTree.AutoSize = true;
            this.pnlTree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlTree.Controls.Add(this.trvNodes);
            this.pnlTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTree.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTree.MinimumSize = new System.Drawing.Size(0, 60);
            this.pnlTree.Name = "pnlTree";
            this.pnlTree.Size = new System.Drawing.Size(839, 60);
            this.pnlTree.TabStop = false;
            //
            // trvNodes
            // 
            this.trvNodes.AllowDrop = true;
            this.trvNodes.CheckBoxes = true;
            this.trvNodes.ContextMenuStrip = this.contextMenuStrip1;
            this.trvNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvNodes.Margin = new System.Windows.Forms.Padding(0);
            this.trvNodes.HideSelection = false;
            this.trvNodes.ImageIndex = 0;
            this.trvNodes.ImageList = this.imageList1;
            this.trvNodes.Location = new System.Drawing.Point(0, 0);
            this.trvNodes.Name = "trvNodes";
            this.trvNodes.SelectedImageIndex = 0;
            this.trvNodes.Size = new System.Drawing.Size(635, 60);
            this.trvNodes.TabStop = false;
            this.trvNodes.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.trvNodes_AfterCheck);
            this.trvNodes.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.trvNodes_ItemDrag);
            this.trvNodes.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvNodes_AfterSelect);
            this.trvNodes.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.trvNodes_NodeMouseClick);
            this.trvNodes.DragDrop += new System.Windows.Forms.DragEventHandler(this.trvNodes_DragDrop);
            this.trvNodes.DragEnter += new System.Windows.Forms.DragEventHandler(this.trvNodes_DragEnter);
            this.trvNodes.DragLeave += new System.EventHandler(this.trvNodes_DragLeave);
            this.trvNodes.DragOver += new System.Windows.Forms.DragEventHandler(this.trvNodes_DragOver);
            this.trvNodes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trvNodes_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxAdd,
            this.toolStripSeparator7,
            this.ctxCopy,
            this.ctxPaste,
            this.ctxPasteOver,
            this.toolStripSeparator2,
            this.ctxTest,
            this.ctxExpandAll,
            this.ctxCollapseAll,
            this.toolStripSeparator3,
            this.ctxRemove});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(202, 126);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // ctxAdd
            // 
            this.ctxAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxAddGroup,
            this.ctxAddCondition,
            this.ctxAddComment});
            this.ctxAdd.Image = ((System.Drawing.Image)(resources.GetObject("ctxAdd.Image")));
            this.ctxAdd.Name = "ctxAdd";
            this.ctxAdd.Size = new System.Drawing.Size(201, 22);
            this.ctxAdd.Text = "Add";
            // 
            // ctxAddGroup
            // 
            this.ctxAddGroup.Image = ((System.Drawing.Image)(resources.GetObject("ctxAddGroup.Image")));
            this.ctxAddGroup.Name = "ctxAddGroup";
            this.ctxAddGroup.Size = new System.Drawing.Size(162, 22);
            this.ctxAddGroup.Text = "Condition group";
            this.ctxAddGroup.Click += new System.EventHandler(this.ctxBtnConditionGroup_Click);
            // 
            // ctxAddCondition
            // 
            this.ctxAddCondition.Image = ((System.Drawing.Image)(resources.GetObject("ctxAddCondition.Image")));
            this.ctxAddCondition.Name = "ctxAddCondition";
            this.ctxAddCondition.Size = new System.Drawing.Size(162, 22);
            this.ctxAddCondition.Text = "Condition";
            this.ctxAddCondition.Click += new System.EventHandler(this.ctxBtnCondition_Click);
            //
            // ctxAddComment
            //
            this.ctxAddComment.Image = ((System.Drawing.Image)(resources.GetObject("ctxAddComment.Image")));
            this.ctxAddComment.Name = "ctxAddComment";
            this.ctxAddComment.Size = new System.Drawing.Size(162, 22);
            this.ctxAddComment.Text = "Comment";
            this.ctxAddComment.Click += new System.EventHandler(this.btnAddComment_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(198, 6);
            // 
            // ctxCopy
            // 
            this.ctxCopy.Image = ((System.Drawing.Image)(resources.GetObject("ctxCopy.Image")));
            this.ctxCopy.Name = "ctxCopy";
            this.ctxCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.ctxCopy.Size = new System.Drawing.Size(201, 22);
            this.ctxCopy.Text = "Copy";
            this.ctxCopy.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // ctxPaste
            // 
            this.ctxPaste.Image = ((System.Drawing.Image)(resources.GetObject("ctxPaste.Image")));
            this.ctxPaste.Name = "ctxPaste";
            this.ctxPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.ctxPaste.Size = new System.Drawing.Size(201, 22);
            this.ctxPaste.Text = "Paste";
            this.ctxPaste.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // ctxPasteOver
            // 
            this.ctxPasteOver.Image = ((System.Drawing.Image)(resources.GetObject("ctxPasteOver.Image")));
            this.ctxPasteOver.Name = "ctxPasteOver";
            this.ctxPasteOver.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
            | System.Windows.Forms.Keys.V)));
            this.ctxPasteOver.Size = new System.Drawing.Size(201, 22);
            this.ctxPasteOver.Text = "Paste over";
            this.ctxPasteOver.Click += new System.EventHandler(this.pasteOverToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(198, 6);
            // 
            // ctxTest
            // 
            this.ctxTest.Name = "ctxTest";
            this.ctxTest.Size = new System.Drawing.Size(201, 22);
            this.ctxTest.Text = "Test Condition";
            this.ctxTest.Click += new System.EventHandler(this.testStripMenuItem_Click);
            // 
            // ctxExpandAll
            // 
            this.ctxExpandAll.Image = ((System.Drawing.Image)(resources.GetObject("ctxExpandAll.Image")));
            this.ctxExpandAll.Name = "ctxExpandAll";
            this.ctxExpandAll.Size = new System.Drawing.Size(201, 22);
            this.ctxExpandAll.Text = "Expand All";
            this.ctxExpandAll.Click += new System.EventHandler(this.expandAllStripMenuItem_Click);
            // 
            // ctxCollapseAll
            // 
            this.ctxCollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("ctxCollapseAll.Image")));
            this.ctxCollapseAll.Name = "ctxCollapseAll";
            this.ctxCollapseAll.Size = new System.Drawing.Size(201, 22);
            this.ctxCollapseAll.Text = "Collapse All";
            this.ctxCollapseAll.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(198, 6);
            // 
            // ctxRemove
            // 
            this.ctxRemove.Image = ((System.Drawing.Image)(resources.GetObject("ctxRemove.Image")));
            this.ctxRemove.Name = "ctxRemove";
            this.ctxRemove.Size = new System.Drawing.Size(201, 22);
            this.ctxRemove.Text = "Remove";
            this.ctxRemove.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Controls.Add(this.pnlEditorComment);
            this.panel3.Controls.Add(this.pnlEditorSingle);
            this.panel3.Controls.Add(this.pnlEditorGroup);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Padding = new System.Windows.Forms.Padding(4);
            this.panel3.Location = new System.Drawing.Point(0, 30);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 546);
            this.panel3.TabStop = false;
            // 
            // ConditionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "ConditionViewer";
            this.Size = new System.Drawing.Size(839, 601);
            this.pnlTree.ResumeLayout(false);
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlEditorSingle.ResumeLayout(false);
            this.pnlEditorSingle.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnlEditorGroup.ResumeLayout(false);
            this.pnlEditorGroup.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.pnlEditorComment.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlTree;
        private TreeViewEx trvNodes;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddCondition;
        private System.Windows.Forms.ToolStripButton btnAddGroup;
        private System.Windows.Forms.ToolStripButton btnAddComment;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.Panel pnlEditorSingle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblRightExpression;
        private System.Windows.Forms.Label lblComparisonDescription;
        private System.Windows.Forms.Label lblLeftExpression;
        private System.Windows.Forms.ComboBox cbxExpLType;
        private ExpressionTextBox expLeft;
        private System.Windows.Forms.ComboBox cbxOpType;
        private System.Windows.Forms.ComboBox cbxComparison;
        private System.Windows.Forms.ToolTip operatorToolTip;
        private ExpressionTextBox expRight;
        private System.Windows.Forms.ComboBox cbxExpRType;
        private System.Windows.Forms.Panel pnlEditorGroup;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox cbxGroupingType;
        private System.Windows.Forms.Label lblGroupingType;
        private System.Windows.Forms.Panel pnlEditorComment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ctxAdd;
        private System.Windows.Forms.ToolStripMenuItem ctxAddGroup;
        private System.Windows.Forms.ToolStripMenuItem ctxAddCondition;
        private System.Windows.Forms.ToolStripMenuItem ctxAddComment;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem ctxCopy;
        private System.Windows.Forms.ToolStripMenuItem ctxPaste;
        private System.Windows.Forms.ToolStripMenuItem ctxPasteOver;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ctxTest;
        private System.Windows.Forms.ToolStripMenuItem ctxExpandAll;
        private System.Windows.Forms.ToolStripMenuItem ctxCollapseAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem ctxRemove;
        private System.Windows.Forms.Panel panel3;
    }
}
