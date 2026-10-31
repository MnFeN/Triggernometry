namespace Triggernometry.UI.Forms
{
    partial class ConfigurationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolTipSettings = new System.Windows.Forms.ToolTip(this.components);
            this.pnlButtonsSpacing = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.grpVolAdjustment = new System.Windows.Forms.GroupBox();
            this.tableAudioVolume = new System.Windows.Forms.TableLayoutPanel();
            this.lblTtsVolP = new System.Windows.Forms.Label();
            this.lblSoundVolP = new System.Windows.Forms.Label();
            this.trbTtsVolume = new System.Windows.Forms.TrackBar();
            this.trbSoundVolume = new System.Windows.Forms.TrackBar();
            this.lblSoundVolume = new System.Windows.Forms.Label();
            this.lblTtsVolume = new System.Windows.Forms.Label();
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.tableLoggingGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.chkLogVariableExpansions = new System.Windows.Forms.CheckBox();
            this.chkLogNormalEvents = new System.Windows.Forms.CheckBox();
            this.lblLoggingLevel = new System.Windows.Forms.Label();
            this.cbxLoggingLevel = new System.Windows.Forms.ComboBox();
            this.trvTrigger = new System.Windows.Forms.TreeView();
            this.ctxStartupTrigger = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxClearSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.grpActHooks = new System.Windows.Forms.GroupBox();
            this.tableAudioActHooks = new System.Windows.Forms.TableLayoutPanel();
            this.chkActSoundFiles = new System.Windows.Forms.CheckBox();
            this.chkActTts = new System.Windows.Forms.CheckBox();
            this.pnlAudioSpacing = new System.Windows.Forms.Panel();
            this.grpFutureProofing = new System.Windows.Forms.GroupBox();
            this.tableEventSeparator = new System.Windows.Forms.TableLayoutPanel();
            this.txtSeparator = new System.Windows.Forms.TextBox();
            this.lblSeparator = new System.Windows.Forms.Label();
            this.tbcMain = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.grpStartupTrigger = new System.Windows.Forms.GroupBox();
            this.tlsDirectPaste = new System.Windows.Forms.ToolStrip();
            this.btnClearSelection = new System.Windows.Forms.ToolStripButton();
            this.lblFolderReminder = new System.Windows.Forms.ToolStripLabel();
            this.pnlStartupTriggerSpacing = new System.Windows.Forms.Panel();
            this.grpStartup = new System.Windows.Forms.GroupBox();
            this.tableStartup = new System.Windows.Forms.TableLayoutPanel();
            this.cbxUpdateMethod = new System.Windows.Forms.ComboBox();
            this.lblUpdateMethod = new System.Windows.Forms.Label();
            this.chkWarnAdmin = new System.Windows.Forms.CheckBox();
            this.chkUpdates = new System.Windows.Forms.CheckBox();
            this.chkWelcome = new System.Windows.Forms.CheckBox();
            this.pnlStartupSpacing = new System.Windows.Forms.Panel();
            this.tabAudio = new System.Windows.Forms.TabPage();
            this.tabShortCuts = new System.Windows.Forms.TabPage();
            this.grpShortCutExpression = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelShortCutExpression = new System.Windows.Forms.TableLayoutPanel();
            this.chkShortcutEnableTemplates = new System.Windows.Forms.CheckBox();
            this.chkShortcutUseAbbrevInTemplates = new System.Windows.Forms.CheckBox();
            this.chkShortcutWrapTextWhenSelected = new System.Windows.Forms.CheckBox();
            this.tabCaching = new System.Windows.Forms.TabPage();
            this.pnlCaching = new System.Windows.Forms.Panel();
            this.grpCacheFile = new System.Windows.Forms.GroupBox();
            this.tableCacheFile = new System.Windows.Forms.TableLayoutPanel();
            this.btnCacheFileBrowse = new System.Windows.Forms.Button();
            this.btnCacheFileClear = new System.Windows.Forms.Button();
            this.txtCacheFileSize = new System.Windows.Forms.TextBox();
            this.lblCacheFileSize = new System.Windows.Forms.Label();
            this.lblCacheFileCount = new System.Windows.Forms.Label();
            this.lblCacheFileExpiry = new System.Windows.Forms.Label();
            this.nudCacheFileExpiry = new System.Windows.Forms.NumericUpDown();
            this.txtCacheFileCount = new System.Windows.Forms.TextBox();
            this.pnlCacheFileSpacing = new System.Windows.Forms.Panel();
            this.grpCacheRepo = new System.Windows.Forms.GroupBox();
            this.tableCacheRepo = new System.Windows.Forms.TableLayoutPanel();
            this.btnCacheRepoBrowse = new System.Windows.Forms.Button();
            this.btnCacheRepoClear = new System.Windows.Forms.Button();
            this.txtCacheRepoSize = new System.Windows.Forms.TextBox();
            this.lblCacheRepoSize = new System.Windows.Forms.Label();
            this.lblCacheRepoCount = new System.Windows.Forms.Label();
            this.lblCacheRepoExpiry = new System.Windows.Forms.Label();
            this.nudCacheRepoExpiry = new System.Windows.Forms.NumericUpDown();
            this.txtCacheRepoCount = new System.Windows.Forms.TextBox();
            this.pnlCacheRepoSpacing = new System.Windows.Forms.Panel();
            this.grpCacheJSON = new System.Windows.Forms.GroupBox();
            this.tableCacheJson = new System.Windows.Forms.TableLayoutPanel();
            this.nudCacheJsonExpiry = new System.Windows.Forms.NumericUpDown();
            this.btnCacheJsonBrowse = new System.Windows.Forms.Button();
            this.btnCacheJsonClear = new System.Windows.Forms.Button();
            this.txtCacheJsonSize = new System.Windows.Forms.TextBox();
            this.lblCacheJsonSize = new System.Windows.Forms.Label();
            this.lblCacheJsonCount = new System.Windows.Forms.Label();
            this.lblCacheJsonExpiry = new System.Windows.Forms.Label();
            this.txtCacheJsonCount = new System.Windows.Forms.TextBox();
            this.pnlCacheJsonSpacing = new System.Windows.Forms.Panel();
            this.grpCacheSound = new System.Windows.Forms.GroupBox();
            this.tableCacheSound = new System.Windows.Forms.TableLayoutPanel();
            this.btnCacheSoundBrowse = new System.Windows.Forms.Button();
            this.btnCacheSoundClear = new System.Windows.Forms.Button();
            this.txtCacheSoundSize = new System.Windows.Forms.TextBox();
            this.lblCacheSoundSize = new System.Windows.Forms.Label();
            this.lblCacheSoundCount = new System.Windows.Forms.Label();
            this.lblCacheSoundExpiry = new System.Windows.Forms.Label();
            this.nudCacheSoundExpiry = new System.Windows.Forms.NumericUpDown();
            this.txtCacheSoundCount = new System.Windows.Forms.TextBox();
            this.pnlCacheSoundSpacing = new System.Windows.Forms.Panel();
            this.grpCacheImage = new System.Windows.Forms.GroupBox();
            this.tableCacheImage = new System.Windows.Forms.TableLayoutPanel();
            this.btnCacheImageBrowse = new System.Windows.Forms.Button();
            this.txtCacheImageSize = new System.Windows.Forms.TextBox();
            this.lblCacheImageSize = new System.Windows.Forms.Label();
            this.lblCacheImageCount = new System.Windows.Forms.Label();
            this.lblCacheImageExpiry = new System.Windows.Forms.Label();
            this.nudCacheImageExpiry = new System.Windows.Forms.NumericUpDown();
            this.txtCacheImageCount = new System.Windows.Forms.TextBox();
            this.btnCacheImageClear = new System.Windows.Forms.Button();
            this.tabEndpoint = new System.Windows.Forms.TabPage();
            this.grpEndpointHistory = new System.Windows.Forms.GroupBox();
            this.pnlEndpointHistory = new System.Windows.Forms.Panel();
            this.dgvEndpointHistory = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colEndpointHistoryTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndpointHistoryData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsEndpointHistory = new System.Windows.Forms.ToolStrip();
            this.btnEndpointHistUpdate = new System.Windows.Forms.ToolStripButton();
            this.tlsEndpointHistoryRecv = new System.Windows.Forms.ToolStripLabel();
            this.tlsEndpointHistoryCount = new System.Windows.Forms.ToolStripLabel();
            this.pnlEndpointHistorySpacing = new System.Windows.Forms.Panel();
            this.grpEndpointSettings = new System.Windows.Forms.GroupBox();
            this.tableEndpointSettings = new System.Windows.Forms.TableLayoutPanel();
            this.chkEndpointStartup = new System.Windows.Forms.CheckBox();
            this.chkEndpointLog = new System.Windows.Forms.CheckBox();
            this.txtEndpoint = new System.Windows.Forms.TextBox();
            this.lblEndpoint = new System.Windows.Forms.Label();
            this.pnlEndpointStatusSpacing = new System.Windows.Forms.Panel();
            this.grpEndpointState = new System.Windows.Forms.GroupBox();
            this.tableEndpointStatus = new System.Windows.Forms.TableLayoutPanel();
            this.btnEndpointStart = new System.Windows.Forms.Button();
            this.btnEndpointStop = new System.Windows.Forms.Button();
            this.txtEndpointStatus = new System.Windows.Forms.TextBox();
            this.tabFFXIV = new System.Windows.Forms.TabPage();
            this.pnlFfxivLoggingSpacing = new System.Windows.Forms.Panel();
            this.grpFfxivEventLogging = new System.Windows.Forms.GroupBox();
            this.tableFfxivLogging = new System.Windows.Forms.TableLayoutPanel();
            this.chkFfxivLogNetwork = new System.Windows.Forms.CheckBox();
            this.tabSubstitutions = new System.Windows.Forms.TabPage();
            this.dgvSubstitutions = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colSubSearchFor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubReplaceWith = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubScope = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsSubstitutions = new System.Windows.Forms.ToolStrip();
            this.btnSubAdd = new System.Windows.Forms.ToolStripButton();
            this.btnSubEdit = new System.Windows.Forms.ToolStripButton();
            this.btnSubRemove = new System.Windows.Forms.ToolStripButton();
            this.tabConsts = new System.Windows.Forms.TabPage();
            this.pnlConstants = new System.Windows.Forms.Panel();
            this.dgvConstVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colScalarName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScalarValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsScalar = new System.Windows.Forms.ToolStrip();
            this.btnConstAdd = new System.Windows.Forms.ToolStripButton();
            this.btnConstEdit = new System.Windows.Forms.ToolStripButton();
            this.sepConstantRemove = new System.Windows.Forms.ToolStripSeparator();
            this.btnConstRemove = new System.Windows.Forms.ToolStripButton();
            this.tabSecurity = new System.Windows.Forms.TabPage();
            this.grpScriptingAccess = new System.Windows.Forms.GroupBox();
            this.dgvAdditionalFeatures = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colFeatureName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFeatureAllowLocal = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFeatureAllowRemote = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFeatureAllowAdmin = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnlSecurityFeaturesSpacing = new System.Windows.Forms.Panel();
            this.dgvApiAccess = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colApiNamespace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colApiAllowLocal = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colApiAllowRemote = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colApiAllowAdmin = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnlSecurityUnlockSpacing = new System.Windows.Forms.Panel();
            this.btnUnlockSecurity = new System.Windows.Forms.Button();
            this.tabMisc = new System.Windows.Forms.TabPage();
            this.grpAuraManagement = new System.Windows.Forms.GroupBox();
            this.tableAuraManagement = new System.Windows.Forms.TableLayoutPanel();
            this.txtMonitorWindow = new System.Windows.Forms.TextBox();
            this.lblMonitorWindow = new System.Windows.Forms.Label();
            this.cbxEnableHwAccel = new System.Windows.Forms.CheckBox();
            this.pnlAuraManagementSpacing = new System.Windows.Forms.Panel();
            this.grpUserInterface = new System.Windows.Forms.GroupBox();
            this.tableUiOptions = new System.Windows.Forms.TableLayoutPanel();
            this.cbxAutoComplete = new System.Windows.Forms.CheckBox();
            this.cbxActionAsync = new System.Windows.Forms.CheckBox();
            this.chkClipboard = new System.Windows.Forms.CheckBox();
            this.lblAutosaveInterval = new System.Windows.Forms.Label();
            this.cbxAutosaveConfig = new System.Windows.Forms.CheckBox();
            this.cbxDevMode = new System.Windows.Forms.CheckBox();
            this.cbxTestIgnoreConditions = new System.Windows.Forms.CheckBox();
            this.nudAutosaveMinutes = new System.Windows.Forms.NumericUpDown();
            this.pnlEventSeparatorSpacing = new System.Windows.Forms.Panel();
            this.pnlDefaultsSpacing = new System.Windows.Forms.Panel();
            this.grpDefaultSettings = new System.Windows.Forms.GroupBox();
            this.tableTriggerTemplate = new System.Windows.Forms.TableLayoutPanel();
            this.cbxTriggerTemplate = new System.Windows.Forms.CheckBox();
            this.btnTriggerTemplate = new System.Windows.Forms.Button();
            this.tableDefaultsReserved = new System.Windows.Forms.TableLayoutPanel();
            this.dlgUiFont = new System.Windows.Forms.FontDialog();
            this.pnlButtons.SuspendLayout();
            this.grpVolAdjustment.SuspendLayout();
            this.tableAudioVolume.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbTtsVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbSoundVolume)).BeginInit();
            this.grpGeneral.SuspendLayout();
            this.tableLoggingGeneral.SuspendLayout();
            this.ctxStartupTrigger.SuspendLayout();
            this.grpActHooks.SuspendLayout();
            this.tableAudioActHooks.SuspendLayout();
            this.grpFutureProofing.SuspendLayout();
            this.tableEventSeparator.SuspendLayout();
            this.tbcMain.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.grpStartupTrigger.SuspendLayout();
            this.tlsDirectPaste.SuspendLayout();
            this.grpStartup.SuspendLayout();
            this.tableStartup.SuspendLayout();
            this.tabAudio.SuspendLayout();
            this.tabShortCuts.SuspendLayout();
            this.grpShortCutExpression.SuspendLayout();
            this.tableLayoutPanelShortCutExpression.SuspendLayout();
            this.tabCaching.SuspendLayout();
            this.pnlCaching.SuspendLayout();
            this.grpCacheFile.SuspendLayout();
            this.tableCacheFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheFileExpiry)).BeginInit();
            this.grpCacheRepo.SuspendLayout();
            this.tableCacheRepo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheRepoExpiry)).BeginInit();
            this.grpCacheJSON.SuspendLayout();
            this.tableCacheJson.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheJsonExpiry)).BeginInit();
            this.grpCacheSound.SuspendLayout();
            this.tableCacheSound.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheSoundExpiry)).BeginInit();
            this.grpCacheImage.SuspendLayout();
            this.tableCacheImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheImageExpiry)).BeginInit();
            this.tabEndpoint.SuspendLayout();
            this.grpEndpointHistory.SuspendLayout();
            this.pnlEndpointHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEndpointHistory)).BeginInit();
            this.tlsEndpointHistory.SuspendLayout();
            this.grpEndpointSettings.SuspendLayout();
            this.tableEndpointSettings.SuspendLayout();
            this.grpEndpointState.SuspendLayout();
            this.tableEndpointStatus.SuspendLayout();
            this.tabFFXIV.SuspendLayout();
            this.grpFfxivEventLogging.SuspendLayout();
            this.tableFfxivLogging.SuspendLayout();
            this.tabSubstitutions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubstitutions)).BeginInit();
            this.tlsSubstitutions.SuspendLayout();
            this.tabConsts.SuspendLayout();
            this.pnlConstants.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConstVariables)).BeginInit();
            this.tlsScalar.SuspendLayout();
            this.tabSecurity.SuspendLayout();
            this.grpScriptingAccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdditionalFeatures)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApiAccess)).BeginInit();
            this.tabMisc.SuspendLayout();
            this.grpAuraManagement.SuspendLayout();
            this.tableAuraManagement.SuspendLayout();
            this.grpUserInterface.SuspendLayout();
            this.tableUiOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutosaveMinutes)).BeginInit();
            this.grpDefaultSettings.SuspendLayout();
            this.tableTriggerTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTipSettings
            // 
            this.toolTipSettings.AutoPopDelay = 60000;
            this.toolTipSettings.InitialDelay = 500;
            this.toolTipSettings.ReshowDelay = 500;
            this.toolTipSettings.ShowAlways = true;
            // 
            // pnlButtonsSpacing
            // 
            this.pnlButtonsSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonsSpacing.Location = new System.Drawing.Point(10, 606);
            this.pnlButtonsSpacing.Name = "pnlButtonsSpacing";
            this.pnlButtonsSpacing.Size = new System.Drawing.Size(764, 10);
            this.pnlButtonsSpacing.TabIndex = 2;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOk);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(10, 616);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(764, 35);
            this.pnlButtons.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(614, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 35);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOk.Location = new System.Drawing.Point(0, 0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 35);
            this.btnOk.TabIndex = 1;
            this.btnOk.TabStop = false;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // grpVolAdjustment
            // 
            this.grpVolAdjustment.AutoSize = true;
            this.grpVolAdjustment.Controls.Add(this.tableAudioVolume);
            this.grpVolAdjustment.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpVolAdjustment.Location = new System.Drawing.Point(7, 7);
            this.grpVolAdjustment.Name = "grpVolAdjustment";
            this.grpVolAdjustment.Padding = new System.Windows.Forms.Padding(10);
            this.grpVolAdjustment.Size = new System.Drawing.Size(742, 89);
            this.grpVolAdjustment.TabIndex = 2;
            this.grpVolAdjustment.TabStop = false;
            this.grpVolAdjustment.Text = " Global volume adjustment ";
            // 
            // tableAudioVolume
            // 
            this.tableAudioVolume.AutoSize = true;
            this.tableAudioVolume.ColumnCount = 3;
            this.tableAudioVolume.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAudioVolume.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAudioVolume.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableAudioVolume.Controls.Add(this.lblTtsVolP, 2, 1);
            this.tableAudioVolume.Controls.Add(this.lblSoundVolP, 2, 0);
            this.tableAudioVolume.Controls.Add(this.trbTtsVolume, 1, 1);
            this.tableAudioVolume.Controls.Add(this.trbSoundVolume, 1, 0);
            this.tableAudioVolume.Controls.Add(this.lblSoundVolume, 0, 0);
            this.tableAudioVolume.Controls.Add(this.lblTtsVolume, 0, 1);
            this.tableAudioVolume.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableAudioVolume.Location = new System.Drawing.Point(10, 23);
            this.tableAudioVolume.Name = "tableAudioVolume";
            this.tableAudioVolume.RowCount = 2;
            this.tableAudioVolume.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioVolume.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioVolume.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableAudioVolume.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableAudioVolume.Size = new System.Drawing.Size(722, 56);
            this.tableAudioVolume.TabIndex = 0;
            // 
            // lblTtsVolP
            // 
            this.lblTtsVolP.AutoEllipsis = true;
            this.lblTtsVolP.AutoSize = true;
            this.lblTtsVolP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTtsVolP.Location = new System.Drawing.Point(675, 28);
            this.lblTtsVolP.Name = "lblTtsVolP";
            this.lblTtsVolP.Size = new System.Drawing.Size(44, 28);
            this.lblTtsVolP.TabIndex = 0;
            this.lblTtsVolP.Text = "100 %";
            this.lblTtsVolP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSoundVolP
            // 
            this.lblSoundVolP.AutoEllipsis = true;
            this.lblSoundVolP.AutoSize = true;
            this.lblSoundVolP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoundVolP.Location = new System.Drawing.Point(675, 0);
            this.lblSoundVolP.Name = "lblSoundVolP";
            this.lblSoundVolP.Size = new System.Drawing.Size(44, 28);
            this.lblSoundVolP.TabIndex = 1;
            this.lblSoundVolP.Text = "100 %";
            this.lblSoundVolP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trbTtsVolume
            // 
            this.trbTtsVolume.AutoSize = false;
            this.trbTtsVolume.Dock = System.Windows.Forms.DockStyle.Top;
            this.trbTtsVolume.Location = new System.Drawing.Point(109, 31);
            this.trbTtsVolume.Maximum = 100;
            this.trbTtsVolume.Name = "trbTtsVolume";
            this.trbTtsVolume.Size = new System.Drawing.Size(560, 22);
            this.trbTtsVolume.TabIndex = 2;
            this.trbTtsVolume.TabStop = false;
            this.trbTtsVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trbTtsVolume.Value = 100;
            this.trbTtsVolume.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            this.trbTtsVolume.ValueChanged += new System.EventHandler(this.trackBar2_ValueChanged);
            // 
            // trbSoundVolume
            // 
            this.trbSoundVolume.AutoSize = false;
            this.trbSoundVolume.Dock = System.Windows.Forms.DockStyle.Top;
            this.trbSoundVolume.Location = new System.Drawing.Point(109, 3);
            this.trbSoundVolume.Maximum = 100;
            this.trbSoundVolume.Name = "trbSoundVolume";
            this.trbSoundVolume.Size = new System.Drawing.Size(560, 22);
            this.trbSoundVolume.TabIndex = 3;
            this.trbSoundVolume.TabStop = false;
            this.trbSoundVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trbSoundVolume.Value = 100;
            this.trbSoundVolume.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            this.trbSoundVolume.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // lblSoundVolume
            // 
            this.lblSoundVolume.AutoEllipsis = true;
            this.lblSoundVolume.AutoSize = true;
            this.lblSoundVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoundVolume.Location = new System.Drawing.Point(3, 0);
            this.lblSoundVolume.Name = "lblSoundVolume";
            this.lblSoundVolume.Size = new System.Drawing.Size(100, 28);
            this.lblSoundVolume.TabIndex = 4;
            this.lblSoundVolume.Text = "Sound file playback";
            this.lblSoundVolume.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTtsVolume
            // 
            this.lblTtsVolume.AutoEllipsis = true;
            this.lblTtsVolume.AutoSize = true;
            this.lblTtsVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTtsVolume.Location = new System.Drawing.Point(3, 28);
            this.lblTtsVolume.Name = "lblTtsVolume";
            this.lblTtsVolume.Size = new System.Drawing.Size(100, 28);
            this.lblTtsVolume.TabIndex = 5;
            this.lblTtsVolume.Text = "Text-to-speech";
            this.lblTtsVolume.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpGeneral
            // 
            this.grpGeneral.AutoSize = true;
            this.grpGeneral.Controls.Add(this.tableLoggingGeneral);
            this.grpGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpGeneral.Location = new System.Drawing.Point(7, 7);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Padding = new System.Windows.Forms.Padding(10);
            this.grpGeneral.Size = new System.Drawing.Size(742, 106);
            this.grpGeneral.TabIndex = 4;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = " Logging ";
            // 
            // tableLoggingGeneral
            // 
            this.tableLoggingGeneral.AutoSize = true;
            this.tableLoggingGeneral.ColumnCount = 2;
            this.tableLoggingGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLoggingGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLoggingGeneral.Controls.Add(this.chkLogVariableExpansions, 0, 3);
            this.tableLoggingGeneral.Controls.Add(this.chkLogNormalEvents, 0, 1);
            this.tableLoggingGeneral.Controls.Add(this.lblLoggingLevel, 0, 0);
            this.tableLoggingGeneral.Controls.Add(this.cbxLoggingLevel, 1, 0);
            this.tableLoggingGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLoggingGeneral.Location = new System.Drawing.Point(10, 23);
            this.tableLoggingGeneral.Name = "tableLoggingGeneral";
            this.tableLoggingGeneral.RowCount = 4;
            this.tableLoggingGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLoggingGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLoggingGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLoggingGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLoggingGeneral.Size = new System.Drawing.Size(722, 73);
            this.tableLoggingGeneral.TabIndex = 0;
            // 
            // chkLogVariableExpansions
            // 
            this.chkLogVariableExpansions.AutoSize = true;
            this.chkLogVariableExpansions.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableLoggingGeneral.SetColumnSpan(this.chkLogVariableExpansions, 3);
            this.chkLogVariableExpansions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLogVariableExpansions.Location = new System.Drawing.Point(3, 53);
            this.chkLogVariableExpansions.Name = "chkLogVariableExpansions";
            this.chkLogVariableExpansions.Size = new System.Drawing.Size(716, 17);
            this.chkLogVariableExpansions.TabIndex = 0;
            this.chkLogVariableExpansions.TabStop = false;
            this.chkLogVariableExpansions.Text = "Log variable expansions";
            this.chkLogVariableExpansions.UseVisualStyleBackColor = true;
            // 
            // chkLogNormalEvents
            // 
            this.chkLogNormalEvents.AutoSize = true;
            this.chkLogNormalEvents.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableLoggingGeneral.SetColumnSpan(this.chkLogNormalEvents, 3);
            this.chkLogNormalEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLogNormalEvents.Location = new System.Drawing.Point(3, 30);
            this.chkLogNormalEvents.Name = "chkLogNormalEvents";
            this.chkLogNormalEvents.Size = new System.Drawing.Size(716, 17);
            this.chkLogNormalEvents.TabIndex = 1;
            this.chkLogNormalEvents.TabStop = false;
            this.chkLogNormalEvents.Text = "Log normal log lines";
            this.chkLogNormalEvents.UseVisualStyleBackColor = true;
            // 
            // lblLoggingLevel
            // 
            this.lblLoggingLevel.AutoEllipsis = true;
            this.lblLoggingLevel.AutoSize = true;
            this.lblLoggingLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoggingLevel.Location = new System.Drawing.Point(3, 0);
            this.lblLoggingLevel.Name = "lblLoggingLevel";
            this.lblLoggingLevel.Size = new System.Drawing.Size(355, 27);
            this.lblLoggingLevel.TabIndex = 2;
            this.lblLoggingLevel.Text = "Logging filtering level";
            this.lblLoggingLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxLoggingLevel
            // 
            this.cbxLoggingLevel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxLoggingLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxLoggingLevel.FormattingEnabled = true;
            this.cbxLoggingLevel.Items.AddRange(new object[] {
            "Nothing",
            "Errors only",
            "Errors and warnings",
            "Above custom",
            "Above custom 2",
            "All informational messages",
            "Verbose debug"});
            this.cbxLoggingLevel.Location = new System.Drawing.Point(364, 3);
            this.cbxLoggingLevel.Name = "cbxLoggingLevel";
            this.cbxLoggingLevel.Size = new System.Drawing.Size(355, 21);
            this.cbxLoggingLevel.TabIndex = 3;
            this.cbxLoggingLevel.TabStop = false;
            // 
            // trvTrigger
            // 
            this.trvTrigger.ContextMenuStrip = this.ctxStartupTrigger;
            this.trvTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvTrigger.HideSelection = false;
            this.trvTrigger.Location = new System.Drawing.Point(10, 48);
            this.trvTrigger.MinimumSize = new System.Drawing.Size(4, 50);
            this.trvTrigger.Name = "trvTrigger";
            this.trvTrigger.ShowNodeToolTips = true;
            this.trvTrigger.Size = new System.Drawing.Size(722, 243);
            this.trvTrigger.TabIndex = 0;
            this.trvTrigger.TabStop = false;
            this.trvTrigger.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvTrigger_BeforeCollapse);
            this.trvTrigger.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvTrigger_BeforeExpand);
            this.trvTrigger.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvTrigger_BeforeSelect);
            this.trvTrigger.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvTrigger_AfterSelect);
            // 
            // ctxStartupTrigger
            // 
            this.ctxStartupTrigger.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxClearSelection});
            this.ctxStartupTrigger.Name = "ctxStartupTrigger";
            this.ctxStartupTrigger.Size = new System.Drawing.Size(152, 26);
            this.ctxStartupTrigger.Opening += new System.ComponentModel.CancelEventHandler(this.ctxStartupTrigger_Opening);
            // 
            // ctxClearSelection
            // 
            this.ctxClearSelection.Image = ((System.Drawing.Image)(resources.GetObject("ctxClearSelection.Image")));
            this.ctxClearSelection.Name = "ctxClearSelection";
            this.ctxClearSelection.Size = new System.Drawing.Size(151, 22);
            this.ctxClearSelection.Text = "Clear selection";
            this.ctxClearSelection.Click += new System.EventHandler(this.clearSelectionToolStripMenuItem_Click);
            // 
            // grpActHooks
            // 
            this.grpActHooks.AutoSize = true;
            this.grpActHooks.Controls.Add(this.tableAudioActHooks);
            this.grpActHooks.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpActHooks.Location = new System.Drawing.Point(7, 106);
            this.grpActHooks.Name = "grpActHooks";
            this.grpActHooks.Padding = new System.Windows.Forms.Padding(10);
            this.grpActHooks.Size = new System.Drawing.Size(742, 79);
            this.grpActHooks.TabIndex = 0;
            this.grpActHooks.TabStop = false;
            this.grpActHooks.Text = " ACT hooks ";
            // 
            // tableAudioActHooks
            // 
            this.tableAudioActHooks.AutoSize = true;
            this.tableAudioActHooks.ColumnCount = 1;
            this.tableAudioActHooks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAudioActHooks.Controls.Add(this.chkActSoundFiles, 0, 0);
            this.tableAudioActHooks.Controls.Add(this.chkActTts, 0, 1);
            this.tableAudioActHooks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableAudioActHooks.Location = new System.Drawing.Point(10, 23);
            this.tableAudioActHooks.Name = "tableAudioActHooks";
            this.tableAudioActHooks.RowCount = 2;
            this.tableAudioActHooks.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioActHooks.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioActHooks.Size = new System.Drawing.Size(722, 46);
            this.tableAudioActHooks.TabIndex = 0;
            // 
            // chkActSoundFiles
            // 
            this.chkActSoundFiles.AutoSize = true;
            this.chkActSoundFiles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkActSoundFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkActSoundFiles.Location = new System.Drawing.Point(3, 3);
            this.chkActSoundFiles.Name = "chkActSoundFiles";
            this.chkActSoundFiles.Size = new System.Drawing.Size(716, 17);
            this.chkActSoundFiles.TabIndex = 0;
            this.chkActSoundFiles.TabStop = false;
            this.chkActSoundFiles.Text = "Use ACT for playing sound files";
            this.chkActSoundFiles.UseVisualStyleBackColor = true;
            // 
            // chkActTts
            // 
            this.chkActTts.AutoSize = true;
            this.chkActTts.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkActTts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkActTts.Location = new System.Drawing.Point(3, 26);
            this.chkActTts.Name = "chkActTts";
            this.chkActTts.Size = new System.Drawing.Size(716, 17);
            this.chkActTts.TabIndex = 1;
            this.chkActTts.TabStop = false;
            this.chkActTts.Text = "Use ACT for text-to-speech";
            this.chkActTts.UseVisualStyleBackColor = true;
            this.chkActTts.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // pnlAudioSpacing
            // 
            this.pnlAudioSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAudioSpacing.Location = new System.Drawing.Point(7, 96);
            this.pnlAudioSpacing.Name = "pnlAudioSpacing";
            this.pnlAudioSpacing.Size = new System.Drawing.Size(742, 10);
            this.pnlAudioSpacing.TabIndex = 1;
            // 
            // grpFutureProofing
            // 
            this.grpFutureProofing.AutoSize = true;
            this.grpFutureProofing.Controls.Add(this.tableEventSeparator);
            this.grpFutureProofing.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFutureProofing.Location = new System.Drawing.Point(7, 79);
            this.grpFutureProofing.Name = "grpFutureProofing";
            this.grpFutureProofing.Padding = new System.Windows.Forms.Padding(10);
            this.grpFutureProofing.Size = new System.Drawing.Size(742, 59);
            this.grpFutureProofing.TabIndex = 4;
            this.grpFutureProofing.TabStop = false;
            this.grpFutureProofing.Text = " Future proofing ";
            // 
            // tableEventSeparator
            // 
            this.tableEventSeparator.AutoSize = true;
            this.tableEventSeparator.ColumnCount = 2;
            this.tableEventSeparator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableEventSeparator.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableEventSeparator.Controls.Add(this.txtSeparator, 0, 0);
            this.tableEventSeparator.Controls.Add(this.lblSeparator, 0, 0);
            this.tableEventSeparator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableEventSeparator.Location = new System.Drawing.Point(10, 23);
            this.tableEventSeparator.Name = "tableEventSeparator";
            this.tableEventSeparator.RowCount = 1;
            this.tableEventSeparator.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEventSeparator.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableEventSeparator.Size = new System.Drawing.Size(722, 26);
            this.tableEventSeparator.TabIndex = 0;
            // 
            // txtSeparator
            // 
            this.txtSeparator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSeparator.Location = new System.Drawing.Point(111, 3);
            this.txtSeparator.Name = "txtSeparator";
            this.txtSeparator.Size = new System.Drawing.Size(608, 20);
            this.txtSeparator.TabIndex = 7;
            // 
            // lblSeparator
            // 
            this.lblSeparator.AutoEllipsis = true;
            this.lblSeparator.AutoSize = true;
            this.lblSeparator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSeparator.Location = new System.Drawing.Point(3, 0);
            this.lblSeparator.Name = "lblSeparator";
            this.lblSeparator.Size = new System.Drawing.Size(102, 26);
            this.lblSeparator.TabIndex = 8;
            this.lblSeparator.Text = "Event text separator";
            this.lblSeparator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbcMain
            // 
            this.tbcMain.Controls.Add(this.tabGeneral);
            this.tbcMain.Controls.Add(this.tabAudio);
            this.tbcMain.Controls.Add(this.tabShortCuts);
            this.tbcMain.Controls.Add(this.tabCaching);
            this.tbcMain.Controls.Add(this.tabEndpoint);
            this.tbcMain.Controls.Add(this.tabFFXIV);
            this.tbcMain.Controls.Add(this.tabSubstitutions);
            this.tbcMain.Controls.Add(this.tabConsts);
            this.tbcMain.Controls.Add(this.tabSecurity);
            this.tbcMain.Controls.Add(this.tabMisc);
            this.tbcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcMain.Location = new System.Drawing.Point(10, 10);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.SelectedIndex = 0;
            this.tbcMain.Size = new System.Drawing.Size(764, 596);
            this.tbcMain.TabIndex = 1;
            this.tbcMain.TabStop = false;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.grpStartupTrigger);
            this.tabGeneral.Controls.Add(this.pnlStartupTriggerSpacing);
            this.tabGeneral.Controls.Add(this.grpStartup);
            this.tabGeneral.Controls.Add(this.pnlStartupSpacing);
            this.tabGeneral.Controls.Add(this.grpGeneral);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(7);
            this.tabGeneral.Size = new System.Drawing.Size(756, 570);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // grpStartupTrigger
            // 
            this.grpStartupTrigger.AutoSize = true;
            this.grpStartupTrigger.Controls.Add(this.trvTrigger);
            this.grpStartupTrigger.Controls.Add(this.tlsDirectPaste);
            this.grpStartupTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpStartupTrigger.Location = new System.Drawing.Point(7, 262);
            this.grpStartupTrigger.Name = "grpStartupTrigger";
            this.grpStartupTrigger.Padding = new System.Windows.Forms.Padding(10);
            this.grpStartupTrigger.Size = new System.Drawing.Size(742, 301);
            this.grpStartupTrigger.TabIndex = 0;
            this.grpStartupTrigger.TabStop = false;
            this.grpStartupTrigger.Text = " Startup trigger/folder ";
            // 
            // tlsDirectPaste
            // 
            this.tlsDirectPaste.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsDirectPaste.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClearSelection,
            this.lblFolderReminder});
            this.tlsDirectPaste.Location = new System.Drawing.Point(10, 23);
            this.tlsDirectPaste.Name = "tlsDirectPaste";
            this.tlsDirectPaste.Size = new System.Drawing.Size(722, 25);
            this.tlsDirectPaste.TabIndex = 1;
            // 
            // btnClearSelection
            // 
            this.btnClearSelection.Enabled = false;
            this.btnClearSelection.Image = ((System.Drawing.Image)(resources.GetObject("btnClearSelection.Image")));
            this.btnClearSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Size = new System.Drawing.Size(104, 22);
            this.btnClearSelection.Text = "Clear selection";
            this.btnClearSelection.Click += new System.EventHandler(this.btnClearSelection_Click);
            // 
            // lblFolderReminder
            // 
            this.lblFolderReminder.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblFolderReminder.Image = ((System.Drawing.Image)(resources.GetObject("lblFolderReminder.Image")));
            this.lblFolderReminder.Name = "lblFolderReminder";
            this.lblFolderReminder.Size = new System.Drawing.Size(301, 22);
            this.lblFolderReminder.Text = "Selecting a folder will fire all triggers inside the folder";
            this.lblFolderReminder.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.lblFolderReminder.Visible = false;
            // 
            // pnlStartupTriggerSpacing
            // 
            this.pnlStartupTriggerSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStartupTriggerSpacing.Location = new System.Drawing.Point(7, 252);
            this.pnlStartupTriggerSpacing.Name = "pnlStartupTriggerSpacing";
            this.pnlStartupTriggerSpacing.Size = new System.Drawing.Size(742, 10);
            this.pnlStartupTriggerSpacing.TabIndex = 1;
            // 
            // grpStartup
            // 
            this.grpStartup.AutoSize = true;
            this.grpStartup.Controls.Add(this.tableStartup);
            this.grpStartup.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpStartup.Location = new System.Drawing.Point(7, 123);
            this.grpStartup.Name = "grpStartup";
            this.grpStartup.Padding = new System.Windows.Forms.Padding(10);
            this.grpStartup.Size = new System.Drawing.Size(742, 129);
            this.grpStartup.TabIndex = 2;
            this.grpStartup.TabStop = false;
            this.grpStartup.Text = " Startup ";
            // 
            // tableStartup
            // 
            this.tableStartup.AutoSize = true;
            this.tableStartup.ColumnCount = 2;
            this.tableStartup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableStartup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableStartup.Controls.Add(this.cbxUpdateMethod, 1, 3);
            this.tableStartup.Controls.Add(this.lblUpdateMethod, 0, 3);
            this.tableStartup.Controls.Add(this.chkWarnAdmin, 0, 1);
            this.tableStartup.Controls.Add(this.chkUpdates, 0, 2);
            this.tableStartup.Controls.Add(this.chkWelcome, 0, 0);
            this.tableStartup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableStartup.Location = new System.Drawing.Point(10, 23);
            this.tableStartup.Name = "tableStartup";
            this.tableStartup.RowCount = 4;
            this.tableStartup.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableStartup.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableStartup.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableStartup.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableStartup.Size = new System.Drawing.Size(722, 96);
            this.tableStartup.TabIndex = 0;
            // 
            // cbxUpdateMethod
            // 
            this.cbxUpdateMethod.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxUpdateMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUpdateMethod.FormattingEnabled = true;
            this.cbxUpdateMethod.Items.AddRange(new object[] {
            "Built-in (legacy)",
            "ACT"});
            this.cbxUpdateMethod.Location = new System.Drawing.Point(364, 72);
            this.cbxUpdateMethod.Name = "cbxUpdateMethod";
            this.cbxUpdateMethod.Size = new System.Drawing.Size(355, 21);
            this.cbxUpdateMethod.TabIndex = 0;
            this.cbxUpdateMethod.TabStop = false;
            // 
            // lblUpdateMethod
            // 
            this.lblUpdateMethod.AutoEllipsis = true;
            this.lblUpdateMethod.AutoSize = true;
            this.lblUpdateMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUpdateMethod.Location = new System.Drawing.Point(3, 69);
            this.lblUpdateMethod.Name = "lblUpdateMethod";
            this.lblUpdateMethod.Size = new System.Drawing.Size(355, 27);
            this.lblUpdateMethod.TabIndex = 1;
            this.lblUpdateMethod.Text = "Plugin update check method";
            this.lblUpdateMethod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkWarnAdmin
            // 
            this.chkWarnAdmin.AutoSize = true;
            this.chkWarnAdmin.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableStartup.SetColumnSpan(this.chkWarnAdmin, 2);
            this.chkWarnAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkWarnAdmin.Location = new System.Drawing.Point(3, 26);
            this.chkWarnAdmin.Name = "chkWarnAdmin";
            this.chkWarnAdmin.Size = new System.Drawing.Size(716, 17);
            this.chkWarnAdmin.TabIndex = 2;
            this.chkWarnAdmin.TabStop = false;
            this.chkWarnAdmin.Text = "Warn if not running as Administrator";
            this.chkWarnAdmin.UseVisualStyleBackColor = true;
            // 
            // chkUpdates
            // 
            this.chkUpdates.AutoSize = true;
            this.chkUpdates.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableStartup.SetColumnSpan(this.chkUpdates, 2);
            this.chkUpdates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkUpdates.Location = new System.Drawing.Point(3, 49);
            this.chkUpdates.Name = "chkUpdates";
            this.chkUpdates.Size = new System.Drawing.Size(716, 17);
            this.chkUpdates.TabIndex = 3;
            this.chkUpdates.TabStop = false;
            this.chkUpdates.Text = "Check for updates on startup";
            this.chkUpdates.UseVisualStyleBackColor = true;
            // 
            // chkWelcome
            // 
            this.chkWelcome.AutoSize = true;
            this.chkWelcome.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableStartup.SetColumnSpan(this.chkWelcome, 2);
            this.chkWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkWelcome.Location = new System.Drawing.Point(3, 3);
            this.chkWelcome.Name = "chkWelcome";
            this.chkWelcome.Size = new System.Drawing.Size(716, 17);
            this.chkWelcome.TabIndex = 4;
            this.chkWelcome.TabStop = false;
            this.chkWelcome.Text = "Show Welcome Screen on startup";
            this.chkWelcome.UseVisualStyleBackColor = true;
            // 
            // pnlStartupSpacing
            // 
            this.pnlStartupSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStartupSpacing.Location = new System.Drawing.Point(7, 113);
            this.pnlStartupSpacing.Name = "pnlStartupSpacing";
            this.pnlStartupSpacing.Size = new System.Drawing.Size(742, 10);
            this.pnlStartupSpacing.TabIndex = 3;
            // 
            // tabAudio
            // 
            this.tabAudio.Controls.Add(this.grpActHooks);
            this.tabAudio.Controls.Add(this.pnlAudioSpacing);
            this.tabAudio.Controls.Add(this.grpVolAdjustment);
            this.tabAudio.Location = new System.Drawing.Point(4, 22);
            this.tabAudio.Name = "tabAudio";
            this.tabAudio.Padding = new System.Windows.Forms.Padding(7);
            this.tabAudio.Size = new System.Drawing.Size(756, 570);
            this.tabAudio.TabIndex = 1;
            this.tabAudio.Text = "Audio";
            this.tabAudio.UseVisualStyleBackColor = true;
            // 
            // tabShortCuts
            // 
            this.tabShortCuts.Controls.Add(this.grpShortCutExpression);
            this.tabShortCuts.Location = new System.Drawing.Point(4, 22);
            this.tabShortCuts.Name = "tabShortCuts";
            this.tabShortCuts.Padding = new System.Windows.Forms.Padding(7);
            this.tabShortCuts.Size = new System.Drawing.Size(756, 570);
            this.tabShortCuts.TabIndex = 2;
            this.tabShortCuts.Text = "Shortcuts";
            this.tabShortCuts.UseVisualStyleBackColor = true;
            // 
            // grpShortCutExpression
            // 
            this.grpShortCutExpression.AutoSize = true;
            this.grpShortCutExpression.Controls.Add(this.tableLayoutPanelShortCutExpression);
            this.grpShortCutExpression.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpShortCutExpression.Location = new System.Drawing.Point(7, 7);
            this.grpShortCutExpression.Name = "grpShortCutExpression";
            this.grpShortCutExpression.Padding = new System.Windows.Forms.Padding(10);
            this.grpShortCutExpression.Size = new System.Drawing.Size(742, 102);
            this.grpShortCutExpression.TabIndex = 0;
            this.grpShortCutExpression.TabStop = false;
            this.grpShortCutExpression.Text = " Expressions ";
            // 
            // tableLayoutPanelShortCutExpression
            // 
            this.tableLayoutPanelShortCutExpression.AutoSize = true;
            this.tableLayoutPanelShortCutExpression.ColumnCount = 2;
            this.tableLayoutPanelShortCutExpression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelShortCutExpression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelShortCutExpression.Controls.Add(this.chkShortcutEnableTemplates, 0, 0);
            this.tableLayoutPanelShortCutExpression.Controls.Add(this.chkShortcutUseAbbrevInTemplates, 0, 1);
            this.tableLayoutPanelShortCutExpression.Controls.Add(this.chkShortcutWrapTextWhenSelected, 0, 2);
            this.tableLayoutPanelShortCutExpression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelShortCutExpression.Location = new System.Drawing.Point(10, 23);
            this.tableLayoutPanelShortCutExpression.Name = "tableLayoutPanelShortCutExpression";
            this.tableLayoutPanelShortCutExpression.RowCount = 3;
            this.tableLayoutPanelShortCutExpression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelShortCutExpression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelShortCutExpression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelShortCutExpression.Size = new System.Drawing.Size(722, 69);
            this.tableLayoutPanelShortCutExpression.TabIndex = 0;
            // 
            // chkShortcutEnableTemplates
            // 
            this.chkShortcutEnableTemplates.AutoSize = true;
            this.chkShortcutEnableTemplates.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableLayoutPanelShortCutExpression.SetColumnSpan(this.chkShortcutEnableTemplates, 2);
            this.chkShortcutEnableTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShortcutEnableTemplates.Location = new System.Drawing.Point(3, 3);
            this.chkShortcutEnableTemplates.Name = "chkShortcutEnableTemplates";
            this.chkShortcutEnableTemplates.Size = new System.Drawing.Size(716, 17);
            this.chkShortcutEnableTemplates.TabIndex = 0;
            this.chkShortcutEnableTemplates.TabStop = false;
            this.chkShortcutEnableTemplates.Text = "Enable shortcuts to input template expressions";
            this.chkShortcutEnableTemplates.UseVisualStyleBackColor = true;
            // 
            // chkShortcutUseAbbrevInTemplates
            // 
            this.chkShortcutUseAbbrevInTemplates.AutoSize = true;
            this.chkShortcutUseAbbrevInTemplates.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableLayoutPanelShortCutExpression.SetColumnSpan(this.chkShortcutUseAbbrevInTemplates, 2);
            this.chkShortcutUseAbbrevInTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShortcutUseAbbrevInTemplates.Location = new System.Drawing.Point(3, 26);
            this.chkShortcutUseAbbrevInTemplates.Name = "chkShortcutUseAbbrevInTemplates";
            this.chkShortcutUseAbbrevInTemplates.Size = new System.Drawing.Size(716, 17);
            this.chkShortcutUseAbbrevInTemplates.TabIndex = 1;
            this.chkShortcutUseAbbrevInTemplates.TabStop = false;
            this.chkShortcutUseAbbrevInTemplates.Text = "Use abbreviation expressions in template expressions";
            this.chkShortcutUseAbbrevInTemplates.UseVisualStyleBackColor = true;
            // 
            // chkShortcutWrapTextWhenSelected
            // 
            this.chkShortcutWrapTextWhenSelected.AutoSize = true;
            this.chkShortcutWrapTextWhenSelected.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableLayoutPanelShortCutExpression.SetColumnSpan(this.chkShortcutWrapTextWhenSelected, 2);
            this.chkShortcutWrapTextWhenSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShortcutWrapTextWhenSelected.Location = new System.Drawing.Point(3, 49);
            this.chkShortcutWrapTextWhenSelected.Name = "chkShortcutWrapTextWhenSelected";
            this.chkShortcutWrapTextWhenSelected.Size = new System.Drawing.Size(716, 17);
            this.chkShortcutWrapTextWhenSelected.TabIndex = 2;
            this.chkShortcutWrapTextWhenSelected.TabStop = false;
            this.chkShortcutWrapTextWhenSelected.Text = "Wrap selected text in template expressions";
            this.chkShortcutWrapTextWhenSelected.UseVisualStyleBackColor = true;
            // 
            // tabCaching
            // 
            this.tabCaching.Controls.Add(this.pnlCaching);
            this.tabCaching.Location = new System.Drawing.Point(4, 22);
            this.tabCaching.Name = "tabCaching";
            this.tabCaching.Padding = new System.Windows.Forms.Padding(7);
            this.tabCaching.Size = new System.Drawing.Size(756, 570);
            this.tabCaching.TabIndex = 6;
            this.tabCaching.Text = "Caching";
            this.tabCaching.UseVisualStyleBackColor = true;
            // 
            // pnlCaching
            // 
            this.pnlCaching.AutoScroll = true;
            this.pnlCaching.Controls.Add(this.grpCacheFile);
            this.pnlCaching.Controls.Add(this.pnlCacheFileSpacing);
            this.pnlCaching.Controls.Add(this.grpCacheRepo);
            this.pnlCaching.Controls.Add(this.pnlCacheRepoSpacing);
            this.pnlCaching.Controls.Add(this.grpCacheJSON);
            this.pnlCaching.Controls.Add(this.pnlCacheJsonSpacing);
            this.pnlCaching.Controls.Add(this.grpCacheSound);
            this.pnlCaching.Controls.Add(this.pnlCacheSoundSpacing);
            this.pnlCaching.Controls.Add(this.grpCacheImage);
            this.pnlCaching.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCaching.Location = new System.Drawing.Point(7, 7);
            this.pnlCaching.Name = "pnlCaching";
            this.pnlCaching.Size = new System.Drawing.Size(742, 556);
            this.pnlCaching.TabIndex = 0;
            // 
            // grpCacheFile
            // 
            this.grpCacheFile.AutoSize = true;
            this.grpCacheFile.Controls.Add(this.tableCacheFile);
            this.grpCacheFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCacheFile.Location = new System.Drawing.Point(0, 600);
            this.grpCacheFile.Name = "grpCacheFile";
            this.grpCacheFile.Padding = new System.Windows.Forms.Padding(10);
            this.grpCacheFile.Size = new System.Drawing.Size(725, 140);
            this.grpCacheFile.TabIndex = 0;
            this.grpCacheFile.TabStop = false;
            this.grpCacheFile.Text = " File downloads ";
            // 
            // tableCacheFile
            // 
            this.tableCacheFile.AutoSize = true;
            this.tableCacheFile.ColumnCount = 3;
            this.tableCacheFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableCacheFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCacheFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCacheFile.Controls.Add(this.btnCacheFileBrowse, 2, 3);
            this.tableCacheFile.Controls.Add(this.btnCacheFileClear, 1, 3);
            this.tableCacheFile.Controls.Add(this.txtCacheFileSize, 1, 2);
            this.tableCacheFile.Controls.Add(this.lblCacheFileSize, 0, 2);
            this.tableCacheFile.Controls.Add(this.lblCacheFileCount, 0, 1);
            this.tableCacheFile.Controls.Add(this.lblCacheFileExpiry, 0, 0);
            this.tableCacheFile.Controls.Add(this.nudCacheFileExpiry, 1, 0);
            this.tableCacheFile.Controls.Add(this.txtCacheFileCount, 1, 1);
            this.tableCacheFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCacheFile.Location = new System.Drawing.Point(10, 23);
            this.tableCacheFile.Name = "tableCacheFile";
            this.tableCacheFile.RowCount = 4;
            this.tableCacheFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheFile.Size = new System.Drawing.Size(705, 107);
            this.tableCacheFile.TabIndex = 0;
            // 
            // btnCacheFileBrowse
            // 
            this.btnCacheFileBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheFileBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheFileBrowse.Image")));
            this.btnCacheFileBrowse.Location = new System.Drawing.Point(420, 81);
            this.btnCacheFileBrowse.Name = "btnCacheFileBrowse";
            this.btnCacheFileBrowse.Size = new System.Drawing.Size(282, 23);
            this.btnCacheFileBrowse.TabIndex = 0;
            this.btnCacheFileBrowse.TabStop = false;
            this.btnCacheFileBrowse.Text = "Browse";
            this.btnCacheFileBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCacheFileBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCacheFileBrowse.UseMnemonic = false;
            this.btnCacheFileBrowse.UseVisualStyleBackColor = true;
            this.btnCacheFileBrowse.Click += new System.EventHandler(this.btnCacheFileBrowse_Click);
            // 
            // btnCacheFileClear
            // 
            this.btnCacheFileClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheFileClear.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheFileClear.Image")));
            this.btnCacheFileClear.Location = new System.Drawing.Point(132, 81);
            this.btnCacheFileClear.Name = "btnCacheFileClear";
            this.btnCacheFileClear.Size = new System.Drawing.Size(282, 23);
            this.btnCacheFileClear.TabIndex = 1;
            this.btnCacheFileClear.TabStop = false;
            this.btnCacheFileClear.Text = "Clear cache";
            this.btnCacheFileClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCacheFileClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCacheFileClear.UseMnemonic = false;
            this.btnCacheFileClear.UseVisualStyleBackColor = true;
            this.btnCacheFileClear.Click += new System.EventHandler(this.btnCacheFileClear_Click);
            // 
            // txtCacheFileSize
            // 
            this.tableCacheFile.SetColumnSpan(this.txtCacheFileSize, 2);
            this.txtCacheFileSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheFileSize.Location = new System.Drawing.Point(132, 55);
            this.txtCacheFileSize.Name = "txtCacheFileSize";
            this.txtCacheFileSize.ReadOnly = true;
            this.txtCacheFileSize.Size = new System.Drawing.Size(570, 20);
            this.txtCacheFileSize.TabIndex = 13;
            this.txtCacheFileSize.Text = "0";
            this.txtCacheFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCacheFileSize
            // 
            this.lblCacheFileSize.AutoEllipsis = true;
            this.lblCacheFileSize.AutoSize = true;
            this.lblCacheFileSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheFileSize.Location = new System.Drawing.Point(3, 52);
            this.lblCacheFileSize.Name = "lblCacheFileSize";
            this.lblCacheFileSize.Size = new System.Drawing.Size(123, 26);
            this.lblCacheFileSize.TabIndex = 14;
            this.lblCacheFileSize.Text = "Current disk size in bytes";
            this.lblCacheFileSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheFileCount
            // 
            this.lblCacheFileCount.AutoEllipsis = true;
            this.lblCacheFileCount.AutoSize = true;
            this.lblCacheFileCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheFileCount.Location = new System.Drawing.Point(3, 26);
            this.lblCacheFileCount.Name = "lblCacheFileCount";
            this.lblCacheFileCount.Size = new System.Drawing.Size(123, 26);
            this.lblCacheFileCount.TabIndex = 15;
            this.lblCacheFileCount.Text = "Current item count";
            this.lblCacheFileCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheFileExpiry
            // 
            this.lblCacheFileExpiry.AutoEllipsis = true;
            this.lblCacheFileExpiry.AutoSize = true;
            this.lblCacheFileExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheFileExpiry.Location = new System.Drawing.Point(3, 0);
            this.lblCacheFileExpiry.Name = "lblCacheFileExpiry";
            this.lblCacheFileExpiry.Size = new System.Drawing.Size(123, 26);
            this.lblCacheFileExpiry.TabIndex = 16;
            this.lblCacheFileExpiry.Text = "Expiration in minutes";
            this.lblCacheFileExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudCacheFileExpiry
            // 
            this.tableCacheFile.SetColumnSpan(this.nudCacheFileExpiry, 2);
            this.nudCacheFileExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCacheFileExpiry.Location = new System.Drawing.Point(132, 3);
            this.nudCacheFileExpiry.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudCacheFileExpiry.Name = "nudCacheFileExpiry";
            this.nudCacheFileExpiry.Size = new System.Drawing.Size(570, 20);
            this.nudCacheFileExpiry.TabIndex = 17;
            this.nudCacheFileExpiry.TabStop = false;
            this.nudCacheFileExpiry.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudCacheFileExpiry.Value = new decimal(new int[] {
            518400,
            0,
            0,
            0});
            // 
            // txtCacheFileCount
            // 
            this.tableCacheFile.SetColumnSpan(this.txtCacheFileCount, 2);
            this.txtCacheFileCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheFileCount.Location = new System.Drawing.Point(132, 29);
            this.txtCacheFileCount.Name = "txtCacheFileCount";
            this.txtCacheFileCount.ReadOnly = true;
            this.txtCacheFileCount.Size = new System.Drawing.Size(570, 20);
            this.txtCacheFileCount.TabIndex = 11;
            this.txtCacheFileCount.Text = "0";
            this.txtCacheFileCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pnlCacheFileSpacing
            // 
            this.pnlCacheFileSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCacheFileSpacing.Location = new System.Drawing.Point(0, 590);
            this.pnlCacheFileSpacing.Name = "pnlCacheFileSpacing";
            this.pnlCacheFileSpacing.Size = new System.Drawing.Size(725, 10);
            this.pnlCacheFileSpacing.TabIndex = 1;
            // 
            // grpCacheRepo
            // 
            this.grpCacheRepo.AutoSize = true;
            this.grpCacheRepo.Controls.Add(this.tableCacheRepo);
            this.grpCacheRepo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCacheRepo.Location = new System.Drawing.Point(0, 450);
            this.grpCacheRepo.Name = "grpCacheRepo";
            this.grpCacheRepo.Padding = new System.Windows.Forms.Padding(10);
            this.grpCacheRepo.Size = new System.Drawing.Size(725, 140);
            this.grpCacheRepo.TabIndex = 2;
            this.grpCacheRepo.TabStop = false;
            this.grpCacheRepo.Text = " Repository backups ";
            // 
            // tableCacheRepo
            // 
            this.tableCacheRepo.AutoSize = true;
            this.tableCacheRepo.ColumnCount = 3;
            this.tableCacheRepo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableCacheRepo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCacheRepo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCacheRepo.Controls.Add(this.btnCacheRepoBrowse, 2, 3);
            this.tableCacheRepo.Controls.Add(this.btnCacheRepoClear, 1, 3);
            this.tableCacheRepo.Controls.Add(this.txtCacheRepoSize, 1, 2);
            this.tableCacheRepo.Controls.Add(this.lblCacheRepoSize, 0, 2);
            this.tableCacheRepo.Controls.Add(this.lblCacheRepoCount, 0, 1);
            this.tableCacheRepo.Controls.Add(this.lblCacheRepoExpiry, 0, 0);
            this.tableCacheRepo.Controls.Add(this.nudCacheRepoExpiry, 1, 0);
            this.tableCacheRepo.Controls.Add(this.txtCacheRepoCount, 1, 1);
            this.tableCacheRepo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCacheRepo.Location = new System.Drawing.Point(10, 23);
            this.tableCacheRepo.Name = "tableCacheRepo";
            this.tableCacheRepo.RowCount = 4;
            this.tableCacheRepo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheRepo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheRepo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheRepo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheRepo.Size = new System.Drawing.Size(705, 107);
            this.tableCacheRepo.TabIndex = 0;
            // 
            // btnCacheRepoBrowse
            // 
            this.btnCacheRepoBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheRepoBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheRepoBrowse.Image")));
            this.btnCacheRepoBrowse.Location = new System.Drawing.Point(420, 81);
            this.btnCacheRepoBrowse.Name = "btnCacheRepoBrowse";
            this.btnCacheRepoBrowse.Size = new System.Drawing.Size(282, 23);
            this.btnCacheRepoBrowse.TabIndex = 0;
            this.btnCacheRepoBrowse.TabStop = false;
            this.btnCacheRepoBrowse.Text = "Browse";
            this.btnCacheRepoBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCacheRepoBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCacheRepoBrowse.UseMnemonic = false;
            this.btnCacheRepoBrowse.UseVisualStyleBackColor = true;
            this.btnCacheRepoBrowse.Click += new System.EventHandler(this.btnCacheRepoBrowse_Click);
            // 
            // btnCacheRepoClear
            // 
            this.btnCacheRepoClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheRepoClear.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheRepoClear.Image")));
            this.btnCacheRepoClear.Location = new System.Drawing.Point(132, 81);
            this.btnCacheRepoClear.Name = "btnCacheRepoClear";
            this.btnCacheRepoClear.Size = new System.Drawing.Size(282, 23);
            this.btnCacheRepoClear.TabIndex = 1;
            this.btnCacheRepoClear.TabStop = false;
            this.btnCacheRepoClear.Text = "Clear cache";
            this.btnCacheRepoClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCacheRepoClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCacheRepoClear.UseMnemonic = false;
            this.btnCacheRepoClear.UseVisualStyleBackColor = true;
            this.btnCacheRepoClear.Click += new System.EventHandler(this.btnCacheRepoClear_Click);
            // 
            // txtCacheRepoSize
            // 
            this.tableCacheRepo.SetColumnSpan(this.txtCacheRepoSize, 2);
            this.txtCacheRepoSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheRepoSize.Location = new System.Drawing.Point(132, 55);
            this.txtCacheRepoSize.Name = "txtCacheRepoSize";
            this.txtCacheRepoSize.ReadOnly = true;
            this.txtCacheRepoSize.Size = new System.Drawing.Size(570, 20);
            this.txtCacheRepoSize.TabIndex = 13;
            this.txtCacheRepoSize.Text = "0";
            this.txtCacheRepoSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCacheRepoSize
            // 
            this.lblCacheRepoSize.AutoEllipsis = true;
            this.lblCacheRepoSize.AutoSize = true;
            this.lblCacheRepoSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheRepoSize.Location = new System.Drawing.Point(3, 52);
            this.lblCacheRepoSize.Name = "lblCacheRepoSize";
            this.lblCacheRepoSize.Size = new System.Drawing.Size(123, 26);
            this.lblCacheRepoSize.TabIndex = 14;
            this.lblCacheRepoSize.Text = "Current disk size in bytes";
            this.lblCacheRepoSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheRepoCount
            // 
            this.lblCacheRepoCount.AutoEllipsis = true;
            this.lblCacheRepoCount.AutoSize = true;
            this.lblCacheRepoCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheRepoCount.Location = new System.Drawing.Point(3, 26);
            this.lblCacheRepoCount.Name = "lblCacheRepoCount";
            this.lblCacheRepoCount.Size = new System.Drawing.Size(123, 26);
            this.lblCacheRepoCount.TabIndex = 15;
            this.lblCacheRepoCount.Text = "Current item count";
            this.lblCacheRepoCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheRepoExpiry
            // 
            this.lblCacheRepoExpiry.AutoEllipsis = true;
            this.lblCacheRepoExpiry.AutoSize = true;
            this.lblCacheRepoExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheRepoExpiry.Location = new System.Drawing.Point(3, 0);
            this.lblCacheRepoExpiry.Name = "lblCacheRepoExpiry";
            this.lblCacheRepoExpiry.Size = new System.Drawing.Size(123, 26);
            this.lblCacheRepoExpiry.TabIndex = 16;
            this.lblCacheRepoExpiry.Text = "Expiration in minutes";
            this.lblCacheRepoExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudCacheRepoExpiry
            // 
            this.tableCacheRepo.SetColumnSpan(this.nudCacheRepoExpiry, 2);
            this.nudCacheRepoExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCacheRepoExpiry.Location = new System.Drawing.Point(132, 3);
            this.nudCacheRepoExpiry.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudCacheRepoExpiry.Name = "nudCacheRepoExpiry";
            this.nudCacheRepoExpiry.Size = new System.Drawing.Size(570, 20);
            this.nudCacheRepoExpiry.TabIndex = 17;
            this.nudCacheRepoExpiry.TabStop = false;
            this.nudCacheRepoExpiry.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudCacheRepoExpiry.Value = new decimal(new int[] {
            518400,
            0,
            0,
            0});
            // 
            // txtCacheRepoCount
            // 
            this.tableCacheRepo.SetColumnSpan(this.txtCacheRepoCount, 2);
            this.txtCacheRepoCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheRepoCount.Location = new System.Drawing.Point(132, 29);
            this.txtCacheRepoCount.Name = "txtCacheRepoCount";
            this.txtCacheRepoCount.ReadOnly = true;
            this.txtCacheRepoCount.Size = new System.Drawing.Size(570, 20);
            this.txtCacheRepoCount.TabIndex = 11;
            this.txtCacheRepoCount.Text = "0";
            this.txtCacheRepoCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pnlCacheRepoSpacing
            // 
            this.pnlCacheRepoSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCacheRepoSpacing.Location = new System.Drawing.Point(0, 440);
            this.pnlCacheRepoSpacing.Name = "pnlCacheRepoSpacing";
            this.pnlCacheRepoSpacing.Size = new System.Drawing.Size(725, 10);
            this.pnlCacheRepoSpacing.TabIndex = 3;
            // 
            // grpCacheJSON
            // 
            this.grpCacheJSON.AutoSize = true;
            this.grpCacheJSON.Controls.Add(this.tableCacheJson);
            this.grpCacheJSON.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCacheJSON.Location = new System.Drawing.Point(0, 300);
            this.grpCacheJSON.Name = "grpCacheJSON";
            this.grpCacheJSON.Padding = new System.Windows.Forms.Padding(10);
            this.grpCacheJSON.Size = new System.Drawing.Size(725, 140);
            this.grpCacheJSON.TabIndex = 4;
            this.grpCacheJSON.TabStop = false;
            this.grpCacheJSON.Text = " JSON responses ";
            // 
            // tableCacheJson
            // 
            this.tableCacheJson.AutoSize = true;
            this.tableCacheJson.ColumnCount = 3;
            this.tableCacheJson.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableCacheJson.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCacheJson.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCacheJson.Controls.Add(this.nudCacheJsonExpiry, 1, 0);
            this.tableCacheJson.Controls.Add(this.btnCacheJsonBrowse, 2, 3);
            this.tableCacheJson.Controls.Add(this.btnCacheJsonClear, 1, 3);
            this.tableCacheJson.Controls.Add(this.txtCacheJsonSize, 1, 2);
            this.tableCacheJson.Controls.Add(this.lblCacheJsonSize, 0, 2);
            this.tableCacheJson.Controls.Add(this.lblCacheJsonCount, 0, 1);
            this.tableCacheJson.Controls.Add(this.lblCacheJsonExpiry, 0, 0);
            this.tableCacheJson.Controls.Add(this.txtCacheJsonCount, 1, 1);
            this.tableCacheJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCacheJson.Location = new System.Drawing.Point(10, 23);
            this.tableCacheJson.Name = "tableCacheJson";
            this.tableCacheJson.RowCount = 4;
            this.tableCacheJson.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheJson.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheJson.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheJson.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheJson.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableCacheJson.Size = new System.Drawing.Size(705, 107);
            this.tableCacheJson.TabIndex = 0;
            // 
            // nudCacheJsonExpiry
            // 
            this.tableCacheJson.SetColumnSpan(this.nudCacheJsonExpiry, 2);
            this.nudCacheJsonExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCacheJsonExpiry.Location = new System.Drawing.Point(132, 3);
            this.nudCacheJsonExpiry.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudCacheJsonExpiry.Name = "nudCacheJsonExpiry";
            this.nudCacheJsonExpiry.Size = new System.Drawing.Size(570, 20);
            this.nudCacheJsonExpiry.TabIndex = 0;
            this.nudCacheJsonExpiry.TabStop = false;
            this.nudCacheJsonExpiry.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudCacheJsonExpiry.Value = new decimal(new int[] {
            10800,
            0,
            0,
            0});
            // 
            // btnCacheJsonBrowse
            // 
            this.btnCacheJsonBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheJsonBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheJsonBrowse.Image")));
            this.btnCacheJsonBrowse.Location = new System.Drawing.Point(420, 81);
            this.btnCacheJsonBrowse.Name = "btnCacheJsonBrowse";
            this.btnCacheJsonBrowse.Size = new System.Drawing.Size(282, 23);
            this.btnCacheJsonBrowse.TabIndex = 1;
            this.btnCacheJsonBrowse.TabStop = false;
            this.btnCacheJsonBrowse.Text = "Browse";
            this.btnCacheJsonBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCacheJsonBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCacheJsonBrowse.UseMnemonic = false;
            this.btnCacheJsonBrowse.UseVisualStyleBackColor = true;
            this.btnCacheJsonBrowse.Click += new System.EventHandler(this.btnCacheJsonBrowse_Click);
            // 
            // btnCacheJsonClear
            // 
            this.btnCacheJsonClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheJsonClear.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheJsonClear.Image")));
            this.btnCacheJsonClear.Location = new System.Drawing.Point(132, 81);
            this.btnCacheJsonClear.Name = "btnCacheJsonClear";
            this.btnCacheJsonClear.Size = new System.Drawing.Size(282, 23);
            this.btnCacheJsonClear.TabIndex = 2;
            this.btnCacheJsonClear.TabStop = false;
            this.btnCacheJsonClear.Text = "Clear cache";
            this.btnCacheJsonClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCacheJsonClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCacheJsonClear.UseMnemonic = false;
            this.btnCacheJsonClear.UseVisualStyleBackColor = true;
            this.btnCacheJsonClear.Click += new System.EventHandler(this.btnCacheJsonClear_Click);
            // 
            // txtCacheJsonSize
            // 
            this.tableCacheJson.SetColumnSpan(this.txtCacheJsonSize, 2);
            this.txtCacheJsonSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheJsonSize.Location = new System.Drawing.Point(132, 55);
            this.txtCacheJsonSize.Name = "txtCacheJsonSize";
            this.txtCacheJsonSize.ReadOnly = true;
            this.txtCacheJsonSize.Size = new System.Drawing.Size(570, 20);
            this.txtCacheJsonSize.TabIndex = 13;
            this.txtCacheJsonSize.Text = "0";
            this.txtCacheJsonSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCacheJsonSize
            // 
            this.lblCacheJsonSize.AutoEllipsis = true;
            this.lblCacheJsonSize.AutoSize = true;
            this.lblCacheJsonSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheJsonSize.Location = new System.Drawing.Point(3, 52);
            this.lblCacheJsonSize.Name = "lblCacheJsonSize";
            this.lblCacheJsonSize.Size = new System.Drawing.Size(123, 26);
            this.lblCacheJsonSize.TabIndex = 14;
            this.lblCacheJsonSize.Text = "Current disk size in bytes";
            this.lblCacheJsonSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheJsonCount
            // 
            this.lblCacheJsonCount.AutoEllipsis = true;
            this.lblCacheJsonCount.AutoSize = true;
            this.lblCacheJsonCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheJsonCount.Location = new System.Drawing.Point(3, 26);
            this.lblCacheJsonCount.Name = "lblCacheJsonCount";
            this.lblCacheJsonCount.Size = new System.Drawing.Size(123, 26);
            this.lblCacheJsonCount.TabIndex = 15;
            this.lblCacheJsonCount.Text = "Current item count";
            this.lblCacheJsonCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheJsonExpiry
            // 
            this.lblCacheJsonExpiry.AutoEllipsis = true;
            this.lblCacheJsonExpiry.AutoSize = true;
            this.lblCacheJsonExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheJsonExpiry.Location = new System.Drawing.Point(3, 0);
            this.lblCacheJsonExpiry.Name = "lblCacheJsonExpiry";
            this.lblCacheJsonExpiry.Size = new System.Drawing.Size(123, 26);
            this.lblCacheJsonExpiry.TabIndex = 16;
            this.lblCacheJsonExpiry.Text = "Expiration in minutes";
            this.lblCacheJsonExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCacheJsonCount
            // 
            this.tableCacheJson.SetColumnSpan(this.txtCacheJsonCount, 2);
            this.txtCacheJsonCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheJsonCount.Location = new System.Drawing.Point(132, 29);
            this.txtCacheJsonCount.Name = "txtCacheJsonCount";
            this.txtCacheJsonCount.ReadOnly = true;
            this.txtCacheJsonCount.Size = new System.Drawing.Size(570, 20);
            this.txtCacheJsonCount.TabIndex = 11;
            this.txtCacheJsonCount.Text = "0";
            this.txtCacheJsonCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pnlCacheJsonSpacing
            // 
            this.pnlCacheJsonSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCacheJsonSpacing.Location = new System.Drawing.Point(0, 290);
            this.pnlCacheJsonSpacing.Name = "pnlCacheJsonSpacing";
            this.pnlCacheJsonSpacing.Size = new System.Drawing.Size(725, 10);
            this.pnlCacheJsonSpacing.TabIndex = 5;
            // 
            // grpCacheSound
            // 
            this.grpCacheSound.AutoSize = true;
            this.grpCacheSound.Controls.Add(this.tableCacheSound);
            this.grpCacheSound.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCacheSound.Location = new System.Drawing.Point(0, 150);
            this.grpCacheSound.Name = "grpCacheSound";
            this.grpCacheSound.Padding = new System.Windows.Forms.Padding(10);
            this.grpCacheSound.Size = new System.Drawing.Size(725, 140);
            this.grpCacheSound.TabIndex = 6;
            this.grpCacheSound.TabStop = false;
            this.grpCacheSound.Text = " Sound files ";
            // 
            // tableCacheSound
            // 
            this.tableCacheSound.AutoSize = true;
            this.tableCacheSound.ColumnCount = 3;
            this.tableCacheSound.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableCacheSound.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCacheSound.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCacheSound.Controls.Add(this.btnCacheSoundBrowse, 2, 3);
            this.tableCacheSound.Controls.Add(this.btnCacheSoundClear, 1, 3);
            this.tableCacheSound.Controls.Add(this.txtCacheSoundSize, 1, 2);
            this.tableCacheSound.Controls.Add(this.lblCacheSoundSize, 0, 2);
            this.tableCacheSound.Controls.Add(this.lblCacheSoundCount, 0, 1);
            this.tableCacheSound.Controls.Add(this.lblCacheSoundExpiry, 0, 0);
            this.tableCacheSound.Controls.Add(this.nudCacheSoundExpiry, 1, 0);
            this.tableCacheSound.Controls.Add(this.txtCacheSoundCount, 1, 1);
            this.tableCacheSound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCacheSound.Location = new System.Drawing.Point(10, 23);
            this.tableCacheSound.Name = "tableCacheSound";
            this.tableCacheSound.RowCount = 4;
            this.tableCacheSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheSound.Size = new System.Drawing.Size(705, 107);
            this.tableCacheSound.TabIndex = 0;
            // 
            // btnCacheSoundBrowse
            // 
            this.btnCacheSoundBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheSoundBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheSoundBrowse.Image")));
            this.btnCacheSoundBrowse.Location = new System.Drawing.Point(420, 81);
            this.btnCacheSoundBrowse.Name = "btnCacheSoundBrowse";
            this.btnCacheSoundBrowse.Size = new System.Drawing.Size(282, 23);
            this.btnCacheSoundBrowse.TabIndex = 0;
            this.btnCacheSoundBrowse.TabStop = false;
            this.btnCacheSoundBrowse.Text = "Browse";
            this.btnCacheSoundBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCacheSoundBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCacheSoundBrowse.UseMnemonic = false;
            this.btnCacheSoundBrowse.UseVisualStyleBackColor = true;
            this.btnCacheSoundBrowse.Click += new System.EventHandler(this.btnCacheSoundBrowse_Click);
            // 
            // btnCacheSoundClear
            // 
            this.btnCacheSoundClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheSoundClear.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheSoundClear.Image")));
            this.btnCacheSoundClear.Location = new System.Drawing.Point(132, 81);
            this.btnCacheSoundClear.Name = "btnCacheSoundClear";
            this.btnCacheSoundClear.Size = new System.Drawing.Size(282, 23);
            this.btnCacheSoundClear.TabIndex = 1;
            this.btnCacheSoundClear.TabStop = false;
            this.btnCacheSoundClear.Text = "Clear cache";
            this.btnCacheSoundClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCacheSoundClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCacheSoundClear.UseMnemonic = false;
            this.btnCacheSoundClear.UseVisualStyleBackColor = true;
            this.btnCacheSoundClear.Click += new System.EventHandler(this.btnCacheSoundClear_Click);
            // 
            // txtCacheSoundSize
            // 
            this.tableCacheSound.SetColumnSpan(this.txtCacheSoundSize, 2);
            this.txtCacheSoundSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheSoundSize.Location = new System.Drawing.Point(132, 55);
            this.txtCacheSoundSize.Name = "txtCacheSoundSize";
            this.txtCacheSoundSize.ReadOnly = true;
            this.txtCacheSoundSize.Size = new System.Drawing.Size(570, 20);
            this.txtCacheSoundSize.TabIndex = 13;
            this.txtCacheSoundSize.Text = "0";
            this.txtCacheSoundSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCacheSoundSize
            // 
            this.lblCacheSoundSize.AutoEllipsis = true;
            this.lblCacheSoundSize.AutoSize = true;
            this.lblCacheSoundSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheSoundSize.Location = new System.Drawing.Point(3, 52);
            this.lblCacheSoundSize.Name = "lblCacheSoundSize";
            this.lblCacheSoundSize.Size = new System.Drawing.Size(123, 26);
            this.lblCacheSoundSize.TabIndex = 14;
            this.lblCacheSoundSize.Text = "Current disk size in bytes";
            this.lblCacheSoundSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheSoundCount
            // 
            this.lblCacheSoundCount.AutoEllipsis = true;
            this.lblCacheSoundCount.AutoSize = true;
            this.lblCacheSoundCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheSoundCount.Location = new System.Drawing.Point(3, 26);
            this.lblCacheSoundCount.Name = "lblCacheSoundCount";
            this.lblCacheSoundCount.Size = new System.Drawing.Size(123, 26);
            this.lblCacheSoundCount.TabIndex = 15;
            this.lblCacheSoundCount.Text = "Current item count";
            this.lblCacheSoundCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheSoundExpiry
            // 
            this.lblCacheSoundExpiry.AutoEllipsis = true;
            this.lblCacheSoundExpiry.AutoSize = true;
            this.lblCacheSoundExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheSoundExpiry.Location = new System.Drawing.Point(3, 0);
            this.lblCacheSoundExpiry.Name = "lblCacheSoundExpiry";
            this.lblCacheSoundExpiry.Size = new System.Drawing.Size(123, 26);
            this.lblCacheSoundExpiry.TabIndex = 16;
            this.lblCacheSoundExpiry.Text = "Expiration in minutes";
            this.lblCacheSoundExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudCacheSoundExpiry
            // 
            this.tableCacheSound.SetColumnSpan(this.nudCacheSoundExpiry, 2);
            this.nudCacheSoundExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCacheSoundExpiry.Location = new System.Drawing.Point(132, 3);
            this.nudCacheSoundExpiry.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudCacheSoundExpiry.Name = "nudCacheSoundExpiry";
            this.nudCacheSoundExpiry.Size = new System.Drawing.Size(570, 20);
            this.nudCacheSoundExpiry.TabIndex = 17;
            this.nudCacheSoundExpiry.TabStop = false;
            this.nudCacheSoundExpiry.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudCacheSoundExpiry.Value = new decimal(new int[] {
            518400,
            0,
            0,
            0});
            // 
            // txtCacheSoundCount
            // 
            this.tableCacheSound.SetColumnSpan(this.txtCacheSoundCount, 2);
            this.txtCacheSoundCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheSoundCount.Location = new System.Drawing.Point(132, 29);
            this.txtCacheSoundCount.Name = "txtCacheSoundCount";
            this.txtCacheSoundCount.ReadOnly = true;
            this.txtCacheSoundCount.Size = new System.Drawing.Size(570, 20);
            this.txtCacheSoundCount.TabIndex = 11;
            this.txtCacheSoundCount.Text = "0";
            this.txtCacheSoundCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pnlCacheSoundSpacing
            // 
            this.pnlCacheSoundSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCacheSoundSpacing.Location = new System.Drawing.Point(0, 140);
            this.pnlCacheSoundSpacing.Name = "pnlCacheSoundSpacing";
            this.pnlCacheSoundSpacing.Size = new System.Drawing.Size(725, 10);
            this.pnlCacheSoundSpacing.TabIndex = 7;
            // 
            // grpCacheImage
            // 
            this.grpCacheImage.AutoSize = true;
            this.grpCacheImage.Controls.Add(this.tableCacheImage);
            this.grpCacheImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCacheImage.Location = new System.Drawing.Point(0, 0);
            this.grpCacheImage.Name = "grpCacheImage";
            this.grpCacheImage.Padding = new System.Windows.Forms.Padding(10);
            this.grpCacheImage.Size = new System.Drawing.Size(725, 140);
            this.grpCacheImage.TabIndex = 8;
            this.grpCacheImage.TabStop = false;
            this.grpCacheImage.Text = " Image files ";
            // 
            // tableCacheImage
            // 
            this.tableCacheImage.AutoSize = true;
            this.tableCacheImage.ColumnCount = 3;
            this.tableCacheImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableCacheImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCacheImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableCacheImage.Controls.Add(this.btnCacheImageBrowse, 2, 3);
            this.tableCacheImage.Controls.Add(this.txtCacheImageSize, 1, 2);
            this.tableCacheImage.Controls.Add(this.lblCacheImageSize, 0, 2);
            this.tableCacheImage.Controls.Add(this.lblCacheImageCount, 0, 1);
            this.tableCacheImage.Controls.Add(this.lblCacheImageExpiry, 0, 0);
            this.tableCacheImage.Controls.Add(this.nudCacheImageExpiry, 1, 0);
            this.tableCacheImage.Controls.Add(this.txtCacheImageCount, 1, 1);
            this.tableCacheImage.Controls.Add(this.btnCacheImageClear, 1, 3);
            this.tableCacheImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCacheImage.Location = new System.Drawing.Point(10, 23);
            this.tableCacheImage.Name = "tableCacheImage";
            this.tableCacheImage.RowCount = 4;
            this.tableCacheImage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheImage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheImage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheImage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableCacheImage.Size = new System.Drawing.Size(705, 107);
            this.tableCacheImage.TabIndex = 0;
            // 
            // btnCacheImageBrowse
            // 
            this.btnCacheImageBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheImageBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheImageBrowse.Image")));
            this.btnCacheImageBrowse.Location = new System.Drawing.Point(420, 81);
            this.btnCacheImageBrowse.Name = "btnCacheImageBrowse";
            this.btnCacheImageBrowse.Size = new System.Drawing.Size(282, 23);
            this.btnCacheImageBrowse.TabIndex = 0;
            this.btnCacheImageBrowse.TabStop = false;
            this.btnCacheImageBrowse.Text = "Browse";
            this.btnCacheImageBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCacheImageBrowse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCacheImageBrowse.UseMnemonic = false;
            this.btnCacheImageBrowse.UseVisualStyleBackColor = true;
            this.btnCacheImageBrowse.Click += new System.EventHandler(this.btnCacheImageBrowse_Click);
            // 
            // txtCacheImageSize
            // 
            this.tableCacheImage.SetColumnSpan(this.txtCacheImageSize, 2);
            this.txtCacheImageSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheImageSize.Location = new System.Drawing.Point(132, 55);
            this.txtCacheImageSize.Name = "txtCacheImageSize";
            this.txtCacheImageSize.ReadOnly = true;
            this.txtCacheImageSize.Size = new System.Drawing.Size(570, 20);
            this.txtCacheImageSize.TabIndex = 13;
            this.txtCacheImageSize.Text = "0";
            this.txtCacheImageSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCacheImageSize
            // 
            this.lblCacheImageSize.AutoEllipsis = true;
            this.lblCacheImageSize.AutoSize = true;
            this.lblCacheImageSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheImageSize.Location = new System.Drawing.Point(3, 52);
            this.lblCacheImageSize.Name = "lblCacheImageSize";
            this.lblCacheImageSize.Size = new System.Drawing.Size(123, 26);
            this.lblCacheImageSize.TabIndex = 14;
            this.lblCacheImageSize.Text = "Current disk size in bytes";
            this.lblCacheImageSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheImageCount
            // 
            this.lblCacheImageCount.AutoEllipsis = true;
            this.lblCacheImageCount.AutoSize = true;
            this.lblCacheImageCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheImageCount.Location = new System.Drawing.Point(3, 26);
            this.lblCacheImageCount.Name = "lblCacheImageCount";
            this.lblCacheImageCount.Size = new System.Drawing.Size(123, 26);
            this.lblCacheImageCount.TabIndex = 15;
            this.lblCacheImageCount.Text = "Current item count";
            this.lblCacheImageCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheImageExpiry
            // 
            this.lblCacheImageExpiry.AutoEllipsis = true;
            this.lblCacheImageExpiry.AutoSize = true;
            this.lblCacheImageExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheImageExpiry.Location = new System.Drawing.Point(3, 0);
            this.lblCacheImageExpiry.Name = "lblCacheImageExpiry";
            this.lblCacheImageExpiry.Size = new System.Drawing.Size(123, 26);
            this.lblCacheImageExpiry.TabIndex = 16;
            this.lblCacheImageExpiry.Text = "Expiration in minutes";
            this.lblCacheImageExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudCacheImageExpiry
            // 
            this.tableCacheImage.SetColumnSpan(this.nudCacheImageExpiry, 2);
            this.nudCacheImageExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCacheImageExpiry.Location = new System.Drawing.Point(132, 3);
            this.nudCacheImageExpiry.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudCacheImageExpiry.Name = "nudCacheImageExpiry";
            this.nudCacheImageExpiry.Size = new System.Drawing.Size(570, 20);
            this.nudCacheImageExpiry.TabIndex = 17;
            this.nudCacheImageExpiry.TabStop = false;
            this.nudCacheImageExpiry.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudCacheImageExpiry.Value = new decimal(new int[] {
            518400,
            0,
            0,
            0});
            // 
            // txtCacheImageCount
            // 
            this.tableCacheImage.SetColumnSpan(this.txtCacheImageCount, 2);
            this.txtCacheImageCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheImageCount.Location = new System.Drawing.Point(132, 29);
            this.txtCacheImageCount.Name = "txtCacheImageCount";
            this.txtCacheImageCount.ReadOnly = true;
            this.txtCacheImageCount.Size = new System.Drawing.Size(570, 20);
            this.txtCacheImageCount.TabIndex = 11;
            this.txtCacheImageCount.Text = "0";
            this.txtCacheImageCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnCacheImageClear
            // 
            this.btnCacheImageClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheImageClear.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheImageClear.Image")));
            this.btnCacheImageClear.Location = new System.Drawing.Point(132, 81);
            this.btnCacheImageClear.Name = "btnCacheImageClear";
            this.btnCacheImageClear.Size = new System.Drawing.Size(282, 23);
            this.btnCacheImageClear.TabIndex = 18;
            this.btnCacheImageClear.TabStop = false;
            this.btnCacheImageClear.Text = "Clear cache";
            this.btnCacheImageClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCacheImageClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCacheImageClear.UseMnemonic = false;
            this.btnCacheImageClear.UseVisualStyleBackColor = true;
            this.btnCacheImageClear.Click += new System.EventHandler(this.btnCacheImageClear_Click);
            // 
            // tabEndpoint
            // 
            this.tabEndpoint.Controls.Add(this.grpEndpointHistory);
            this.tabEndpoint.Controls.Add(this.pnlEndpointHistorySpacing);
            this.tabEndpoint.Controls.Add(this.grpEndpointSettings);
            this.tabEndpoint.Controls.Add(this.pnlEndpointStatusSpacing);
            this.tabEndpoint.Controls.Add(this.grpEndpointState);
            this.tabEndpoint.Location = new System.Drawing.Point(4, 22);
            this.tabEndpoint.Name = "tabEndpoint";
            this.tabEndpoint.Padding = new System.Windows.Forms.Padding(7);
            this.tabEndpoint.Size = new System.Drawing.Size(756, 570);
            this.tabEndpoint.TabIndex = 5;
            this.tabEndpoint.Text = "Endpoint";
            this.tabEndpoint.UseVisualStyleBackColor = true;
            // 
            // grpEndpointHistory
            // 
            this.grpEndpointHistory.AutoSize = true;
            this.grpEndpointHistory.Controls.Add(this.pnlEndpointHistory);
            this.grpEndpointHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEndpointHistory.Location = new System.Drawing.Point(7, 242);
            this.grpEndpointHistory.Name = "grpEndpointHistory";
            this.grpEndpointHistory.Padding = new System.Windows.Forms.Padding(10);
            this.grpEndpointHistory.Size = new System.Drawing.Size(742, 321);
            this.grpEndpointHistory.TabIndex = 0;
            this.grpEndpointHistory.TabStop = false;
            this.grpEndpointHistory.Text = " Endpoint history ";
            // 
            // pnlEndpointHistory
            // 
            this.pnlEndpointHistory.Controls.Add(this.dgvEndpointHistory);
            this.pnlEndpointHistory.Controls.Add(this.tlsEndpointHistory);
            this.pnlEndpointHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEndpointHistory.Location = new System.Drawing.Point(10, 23);
            this.pnlEndpointHistory.Name = "pnlEndpointHistory";
            this.pnlEndpointHistory.Size = new System.Drawing.Size(722, 288);
            this.pnlEndpointHistory.TabIndex = 0;
            // 
            // dgvEndpointHistory
            // 
            this.dgvEndpointHistory.AllowUserToAddRows = false;
            this.dgvEndpointHistory.AllowUserToDeleteRows = false;
            this.dgvEndpointHistory.AllowUserToResizeColumns = false;
            this.dgvEndpointHistory.AllowUserToResizeRows = false;
            this.dgvEndpointHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEndpointHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEndpointHistoryTimestamp,
            this.colEndpointHistoryData});
            this.dgvEndpointHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEndpointHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvEndpointHistory.Location = new System.Drawing.Point(0, 25);
            this.dgvEndpointHistory.Name = "dgvEndpointHistory";
            this.dgvEndpointHistory.RowHeadersVisible = false;
            this.dgvEndpointHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEndpointHistory.Size = new System.Drawing.Size(722, 263);
            this.dgvEndpointHistory.TabIndex = 0;
            this.dgvEndpointHistory.TabStop = false;
            this.dgvEndpointHistory.VirtualMode = true;
            this.dgvEndpointHistory.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvEndpointHistory_CellValueNeeded);
            // 
            // colEndpointHistoryTimestamp
            // 
            this.colEndpointHistoryTimestamp.Frozen = true;
            this.colEndpointHistoryTimestamp.HeaderText = "Timestamp";
            this.colEndpointHistoryTimestamp.Name = "colEndpointHistoryTimestamp";
            this.colEndpointHistoryTimestamp.ReadOnly = true;
            this.colEndpointHistoryTimestamp.Width = 150;
            // 
            // colEndpointHistoryData
            // 
            this.colEndpointHistoryData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colEndpointHistoryData.HeaderText = "Data";
            this.colEndpointHistoryData.Name = "colEndpointHistoryData";
            this.colEndpointHistoryData.ReadOnly = true;
            // 
            // tlsEndpointHistory
            // 
            this.tlsEndpointHistory.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsEndpointHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEndpointHistUpdate,
            this.tlsEndpointHistoryRecv,
            this.tlsEndpointHistoryCount});
            this.tlsEndpointHistory.Location = new System.Drawing.Point(0, 0);
            this.tlsEndpointHistory.Name = "tlsEndpointHistory";
            this.tlsEndpointHistory.Size = new System.Drawing.Size(722, 25);
            this.tlsEndpointHistory.TabIndex = 1;
            this.tlsEndpointHistory.Text = "tlsEndpointHistory";
            // 
            // btnEndpointHistUpdate
            // 
            this.btnEndpointHistUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnEndpointHistUpdate.Image")));
            this.btnEndpointHistUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEndpointHistUpdate.Name = "btnEndpointHistUpdate";
            this.btnEndpointHistUpdate.Size = new System.Drawing.Size(65, 22);
            this.btnEndpointHistUpdate.Text = "Update";
            this.btnEndpointHistUpdate.Click += new System.EventHandler(this.btnEndpointHistUpdate_Click);
            // 
            // tlsEndpointHistoryRecv
            // 
            this.tlsEndpointHistoryRecv.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tlsEndpointHistoryRecv.Name = "tlsEndpointHistoryRecv";
            this.tlsEndpointHistoryRecv.Size = new System.Drawing.Size(114, 22);
            this.tlsEndpointHistoryRecv.Text = "telegram(s) received";
            // 
            // tlsEndpointHistoryCount
            // 
            this.tlsEndpointHistoryCount.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tlsEndpointHistoryCount.Name = "tlsEndpointHistoryCount";
            this.tlsEndpointHistoryCount.Size = new System.Drawing.Size(13, 22);
            this.tlsEndpointHistoryCount.Text = "0";
            // 
            // pnlEndpointHistorySpacing
            // 
            this.pnlEndpointHistorySpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEndpointHistorySpacing.Location = new System.Drawing.Point(7, 232);
            this.pnlEndpointHistorySpacing.Name = "pnlEndpointHistorySpacing";
            this.pnlEndpointHistorySpacing.Size = new System.Drawing.Size(742, 10);
            this.pnlEndpointHistorySpacing.TabIndex = 1;
            // 
            // grpEndpointSettings
            // 
            this.grpEndpointSettings.AutoSize = true;
            this.grpEndpointSettings.Controls.Add(this.tableEndpointSettings);
            this.grpEndpointSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpEndpointSettings.Location = new System.Drawing.Point(7, 127);
            this.grpEndpointSettings.Name = "grpEndpointSettings";
            this.grpEndpointSettings.Padding = new System.Windows.Forms.Padding(10);
            this.grpEndpointSettings.Size = new System.Drawing.Size(742, 105);
            this.grpEndpointSettings.TabIndex = 2;
            this.grpEndpointSettings.TabStop = false;
            this.grpEndpointSettings.Text = " Endpoint settings ";
            // 
            // tableEndpointSettings
            // 
            this.tableEndpointSettings.AutoSize = true;
            this.tableEndpointSettings.ColumnCount = 2;
            this.tableEndpointSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableEndpointSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableEndpointSettings.Controls.Add(this.chkEndpointStartup, 0, 1);
            this.tableEndpointSettings.Controls.Add(this.chkEndpointLog, 0, 2);
            this.tableEndpointSettings.Controls.Add(this.txtEndpoint, 1, 0);
            this.tableEndpointSettings.Controls.Add(this.lblEndpoint, 0, 0);
            this.tableEndpointSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableEndpointSettings.Location = new System.Drawing.Point(10, 23);
            this.tableEndpointSettings.Name = "tableEndpointSettings";
            this.tableEndpointSettings.RowCount = 3;
            this.tableEndpointSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEndpointSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEndpointSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEndpointSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableEndpointSettings.Size = new System.Drawing.Size(722, 72);
            this.tableEndpointSettings.TabIndex = 0;
            // 
            // chkEndpointStartup
            // 
            this.chkEndpointStartup.AutoSize = true;
            this.chkEndpointStartup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableEndpointSettings.SetColumnSpan(this.chkEndpointStartup, 3);
            this.chkEndpointStartup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkEndpointStartup.Location = new System.Drawing.Point(3, 29);
            this.chkEndpointStartup.Name = "chkEndpointStartup";
            this.chkEndpointStartup.Size = new System.Drawing.Size(716, 17);
            this.chkEndpointStartup.TabIndex = 0;
            this.chkEndpointStartup.TabStop = false;
            this.chkEndpointStartup.Text = "Start endpoint on launch";
            this.chkEndpointStartup.UseVisualStyleBackColor = true;
            // 
            // chkEndpointLog
            // 
            this.chkEndpointLog.AutoSize = true;
            this.chkEndpointLog.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableEndpointSettings.SetColumnSpan(this.chkEndpointLog, 3);
            this.chkEndpointLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkEndpointLog.Location = new System.Drawing.Point(3, 52);
            this.chkEndpointLog.Name = "chkEndpointLog";
            this.chkEndpointLog.Size = new System.Drawing.Size(716, 17);
            this.chkEndpointLog.TabIndex = 1;
            this.chkEndpointLog.TabStop = false;
            this.chkEndpointLog.Text = "Log received data";
            this.chkEndpointLog.UseVisualStyleBackColor = true;
            // 
            // txtEndpoint
            // 
            this.txtEndpoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEndpoint.Location = new System.Drawing.Point(89, 3);
            this.txtEndpoint.Name = "txtEndpoint";
            this.txtEndpoint.Size = new System.Drawing.Size(630, 20);
            this.txtEndpoint.TabIndex = 7;
            // 
            // lblEndpoint
            // 
            this.lblEndpoint.AutoEllipsis = true;
            this.lblEndpoint.AutoSize = true;
            this.lblEndpoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEndpoint.Location = new System.Drawing.Point(3, 0);
            this.lblEndpoint.Name = "lblEndpoint";
            this.lblEndpoint.Size = new System.Drawing.Size(80, 26);
            this.lblEndpoint.TabIndex = 8;
            this.lblEndpoint.Text = "HTTP endpoint";
            this.lblEndpoint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlEndpointStatusSpacing
            // 
            this.pnlEndpointStatusSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEndpointStatusSpacing.Location = new System.Drawing.Point(7, 117);
            this.pnlEndpointStatusSpacing.Name = "pnlEndpointStatusSpacing";
            this.pnlEndpointStatusSpacing.Size = new System.Drawing.Size(742, 10);
            this.pnlEndpointStatusSpacing.TabIndex = 3;
            // 
            // grpEndpointState
            // 
            this.grpEndpointState.AutoSize = true;
            this.grpEndpointState.Controls.Add(this.tableEndpointStatus);
            this.grpEndpointState.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpEndpointState.Location = new System.Drawing.Point(7, 7);
            this.grpEndpointState.Name = "grpEndpointState";
            this.grpEndpointState.Padding = new System.Windows.Forms.Padding(10);
            this.grpEndpointState.Size = new System.Drawing.Size(742, 110);
            this.grpEndpointState.TabIndex = 4;
            this.grpEndpointState.TabStop = false;
            this.grpEndpointState.Text = " Status ";
            // 
            // tableEndpointStatus
            // 
            this.tableEndpointStatus.ColumnCount = 2;
            this.tableEndpointStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableEndpointStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableEndpointStatus.Controls.Add(this.btnEndpointStart, 1, 0);
            this.tableEndpointStatus.Controls.Add(this.btnEndpointStop, 1, 1);
            this.tableEndpointStatus.Controls.Add(this.txtEndpointStatus, 0, 0);
            this.tableEndpointStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableEndpointStatus.Location = new System.Drawing.Point(10, 23);
            this.tableEndpointStatus.Name = "tableEndpointStatus";
            this.tableEndpointStatus.RowCount = 2;
            this.tableEndpointStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableEndpointStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableEndpointStatus.Size = new System.Drawing.Size(722, 77);
            this.tableEndpointStatus.TabIndex = 0;
            // 
            // btnEndpointStart
            // 
            this.btnEndpointStart.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEndpointStart.Image = ((System.Drawing.Image)(resources.GetObject("btnEndpointStart.Image")));
            this.btnEndpointStart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEndpointStart.Location = new System.Drawing.Point(639, 3);
            this.btnEndpointStart.Name = "btnEndpointStart";
            this.btnEndpointStart.Size = new System.Drawing.Size(80, 32);
            this.btnEndpointStart.TabIndex = 0;
            this.btnEndpointStart.TabStop = false;
            this.btnEndpointStart.Text = "Start";
            this.btnEndpointStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEndpointStart.UseVisualStyleBackColor = true;
            this.btnEndpointStart.Click += new System.EventHandler(this.btnEndpointStart_Click);
            // 
            // btnEndpointStop
            // 
            this.btnEndpointStop.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEndpointStop.Image = ((System.Drawing.Image)(resources.GetObject("btnEndpointStop.Image")));
            this.btnEndpointStop.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEndpointStop.Location = new System.Drawing.Point(639, 41);
            this.btnEndpointStop.Name = "btnEndpointStop";
            this.btnEndpointStop.Size = new System.Drawing.Size(80, 33);
            this.btnEndpointStop.TabIndex = 1;
            this.btnEndpointStop.TabStop = false;
            this.btnEndpointStop.Text = "Stop";
            this.btnEndpointStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEndpointStop.UseVisualStyleBackColor = true;
            this.btnEndpointStop.Click += new System.EventHandler(this.btnEndpointStop_Click);
            // 
            // txtEndpointStatus
            // 
            this.txtEndpointStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEndpointStatus.Location = new System.Drawing.Point(3, 3);
            this.txtEndpointStatus.Multiline = true;
            this.txtEndpointStatus.Name = "txtEndpointStatus";
            this.txtEndpointStatus.ReadOnly = true;
            this.tableEndpointStatus.SetRowSpan(this.txtEndpointStatus, 2);
            this.txtEndpointStatus.Size = new System.Drawing.Size(630, 71);
            this.txtEndpointStatus.TabIndex = 2;
            // 
            // tabFFXIV
            // 
            this.tabFFXIV.Controls.Add(this.pnlFfxivLoggingSpacing);
            this.tabFFXIV.Controls.Add(this.grpFfxivEventLogging);
            this.tabFFXIV.Location = new System.Drawing.Point(4, 22);
            this.tabFFXIV.Name = "tabFFXIV";
            this.tabFFXIV.Padding = new System.Windows.Forms.Padding(7);
            this.tabFFXIV.Size = new System.Drawing.Size(756, 570);
            this.tabFFXIV.TabIndex = 3;
            this.tabFFXIV.Text = "Final Fantasy XIV";
            this.tabFFXIV.UseVisualStyleBackColor = true;
            // 
            // pnlFfxivLoggingSpacing
            // 
            this.pnlFfxivLoggingSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFfxivLoggingSpacing.Location = new System.Drawing.Point(7, 63);
            this.pnlFfxivLoggingSpacing.Name = "pnlFfxivLoggingSpacing";
            this.pnlFfxivLoggingSpacing.Size = new System.Drawing.Size(742, 10);
            this.pnlFfxivLoggingSpacing.TabIndex = 1;
            // 
            // grpFfxivEventLogging
            // 
            this.grpFfxivEventLogging.AutoSize = true;
            this.grpFfxivEventLogging.Controls.Add(this.tableFfxivLogging);
            this.grpFfxivEventLogging.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFfxivEventLogging.Location = new System.Drawing.Point(7, 7);
            this.grpFfxivEventLogging.Name = "grpFfxivEventLogging";
            this.grpFfxivEventLogging.Padding = new System.Windows.Forms.Padding(10);
            this.grpFfxivEventLogging.Size = new System.Drawing.Size(742, 56);
            this.grpFfxivEventLogging.TabIndex = 2;
            this.grpFfxivEventLogging.TabStop = false;
            this.grpFfxivEventLogging.Text = " Event logging ";
            // 
            // tableFfxivLogging
            // 
            this.tableFfxivLogging.AutoSize = true;
            this.tableFfxivLogging.ColumnCount = 1;
            this.tableFfxivLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableFfxivLogging.Controls.Add(this.chkFfxivLogNetwork, 0, 1);
            this.tableFfxivLogging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableFfxivLogging.Location = new System.Drawing.Point(10, 23);
            this.tableFfxivLogging.Name = "tableFfxivLogging";
            this.tableFfxivLogging.RowCount = 2;
            this.tableFfxivLogging.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableFfxivLogging.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableFfxivLogging.Size = new System.Drawing.Size(722, 23);
            this.tableFfxivLogging.TabIndex = 0;
            // 
            // chkFfxivLogNetwork
            // 
            this.chkFfxivLogNetwork.AutoSize = true;
            this.chkFfxivLogNetwork.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkFfxivLogNetwork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkFfxivLogNetwork.Location = new System.Drawing.Point(3, 3);
            this.chkFfxivLogNetwork.Name = "chkFfxivLogNetwork";
            this.chkFfxivLogNetwork.Size = new System.Drawing.Size(716, 17);
            this.chkFfxivLogNetwork.TabIndex = 0;
            this.chkFfxivLogNetwork.TabStop = false;
            this.chkFfxivLogNetwork.Text = "Log network events";
            this.chkFfxivLogNetwork.UseVisualStyleBackColor = true;
            // 
            // tabSubstitutions
            // 
            this.tabSubstitutions.Controls.Add(this.dgvSubstitutions);
            this.tabSubstitutions.Controls.Add(this.tlsSubstitutions);
            this.tabSubstitutions.Location = new System.Drawing.Point(4, 22);
            this.tabSubstitutions.Name = "tabSubstitutions";
            this.tabSubstitutions.Padding = new System.Windows.Forms.Padding(7);
            this.tabSubstitutions.Size = new System.Drawing.Size(756, 570);
            this.tabSubstitutions.TabIndex = 7;
            this.tabSubstitutions.Text = "Substitutions";
            this.tabSubstitutions.UseVisualStyleBackColor = true;
            // 
            // dgvSubstitutions
            // 
            this.dgvSubstitutions.AllowUserToAddRows = false;
            this.dgvSubstitutions.AllowUserToDeleteRows = false;
            this.dgvSubstitutions.AllowUserToResizeRows = false;
            this.dgvSubstitutions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubstitutions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSubSearchFor,
            this.colSubReplaceWith,
            this.colSubScope});
            this.dgvSubstitutions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSubstitutions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvSubstitutions.Location = new System.Drawing.Point(7, 32);
            this.dgvSubstitutions.Name = "dgvSubstitutions";
            this.dgvSubstitutions.RowHeadersVisible = false;
            this.dgvSubstitutions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSubstitutions.ShowCellErrors = false;
            this.dgvSubstitutions.ShowEditingIcon = false;
            this.dgvSubstitutions.ShowRowErrors = false;
            this.dgvSubstitutions.Size = new System.Drawing.Size(742, 531);
            this.dgvSubstitutions.TabIndex = 0;
            this.dgvSubstitutions.TabStop = false;
            this.dgvSubstitutions.VirtualMode = true;
            this.dgvSubstitutions.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSubstitutions_CellDoubleClick);
            this.dgvSubstitutions.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvSubstitutions_CellValueNeeded);
            this.dgvSubstitutions.SelectionChanged += new System.EventHandler(this.dgvSubstitutions_SelectionChanged);
            this.dgvSubstitutions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSubstitutions_KeyDown);
            // 
            // colSubSearchFor
            // 
            this.colSubSearchFor.HeaderText = "Search for";
            this.colSubSearchFor.Name = "colSubSearchFor";
            this.colSubSearchFor.ReadOnly = true;
            this.colSubSearchFor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSubSearchFor.Width = 150;
            // 
            // colSubReplaceWith
            // 
            this.colSubReplaceWith.HeaderText = "Replace with";
            this.colSubReplaceWith.Name = "colSubReplaceWith";
            this.colSubReplaceWith.ReadOnly = true;
            this.colSubReplaceWith.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSubReplaceWith.Width = 150;
            // 
            // colSubScope
            // 
            this.colSubScope.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSubScope.HeaderText = "Scope";
            this.colSubScope.Name = "colSubScope";
            this.colSubScope.ReadOnly = true;
            this.colSubScope.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tlsSubstitutions
            // 
            this.tlsSubstitutions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsSubstitutions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSubAdd,
            this.btnSubEdit,
            this.btnSubRemove});
            this.tlsSubstitutions.Location = new System.Drawing.Point(7, 7);
            this.tlsSubstitutions.Name = "tlsSubstitutions";
            this.tlsSubstitutions.Size = new System.Drawing.Size(742, 25);
            this.tlsSubstitutions.TabIndex = 1;
            // 
            // btnSubAdd
            // 
            this.btnSubAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnSubAdd.Image")));
            this.btnSubAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSubAdd.Name = "btnSubAdd";
            this.btnSubAdd.Size = new System.Drawing.Size(115, 22);
            this.btnSubAdd.Text = "Add substitution";
            this.btnSubAdd.Click += new System.EventHandler(this.btnSubAdd_Click);
            // 
            // btnSubEdit
            // 
            this.btnSubEdit.Enabled = false;
            this.btnSubEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnSubEdit.Image")));
            this.btnSubEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSubEdit.Name = "btnSubEdit";
            this.btnSubEdit.Size = new System.Drawing.Size(113, 22);
            this.btnSubEdit.Text = "Edit substitution";
            this.btnSubEdit.Click += new System.EventHandler(this.btnSubEdit_Click);
            // 
            // btnSubRemove
            // 
            this.btnSubRemove.Enabled = false;
            this.btnSubRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnSubRemove.Image")));
            this.btnSubRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSubRemove.Name = "btnSubRemove";
            this.btnSubRemove.Size = new System.Drawing.Size(136, 22);
            this.btnSubRemove.Text = "Remove substitution";
            this.btnSubRemove.Click += new System.EventHandler(this.btnSubRemove_Click);
            // 
            // tabConsts
            // 
            this.tabConsts.Controls.Add(this.pnlConstants);
            this.tabConsts.Location = new System.Drawing.Point(4, 22);
            this.tabConsts.Name = "tabConsts";
            this.tabConsts.Padding = new System.Windows.Forms.Padding(7);
            this.tabConsts.Size = new System.Drawing.Size(756, 570);
            this.tabConsts.TabIndex = 9;
            this.tabConsts.Text = "Constants";
            this.tabConsts.UseVisualStyleBackColor = true;
            // 
            // pnlConstants
            // 
            this.pnlConstants.Controls.Add(this.dgvConstVariables);
            this.pnlConstants.Controls.Add(this.tlsScalar);
            this.pnlConstants.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConstants.Location = new System.Drawing.Point(7, 7);
            this.pnlConstants.Name = "pnlConstants";
            this.pnlConstants.Size = new System.Drawing.Size(742, 556);
            this.pnlConstants.TabIndex = 0;
            // 
            // dgvConstVariables
            // 
            this.dgvConstVariables.AllowUserToAddRows = false;
            this.dgvConstVariables.AllowUserToDeleteRows = false;
            this.dgvConstVariables.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvConstVariables.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvConstVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConstVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colScalarName,
            this.colScalarValue});
            this.dgvConstVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvConstVariables.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvConstVariables.Location = new System.Drawing.Point(0, 25);
            this.dgvConstVariables.Name = "dgvConstVariables";
            this.dgvConstVariables.RowHeadersVisible = false;
            this.dgvConstVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConstVariables.ShowCellErrors = false;
            this.dgvConstVariables.ShowEditingIcon = false;
            this.dgvConstVariables.ShowRowErrors = false;
            this.dgvConstVariables.Size = new System.Drawing.Size(742, 531);
            this.dgvConstVariables.TabIndex = 0;
            this.dgvConstVariables.TabStop = false;
            this.dgvConstVariables.VirtualMode = true;
            this.dgvConstVariables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConstVariables_CellDoubleClick);
            this.dgvConstVariables.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvConstVariables_CellValueNeeded);
            this.dgvConstVariables.SelectionChanged += new System.EventHandler(this.dgvConstVariables_SelectionChanged);
            this.dgvConstVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvConstVariables_KeyDown);
            // 
            // colScalarName
            // 
            this.colScalarName.Frozen = true;
            this.colScalarName.HeaderText = "Name";
            this.colScalarName.Name = "colScalarName";
            this.colScalarName.ReadOnly = true;
            this.colScalarName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colScalarName.Width = 200;
            // 
            // colScalarValue
            // 
            this.colScalarValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colScalarValue.HeaderText = "Value";
            this.colScalarValue.Name = "colScalarValue";
            this.colScalarValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tlsScalar
            // 
            this.tlsScalar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsScalar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConstAdd,
            this.btnConstEdit,
            this.sepConstantRemove,
            this.btnConstRemove});
            this.tlsScalar.Location = new System.Drawing.Point(0, 0);
            this.tlsScalar.Name = "tlsScalar";
            this.tlsScalar.Size = new System.Drawing.Size(742, 25);
            this.tlsScalar.TabIndex = 1;
            // 
            // btnConstAdd
            // 
            this.btnConstAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnConstAdd.Image")));
            this.btnConstAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConstAdd.Name = "btnConstAdd";
            this.btnConstAdd.Size = new System.Drawing.Size(98, 22);
            this.btnConstAdd.Text = "Add constant";
            this.btnConstAdd.Click += new System.EventHandler(this.btnConstAdd_Click);
            // 
            // btnConstEdit
            // 
            this.btnConstEdit.Enabled = false;
            this.btnConstEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnConstEdit.Image")));
            this.btnConstEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConstEdit.Name = "btnConstEdit";
            this.btnConstEdit.Size = new System.Drawing.Size(96, 22);
            this.btnConstEdit.Text = "Edit constant";
            this.btnConstEdit.Click += new System.EventHandler(this.btnConstEdit_Click);
            // 
            // sepConstantRemove
            // 
            this.sepConstantRemove.Name = "sepConstantRemove";
            this.sepConstantRemove.Size = new System.Drawing.Size(6, 25);
            // 
            // btnConstRemove
            // 
            this.btnConstRemove.Enabled = false;
            this.btnConstRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnConstRemove.Image")));
            this.btnConstRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConstRemove.Name = "btnConstRemove";
            this.btnConstRemove.Size = new System.Drawing.Size(119, 22);
            this.btnConstRemove.Text = "Remove constant";
            this.btnConstRemove.ToolTipText = "Remove value";
            this.btnConstRemove.Click += new System.EventHandler(this.btnConstRemove_Click);
            // 
            // tabSecurity
            // 
            this.tabSecurity.Controls.Add(this.grpScriptingAccess);
            this.tabSecurity.Location = new System.Drawing.Point(4, 22);
            this.tabSecurity.Name = "tabSecurity";
            this.tabSecurity.Padding = new System.Windows.Forms.Padding(7);
            this.tabSecurity.Size = new System.Drawing.Size(756, 570);
            this.tabSecurity.TabIndex = 8;
            this.tabSecurity.Text = "Security";
            this.tabSecurity.UseVisualStyleBackColor = true;
            // 
            // grpScriptingAccess
            // 
            this.grpScriptingAccess.AutoSize = true;
            this.grpScriptingAccess.Controls.Add(this.dgvAdditionalFeatures);
            this.grpScriptingAccess.Controls.Add(this.pnlSecurityFeaturesSpacing);
            this.grpScriptingAccess.Controls.Add(this.dgvApiAccess);
            this.grpScriptingAccess.Controls.Add(this.pnlSecurityUnlockSpacing);
            this.grpScriptingAccess.Controls.Add(this.btnUnlockSecurity);
            this.grpScriptingAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpScriptingAccess.Location = new System.Drawing.Point(7, 7);
            this.grpScriptingAccess.Name = "grpScriptingAccess";
            this.grpScriptingAccess.Padding = new System.Windows.Forms.Padding(10);
            this.grpScriptingAccess.Size = new System.Drawing.Size(742, 556);
            this.grpScriptingAccess.TabIndex = 0;
            this.grpScriptingAccess.TabStop = false;
            this.grpScriptingAccess.Text = " Scripting API access ";
            // 
            // dgvAdditionalFeatures
            // 
            this.dgvAdditionalFeatures.AllowUserToAddRows = false;
            this.dgvAdditionalFeatures.AllowUserToDeleteRows = false;
            this.dgvAdditionalFeatures.AllowUserToResizeColumns = false;
            this.dgvAdditionalFeatures.AllowUserToResizeRows = false;
            this.dgvAdditionalFeatures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdditionalFeatures.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFeatureName,
            this.colFeatureAllowLocal,
            this.colFeatureAllowRemote,
            this.colFeatureAllowAdmin});
            this.dgvAdditionalFeatures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAdditionalFeatures.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvAdditionalFeatures.Enabled = false;
            this.dgvAdditionalFeatures.Location = new System.Drawing.Point(10, 357);
            this.dgvAdditionalFeatures.Name = "dgvAdditionalFeatures";
            this.dgvAdditionalFeatures.ReadOnly = true;
            this.dgvAdditionalFeatures.RowHeadersVisible = false;
            this.dgvAdditionalFeatures.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdditionalFeatures.ShowCellErrors = false;
            this.dgvAdditionalFeatures.ShowEditingIcon = false;
            this.dgvAdditionalFeatures.ShowRowErrors = false;
            this.dgvAdditionalFeatures.Size = new System.Drawing.Size(722, 189);
            this.dgvAdditionalFeatures.TabIndex = 0;
            this.dgvAdditionalFeatures.TabStop = false;
            this.dgvAdditionalFeatures.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAdditionalFeatures_CellContentClick);
            this.dgvAdditionalFeatures.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAdditionalFeatures_CellContentDoubleClick);
            this.dgvAdditionalFeatures.SelectionChanged += new System.EventHandler(this.dgvAdditionalFeatures_SelectionChanged);
            // 
            // colFeatureName
            // 
            this.colFeatureName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFeatureName.HeaderText = "Additional features";
            this.colFeatureName.Name = "colFeatureName";
            this.colFeatureName.ReadOnly = true;
            this.colFeatureName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colFeatureName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colFeatureAllowLocal
            // 
            this.colFeatureAllowLocal.HeaderText = "Allow for local triggers";
            this.colFeatureAllowLocal.Name = "colFeatureAllowLocal";
            this.colFeatureAllowLocal.ReadOnly = true;
            this.colFeatureAllowLocal.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // colFeatureAllowRemote
            // 
            this.colFeatureAllowRemote.HeaderText = "Allow for remote triggers";
            this.colFeatureAllowRemote.Name = "colFeatureAllowRemote";
            this.colFeatureAllowRemote.ReadOnly = true;
            this.colFeatureAllowRemote.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // colFeatureAllowAdmin
            // 
            this.colFeatureAllowAdmin.HeaderText = "Allow if running as admin";
            this.colFeatureAllowAdmin.Name = "colFeatureAllowAdmin";
            this.colFeatureAllowAdmin.ReadOnly = true;
            this.colFeatureAllowAdmin.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // pnlSecurityFeaturesSpacing
            // 
            this.pnlSecurityFeaturesSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSecurityFeaturesSpacing.Location = new System.Drawing.Point(10, 347);
            this.pnlSecurityFeaturesSpacing.Name = "pnlSecurityFeaturesSpacing";
            this.pnlSecurityFeaturesSpacing.Size = new System.Drawing.Size(722, 10);
            this.pnlSecurityFeaturesSpacing.TabIndex = 1;
            // 
            // dgvApiAccess
            // 
            this.dgvApiAccess.AllowUserToAddRows = false;
            this.dgvApiAccess.AllowUserToDeleteRows = false;
            this.dgvApiAccess.AllowUserToResizeColumns = false;
            this.dgvApiAccess.AllowUserToResizeRows = false;
            this.dgvApiAccess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvApiAccess.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colApiNamespace,
            this.colApiAllowLocal,
            this.colApiAllowRemote,
            this.colApiAllowAdmin});
            this.dgvApiAccess.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvApiAccess.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvApiAccess.Enabled = false;
            this.dgvApiAccess.Location = new System.Drawing.Point(10, 63);
            this.dgvApiAccess.Name = "dgvApiAccess";
            this.dgvApiAccess.ReadOnly = true;
            this.dgvApiAccess.RowHeadersVisible = false;
            this.dgvApiAccess.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvApiAccess.ShowCellErrors = false;
            this.dgvApiAccess.ShowEditingIcon = false;
            this.dgvApiAccess.ShowRowErrors = false;
            this.dgvApiAccess.Size = new System.Drawing.Size(722, 284);
            this.dgvApiAccess.TabIndex = 2;
            this.dgvApiAccess.TabStop = false;
            this.dgvApiAccess.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvApiAccess_CellContentClick);
            this.dgvApiAccess.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvApiAccess_CellContentDoubleClick);
            this.dgvApiAccess.SelectionChanged += new System.EventHandler(this.dgvApiAccess_SelectionChanged);
            // 
            // colApiNamespace
            // 
            this.colApiNamespace.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colApiNamespace.HeaderText = "Namespace";
            this.colApiNamespace.Name = "colApiNamespace";
            this.colApiNamespace.ReadOnly = true;
            this.colApiNamespace.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colApiNamespace.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colApiAllowLocal
            // 
            this.colApiAllowLocal.HeaderText = "Allow for local triggers";
            this.colApiAllowLocal.Name = "colApiAllowLocal";
            this.colApiAllowLocal.ReadOnly = true;
            this.colApiAllowLocal.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // colApiAllowRemote
            // 
            this.colApiAllowRemote.HeaderText = "Allow for remote triggers";
            this.colApiAllowRemote.Name = "colApiAllowRemote";
            this.colApiAllowRemote.ReadOnly = true;
            this.colApiAllowRemote.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // colApiAllowAdmin
            // 
            this.colApiAllowAdmin.HeaderText = "Allow if running as admin";
            this.colApiAllowAdmin.Name = "colApiAllowAdmin";
            this.colApiAllowAdmin.ReadOnly = true;
            this.colApiAllowAdmin.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // pnlSecurityUnlockSpacing
            // 
            this.pnlSecurityUnlockSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSecurityUnlockSpacing.Location = new System.Drawing.Point(10, 53);
            this.pnlSecurityUnlockSpacing.Name = "pnlSecurityUnlockSpacing";
            this.pnlSecurityUnlockSpacing.Size = new System.Drawing.Size(722, 10);
            this.pnlSecurityUnlockSpacing.TabIndex = 3;
            // 
            // btnUnlockSecurity
            // 
            this.btnUnlockSecurity.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUnlockSecurity.Image = ((System.Drawing.Image)(resources.GetObject("btnUnlockSecurity.Image")));
            this.btnUnlockSecurity.Location = new System.Drawing.Point(10, 23);
            this.btnUnlockSecurity.Name = "btnUnlockSecurity";
            this.btnUnlockSecurity.Size = new System.Drawing.Size(722, 30);
            this.btnUnlockSecurity.TabIndex = 4;
            this.btnUnlockSecurity.TabStop = false;
            this.btnUnlockSecurity.Text = "Unlock security settings for editing";
            this.btnUnlockSecurity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUnlockSecurity.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUnlockSecurity.UseVisualStyleBackColor = true;
            this.btnUnlockSecurity.Click += new System.EventHandler(this.btnUnlockSecurity_Click);
            // 
            // tabMisc
            // 
            this.tabMisc.Controls.Add(this.grpAuraManagement);
            this.tabMisc.Controls.Add(this.pnlAuraManagementSpacing);
            this.tabMisc.Controls.Add(this.grpUserInterface);
            this.tabMisc.Controls.Add(this.pnlEventSeparatorSpacing);
            this.tabMisc.Controls.Add(this.grpFutureProofing);
            this.tabMisc.Controls.Add(this.pnlDefaultsSpacing);
            this.tabMisc.Controls.Add(this.grpDefaultSettings);
            this.tabMisc.Location = new System.Drawing.Point(4, 22);
            this.tabMisc.Name = "tabMisc";
            this.tabMisc.Padding = new System.Windows.Forms.Padding(7);
            this.tabMisc.Size = new System.Drawing.Size(756, 570);
            this.tabMisc.TabIndex = 4;
            this.tabMisc.Text = "Miscellaneous";
            this.tabMisc.UseVisualStyleBackColor = true;
            // 
            // grpAuraManagement
            // 
            this.grpAuraManagement.AutoSize = true;
            this.grpAuraManagement.Controls.Add(this.tableAuraManagement);
            this.grpAuraManagement.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpAuraManagement.Location = new System.Drawing.Point(7, 378);
            this.grpAuraManagement.Name = "grpAuraManagement";
            this.grpAuraManagement.Padding = new System.Windows.Forms.Padding(10);
            this.grpAuraManagement.Size = new System.Drawing.Size(742, 82);
            this.grpAuraManagement.TabIndex = 0;
            this.grpAuraManagement.TabStop = false;
            this.grpAuraManagement.Text = " Aura control ";
            // 
            // tableAuraManagement
            // 
            this.tableAuraManagement.AutoSize = true;
            this.tableAuraManagement.ColumnCount = 2;
            this.tableAuraManagement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAuraManagement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAuraManagement.Controls.Add(this.txtMonitorWindow, 1, 0);
            this.tableAuraManagement.Controls.Add(this.lblMonitorWindow, 0, 0);
            this.tableAuraManagement.Controls.Add(this.cbxEnableHwAccel, 0, 1);
            this.tableAuraManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableAuraManagement.Location = new System.Drawing.Point(10, 23);
            this.tableAuraManagement.Name = "tableAuraManagement";
            this.tableAuraManagement.RowCount = 2;
            this.tableAuraManagement.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAuraManagement.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAuraManagement.Size = new System.Drawing.Size(722, 49);
            this.tableAuraManagement.TabIndex = 0;
            // 
            // txtMonitorWindow
            // 
            this.txtMonitorWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMonitorWindow.Location = new System.Drawing.Point(204, 3);
            this.txtMonitorWindow.Name = "txtMonitorWindow";
            this.txtMonitorWindow.Size = new System.Drawing.Size(515, 20);
            this.txtMonitorWindow.TabIndex = 8;
            // 
            // lblMonitorWindow
            // 
            this.lblMonitorWindow.AutoEllipsis = true;
            this.lblMonitorWindow.AutoSize = true;
            this.lblMonitorWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMonitorWindow.Location = new System.Drawing.Point(3, 0);
            this.lblMonitorWindow.Name = "lblMonitorWindow";
            this.lblMonitorWindow.Size = new System.Drawing.Size(195, 26);
            this.lblMonitorWindow.TabIndex = 9;
            this.lblMonitorWindow.Text = "Monitor window for showing/hiding aura";
            this.lblMonitorWindow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxEnableHwAccel
            // 
            this.cbxEnableHwAccel.AutoSize = true;
            this.cbxEnableHwAccel.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableAuraManagement.SetColumnSpan(this.cbxEnableHwAccel, 2);
            this.cbxEnableHwAccel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxEnableHwAccel.Location = new System.Drawing.Point(3, 29);
            this.cbxEnableHwAccel.Name = "cbxEnableHwAccel";
            this.cbxEnableHwAccel.Size = new System.Drawing.Size(716, 17);
            this.cbxEnableHwAccel.TabIndex = 10;
            this.cbxEnableHwAccel.TabStop = false;
            this.cbxEnableHwAccel.Text = "Enable aura hardware acceleration";
            this.cbxEnableHwAccel.UseVisualStyleBackColor = true;
            // 
            // pnlAuraManagementSpacing
            // 
            this.pnlAuraManagementSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAuraManagementSpacing.Location = new System.Drawing.Point(7, 368);
            this.pnlAuraManagementSpacing.Name = "pnlAuraManagementSpacing";
            this.pnlAuraManagementSpacing.Size = new System.Drawing.Size(742, 10);
            this.pnlAuraManagementSpacing.TabIndex = 1;
            // 
            // grpUserInterface
            // 
            this.grpUserInterface.AutoSize = true;
            this.grpUserInterface.Controls.Add(this.tableUiOptions);
            this.grpUserInterface.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpUserInterface.Location = new System.Drawing.Point(7, 148);
            this.grpUserInterface.Name = "grpUserInterface";
            this.grpUserInterface.Padding = new System.Windows.Forms.Padding(10);
            this.grpUserInterface.Size = new System.Drawing.Size(742, 220);
            this.grpUserInterface.TabIndex = 2;
            this.grpUserInterface.TabStop = false;
            this.grpUserInterface.Text = " User interface ";
            // 
            // tableUiOptions
            // 
            this.tableUiOptions.AutoSize = true;
            this.tableUiOptions.ColumnCount = 2;
            this.tableUiOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableUiOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableUiOptions.Controls.Add(this.cbxAutoComplete, 0, 4);
            this.tableUiOptions.Controls.Add(this.cbxActionAsync, 0, 2);
            this.tableUiOptions.Controls.Add(this.chkClipboard, 0, 0);
            this.tableUiOptions.Controls.Add(this.lblAutosaveInterval, 0, 6);
            this.tableUiOptions.Controls.Add(this.cbxAutosaveConfig, 0, 5);
            this.tableUiOptions.Controls.Add(this.cbxDevMode, 0, 3);
            this.tableUiOptions.Controls.Add(this.cbxTestIgnoreConditions, 0, 1);
            this.tableUiOptions.Controls.Add(this.nudAutosaveMinutes, 1, 6);
            this.tableUiOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableUiOptions.Location = new System.Drawing.Point(10, 23);
            this.tableUiOptions.Name = "tableUiOptions";
            this.tableUiOptions.RowCount = 7;
            this.tableUiOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiOptions.Size = new System.Drawing.Size(722, 164);
            this.tableUiOptions.TabIndex = 0;
            // 
            // cbxAutoComplete
            // 
            this.cbxAutoComplete.AutoSize = true;
            this.cbxAutoComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableUiOptions.SetColumnSpan(this.cbxAutoComplete, 2);
            this.cbxAutoComplete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxAutoComplete.Location = new System.Drawing.Point(3, 95);
            this.cbxAutoComplete.Name = "cbxAutoComplete";
            this.cbxAutoComplete.Size = new System.Drawing.Size(716, 17);
            this.cbxAutoComplete.TabIndex = 8;
            this.cbxAutoComplete.TabStop = false;
            this.cbxAutoComplete.Text = "Enable autocomplete on text fields";
            this.cbxAutoComplete.UseVisualStyleBackColor = true;
            // 
            // cbxActionAsync
            // 
            this.cbxActionAsync.AutoSize = true;
            this.cbxActionAsync.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableUiOptions.SetColumnSpan(this.cbxActionAsync, 2);
            this.cbxActionAsync.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxActionAsync.Location = new System.Drawing.Point(3, 49);
            this.cbxActionAsync.Name = "cbxActionAsync";
            this.cbxActionAsync.Size = new System.Drawing.Size(716, 17);
            this.cbxActionAsync.TabIndex = 0;
            this.cbxActionAsync.TabStop = false;
            this.cbxActionAsync.Text = "New actions asynchronous by default";
            this.cbxActionAsync.UseVisualStyleBackColor = true;
            // 
            // chkClipboard
            // 
            this.chkClipboard.AutoSize = true;
            this.chkClipboard.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableUiOptions.SetColumnSpan(this.chkClipboard, 2);
            this.chkClipboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkClipboard.Location = new System.Drawing.Point(3, 3);
            this.chkClipboard.Name = "chkClipboard";
            this.chkClipboard.Size = new System.Drawing.Size(716, 17);
            this.chkClipboard.TabIndex = 1;
            this.chkClipboard.TabStop = false;
            this.chkClipboard.Text = "Use operating system clipboard";
            this.chkClipboard.UseVisualStyleBackColor = true;
            // 
            // lblAutosaveInterval
            // 
            this.lblAutosaveInterval.AutoEllipsis = true;
            this.lblAutosaveInterval.AutoSize = true;
            this.lblAutosaveInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAutosaveInterval.Location = new System.Drawing.Point(3, 138);
            this.lblAutosaveInterval.Name = "lblAutosaveInterval";
            this.lblAutosaveInterval.Size = new System.Drawing.Size(616, 26);
            this.lblAutosaveInterval.TabIndex = 2;
            this.lblAutosaveInterval.Text = "Autosaving time interval in minutes";
            this.lblAutosaveInterval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxAutosaveConfig
            // 
            this.cbxAutosaveConfig.AutoSize = true;
            this.cbxAutosaveConfig.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableUiOptions.SetColumnSpan(this.cbxAutosaveConfig, 2);
            this.cbxAutosaveConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxAutosaveConfig.Location = new System.Drawing.Point(3, 118);
            this.cbxAutosaveConfig.Name = "cbxAutosaveConfig";
            this.cbxAutosaveConfig.Size = new System.Drawing.Size(716, 17);
            this.cbxAutosaveConfig.TabIndex = 3;
            this.cbxAutosaveConfig.TabStop = false;
            this.cbxAutosaveConfig.Text = "Enable configuration auto-save";
            this.cbxAutosaveConfig.UseVisualStyleBackColor = true;
            this.cbxAutosaveConfig.CheckedChanged += new System.EventHandler(this.cbxAutosaveConfig_CheckedChanged);
            // 
            // cbxDevMode
            // 
            this.cbxDevMode.AutoSize = true;
            this.cbxDevMode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableUiOptions.SetColumnSpan(this.cbxDevMode, 2);
            this.cbxDevMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxDevMode.Location = new System.Drawing.Point(3, 72);
            this.cbxDevMode.Name = "cbxDevMode";
            this.cbxDevMode.Size = new System.Drawing.Size(716, 17);
            this.cbxDevMode.TabIndex = 4;
            this.cbxDevMode.TabStop = false;
            this.cbxDevMode.Text = "Developer mode";
            this.cbxDevMode.UseVisualStyleBackColor = true;
            // 
            // cbxTestIgnoreConditions
            // 
            this.cbxTestIgnoreConditions.AutoSize = true;
            this.cbxTestIgnoreConditions.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableUiOptions.SetColumnSpan(this.cbxTestIgnoreConditions, 2);
            this.cbxTestIgnoreConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxTestIgnoreConditions.Location = new System.Drawing.Point(3, 26);
            this.cbxTestIgnoreConditions.Name = "cbxTestIgnoreConditions";
            this.cbxTestIgnoreConditions.Size = new System.Drawing.Size(716, 17);
            this.cbxTestIgnoreConditions.TabIndex = 6;
            this.cbxTestIgnoreConditions.Text = "Ignore conditions as default when testing actions";
            this.cbxTestIgnoreConditions.UseVisualStyleBackColor = true;
            // 
            // nudAutosaveMinutes
            // 
            this.nudAutosaveMinutes.Location = new System.Drawing.Point(625, 141);
            this.nudAutosaveMinutes.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudAutosaveMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAutosaveMinutes.Name = "nudAutosaveMinutes";
            this.nudAutosaveMinutes.Size = new System.Drawing.Size(94, 20);
            this.nudAutosaveMinutes.TabIndex = 7;
            this.nudAutosaveMinutes.TabStop = false;
            this.nudAutosaveMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudAutosaveMinutes.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // pnlEventSeparatorSpacing
            // 
            this.pnlEventSeparatorSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEventSeparatorSpacing.Location = new System.Drawing.Point(7, 138);
            this.pnlEventSeparatorSpacing.Name = "pnlEventSeparatorSpacing";
            this.pnlEventSeparatorSpacing.Size = new System.Drawing.Size(742, 10);
            this.pnlEventSeparatorSpacing.TabIndex = 3;
            // 
            // pnlDefaultsSpacing
            // 
            this.pnlDefaultsSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDefaultsSpacing.Location = new System.Drawing.Point(7, 69);
            this.pnlDefaultsSpacing.Name = "pnlDefaultsSpacing";
            this.pnlDefaultsSpacing.Size = new System.Drawing.Size(742, 10);
            this.pnlDefaultsSpacing.TabIndex = 5;
            // 
            // grpDefaultSettings
            // 
            this.grpDefaultSettings.AutoSize = true;
            this.grpDefaultSettings.Controls.Add(this.tableTriggerTemplate);
            this.grpDefaultSettings.Controls.Add(this.tableDefaultsReserved);
            this.grpDefaultSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDefaultSettings.Location = new System.Drawing.Point(7, 7);
            this.grpDefaultSettings.Name = "grpDefaultSettings";
            this.grpDefaultSettings.Padding = new System.Windows.Forms.Padding(10);
            this.grpDefaultSettings.Size = new System.Drawing.Size(742, 62);
            this.grpDefaultSettings.TabIndex = 6;
            this.grpDefaultSettings.TabStop = false;
            this.grpDefaultSettings.Text = " Default settings ";
            // 
            // tableTriggerTemplate
            // 
            this.tableTriggerTemplate.AutoSize = true;
            this.tableTriggerTemplate.ColumnCount = 2;
            this.tableTriggerTemplate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableTriggerTemplate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableTriggerTemplate.Controls.Add(this.cbxTriggerTemplate, 0, 0);
            this.tableTriggerTemplate.Controls.Add(this.btnTriggerTemplate, 1, 0);
            this.tableTriggerTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableTriggerTemplate.Location = new System.Drawing.Point(10, 23);
            this.tableTriggerTemplate.Name = "tableTriggerTemplate";
            this.tableTriggerTemplate.RowCount = 2;
            this.tableTriggerTemplate.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTriggerTemplate.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTriggerTemplate.Size = new System.Drawing.Size(722, 29);
            this.tableTriggerTemplate.TabIndex = 0;
            // 
            // cbxTriggerTemplate
            // 
            this.cbxTriggerTemplate.AutoSize = true;
            this.cbxTriggerTemplate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxTriggerTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxTriggerTemplate.Location = new System.Drawing.Point(3, 3);
            this.cbxTriggerTemplate.Name = "cbxTriggerTemplate";
            this.cbxTriggerTemplate.Size = new System.Drawing.Size(516, 23);
            this.cbxTriggerTemplate.TabIndex = 0;
            this.cbxTriggerTemplate.TabStop = false;
            this.cbxTriggerTemplate.Text = "Use template trigger for default values";
            this.cbxTriggerTemplate.UseVisualStyleBackColor = true;
            // 
            // btnTriggerTemplate
            // 
            this.btnTriggerTemplate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTriggerTemplate.Location = new System.Drawing.Point(525, 3);
            this.btnTriggerTemplate.Name = "btnTriggerTemplate";
            this.btnTriggerTemplate.Size = new System.Drawing.Size(194, 23);
            this.btnTriggerTemplate.TabIndex = 1;
            this.btnTriggerTemplate.TabStop = false;
            this.btnTriggerTemplate.Text = "Edit template trigger";
            this.btnTriggerTemplate.UseVisualStyleBackColor = true;
            this.btnTriggerTemplate.Click += new System.EventHandler(this.btnTriggerTemplate_Click);
            // 
            // tableDefaultsReserved
            // 
            this.tableDefaultsReserved.AutoSize = true;
            this.tableDefaultsReserved.ColumnCount = 1;
            this.tableDefaultsReserved.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDefaultsReserved.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableDefaultsReserved.Location = new System.Drawing.Point(10, 23);
            this.tableDefaultsReserved.Name = "tableDefaultsReserved";
            this.tableDefaultsReserved.RowCount = 1;
            this.tableDefaultsReserved.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDefaultsReserved.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableDefaultsReserved.Size = new System.Drawing.Size(722, 29);
            this.tableDefaultsReserved.TabIndex = 1;
            // 
            // dlgUiFont
            // 
            this.dlgUiFont.AllowScriptChange = false;
            // 
            // ConfigurationForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(784, 661);
            this.Controls.Add(this.tbcMain);
            this.Controls.Add(this.pnlButtonsSpacing);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "ConfigurationForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            this.Shown += new System.EventHandler(this.ConfigurationForm_Shown);
            this.pnlButtons.ResumeLayout(false);
            this.grpVolAdjustment.ResumeLayout(false);
            this.grpVolAdjustment.PerformLayout();
            this.tableAudioVolume.ResumeLayout(false);
            this.tableAudioVolume.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbTtsVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbSoundVolume)).EndInit();
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            this.tableLoggingGeneral.ResumeLayout(false);
            this.tableLoggingGeneral.PerformLayout();
            this.ctxStartupTrigger.ResumeLayout(false);
            this.grpActHooks.ResumeLayout(false);
            this.grpActHooks.PerformLayout();
            this.tableAudioActHooks.ResumeLayout(false);
            this.tableAudioActHooks.PerformLayout();
            this.grpFutureProofing.ResumeLayout(false);
            this.grpFutureProofing.PerformLayout();
            this.tableEventSeparator.ResumeLayout(false);
            this.tableEventSeparator.PerformLayout();
            this.tbcMain.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.grpStartupTrigger.ResumeLayout(false);
            this.grpStartupTrigger.PerformLayout();
            this.tlsDirectPaste.ResumeLayout(false);
            this.tlsDirectPaste.PerformLayout();
            this.grpStartup.ResumeLayout(false);
            this.grpStartup.PerformLayout();
            this.tableStartup.ResumeLayout(false);
            this.tableStartup.PerformLayout();
            this.tabAudio.ResumeLayout(false);
            this.tabAudio.PerformLayout();
            this.tabShortCuts.ResumeLayout(false);
            this.tabShortCuts.PerformLayout();
            this.grpShortCutExpression.ResumeLayout(false);
            this.grpShortCutExpression.PerformLayout();
            this.tableLayoutPanelShortCutExpression.ResumeLayout(false);
            this.tableLayoutPanelShortCutExpression.PerformLayout();
            this.tabCaching.ResumeLayout(false);
            this.pnlCaching.ResumeLayout(false);
            this.pnlCaching.PerformLayout();
            this.grpCacheFile.ResumeLayout(false);
            this.grpCacheFile.PerformLayout();
            this.tableCacheFile.ResumeLayout(false);
            this.tableCacheFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheFileExpiry)).EndInit();
            this.grpCacheRepo.ResumeLayout(false);
            this.grpCacheRepo.PerformLayout();
            this.tableCacheRepo.ResumeLayout(false);
            this.tableCacheRepo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheRepoExpiry)).EndInit();
            this.grpCacheJSON.ResumeLayout(false);
            this.grpCacheJSON.PerformLayout();
            this.tableCacheJson.ResumeLayout(false);
            this.tableCacheJson.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheJsonExpiry)).EndInit();
            this.grpCacheSound.ResumeLayout(false);
            this.grpCacheSound.PerformLayout();
            this.tableCacheSound.ResumeLayout(false);
            this.tableCacheSound.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheSoundExpiry)).EndInit();
            this.grpCacheImage.ResumeLayout(false);
            this.grpCacheImage.PerformLayout();
            this.tableCacheImage.ResumeLayout(false);
            this.tableCacheImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheImageExpiry)).EndInit();
            this.tabEndpoint.ResumeLayout(false);
            this.tabEndpoint.PerformLayout();
            this.grpEndpointHistory.ResumeLayout(false);
            this.pnlEndpointHistory.ResumeLayout(false);
            this.pnlEndpointHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEndpointHistory)).EndInit();
            this.tlsEndpointHistory.ResumeLayout(false);
            this.tlsEndpointHistory.PerformLayout();
            this.grpEndpointSettings.ResumeLayout(false);
            this.grpEndpointSettings.PerformLayout();
            this.tableEndpointSettings.ResumeLayout(false);
            this.tableEndpointSettings.PerformLayout();
            this.grpEndpointState.ResumeLayout(false);
            this.tableEndpointStatus.ResumeLayout(false);
            this.tableEndpointStatus.PerformLayout();
            this.tabFFXIV.ResumeLayout(false);
            this.tabFFXIV.PerformLayout();
            this.grpFfxivEventLogging.ResumeLayout(false);
            this.grpFfxivEventLogging.PerformLayout();
            this.tableFfxivLogging.ResumeLayout(false);
            this.tableFfxivLogging.PerformLayout();
            this.tabSubstitutions.ResumeLayout(false);
            this.tabSubstitutions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubstitutions)).EndInit();
            this.tlsSubstitutions.ResumeLayout(false);
            this.tlsSubstitutions.PerformLayout();
            this.tabConsts.ResumeLayout(false);
            this.pnlConstants.ResumeLayout(false);
            this.pnlConstants.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConstVariables)).EndInit();
            this.tlsScalar.ResumeLayout(false);
            this.tlsScalar.PerformLayout();
            this.tabSecurity.ResumeLayout(false);
            this.tabSecurity.PerformLayout();
            this.grpScriptingAccess.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdditionalFeatures)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApiAccess)).EndInit();
            this.tabMisc.ResumeLayout(false);
            this.tabMisc.PerformLayout();
            this.grpAuraManagement.ResumeLayout(false);
            this.grpAuraManagement.PerformLayout();
            this.tableAuraManagement.ResumeLayout(false);
            this.tableAuraManagement.PerformLayout();
            this.grpUserInterface.ResumeLayout(false);
            this.grpUserInterface.PerformLayout();
            this.tableUiOptions.ResumeLayout(false);
            this.tableUiOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutosaveMinutes)).EndInit();
            this.grpDefaultSettings.ResumeLayout(false);
            this.grpDefaultSettings.PerformLayout();
            this.tableTriggerTemplate.ResumeLayout(false);
            this.tableTriggerTemplate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTipSettings;
        private System.Windows.Forms.Panel pnlButtonsSpacing;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox grpVolAdjustment;
        private System.Windows.Forms.TableLayoutPanel tableAudioVolume;
        private System.Windows.Forms.Label lblTtsVolP;
        private System.Windows.Forms.Label lblSoundVolP;
        private System.Windows.Forms.TrackBar trbTtsVolume;
        private System.Windows.Forms.TrackBar trbSoundVolume;
        private System.Windows.Forms.Label lblSoundVolume;
        private System.Windows.Forms.Label lblTtsVolume;
        private System.Windows.Forms.GroupBox grpGeneral;
        private System.Windows.Forms.TableLayoutPanel tableLoggingGeneral;
        private System.Windows.Forms.GroupBox grpActHooks;
        private System.Windows.Forms.TableLayoutPanel tableAudioActHooks;
        private System.Windows.Forms.CheckBox chkActTts;
        private System.Windows.Forms.Panel pnlAudioSpacing;
        private System.Windows.Forms.CheckBox chkActSoundFiles;
        private System.Windows.Forms.GroupBox grpFutureProofing;
        private System.Windows.Forms.TableLayoutPanel tableEventSeparator;
        private System.Windows.Forms.TextBox txtSeparator;
        private System.Windows.Forms.Label lblSeparator;
        internal System.Windows.Forms.TreeView trvTrigger;
        private System.Windows.Forms.ContextMenuStrip ctxStartupTrigger;
        private System.Windows.Forms.ToolStripMenuItem ctxClearSelection;
        private System.Windows.Forms.Label lblLoggingLevel;
        private System.Windows.Forms.ComboBox cbxLoggingLevel;
        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabAudio;
        private System.Windows.Forms.TabPage tabShortCuts;
        private System.Windows.Forms.GroupBox grpShortCutExpression;
        private System.Windows.Forms.CheckBox chkShortcutEnableTemplates;
        private System.Windows.Forms.CheckBox chkShortcutUseAbbrevInTemplates;
        private System.Windows.Forms.CheckBox chkShortcutWrapTextWhenSelected;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelShortCutExpression;
        private System.Windows.Forms.TabPage tabFFXIV;
        private System.Windows.Forms.TabPage tabMisc;
        private System.Windows.Forms.Panel pnlDefaultsSpacing;
        private System.Windows.Forms.GroupBox grpDefaultSettings;
        private System.Windows.Forms.TableLayoutPanel tableDefaultsReserved;
        private System.Windows.Forms.CheckBox chkClipboard;
        private System.Windows.Forms.GroupBox grpStartup;
        private System.Windows.Forms.TableLayoutPanel tableStartup;
        private System.Windows.Forms.CheckBox chkWelcome;
        private System.Windows.Forms.Panel pnlEventSeparatorSpacing;
        private System.Windows.Forms.CheckBox chkUpdates;
        private System.Windows.Forms.TabPage tabEndpoint;
        private System.Windows.Forms.GroupBox grpEndpointSettings;
        private System.Windows.Forms.TableLayoutPanel tableEndpointSettings;
        private System.Windows.Forms.TextBox txtEndpoint;
        private System.Windows.Forms.Label lblEndpoint;
        private System.Windows.Forms.Panel pnlEndpointStatusSpacing;
        private System.Windows.Forms.GroupBox grpEndpointState;
        private System.Windows.Forms.Button btnEndpointStop;
        private System.Windows.Forms.Button btnEndpointStart;
        private System.Windows.Forms.GroupBox grpFfxivEventLogging;
        private System.Windows.Forms.TableLayoutPanel tableFfxivLogging;
        private System.Windows.Forms.CheckBox chkFfxivLogNetwork;
        private System.Windows.Forms.CheckBox chkLogNormalEvents;
        private System.Windows.Forms.Panel pnlFfxivLoggingSpacing;
        private System.Windows.Forms.GroupBox grpStartupTrigger;
        private System.Windows.Forms.Panel pnlStartupTriggerSpacing;
        private System.Windows.Forms.Panel pnlStartupSpacing;
        private System.Windows.Forms.ToolStrip tlsDirectPaste;
        private System.Windows.Forms.ToolStripButton btnClearSelection;
        private System.Windows.Forms.ToolStripLabel lblFolderReminder;
        private System.Windows.Forms.CheckBox chkWarnAdmin;
        private System.Windows.Forms.GroupBox grpUserInterface;
        private System.Windows.Forms.TableLayoutPanel tableUiOptions;
        private System.Windows.Forms.CheckBox cbxTestIgnoreConditions;
        private System.Windows.Forms.GroupBox grpAuraManagement;
        private System.Windows.Forms.TableLayoutPanel tableAuraManagement;
        private System.Windows.Forms.CheckBox cbxEnableHwAccel;
        private System.Windows.Forms.Panel pnlAuraManagementSpacing;
        private System.Windows.Forms.TextBox txtMonitorWindow;
        private System.Windows.Forms.Label lblMonitorWindow;
        private System.Windows.Forms.CheckBox cbxDevMode;
        private System.Windows.Forms.TabPage tabCaching;
        private System.Windows.Forms.Panel pnlCaching;
        private System.Windows.Forms.GroupBox grpCacheRepo;
        private System.Windows.Forms.TableLayoutPanel tableCacheRepo;
        private System.Windows.Forms.Button btnCacheRepoClear;
        private System.Windows.Forms.TextBox txtCacheRepoSize;
        private System.Windows.Forms.Label lblCacheRepoSize;
        private System.Windows.Forms.Label lblCacheRepoCount;
        private System.Windows.Forms.Label lblCacheRepoExpiry;
        private System.Windows.Forms.NumericUpDown nudCacheRepoExpiry;
        private System.Windows.Forms.TextBox txtCacheRepoCount;
        private System.Windows.Forms.Panel pnlCacheRepoSpacing;
        private System.Windows.Forms.GroupBox grpCacheJSON;
        private System.Windows.Forms.TableLayoutPanel tableCacheJson;
        private System.Windows.Forms.Button btnCacheJsonClear;
        private System.Windows.Forms.TextBox txtCacheJsonSize;
        private System.Windows.Forms.Label lblCacheJsonSize;
        private System.Windows.Forms.Label lblCacheJsonCount;
        private System.Windows.Forms.Label lblCacheJsonExpiry;
        private System.Windows.Forms.TextBox txtCacheJsonCount;
        private System.Windows.Forms.Panel pnlCacheJsonSpacing;
        private System.Windows.Forms.GroupBox grpCacheSound;
        private System.Windows.Forms.TableLayoutPanel tableCacheSound;
        private System.Windows.Forms.Button btnCacheSoundClear;
        private System.Windows.Forms.TextBox txtCacheSoundSize;
        private System.Windows.Forms.Label lblCacheSoundSize;
        private System.Windows.Forms.Label lblCacheSoundCount;
        private System.Windows.Forms.Label lblCacheSoundExpiry;
        private System.Windows.Forms.NumericUpDown nudCacheSoundExpiry;
        private System.Windows.Forms.TextBox txtCacheSoundCount;
        private System.Windows.Forms.Panel pnlCacheSoundSpacing;
        private System.Windows.Forms.GroupBox grpCacheImage;
        private System.Windows.Forms.TableLayoutPanel tableCacheImage;
        private System.Windows.Forms.TextBox txtCacheImageSize;
        private System.Windows.Forms.Label lblCacheImageSize;
        private System.Windows.Forms.Label lblCacheImageCount;
        private System.Windows.Forms.Label lblCacheImageExpiry;
        private System.Windows.Forms.NumericUpDown nudCacheImageExpiry;
        private System.Windows.Forms.TextBox txtCacheImageCount;
        private System.Windows.Forms.Button btnCacheImageClear;
        private System.Windows.Forms.Button btnCacheRepoBrowse;
        private System.Windows.Forms.Button btnCacheJsonBrowse;
        private System.Windows.Forms.Button btnCacheSoundBrowse;
        private System.Windows.Forms.Button btnCacheImageBrowse;
        private System.Windows.Forms.NumericUpDown nudCacheJsonExpiry;
        private System.Windows.Forms.GroupBox grpCacheFile;
        private System.Windows.Forms.TableLayoutPanel tableCacheFile;
        private System.Windows.Forms.Button btnCacheFileBrowse;
        private System.Windows.Forms.Button btnCacheFileClear;
        private System.Windows.Forms.TextBox txtCacheFileSize;
        private System.Windows.Forms.Label lblCacheFileSize;
        private System.Windows.Forms.Label lblCacheFileCount;
        private System.Windows.Forms.Label lblCacheFileExpiry;
        private System.Windows.Forms.NumericUpDown nudCacheFileExpiry;
        private System.Windows.Forms.TextBox txtCacheFileCount;
        private System.Windows.Forms.Panel pnlCacheFileSpacing;
        private System.Windows.Forms.TabPage tabSubstitutions;
        private CustomControls.DataGridViewEx dgvSubstitutions;
        private System.Windows.Forms.ToolStrip tlsSubstitutions;
        private System.Windows.Forms.ToolStripButton btnSubAdd;
        private System.Windows.Forms.ToolStripButton btnSubEdit;
        private System.Windows.Forms.ToolStripButton btnSubRemove;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubSearchFor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubReplaceWith;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubScope;
        private System.Windows.Forms.CheckBox chkLogVariableExpansions;
        private System.Windows.Forms.TabPage tabSecurity;
        private System.Windows.Forms.GroupBox grpScriptingAccess;
        private CustomControls.DataGridViewEx dgvApiAccess;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApiNamespace;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colApiAllowLocal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colApiAllowRemote;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colApiAllowAdmin;
        private System.Windows.Forms.Panel pnlSecurityUnlockSpacing;
        private System.Windows.Forms.Button btnUnlockSecurity;
        private System.Windows.Forms.ComboBox cbxUpdateMethod;
        private System.Windows.Forms.Label lblUpdateMethod;
        private System.Windows.Forms.Label lblAutosaveInterval;
        private System.Windows.Forms.CheckBox cbxAutosaveConfig;
        private System.Windows.Forms.NumericUpDown nudAutosaveMinutes;
        private System.Windows.Forms.TableLayoutPanel tableTriggerTemplate;
        private System.Windows.Forms.CheckBox cbxTriggerTemplate;
        private System.Windows.Forms.Button btnTriggerTemplate;
        private System.Windows.Forms.TabPage tabConsts;
        private System.Windows.Forms.Panel pnlConstants;
        private CustomControls.DataGridViewEx dgvConstVariables;
        private System.Windows.Forms.ToolStrip tlsScalar;
        private System.Windows.Forms.ToolStripButton btnConstAdd;
        private System.Windows.Forms.ToolStripButton btnConstEdit;
        private System.Windows.Forms.ToolStripSeparator sepConstantRemove;
        private System.Windows.Forms.ToolStripButton btnConstRemove;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScalarName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScalarValue;
        private System.Windows.Forms.FontDialog dlgUiFont;
        private System.Windows.Forms.CheckBox cbxActionAsync;
        private System.Windows.Forms.CheckBox chkEndpointLog;
        private System.Windows.Forms.CheckBox chkEndpointStartup;
        private System.Windows.Forms.GroupBox grpEndpointHistory;
        private System.Windows.Forms.Panel pnlEndpointHistory;
        private System.Windows.Forms.ToolStrip tlsEndpointHistory;
        private System.Windows.Forms.ToolStripButton btnEndpointHistUpdate;
        private System.Windows.Forms.ToolStripLabel tlsEndpointHistoryRecv;
        private System.Windows.Forms.Panel pnlEndpointHistorySpacing;
        private System.Windows.Forms.TableLayoutPanel tableEndpointStatus;
        private System.Windows.Forms.TextBox txtEndpointStatus;
        private System.Windows.Forms.ToolStripLabel tlsEndpointHistoryCount;
        private CustomControls.DataGridViewEx dgvEndpointHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndpointHistoryTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndpointHistoryData;
        private CustomControls.DataGridViewEx dgvAdditionalFeatures;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFeatureName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFeatureAllowLocal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFeatureAllowRemote;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFeatureAllowAdmin;
        private System.Windows.Forms.Panel pnlSecurityFeaturesSpacing;
        private System.Windows.Forms.CheckBox cbxAutoComplete;
    }
}