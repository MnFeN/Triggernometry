namespace Triggernometry.UI.Forms
{
    partial class SearchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm));
            this.pnlBottomSpacing = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableSearch = new System.Windows.Forms.TableLayoutPanel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblRegExp = new System.Windows.Forms.Label();
            this.expSearchTerm = new Triggernometry.UI.CustomControls.ExpressionTextBox();
            this.pnlSearchResults = new System.Windows.Forms.Panel();
            this.dgvResults = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.hdrMatchedTrigger = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hdrMatchType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stsSearchResults = new System.Windows.Forms.StatusStrip();
            this.lblResultInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitSearch = new System.Windows.Forms.SplitContainer();
            this.pnlSearchOptions = new System.Windows.Forms.Panel();
            this.tableSearchOptions = new System.Windows.Forms.TableLayoutPanel();
            this.clbMisc = new System.Windows.Forms.CheckedListBox();
            this.lblMisc = new System.Windows.Forms.Label();
            this.clbScope = new System.Windows.Forms.CheckedListBox();
            this.lblScope = new System.Windows.Forms.Label();
            this.clbRepos = new System.Windows.Forms.CheckedListBox();
            this.lblRepos = new System.Windows.Forms.Label();
            this.clbActions = new System.Windows.Forms.CheckedListBox();
            this.lblActions = new System.Windows.Forms.Label();
            this.lblTriggers = new System.Windows.Forms.Label();
            this.clbTriggers = new System.Windows.Forms.CheckedListBox();
            this.tlsModify = new System.Windows.Forms.ToolStrip();
            this.btnModifySelection = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnCheckAll = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUncheckAll = new System.Windows.Forms.ToolStripMenuItem();
            this.capSearchScope = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.tableSearch.SuspendLayout();
            this.pnlSearchResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.stsSearchResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSearch)).BeginInit();
            this.splitSearch.Panel1.SuspendLayout();
            this.splitSearch.Panel2.SuspendLayout();
            this.splitSearch.SuspendLayout();
            this.pnlSearchOptions.SuspendLayout();
            this.tableSearchOptions.SuspendLayout();
            this.tlsModify.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottomSpacing
            // 
            this.pnlBottomSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomSpacing.Location = new System.Drawing.Point(10, 556);
            this.pnlBottomSpacing.Name = "pnlBottomSpacing";
            this.pnlBottomSpacing.Size = new System.Drawing.Size(764, 10);
            this.pnlBottomSpacing.TabIndex = 21;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClose.Location = new System.Drawing.Point(10, 566);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(764, 35);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tableSearch
            // 
            this.tableSearch.AutoSize = true;
            this.tableSearch.ColumnCount = 3;
            this.tableSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableSearch.Controls.Add(this.btnSearch, 2, 0);
            this.tableSearch.Controls.Add(this.lblRegExp, 0, 0);
            this.tableSearch.Controls.Add(this.expSearchTerm, 1, 0);
            this.tableSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableSearch.Location = new System.Drawing.Point(0, 0);
            this.tableSearch.Name = "tableSearch";
            this.tableSearch.RowCount = 1;
            this.tableSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSearch.Size = new System.Drawing.Size(551, 26);
            this.tableSearch.TabIndex = 22;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(448, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 20);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblRegExp
            // 
            this.lblRegExp.AutoSize = true;
            this.lblRegExp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRegExp.Location = new System.Drawing.Point(3, 0);
            this.lblRegExp.Name = "lblRegExp";
            this.lblRegExp.Size = new System.Drawing.Size(97, 26);
            this.lblRegExp.TabIndex = 0;
            this.lblRegExp.Text = "Regular expression";
            this.lblRegExp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // expSearchTerm
            // 
            this.expSearchTerm.AutoSize = true;
            this.expSearchTerm.Dock = System.Windows.Forms.DockStyle.Top;
            this.expSearchTerm.Expression = "";
            this.expSearchTerm.ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum.Regex;
            this.expSearchTerm.Location = new System.Drawing.Point(106, 3);
            this.expSearchTerm.Name = "expSearchTerm";
            this.expSearchTerm.ReadOnly = false;
            this.expSearchTerm.RegexSupportsExpressions = false;
            this.expSearchTerm.Size = new System.Drawing.Size(336, 20);
            this.expSearchTerm.TabIndex = 1;
            // 
            // pnlSearchResults
            // 
            this.pnlSearchResults.Controls.Add(this.dgvResults);
            this.pnlSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearchResults.Location = new System.Drawing.Point(0, 26);
            this.pnlSearchResults.Name = "pnlSearchResults";
            this.pnlSearchResults.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pnlSearchResults.Size = new System.Drawing.Size(551, 498);
            this.pnlSearchResults.TabIndex = 24;
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.AllowUserToResizeColumns = false;
            this.dgvResults.AllowUserToResizeRows = false;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.hdrMatchedTrigger,
            this.hdrMatchType});
            this.dgvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvResults.Location = new System.Drawing.Point(0, 5);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.RowHeadersVisible = false;
            this.dgvResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResults.ShowCellErrors = false;
            this.dgvResults.ShowEditingIcon = false;
            this.dgvResults.ShowRowErrors = false;
            this.dgvResults.Size = new System.Drawing.Size(551, 493);
            this.dgvResults.TabIndex = 23;
            this.dgvResults.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvResults_CellDoubleClick);
            // 
            // hdrMatchedTrigger
            // 
            this.hdrMatchedTrigger.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.hdrMatchedTrigger.HeaderText = "Matched item";
            this.hdrMatchedTrigger.Name = "hdrMatchedTrigger";
            this.hdrMatchedTrigger.ReadOnly = true;
            // 
            // hdrMatchType
            // 
            this.hdrMatchType.HeaderText = "Match type";
            this.hdrMatchType.Name = "hdrMatchType";
            this.hdrMatchType.ReadOnly = true;
            this.hdrMatchType.Width = 200;
            // 
            // stsSearchResults
            // 
            this.stsSearchResults.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblResultInfo});
            this.stsSearchResults.Location = new System.Drawing.Point(10, 534);
            this.stsSearchResults.Name = "stsSearchResults";
            this.stsSearchResults.Size = new System.Drawing.Size(764, 22);
            this.stsSearchResults.SizingGrip = false;
            this.stsSearchResults.TabIndex = 26;
            this.stsSearchResults.Text = "stsSearchResults";
            // 
            // lblResultInfo
            // 
            this.lblResultInfo.Name = "lblResultInfo";
            this.lblResultInfo.Size = new System.Drawing.Size(60, 17);
            this.lblResultInfo.Text = "No results";
            // 
            // splitSearch
            // 
            this.splitSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSearch.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitSearch.Location = new System.Drawing.Point(10, 10);
            this.splitSearch.Name = "splitSearch";
            // 
            // splitSearch.Panel1
            // 
            this.splitSearch.Panel1.Controls.Add(this.pnlSearchResults);
            this.splitSearch.Panel1.Controls.Add(this.tableSearch);
            // 
            // splitSearch.Panel2
            // 
            this.splitSearch.Panel2.Controls.Add(this.pnlSearchOptions);
            this.splitSearch.Panel2.Controls.Add(this.tlsModify);
            this.splitSearch.Panel2.Controls.Add(this.capSearchScope);
            this.splitSearch.Panel2MinSize = 200;
            this.splitSearch.Size = new System.Drawing.Size(764, 524);
            this.splitSearch.SplitterDistance = 551;
            this.splitSearch.TabIndex = 27;
            // 
            // pnlSearchOptions
            // 
            this.pnlSearchOptions.AutoScroll = true;
            this.pnlSearchOptions.Controls.Add(this.tableSearchOptions);
            this.pnlSearchOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearchOptions.Location = new System.Drawing.Point(0, 55);
            this.pnlSearchOptions.Name = "pnlSearchOptions";
            this.pnlSearchOptions.Size = new System.Drawing.Size(209, 469);
            this.pnlSearchOptions.TabIndex = 8;
            // 
            // tableSearchOptions
            // 
            this.tableSearchOptions.AutoSize = true;
            this.tableSearchOptions.ColumnCount = 1;
            this.tableSearchOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableSearchOptions.Controls.Add(this.clbMisc, 0, 9);
            this.tableSearchOptions.Controls.Add(this.lblMisc, 0, 8);
            this.tableSearchOptions.Controls.Add(this.clbScope, 0, 7);
            this.tableSearchOptions.Controls.Add(this.lblScope, 0, 6);
            this.tableSearchOptions.Controls.Add(this.clbRepos, 0, 5);
            this.tableSearchOptions.Controls.Add(this.lblRepos, 0, 4);
            this.tableSearchOptions.Controls.Add(this.clbActions, 0, 3);
            this.tableSearchOptions.Controls.Add(this.lblActions, 0, 2);
            this.tableSearchOptions.Controls.Add(this.lblTriggers, 0, 0);
            this.tableSearchOptions.Controls.Add(this.clbTriggers, 0, 1);
            this.tableSearchOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableSearchOptions.Location = new System.Drawing.Point(0, 0);
            this.tableSearchOptions.Name = "tableSearchOptions";
            this.tableSearchOptions.RowCount = 10;
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableSearchOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableSearchOptions.Size = new System.Drawing.Size(209, 420);
            this.tableSearchOptions.TabIndex = 7;
            // 
            // clbMisc
            // 
            this.clbMisc.CheckOnClick = true;
            this.clbMisc.Dock = System.Windows.Forms.DockStyle.Top;
            this.clbMisc.FormattingEnabled = true;
            this.clbMisc.Items.AddRange(new object[] {
            "Include folders",
            "Include disabled"});
            this.clbMisc.Location = new System.Drawing.Point(3, 383);
            this.clbMisc.Name = "clbMisc";
            this.clbMisc.Size = new System.Drawing.Size(203, 34);
            this.clbMisc.TabIndex = 10;
            this.clbMisc.SelectedIndexChanged += new System.EventHandler(this.clbTriggers_SelectedIndexChanged);
            // 
            // lblMisc
            // 
            this.lblMisc.AutoSize = true;
            this.lblMisc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMisc.Location = new System.Drawing.Point(3, 351);
            this.lblMisc.Name = "lblMisc";
            this.lblMisc.Padding = new System.Windows.Forms.Padding(0, 15, 0, 1);
            this.lblMisc.Size = new System.Drawing.Size(203, 29);
            this.lblMisc.TabIndex = 9;
            this.lblMisc.Text = "Miscellaneous";
            // 
            // clbScope
            // 
            this.clbScope.CheckOnClick = true;
            this.clbScope.Dock = System.Windows.Forms.DockStyle.Top;
            this.clbScope.FormattingEnabled = true;
            this.clbScope.Items.AddRange(new object[] {
            "Local",
            "Remote"});
            this.clbScope.Location = new System.Drawing.Point(3, 314);
            this.clbScope.Name = "clbScope";
            this.clbScope.Size = new System.Drawing.Size(203, 34);
            this.clbScope.TabIndex = 8;
            this.clbScope.SelectedIndexChanged += new System.EventHandler(this.clbTriggers_SelectedIndexChanged);
            // 
            // lblScope
            // 
            this.lblScope.AutoSize = true;
            this.lblScope.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblScope.Location = new System.Drawing.Point(3, 282);
            this.lblScope.Name = "lblScope";
            this.lblScope.Padding = new System.Windows.Forms.Padding(0, 15, 0, 1);
            this.lblScope.Size = new System.Drawing.Size(203, 29);
            this.lblScope.TabIndex = 7;
            this.lblScope.Text = "Object scope";
            // 
            // clbRepos
            // 
            this.clbRepos.CheckOnClick = true;
            this.clbRepos.Dock = System.Windows.Forms.DockStyle.Top;
            this.clbRepos.FormattingEnabled = true;
            this.clbRepos.Items.AddRange(new object[] {
            "Name",
            "Address"});
            this.clbRepos.Location = new System.Drawing.Point(3, 245);
            this.clbRepos.Name = "clbRepos";
            this.clbRepos.Size = new System.Drawing.Size(203, 34);
            this.clbRepos.TabIndex = 6;
            this.clbRepos.SelectedIndexChanged += new System.EventHandler(this.clbTriggers_SelectedIndexChanged);
            // 
            // lblRepos
            // 
            this.lblRepos.AutoSize = true;
            this.lblRepos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRepos.Location = new System.Drawing.Point(3, 213);
            this.lblRepos.Name = "lblRepos";
            this.lblRepos.Padding = new System.Windows.Forms.Padding(0, 15, 0, 1);
            this.lblRepos.Size = new System.Drawing.Size(203, 29);
            this.lblRepos.TabIndex = 5;
            this.lblRepos.Text = "Repositories";
            // 
            // clbActions
            // 
            this.clbActions.CheckOnClick = true;
            this.clbActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.clbActions.FormattingEnabled = true;
            this.clbActions.Items.AddRange(new object[] {
            "Description",
            "Condition",
            "Action-specific properties"});
            this.clbActions.Location = new System.Drawing.Point(3, 161);
            this.clbActions.Name = "clbActions";
            this.clbActions.Size = new System.Drawing.Size(203, 49);
            this.clbActions.TabIndex = 4;
            this.clbActions.SelectedIndexChanged += new System.EventHandler(this.clbTriggers_SelectedIndexChanged);
            // 
            // lblActions
            // 
            this.lblActions.AutoSize = true;
            this.lblActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblActions.Location = new System.Drawing.Point(3, 129);
            this.lblActions.Name = "lblActions";
            this.lblActions.Padding = new System.Windows.Forms.Padding(0, 15, 0, 1);
            this.lblActions.Size = new System.Drawing.Size(203, 29);
            this.lblActions.TabIndex = 3;
            this.lblActions.Text = "Actions";
            // 
            // lblTriggers
            // 
            this.lblTriggers.AutoSize = true;
            this.lblTriggers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTriggers.Location = new System.Drawing.Point(3, 0);
            this.lblTriggers.Name = "lblTriggers";
            this.lblTriggers.Padding = new System.Windows.Forms.Padding(0, 15, 0, 1);
            this.lblTriggers.Size = new System.Drawing.Size(203, 29);
            this.lblTriggers.TabIndex = 1;
            this.lblTriggers.Text = "Triggers";
            // 
            // clbTriggers
            // 
            this.clbTriggers.CheckOnClick = true;
            this.clbTriggers.Dock = System.Windows.Forms.DockStyle.Top;
            this.clbTriggers.FormattingEnabled = true;
            this.clbTriggers.Items.AddRange(new object[] {
            "ID",
            "Name",
            "Description",
            "Condition",
            "Full path",
            "Regular expression"});
            this.clbTriggers.Location = new System.Drawing.Point(3, 32);
            this.clbTriggers.Name = "clbTriggers";
            this.clbTriggers.Size = new System.Drawing.Size(203, 94);
            this.clbTriggers.TabIndex = 2;
            this.clbTriggers.SelectedIndexChanged += new System.EventHandler(this.clbTriggers_SelectedIndexChanged);
            // 
            // tlsModify
            // 
            this.tlsModify.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsModify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnModifySelection});
            this.tlsModify.Location = new System.Drawing.Point(0, 30);
            this.tlsModify.Name = "tlsModify";
            this.tlsModify.Size = new System.Drawing.Size(209, 25);
            this.tlsModify.TabIndex = 9;
            this.tlsModify.Text = "toolStrip1";
            // 
            // btnModifySelection
            // 
            this.btnModifySelection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCheckAll,
            this.btnUncheckAll});
            this.btnModifySelection.Image = ((System.Drawing.Image)(resources.GetObject("btnModifySelection.Image")));
            this.btnModifySelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnModifySelection.Name = "btnModifySelection";
            this.btnModifySelection.Size = new System.Drawing.Size(124, 22);
            this.btnModifySelection.Text = "Modify selection";
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(135, 22);
            this.btnCheckAll.Text = "Check all";
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(135, 22);
            this.btnUncheckAll.Text = "Uncheck all";
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // capSearchScope
            // 
            this.capSearchScope.Caption = "Search scope";
            this.capSearchScope.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capSearchScope.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capSearchScope.Dock = System.Windows.Forms.DockStyle.Top;
            this.capSearchScope.Location = new System.Drawing.Point(0, 0);
            this.capSearchScope.Name = "capSearchScope";
            this.capSearchScope.Size = new System.Drawing.Size(209, 30);
            this.capSearchScope.TabIndex = 6;
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(784, 611);
            this.Controls.Add(this.splitSearch);
            this.Controls.Add(this.stsSearchResults);
            this.Controls.Add(this.pnlBottomSpacing);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "SearchForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search";
            this.tableSearch.ResumeLayout(false);
            this.tableSearch.PerformLayout();
            this.pnlSearchResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.stsSearchResults.ResumeLayout(false);
            this.stsSearchResults.PerformLayout();
            this.splitSearch.Panel1.ResumeLayout(false);
            this.splitSearch.Panel1.PerformLayout();
            this.splitSearch.Panel2.ResumeLayout(false);
            this.splitSearch.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSearch)).EndInit();
            this.splitSearch.ResumeLayout(false);
            this.pnlSearchOptions.ResumeLayout(false);
            this.pnlSearchOptions.PerformLayout();
            this.tableSearchOptions.ResumeLayout(false);
            this.tableSearchOptions.PerformLayout();
            this.tlsModify.ResumeLayout(false);
            this.tlsModify.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBottomSpacing;
        internal System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TableLayoutPanel tableSearch;
        private System.Windows.Forms.Label lblRegExp;
        private CustomControls.ExpressionTextBox expSearchTerm;
        private System.Windows.Forms.Button btnSearch;
        private CustomControls.DataGridViewEx dgvResults;
        private System.Windows.Forms.Panel pnlSearchResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn hdrMatchedTrigger;
        private System.Windows.Forms.DataGridViewTextBoxColumn hdrMatchType;
        private System.Windows.Forms.StatusStrip stsSearchResults;
        private System.Windows.Forms.ToolStripStatusLabel lblResultInfo;
        private System.Windows.Forms.SplitContainer splitSearch;
        private CustomControls.PrettyCaption capSearchScope;
        private System.Windows.Forms.TableLayoutPanel tableSearchOptions;
        private System.Windows.Forms.Label lblTriggers;
        private System.Windows.Forms.CheckedListBox clbRepos;
        private System.Windows.Forms.Label lblRepos;
        private System.Windows.Forms.CheckedListBox clbActions;
        private System.Windows.Forms.Label lblActions;
        private System.Windows.Forms.CheckedListBox clbTriggers;
        private System.Windows.Forms.Label lblMisc;
        private System.Windows.Forms.CheckedListBox clbScope;
        private System.Windows.Forms.Label lblScope;
        private System.Windows.Forms.ToolStrip tlsModify;
        private System.Windows.Forms.ToolStripDropDownButton btnModifySelection;
        private System.Windows.Forms.ToolStripMenuItem btnCheckAll;
        private System.Windows.Forms.ToolStripMenuItem btnUncheckAll;
        private System.Windows.Forms.Panel pnlSearchOptions;
        private System.Windows.Forms.CheckedListBox clbMisc;
    }
}