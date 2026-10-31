namespace Triggernometry.UI.Forms
{
    partial class LogForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogForm));
            this.panel2 = new System.Windows.Forms.Panel();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.dgvLog = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxCopyRows = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxCopyMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlSearchOptions = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.layoutRegex = new System.Windows.Forms.TableLayoutPanel();
            this.rexSearch = new Triggernometry.UI.CustomControls.ExpressionTextBox();
            this.lblRegularExpression = new System.Windows.Forms.Label();
            this.layoutLevels = new System.Windows.Forms.TableLayoutPanel();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.chkError = new System.Windows.Forms.CheckBox();
            this.chkWarning = new System.Windows.Forms.CheckBox();
            this.chkInfo = new System.Windows.Forms.CheckBox();
            this.chkVerbose = new System.Windows.Forms.CheckBox();
            this.chkCustom = new System.Windows.Forms.CheckBox();
            this.chkCustom2 = new System.Windows.Forms.CheckBox();
            this.panelBar = new System.Windows.Forms.Panel();
            this.capSearch = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.tlsMain = new System.Windows.Forms.ToolStrip();
            this.btnCopyRows = new System.Windows.Forms.ToolStripButton();
            this.btnCopyMessages = new System.Windows.Forms.ToolStripButton();
            this.btnSortOrder = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.btnSearchOptions = new System.Windows.Forms.ToolStripButton();
            this.lblStatus = new System.Windows.Forms.ToolStripLabel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.pnlSearchOptions.SuspendLayout();
            this.layoutRegex.SuspendLayout();
            this.layoutLevels.SuspendLayout();
            this.tlsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.layoutMain);
            this.panel2.Controls.Add(this.tlsMain);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(734, 461);
            this.panel2.TabIndex = 2;
            // 
            // layoutMain
            //
            this.layoutMain.ColumnCount = 2;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutMain.Controls.Add(this.dgvLog, 0, 0);
            this.layoutMain.Controls.Add(this.pnlSearchOptions, 1, 0);
            this.layoutMain.Controls.Add(this.layoutRegex, 0, 1);
            this.layoutMain.Controls.Add(this.btnSearch, 1, 1);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(10, 35);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 2;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.Size = new System.Drawing.Size(714, 371);
            this.layoutMain.TabIndex = 18;
            //
            // dgvLog
            // 
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.AllowUserToResizeRows = false;
            this.dgvLog.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvLog.ColumnHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTimestamp,
            this.colLevel,
            this.colMessage});
            this.dgvLog.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLog.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLog.Location = new System.Drawing.Point(0, 0);
            this.dgvLog.Margin = new System.Windows.Forms.Padding(0);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.ReadOnly = true;
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLog.ShowCellErrors = false;
            this.dgvLog.ShowEditingIcon = false;
            this.dgvLog.ShowRowErrors = false;
            this.dgvLog.Size = new System.Drawing.Size(510, 371);
            this.dgvLog.TabIndex = 0;
            this.dgvLog.VirtualMode = true;
            // 
            // colTimestamp
            // 
            this.colTimestamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colTimestamp.HeaderText = "Timestamp";
            this.colTimestamp.Name = "colTimestamp";
            this.colTimestamp.ReadOnly = true;
            this.colTimestamp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colLevel
            // 
            this.colLevel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colLevel.HeaderText = "Level";
            this.colLevel.Name = "colLevel";
            this.colLevel.ReadOnly = true;
            this.colLevel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colMessage
            // 
            this.colMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMessage.HeaderText = "Message";
            this.colMessage.Name = "colMessage";
            this.colMessage.ReadOnly = true;
            this.colMessage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxCopyRows,
            this.ctxCopyMessages});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 48);
            //
            // ctxCopyRows
            //
            this.ctxCopyRows.Enabled = false;
            this.ctxCopyRows.Image = ((System.Drawing.Image)(resources.GetObject("ctxCopyRows.Image")));
            this.ctxCopyRows.Name = "ctxCopyRows";
            this.ctxCopyRows.Size = new System.Drawing.Size(180, 22);
            this.ctxCopyRows.Text = "Copy rows";
            this.ctxCopyRows.Click += new System.EventHandler(this.CopyRows_Click);
            //
            // ctxCopyMessages
            //
            this.ctxCopyMessages.Enabled = false;
            this.ctxCopyMessages.Image = ((System.Drawing.Image)(resources.GetObject("ctxCopyMessages.Image")));
            this.ctxCopyMessages.Name = "ctxCopyMessages";
            this.ctxCopyMessages.ShortcutKeyDisplayString = "Ctrl+C";
            this.ctxCopyMessages.Size = new System.Drawing.Size(180, 22);
            this.ctxCopyMessages.Text = "Copy messages";
            this.ctxCopyMessages.Click += new System.EventHandler(this.CopyMessages_Click);
            //
            // pnlSearchOptions
            //
            this.pnlSearchOptions.AutoScroll = true;
            this.pnlSearchOptions.Controls.Add(this.layoutLevels);
            this.pnlSearchOptions.Controls.Add(this.capSearch);
            this.pnlSearchOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearchOptions.Location = new System.Drawing.Point(514, 0);
            this.pnlSearchOptions.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.pnlSearchOptions.MinimumSize = new System.Drawing.Size(150, 0);
            this.pnlSearchOptions.Name = "pnlSearchOptions";
            this.pnlSearchOptions.Size = new System.Drawing.Size(150, 328);
            this.pnlSearchOptions.TabIndex = 8;
            //
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.btnSearch.AutoSize = true;
            this.btnSearch.Margin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.btnSearch.MinimumSize = new System.Drawing.Size(0, 35);
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(0, 26);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(200, 35);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // layoutRegex
            //
            this.layoutRegex.AutoSize = true;
            this.layoutRegex.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layoutRegex.ColumnCount = 2;
            this.layoutRegex.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutRegex.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRegex.Controls.Add(this.lblRegularExpression, 0, 0);
            this.layoutRegex.Controls.Add(this.rexSearch, 1, 0);
            this.layoutRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRegex.Location = new System.Drawing.Point(0, 336);
            this.layoutRegex.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.layoutRegex.Name = "layoutRegex";
            this.layoutRegex.RowCount = 1;
            this.layoutRegex.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutRegex.Size = new System.Drawing.Size(506, 35);
            this.layoutRegex.TabIndex = 7;
            //
            // rexSearch
            // 
            this.rexSearch.AutoSize = true;
            this.rexSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right));
            this.rexSearch.Expression = "";
            this.rexSearch.ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum.Regex;
            this.rexSearch.Location = new System.Drawing.Point(110, 0);
            this.rexSearch.Margin = new System.Windows.Forms.Padding(0);
            this.rexSearch.Name = "rexSearch";
            this.rexSearch.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.rexSearch.ReadOnly = false;
            this.rexSearch.RegexSupportsExpressions = false;
            this.rexSearch.Size = new System.Drawing.Size(194, 20);
            this.rexSearch.TabIndex = 8;
            // 
            // lblRegularExpression
            // 
            this.lblRegularExpression.AutoSize = true;
            this.lblRegularExpression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRegularExpression.Location = new System.Drawing.Point(0, 5);
            this.lblRegularExpression.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.lblRegularExpression.Name = "lblRegularExpression";
            this.lblRegularExpression.Size = new System.Drawing.Size(102, 13);
            this.lblRegularExpression.TabIndex = 0;
            this.lblRegularExpression.Text = "Regular expression";
            this.lblRegularExpression.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // layoutLevels
            //
            this.layoutLevels.AutoSize = true;
            this.layoutLevels.ColumnCount = 1;
            this.layoutLevels.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutLevels.Controls.Add(this.chkError, 0, 0);
            this.layoutLevels.Controls.Add(this.chkWarning, 0, 1);
            this.layoutLevels.Controls.Add(this.chkCustom, 0, 2);
            this.layoutLevels.Controls.Add(this.chkCustom2, 0, 3);
            this.layoutLevels.Controls.Add(this.chkInfo, 0, 4);
            this.layoutLevels.Controls.Add(this.chkVerbose, 0, 5);
            this.layoutLevels.Controls.Add(this.panelBar, 0, 6);
            this.layoutLevels.Controls.Add(this.chkAll, 0, 7);
            this.layoutLevels.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutLevels.Location = new System.Drawing.Point(0, 30);
            this.layoutLevels.Name = "layoutLevels";
            this.layoutLevels.RowCount = 8;
            this.layoutLevels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLevels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLevels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLevels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLevels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLevels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLevels.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 4));
            this.layoutLevels.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLevels.Size = new System.Drawing.Size(200, 83);
            this.layoutLevels.TabIndex = 6;
            // 
            // chkError
            // 
            this.chkError.AutoSize = true;
            this.chkError.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.chkError.Margin = new System.Windows.Forms.Padding(20, 17, 20, 3);
            this.chkError.Name = "chkError";
            this.chkError.Text = "Error";
            this.chkError.UseVisualStyleBackColor = true;
            this.chkError.CheckedChanged += new System.EventHandler(this.ChkOther_CheckedChanged);
            this.chkError.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChkOther_RightClick);
            // 
            // chkWarning
            // 
            this.chkWarning.AutoSize = true;
            this.chkWarning.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkWarning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(128)))), ((int)(((byte)(24)))));
            this.chkWarning.Margin = new System.Windows.Forms.Padding(20, 7, 20, 3);
            this.chkWarning.Name = "chkWarning";
            this.chkWarning.Text = "Warning";
            this.chkWarning.UseVisualStyleBackColor = true;
            this.chkWarning.CheckedChanged += new System.EventHandler(this.ChkOther_CheckedChanged);
            this.chkWarning.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChkOther_RightClick);
            // 
            // chkCustom
            // 
            this.chkCustom.AutoSize = true;
            this.chkCustom.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCustom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkCustom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(144)))), ((int)(((byte)(72)))));
            this.chkCustom.Margin = new System.Windows.Forms.Padding(20, 7, 20, 3);
            this.chkCustom.Name = "chkCustom";
            this.chkCustom.Text = "Custom";
            this.chkCustom.UseVisualStyleBackColor = true;
            this.chkCustom.CheckedChanged += new System.EventHandler(this.ChkOther_CheckedChanged);
            this.chkCustom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChkOther_RightClick);
            // 
            // chkCustom2
            // 
            this.chkCustom2.AutoSize = true;
            this.chkCustom2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCustom2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkCustom2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(72)))), ((int)(((byte)(144)))));
            this.chkCustom2.Margin = new System.Windows.Forms.Padding(20, 7, 20, 3);
            this.chkCustom2.Name = "chkCustom2";
            this.chkCustom2.Text = "Custom 2";
            this.chkCustom2.UseVisualStyleBackColor = true;
            this.chkCustom2.CheckedChanged += new System.EventHandler(this.ChkOther_CheckedChanged);
            this.chkCustom2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChkOther_RightClick);
            // 
            // chkInfo
            // 
            this.chkInfo.AutoSize = true;
            this.chkInfo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.chkInfo.Margin = new System.Windows.Forms.Padding(20, 7, 20, 3);
            this.chkInfo.Name = "chkInfo";
            this.chkInfo.Text = "Info";
            this.chkInfo.UseVisualStyleBackColor = true;
            this.chkInfo.CheckedChanged += new System.EventHandler(this.ChkOther_CheckedChanged);
            this.chkInfo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChkOther_RightClick);
            // 
            // chkVerbose
            // 
            this.chkVerbose.AutoSize = true;
            this.chkVerbose.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkVerbose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkVerbose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.chkVerbose.Margin = new System.Windows.Forms.Padding(20, 7, 20, 3);
            this.chkVerbose.Name = "chkVerbose";
            this.chkVerbose.Text = "Verbose";
            this.chkVerbose.UseVisualStyleBackColor = true;
            this.chkVerbose.CheckedChanged += new System.EventHandler(this.ChkOther_CheckedChanged);
            this.chkVerbose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChkOther_RightClick);
            // 
            // panelBar
            // 
            this.panelBar.AutoSize = true;
            this.panelBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panelBar.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.panelBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBar.Name = "panelBar";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.chkAll.Margin = new System.Windows.Forms.Padding(20, 17, 20, 3);
            this.chkAll.Name = "chkAll";
            this.chkAll.Text = "Select All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.ChkAll_CheckedChanged);
            this.chkAll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChkAll_RightClick);
            // 
            // capSearch
            // 
            this.capSearch.Caption = "Search options";
            this.capSearch.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capSearch.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.capSearch.Location = new System.Drawing.Point(0, 0);
            this.capSearch.Name = "capSearch";
            this.capSearch.Size = new System.Drawing.Size(200, 30);
            this.capSearch.TabIndex = 5;
            // 
            // tlsMain
            // 
            this.tlsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCopyRows,
            this.btnCopyMessages,
            this.toolStripSeparator3,
            this.btnClear,
            this.btnSortOrder,
            this.btnSearchOptions,
            this.lblStatus});
            this.tlsMain.Location = new System.Drawing.Point(10, 10);
            this.tlsMain.Name = "tlsMain";
            this.tlsMain.Size = new System.Drawing.Size(714, 25);
            this.tlsMain.TabIndex = 3;
            // 
            // btnCopyRows
            //
            this.btnCopyRows.Enabled = false;
            this.btnCopyRows.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyRows.Image")));
            this.btnCopyRows.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyRows.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.btnCopyRows.Name = "btnCopyRows";
            this.btnCopyRows.Size = new System.Drawing.Size(82, 22);
            this.btnCopyRows.Text = "Copy rows";
            this.btnCopyRows.Click += new System.EventHandler(this.CopyRows_Click);
            //
            // btnCopyMessages
            //
            this.btnCopyMessages.Enabled = false;
            this.btnCopyMessages.Image = ((System.Drawing.Image)(resources.GetObject("btnCopyMessages.Image")));
            this.btnCopyMessages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyMessages.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.btnCopyMessages.Name = "btnCopyMessages";
            this.btnCopyMessages.Size = new System.Drawing.Size(110, 22);
            this.btnCopyMessages.Text = "Copy messages";
            this.btnCopyMessages.Click += new System.EventHandler(this.CopyMessages_Click);
            //
            // btnSortOrder
            //
            this.btnSortOrder.CheckOnClick = true;
            this.btnSortOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSortOrder.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.btnSortOrder.Name = "btnSortOrder";
            this.btnSortOrder.Size = new System.Drawing.Size(76, 22);
            this.btnSortOrder.Text = "Descending";
            this.btnSortOrder.CheckedChanged += new System.EventHandler(this.SortOrder_CheckedChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClear
            // 
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(74, 22);
            this.btnClear.Text = "Clear log";
            this.btnClear.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // btnSearchOptions
            // 
            this.btnSearchOptions.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnSearchOptions.Checked = true;
            this.btnSearchOptions.CheckOnClick = true;
            this.btnSearchOptions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnSearchOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSearchOptions.Image")));
            this.btnSearchOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSearchOptions.Margin = new System.Windows.Forms.Padding(4, 1, 4, 1);
            this.btnSearchOptions.Name = "btnSearchOptions";
            this.btnSearchOptions.Size = new System.Drawing.Size(105, 22);
            this.btnSearchOptions.Text = "Search options";
            this.btnSearchOptions.CheckedChanged += new System.EventHandler(this.btnSearchOptions_CheckedChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 22);
            this.lblStatus.Text = "Status";
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(10, 406);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(714, 10);
            this.panel3.TabIndex = 17;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClose.Location = new System.Drawing.Point(10, 416);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(714, 35);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1300, 750);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "LogForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Triggernometry log";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.pnlSearchOptions.ResumeLayout(false);
            this.layoutRegex.ResumeLayout(false);
            this.layoutRegex.PerformLayout();
            this.layoutLevels.ResumeLayout(false);
            this.layoutLevels.PerformLayout();
            this.tlsMain.ResumeLayout(false);
            this.tlsMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip tlsMain;
        private System.Windows.Forms.ToolStripButton btnCopyRows;
        private System.Windows.Forms.ToolStripButton btnCopyMessages;
        private System.Windows.Forms.ToolStripButton btnSortOrder;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ctxCopyRows;
        private System.Windows.Forms.ToolStripMenuItem ctxCopyMessages;
        private System.Windows.Forms.Panel panel3;
        internal System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnClear;
        private CustomControls.DataGridViewEx dgvLog;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.ToolStripButton btnSearchOptions;
        private CustomControls.PrettyCaption capSearch;
        private System.Windows.Forms.TableLayoutPanel layoutLevels;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.CheckBox chkError;
        private System.Windows.Forms.CheckBox chkWarning;
        private System.Windows.Forms.CheckBox chkInfo;
        private System.Windows.Forms.CheckBox chkVerbose;
        private System.Windows.Forms.CheckBox chkCustom;
        private System.Windows.Forms.CheckBox chkCustom2;
        private System.Windows.Forms.Panel panelBar;
        private System.Windows.Forms.Panel pnlSearchOptions;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TableLayoutPanel layoutRegex;
        private CustomControls.ExpressionTextBox rexSearch;
        private System.Windows.Forms.Label lblRegularExpression;
        private System.Windows.Forms.ToolStripLabel lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
    }
}