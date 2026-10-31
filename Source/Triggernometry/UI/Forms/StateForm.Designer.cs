namespace Triggernometry.UI.Forms
{
    partial class StateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StateForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvCellStyleDict = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvCellStylePeDict = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlButtonsSpacing = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.tlsScalar = new System.Windows.Forms.ToolStrip();
            this.btnScalarAdd = new System.Windows.Forms.ToolStripButton();
            this.btnScalarEdit = new System.Windows.Forms.ToolStripButton();
            this.sepScalarRemove = new System.Windows.Forms.ToolStripSeparator();
            this.btnScalarRemove = new System.Windows.Forms.ToolStripButton();
            this.btnScalarRefresh = new System.Windows.Forms.ToolStripSplitButton();
            this.btnScalarRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxState = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxLocateSource = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlScalarSession = new System.Windows.Forms.Panel();
            this.dgvScalarVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colScalarName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScalarValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScalarLastChanged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScalarChangedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbcMain = new System.Windows.Forms.TabControl();
            this.tabScalar = new System.Windows.Forms.TabPage();
            this.tbcScalar = new System.Windows.Forms.TabControl();
            this.tabScalarSession = new System.Windows.Forms.TabPage();
            this.tabScalarPersistent = new System.Windows.Forms.TabPage();
            this.pnlScalarPersistent = new System.Windows.Forms.Panel();
            this.dgvPeScalarVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colPeScalarName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeScalarValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeScalarLastChanged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeScalarChangedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsPeScalar = new System.Windows.Forms.ToolStrip();
            this.btnPeScalarAdd = new System.Windows.Forms.ToolStripButton();
            this.btnPeScalarEdit = new System.Windows.Forms.ToolStripButton();
            this.sepPeScalarRemove = new System.Windows.Forms.ToolStripSeparator();
            this.btnPeScalarRemove = new System.Windows.Forms.ToolStripButton();
            this.btnPeScalarRefresh = new System.Windows.Forms.ToolStripSplitButton();
            this.btnPeScalarRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.imlStateTabs = new System.Windows.Forms.ImageList(this.components);
            this.tabList = new System.Windows.Forms.TabPage();
            this.tbcList = new System.Windows.Forms.TabControl();
            this.tabListSession = new System.Windows.Forms.TabPage();
            this.pnlListSession = new System.Windows.Forms.Panel();
            this.dgvListVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colListName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colListSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colListValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colListLastChanged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colListChangedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsList = new System.Windows.Forms.ToolStrip();
            this.btnListAdd = new System.Windows.Forms.ToolStripButton();
            this.btnListEdit = new System.Windows.Forms.ToolStripButton();
            this.sepListRemove = new System.Windows.Forms.ToolStripSeparator();
            this.btnListRemove = new System.Windows.Forms.ToolStripButton();
            this.btnListRefresh = new System.Windows.Forms.ToolStripSplitButton();
            this.btnListRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tabListPersistent = new System.Windows.Forms.TabPage();
            this.pnlListPersistent = new System.Windows.Forms.Panel();
            this.dgvPeListVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colPeListName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeListSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeListValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeListLastChanged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeListChangedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsPeList = new System.Windows.Forms.ToolStrip();
            this.btnPeListAdd = new System.Windows.Forms.ToolStripButton();
            this.btnPeListEdit = new System.Windows.Forms.ToolStripButton();
            this.sepPeListRemove = new System.Windows.Forms.ToolStripSeparator();
            this.btnPeListRemove = new System.Windows.Forms.ToolStripButton();
            this.btnPeListRefresh = new System.Windows.Forms.ToolStripSplitButton();
            this.btnPeListRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tabTable = new System.Windows.Forms.TabPage();
            this.tbcTable = new System.Windows.Forms.TabControl();
            this.tabTableSession = new System.Windows.Forms.TabPage();
            this.pnlTableSession = new System.Windows.Forms.Panel();
            this.dgvTableVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colTableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTableWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTableHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTableLastChanged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTableChangedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsTable = new System.Windows.Forms.ToolStrip();
            this.btnTableAdd = new System.Windows.Forms.ToolStripButton();
            this.btnTableEdit = new System.Windows.Forms.ToolStripButton();
            this.sepTableRemove = new System.Windows.Forms.ToolStripSeparator();
            this.btnTableRemove = new System.Windows.Forms.ToolStripButton();
            this.btnTableRefresh = new System.Windows.Forms.ToolStripSplitButton();
            this.btnTableRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tabTablePersistent = new System.Windows.Forms.TabPage();
            this.pnlTablePersistent = new System.Windows.Forms.Panel();
            this.dgvPeTableVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colPeTableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeTableWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeTableHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeTableLastChanged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeTableChangedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsPeTable = new System.Windows.Forms.ToolStrip();
            this.btnPeTableAdd = new System.Windows.Forms.ToolStripButton();
            this.btnPeTableEdit = new System.Windows.Forms.ToolStripButton();
            this.sepPeTableRemove = new System.Windows.Forms.ToolStripSeparator();
            this.btnPeTableRemove = new System.Windows.Forms.ToolStripButton();
            this.btnPeTableRefresh = new System.Windows.Forms.ToolStripSplitButton();
            this.btnPeTableRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tabDict = new System.Windows.Forms.TabPage();
            this.tbcDict = new System.Windows.Forms.TabControl();
            this.tabDictSession = new System.Windows.Forms.TabPage();
            this.panelDict = new System.Windows.Forms.Panel();
            this.dgvDictVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colDictName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDictSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDictValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDictLastChanged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDictChangedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsDict = new System.Windows.Forms.ToolStrip();
            this.btnDictAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDictEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDict = new System.Windows.Forms.ToolStripSeparator();
            this.btnDictRemove = new System.Windows.Forms.ToolStripButton();
            this.btnDictRefresh = new System.Windows.Forms.ToolStripSplitButton();
            this.btnDictRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tabDictPersistent = new System.Windows.Forms.TabPage();
            this.panelPeDict = new System.Windows.Forms.Panel();
            this.dgvPeDictVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colPeDictName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeDictSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeDictValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeDictLastChanged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeDictChangedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsPeDict = new System.Windows.Forms.ToolStrip();
            this.btnPeDictAdd = new System.Windows.Forms.ToolStripButton();
            this.btnPeDictEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorPeDict = new System.Windows.Forms.ToolStripSeparator();
            this.btnPeDictRemove = new System.Windows.Forms.ToolStripButton();
            this.btnPeDictRefresh = new System.Windows.Forms.ToolStripSplitButton();
            this.btnPeDictRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tabRegisteredVariables = new System.Windows.Forms.TabPage();
            this.dgvRegisteredVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colRegisteredName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegisteredValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegisteredLastChanged = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegisteredChangedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsRegistered = new System.Windows.Forms.ToolStrip();
            this.btnRegisteredAdd = new System.Windows.Forms.ToolStripButton();
            this.btnRegisteredEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorRegistered = new System.Windows.Forms.ToolStripSeparator();
            this.btnRegisteredRemove = new System.Windows.Forms.ToolStripButton();
            this.btnRegisteredRefresh = new System.Windows.Forms.ToolStripSplitButton();
            this.btnRegisteredRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tabMutexes = new System.Windows.Forms.TabPage();
            this.dgvMutexes = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colMutexName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMutexOwner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMutexRefCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMutexQueue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsMutex = new System.Windows.Forms.ToolStrip();
            this.btnMutexRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnMutexForce = new System.Windows.Forms.ToolStripButton();
            this.tabImageAura = new System.Windows.Forms.TabPage();
            this.dgvImage = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colImageName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colImageCreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsImageAura = new System.Windows.Forms.ToolStrip();
            this.btnImageRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnImageForce = new System.Windows.Forms.ToolStripButton();
            this.tabTextAura = new System.Windows.Forms.TabPage();
            this.dgvText = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colTextName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTextCreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsTextAura = new System.Windows.Forms.ToolStrip();
            this.btnTextRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnTextForce = new System.Windows.Forms.ToolStripButton();
            this.tabNamedCallbacks = new System.Windows.Forms.TabPage();
            this.dgvCallback = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colCallbackId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCallbackName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCallbackRegistrant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCallbackRegistrationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCallbackLastInvoked = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsCallbacks = new System.Windows.Forms.ToolStrip();
            this.btnCallbackRefresh = new System.Windows.Forms.ToolStripButton();
            this.tlsScalar.SuspendLayout();
            this.ctxState.SuspendLayout();
            this.pnlScalarSession.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScalarVariables)).BeginInit();
            this.tbcMain.SuspendLayout();
            this.tabScalar.SuspendLayout();
            this.tbcScalar.SuspendLayout();
            this.tabScalarSession.SuspendLayout();
            this.tabScalarPersistent.SuspendLayout();
            this.pnlScalarPersistent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeScalarVariables)).BeginInit();
            this.tlsPeScalar.SuspendLayout();
            this.tabList.SuspendLayout();
            this.tbcList.SuspendLayout();
            this.tabListSession.SuspendLayout();
            this.pnlListSession.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListVariables)).BeginInit();
            this.tlsList.SuspendLayout();
            this.tabListPersistent.SuspendLayout();
            this.pnlListPersistent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeListVariables)).BeginInit();
            this.tlsPeList.SuspendLayout();
            this.tabTable.SuspendLayout();
            this.tbcTable.SuspendLayout();
            this.tabTableSession.SuspendLayout();
            this.pnlTableSession.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableVariables)).BeginInit();
            this.tlsTable.SuspendLayout();
            this.tabTablePersistent.SuspendLayout();
            this.pnlTablePersistent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeTableVariables)).BeginInit();
            this.tlsPeTable.SuspendLayout();
            this.tbcDict.SuspendLayout();
            this.tabDictSession.SuspendLayout();
            this.panelDict.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDictVariables)).BeginInit();
            this.tlsDict.SuspendLayout();
            this.tabDictPersistent.SuspendLayout();
            this.panelPeDict.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeDictVariables)).BeginInit();
            this.tlsPeDict.SuspendLayout();
            this.tabRegisteredVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegisteredVariables)).BeginInit();
            this.tlsRegistered.SuspendLayout();
            this.tabMutexes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMutexes)).BeginInit();
            this.tlsMutex.SuspendLayout();
            this.tabImageAura.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImage)).BeginInit();
            this.tlsImageAura.SuspendLayout();
            this.tabTextAura.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvText)).BeginInit();
            this.tlsTextAura.SuspendLayout();
            this.tabNamedCallbacks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCallback)).BeginInit();
            this.tlsCallbacks.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtonsSpacing
            // 
            this.pnlButtonsSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonsSpacing.Location = new System.Drawing.Point(10, 456);
            this.pnlButtonsSpacing.Name = "pnlButtonsSpacing";
            this.pnlButtonsSpacing.Size = new System.Drawing.Size(714, 10);
            this.pnlButtonsSpacing.TabIndex = 19;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClose.Location = new System.Drawing.Point(10, 466);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(714, 35);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // tlsScalar
            // 
            this.tlsScalar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsScalar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnScalarAdd,
            this.btnScalarEdit,
            this.sepScalarRemove,
            this.btnScalarRemove,
            this.btnScalarRefresh});
            this.tlsScalar.Location = new System.Drawing.Point(0, 0);
            this.tlsScalar.Name = "tlsScalar";
            this.tlsScalar.Size = new System.Drawing.Size(672, 25);
            this.tlsScalar.TabIndex = 20;
            // 
            // btnScalarAdd
            // 
            this.btnScalarAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnScalarAdd.Image")));
            this.btnScalarAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnScalarAdd.Name = "btnScalarAdd";
            this.btnScalarAdd.Size = new System.Drawing.Size(93, 22);
            this.btnScalarAdd.Text = "Add variable";
            this.btnScalarAdd.Click += new System.EventHandler(this.btnScalarAdd_Click);
            // 
            // btnScalarEdit
            // 
            this.btnScalarEdit.Enabled = false;
            this.btnScalarEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnScalarEdit.Image")));
            this.btnScalarEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnScalarEdit.Name = "btnScalarEdit";
            this.btnScalarEdit.Size = new System.Drawing.Size(91, 22);
            this.btnScalarEdit.Text = "Edit variable";
            this.btnScalarEdit.Click += new System.EventHandler(this.btnScalarEdit_Click);
            // 
            // sepScalarRemove
            // 
            this.sepScalarRemove.Name = "sepScalarRemove";
            this.sepScalarRemove.Size = new System.Drawing.Size(6, 25);
            // 
            // btnScalarRemove
            // 
            this.btnScalarRemove.Enabled = false;
            this.btnScalarRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnScalarRemove.Image")));
            this.btnScalarRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnScalarRemove.Name = "btnScalarRemove";
            this.btnScalarRemove.Size = new System.Drawing.Size(114, 22);
            this.btnScalarRemove.Text = "Remove variable";
            this.btnScalarRemove.Click += new System.EventHandler(this.btnScalarRemove_Click);
            // 
            // btnScalarRefresh
            // 
            this.btnScalarRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnScalarRefresh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnScalarRemoveAll});
            this.btnScalarRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnScalarRefresh.Image")));
            this.btnScalarRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnScalarRefresh.Name = "btnScalarRefresh";
            this.btnScalarRefresh.Size = new System.Drawing.Size(78, 22);
            this.btnScalarRefresh.Text = "Refresh";
            this.btnScalarRefresh.ButtonClick += new System.EventHandler(this.btnRefreshScalar_ButtonClick);
            // 
            // btnScalarRemoveAll
            // 
            this.btnScalarRemoveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnScalarRemoveAll.Image")));
            this.btnScalarRemoveAll.Name = "btnScalarRemoveAll";
            this.btnScalarRemoveAll.Size = new System.Drawing.Size(181, 22);
            this.btnScalarRemoveAll.Text = "Remove all variables";
            this.btnScalarRemoveAll.Click += new System.EventHandler(this.btnRemoveAllScalar_Click);
            // 
            // ctxState
            // 
            this.ctxState.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxRefresh,
            this.ctxLocateSource});
            this.ctxState.Name = "ctxState";
            this.ctxState.Size = new System.Drawing.Size(150, 48);
            this.ctxState.Opening += new System.ComponentModel.CancelEventHandler(this.ctxState_Opening);
            // 
            // ctxRefresh
            // 
            this.ctxRefresh.Image = ((System.Drawing.Image)(resources.GetObject("ctxRefresh.Image")));
            this.ctxRefresh.Name = "ctxRefresh";
            this.ctxRefresh.Size = new System.Drawing.Size(113, 22);
            this.ctxRefresh.Text = "Refresh";
            //
            // ctxLocateSource
            //
            this.ctxLocateSource.Name = "ctxLocateSource";
            this.ctxLocateSource.Size = new System.Drawing.Size(149, 22);
            this.ctxLocateSource.Text = "Locate source";
            this.ctxLocateSource.Click += new System.EventHandler(this.ctxLocateSource_Click);
            // 
            // pnlScalarSession
            // 
            this.pnlScalarSession.Controls.Add(this.dgvScalarVariables);
            this.pnlScalarSession.Controls.Add(this.tlsScalar);
            this.pnlScalarSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScalarSession.Location = new System.Drawing.Point(3, 3);
            this.pnlScalarSession.Name = "pnlScalarSession";
            this.pnlScalarSession.Size = new System.Drawing.Size(672, 366);
            this.pnlScalarSession.TabIndex = 22;
            // 
            // dgvScalarVariables
            // 
            this.dgvScalarVariables.AllowUserToAddRows = false;
            this.dgvScalarVariables.AllowUserToDeleteRows = false;
            this.dgvScalarVariables.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvScalarVariables.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvScalarVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScalarVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colScalarName,
            this.colScalarValue,
            this.colScalarLastChanged,
            this.colScalarChangedBy});
            this.dgvScalarVariables.ContextMenuStrip = this.ctxState;
            this.dgvScalarVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvScalarVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvScalarVariables.Location = new System.Drawing.Point(0, 25);
            this.dgvScalarVariables.Name = "dgvScalarVariables";
            this.dgvScalarVariables.RowHeadersVisible = false;
            this.dgvScalarVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvScalarVariables.ShowCellErrors = false;
            this.dgvScalarVariables.ShowEditingIcon = false;
            this.dgvScalarVariables.ShowRowErrors = false;
            this.dgvScalarVariables.Size = new System.Drawing.Size(672, 341);
            this.dgvScalarVariables.TabIndex = 21;
            this.dgvScalarVariables.VirtualMode = true;
            this.dgvScalarVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvScalarVariables_CellDoubleClick);
            this.dgvScalarVariables.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvScalarVariables_CellValueNeeded);
            this.dgvScalarVariables.SelectionChanged += new System.EventHandler(this.dgvScalarVariables_SelectionChanged);
            this.dgvScalarVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvScalarVariables_KeyDown);
            this.dgvScalarVariables.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colScalarName
            // 
            this.colScalarName.Frozen = true;
            this.colScalarName.HeaderText = "Name";
            this.colScalarName.Name = "colScalarName";
            this.colScalarName.ReadOnly = true;
            this.colScalarName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colScalarName.Width = 160;
            // 
            // colScalarValue
            // 
            this.colScalarValue.HeaderText = "Value";
            this.colScalarValue.Name = "colScalarValue";
            this.colScalarValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colScalarValue.Width = 160;
            // 
            // colScalarLastChanged
            // 
            this.colScalarLastChanged.HeaderText = "Last changed";
            this.colScalarLastChanged.Name = "colScalarLastChanged";
            this.colScalarLastChanged.ReadOnly = true;
            this.colScalarLastChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colScalarLastChanged.Width = 125;
            // 
            // colScalarChangedBy
            // 
            this.colScalarChangedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colScalarChangedBy.HeaderText = "Changed by";
            this.colScalarChangedBy.Name = "colScalarChangedBy";
            this.colScalarChangedBy.ReadOnly = true;
            this.colScalarChangedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // tbcMain
            // 
            this.tbcMain.Controls.Add(this.tabScalar);
            this.tbcMain.Controls.Add(this.tabList);
            this.tbcMain.Controls.Add(this.tabTable);
            this.tbcMain.Controls.Add(this.tabDict);
            this.tbcMain.Controls.Add(this.tabRegisteredVariables);
            this.tbcMain.Controls.Add(this.tabMutexes);
            this.tbcMain.Controls.Add(this.tabImageAura);
            this.tbcMain.Controls.Add(this.tabTextAura);
            this.tbcMain.Controls.Add(this.tabNamedCallbacks);
            this.tbcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcMain.ImageList = this.imlStateTabs;
            this.tbcMain.Location = new System.Drawing.Point(10, 10);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.SelectedIndex = 0;
            this.tbcMain.Size = new System.Drawing.Size(714, 446);
            this.tbcMain.TabIndex = 23;
            this.tbcMain.SelectedIndexChanged += new System.EventHandler(this.tbcMain_SelectedIndexChanged);
            // 
            // tabScalar
            // 
            this.tabScalar.Controls.Add(this.tbcScalar);
            this.tabScalar.ImageKey = "var_scalar.png";
            this.tabScalar.Location = new System.Drawing.Point(4, 23);
            this.tabScalar.Name = "tabScalar";
            this.tabScalar.Padding = new System.Windows.Forms.Padding(10);
            this.tabScalar.Size = new System.Drawing.Size(706, 419);
            this.tabScalar.TabIndex = 0;
            this.tabScalar.Text = "Scalar variables";
            this.tabScalar.UseVisualStyleBackColor = true;
            // 
            // tbcScalar
            // 
            this.tbcScalar.Controls.Add(this.tabScalarSession);
            this.tbcScalar.Controls.Add(this.tabScalarPersistent);
            this.tbcScalar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcScalar.ImageList = this.imlStateTabs;
            this.tbcScalar.Location = new System.Drawing.Point(10, 10);
            this.tbcScalar.Name = "tbcScalar";
            this.tbcScalar.SelectedIndex = 0;
            this.tbcScalar.Size = new System.Drawing.Size(686, 399);
            this.tbcScalar.TabIndex = 23;
            this.tbcScalar.SelectedIndexChanged += new System.EventHandler(this.tbcScalar_SelectedIndexChanged);
            // 
            // tabScalarSession
            // 
            this.tabScalarSession.Controls.Add(this.pnlScalarSession);
            this.tabScalarSession.ImageKey = "appointment-new.png";
            this.tabScalarSession.Location = new System.Drawing.Point(4, 23);
            this.tabScalarSession.Name = "tabScalarSession";
            this.tabScalarSession.Padding = new System.Windows.Forms.Padding(3);
            this.tabScalarSession.Size = new System.Drawing.Size(678, 372);
            this.tabScalarSession.TabIndex = 0;
            this.tabScalarSession.Text = "Session";
            this.tabScalarSession.UseVisualStyleBackColor = true;
            // 
            // tabScalarPersistent
            // 
            this.tabScalarPersistent.Controls.Add(this.pnlScalarPersistent);
            this.tabScalarPersistent.ImageKey = "media-floppy.png";
            this.tabScalarPersistent.Location = new System.Drawing.Point(4, 23);
            this.tabScalarPersistent.Name = "tabScalarPersistent";
            this.tabScalarPersistent.Padding = new System.Windows.Forms.Padding(3);
            this.tabScalarPersistent.Size = new System.Drawing.Size(678, 372);
            this.tabScalarPersistent.TabIndex = 1;
            this.tabScalarPersistent.Text = "Persistent";
            this.tabScalarPersistent.UseVisualStyleBackColor = true;
            // 
            // pnlScalarPersistent
            // 
            this.pnlScalarPersistent.Controls.Add(this.dgvPeScalarVariables);
            this.pnlScalarPersistent.Controls.Add(this.tlsPeScalar);
            this.pnlScalarPersistent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScalarPersistent.Location = new System.Drawing.Point(3, 3);
            this.pnlScalarPersistent.Name = "pnlScalarPersistent";
            this.pnlScalarPersistent.Size = new System.Drawing.Size(672, 366);
            this.pnlScalarPersistent.TabIndex = 23;
            // 
            // dgvPeScalarVariables
            // 
            this.dgvPeScalarVariables.AllowUserToAddRows = false;
            this.dgvPeScalarVariables.AllowUserToDeleteRows = false;
            this.dgvPeScalarVariables.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvPeScalarVariables.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPeScalarVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPeScalarVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPeScalarName,
            this.colPeScalarValue,
            this.colPeScalarLastChanged,
            this.colPeScalarChangedBy});
            this.dgvPeScalarVariables.ContextMenuStrip = this.ctxState;
            this.dgvPeScalarVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPeScalarVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPeScalarVariables.Location = new System.Drawing.Point(0, 25);
            this.dgvPeScalarVariables.Name = "dgvPeScalarVariables";
            this.dgvPeScalarVariables.RowHeadersVisible = false;
            this.dgvPeScalarVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPeScalarVariables.ShowCellErrors = false;
            this.dgvPeScalarVariables.ShowEditingIcon = false;
            this.dgvPeScalarVariables.ShowRowErrors = false;
            this.dgvPeScalarVariables.Size = new System.Drawing.Size(672, 341);
            this.dgvPeScalarVariables.TabIndex = 21;
            this.dgvPeScalarVariables.VirtualMode = true;
            this.dgvPeScalarVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPeScalarVariables_CellDoubleClick);
            this.dgvPeScalarVariables.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvPeScalarVariables_CellValueNeeded);
            this.dgvPeScalarVariables.SelectionChanged += new System.EventHandler(this.dgvPeScalarVariables_SelectionChanged);
            this.dgvPeScalarVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPeScalarVariables_KeyDown);
            this.dgvPeScalarVariables.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colPeScalarName
            // 
            this.colPeScalarName.Frozen = true;
            this.colPeScalarName.HeaderText = "Name";
            this.colPeScalarName.Name = "colPeScalarName";
            this.colPeScalarName.ReadOnly = true;
            this.colPeScalarName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeScalarName.MinimumWidth = 50;
            this.colPeScalarName.Width = 240;
            // 
            // colPeScalarValue
            // 
            this.colPeScalarValue.HeaderText = "Value";
            this.colPeScalarValue.Name = "colPeScalarValue";
            this.colPeScalarValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeScalarValue.MinimumWidth = 50;
            this.colPeScalarValue.Width = 240;
            // 
            // colPeScalarLastChanged
            // 
            this.colPeScalarLastChanged.HeaderText = "Last changed";
            this.colPeScalarLastChanged.Name = "colPeScalarLastChanged";
            this.colPeScalarLastChanged.ReadOnly = true;
            this.colPeScalarLastChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeScalarLastChanged.MinimumWidth = 50;
            this.colPeScalarLastChanged.Width = 100;
            // 
            // colPeScalarChangedBy
            // 
            this.colPeScalarChangedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPeScalarChangedBy.HeaderText = "Changed by";
            this.colPeScalarChangedBy.Name = "colPeScalarChangedBy";
            this.colPeScalarChangedBy.ReadOnly = true;
            this.colPeScalarChangedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeScalarChangedBy.MinimumWidth = 50;
            // 
            // tlsPeScalar
            // 
            this.tlsPeScalar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsPeScalar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPeScalarAdd,
            this.btnPeScalarEdit,
            this.sepPeScalarRemove,
            this.btnPeScalarRemove,
            this.btnPeScalarRefresh});
            this.tlsPeScalar.Location = new System.Drawing.Point(0, 0);
            this.tlsPeScalar.Name = "tlsPeScalar";
            this.tlsPeScalar.Size = new System.Drawing.Size(672, 25);
            this.tlsPeScalar.TabIndex = 20;
            // 
            // btnPeScalarAdd
            // 
            this.btnPeScalarAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnPeScalarAdd.Image")));
            this.btnPeScalarAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeScalarAdd.Name = "btnPeScalarAdd";
            this.btnPeScalarAdd.Size = new System.Drawing.Size(93, 22);
            this.btnPeScalarAdd.Text = "Add variable";
            this.btnPeScalarAdd.Click += new System.EventHandler(this.btnPeScalarAdd_Click);
            // 
            // btnPeScalarEdit
            // 
            this.btnPeScalarEdit.Enabled = false;
            this.btnPeScalarEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnPeScalarEdit.Image")));
            this.btnPeScalarEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeScalarEdit.Name = "btnPeScalarEdit";
            this.btnPeScalarEdit.Size = new System.Drawing.Size(91, 22);
            this.btnPeScalarEdit.Text = "Edit variable";
            this.btnPeScalarEdit.Click += new System.EventHandler(this.btnPeScalarEdit_Click);
            // 
            // sepPeScalarRemove
            // 
            this.sepPeScalarRemove.Name = "sepPeScalarRemove";
            this.sepPeScalarRemove.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPeScalarRemove
            // 
            this.btnPeScalarRemove.Enabled = false;
            this.btnPeScalarRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnPeScalarRemove.Image")));
            this.btnPeScalarRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeScalarRemove.Name = "btnPeScalarRemove";
            this.btnPeScalarRemove.Size = new System.Drawing.Size(114, 22);
            this.btnPeScalarRemove.Text = "Remove variable";
            this.btnPeScalarRemove.Click += new System.EventHandler(this.btnPeScalarRemove_Click);
            // 
            // btnPeScalarRefresh
            // 
            this.btnPeScalarRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnPeScalarRefresh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPeScalarRemoveAll});
            this.btnPeScalarRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnPeScalarRefresh.Image")));
            this.btnPeScalarRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeScalarRefresh.Name = "btnPeScalarRefresh";
            this.btnPeScalarRefresh.Size = new System.Drawing.Size(78, 22);
            this.btnPeScalarRefresh.Text = "Refresh";
            this.btnPeScalarRefresh.ButtonClick += new System.EventHandler(this.btnPeScalarRefresh_ButtonClick);
            // 
            // btnPeScalarRemoveAll
            // 
            this.btnPeScalarRemoveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnPeScalarRemoveAll.Image")));
            this.btnPeScalarRemoveAll.Name = "btnPeScalarRemoveAll";
            this.btnPeScalarRemoveAll.Size = new System.Drawing.Size(181, 22);
            this.btnPeScalarRemoveAll.Text = "Remove all variables";
            this.btnPeScalarRemoveAll.Click += new System.EventHandler(this.btnPeScalarRemoveAll_Click);
            // 
            // imlStateTabs
            // 
            this.imlStateTabs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlStateTabs.ImageStream")));
            this.imlStateTabs.TransparentColor = System.Drawing.Color.Transparent;
            this.imlStateTabs.Images.SetKeyName(0, "var_scalar.png");
            this.imlStateTabs.Images.SetKeyName(1, "var_list.png");
            this.imlStateTabs.Images.SetKeyName(2, "var_table.png");
            this.imlStateTabs.Images.SetKeyName(3, "mutex.png");
            this.imlStateTabs.Images.SetKeyName(4, "imageaura.png");
            this.imlStateTabs.Images.SetKeyName(5, "textaura.png");
            this.imlStateTabs.Images.SetKeyName(6, "callback.png");
            this.imlStateTabs.Images.SetKeyName(7, "appointment-new.png");
            this.imlStateTabs.Images.SetKeyName(8, "media-floppy.png");
            this.imlStateTabs.Images.SetKeyName(9, "var_dict.png");
            this.imlStateTabs.Images.SetKeyName(10, "var_registered.png");
            // 
            // tabList
            // 
            this.tabList.Controls.Add(this.tbcList);
            this.tabList.ImageKey = "var_list.png";
            this.tabList.Location = new System.Drawing.Point(4, 23);
            this.tabList.Name = "tabList";
            this.tabList.Padding = new System.Windows.Forms.Padding(10);
            this.tabList.Size = new System.Drawing.Size(706, 419);
            this.tabList.TabIndex = 1;
            this.tabList.Text = "List variables";
            this.tabList.UseVisualStyleBackColor = true;
            // 
            // tbcList
            // 
            this.tbcList.Controls.Add(this.tabListSession);
            this.tbcList.Controls.Add(this.tabListPersistent);
            this.tbcList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcList.ImageList = this.imlStateTabs;
            this.tbcList.Location = new System.Drawing.Point(10, 10);
            this.tbcList.Name = "tbcList";
            this.tbcList.SelectedIndex = 0;
            this.tbcList.Size = new System.Drawing.Size(686, 399);
            this.tbcList.TabIndex = 24;
            this.tbcList.SelectedIndexChanged += new System.EventHandler(this.tbcList_SelectedIndexChanged);
            // 
            // tabListSession
            // 
            this.tabListSession.Controls.Add(this.pnlListSession);
            this.tabListSession.ImageKey = "appointment-new.png";
            this.tabListSession.Location = new System.Drawing.Point(4, 23);
            this.tabListSession.Name = "tabListSession";
            this.tabListSession.Padding = new System.Windows.Forms.Padding(3);
            this.tabListSession.Size = new System.Drawing.Size(678, 372);
            this.tabListSession.TabIndex = 0;
            this.tabListSession.Text = "Session";
            this.tabListSession.UseVisualStyleBackColor = true;
            // 
            // pnlListSession
            // 
            this.pnlListSession.Controls.Add(this.dgvListVariables);
            this.pnlListSession.Controls.Add(this.tlsList);
            this.pnlListSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlListSession.Location = new System.Drawing.Point(3, 3);
            this.pnlListSession.Name = "pnlListSession";
            this.pnlListSession.Size = new System.Drawing.Size(672, 366);
            this.pnlListSession.TabIndex = 23;
            // 
            // dgvListVariables
            // 
            this.dgvListVariables.AllowUserToAddRows = false;
            this.dgvListVariables.AllowUserToDeleteRows = false;
            this.dgvListVariables.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvListVariables.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvListVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colListName,
            this.colListSize,
            this.colListValue,
            this.colListLastChanged,
            this.colListChangedBy});
            this.dgvListVariables.ContextMenuStrip = this.ctxState;
            this.dgvListVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvListVariables.Location = new System.Drawing.Point(0, 25);
            this.dgvListVariables.Name = "dgvListVariables";
            this.dgvListVariables.RowHeadersVisible = false;
            this.dgvListVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListVariables.ShowCellErrors = false;
            this.dgvListVariables.ShowEditingIcon = false;
            this.dgvListVariables.ShowRowErrors = false;
            this.dgvListVariables.Size = new System.Drawing.Size(672, 341);
            this.dgvListVariables.TabIndex = 21;
            this.dgvListVariables.VirtualMode = true;
            this.dgvListVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListVariables_CellDoubleClick);
            this.dgvListVariables.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvListVariables_CellValueNeeded);
            this.dgvListVariables.SelectionChanged += new System.EventHandler(this.dgvListVariables_SelectionChanged);
            this.dgvListVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvListVariables_KeyDown);
            this.dgvListVariables.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colListName
            // 
            this.colListName.Frozen = true;
            this.colListName.HeaderText = "Name";
            this.colListName.Name = "colListName";
            this.colListName.ReadOnly = true;
            this.colListName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colListName.Width = 200;
            // 
            // colListSize
            // 
            this.colListSize.HeaderText = "Size";
            this.colListSize.Name = "colListSize";
            this.colListSize.ReadOnly = true;
            this.colListSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colListSize.Width = 50;
            // 
            // colListValue
            // 
            this.colListValue.HeaderText = "Value";
            this.colListValue.Name = "colListValue";
            this.colListValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colListValue.Width = 240;
            // 
            // colListLastChanged
            // 
            this.colListLastChanged.HeaderText = "Last changed";
            this.colListLastChanged.Name = "colListLastChanged";
            this.colListLastChanged.ReadOnly = true;
            this.colListLastChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colListLastChanged.Width = 100;
            // 
            // colListChangedBy
            // 
            this.colListChangedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colListChangedBy.HeaderText = "Changed by";
            this.colListChangedBy.Name = "colListChangedBy";
            this.colListChangedBy.ReadOnly = true;
            this.colListChangedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // tlsList
            // 
            this.tlsList.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnListAdd,
            this.btnListEdit,
            this.sepListRemove,
            this.btnListRemove,
            this.btnListRefresh});
            this.tlsList.Location = new System.Drawing.Point(0, 0);
            this.tlsList.Name = "tlsList";
            this.tlsList.Size = new System.Drawing.Size(672, 25);
            this.tlsList.TabIndex = 22;
            // 
            // btnListAdd
            // 
            this.btnListAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnListAdd.Image")));
            this.btnListAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnListAdd.Name = "btnListAdd";
            this.btnListAdd.Size = new System.Drawing.Size(93, 22);
            this.btnListAdd.Text = "Add variable";
            this.btnListAdd.Click += new System.EventHandler(this.btnListAdd_Click);
            // 
            // btnListEdit
            // 
            this.btnListEdit.Enabled = false;
            this.btnListEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnListEdit.Image")));
            this.btnListEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnListEdit.Name = "btnListEdit";
            this.btnListEdit.Size = new System.Drawing.Size(91, 22);
            this.btnListEdit.Text = "Edit variable";
            this.btnListEdit.Click += new System.EventHandler(this.btnListEdit_Click);
            // 
            // sepListRemove
            // 
            this.sepListRemove.Name = "sepListRemove";
            this.sepListRemove.Size = new System.Drawing.Size(6, 25);
            // 
            // btnListRemove
            // 
            this.btnListRemove.Enabled = false;
            this.btnListRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnListRemove.Image")));
            this.btnListRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnListRemove.Name = "btnListRemove";
            this.btnListRemove.Size = new System.Drawing.Size(114, 22);
            this.btnListRemove.Text = "Remove variable";
            this.btnListRemove.Click += new System.EventHandler(this.btnListRemove_Click);
            // 
            // btnListRefresh
            // 
            this.btnListRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnListRefresh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnListRemoveAll});
            this.btnListRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnListRefresh.Image")));
            this.btnListRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnListRefresh.Name = "btnListRefresh";
            this.btnListRefresh.Size = new System.Drawing.Size(78, 22);
            this.btnListRefresh.Text = "Refresh";
            this.btnListRefresh.ButtonClick += new System.EventHandler(this.btnListRefresh_ButtonClick);
            // 
            // btnListRemoveAll
            // 
            this.btnListRemoveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnListRemoveAll.Image")));
            this.btnListRemoveAll.Name = "btnListRemoveAll";
            this.btnListRemoveAll.Size = new System.Drawing.Size(181, 22);
            this.btnListRemoveAll.Text = "Remove all variables";
            this.btnListRemoveAll.Click += new System.EventHandler(this.btnListRemoveAll_Click);
            // 
            // tabListPersistent
            // 
            this.tabListPersistent.Controls.Add(this.pnlListPersistent);
            this.tabListPersistent.ImageKey = "media-floppy.png";
            this.tabListPersistent.Location = new System.Drawing.Point(4, 23);
            this.tabListPersistent.Name = "tabListPersistent";
            this.tabListPersistent.Padding = new System.Windows.Forms.Padding(3);
            this.tabListPersistent.Size = new System.Drawing.Size(678, 372);
            this.tabListPersistent.TabIndex = 1;
            this.tabListPersistent.Text = "Persistent";
            this.tabListPersistent.UseVisualStyleBackColor = true;
            // 
            // pnlListPersistent
            // 
            this.pnlListPersistent.Controls.Add(this.dgvPeListVariables);
            this.pnlListPersistent.Controls.Add(this.tlsPeList);
            this.pnlListPersistent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlListPersistent.Location = new System.Drawing.Point(3, 3);
            this.pnlListPersistent.Name = "pnlListPersistent";
            this.pnlListPersistent.Size = new System.Drawing.Size(672, 366);
            this.pnlListPersistent.TabIndex = 24;
            // 
            // dgvPeListVariables
            // 
            this.dgvPeListVariables.AllowUserToAddRows = false;
            this.dgvPeListVariables.AllowUserToDeleteRows = false;
            this.dgvPeListVariables.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvPeListVariables.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPeListVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPeListVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPeListName,
            this.colPeListSize,
            this.colPeListValue,
            this.colPeListLastChanged,
            this.colPeListChangedBy});
            this.dgvPeListVariables.ContextMenuStrip = this.ctxState;
            this.dgvPeListVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPeListVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPeListVariables.Location = new System.Drawing.Point(0, 25);
            this.dgvPeListVariables.Name = "dgvPeListVariables";
            this.dgvPeListVariables.RowHeadersVisible = false;
            this.dgvPeListVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPeListVariables.ShowCellErrors = false;
            this.dgvPeListVariables.ShowEditingIcon = false;
            this.dgvPeListVariables.ShowRowErrors = false;
            this.dgvPeListVariables.Size = new System.Drawing.Size(672, 341);
            this.dgvPeListVariables.TabIndex = 21;
            this.dgvPeListVariables.VirtualMode = true;
            this.dgvPeListVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPeListVariables_CellDoubleClick);
            this.dgvPeListVariables.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvPeListVariables_CellValueNeeded);
            this.dgvPeListVariables.SelectionChanged += new System.EventHandler(this.dgvPeListVariables_SelectionChanged);
            this.dgvPeListVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPeListVariables_KeyDown);
            this.dgvPeListVariables.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colPeListName
            // 
            this.colPeListName.Frozen = true;
            this.colPeListName.HeaderText = "Name";
            this.colPeListName.Name = "colPeListName";
            this.colPeListName.ReadOnly = true;
            this.colPeListName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeListName.MinimumWidth = 50;
            this.colPeListName.Width = 200;
            // 
            // colPeListSize
            // 
            this.colPeListSize.HeaderText = "Size";
            this.colPeListSize.Name = "colPeListSize";
            this.colPeListSize.ReadOnly = true;
            this.colPeListSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeListSize.MinimumWidth = 50;
            this.colPeListSize.Width = 50;
            // 
            // colPeListValue
            // 
            this.colPeListValue.HeaderText = "Value";
            this.colPeListValue.Name = "colPeListValue";
            this.colPeListValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeListValue.MinimumWidth = 50;
            this.colPeListValue.Width = 240;
            // 
            // colPeListLastChanged
            // 
            this.colPeListLastChanged.HeaderText = "Last changed";
            this.colPeListLastChanged.Name = "colPeListLastChanged";
            this.colPeListLastChanged.ReadOnly = true;
            this.colPeListLastChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeListLastChanged.MinimumWidth = 50;
            this.colPeListLastChanged.Width = 100;
            // 
            // colPeListChangedBy
            // 
            this.colPeListChangedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPeListChangedBy.HeaderText = "Changed by";
            this.colPeListChangedBy.Name = "colPeListChangedBy";
            this.colPeListChangedBy.ReadOnly = true;
            this.colPeListChangedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeListChangedBy.MinimumWidth = 50;
            // 
            // tlsPeList
            // 
            this.tlsPeList.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsPeList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPeListAdd,
            this.btnPeListEdit,
            this.sepPeListRemove,
            this.btnPeListRemove,
            this.btnPeListRefresh});
            this.tlsPeList.Location = new System.Drawing.Point(0, 0);
            this.tlsPeList.Name = "tlsPeList";
            this.tlsPeList.Size = new System.Drawing.Size(672, 25);
            this.tlsPeList.TabIndex = 22;
            // 
            // btnPeListAdd
            // 
            this.btnPeListAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnPeListAdd.Image")));
            this.btnPeListAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeListAdd.Name = "btnPeListAdd";
            this.btnPeListAdd.Size = new System.Drawing.Size(93, 22);
            this.btnPeListAdd.Text = "Add variable";
            this.btnPeListAdd.Click += new System.EventHandler(this.btnPeListAdd_Click);
            // 
            // btnPeListEdit
            // 
            this.btnPeListEdit.Enabled = false;
            this.btnPeListEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnPeListEdit.Image")));
            this.btnPeListEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeListEdit.Name = "btnPeListEdit";
            this.btnPeListEdit.Size = new System.Drawing.Size(91, 22);
            this.btnPeListEdit.Text = "Edit variable";
            this.btnPeListEdit.Click += new System.EventHandler(this.btnPeListEdit_Click);
            // 
            // sepPeListRemove
            // 
            this.sepPeListRemove.Name = "sepPeListRemove";
            this.sepPeListRemove.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPeListRemove
            // 
            this.btnPeListRemove.Enabled = false;
            this.btnPeListRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnPeListRemove.Image")));
            this.btnPeListRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeListRemove.Name = "btnPeListRemove";
            this.btnPeListRemove.Size = new System.Drawing.Size(114, 22);
            this.btnPeListRemove.Text = "Remove variable";
            this.btnPeListRemove.Click += new System.EventHandler(this.btnPeListRemove_Click);
            // 
            // btnPeListRefresh
            // 
            this.btnPeListRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnPeListRefresh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPeListRemoveAll});
            this.btnPeListRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnPeListRefresh.Image")));
            this.btnPeListRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeListRefresh.Name = "btnPeListRefresh";
            this.btnPeListRefresh.Size = new System.Drawing.Size(78, 22);
            this.btnPeListRefresh.Text = "Refresh";
            this.btnPeListRefresh.ButtonClick += new System.EventHandler(this.btnPeListRefresh_ButtonClick);
            // 
            // btnPeListRemoveAll
            // 
            this.btnPeListRemoveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnPeListRemoveAll.Image")));
            this.btnPeListRemoveAll.Name = "btnPeListRemoveAll";
            this.btnPeListRemoveAll.Size = new System.Drawing.Size(181, 22);
            this.btnPeListRemoveAll.Text = "Remove all variables";
            this.btnPeListRemoveAll.Click += new System.EventHandler(this.btnPeListRemoveAll_Click);
            // 
            // tabTable
            // 
            this.tabTable.Controls.Add(this.tbcTable);
            this.tabTable.ImageKey = "var_table.png";
            this.tabTable.Location = new System.Drawing.Point(4, 23);
            this.tabTable.Name = "tabTable";
            this.tabTable.Padding = new System.Windows.Forms.Padding(10);
            this.tabTable.Size = new System.Drawing.Size(706, 419);
            this.tabTable.TabIndex = 2;
            this.tabTable.Text = "Table variables";
            this.tabTable.UseVisualStyleBackColor = true;
            // 
            // tbcTable
            // 
            this.tbcTable.Controls.Add(this.tabTableSession);
            this.tbcTable.Controls.Add(this.tabTablePersistent);
            this.tbcTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcTable.ImageList = this.imlStateTabs;
            this.tbcTable.Location = new System.Drawing.Point(10, 10);
            this.tbcTable.Name = "tbcTable";
            this.tbcTable.SelectedIndex = 0;
            this.tbcTable.Size = new System.Drawing.Size(686, 399);
            this.tbcTable.TabIndex = 25;
            this.tbcTable.SelectedIndexChanged += new System.EventHandler(this.tbcTable_SelectedIndexChanged);
            // 
            // tabTableSession
            // 
            this.tabTableSession.Controls.Add(this.pnlTableSession);
            this.tabTableSession.ImageKey = "appointment-new.png";
            this.tabTableSession.Location = new System.Drawing.Point(4, 23);
            this.tabTableSession.Name = "tabTableSession";
            this.tabTableSession.Padding = new System.Windows.Forms.Padding(3);
            this.tabTableSession.Size = new System.Drawing.Size(678, 372);
            this.tabTableSession.TabIndex = 0;
            this.tabTableSession.Text = "Session";
            this.tabTableSession.UseVisualStyleBackColor = true;
            // 
            // pnlTableSession
            // 
            this.pnlTableSession.Controls.Add(this.dgvTableVariables);
            this.pnlTableSession.Controls.Add(this.tlsTable);
            this.pnlTableSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTableSession.Location = new System.Drawing.Point(3, 3);
            this.pnlTableSession.Name = "pnlTableSession";
            this.pnlTableSession.Size = new System.Drawing.Size(672, 366);
            this.pnlTableSession.TabIndex = 24;
            // 
            // dgvTableVariables
            // 
            this.dgvTableVariables.AllowUserToAddRows = false;
            this.dgvTableVariables.AllowUserToDeleteRows = false;
            this.dgvTableVariables.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvTableVariables.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvTableVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTableVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTableName,
            this.colTableWidth,
            this.colTableHeight,
            this.colTableLastChanged,
            this.colTableChangedBy});
            this.dgvTableVariables.ContextMenuStrip = this.ctxState;
            this.dgvTableVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTableVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvTableVariables.Location = new System.Drawing.Point(0, 25);
            this.dgvTableVariables.Name = "dgvTableVariables";
            this.dgvTableVariables.RowHeadersVisible = false;
            this.dgvTableVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTableVariables.ShowCellErrors = false;
            this.dgvTableVariables.ShowEditingIcon = false;
            this.dgvTableVariables.ShowRowErrors = false;
            this.dgvTableVariables.Size = new System.Drawing.Size(672, 341);
            this.dgvTableVariables.TabIndex = 22;
            this.dgvTableVariables.VirtualMode = true;
            this.dgvTableVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTableVariables_CellDoubleClick);
            this.dgvTableVariables.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvTableVariables_CellValueNeeded);
            this.dgvTableVariables.SelectionChanged += new System.EventHandler(this.dgvTableVariables_SelectionChanged);
            this.dgvTableVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTableVariables_KeyDown);
            this.dgvTableVariables.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colTableName
            // 
            this.colTableName.HeaderText = "Name";
            this.colTableName.Name = "colTableName";
            this.colTableName.ReadOnly = true;
            this.colTableName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTableName.MinimumWidth = 50;
            this.colTableName.Width = 240;
            // 
            // colTableWidth
            // 
            this.colTableWidth.HeaderText = "Width";
            this.colTableWidth.Name = "colTableWidth";
            this.colTableWidth.ReadOnly = true;
            this.colTableWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTableWidth.MinimumWidth = 50;
            this.colTableWidth.Width = 70;
            // 
            // colTableHeight
            // 
            this.colTableHeight.HeaderText = "Height";
            this.colTableHeight.Name = "colTableHeight";
            this.colTableHeight.ReadOnly = true;
            this.colTableHeight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTableHeight.Width = 70;
            // 
            // colTableLastChanged
            // 
            this.colTableLastChanged.HeaderText = "Last changed";
            this.colTableLastChanged.Name = "colTableLastChanged";
            this.colTableLastChanged.ReadOnly = true;
            this.colTableLastChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTableLastChanged.MinimumWidth = 50;
            this.colTableLastChanged.Width = 120;
            // 
            // colTableChangedBy
            // 
            this.colTableChangedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTableChangedBy.HeaderText = "Changed by";
            this.colTableChangedBy.Name = "colTableChangedBy";
            this.colTableChangedBy.ReadOnly = true;
            this.colTableChangedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTableChangedBy.MinimumWidth = 50;
            // 
            // tlsTable
            // 
            this.tlsTable.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTableAdd,
            this.btnTableEdit,
            this.sepTableRemove,
            this.btnTableRemove,
            this.btnTableRefresh});
            this.tlsTable.Location = new System.Drawing.Point(0, 0);
            this.tlsTable.Name = "tlsTable";
            this.tlsTable.Size = new System.Drawing.Size(672, 25);
            this.tlsTable.TabIndex = 23;
            // 
            // btnTableAdd
            // 
            this.btnTableAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnTableAdd.Image")));
            this.btnTableAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTableAdd.Name = "btnTableAdd";
            this.btnTableAdd.Size = new System.Drawing.Size(93, 22);
            this.btnTableAdd.Text = "Add variable";
            this.btnTableAdd.Click += new System.EventHandler(this.btnTableAdd_Click);
            // 
            // btnTableEdit
            // 
            this.btnTableEdit.Enabled = false;
            this.btnTableEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnTableEdit.Image")));
            this.btnTableEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTableEdit.Name = "btnTableEdit";
            this.btnTableEdit.Size = new System.Drawing.Size(91, 22);
            this.btnTableEdit.Text = "Edit variable";
            this.btnTableEdit.Click += new System.EventHandler(this.btnTableEdit_Click);
            // 
            // sepTableRemove
            // 
            this.sepTableRemove.Name = "sepTableRemove";
            this.sepTableRemove.Size = new System.Drawing.Size(6, 25);
            // 
            // btnTableRemove
            // 
            this.btnTableRemove.Enabled = false;
            this.btnTableRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnTableRemove.Image")));
            this.btnTableRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTableRemove.Name = "btnTableRemove";
            this.btnTableRemove.Size = new System.Drawing.Size(114, 22);
            this.btnTableRemove.Text = "Remove variable";
            this.btnTableRemove.Click += new System.EventHandler(this.btnTableRemove_Click);
            // 
            // btnTableRefresh
            // 
            this.btnTableRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnTableRefresh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTableRemoveAll});
            this.btnTableRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnTableRefresh.Image")));
            this.btnTableRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTableRefresh.Name = "btnTableRefresh";
            this.btnTableRefresh.Size = new System.Drawing.Size(78, 22);
            this.btnTableRefresh.Text = "Refresh";
            this.btnTableRefresh.ButtonClick += new System.EventHandler(this.btnTableRefresh_ButtonClick);
            // 
            // btnTableRemoveAll
            // 
            this.btnTableRemoveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnTableRemoveAll.Image")));
            this.btnTableRemoveAll.Name = "btnTableRemoveAll";
            this.btnTableRemoveAll.Size = new System.Drawing.Size(181, 22);
            this.btnTableRemoveAll.Text = "Remove all variables";
            this.btnTableRemoveAll.Click += new System.EventHandler(this.btnTableRemoveAll_Click);
            // 
            // tabTablePersistent
            // 
            this.tabTablePersistent.Controls.Add(this.pnlTablePersistent);
            this.tabTablePersistent.ImageKey = "media-floppy.png";
            this.tabTablePersistent.Location = new System.Drawing.Point(4, 23);
            this.tabTablePersistent.Name = "tabTablePersistent";
            this.tabTablePersistent.Padding = new System.Windows.Forms.Padding(3);
            this.tabTablePersistent.Size = new System.Drawing.Size(678, 372);
            this.tabTablePersistent.TabIndex = 1;
            this.tabTablePersistent.Text = "Persistent";
            this.tabTablePersistent.UseVisualStyleBackColor = true;
            // 
            // pnlTablePersistent
            // 
            this.pnlTablePersistent.Controls.Add(this.dgvPeTableVariables);
            this.pnlTablePersistent.Controls.Add(this.tlsPeTable);
            this.pnlTablePersistent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTablePersistent.Location = new System.Drawing.Point(3, 3);
            this.pnlTablePersistent.Name = "pnlTablePersistent";
            this.pnlTablePersistent.Size = new System.Drawing.Size(672, 366);
            this.pnlTablePersistent.TabIndex = 25;
            // 
            // dgvPeTableVariables
            // 
            this.dgvPeTableVariables.AllowUserToAddRows = false;
            this.dgvPeTableVariables.AllowUserToDeleteRows = false;
            this.dgvPeTableVariables.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvPeTableVariables.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPeTableVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPeTableVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPeTableName,
            this.colPeTableWidth,
            this.colPeTableHeight,
            this.colPeTableLastChanged,
            this.colPeTableChangedBy});
            this.dgvPeTableVariables.ContextMenuStrip = this.ctxState;
            this.dgvPeTableVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPeTableVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPeTableVariables.Location = new System.Drawing.Point(0, 25);
            this.dgvPeTableVariables.Name = "dgvPeTableVariables";
            this.dgvPeTableVariables.RowHeadersVisible = false;
            this.dgvPeTableVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPeTableVariables.ShowCellErrors = false;
            this.dgvPeTableVariables.ShowEditingIcon = false;
            this.dgvPeTableVariables.ShowRowErrors = false;
            this.dgvPeTableVariables.Size = new System.Drawing.Size(672, 341);
            this.dgvPeTableVariables.TabIndex = 22;
            this.dgvPeTableVariables.VirtualMode = true;
            this.dgvPeTableVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPeTableVariables_CellDoubleClick);
            this.dgvPeTableVariables.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvPeTableVariables_CellValueNeeded);
            this.dgvPeTableVariables.SelectionChanged += new System.EventHandler(this.dgvPeTableVariables_SelectionChanged);
            this.dgvPeTableVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPeTableVariables_KeyDown);
            this.dgvPeTableVariables.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colPeTableName
            // 
            this.colPeTableName.HeaderText = "Name";
            this.colPeTableName.Name = "colPeTableName";
            this.colPeTableName.ReadOnly = true;
            this.colPeTableName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeTableName.MinimumWidth = 50;
            this.colPeTableName.Width = 240;
            // 
            // colPeTableWidth
            // 
            this.colPeTableWidth.HeaderText = "Width";
            this.colPeTableWidth.Name = "colPeTableWidth";
            this.colPeTableWidth.ReadOnly = true;
            this.colPeTableWidth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeTableWidth.MinimumWidth = 50;
            this.colPeTableWidth.Width = 70;
            // 
            // colPeTableHeight
            // 
            this.colPeTableHeight.HeaderText = "Height";
            this.colPeTableHeight.Name = "colPeTableHeight";
            this.colPeTableHeight.ReadOnly = true;
            this.colPeTableHeight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeTableHeight.MinimumWidth = 50;
            this.colPeTableHeight.Width = 70;
            // 
            // colPeTableLastChanged
            // 
            this.colPeTableLastChanged.HeaderText = "Last changed";
            this.colPeTableLastChanged.Name = "colPeTableLastChanged";
            this.colPeTableLastChanged.ReadOnly = true;
            this.colPeTableLastChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeTableLastChanged.MinimumWidth = 50;
            this.colPeTableLastChanged.Width = 160;
            // 
            // colPeTableChangedBy
            // 
            this.colPeTableChangedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPeTableChangedBy.HeaderText = "Changed by";
            this.colPeTableChangedBy.Name = "colPeTableChangedBy";
            this.colPeTableChangedBy.ReadOnly = true;
            this.colPeTableChangedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeTableChangedBy.MinimumWidth = 50;
            // 
            // tlsPeTable
            // 
            this.tlsPeTable.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsPeTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPeTableAdd,
            this.btnPeTableEdit,
            this.sepPeTableRemove,
            this.btnPeTableRemove,
            this.btnPeTableRefresh});
            this.tlsPeTable.Location = new System.Drawing.Point(0, 0);
            this.tlsPeTable.Name = "tlsPeTable";
            this.tlsPeTable.Size = new System.Drawing.Size(672, 25);
            this.tlsPeTable.TabIndex = 23;
            // 
            // btnPeTableAdd
            // 
            this.btnPeTableAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnPeTableAdd.Image")));
            this.btnPeTableAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeTableAdd.Name = "btnPeTableAdd";
            this.btnPeTableAdd.Size = new System.Drawing.Size(93, 22);
            this.btnPeTableAdd.Text = "Add variable";
            this.btnPeTableAdd.Click += new System.EventHandler(this.btnPeTableAdd_Click);
            // 
            // btnPeTableEdit
            // 
            this.btnPeTableEdit.Enabled = false;
            this.btnPeTableEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnPeTableEdit.Image")));
            this.btnPeTableEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeTableEdit.Name = "btnPeTableEdit";
            this.btnPeTableEdit.Size = new System.Drawing.Size(91, 22);
            this.btnPeTableEdit.Text = "Edit variable";
            this.btnPeTableEdit.Click += new System.EventHandler(this.btnPeTableEdit_Click);
            // 
            // sepPeTableRemove
            // 
            this.sepPeTableRemove.Name = "sepPeTableRemove";
            this.sepPeTableRemove.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPeTableRemove
            // 
            this.btnPeTableRemove.Enabled = false;
            this.btnPeTableRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnPeTableRemove.Image")));
            this.btnPeTableRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeTableRemove.Name = "btnPeTableRemove";
            this.btnPeTableRemove.Size = new System.Drawing.Size(114, 22);
            this.btnPeTableRemove.Text = "Remove variable";
            this.btnPeTableRemove.Click += new System.EventHandler(this.btnPeTableRemove_Click);
            // 
            // btnPeTableRefresh
            // 
            this.btnPeTableRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnPeTableRefresh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPeTableRemoveAll});
            this.btnPeTableRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnPeTableRefresh.Image")));
            this.btnPeTableRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeTableRefresh.Name = "btnPeTableRefresh";
            this.btnPeTableRefresh.Size = new System.Drawing.Size(78, 22);
            this.btnPeTableRefresh.Text = "Refresh";
            this.btnPeTableRefresh.ButtonClick += new System.EventHandler(this.btnPeTableRefresh_ButtonClick);
            // 
            // btnPeTableRemoveAll
            // 
            this.btnPeTableRemoveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnPeTableRemoveAll.Image")));
            this.btnPeTableRemoveAll.Name = "btnPeTableRemoveAll";
            this.btnPeTableRemoveAll.Size = new System.Drawing.Size(181, 22);
            this.btnPeTableRemoveAll.Text = "Remove all variables";
            this.btnPeTableRemoveAll.Click += new System.EventHandler(this.btnPeTableRemoveAll_Click);
            // 
            // tabDict
            // 
            this.tabDict.Controls.Add(this.tbcDict);
            this.tabDict.ImageKey = "var_dict.png";
            this.tabDict.Location = new System.Drawing.Point(4, 23);
            this.tabDict.Name = "tabDict";
            this.tabDict.Padding = new System.Windows.Forms.Padding(10);
            this.tabDict.Size = new System.Drawing.Size(706, 419);
            this.tabDict.TabIndex = 1;
            this.tabDict.Text = "Dict variables";
            this.tabDict.UseVisualStyleBackColor = true;
            // 
            // tbcDict
            // 
            this.tbcDict.Controls.Add(this.tabDictSession);
            this.tbcDict.Controls.Add(this.tabDictPersistent);
            this.tbcDict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcDict.ImageList = this.imlStateTabs;
            this.tbcDict.Location = new System.Drawing.Point(10, 10);
            this.tbcDict.Name = "tbcDict";
            this.tbcDict.SelectedIndex = 0;
            this.tbcDict.Size = new System.Drawing.Size(686, 399);
            this.tbcDict.TabIndex = 24;
            this.tbcDict.SelectedIndexChanged += new System.EventHandler(this.tbcDict_SelectedIndexChanged);
            // 
            // tabDictSession
            // 
            this.tabDictSession.Controls.Add(this.panelDict);
            this.tabDictSession.ImageKey = "appointment-new.png";
            this.tabDictSession.Location = new System.Drawing.Point(4, 23);
            this.tabDictSession.Name = "tabDictSession";
            this.tabDictSession.Padding = new System.Windows.Forms.Padding(3);
            this.tabDictSession.Size = new System.Drawing.Size(678, 372);
            this.tabDictSession.TabIndex = 0;
            this.tabDictSession.Text = "Session";
            this.tabDictSession.UseVisualStyleBackColor = true;
            // 
            // panelDict
            // 
            this.panelDict.Controls.Add(this.dgvDictVariables);
            this.panelDict.Controls.Add(this.tlsDict);
            this.panelDict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDict.Location = new System.Drawing.Point(3, 3);
            this.panelDict.Name = "panelDict";
            this.panelDict.Size = new System.Drawing.Size(672, 366);
            this.panelDict.TabIndex = 23;
            // 
            // dgvDictVariables
            // 
            this.dgvDictVariables.AllowUserToAddRows = false;
            this.dgvDictVariables.AllowUserToDeleteRows = false;
            this.dgvDictVariables.AllowUserToResizeRows = false;
            dgvCellStyleDict.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvDictVariables.AlternatingRowsDefaultCellStyle = dgvCellStyleDict;
            this.dgvDictVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDictVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDictName,
            this.colDictSize,
            this.colDictValue,
            this.colDictLastChanged,
            this.colDictChangedBy});
            this.dgvDictVariables.ContextMenuStrip = this.ctxState;
            this.dgvDictVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDictVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvDictVariables.Location = new System.Drawing.Point(0, 25);
            this.dgvDictVariables.Name = "dgvDictVariables";
            this.dgvDictVariables.RowHeadersVisible = false;
            this.dgvDictVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDictVariables.ShowCellErrors = false;
            this.dgvDictVariables.ShowEditingIcon = false;
            this.dgvDictVariables.ShowRowErrors = false;
            this.dgvDictVariables.Size = new System.Drawing.Size(672, 341);
            this.dgvDictVariables.TabIndex = 21;
            this.dgvDictVariables.VirtualMode = true;
            this.dgvDictVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDictVariables_CellDoubleClick);
            this.dgvDictVariables.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvDictVariables_CellValueNeeded);
            this.dgvDictVariables.SelectionChanged += new System.EventHandler(this.dgvDictVariables_SelectionChanged);
            this.dgvDictVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvDictVariables_KeyDown);
            this.dgvDictVariables.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colDictName
            // 
            this.colDictName.Frozen = true;
            this.colDictName.HeaderText = "Name";
            this.colDictName.Name = "colDictName";
            this.colDictName.ReadOnly = true;
            this.colDictName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colDictName.Width = 200;
            // 
            // colDictSize
            // 
            this.colDictSize.HeaderText = "Size";
            this.colDictSize.Name = "colDictSize";
            this.colDictSize.ReadOnly = true;
            this.colDictSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colDictSize.Width = 50;
            // 
            // colDictValue
            // 
            this.colDictValue.HeaderText = "Value";
            this.colDictValue.Name = "colDictValue";
            this.colDictValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDictValue.Width = 240;
            // 
            // colDictLastChanged
            // 
            this.colDictLastChanged.HeaderText = "Last changed";
            this.colDictLastChanged.Name = "colDictLastChanged";
            this.colDictLastChanged.ReadOnly = true;
            this.colDictLastChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colDictLastChanged.Width = 100;
            // 
            // colDictChangedBy
            // 
            this.colDictChangedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDictChangedBy.HeaderText = "Changed by";
            this.colDictChangedBy.Name = "colDictChangedBy";
            this.colDictChangedBy.ReadOnly = true;
            this.colDictChangedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // tlsDict
            // 
            this.tlsDict.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsDict.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDictAdd,
            this.btnDictEdit,
            this.toolStripSeparatorDict,
            this.btnDictRemove,
            this.btnDictRefresh});
            this.tlsDict.Location = new System.Drawing.Point(0, 0);
            this.tlsDict.Name = "tlsDict";
            this.tlsDict.Size = new System.Drawing.Size(672, 25);
            this.tlsDict.TabIndex = 22;
            // 
            // btnDictAdd
            // 
            this.btnDictAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnDictAdd.Image")));
            this.btnDictAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDictAdd.Name = "btnDictAdd";
            this.btnDictAdd.Size = new System.Drawing.Size(93, 22);
            this.btnDictAdd.Text = "Add variable";
            this.btnDictAdd.Click += new System.EventHandler(this.btnDictAdd_Click);
            // 
            // btnDictEdit
            // 
            this.btnDictEdit.Enabled = false;
            this.btnDictEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnDictEdit.Image")));
            this.btnDictEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDictEdit.Name = "btnDictEdit";
            this.btnDictEdit.Size = new System.Drawing.Size(91, 22);
            this.btnDictEdit.Text = "Edit variable";
            this.btnDictEdit.Click += new System.EventHandler(this.btnDictEdit_Click);
            // 
            // toolStripSeparatorDict
            // 
            this.toolStripSeparatorDict.Name = "toolStripSeparatorDict";
            this.toolStripSeparatorDict.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDictRemove
            // 
            this.btnDictRemove.Enabled = false;
            this.btnDictRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnDictRemove.Image")));
            this.btnDictRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDictRemove.Name = "btnDictRemove";
            this.btnDictRemove.Size = new System.Drawing.Size(114, 22);
            this.btnDictRemove.Text = "Remove variable";
            this.btnDictRemove.Click += new System.EventHandler(this.btnDictRemove_Click);
            // 
            // btnDictRefresh
            // 
            this.btnDictRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnDictRefresh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDictRemoveAll});
            this.btnDictRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnDictRefresh.Image")));
            this.btnDictRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDictRefresh.Name = "btnDictRefresh";
            this.btnDictRefresh.Size = new System.Drawing.Size(78, 22);
            this.btnDictRefresh.Text = "Refresh";
            this.btnDictRefresh.ButtonClick += new System.EventHandler(this.btnDictRefresh_ButtonClick);
            // 
            // btnDictRemoveAll
            // 
            this.btnDictRemoveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnDictRemoveAll.Image")));
            this.btnDictRemoveAll.Name = "btnDictRemoveAll";
            this.btnDictRemoveAll.Size = new System.Drawing.Size(181, 22);
            this.btnDictRemoveAll.Text = "Remove all variables";
            this.btnDictRemoveAll.Click += new System.EventHandler(this.btnDictRemoveAll_Click);
            // 
            // tabDictPersistent
            // 
            this.tabDictPersistent.Controls.Add(this.panelPeDict);
            this.tabDictPersistent.ImageKey = "media-floppy.png";
            this.tabDictPersistent.Location = new System.Drawing.Point(4, 23);
            this.tabDictPersistent.Name = "tabDictPersistent";
            this.tabDictPersistent.Padding = new System.Windows.Forms.Padding(3);
            this.tabDictPersistent.Size = new System.Drawing.Size(678, 372);
            this.tabDictPersistent.TabIndex = 1;
            this.tabDictPersistent.Text = "Persistent";
            this.tabDictPersistent.UseVisualStyleBackColor = true;
            // 
            // panelPeDict
            // 
            this.panelPeDict.Controls.Add(this.dgvPeDictVariables);
            this.panelPeDict.Controls.Add(this.tlsPeDict);
            this.panelPeDict.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPeDict.Location = new System.Drawing.Point(3, 3);
            this.panelPeDict.Name = "panelPeDict";
            this.panelPeDict.Size = new System.Drawing.Size(672, 366);
            this.panelPeDict.TabIndex = 24;
            // 
            // dgvPeDictVariables
            // 
            this.dgvPeDictVariables.AllowUserToAddRows = false;
            this.dgvPeDictVariables.AllowUserToDeleteRows = false;
            this.dgvPeDictVariables.AllowUserToResizeRows = false;
            dgvCellStylePeDict.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvPeDictVariables.AlternatingRowsDefaultCellStyle = dgvCellStylePeDict;
            this.dgvPeDictVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPeDictVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPeDictName,
            this.colPeDictSize,
            this.colPeDictValue,
            this.colPeDictLastChanged,
            this.colPeDictChangedBy});
            this.dgvPeDictVariables.ContextMenuStrip = this.ctxState;
            this.dgvPeDictVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPeDictVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPeDictVariables.Location = new System.Drawing.Point(0, 25);
            this.dgvPeDictVariables.Name = "dgvPeDictVariables";
            this.dgvPeDictVariables.RowHeadersVisible = false;
            this.dgvPeDictVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPeDictVariables.ShowCellErrors = false;
            this.dgvPeDictVariables.ShowEditingIcon = false;
            this.dgvPeDictVariables.ShowRowErrors = false;
            this.dgvPeDictVariables.Size = new System.Drawing.Size(672, 341);
            this.dgvPeDictVariables.TabIndex = 21;
            this.dgvPeDictVariables.VirtualMode = true;
            this.dgvPeDictVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPeDictVariables_CellDoubleClick);
            this.dgvPeDictVariables.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvPeDictVariables_CellValueNeeded);
            this.dgvPeDictVariables.SelectionChanged += new System.EventHandler(this.dgvPeDictVariables_SelectionChanged);
            this.dgvPeDictVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPeDictVariables_KeyDown);
            this.dgvPeDictVariables.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colPeDictName
            // 
            this.colPeDictName.Frozen = true;
            this.colPeDictName.HeaderText = "Name";
            this.colPeDictName.Name = "colPeDictName";
            this.colPeDictName.ReadOnly = true;
            this.colPeDictName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeDictName.Width = 200;
            // 
            // colPeDictSize
            // 
            this.colPeDictSize.HeaderText = "Size";
            this.colPeDictSize.Name = "colPeDictSize";
            this.colPeDictSize.ReadOnly = true;
            this.colPeDictSize.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeDictSize.Width = 50;
            // 
            // colPeDictValue
            // 
            this.colPeDictValue.HeaderText = "Value";
            this.colPeDictValue.Name = "colPeDictValue";
            this.colPeDictValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPeDictValue.Width = 240;
            // 
            // colPeDictLastChanged
            // 
            this.colPeDictLastChanged.HeaderText = "Last changed";
            this.colPeDictLastChanged.Name = "colPeDictLastChanged";
            this.colPeDictLastChanged.ReadOnly = true;
            this.colPeDictLastChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colPeDictLastChanged.Width = 100;
            // 
            // colPeDictChangedBy
            // 
            this.colPeDictChangedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPeDictChangedBy.HeaderText = "Changed by";
            this.colPeDictChangedBy.Name = "colPeDictChangedBy";
            this.colPeDictChangedBy.ReadOnly = true;
            this.colPeDictChangedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // tlsPeDict
            // 
            this.tlsPeDict.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsPeDict.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPeDictAdd,
            this.btnPeDictEdit,
            this.toolStripSeparatorPeDict,
            this.btnPeDictRemove,
            this.btnPeDictRefresh});
            this.tlsPeDict.Location = new System.Drawing.Point(0, 0);
            this.tlsPeDict.Name = "tlsPeDict";
            this.tlsPeDict.Size = new System.Drawing.Size(672, 25);
            this.tlsPeDict.TabIndex = 22;
            // 
            // btnPeDictAdd
            // 
            this.btnPeDictAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnPeDictAdd.Image")));
            this.btnPeDictAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeDictAdd.Name = "btnPeDictAdd";
            this.btnPeDictAdd.Size = new System.Drawing.Size(93, 22);
            this.btnPeDictAdd.Text = "Add variable";
            this.btnPeDictAdd.Click += new System.EventHandler(this.btnPeDictAdd_Click);
            // 
            // btnPeDictEdit
            // 
            this.btnPeDictEdit.Enabled = false;
            this.btnPeDictEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnPeDictEdit.Image")));
            this.btnPeDictEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeDictEdit.Name = "btnPeDictEdit";
            this.btnPeDictEdit.Size = new System.Drawing.Size(91, 22);
            this.btnPeDictEdit.Text = "Edit variable";
            this.btnPeDictEdit.Click += new System.EventHandler(this.btnPeDictEdit_Click);
            // 
            // toolStripSeparatorPeDict
            // 
            this.toolStripSeparatorPeDict.Name = "toolStripSeparatorPeDict";
            this.toolStripSeparatorPeDict.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPeDictRemove
            // 
            this.btnPeDictRemove.Enabled = false;
            this.btnPeDictRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnPeDictRemove.Image")));
            this.btnPeDictRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeDictRemove.Name = "btnPeDictRemove";
            this.btnPeDictRemove.Size = new System.Drawing.Size(114, 22);
            this.btnPeDictRemove.Text = "Remove variable";
            this.btnPeDictRemove.Click += new System.EventHandler(this.btnPeDictRemove_Click);
            // 
            // btnPeDictRefresh
            // 
            this.btnPeDictRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnPeDictRefresh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPeDictRemoveAll});
            this.btnPeDictRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnPeDictRefresh.Image")));
            this.btnPeDictRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPeDictRefresh.Name = "btnPeDictRefresh";
            this.btnPeDictRefresh.Size = new System.Drawing.Size(78, 22);
            this.btnPeDictRefresh.Text = "Refresh";
            this.btnPeDictRefresh.ButtonClick += new System.EventHandler(this.btnPeDictRefresh_ButtonClick);
            // 
            // btnPeDictRemoveAll
            // 
            this.btnPeDictRemoveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnPeDictRemoveAll.Image")));
            this.btnPeDictRemoveAll.Name = "btnPeDictRemoveAll";
            this.btnPeDictRemoveAll.Size = new System.Drawing.Size(181, 22);
            this.btnPeDictRemoveAll.Text = "Remove all variables";
            this.btnPeDictRemoveAll.Click += new System.EventHandler(this.btnPeDictRemoveAll_Click);
            // 
            // tabRegisteredVariables
            //
            this.tabRegisteredVariables.Controls.Add(this.dgvRegisteredVariables);
            this.tabRegisteredVariables.Controls.Add(this.tlsRegistered);
            this.tabRegisteredVariables.ImageKey = "var_registered.png";
            this.tabRegisteredVariables.Location = new System.Drawing.Point(4, 23);
            this.tabRegisteredVariables.Name = "tabRegisteredVariables";
            this.tabRegisteredVariables.Padding = new System.Windows.Forms.Padding(3);
            this.tabRegisteredVariables.Size = new System.Drawing.Size(706, 419);
            this.tabRegisteredVariables.TabIndex = 4;
            this.tabRegisteredVariables.Text = "Registered Variables";
            this.tabRegisteredVariables.UseVisualStyleBackColor = true;
            //
            // dgvRegisteredVariables
            //
            this.dgvRegisteredVariables.AllowUserToAddRows = false;
            this.dgvRegisteredVariables.AllowUserToDeleteRows = false;
            this.dgvRegisteredVariables.AllowUserToResizeRows = false;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvRegisteredVariables.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvRegisteredVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRegisteredVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRegisteredName,
            this.colRegisteredValue,
            this.colRegisteredLastChanged,
            this.colRegisteredChangedBy});
            this.dgvRegisteredVariables.ContextMenuStrip = this.ctxState;
            this.dgvRegisteredVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRegisteredVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvRegisteredVariables.Location = new System.Drawing.Point(3, 28);
            this.dgvRegisteredVariables.Name = "dgvRegisteredVariables";
            this.dgvRegisteredVariables.RowHeadersVisible = false;
            this.dgvRegisteredVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRegisteredVariables.ShowCellErrors = false;
            this.dgvRegisteredVariables.ShowEditingIcon = false;
            this.dgvRegisteredVariables.ShowRowErrors = false;
            this.dgvRegisteredVariables.Size = new System.Drawing.Size(700, 388);
            this.dgvRegisteredVariables.TabIndex = 21;
            this.dgvRegisteredVariables.VirtualMode = true;
            this.dgvRegisteredVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRegisteredVariables_CellDoubleClick);
            this.dgvRegisteredVariables.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvRegisteredVariables_CellValueNeeded);
            this.dgvRegisteredVariables.SelectionChanged += new System.EventHandler(this.dgvRegisteredVariables_SelectionChanged);
            this.dgvRegisteredVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvRegisteredVariables_KeyDown);
            this.dgvRegisteredVariables.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            //
            // colRegisteredName
            //
            this.colRegisteredName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colRegisteredName.Frozen = true;
            this.colRegisteredName.HeaderText = "Name";
            this.colRegisteredName.Name = "colRegisteredName";
            this.colRegisteredName.ReadOnly = true;
            this.colRegisteredName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            //
            // colRegisteredValue
            //
            this.colRegisteredValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRegisteredValue.FillWeight = 65F;
            this.colRegisteredValue.HeaderText = "Value";
            this.colRegisteredValue.Name = "colRegisteredValue";
            this.colRegisteredValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            //
            // colRegisteredLastChanged
            //
            this.colRegisteredLastChanged.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colRegisteredLastChanged.HeaderText = "Last changed";
            this.colRegisteredLastChanged.Name = "colRegisteredLastChanged";
            this.colRegisteredLastChanged.ReadOnly = true;
            this.colRegisteredLastChanged.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            //
            // colRegisteredChangedBy
            //
            this.colRegisteredChangedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRegisteredChangedBy.FillWeight = 35F;
            this.colRegisteredChangedBy.HeaderText = "Changed by";
            this.colRegisteredChangedBy.Name = "colRegisteredChangedBy";
            this.colRegisteredChangedBy.ReadOnly = true;
            this.colRegisteredChangedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            //
            // tlsRegistered
            //
            this.tlsRegistered.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsRegistered.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRegisteredAdd,
            this.btnRegisteredEdit,
            this.toolStripSeparatorRegistered,
            this.btnRegisteredRemove,
            this.btnRegisteredRefresh});
            this.tlsRegistered.Location = new System.Drawing.Point(3, 3);
            this.tlsRegistered.Name = "tlsRegistered";
            this.tlsRegistered.Size = new System.Drawing.Size(700, 25);
            this.tlsRegistered.TabIndex = 20;
            //
            // btnRegisteredAdd
            //
            this.btnRegisteredAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnRegisteredAdd.Image")));
            this.btnRegisteredAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRegisteredAdd.Name = "btnRegisteredAdd";
            this.btnRegisteredAdd.Size = new System.Drawing.Size(93, 22);
            this.btnRegisteredAdd.Text = "Add variable";
            this.btnRegisteredAdd.Click += new System.EventHandler(this.btnRegisteredAdd_Click);
            //
            // btnRegisteredEdit
            //
            this.btnRegisteredEdit.Enabled = false;
            this.btnRegisteredEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnRegisteredEdit.Image")));
            this.btnRegisteredEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRegisteredEdit.Name = "btnRegisteredEdit";
            this.btnRegisteredEdit.Size = new System.Drawing.Size(91, 22);
            this.btnRegisteredEdit.Text = "Edit variable";
            this.btnRegisteredEdit.Click += new System.EventHandler(this.btnRegisteredEdit_Click);
            //
            // toolStripSeparatorRegistered
            //
            this.toolStripSeparatorRegistered.Name = "toolStripSeparatorRegistered";
            this.toolStripSeparatorRegistered.Size = new System.Drawing.Size(6, 25);
            //
            // btnRegisteredRemove
            //
            this.btnRegisteredRemove.Enabled = false;
            this.btnRegisteredRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnRegisteredRemove.Image")));
            this.btnRegisteredRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRegisteredRemove.Name = "btnRegisteredRemove";
            this.btnRegisteredRemove.Size = new System.Drawing.Size(114, 22);
            this.btnRegisteredRemove.Text = "Remove variable";
            this.btnRegisteredRemove.Click += new System.EventHandler(this.btnRegisteredRemove_Click);
            //
            // btnRegisteredRefresh
            //
            this.btnRegisteredRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnRegisteredRefresh.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRegisteredRemoveAll});
            this.btnRegisteredRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRegisteredRefresh.Image")));
            this.btnRegisteredRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRegisteredRefresh.Name = "btnRegisteredRefresh";
            this.btnRegisteredRefresh.Size = new System.Drawing.Size(78, 22);
            this.btnRegisteredRefresh.Text = "Refresh";
            this.btnRegisteredRefresh.ButtonClick += new System.EventHandler(this.btnRefreshRegistered_ButtonClick);
            //
            // btnRegisteredRemoveAll
            //
            this.btnRegisteredRemoveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnRegisteredRemoveAll.Image")));
            this.btnRegisteredRemoveAll.Name = "btnRegisteredRemoveAll";
            this.btnRegisteredRemoveAll.Size = new System.Drawing.Size(181, 22);
            this.btnRegisteredRemoveAll.Text = "Remove all variables";
            this.btnRegisteredRemoveAll.Click += new System.EventHandler(this.btnRemoveAllRegistered_Click);
            //
            // tabMutexes
            // 
            this.tabMutexes.Controls.Add(this.dgvMutexes);
            this.tabMutexes.Controls.Add(this.tlsMutex);
            this.tabMutexes.ImageKey = "mutex.png";
            this.tabMutexes.Location = new System.Drawing.Point(4, 23);
            this.tabMutexes.Name = "tabMutexes";
            this.tabMutexes.Padding = new System.Windows.Forms.Padding(3);
            this.tabMutexes.Size = new System.Drawing.Size(706, 419);
            this.tabMutexes.TabIndex = 3;
            this.tabMutexes.Text = "Mutexes";
            this.tabMutexes.UseVisualStyleBackColor = true;
            // 
            // dgvMutexes
            // 
            this.dgvMutexes.AllowUserToAddRows = false;
            this.dgvMutexes.AllowUserToDeleteRows = false;
            this.dgvMutexes.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvMutexes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvMutexes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMutexes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMutexName,
            this.colMutexOwner,
            this.colMutexRefCount,
            this.colMutexQueue});
            this.dgvMutexes.ContextMenuStrip = this.ctxState;
            this.dgvMutexes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMutexes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvMutexes.Location = new System.Drawing.Point(3, 28);
            this.dgvMutexes.Name = "dgvMutexes";
            this.dgvMutexes.RowHeadersVisible = false;
            this.dgvMutexes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMutexes.ShowCellErrors = false;
            this.dgvMutexes.ShowEditingIcon = false;
            this.dgvMutexes.ShowRowErrors = false;
            this.dgvMutexes.Size = new System.Drawing.Size(700, 388);
            this.dgvMutexes.TabIndex = 24;
            this.dgvMutexes.VirtualMode = true;
            this.dgvMutexes.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvMutexes_CellValueNeeded);
            this.dgvMutexes.SelectionChanged += new System.EventHandler(this.dgvMutexes_SelectionChanged);
            this.dgvMutexes.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colMutexName
            // 
            this.colMutexName.HeaderText = "Name";
            this.colMutexName.Name = "colMutexName";
            this.colMutexName.ReadOnly = true;
            this.colMutexName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colMutexName.Width = 150;
            // 
            // colMutexOwner
            // 
            this.colMutexOwner.HeaderText = "Current owner";
            this.colMutexOwner.Name = "colMutexOwner";
            // 
            // colMutexRefCount
            // 
            this.colMutexRefCount.HeaderText = "Refcount";
            this.colMutexRefCount.Name = "colMutexRefCount";
            this.colMutexRefCount.Width = 75;
            // 
            // colMutexQueue
            // 
            this.colMutexQueue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMutexQueue.HeaderText = "Acquisition queue";
            this.colMutexQueue.Name = "colMutexQueue";
            // 
            // tlsMutex
            // 
            this.tlsMutex.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsMutex.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMutexRefresh,
            this.btnMutexForce});
            this.tlsMutex.Location = new System.Drawing.Point(3, 3);
            this.tlsMutex.Name = "tlsMutex";
            this.tlsMutex.Size = new System.Drawing.Size(700, 25);
            this.tlsMutex.TabIndex = 25;
            // 
            // btnMutexRefresh
            // 
            this.btnMutexRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnMutexRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnMutexRefresh.Image")));
            this.btnMutexRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMutexRefresh.Name = "btnMutexRefresh";
            this.btnMutexRefresh.Size = new System.Drawing.Size(66, 22);
            this.btnMutexRefresh.Text = "Refresh";
            this.btnMutexRefresh.Click += new System.EventHandler(this.btnMutexRefresh_Click);
            // 
            // btnMutexForce
            // 
            this.btnMutexForce.Enabled = false;
            this.btnMutexForce.Image = ((System.Drawing.Image)(resources.GetObject("btnMutexForce.Image")));
            this.btnMutexForce.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMutexForce.Name = "btnMutexForce";
            this.btnMutexForce.Size = new System.Drawing.Size(95, 22);
            this.btnMutexForce.Text = "Force release";
            this.btnMutexForce.Click += new System.EventHandler(this.btnMutexForce_Click);
            // 
            // tabImageAura
            // 
            this.tabImageAura.Controls.Add(this.dgvImage);
            this.tabImageAura.Controls.Add(this.tlsImageAura);
            this.tabImageAura.ImageKey = "imageaura.png";
            this.tabImageAura.Location = new System.Drawing.Point(4, 23);
            this.tabImageAura.Name = "tabImageAura";
            this.tabImageAura.Padding = new System.Windows.Forms.Padding(3);
            this.tabImageAura.Size = new System.Drawing.Size(706, 419);
            this.tabImageAura.TabIndex = 4;
            this.tabImageAura.Text = "Image auras";
            this.tabImageAura.UseVisualStyleBackColor = true;
            // 
            // dgvImage
            // 
            this.dgvImage.AllowUserToAddRows = false;
            this.dgvImage.AllowUserToDeleteRows = false;
            this.dgvImage.AllowUserToResizeRows = false;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvImage.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvImage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvImage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colImageName,
            this.colImageCreatedBy});
            this.dgvImage.ContextMenuStrip = this.ctxState;
            this.dgvImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvImage.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvImage.Location = new System.Drawing.Point(3, 28);
            this.dgvImage.Name = "dgvImage";
            this.dgvImage.RowHeadersVisible = false;
            this.dgvImage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvImage.ShowCellErrors = false;
            this.dgvImage.ShowEditingIcon = false;
            this.dgvImage.ShowRowErrors = false;
            this.dgvImage.Size = new System.Drawing.Size(700, 388);
            this.dgvImage.TabIndex = 26;
            this.dgvImage.VirtualMode = true;
            this.dgvImage.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvImage_CellValueNeeded);
            this.dgvImage.SelectionChanged += new System.EventHandler(this.dgvImage_SelectionChanged);
            this.dgvImage.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colImageName
            // 
            this.colImageName.HeaderText = "Name";
            this.colImageName.Name = "colImageName";
            this.colImageName.ReadOnly = true;
            this.colImageName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colImageName.Width = 200;
            // 
            // colImageCreatedBy
            // 
            this.colImageCreatedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colImageCreatedBy.HeaderText = "Created by";
            this.colImageCreatedBy.Name = "colImageCreatedBy";
            this.colImageCreatedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // tlsImageAura
            // 
            this.tlsImageAura.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsImageAura.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImageRefresh,
            this.btnImageForce});
            this.tlsImageAura.Location = new System.Drawing.Point(3, 3);
            this.tlsImageAura.Name = "tlsImageAura";
            this.tlsImageAura.Size = new System.Drawing.Size(700, 25);
            this.tlsImageAura.TabIndex = 27;
            // 
            // btnImageRefresh
            // 
            this.btnImageRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnImageRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnImageRefresh.Image")));
            this.btnImageRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImageRefresh.Name = "btnImageRefresh";
            this.btnImageRefresh.Size = new System.Drawing.Size(66, 22);
            this.btnImageRefresh.Text = "Refresh";
            this.btnImageRefresh.Click += new System.EventHandler(this.btnImageRefresh_Click);
            // 
            // btnImageForce
            // 
            this.btnImageForce.Enabled = false;
            this.btnImageForce.Image = ((System.Drawing.Image)(resources.GetObject("btnImageForce.Image")));
            this.btnImageForce.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImageForce.Name = "btnImageForce";
            this.btnImageForce.Size = new System.Drawing.Size(113, 22);
            this.btnImageForce.Text = "Force deactivate";
            this.btnImageForce.Click += new System.EventHandler(this.btnImageForce_Click);
            // 
            // tabTextAura
            // 
            this.tabTextAura.Controls.Add(this.dgvText);
            this.tabTextAura.Controls.Add(this.tlsTextAura);
            this.tabTextAura.ImageKey = "textaura.png";
            this.tabTextAura.Location = new System.Drawing.Point(4, 23);
            this.tabTextAura.Name = "tabTextAura";
            this.tabTextAura.Padding = new System.Windows.Forms.Padding(3);
            this.tabTextAura.Size = new System.Drawing.Size(706, 419);
            this.tabTextAura.TabIndex = 5;
            this.tabTextAura.Text = "Text auras";
            this.tabTextAura.UseVisualStyleBackColor = true;
            // 
            // dgvText
            // 
            this.dgvText.AllowUserToAddRows = false;
            this.dgvText.AllowUserToDeleteRows = false;
            this.dgvText.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvText.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvText.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvText.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTextName,
            this.colTextCreatedBy});
            this.dgvText.ContextMenuStrip = this.ctxState;
            this.dgvText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvText.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvText.Location = new System.Drawing.Point(3, 28);
            this.dgvText.Name = "dgvText";
            this.dgvText.RowHeadersVisible = false;
            this.dgvText.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvText.ShowCellErrors = false;
            this.dgvText.ShowEditingIcon = false;
            this.dgvText.ShowRowErrors = false;
            this.dgvText.Size = new System.Drawing.Size(700, 388);
            this.dgvText.TabIndex = 28;
            this.dgvText.VirtualMode = true;
            this.dgvText.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvText_CellValueNeeded);
            this.dgvText.SelectionChanged += new System.EventHandler(this.dgvText_SelectionChanged);
            this.dgvText.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colTextName
            // 
            this.colTextName.HeaderText = "Name";
            this.colTextName.Name = "colTextName";
            this.colTextName.ReadOnly = true;
            this.colTextName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colTextName.Width = 200;
            // 
            // colTextCreatedBy
            // 
            this.colTextCreatedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTextCreatedBy.HeaderText = "Created by";
            this.colTextCreatedBy.Name = "colTextCreatedBy";
            this.colTextCreatedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // tlsTextAura
            // 
            this.tlsTextAura.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsTextAura.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTextRefresh,
            this.btnTextForce});
            this.tlsTextAura.Location = new System.Drawing.Point(3, 3);
            this.tlsTextAura.Name = "tlsTextAura";
            this.tlsTextAura.Size = new System.Drawing.Size(700, 25);
            this.tlsTextAura.TabIndex = 27;
            // 
            // btnTextRefresh
            // 
            this.btnTextRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnTextRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnTextRefresh.Image")));
            this.btnTextRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTextRefresh.Name = "btnTextRefresh";
            this.btnTextRefresh.Size = new System.Drawing.Size(66, 22);
            this.btnTextRefresh.Text = "Refresh";
            this.btnTextRefresh.Click += new System.EventHandler(this.btnTextRefresh_Click);
            // 
            // btnTextForce
            // 
            this.btnTextForce.Enabled = false;
            this.btnTextForce.Image = ((System.Drawing.Image)(resources.GetObject("btnTextForce.Image")));
            this.btnTextForce.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTextForce.Name = "btnTextForce";
            this.btnTextForce.Size = new System.Drawing.Size(113, 22);
            this.btnTextForce.Text = "Force deactivate";
            this.btnTextForce.Click += new System.EventHandler(this.btnTextForce_Click);
            // 
            // tabNamedCallbacks
            // 
            this.tabNamedCallbacks.Controls.Add(this.dgvCallback);
            this.tabNamedCallbacks.Controls.Add(this.tlsCallbacks);
            this.tabNamedCallbacks.ImageKey = "callback.png";
            this.tabNamedCallbacks.Location = new System.Drawing.Point(4, 23);
            this.tabNamedCallbacks.Name = "tabNamedCallbacks";
            this.tabNamedCallbacks.Padding = new System.Windows.Forms.Padding(3);
            this.tabNamedCallbacks.Size = new System.Drawing.Size(706, 419);
            this.tabNamedCallbacks.TabIndex = 6;
            this.tabNamedCallbacks.Text = "Named callbacks";
            this.tabNamedCallbacks.UseVisualStyleBackColor = true;
            // 
            // dgvCallback
            // 
            this.dgvCallback.AllowUserToAddRows = false;
            this.dgvCallback.AllowUserToDeleteRows = false;
            this.dgvCallback.AllowUserToResizeRows = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvCallback.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvCallback.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCallback.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCallbackId,
            this.colCallbackName,
            this.colCallbackRegistrant,
            this.colCallbackRegistrationTime,
            this.colCallbackLastInvoked});
            this.dgvCallback.ContextMenuStrip = this.ctxState;
            this.dgvCallback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCallback.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvCallback.Location = new System.Drawing.Point(3, 28);
            this.dgvCallback.Name = "dgvCallback";
            this.dgvCallback.RowHeadersVisible = false;
            this.dgvCallback.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCallback.ShowCellErrors = false;
            this.dgvCallback.ShowEditingIcon = false;
            this.dgvCallback.ShowRowErrors = false;
            this.dgvCallback.Size = new System.Drawing.Size(700, 388);
            this.dgvCallback.TabIndex = 29;
            this.dgvCallback.VirtualMode = true;
            this.dgvCallback.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvCallback_CellValueNeeded);
            this.dgvCallback.SelectionChanged += new System.EventHandler(this.dgvCallback_SelectionChanged);
            this.dgvCallback.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommon_ColumnHeaderMouseClick);
            // 
            // colCallbackId
            // 
            this.colCallbackId.HeaderText = "Id";
            this.colCallbackId.Name = "colCallbackId";
            this.colCallbackId.ReadOnly = true;
            this.colCallbackId.Width = 50;
            // 
            // colCallbackName
            // 
            this.colCallbackName.HeaderText = "Name";
            this.colCallbackName.Name = "colCallbackName";
            this.colCallbackName.ReadOnly = true;
            this.colCallbackName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.colCallbackName.Width = 240;
            // 
            // colCallbackRegistrant
            // 
            this.colCallbackRegistrant.HeaderText = "Registrant";
            this.colCallbackRegistrant.Name = "colCallbackRegistrant";
            this.colCallbackRegistrant.ReadOnly = true;
            this.colCallbackRegistrant.Width = 240;
            // 
            // colCallbackRegistrationTime
            // 
            this.colCallbackRegistrationTime.HeaderText = "Registration Time";
            this.colCallbackRegistrationTime.Name = "colCallbackRegistrationTime";
            this.colCallbackRegistrationTime.ReadOnly = true;
            this.colCallbackRegistrationTime.Width = 120;
            // 
            // colCallbackLastInvoked
            // 
            this.colCallbackLastInvoked.HeaderText = "Last Invoked";
            this.colCallbackLastInvoked.Name = "colCallbackLastInvoked";
            this.colCallbackLastInvoked.ReadOnly = true;
            this.colCallbackLastInvoked.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            // 
            // tlsCallbacks
            // 
            this.tlsCallbacks.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsCallbacks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCallbackRefresh});
            this.tlsCallbacks.Location = new System.Drawing.Point(3, 3);
            this.tlsCallbacks.Name = "tlsCallbacks";
            this.tlsCallbacks.Size = new System.Drawing.Size(700, 25);
            this.tlsCallbacks.TabIndex = 28;
            // 
            // btnCallbackRefresh
            // 
            this.btnCallbackRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnCallbackRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnCallbackRefresh.Image")));
            this.btnCallbackRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCallbackRefresh.Name = "btnCallbackRefresh";
            this.btnCallbackRefresh.Size = new System.Drawing.Size(66, 22);
            this.btnCallbackRefresh.Text = "Refresh";
            this.btnCallbackRefresh.Click += new System.EventHandler(this.btnCallbackRefresh_Click);
            // 
            // StateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1101, 767);
            this.Controls.Add(this.tbcMain);
            this.Controls.Add(this.pnlButtonsSpacing);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "StateForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Triggernometry state";
            this.tlsScalar.ResumeLayout(false);
            this.tlsScalar.PerformLayout();
            this.ctxState.ResumeLayout(false);
            this.pnlScalarSession.ResumeLayout(false);
            this.pnlScalarSession.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScalarVariables)).EndInit();
            this.tbcMain.ResumeLayout(false);
            this.tabScalar.ResumeLayout(false);
            this.tbcScalar.ResumeLayout(false);
            this.tabScalarSession.ResumeLayout(false);
            this.tabScalarPersistent.ResumeLayout(false);
            this.pnlScalarPersistent.ResumeLayout(false);
            this.pnlScalarPersistent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeScalarVariables)).EndInit();
            this.tlsPeScalar.ResumeLayout(false);
            this.tlsPeScalar.PerformLayout();
            this.tabList.ResumeLayout(false);
            this.tbcList.ResumeLayout(false);
            this.tabListSession.ResumeLayout(false);
            this.pnlListSession.ResumeLayout(false);
            this.pnlListSession.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListVariables)).EndInit();
            this.tlsList.ResumeLayout(false);
            this.tlsList.PerformLayout();
            this.tabListPersistent.ResumeLayout(false);
            this.pnlListPersistent.ResumeLayout(false);
            this.pnlListPersistent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeListVariables)).EndInit();
            this.tlsPeList.ResumeLayout(false);
            this.tlsPeList.PerformLayout();
            this.tabTable.ResumeLayout(false);
            this.tbcTable.ResumeLayout(false);
            this.tabTableSession.ResumeLayout(false);
            this.pnlTableSession.ResumeLayout(false);
            this.pnlTableSession.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableVariables)).EndInit();
            this.tlsTable.ResumeLayout(false);
            this.tlsTable.PerformLayout();
            this.tabTablePersistent.ResumeLayout(false);
            this.pnlTablePersistent.ResumeLayout(false);
            this.pnlTablePersistent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeTableVariables)).EndInit();
            this.tlsPeTable.ResumeLayout(false);
            this.tlsPeTable.PerformLayout();
            this.tabDict.ResumeLayout(false);
            this.tbcDict.ResumeLayout(false);
            this.tabDictSession.ResumeLayout(false);
            this.panelDict.ResumeLayout(false);
            this.panelDict.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDictVariables)).EndInit();
            this.tlsDict.ResumeLayout(false);
            this.tlsDict.PerformLayout();
            this.tabDictPersistent.ResumeLayout(false);
            this.panelPeDict.ResumeLayout(false);
            this.panelPeDict.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeDictVariables)).EndInit();
            this.tlsPeDict.ResumeLayout(false);
            this.tlsPeDict.PerformLayout();
            this.tabRegisteredVariables.ResumeLayout(false);
            this.tabRegisteredVariables.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegisteredVariables)).EndInit();
            this.tlsRegistered.ResumeLayout(false);
            this.tlsRegistered.PerformLayout();
            this.tabMutexes.ResumeLayout(false);
            this.tabMutexes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMutexes)).EndInit();
            this.tlsMutex.ResumeLayout(false);
            this.tlsMutex.PerformLayout();
            this.tabImageAura.ResumeLayout(false);
            this.tabImageAura.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImage)).EndInit();
            this.tlsImageAura.ResumeLayout(false);
            this.tlsImageAura.PerformLayout();
            this.tabTextAura.ResumeLayout(false);
            this.tabTextAura.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvText)).EndInit();
            this.tlsTextAura.ResumeLayout(false);
            this.tlsTextAura.PerformLayout();
            this.tabNamedCallbacks.ResumeLayout(false);
            this.tabNamedCallbacks.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCallback)).EndInit();
            this.tlsCallbacks.ResumeLayout(false);
            this.tlsCallbacks.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlButtonsSpacing;
        internal System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolStrip tlsScalar;
        private CustomControls.DataGridViewEx dgvScalarVariables;
        private System.Windows.Forms.ContextMenuStrip ctxState;
        private System.Windows.Forms.ToolStripMenuItem ctxRefresh;
        private System.Windows.Forms.ToolStripMenuItem ctxLocateSource;
        private System.Windows.Forms.Panel pnlScalarSession;
        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tabScalar;
        private System.Windows.Forms.TabPage tabList;
        private System.Windows.Forms.Panel pnlListSession;
        private CustomControls.DataGridViewEx dgvListVariables;
        private System.Windows.Forms.DataGridViewTextBoxColumn colListName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colListSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colListValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colListLastChanged;
        private System.Windows.Forms.DataGridViewTextBoxColumn colListChangedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScalarName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScalarValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScalarLastChanged;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScalarChangedBy;
        private System.Windows.Forms.TabPage tabTable;
        private CustomControls.DataGridViewEx dgvTableVariables;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTableWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTableHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTableLastChanged;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTableChangedBy;
        private System.Windows.Forms.ToolStripButton btnScalarAdd;
        private System.Windows.Forms.ToolStripButton btnScalarEdit;
        private System.Windows.Forms.ToolStripSeparator sepScalarRemove;
        private System.Windows.Forms.ToolStripButton btnScalarRemove;
        private System.Windows.Forms.ToolStripSplitButton btnScalarRefresh;
        private System.Windows.Forms.ToolStripMenuItem btnScalarRemoveAll;
        private System.Windows.Forms.ToolStrip tlsList;
        private System.Windows.Forms.ToolStripButton btnListAdd;
        private System.Windows.Forms.ToolStripButton btnListEdit;
        private System.Windows.Forms.ToolStripSeparator sepListRemove;
        private System.Windows.Forms.ToolStripButton btnListRemove;
        private System.Windows.Forms.ToolStripSplitButton btnListRefresh;
        private System.Windows.Forms.ToolStripMenuItem btnListRemoveAll;
        private System.Windows.Forms.ToolStrip tlsTable;
        private System.Windows.Forms.ToolStripButton btnTableAdd;
        private System.Windows.Forms.ToolStripButton btnTableEdit;
        private System.Windows.Forms.ToolStripSeparator sepTableRemove;
        private System.Windows.Forms.ToolStripButton btnTableRemove;
        private System.Windows.Forms.ToolStripSplitButton btnTableRefresh;
        private System.Windows.Forms.ToolStripMenuItem btnTableRemoveAll;
        private System.Windows.Forms.TabPage tabRegisteredVariables;
        private CustomControls.DataGridViewEx dgvRegisteredVariables;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegisteredName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegisteredValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegisteredLastChanged;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegisteredChangedBy;
        private System.Windows.Forms.ToolStrip tlsRegistered;
        private System.Windows.Forms.ToolStripButton btnRegisteredAdd;
        private System.Windows.Forms.ToolStripButton btnRegisteredEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorRegistered;
        private System.Windows.Forms.ToolStripButton btnRegisteredRemove;
        private System.Windows.Forms.ToolStripSplitButton btnRegisteredRefresh;
        private System.Windows.Forms.ToolStripMenuItem btnRegisteredRemoveAll;
        private System.Windows.Forms.TabPage tabMutexes;
        private CustomControls.DataGridViewEx dgvMutexes;
        private System.Windows.Forms.ToolStrip tlsMutex;
        private System.Windows.Forms.ToolStripButton btnMutexRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMutexName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMutexOwner;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMutexRefCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMutexQueue;
        private System.Windows.Forms.ToolStripButton btnMutexForce;
        private System.Windows.Forms.TabPage tabImageAura;
        private System.Windows.Forms.TabPage tabTextAura;
        private CustomControls.DataGridViewEx dgvImage;
        private System.Windows.Forms.ToolStrip tlsImageAura;
        private System.Windows.Forms.ToolStripButton btnImageRefresh;
        private System.Windows.Forms.ToolStripButton btnImageForce;
        private System.Windows.Forms.ToolStrip tlsTextAura;
        private System.Windows.Forms.ToolStripButton btnTextRefresh;
        private System.Windows.Forms.ToolStripButton btnTextForce;
        private CustomControls.DataGridViewEx dgvText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImageName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImageCreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTextName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTextCreatedBy;
        private System.Windows.Forms.TabPage tabNamedCallbacks;
        private CustomControls.DataGridViewEx dgvCallback;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCallbackId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCallbackName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCallbackRegistrant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCallbackRegistrationTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCallbackLastInvoked;
        private System.Windows.Forms.ToolStrip tlsCallbacks;
        private System.Windows.Forms.ToolStripButton btnCallbackRefresh;
        private System.Windows.Forms.ImageList imlStateTabs;
        private System.Windows.Forms.TabControl tbcScalar;
        private System.Windows.Forms.TabPage tabScalarSession;
        private System.Windows.Forms.TabPage tabScalarPersistent;
        private System.Windows.Forms.TabControl tbcList;
        private System.Windows.Forms.TabPage tabListSession;
        private System.Windows.Forms.TabPage tabListPersistent;
        private System.Windows.Forms.TabControl tbcTable;
        private System.Windows.Forms.TabPage tabTableSession;
        private System.Windows.Forms.TabPage tabTablePersistent;
        private System.Windows.Forms.Panel pnlTableSession;
        private System.Windows.Forms.Panel pnlScalarPersistent;
        private CustomControls.DataGridViewEx dgvPeScalarVariables;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeScalarName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeScalarValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeScalarLastChanged;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeScalarChangedBy;
        private System.Windows.Forms.ToolStrip tlsPeScalar;
        private System.Windows.Forms.ToolStripButton btnPeScalarAdd;
        private System.Windows.Forms.ToolStripButton btnPeScalarEdit;
        private System.Windows.Forms.ToolStripSeparator sepPeScalarRemove;
        private System.Windows.Forms.ToolStripButton btnPeScalarRemove;
        private System.Windows.Forms.ToolStripSplitButton btnPeScalarRefresh;
        private System.Windows.Forms.ToolStripMenuItem btnPeScalarRemoveAll;
        private System.Windows.Forms.Panel pnlListPersistent;
        private CustomControls.DataGridViewEx dgvPeListVariables;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeListName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeListSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeListValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeListLastChanged;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeListChangedBy;
        private System.Windows.Forms.ToolStrip tlsPeList;
        private System.Windows.Forms.ToolStripButton btnPeListAdd;
        private System.Windows.Forms.ToolStripButton btnPeListEdit;
        private System.Windows.Forms.ToolStripSeparator sepPeListRemove;
        private System.Windows.Forms.ToolStripButton btnPeListRemove;
        private System.Windows.Forms.ToolStripSplitButton btnPeListRefresh;
        private System.Windows.Forms.ToolStripMenuItem btnPeListRemoveAll;
        private System.Windows.Forms.Panel pnlTablePersistent;
        private CustomControls.DataGridViewEx dgvPeTableVariables;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeTableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeTableWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeTableHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeTableLastChanged;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeTableChangedBy;
        private System.Windows.Forms.ToolStrip tlsPeTable;
        private System.Windows.Forms.ToolStripButton btnPeTableAdd;
        private System.Windows.Forms.ToolStripButton btnPeTableEdit;
        private System.Windows.Forms.ToolStripSeparator sepPeTableRemove;
        private System.Windows.Forms.ToolStripButton btnPeTableRemove;
        private System.Windows.Forms.ToolStripSplitButton btnPeTableRefresh;
        private System.Windows.Forms.ToolStripMenuItem btnPeTableRemoveAll;
        private System.Windows.Forms.TabPage tabDict;
        private System.Windows.Forms.TabControl tbcDict;
        private System.Windows.Forms.TabPage tabDictSession;
        private System.Windows.Forms.Panel panelDict;
        private CustomControls.DataGridViewEx dgvDictVariables;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDictName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDictSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDictValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDictLastChanged;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDictChangedBy;
        private System.Windows.Forms.ToolStrip tlsDict;
        private System.Windows.Forms.ToolStripButton btnDictAdd;
        private System.Windows.Forms.ToolStripButton btnDictEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDict;
        private System.Windows.Forms.ToolStripButton btnDictRemove;
        private System.Windows.Forms.ToolStripSplitButton btnDictRefresh;
        private System.Windows.Forms.ToolStripMenuItem btnDictRemoveAll;
        private System.Windows.Forms.TabPage tabDictPersistent;
        private System.Windows.Forms.Panel panelPeDict;
        private CustomControls.DataGridViewEx dgvPeDictVariables;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeDictName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeDictSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeDictValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeDictLastChanged;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeDictChangedBy;
        private System.Windows.Forms.ToolStrip tlsPeDict;
        private System.Windows.Forms.ToolStripButton btnPeDictAdd;
        private System.Windows.Forms.ToolStripButton btnPeDictEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorPeDict;
        private System.Windows.Forms.ToolStripButton btnPeDictRemove;
        private System.Windows.Forms.ToolStripSplitButton btnPeDictRefresh;
        private System.Windows.Forms.ToolStripMenuItem btnPeDictRemoveAll;
    }
}
