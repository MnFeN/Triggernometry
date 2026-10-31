namespace Triggernometry.UI.Forms
{
    partial class ConfigurationForm2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationForm2));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.btnMisc = new Triggernometry.UI.CustomControls.MenuButton();
            this.tbcMain = new System.Windows.Forms.TabControl();
            this.tabStartup = new System.Windows.Forms.TabPage();
            this.pnlStartupTrigger = new System.Windows.Forms.Panel();
            this.trvTrigger = new System.Windows.Forms.TreeView();
            this.ctxStartupTrigger = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxClearSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.tlsStartupTrigger = new System.Windows.Forms.ToolStrip();
            this.btnClearSelection = new System.Windows.Forms.ToolStripButton();
            this.lblFolderReminder = new System.Windows.Forms.ToolStripLabel();
            this.capStartupTrigger = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.pnlStartup = new System.Windows.Forms.Panel();
            this.tableStartup = new System.Windows.Forms.TableLayoutPanel();
            this.lblWarnAdmin = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.chkWarnAdmin = new System.Windows.Forms.CheckBox();
            this.chkWelcome = new System.Windows.Forms.CheckBox();
            this.capStartupOptions = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.tabUserInterface = new System.Windows.Forms.TabPage();
            this.pnlUiOptions = new System.Windows.Forms.Panel();
            this.tableUiOptions = new System.Windows.Forms.TableLayoutPanel();
            this.lblDevMode = new System.Windows.Forms.Label();
            this.lblAutoComplete = new System.Windows.Forms.Label();
            this.lblActionAsync = new System.Windows.Forms.Label();
            this.lblTestIgnoreConditions = new System.Windows.Forms.Label();
            this.cbxAutoComplete = new System.Windows.Forms.CheckBox();
            this.cbxDevMode = new System.Windows.Forms.CheckBox();
            this.cbxActionAsync = new System.Windows.Forms.CheckBox();
            this.cbxTestIgnoreConditions = new System.Windows.Forms.CheckBox();
            this.capBehavior = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.pnlUiFont = new System.Windows.Forms.Panel();
            this.tableUiFont = new System.Windows.Forms.TableLayoutPanel();
            this.lblUiFontDefault = new System.Windows.Forms.Label();
            this.cbxUiFontDefault = new System.Windows.Forms.CheckBox();
            this.lblUiFont = new System.Windows.Forms.Label();
            this.txtUiFontDesc = new System.Windows.Forms.TextBox();
            this.btnUiFont = new System.Windows.Forms.Button();
            this.capAppearance = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.tabAudio = new System.Windows.Forms.TabPage();
            this.panelAudioTts = new System.Windows.Forms.Panel();
            this.tableAudioTts = new System.Windows.Forms.TableLayoutPanel();
            this.lblTtsRepV = new System.Windows.Forms.Label();
            this.trbTtsRepetition = new System.Windows.Forms.TrackBar();
            this.lblTtsRepetition = new System.Windows.Forms.Label();
            this.txtTtsPathArgs = new System.Windows.Forms.TextBox();
            this.lblTtsPathArgs = new System.Windows.Forms.Label();
            this.btnTtsPath = new System.Windows.Forms.Button();
            this.txtTtsPath = new System.Windows.Forms.TextBox();
            this.cbxTtsMethod = new System.Windows.Forms.ComboBox();
            this.lblTtsPath = new System.Windows.Forms.Label();
            this.lblTtsMethod = new System.Windows.Forms.Label();
            this.lblTtsVolP = new System.Windows.Forms.Label();
            this.trbTtsVolume = new System.Windows.Forms.TrackBar();
            this.lblTtsVolume = new System.Windows.Forms.Label();
            this.pnlTtsWarnings = new System.Windows.Forms.Panel();
            this.lblExternalTtsWarn = new System.Windows.Forms.Label();
            this.lblActTtsWarn = new System.Windows.Forms.Label();
            this.capAudioTts = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.panelAudioSound = new System.Windows.Forms.Panel();
            this.tableAudioSound = new System.Windows.Forms.TableLayoutPanel();
            this.lblSoundRepV = new System.Windows.Forms.Label();
            this.trbSoundRepetition = new System.Windows.Forms.TrackBar();
            this.lblSoundRepetition = new System.Windows.Forms.Label();
            this.txtSoundPathArgs = new System.Windows.Forms.TextBox();
            this.lblSoundPathArgs = new System.Windows.Forms.Label();
            this.pnlExternalSoundWarning = new System.Windows.Forms.Panel();
            this.lblExternalSoundWarn = new System.Windows.Forms.Label();
            this.btnSoundPath = new System.Windows.Forms.Button();
            this.lblSoundPath = new System.Windows.Forms.Label();
            this.lblSoundMethod = new System.Windows.Forms.Label();
            this.lblSoundVolP = new System.Windows.Forms.Label();
            this.trbSoundVolume = new System.Windows.Forms.TrackBar();
            this.lblSoundVolume = new System.Windows.Forms.Label();
            this.cbxSoundMethod = new System.Windows.Forms.ComboBox();
            this.txtSoundPath = new System.Windows.Forms.TextBox();
            this.capAudioSound = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.panelAudioBeep = new System.Windows.Forms.Panel();
            this.tableAudioBeep = new System.Windows.Forms.TableLayoutPanel();
            this.lblUseSimulatedBeep = new System.Windows.Forms.Label();
            this.chkUseSimulatedBeep = new System.Windows.Forms.CheckBox();
            this.lblBeepVolume = new System.Windows.Forms.Label();
            this.trbBeepVolume = new System.Windows.Forms.TrackBar();
            this.lblBeepVolP = new System.Windows.Forms.Label();
            this.btnBeepTest = new System.Windows.Forms.Button();
            this.capAudioBeep = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.tabLogging = new System.Windows.Forms.TabPage();
            this.pnlLoggingGeneral = new System.Windows.Forms.Panel();
            this.tableLoggingGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.lblLogNormalEvents = new System.Windows.Forms.Label();
            this.lblLogVariableExpansions = new System.Windows.Forms.Label();
            this.chkLogVariableExpansions = new System.Windows.Forms.CheckBox();
            this.chkLogNormalEvents = new System.Windows.Forms.CheckBox();
            this.lblLoggingLevel = new System.Windows.Forms.Label();
            this.cbxLoggingLevel = new System.Windows.Forms.ComboBox();
            this.capLogging = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.capLogLimits = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.panelLogLimits = new System.Windows.Forms.Panel();
            this.tableLogLimits = new System.Windows.Forms.TableLayoutPanel();
            this.lblLogLimitError = new System.Windows.Forms.Label();
            this.nudLogLimitError = new System.Windows.Forms.NumericUpDown();
            this.lblLogLimitWarning = new System.Windows.Forms.Label();
            this.nudLogLimitWarning = new System.Windows.Forms.NumericUpDown();
            this.lblLogLimitCustom = new System.Windows.Forms.Label();
            this.nudLogLimitCustom = new System.Windows.Forms.NumericUpDown();
            this.lblLogLimitCustom2 = new System.Windows.Forms.Label();
            this.nudLogLimitCustom2 = new System.Windows.Forms.NumericUpDown();
            this.lblLogLimitInfo = new System.Windows.Forms.Label();
            this.nudLogLimitInfo = new System.Windows.Forms.NumericUpDown();
            this.lblLogLimitVerbose = new System.Windows.Forms.Label();
            this.nudLogLimitVerbose = new System.Windows.Forms.NumericUpDown();
            this.tabShortcuts = new System.Windows.Forms.TabPage();
            this.pnlShortcutTemplates = new System.Windows.Forms.Panel();
            this.tableLayoutPanelShortCutExpression = new System.Windows.Forms.TableLayoutPanel();
            this.lblShortcutWrapTextWhenSelected = new System.Windows.Forms.Label();
            this.lblShortcutUseAbbrevInTemplates = new System.Windows.Forms.Label();
            this.lblShortcutEnableTemplates = new System.Windows.Forms.Label();
            this.chkShortcutEnableTemplates = new System.Windows.Forms.CheckBox();
            this.chkShortcutUseAbbrevInTemplates = new System.Windows.Forms.CheckBox();
            this.chkShortcutWrapTextWhenSelected = new System.Windows.Forms.CheckBox();
            this.capExpressions = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.tabCaching = new System.Windows.Forms.TabPage();
            this.pnlCacheFile = new System.Windows.Forms.Panel();
            this.tableLayoutPanelDownloads = new System.Windows.Forms.TableLayoutPanel();
            this.btnCacheFileBrowse = new System.Windows.Forms.Button();
            this.btnCacheFileClear = new System.Windows.Forms.Button();
            this.txtCacheFileSize = new System.Windows.Forms.TextBox();
            this.lblCacheFileSize = new System.Windows.Forms.Label();
            this.lblCacheFileCount = new System.Windows.Forms.Label();
            this.lblCacheFileExpiry = new System.Windows.Forms.Label();
            this.nudCacheFileExpiry = new System.Windows.Forms.NumericUpDown();
            this.txtCacheFileCount = new System.Windows.Forms.TextBox();
            this.capDownloads = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.pnlCacheRepo = new System.Windows.Forms.Panel();
            this.tableLayoutPanelRepo = new System.Windows.Forms.TableLayoutPanel();
            this.btnCacheRepoBrowse = new System.Windows.Forms.Button();
            this.btnCacheRepoClear = new System.Windows.Forms.Button();
            this.txtCacheRepoSize = new System.Windows.Forms.TextBox();
            this.lblCacheRepoSize = new System.Windows.Forms.Label();
            this.lblCacheRepoCount = new System.Windows.Forms.Label();
            this.lblCacheRepoExpiry = new System.Windows.Forms.Label();
            this.nudCacheRepoExpiry = new System.Windows.Forms.NumericUpDown();
            this.txtCacheRepoCount = new System.Windows.Forms.TextBox();
            this.capRepoBackups = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.pnlCacheJson = new System.Windows.Forms.Panel();
            this.tableLayoutPanelJason = new System.Windows.Forms.TableLayoutPanel();
            this.nudCacheJsonExpiry = new System.Windows.Forms.NumericUpDown();
            this.btnCacheJsonBrowse = new System.Windows.Forms.Button();
            this.btnCacheJsonClear = new System.Windows.Forms.Button();
            this.txtCacheJsonSize = new System.Windows.Forms.TextBox();
            this.lblCacheJsonSize = new System.Windows.Forms.Label();
            this.lblCacheJsonCount = new System.Windows.Forms.Label();
            this.lblCacheJsonExpiry = new System.Windows.Forms.Label();
            this.txtCacheJsonCount = new System.Windows.Forms.TextBox();
            this.capJason = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.pnlCacheSound = new System.Windows.Forms.Panel();
            this.tableLayoutPanelSound = new System.Windows.Forms.TableLayoutPanel();
            this.btnCacheSoundBrowse = new System.Windows.Forms.Button();
            this.btnCacheSoundClear = new System.Windows.Forms.Button();
            this.txtCacheSoundSize = new System.Windows.Forms.TextBox();
            this.lblCacheSoundSize = new System.Windows.Forms.Label();
            this.lblCacheSoundCount = new System.Windows.Forms.Label();
            this.lblCacheSoundExpiry = new System.Windows.Forms.Label();
            this.nudCacheSoundExpiry = new System.Windows.Forms.NumericUpDown();
            this.txtCacheSoundCount = new System.Windows.Forms.TextBox();
            this.capSoundFiles = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.pnlCacheImage = new System.Windows.Forms.Panel();
            this.tableLayoutPanelImages = new System.Windows.Forms.TableLayoutPanel();
            this.btnCacheImageBrowse = new System.Windows.Forms.Button();
            this.txtCacheImageSize = new System.Windows.Forms.TextBox();
            this.lblCacheImageSize = new System.Windows.Forms.Label();
            this.lblCacheImageCount = new System.Windows.Forms.Label();
            this.lblCacheImageExpiry = new System.Windows.Forms.Label();
            this.nudCacheImageExpiry = new System.Windows.Forms.NumericUpDown();
            this.txtCacheImageCount = new System.Windows.Forms.TextBox();
            this.btnCacheImageClear = new System.Windows.Forms.Button();
            this.capImageFiles = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.tabUpdates = new System.Windows.Forms.TabPage();
            this.pnlUpdateChannel = new System.Windows.Forms.Panel();
            this.tableUpdateChannel = new System.Windows.Forms.TableLayoutPanel();
            this.chkUpdates = new System.Windows.Forms.CheckBox();
            this.lblUpdates = new System.Windows.Forms.Label();
            this.nudUpdateInterval = new System.Windows.Forms.NumericUpDown();
            this.lblUpdateInterval = new System.Windows.Forms.Label();
            this.lblExternalUpdateWarn = new System.Windows.Forms.Label();
            this.txtUpdateChannelUrl = new System.Windows.Forms.TextBox();
            this.cbxUpdateMethod = new System.Windows.Forms.ComboBox();
            this.lblUpdateChannelUrl = new System.Windows.Forms.Label();
            this.lblUpdateMethod = new System.Windows.Forms.Label();
            this.btnUpdateCheck = new System.Windows.Forms.Button();
            this.capUpdateChannel = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.tabEndpoint = new System.Windows.Forms.TabPage();
            this.pnlEndpointHistory = new System.Windows.Forms.Panel();
            this.dgvEndpointHistory = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colEndpointHistoryTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndpointHistoryData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsEndpointHistory = new System.Windows.Forms.ToolStrip();
            this.btnEndpointHistUpdate = new System.Windows.Forms.ToolStripButton();
            this.tlsEndpointHistoryRecv = new System.Windows.Forms.ToolStripLabel();
            this.tlsEndpointHistoryCount = new System.Windows.Forms.ToolStripLabel();
            this.capEndpointHistory = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.pnlEndpointSettings = new System.Windows.Forms.Panel();
            this.tableEndpointSettings = new System.Windows.Forms.TableLayoutPanel();
            this.lblEndpointLog = new System.Windows.Forms.Label();
            this.lblEndpointStartup = new System.Windows.Forms.Label();
            this.chkEndpointStartup = new System.Windows.Forms.CheckBox();
            this.chkEndpointLog = new System.Windows.Forms.CheckBox();
            this.txtEndpoint = new System.Windows.Forms.TextBox();
            this.lblEndpoint = new System.Windows.Forms.Label();
            this.capEndpointSettings = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.pnlEndpointStatus = new System.Windows.Forms.Panel();
            this.tableEndpointStatus = new System.Windows.Forms.TableLayoutPanel();
            this.btnEndpointStart = new System.Windows.Forms.Button();
            this.btnEndpointStop = new System.Windows.Forms.Button();
            this.txtEndpointStatus = new System.Windows.Forms.TextBox();
            this.capEndpointStatus = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.tabConstants = new System.Windows.Forms.TabPage();
            this.dgvConstVariables = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colScalarName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScalarValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsConstant = new System.Windows.Forms.ToolStrip();
            this.btnConstAdd = new System.Windows.Forms.ToolStripButton();
            this.btnConstEdit = new System.Windows.Forms.ToolStripButton();
            this.btnConstRemove = new System.Windows.Forms.ToolStripButton();
            this.tabSubs = new System.Windows.Forms.TabPage();
            this.dgvSubstitutions = new Triggernometry.UI.CustomControls.DataGridViewEx();
            this.colSubSearchFor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubReplaceWith = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubScope = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsSubstitutions = new System.Windows.Forms.ToolStrip();
            this.btnSubAdd = new System.Windows.Forms.ToolStripButton();
            this.btnSubEdit = new System.Windows.Forms.ToolStripButton();
            this.btnSubRemove = new System.Windows.Forms.ToolStripButton();
            this.tabGameSpecific = new System.Windows.Forms.TabPage();
            this.tbcGames = new System.Windows.Forms.TabControl();
            this.tabFFXIV = new System.Windows.Forms.TabPage();
            this.pnlFfxivLogging = new System.Windows.Forms.Panel();
            this.tableFfxivLogging = new System.Windows.Forms.TableLayoutPanel();
            this.lblFfxivLogNetwork = new System.Windows.Forms.Label();
            this.chkFfxivLogNetwork = new System.Windows.Forms.CheckBox();
            this.capFfxivLogging = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.tabSecurity = new System.Windows.Forms.TabPage();
            this.pnlScriptingAccess = new System.Windows.Forms.Panel();
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
            this.capScripting = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.tabMiscellaneous = new System.Windows.Forms.TabPage();
            this.pnlAuraManagement = new System.Windows.Forms.Panel();
            this.tableAuraManagement = new System.Windows.Forms.TableLayoutPanel();
            this.lblEnableHwAccel = new System.Windows.Forms.Label();
            this.txtMonitorWindow = new System.Windows.Forms.TextBox();
            this.lblMonitorWindow = new System.Windows.Forms.Label();
            this.cbxEnableHwAccel = new System.Windows.Forms.CheckBox();
            this.capAuraManagement = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.pnlAutosave = new System.Windows.Forms.Panel();
            this.tableAutosave = new System.Windows.Forms.TableLayoutPanel();
            this.lblAutosaveConfig = new System.Windows.Forms.Label();
            this.lblAutosaveInterval = new System.Windows.Forms.Label();
            this.cbxAutosaveConfig = new System.Windows.Forms.CheckBox();
            this.nudAutosaveMinutes = new System.Windows.Forms.NumericUpDown();
            this.capAutosaving = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.pnlTriggerTemplate = new System.Windows.Forms.Panel();
            this.tableTriggerTemplate = new System.Windows.Forms.TableLayoutPanel();
            this.lblTriggerTemplate = new System.Windows.Forms.Label();
            this.cbxTriggerTemplate = new System.Windows.Forms.CheckBox();
            this.btnTriggerTemplate = new System.Windows.Forms.Button();
            this.capTemplate = new Triggernometry.UI.CustomControls.PrettyCaption();
            this.btnSecurity = new Triggernometry.UI.CustomControls.MenuButton();
            this.btnGameSpecific = new Triggernometry.UI.CustomControls.MenuButton();
            this.btnSubstitutions = new Triggernometry.UI.CustomControls.MenuButton();
            this.btnConstants = new Triggernometry.UI.CustomControls.MenuButton();
            this.btnEndpoint = new Triggernometry.UI.CustomControls.MenuButton();
            this.btnUpdates = new Triggernometry.UI.CustomControls.MenuButton();
            this.btnCaching = new Triggernometry.UI.CustomControls.MenuButton();
            this.btnShortcuts = new Triggernometry.UI.CustomControls.MenuButton();
            this.btnLogging = new Triggernometry.UI.CustomControls.MenuButton();
            this.btnAudio = new Triggernometry.UI.CustomControls.MenuButton();
            this.btnUserInterface = new Triggernometry.UI.CustomControls.MenuButton();
            this.btnStartup = new Triggernometry.UI.CustomControls.MenuButton();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTipSettings = new System.Windows.Forms.ToolTip(this.components);
            this.dlgUiFont = new System.Windows.Forms.FontDialog();
            this.pnlMenu.SuspendLayout();
            this.tbcMain.SuspendLayout();
            this.tabStartup.SuspendLayout();
            this.pnlStartupTrigger.SuspendLayout();
            this.ctxStartupTrigger.SuspendLayout();
            this.tlsStartupTrigger.SuspendLayout();
            this.pnlStartup.SuspendLayout();
            this.tableStartup.SuspendLayout();
            this.tabUserInterface.SuspendLayout();
            this.pnlUiOptions.SuspendLayout();
            this.tableUiOptions.SuspendLayout();
            this.pnlUiFont.SuspendLayout();
            this.tableUiFont.SuspendLayout();
            this.tabAudio.SuspendLayout();
            this.panelAudioTts.SuspendLayout();
            this.tableAudioTts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbTtsRepetition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbTtsVolume)).BeginInit();
            this.pnlTtsWarnings.SuspendLayout();
            this.panelAudioSound.SuspendLayout();
            this.tableAudioSound.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbSoundRepetition)).BeginInit();
            this.pnlExternalSoundWarning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbSoundVolume)).BeginInit();
            this.panelAudioBeep.SuspendLayout();
            this.tableAudioBeep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbBeepVolume)).BeginInit();
            this.tabLogging.SuspendLayout();
            this.pnlLoggingGeneral.SuspendLayout();
            this.tableLoggingGeneral.SuspendLayout();
            this.panelLogLimits.SuspendLayout();
            this.tableLogLimits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitCustom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitCustom2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitVerbose)).BeginInit();
            this.tabShortcuts.SuspendLayout();
            this.pnlShortcutTemplates.SuspendLayout();
            this.tableLayoutPanelShortCutExpression.SuspendLayout();
            this.tabCaching.SuspendLayout();
            this.pnlCacheFile.SuspendLayout();
            this.tableLayoutPanelDownloads.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheFileExpiry)).BeginInit();
            this.pnlCacheRepo.SuspendLayout();
            this.tableLayoutPanelRepo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheRepoExpiry)).BeginInit();
            this.pnlCacheJson.SuspendLayout();
            this.tableLayoutPanelJason.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheJsonExpiry)).BeginInit();
            this.pnlCacheSound.SuspendLayout();
            this.tableLayoutPanelSound.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheSoundExpiry)).BeginInit();
            this.pnlCacheImage.SuspendLayout();
            this.tableLayoutPanelImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheImageExpiry)).BeginInit();
            this.tabUpdates.SuspendLayout();
            this.pnlUpdateChannel.SuspendLayout();
            this.tableUpdateChannel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpdateInterval)).BeginInit();
            this.tabEndpoint.SuspendLayout();
            this.pnlEndpointHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEndpointHistory)).BeginInit();
            this.tlsEndpointHistory.SuspendLayout();
            this.pnlEndpointSettings.SuspendLayout();
            this.tableEndpointSettings.SuspendLayout();
            this.pnlEndpointStatus.SuspendLayout();
            this.tableEndpointStatus.SuspendLayout();
            this.tabConstants.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConstVariables)).BeginInit();
            this.tlsConstant.SuspendLayout();
            this.tabSubs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubstitutions)).BeginInit();
            this.tlsSubstitutions.SuspendLayout();
            this.tabGameSpecific.SuspendLayout();
            this.tbcGames.SuspendLayout();
            this.tabFFXIV.SuspendLayout();
            this.pnlFfxivLogging.SuspendLayout();
            this.tableFfxivLogging.SuspendLayout();
            this.tabSecurity.SuspendLayout();
            this.pnlScriptingAccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdditionalFeatures)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApiAccess)).BeginInit();
            this.tabMiscellaneous.SuspendLayout();
            this.pnlAuraManagement.SuspendLayout();
            this.tableAuraManagement.SuspendLayout();
            this.pnlAutosave.SuspendLayout();
            this.tableAutosave.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutosaveMinutes)).BeginInit();
            this.pnlTriggerTemplate.SuspendLayout();
            this.tableTriggerTemplate.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMenu
            // 
            this.pnlMenu.AutoScroll = true;
            this.pnlMenu.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.pnlMenu.Controls.Add(this.btnMisc);
            this.pnlMenu.Controls.Add(this.btnSecurity);
            this.pnlMenu.Controls.Add(this.btnGameSpecific);
            this.pnlMenu.Controls.Add(this.btnSubstitutions);
            this.pnlMenu.Controls.Add(this.btnConstants);
            this.pnlMenu.Controls.Add(this.btnEndpoint);
            this.pnlMenu.Controls.Add(this.btnUpdates);
            this.pnlMenu.Controls.Add(this.btnCaching);
            this.pnlMenu.Controls.Add(this.btnShortcuts);
            this.pnlMenu.Controls.Add(this.btnLogging);
            this.pnlMenu.Controls.Add(this.btnAudio);
            this.pnlMenu.Controls.Add(this.btnUserInterface);
            this.pnlMenu.Controls.Add(this.btnStartup);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(293, 763);
            this.pnlMenu.TabIndex = 0;
            // 
            // btnMisc
            // 
            this.btnMisc.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnMisc.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnMisc.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMisc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMisc.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMisc.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnMisc.IconCharacter = "@";
            this.btnMisc.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnMisc.Location = new System.Drawing.Point(0, 552);
            this.btnMisc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMisc.Name = "btnMisc";
            this.btnMisc.Size = new System.Drawing.Size(293, 46);
            this.btnMisc.TabControl = this.tbcMain;
            this.btnMisc.TabIndex = 14;
            this.btnMisc.TabNumber = 12;
            this.btnMisc.Text = "Miscellaneous";
            this.btnMisc.UseVisualStyleBackColor = false;
            // 
            // tbcMain
            // 
            this.tbcMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tbcMain.Controls.Add(this.tabStartup);
            this.tbcMain.Controls.Add(this.tabUserInterface);
            this.tbcMain.Controls.Add(this.tabAudio);
            this.tbcMain.Controls.Add(this.tabLogging);
            this.tbcMain.Controls.Add(this.tabShortcuts);
            this.tbcMain.Controls.Add(this.tabCaching);
            this.tbcMain.Controls.Add(this.tabUpdates);
            this.tbcMain.Controls.Add(this.tabEndpoint);
            this.tbcMain.Controls.Add(this.tabConstants);
            this.tbcMain.Controls.Add(this.tabSubs);
            this.tbcMain.Controls.Add(this.tabGameSpecific);
            this.tbcMain.Controls.Add(this.tabSecurity);
            this.tbcMain.Controls.Add(this.tabMiscellaneous);
            this.tbcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcMain.Location = new System.Drawing.Point(11, 9);
            this.tbcMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.SelectedIndex = 0;
            this.tbcMain.Size = new System.Drawing.Size(864, 629);
            this.tbcMain.TabIndex = 2;
            this.tbcMain.TabStop = false;
            // 
            // tabStartup
            // 
            this.tabStartup.AutoScroll = true;
            this.tabStartup.Controls.Add(this.pnlStartupTrigger);
            this.tabStartup.Controls.Add(this.capStartupTrigger);
            this.tabStartup.Controls.Add(this.pnlStartup);
            this.tabStartup.Controls.Add(this.capStartupOptions);
            this.tabStartup.Location = new System.Drawing.Point(4, 28);
            this.tabStartup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabStartup.Name = "tabStartup";
            this.tabStartup.Size = new System.Drawing.Size(856, 597);
            this.tabStartup.TabIndex = 0;
            this.tabStartup.Text = "Startup";
            this.tabStartup.UseVisualStyleBackColor = true;
            // 
            // pnlStartupTrigger
            // 
            this.pnlStartupTrigger.Controls.Add(this.trvTrigger);
            this.pnlStartupTrigger.Controls.Add(this.tlsStartupTrigger);
            this.pnlStartupTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStartupTrigger.Location = new System.Drawing.Point(0, 146);
            this.pnlStartupTrigger.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlStartupTrigger.Name = "pnlStartupTrigger";
            this.pnlStartupTrigger.Size = new System.Drawing.Size(856, 451);
            this.pnlStartupTrigger.TabIndex = 10;
            // 
            // trvTrigger
            // 
            this.trvTrigger.ContextMenuStrip = this.ctxStartupTrigger;
            this.trvTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvTrigger.HideSelection = false;
            this.trvTrigger.Location = new System.Drawing.Point(0, 39);
            this.trvTrigger.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.trvTrigger.MinimumSize = new System.Drawing.Size(4, 57);
            this.trvTrigger.Name = "trvTrigger";
            this.trvTrigger.ShowNodeToolTips = true;
            this.trvTrigger.Size = new System.Drawing.Size(856, 412);
            this.trvTrigger.TabIndex = 2;
            this.trvTrigger.TabStop = false;
            this.trvTrigger.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvTrigger_BeforeCollapse);
            this.trvTrigger.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvTrigger_BeforeExpand);
            this.trvTrigger.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvTrigger_BeforeSelect);
            this.trvTrigger.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvTrigger_AfterSelect);
            // 
            // ctxStartupTrigger
            // 
            this.ctxStartupTrigger.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxStartupTrigger.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxClearSelection});
            this.ctxStartupTrigger.Name = "contextMenuStrip1";
            this.ctxStartupTrigger.Size = new System.Drawing.Size(190, 30);
            this.ctxStartupTrigger.Opening += new System.ComponentModel.CancelEventHandler(this.ctxStartupTrigger_Opening);
            // 
            // ctxClearSelection
            // 
            this.ctxClearSelection.Image = ((System.Drawing.Image)(resources.GetObject("ctxClearSelection.Image")));
            this.ctxClearSelection.Name = "ctxClearSelection";
            this.ctxClearSelection.Size = new System.Drawing.Size(189, 26);
            this.ctxClearSelection.Text = "Clear selection";
            this.ctxClearSelection.Click += new System.EventHandler(this.clearSelectionToolStripMenuItem_Click);
            // 
            // tlsStartupTrigger
            // 
            this.tlsStartupTrigger.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsStartupTrigger.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tlsStartupTrigger.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnClearSelection,
            this.lblFolderReminder});
            this.tlsStartupTrigger.Location = new System.Drawing.Point(0, 0);
            this.tlsStartupTrigger.Name = "tlsStartupTrigger";
            this.tlsStartupTrigger.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlsStartupTrigger.Size = new System.Drawing.Size(856, 39);
            this.tlsStartupTrigger.TabIndex = 3;
            // 
            // btnClearSelection
            // 
            this.btnClearSelection.Enabled = false;
            this.btnClearSelection.Image = ((System.Drawing.Image)(resources.GetObject("btnClearSelection.Image")));
            this.btnClearSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Padding = new System.Windows.Forms.Padding(4);
            this.btnClearSelection.Size = new System.Drawing.Size(148, 32);
            this.btnClearSelection.Text = "Clear selection";
            this.btnClearSelection.Click += new System.EventHandler(this.btnClearSelection_Click);
            // 
            // lblFolderReminder
            // 
            this.lblFolderReminder.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblFolderReminder.Image = ((System.Drawing.Image)(resources.GetObject("lblFolderReminder.Image")));
            this.lblFolderReminder.Name = "lblFolderReminder";
            this.lblFolderReminder.Size = new System.Drawing.Size(418, 32);
            this.lblFolderReminder.Text = "Selecting a folder will fire all triggers inside the folder";
            this.lblFolderReminder.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.lblFolderReminder.Visible = false;
            // 
            // capStartupTrigger
            // 
            this.capStartupTrigger.Caption = "Startup trigger/folder";
            this.capStartupTrigger.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capStartupTrigger.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capStartupTrigger.Collapsed = false;
            this.capStartupTrigger.Collapsee = null;
            this.capStartupTrigger.Collapsible = false;
            this.capStartupTrigger.Dock = System.Windows.Forms.DockStyle.Top;
            this.capStartupTrigger.Location = new System.Drawing.Point(0, 111);
            this.capStartupTrigger.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capStartupTrigger.Name = "capStartupTrigger";
            this.capStartupTrigger.Size = new System.Drawing.Size(856, 35);
            this.capStartupTrigger.TabIndex = 9;
            // 
            // pnlStartup
            // 
            this.pnlStartup.AutoSize = true;
            this.pnlStartup.Controls.Add(this.tableStartup);
            this.pnlStartup.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStartup.Location = new System.Drawing.Point(0, 35);
            this.pnlStartup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlStartup.Name = "pnlStartup";
            this.pnlStartup.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlStartup.Size = new System.Drawing.Size(856, 76);
            this.pnlStartup.TabIndex = 8;
            // 
            // tableStartup
            // 
            this.tableStartup.AutoSize = true;
            this.tableStartup.ColumnCount = 2;
            this.tableStartup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableStartup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableStartup.Controls.Add(this.lblWarnAdmin, 0, 1);
            this.tableStartup.Controls.Add(this.lblWelcome, 0, 0);
            this.tableStartup.Controls.Add(this.chkWarnAdmin, 1, 1);
            this.tableStartup.Controls.Add(this.chkWelcome, 1, 0);
            this.tableStartup.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableStartup.Location = new System.Drawing.Point(0, 9);
            this.tableStartup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableStartup.Name = "tableStartup";
            this.tableStartup.RowCount = 3;
            this.tableStartup.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableStartup.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableStartup.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableStartup.Size = new System.Drawing.Size(856, 58);
            this.tableStartup.TabIndex = 8;
            // 
            // lblWarnAdmin
            // 
            this.lblWarnAdmin.AutoEllipsis = true;
            this.lblWarnAdmin.AutoSize = true;
            this.lblWarnAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWarnAdmin.Location = new System.Drawing.Point(4, 29);
            this.lblWarnAdmin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWarnAdmin.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblWarnAdmin.Name = "lblWarnAdmin";
            this.lblWarnAdmin.Size = new System.Drawing.Size(295, 29);
            this.lblWarnAdmin.TabIndex = 14;
            this.lblWarnAdmin.Text = "Warn if not running as Administrator";
            this.lblWarnAdmin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoEllipsis = true;
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWelcome.Location = new System.Drawing.Point(4, 0);
            this.lblWelcome.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWelcome.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(295, 29);
            this.lblWelcome.TabIndex = 13;
            this.lblWelcome.Text = "Show Welcome Screen";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkWarnAdmin
            // 
            this.chkWarnAdmin.AutoSize = true;
            this.chkWarnAdmin.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkWarnAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkWarnAdmin.Location = new System.Drawing.Point(307, 32);
            this.chkWarnAdmin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkWarnAdmin.Name = "chkWarnAdmin";
            this.chkWarnAdmin.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkWarnAdmin.Size = new System.Drawing.Size(545, 23);
            this.chkWarnAdmin.TabIndex = 2;
            this.chkWarnAdmin.TabStop = false;
            this.chkWarnAdmin.UseVisualStyleBackColor = true;
            // 
            // chkWelcome
            // 
            this.chkWelcome.AutoSize = true;
            this.chkWelcome.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkWelcome.Location = new System.Drawing.Point(307, 3);
            this.chkWelcome.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkWelcome.Name = "chkWelcome";
            this.chkWelcome.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkWelcome.Size = new System.Drawing.Size(545, 23);
            this.chkWelcome.TabIndex = 4;
            this.chkWelcome.TabStop = false;
            this.chkWelcome.UseVisualStyleBackColor = true;
            // 
            // capStartupOptions
            // 
            this.capStartupOptions.Caption = "Startup options";
            this.capStartupOptions.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capStartupOptions.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capStartupOptions.Collapsed = false;
            this.capStartupOptions.Collapsee = null;
            this.capStartupOptions.Collapsible = false;
            this.capStartupOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.capStartupOptions.Location = new System.Drawing.Point(0, 0);
            this.capStartupOptions.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capStartupOptions.Name = "capStartupOptions";
            this.capStartupOptions.Size = new System.Drawing.Size(856, 35);
            this.capStartupOptions.TabIndex = 6;
            // 
            // tabUserInterface
            // 
            this.tabUserInterface.Controls.Add(this.pnlUiOptions);
            this.tabUserInterface.Controls.Add(this.capBehavior);
            this.tabUserInterface.Controls.Add(this.pnlUiFont);
            this.tabUserInterface.Controls.Add(this.capAppearance);
            this.tabUserInterface.Location = new System.Drawing.Point(4, 28);
            this.tabUserInterface.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabUserInterface.Name = "tabUserInterface";
            this.tabUserInterface.Size = new System.Drawing.Size(856, 597);
            this.tabUserInterface.TabIndex = 12;
            this.tabUserInterface.Text = "User interface";
            this.tabUserInterface.UseVisualStyleBackColor = true;
            // 
            // pnlUiOptions
            // 
            this.pnlUiOptions.AutoSize = true;
            this.pnlUiOptions.Controls.Add(this.tableUiOptions);
            this.pnlUiOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUiOptions.Location = new System.Drawing.Point(0, 150);
            this.pnlUiOptions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlUiOptions.Name = "pnlUiOptions";
            this.pnlUiOptions.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.pnlUiOptions.Size = new System.Drawing.Size(856, 125);
            this.pnlUiOptions.TabIndex = 10;
            // 
            // tableUiOptions
            // 
            this.tableUiOptions.AutoSize = true;
            this.tableUiOptions.ColumnCount = 2;
            this.tableUiOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableUiOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableUiOptions.Controls.Add(this.lblDevMode, 0, 3);
            this.tableUiOptions.Controls.Add(this.lblAutoComplete, 0, 2);
            this.tableUiOptions.Controls.Add(this.lblActionAsync, 0, 1);
            this.tableUiOptions.Controls.Add(this.lblTestIgnoreConditions, 0, 0);
            this.tableUiOptions.Controls.Add(this.cbxAutoComplete, 1, 2);
            this.tableUiOptions.Controls.Add(this.cbxDevMode, 1, 3);
            this.tableUiOptions.Controls.Add(this.cbxActionAsync, 1, 1);
            this.tableUiOptions.Controls.Add(this.cbxTestIgnoreConditions, 1, 0);
            this.tableUiOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableUiOptions.Location = new System.Drawing.Point(0, 9);
            this.tableUiOptions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableUiOptions.Name = "tableUiOptions";
            this.tableUiOptions.RowCount = 4;
            this.tableUiOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiOptions.Size = new System.Drawing.Size(856, 116);
            this.tableUiOptions.TabIndex = 2;
            // 
            // lblDevMode
            // 
            this.lblDevMode.AutoEllipsis = true;
            this.lblDevMode.AutoSize = true;
            this.lblDevMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDevMode.Location = new System.Drawing.Point(4, 87);
            this.lblDevMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDevMode.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblDevMode.Name = "lblDevMode";
            this.lblDevMode.Size = new System.Drawing.Size(503, 29);
            this.lblDevMode.TabIndex = 15;
            this.lblDevMode.Text = "Developer mode";
            this.lblDevMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAutoComplete
            // 
            this.lblAutoComplete.AutoEllipsis = true;
            this.lblAutoComplete.AutoSize = true;
            this.lblAutoComplete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAutoComplete.Location = new System.Drawing.Point(4, 58);
            this.lblAutoComplete.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAutoComplete.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblAutoComplete.Name = "lblAutoComplete";
            this.lblAutoComplete.Size = new System.Drawing.Size(503, 29);
            this.lblAutoComplete.TabIndex = 14;
            this.lblAutoComplete.Text = "Enable autocomplete on text fields";
            this.lblAutoComplete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblActionAsync
            // 
            this.lblActionAsync.AutoEllipsis = true;
            this.lblActionAsync.AutoSize = true;
            this.lblActionAsync.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblActionAsync.Location = new System.Drawing.Point(4, 29);
            this.lblActionAsync.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActionAsync.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblActionAsync.Name = "lblActionAsync";
            this.lblActionAsync.Size = new System.Drawing.Size(503, 29);
            this.lblActionAsync.TabIndex = 13;
            this.lblActionAsync.Text = "New actions asynchronous by default";
            this.lblActionAsync.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTestIgnoreConditions
            // 
            this.lblTestIgnoreConditions.AutoEllipsis = true;
            this.lblTestIgnoreConditions.AutoSize = true;
            this.lblTestIgnoreConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTestIgnoreConditions.Location = new System.Drawing.Point(4, 0);
            this.lblTestIgnoreConditions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTestIgnoreConditions.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblTestIgnoreConditions.Name = "lblTestIgnoreConditions";
            this.lblTestIgnoreConditions.Size = new System.Drawing.Size(503, 29);
            this.lblTestIgnoreConditions.TabIndex = 12;
            this.lblTestIgnoreConditions.Text = "Ignore conditions as default when testing actions";
            this.lblTestIgnoreConditions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxAutoComplete
            // 
            this.cbxAutoComplete.AutoSize = true;
            this.cbxAutoComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAutoComplete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxAutoComplete.Location = new System.Drawing.Point(515, 61);
            this.cbxAutoComplete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxAutoComplete.Name = "cbxAutoComplete";
            this.cbxAutoComplete.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.cbxAutoComplete.Size = new System.Drawing.Size(337, 23);
            this.cbxAutoComplete.TabIndex = 10;
            this.cbxAutoComplete.TabStop = false;
            this.cbxAutoComplete.UseVisualStyleBackColor = true;
            // 
            // cbxDevMode
            // 
            this.cbxDevMode.AutoSize = true;
            this.cbxDevMode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxDevMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxDevMode.Location = new System.Drawing.Point(515, 90);
            this.cbxDevMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxDevMode.Name = "cbxDevMode";
            this.cbxDevMode.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.cbxDevMode.Size = new System.Drawing.Size(337, 23);
            this.cbxDevMode.TabIndex = 9;
            this.cbxDevMode.TabStop = false;
            this.cbxDevMode.UseVisualStyleBackColor = true;
            // 
            // cbxActionAsync
            // 
            this.cbxActionAsync.AutoSize = true;
            this.cbxActionAsync.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxActionAsync.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxActionAsync.Location = new System.Drawing.Point(515, 32);
            this.cbxActionAsync.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxActionAsync.Name = "cbxActionAsync";
            this.cbxActionAsync.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.cbxActionAsync.Size = new System.Drawing.Size(337, 23);
            this.cbxActionAsync.TabIndex = 8;
            this.cbxActionAsync.TabStop = false;
            this.cbxActionAsync.UseVisualStyleBackColor = true;
            // 
            // cbxTestIgnoreConditions
            // 
            this.cbxTestIgnoreConditions.AutoSize = true;
            this.cbxTestIgnoreConditions.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxTestIgnoreConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxTestIgnoreConditions.Location = new System.Drawing.Point(515, 3);
            this.cbxTestIgnoreConditions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxTestIgnoreConditions.Name = "cbxTestIgnoreConditions";
            this.cbxTestIgnoreConditions.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.cbxTestIgnoreConditions.Size = new System.Drawing.Size(337, 23);
            this.cbxTestIgnoreConditions.TabIndex = 7;
            this.cbxTestIgnoreConditions.UseVisualStyleBackColor = true;
            // 
            // capBehavior
            // 
            this.capBehavior.Caption = "Behavior";
            this.capBehavior.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capBehavior.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capBehavior.Collapsed = false;
            this.capBehavior.Collapsee = null;
            this.capBehavior.Collapsible = false;
            this.capBehavior.Dock = System.Windows.Forms.DockStyle.Top;
            this.capBehavior.Location = new System.Drawing.Point(0, 115);
            this.capBehavior.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capBehavior.Name = "capBehavior";
            this.capBehavior.Size = new System.Drawing.Size(856, 35);
            this.capBehavior.TabIndex = 9;
            // 
            // pnlUiFont
            // 
            this.pnlUiFont.AutoSize = true;
            this.pnlUiFont.Controls.Add(this.tableUiFont);
            this.pnlUiFont.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUiFont.Location = new System.Drawing.Point(0, 35);
            this.pnlUiFont.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlUiFont.Name = "pnlUiFont";
            this.pnlUiFont.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlUiFont.Size = new System.Drawing.Size(856, 80);
            this.pnlUiFont.TabIndex = 8;
            // 
            // tableUiFont
            // 
            this.tableUiFont.AutoSize = true;
            this.tableUiFont.ColumnCount = 3;
            this.tableUiFont.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableUiFont.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableUiFont.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableUiFont.Controls.Add(this.lblUiFontDefault, 0, 0);
            this.tableUiFont.Controls.Add(this.cbxUiFontDefault, 1, 0);
            this.tableUiFont.Controls.Add(this.lblUiFont, 0, 1);
            this.tableUiFont.Controls.Add(this.txtUiFontDesc, 1, 1);
            this.tableUiFont.Controls.Add(this.btnUiFont, 2, 1);
            this.tableUiFont.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableUiFont.Location = new System.Drawing.Point(0, 9);
            this.tableUiFont.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableUiFont.Name = "tableUiFont";
            this.tableUiFont.RowCount = 2;
            this.tableUiFont.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiFont.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUiFont.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableUiFont.Size = new System.Drawing.Size(856, 62);
            this.tableUiFont.TabIndex = 2;
            // 
            // lblUiFontDefault
            // 
            this.lblUiFontDefault.AutoEllipsis = true;
            this.lblUiFontDefault.AutoSize = true;
            this.lblUiFontDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUiFontDefault.Location = new System.Drawing.Point(4, 0);
            this.lblUiFontDefault.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUiFontDefault.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblUiFontDefault.Name = "lblUiFontDefault";
            this.lblUiFontDefault.Size = new System.Drawing.Size(200, 29);
            this.lblUiFontDefault.TabIndex = 5;
            this.lblUiFontDefault.Text = "Use default font";
            this.lblUiFontDefault.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxUiFontDefault
            // 
            this.cbxUiFontDefault.AutoSize = true;
            this.cbxUiFontDefault.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableUiFont.SetColumnSpan(this.cbxUiFontDefault, 2);
            this.cbxUiFontDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxUiFontDefault.Location = new System.Drawing.Point(212, 3);
            this.cbxUiFontDefault.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxUiFontDefault.Name = "cbxUiFontDefault";
            this.cbxUiFontDefault.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.cbxUiFontDefault.Size = new System.Drawing.Size(640, 23);
            this.cbxUiFontDefault.TabIndex = 0;
            this.cbxUiFontDefault.TabStop = false;
            this.cbxUiFontDefault.UseVisualStyleBackColor = true;
            this.cbxUiFontDefault.CheckedChanged += new System.EventHandler(this.cbxUiFontDefault_CheckedChanged);
            // 
            // lblUiFont
            // 
            this.lblUiFont.AutoEllipsis = true;
            this.lblUiFont.AutoSize = true;
            this.lblUiFont.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUiFont.Location = new System.Drawing.Point(4, 29);
            this.lblUiFont.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUiFont.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblUiFont.Name = "lblUiFont";
            this.lblUiFont.Size = new System.Drawing.Size(200, 33);
            this.lblUiFont.TabIndex = 2;
            this.lblUiFont.Text = "Custom font";
            this.lblUiFont.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUiFontDesc
            // 
            this.txtUiFontDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtUiFontDesc.Location = new System.Drawing.Point(212, 32);
            this.txtUiFontDesc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUiFontDesc.Name = "txtUiFontDesc";
            this.txtUiFontDesc.ReadOnly = true;
            this.txtUiFontDesc.Size = new System.Drawing.Size(573, 25);
            this.txtUiFontDesc.TabIndex = 3;
            // 
            // btnUiFont
            // 
            this.btnUiFont.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUiFont.Image = ((System.Drawing.Image)(resources.GetObject("btnUiFont.Image")));
            this.btnUiFont.Location = new System.Drawing.Point(793, 32);
            this.btnUiFont.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUiFont.Name = "btnUiFont";
            this.btnUiFont.Size = new System.Drawing.Size(59, 27);
            this.btnUiFont.TabIndex = 4;
            this.btnUiFont.UseVisualStyleBackColor = true;
            this.btnUiFont.Click += new System.EventHandler(this.btnUiFont_Click);
            // 
            // capAppearance
            // 
            this.capAppearance.Caption = "Appearance";
            this.capAppearance.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capAppearance.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capAppearance.Collapsed = false;
            this.capAppearance.Collapsee = null;
            this.capAppearance.Collapsible = false;
            this.capAppearance.Dock = System.Windows.Forms.DockStyle.Top;
            this.capAppearance.Location = new System.Drawing.Point(0, 0);
            this.capAppearance.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capAppearance.Name = "capAppearance";
            this.capAppearance.Size = new System.Drawing.Size(856, 35);
            this.capAppearance.TabIndex = 7;
            // 
            // tabAudio
            // 
            this.tabAudio.AutoScroll = true;
            this.tabAudio.Controls.Add(this.panelAudioTts);
            this.tabAudio.Controls.Add(this.capAudioTts);
            this.tabAudio.Controls.Add(this.panelAudioSound);
            this.tabAudio.Controls.Add(this.capAudioSound);
            this.tabAudio.Controls.Add(this.panelAudioBeep);
            this.tabAudio.Controls.Add(this.capAudioBeep);
            this.tabAudio.Location = new System.Drawing.Point(4, 28);
            this.tabAudio.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabAudio.Name = "tabAudio";
            this.tabAudio.Size = new System.Drawing.Size(856, 597);
            this.tabAudio.TabIndex = 1;
            this.tabAudio.Text = "Audio";
            this.tabAudio.UseVisualStyleBackColor = true;
            // 
            // panelAudioTts
            // 
            this.panelAudioTts.AutoSize = true;
            this.panelAudioTts.Controls.Add(this.tableAudioTts);
            this.panelAudioTts.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAudioTts.Location = new System.Drawing.Point(0, 599);
            this.panelAudioTts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelAudioTts.Name = "panelAudioTts";
            this.panelAudioTts.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.panelAudioTts.Size = new System.Drawing.Size(835, 311);
            this.panelAudioTts.TabIndex = 12;
            // 
            // tableAudioTts
            // 
            this.tableAudioTts.AutoSize = true;
            this.tableAudioTts.ColumnCount = 3;
            this.tableAudioTts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAudioTts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAudioTts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableAudioTts.Controls.Add(this.lblTtsRepV, 2, 2);
            this.tableAudioTts.Controls.Add(this.trbTtsRepetition, 1, 2);
            this.tableAudioTts.Controls.Add(this.lblTtsRepetition, 0, 2);
            this.tableAudioTts.Controls.Add(this.txtTtsPathArgs, 1, 5);
            this.tableAudioTts.Controls.Add(this.lblTtsPathArgs, 0, 5);
            this.tableAudioTts.Controls.Add(this.btnTtsPath, 2, 4);
            this.tableAudioTts.Controls.Add(this.txtTtsPath, 1, 4);
            this.tableAudioTts.Controls.Add(this.cbxTtsMethod, 1, 0);
            this.tableAudioTts.Controls.Add(this.lblTtsPath, 0, 4);
            this.tableAudioTts.Controls.Add(this.lblTtsMethod, 0, 0);
            this.tableAudioTts.Controls.Add(this.lblTtsVolP, 2, 1);
            this.tableAudioTts.Controls.Add(this.trbTtsVolume, 1, 1);
            this.tableAudioTts.Controls.Add(this.lblTtsVolume, 0, 1);
            this.tableAudioTts.Controls.Add(this.pnlTtsWarnings, 1, 3);
            this.tableAudioTts.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableAudioTts.Location = new System.Drawing.Point(0, 9);
            this.tableAudioTts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableAudioTts.Name = "tableAudioTts";
            this.tableAudioTts.RowCount = 6;
            this.tableAudioTts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioTts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioTts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioTts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioTts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioTts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioTts.Size = new System.Drawing.Size(835, 302);
            this.tableAudioTts.TabIndex = 1;
            // 
            // lblTtsRepV
            // 
            this.lblTtsRepV.AutoEllipsis = true;
            this.lblTtsRepV.AutoSize = true;
            this.lblTtsRepV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTtsRepV.Location = new System.Drawing.Point(732, 60);
            this.lblTtsRepV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTtsRepV.Name = "lblTtsRepV";
            this.lblTtsRepV.Size = new System.Drawing.Size(99, 31);
            this.lblTtsRepV.TabIndex = 33;
            this.lblTtsRepV.Text = "500 ms";
            this.lblTtsRepV.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trbTtsRepetition
            // 
            this.trbTtsRepetition.AutoSize = false;
            this.trbTtsRepetition.Dock = System.Windows.Forms.DockStyle.Top;
            this.trbTtsRepetition.LargeChange = 250;
            this.trbTtsRepetition.Location = new System.Drawing.Point(212, 63);
            this.trbTtsRepetition.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.trbTtsRepetition.Maximum = 3000;
            this.trbTtsRepetition.Name = "trbTtsRepetition";
            this.trbTtsRepetition.Size = new System.Drawing.Size(512, 25);
            this.trbTtsRepetition.SmallChange = 100;
            this.trbTtsRepetition.TabIndex = 32;
            this.trbTtsRepetition.TabStop = false;
            this.trbTtsRepetition.TickFrequency = 100;
            this.trbTtsRepetition.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trbTtsRepetition.Value = 500;
            this.trbTtsRepetition.Scroll += new System.EventHandler(this.trbTtsRepetition_Scroll);
            this.trbTtsRepetition.ValueChanged += new System.EventHandler(this.trbTtsRepetition_ValueChanged);
            // 
            // lblTtsRepetition
            // 
            this.lblTtsRepetition.AutoEllipsis = true;
            this.lblTtsRepetition.AutoSize = true;
            this.lblTtsRepetition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTtsRepetition.Location = new System.Drawing.Point(4, 60);
            this.lblTtsRepetition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTtsRepetition.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblTtsRepetition.Name = "lblTtsRepetition";
            this.lblTtsRepetition.Size = new System.Drawing.Size(200, 31);
            this.lblTtsRepetition.TabIndex = 31;
            this.lblTtsRepetition.Text = "Repetition cooldown";
            this.lblTtsRepetition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTtsPathArgs
            // 
            this.tableAudioTts.SetColumnSpan(this.txtTtsPathArgs, 2);
            this.txtTtsPathArgs.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTtsPathArgs.Location = new System.Drawing.Point(212, 274);
            this.txtTtsPathArgs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtTtsPathArgs.Name = "txtTtsPathArgs";
            this.txtTtsPathArgs.Size = new System.Drawing.Size(619, 25);
            this.txtTtsPathArgs.TabIndex = 29;
            // 
            // lblTtsPathArgs
            // 
            this.lblTtsPathArgs.AutoEllipsis = true;
            this.lblTtsPathArgs.AutoSize = true;
            this.lblTtsPathArgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTtsPathArgs.Location = new System.Drawing.Point(4, 271);
            this.lblTtsPathArgs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTtsPathArgs.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblTtsPathArgs.Name = "lblTtsPathArgs";
            this.lblTtsPathArgs.Size = new System.Drawing.Size(200, 31);
            this.lblTtsPathArgs.TabIndex = 28;
            this.lblTtsPathArgs.Text = "Arguments";
            this.lblTtsPathArgs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnTtsPath
            // 
            this.btnTtsPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTtsPath.Image = ((System.Drawing.Image)(resources.GetObject("btnTtsPath.Image")));
            this.btnTtsPath.Location = new System.Drawing.Point(728, 235);
            this.btnTtsPath.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnTtsPath.Name = "btnTtsPath";
            this.btnTtsPath.Size = new System.Drawing.Size(103, 36);
            this.btnTtsPath.TabIndex = 23;
            this.btnTtsPath.TabStop = false;
            this.btnTtsPath.UseVisualStyleBackColor = true;
            // 
            // txtTtsPath
            // 
            this.txtTtsPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTtsPath.Location = new System.Drawing.Point(212, 238);
            this.txtTtsPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtTtsPath.Name = "txtTtsPath";
            this.txtTtsPath.Size = new System.Drawing.Size(512, 25);
            this.txtTtsPath.TabIndex = 22;
            // 
            // cbxTtsMethod
            // 
            this.tableAudioTts.SetColumnSpan(this.cbxTtsMethod, 2);
            this.cbxTtsMethod.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxTtsMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTtsMethod.FormattingEnabled = true;
            this.cbxTtsMethod.Items.AddRange(new object[] {
            "None (disabled)",
            "Windows Speech API",
            "ACT",
            "External application"});
            this.cbxTtsMethod.Location = new System.Drawing.Point(212, 3);
            this.cbxTtsMethod.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxTtsMethod.Name = "cbxTtsMethod";
            this.cbxTtsMethod.Size = new System.Drawing.Size(619, 23);
            this.cbxTtsMethod.TabIndex = 12;
            this.cbxTtsMethod.SelectedIndexChanged += new System.EventHandler(this.cbxTtsMethod_SelectedIndexChanged);
            // 
            // lblTtsPath
            // 
            this.lblTtsPath.AutoEllipsis = true;
            this.lblTtsPath.AutoSize = true;
            this.lblTtsPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTtsPath.Location = new System.Drawing.Point(4, 235);
            this.lblTtsPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTtsPath.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblTtsPath.Name = "lblTtsPath";
            this.lblTtsPath.Size = new System.Drawing.Size(200, 36);
            this.lblTtsPath.TabIndex = 11;
            this.lblTtsPath.Text = "Application path";
            this.lblTtsPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTtsMethod
            // 
            this.lblTtsMethod.AutoEllipsis = true;
            this.lblTtsMethod.AutoSize = true;
            this.lblTtsMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTtsMethod.Location = new System.Drawing.Point(4, 0);
            this.lblTtsMethod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTtsMethod.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblTtsMethod.Name = "lblTtsMethod";
            this.lblTtsMethod.Size = new System.Drawing.Size(200, 29);
            this.lblTtsMethod.TabIndex = 9;
            this.lblTtsMethod.Text = "Playback method";
            this.lblTtsMethod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTtsVolP
            // 
            this.lblTtsVolP.AutoEllipsis = true;
            this.lblTtsVolP.AutoSize = true;
            this.lblTtsVolP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTtsVolP.Location = new System.Drawing.Point(732, 29);
            this.lblTtsVolP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTtsVolP.Name = "lblTtsVolP";
            this.lblTtsVolP.Size = new System.Drawing.Size(99, 31);
            this.lblTtsVolP.TabIndex = 7;
            this.lblTtsVolP.Text = "100 %";
            this.lblTtsVolP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trbTtsVolume
            // 
            this.trbTtsVolume.AutoSize = false;
            this.trbTtsVolume.Dock = System.Windows.Forms.DockStyle.Top;
            this.trbTtsVolume.Location = new System.Drawing.Point(212, 32);
            this.trbTtsVolume.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.trbTtsVolume.Maximum = 100;
            this.trbTtsVolume.Name = "trbTtsVolume";
            this.trbTtsVolume.Size = new System.Drawing.Size(512, 25);
            this.trbTtsVolume.TabIndex = 6;
            this.trbTtsVolume.TabStop = false;
            this.trbTtsVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trbTtsVolume.Value = 100;
            this.trbTtsVolume.Scroll += new System.EventHandler(this.trbTtsVolume_Scroll);
            this.trbTtsVolume.ValueChanged += new System.EventHandler(this.trbTtsVolume_ValueChanged);
            // 
            // lblTtsVolume
            // 
            this.lblTtsVolume.AutoEllipsis = true;
            this.lblTtsVolume.AutoSize = true;
            this.lblTtsVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTtsVolume.Location = new System.Drawing.Point(4, 29);
            this.lblTtsVolume.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTtsVolume.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblTtsVolume.Name = "lblTtsVolume";
            this.lblTtsVolume.Size = new System.Drawing.Size(200, 31);
            this.lblTtsVolume.TabIndex = 5;
            this.lblTtsVolume.Text = "Volume";
            this.lblTtsVolume.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTtsWarnings
            // 
            this.pnlTtsWarnings.AutoSize = true;
            this.tableAudioTts.SetColumnSpan(this.pnlTtsWarnings, 2);
            this.pnlTtsWarnings.Controls.Add(this.lblExternalTtsWarn);
            this.pnlTtsWarnings.Controls.Add(this.lblActTtsWarn);
            this.pnlTtsWarnings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTtsWarnings.Location = new System.Drawing.Point(212, 94);
            this.pnlTtsWarnings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlTtsWarnings.Name = "pnlTtsWarnings";
            this.pnlTtsWarnings.Size = new System.Drawing.Size(619, 138);
            this.pnlTtsWarnings.TabIndex = 30;
            // 
            // lblExternalTtsWarn
            // 
            this.lblExternalTtsWarn.AutoEllipsis = true;
            this.lblExternalTtsWarn.BackColor = System.Drawing.SystemColors.Info;
            this.lblExternalTtsWarn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblExternalTtsWarn.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblExternalTtsWarn.Location = new System.Drawing.Point(0, 69);
            this.lblExternalTtsWarn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExternalTtsWarn.Name = "lblExternalTtsWarn";
            this.lblExternalTtsWarn.Padding = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.lblExternalTtsWarn.Size = new System.Drawing.Size(619, 69);
            this.lblExternalTtsWarn.TabIndex = 30;
            this.lblExternalTtsWarn.Text = "This option will make Triggernometry launch an external application. Use this opt" +
    "ion with caution, and only if you understand what you are doing, as this may exp" +
    "ose your system to risk of malware.";
            this.lblExternalTtsWarn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblActTtsWarn
            // 
            this.lblActTtsWarn.AutoEllipsis = true;
            this.lblActTtsWarn.BackColor = System.Drawing.SystemColors.Info;
            this.lblActTtsWarn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblActTtsWarn.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblActTtsWarn.Location = new System.Drawing.Point(0, 0);
            this.lblActTtsWarn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActTtsWarn.Name = "lblActTtsWarn";
            this.lblActTtsWarn.Padding = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.lblActTtsWarn.Size = new System.Drawing.Size(619, 69);
            this.lblActTtsWarn.TabIndex = 29;
            this.lblActTtsWarn.Text = "Using ACT for text-to-speech will cause the rate and volume options on text-to-sp" +
    "eech actions to be ignored.";
            this.lblActTtsWarn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // capAudioTts
            // 
            this.capAudioTts.Caption = "Text-to-speech";
            this.capAudioTts.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capAudioTts.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capAudioTts.Collapsed = false;
            this.capAudioTts.Collapsee = null;
            this.capAudioTts.Collapsible = false;
            this.capAudioTts.Dock = System.Windows.Forms.DockStyle.Top;
            this.capAudioTts.Location = new System.Drawing.Point(0, 564);
            this.capAudioTts.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capAudioTts.Name = "capAudioTts";
            this.capAudioTts.Size = new System.Drawing.Size(835, 35);
            this.capAudioTts.TabIndex = 11;
            // 
            // panelAudioSound
            // 
            this.panelAudioSound.AutoSize = true;
            this.panelAudioSound.Controls.Add(this.tableAudioSound);
            this.panelAudioSound.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAudioSound.Location = new System.Drawing.Point(0, 313);
            this.panelAudioSound.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelAudioSound.Name = "panelAudioSound";
            this.panelAudioSound.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.panelAudioSound.Size = new System.Drawing.Size(835, 251);
            this.panelAudioSound.TabIndex = 10;
            // 
            // tableAudioSound
            // 
            this.tableAudioSound.AutoSize = true;
            this.tableAudioSound.ColumnCount = 3;
            this.tableAudioSound.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAudioSound.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAudioSound.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableAudioSound.Controls.Add(this.lblSoundRepV, 2, 2);
            this.tableAudioSound.Controls.Add(this.trbSoundRepetition, 1, 2);
            this.tableAudioSound.Controls.Add(this.lblSoundRepetition, 0, 2);
            this.tableAudioSound.Controls.Add(this.txtSoundPathArgs, 1, 5);
            this.tableAudioSound.Controls.Add(this.lblSoundPathArgs, 0, 5);
            this.tableAudioSound.Controls.Add(this.pnlExternalSoundWarning, 1, 3);
            this.tableAudioSound.Controls.Add(this.btnSoundPath, 2, 4);
            this.tableAudioSound.Controls.Add(this.lblSoundPath, 0, 4);
            this.tableAudioSound.Controls.Add(this.lblSoundMethod, 0, 0);
            this.tableAudioSound.Controls.Add(this.lblSoundVolP, 2, 1);
            this.tableAudioSound.Controls.Add(this.trbSoundVolume, 1, 1);
            this.tableAudioSound.Controls.Add(this.lblSoundVolume, 0, 1);
            this.tableAudioSound.Controls.Add(this.cbxSoundMethod, 1, 0);
            this.tableAudioSound.Controls.Add(this.txtSoundPath, 1, 4);
            this.tableAudioSound.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableAudioSound.Location = new System.Drawing.Point(0, 9);
            this.tableAudioSound.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableAudioSound.Name = "tableAudioSound";
            this.tableAudioSound.RowCount = 6;
            this.tableAudioSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioSound.Size = new System.Drawing.Size(835, 233);
            this.tableAudioSound.TabIndex = 1;
            // 
            // lblSoundRepV
            // 
            this.lblSoundRepV.AutoEllipsis = true;
            this.lblSoundRepV.AutoSize = true;
            this.lblSoundRepV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoundRepV.Location = new System.Drawing.Point(732, 60);
            this.lblSoundRepV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSoundRepV.Name = "lblSoundRepV";
            this.lblSoundRepV.Size = new System.Drawing.Size(99, 31);
            this.lblSoundRepV.TabIndex = 31;
            this.lblSoundRepV.Text = "500 ms";
            this.lblSoundRepV.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trbSoundRepetition
            // 
            this.trbSoundRepetition.AutoSize = false;
            this.trbSoundRepetition.Dock = System.Windows.Forms.DockStyle.Top;
            this.trbSoundRepetition.LargeChange = 250;
            this.trbSoundRepetition.Location = new System.Drawing.Point(212, 63);
            this.trbSoundRepetition.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.trbSoundRepetition.Maximum = 3000;
            this.trbSoundRepetition.Name = "trbSoundRepetition";
            this.trbSoundRepetition.Size = new System.Drawing.Size(512, 25);
            this.trbSoundRepetition.SmallChange = 100;
            this.trbSoundRepetition.TabIndex = 30;
            this.trbSoundRepetition.TabStop = false;
            this.trbSoundRepetition.TickFrequency = 100;
            this.trbSoundRepetition.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trbSoundRepetition.Value = 500;
            this.trbSoundRepetition.Scroll += new System.EventHandler(this.trbSoundRepetition_Scroll);
            this.trbSoundRepetition.ValueChanged += new System.EventHandler(this.trbSoundRepetition_ValueChanged);
            // 
            // lblSoundRepetition
            // 
            this.lblSoundRepetition.AutoEllipsis = true;
            this.lblSoundRepetition.AutoSize = true;
            this.lblSoundRepetition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoundRepetition.Location = new System.Drawing.Point(4, 60);
            this.lblSoundRepetition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSoundRepetition.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblSoundRepetition.Name = "lblSoundRepetition";
            this.lblSoundRepetition.Size = new System.Drawing.Size(200, 31);
            this.lblSoundRepetition.TabIndex = 29;
            this.lblSoundRepetition.Text = "Repetition cooldown";
            this.lblSoundRepetition.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSoundPathArgs
            // 
            this.tableAudioSound.SetColumnSpan(this.txtSoundPathArgs, 2);
            this.txtSoundPathArgs.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSoundPathArgs.Location = new System.Drawing.Point(212, 205);
            this.txtSoundPathArgs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSoundPathArgs.Name = "txtSoundPathArgs";
            this.txtSoundPathArgs.Size = new System.Drawing.Size(619, 25);
            this.txtSoundPathArgs.TabIndex = 28;
            // 
            // lblSoundPathArgs
            // 
            this.lblSoundPathArgs.AutoEllipsis = true;
            this.lblSoundPathArgs.AutoSize = true;
            this.lblSoundPathArgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoundPathArgs.Location = new System.Drawing.Point(4, 202);
            this.lblSoundPathArgs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSoundPathArgs.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblSoundPathArgs.Name = "lblSoundPathArgs";
            this.lblSoundPathArgs.Size = new System.Drawing.Size(200, 31);
            this.lblSoundPathArgs.TabIndex = 27;
            this.lblSoundPathArgs.Text = "Arguments";
            this.lblSoundPathArgs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlExternalSoundWarning
            // 
            this.pnlExternalSoundWarning.AutoSize = true;
            this.tableAudioSound.SetColumnSpan(this.pnlExternalSoundWarning, 2);
            this.pnlExternalSoundWarning.Controls.Add(this.lblExternalSoundWarn);
            this.pnlExternalSoundWarning.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlExternalSoundWarning.Location = new System.Drawing.Point(212, 94);
            this.pnlExternalSoundWarning.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlExternalSoundWarning.Name = "pnlExternalSoundWarning";
            this.pnlExternalSoundWarning.Size = new System.Drawing.Size(619, 69);
            this.pnlExternalSoundWarning.TabIndex = 26;
            // 
            // lblExternalSoundWarn
            // 
            this.lblExternalSoundWarn.AutoEllipsis = true;
            this.lblExternalSoundWarn.BackColor = System.Drawing.SystemColors.Info;
            this.lblExternalSoundWarn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblExternalSoundWarn.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblExternalSoundWarn.Location = new System.Drawing.Point(0, 0);
            this.lblExternalSoundWarn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExternalSoundWarn.Name = "lblExternalSoundWarn";
            this.lblExternalSoundWarn.Padding = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.lblExternalSoundWarn.Size = new System.Drawing.Size(619, 69);
            this.lblExternalSoundWarn.TabIndex = 28;
            this.lblExternalSoundWarn.Text = "This option will make Triggernometry launch an external application. Use this opt" +
    "ion with caution, and only if you understand what you are doing, as this may exp" +
    "ose your system to risk of malware.";
            this.lblExternalSoundWarn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSoundPath
            // 
            this.btnSoundPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSoundPath.Image = ((System.Drawing.Image)(resources.GetObject("btnSoundPath.Image")));
            this.btnSoundPath.Location = new System.Drawing.Point(728, 166);
            this.btnSoundPath.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.btnSoundPath.Name = "btnSoundPath";
            this.btnSoundPath.Size = new System.Drawing.Size(103, 36);
            this.btnSoundPath.TabIndex = 20;
            this.btnSoundPath.TabStop = false;
            this.btnSoundPath.UseVisualStyleBackColor = true;
            // 
            // lblSoundPath
            // 
            this.lblSoundPath.AutoEllipsis = true;
            this.lblSoundPath.AutoSize = true;
            this.lblSoundPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoundPath.Location = new System.Drawing.Point(4, 166);
            this.lblSoundPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSoundPath.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblSoundPath.Name = "lblSoundPath";
            this.lblSoundPath.Size = new System.Drawing.Size(200, 36);
            this.lblSoundPath.TabIndex = 10;
            this.lblSoundPath.Text = "Application path";
            this.lblSoundPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSoundMethod
            // 
            this.lblSoundMethod.AutoEllipsis = true;
            this.lblSoundMethod.AutoSize = true;
            this.lblSoundMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoundMethod.Location = new System.Drawing.Point(4, 0);
            this.lblSoundMethod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSoundMethod.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblSoundMethod.Name = "lblSoundMethod";
            this.lblSoundMethod.Size = new System.Drawing.Size(200, 29);
            this.lblSoundMethod.TabIndex = 8;
            this.lblSoundMethod.Text = "Playback method";
            this.lblSoundMethod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSoundVolP
            // 
            this.lblSoundVolP.AutoEllipsis = true;
            this.lblSoundVolP.AutoSize = true;
            this.lblSoundVolP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoundVolP.Location = new System.Drawing.Point(732, 29);
            this.lblSoundVolP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSoundVolP.Name = "lblSoundVolP";
            this.lblSoundVolP.Size = new System.Drawing.Size(99, 31);
            this.lblSoundVolP.TabIndex = 7;
            this.lblSoundVolP.Text = "100 %";
            this.lblSoundVolP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trbSoundVolume
            // 
            this.trbSoundVolume.AutoSize = false;
            this.trbSoundVolume.Dock = System.Windows.Forms.DockStyle.Top;
            this.trbSoundVolume.Location = new System.Drawing.Point(212, 32);
            this.trbSoundVolume.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.trbSoundVolume.Maximum = 100;
            this.trbSoundVolume.Name = "trbSoundVolume";
            this.trbSoundVolume.Size = new System.Drawing.Size(512, 25);
            this.trbSoundVolume.TabIndex = 6;
            this.trbSoundVolume.TabStop = false;
            this.trbSoundVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trbSoundVolume.Value = 100;
            this.trbSoundVolume.Scroll += new System.EventHandler(this.trbSoundVolume_Scroll);
            this.trbSoundVolume.ValueChanged += new System.EventHandler(this.trbSoundVolume_ValueChanged);
            // 
            // lblSoundVolume
            // 
            this.lblSoundVolume.AutoEllipsis = true;
            this.lblSoundVolume.AutoSize = true;
            this.lblSoundVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSoundVolume.Location = new System.Drawing.Point(4, 29);
            this.lblSoundVolume.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSoundVolume.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblSoundVolume.Name = "lblSoundVolume";
            this.lblSoundVolume.Size = new System.Drawing.Size(200, 31);
            this.lblSoundVolume.TabIndex = 5;
            this.lblSoundVolume.Text = "Volume";
            this.lblSoundVolume.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxSoundMethod
            // 
            this.tableAudioSound.SetColumnSpan(this.cbxSoundMethod, 2);
            this.cbxSoundMethod.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxSoundMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSoundMethod.FormattingEnabled = true;
            this.cbxSoundMethod.Items.AddRange(new object[] {
            "None (disabled)",
            "Windows Media Player API",
            "ACT",
            "External application"});
            this.cbxSoundMethod.Location = new System.Drawing.Point(212, 3);
            this.cbxSoundMethod.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxSoundMethod.Name = "cbxSoundMethod";
            this.cbxSoundMethod.Size = new System.Drawing.Size(619, 23);
            this.cbxSoundMethod.TabIndex = 9;
            this.cbxSoundMethod.SelectedIndexChanged += new System.EventHandler(this.cbxSoundMethod_SelectedIndexChanged);
            // 
            // txtSoundPath
            // 
            this.txtSoundPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSoundPath.Location = new System.Drawing.Point(212, 169);
            this.txtSoundPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtSoundPath.Name = "txtSoundPath";
            this.txtSoundPath.Size = new System.Drawing.Size(512, 25);
            this.txtSoundPath.TabIndex = 21;
            // 
            // capAudioSound
            // 
            this.capAudioSound.Caption = "Sound playback";
            this.capAudioSound.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capAudioSound.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capAudioSound.Collapsed = false;
            this.capAudioSound.Collapsee = null;
            this.capAudioSound.Collapsible = false;
            this.capAudioSound.Dock = System.Windows.Forms.DockStyle.Top;
            this.capAudioSound.Location = new System.Drawing.Point(0, 278);
            this.capAudioSound.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capAudioSound.Name = "capAudioSound";
            this.capAudioSound.Size = new System.Drawing.Size(835, 35);
            this.capAudioSound.TabIndex = 13;
            // 
            // panelAudioBeep
            // 
            this.panelAudioBeep.AutoSize = true;
            this.panelAudioBeep.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelAudioBeep.Controls.Add(this.tableAudioBeep);
            this.panelAudioBeep.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAudioBeep.Location = new System.Drawing.Point(0, 35);
            this.panelAudioBeep.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelAudioBeep.Name = "panelAudioBeep";
            this.panelAudioBeep.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.panelAudioBeep.Size = new System.Drawing.Size(835, 243);
            this.panelAudioBeep.TabIndex = 14;
            // 
            // tableAudioBeep
            // 
            this.tableAudioBeep.AutoSize = true;
            this.tableAudioBeep.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableAudioBeep.ColumnCount = 3;
            this.tableAudioBeep.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAudioBeep.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAudioBeep.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableAudioBeep.Controls.Add(this.lblUseSimulatedBeep, 0, 0);
            this.tableAudioBeep.Controls.Add(this.chkUseSimulatedBeep, 1, 0);
            this.tableAudioBeep.Controls.Add(this.lblBeepVolume, 0, 1);
            this.tableAudioBeep.Controls.Add(this.trbBeepVolume, 1, 1);
            this.tableAudioBeep.Controls.Add(this.lblBeepVolP, 2, 1);
            this.tableAudioBeep.Controls.Add(this.btnBeepTest, 2, 2);
            this.tableAudioBeep.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableAudioBeep.Location = new System.Drawing.Point(0, 9);
            this.tableAudioBeep.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableAudioBeep.Name = "tableAudioBeep";
            this.tableAudioBeep.RowCount = 3;
            this.tableAudioBeep.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioBeep.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioBeep.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAudioBeep.Size = new System.Drawing.Size(835, 225);
            this.tableAudioBeep.TabIndex = 0;
            // 
            // lblUseSimulatedBeep
            // 
            this.lblUseSimulatedBeep.AutoEllipsis = true;
            this.lblUseSimulatedBeep.AutoSize = true;
            this.lblUseSimulatedBeep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUseSimulatedBeep.Location = new System.Drawing.Point(4, 0);
            this.lblUseSimulatedBeep.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUseSimulatedBeep.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblUseSimulatedBeep.Name = "lblUseSimulatedBeep";
            this.lblUseSimulatedBeep.Size = new System.Drawing.Size(200, 29);
            this.lblUseSimulatedBeep.TabIndex = 0;
            this.lblUseSimulatedBeep.Text = "Use Simulated Beep";
            this.lblUseSimulatedBeep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkUseSimulatedBeep
            // 
            this.chkUseSimulatedBeep.AutoSize = true;
            this.chkUseSimulatedBeep.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableAudioBeep.SetColumnSpan(this.chkUseSimulatedBeep, 2);
            this.chkUseSimulatedBeep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkUseSimulatedBeep.Location = new System.Drawing.Point(212, 3);
            this.chkUseSimulatedBeep.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkUseSimulatedBeep.Name = "chkUseSimulatedBeep";
            this.chkUseSimulatedBeep.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkUseSimulatedBeep.Size = new System.Drawing.Size(619, 23);
            this.chkUseSimulatedBeep.TabIndex = 1;
            this.chkUseSimulatedBeep.TabStop = false;
            this.chkUseSimulatedBeep.UseVisualStyleBackColor = true;
            this.chkUseSimulatedBeep.CheckedChanged += new System.EventHandler(this.chkUseSimulatedBeep_CheckedChanged);
            // 
            // lblBeepVolume
            // 
            this.lblBeepVolume.AutoEllipsis = true;
            this.lblBeepVolume.AutoSize = true;
            this.lblBeepVolume.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBeepVolume.Location = new System.Drawing.Point(4, 29);
            this.lblBeepVolume.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBeepVolume.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblBeepVolume.Name = "lblBeepVolume";
            this.lblBeepVolume.Size = new System.Drawing.Size(200, 31);
            this.lblBeepVolume.TabIndex = 2;
            this.lblBeepVolume.Text = "Volume";
            this.lblBeepVolume.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trbBeepVolume
            // 
            this.trbBeepVolume.AutoSize = false;
            this.trbBeepVolume.Dock = System.Windows.Forms.DockStyle.Top;
            this.trbBeepVolume.Location = new System.Drawing.Point(212, 32);
            this.trbBeepVolume.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.trbBeepVolume.Maximum = 100;
            this.trbBeepVolume.Name = "trbBeepVolume";
            this.trbBeepVolume.Size = new System.Drawing.Size(512, 25);
            this.trbBeepVolume.TabIndex = 6;
            this.trbBeepVolume.TabStop = false;
            this.trbBeepVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trbBeepVolume.Value = 100;
            this.trbBeepVolume.Scroll += new System.EventHandler(this.trbBeepVolume_Scroll);
            this.trbBeepVolume.ValueChanged += new System.EventHandler(this.trbBeepVolume_ValueChanged);
            // 
            // lblBeepVolP
            // 
            this.lblBeepVolP.AutoEllipsis = true;
            this.lblBeepVolP.AutoSize = true;
            this.lblBeepVolP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBeepVolP.Location = new System.Drawing.Point(732, 29);
            this.lblBeepVolP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBeepVolP.Name = "lblBeepVolP";
            this.lblBeepVolP.Size = new System.Drawing.Size(99, 31);
            this.lblBeepVolP.TabIndex = 7;
            this.lblBeepVolP.Text = "100 %";
            this.lblBeepVolP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnBeepTest
            // 
            this.btnBeepTest.AutoSize = true;
            this.btnBeepTest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBeepTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBeepTest.Location = new System.Drawing.Point(732, 63);
            this.btnBeepTest.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnBeepTest.Name = "btnBeepTest";
            this.btnBeepTest.Size = new System.Drawing.Size(99, 159);
            this.btnBeepTest.TabIndex = 8;
            this.btnBeepTest.TabStop = false;
            this.btnBeepTest.Text = "Test";
            this.btnBeepTest.UseVisualStyleBackColor = true;
            this.btnBeepTest.Click += new System.EventHandler(this.btnBeepTest_Click);
            // 
            // capAudioBeep
            // 
            this.capAudioBeep.Caption = "Beep";
            this.capAudioBeep.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capAudioBeep.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capAudioBeep.Collapsed = false;
            this.capAudioBeep.Collapsee = null;
            this.capAudioBeep.Collapsible = false;
            this.capAudioBeep.Dock = System.Windows.Forms.DockStyle.Top;
            this.capAudioBeep.Location = new System.Drawing.Point(0, 0);
            this.capAudioBeep.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capAudioBeep.Name = "capAudioBeep";
            this.capAudioBeep.Size = new System.Drawing.Size(835, 35);
            this.capAudioBeep.TabIndex = 15;
            this.capAudioBeep.TabStop = false;
            // 
            // tabLogging
            // 
            this.tabLogging.AutoScroll = true;
            this.tabLogging.Controls.Add(this.panelLogLimits);
            this.tabLogging.Controls.Add(this.capLogLimits);
            this.tabLogging.Controls.Add(this.pnlLoggingGeneral);
            this.tabLogging.Controls.Add(this.capLogging);
            this.tabLogging.Location = new System.Drawing.Point(4, 28);
            this.tabLogging.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabLogging.Name = "tabLogging";
            this.tabLogging.Size = new System.Drawing.Size(856, 597);
            this.tabLogging.TabIndex = 2;
            this.tabLogging.Text = "Logging";
            this.tabLogging.UseVisualStyleBackColor = true;
            // 
            // pnlLoggingGeneral
            // 
            this.pnlLoggingGeneral.AutoSize = true;
            this.pnlLoggingGeneral.Controls.Add(this.tableLoggingGeneral);
            this.pnlLoggingGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLoggingGeneral.Location = new System.Drawing.Point(0, 35);
            this.pnlLoggingGeneral.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlLoggingGeneral.Name = "pnlLoggingGeneral";
            this.pnlLoggingGeneral.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.pnlLoggingGeneral.Size = new System.Drawing.Size(856, 96);
            this.pnlLoggingGeneral.TabIndex = 10;
            // 
            // tableLoggingGeneral
            // 
            this.tableLoggingGeneral.AutoSize = true;
            this.tableLoggingGeneral.ColumnCount = 2;
            this.tableLoggingGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLoggingGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLoggingGeneral.Controls.Add(this.lblLogNormalEvents, 0, 1);
            this.tableLoggingGeneral.Controls.Add(this.lblLogVariableExpansions, 0, 3);
            this.tableLoggingGeneral.Controls.Add(this.chkLogVariableExpansions, 1, 3);
            this.tableLoggingGeneral.Controls.Add(this.chkLogNormalEvents, 1, 1);
            this.tableLoggingGeneral.Controls.Add(this.lblLoggingLevel, 0, 0);
            this.tableLoggingGeneral.Controls.Add(this.cbxLoggingLevel, 1, 0);
            this.tableLoggingGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLoggingGeneral.Location = new System.Drawing.Point(0, 9);
            this.tableLoggingGeneral.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLoggingGeneral.Name = "tableLoggingGeneral";
            this.tableLoggingGeneral.RowCount = 4;
            this.tableLoggingGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLoggingGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLoggingGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLoggingGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLoggingGeneral.Size = new System.Drawing.Size(856, 87);
            this.tableLoggingGeneral.TabIndex = 1;
            // 
            // lblLogNormalEvents
            // 
            this.lblLogNormalEvents.AutoEllipsis = true;
            this.lblLogNormalEvents.AutoSize = true;
            this.lblLogNormalEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogNormalEvents.Location = new System.Drawing.Point(4, 29);
            this.lblLogNormalEvents.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogNormalEvents.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblLogNormalEvents.Name = "lblLogNormalEvents";
            this.lblLogNormalEvents.Size = new System.Drawing.Size(200, 29);
            this.lblLogNormalEvents.TabIndex = 5;
            this.lblLogNormalEvents.Text = "Log normal log lines";
            this.lblLogNormalEvents.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLogVariableExpansions
            // 
            this.lblLogVariableExpansions.AutoEllipsis = true;
            this.lblLogVariableExpansions.AutoSize = true;
            this.lblLogVariableExpansions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogVariableExpansions.Location = new System.Drawing.Point(4, 58);
            this.lblLogVariableExpansions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogVariableExpansions.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblLogVariableExpansions.Name = "lblLogVariableExpansions";
            this.lblLogVariableExpansions.Size = new System.Drawing.Size(200, 29);
            this.lblLogVariableExpansions.TabIndex = 4;
            this.lblLogVariableExpansions.Text = "Log variable expansions";
            this.lblLogVariableExpansions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkLogVariableExpansions
            // 
            this.chkLogVariableExpansions.AutoSize = true;
            this.chkLogVariableExpansions.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkLogVariableExpansions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLogVariableExpansions.Location = new System.Drawing.Point(212, 61);
            this.chkLogVariableExpansions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkLogVariableExpansions.Name = "chkLogVariableExpansions";
            this.chkLogVariableExpansions.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkLogVariableExpansions.Size = new System.Drawing.Size(640, 23);
            this.chkLogVariableExpansions.TabIndex = 0;
            this.chkLogVariableExpansions.TabStop = false;
            this.chkLogVariableExpansions.UseVisualStyleBackColor = true;
            // 
            // chkLogNormalEvents
            // 
            this.chkLogNormalEvents.AutoSize = true;
            this.chkLogNormalEvents.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkLogNormalEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLogNormalEvents.Location = new System.Drawing.Point(212, 32);
            this.chkLogNormalEvents.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkLogNormalEvents.Name = "chkLogNormalEvents";
            this.chkLogNormalEvents.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkLogNormalEvents.Size = new System.Drawing.Size(640, 23);
            this.chkLogNormalEvents.TabIndex = 1;
            this.chkLogNormalEvents.TabStop = false;
            this.chkLogNormalEvents.UseVisualStyleBackColor = true;
            // 
            // lblLoggingLevel
            // 
            this.lblLoggingLevel.AutoEllipsis = true;
            this.lblLoggingLevel.AutoSize = true;
            this.lblLoggingLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLoggingLevel.Location = new System.Drawing.Point(4, 0);
            this.lblLoggingLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoggingLevel.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblLoggingLevel.Name = "lblLoggingLevel";
            this.lblLoggingLevel.Size = new System.Drawing.Size(200, 29);
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
            this.cbxLoggingLevel.Location = new System.Drawing.Point(212, 3);
            this.cbxLoggingLevel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxLoggingLevel.Name = "cbxLoggingLevel";
            this.cbxLoggingLevel.Size = new System.Drawing.Size(640, 23);
            this.cbxLoggingLevel.TabIndex = 3;
            this.cbxLoggingLevel.TabStop = false;
            // 
            // capLogging
            // 
            this.capLogging.Caption = "Logging settings";
            this.capLogging.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capLogging.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capLogging.Collapsed = false;
            this.capLogging.Collapsee = null;
            this.capLogging.Collapsible = false;
            this.capLogging.Dock = System.Windows.Forms.DockStyle.Top;
            this.capLogging.Location = new System.Drawing.Point(0, 0);
            this.capLogging.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capLogging.Name = "capLogging";
            this.capLogging.Size = new System.Drawing.Size(856, 35);
            this.capLogging.TabIndex = 9;
            //
            // capLogLimits
            //
            this.capLogLimits.Caption = "Log record limits";
            this.capLogLimits.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capLogLimits.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capLogLimits.Collapsed = false;
            this.capLogLimits.Collapsee = null;
            this.capLogLimits.Collapsible = false;
            this.capLogLimits.Dock = System.Windows.Forms.DockStyle.Top;
            this.capLogLimits.Location = new System.Drawing.Point(0, 131);
            this.capLogLimits.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capLogLimits.Name = "capLogLimits";
            this.capLogLimits.Size = new System.Drawing.Size(856, 35);
            this.capLogLimits.TabIndex = 11;
            //
            // panelLogLimits
            //
            this.panelLogLimits.AutoSize = true;
            this.panelLogLimits.Controls.Add(this.tableLogLimits);
            this.panelLogLimits.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogLimits.Location = new System.Drawing.Point(0, 166);
            this.panelLogLimits.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelLogLimits.Name = "panelLogLimits";
            this.panelLogLimits.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.panelLogLimits.Size = new System.Drawing.Size(856, 195);
            this.panelLogLimits.TabIndex = 12;
            //
            // tableLogLimits
            //
            this.tableLogLimits.AutoSize = true;
            this.tableLogLimits.ColumnCount = 2;
            this.tableLogLimits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLogLimits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLogLimits.Controls.Add(this.lblLogLimitError, 0, 0);
            this.tableLogLimits.Controls.Add(this.nudLogLimitError, 1, 0);
            this.tableLogLimits.Controls.Add(this.lblLogLimitWarning, 0, 1);
            this.tableLogLimits.Controls.Add(this.nudLogLimitWarning, 1, 1);
            this.tableLogLimits.Controls.Add(this.lblLogLimitCustom, 0, 2);
            this.tableLogLimits.Controls.Add(this.nudLogLimitCustom, 1, 2);
            this.tableLogLimits.Controls.Add(this.lblLogLimitCustom2, 0, 3);
            this.tableLogLimits.Controls.Add(this.nudLogLimitCustom2, 1, 3);
            this.tableLogLimits.Controls.Add(this.lblLogLimitInfo, 0, 4);
            this.tableLogLimits.Controls.Add(this.nudLogLimitInfo, 1, 4);
            this.tableLogLimits.Controls.Add(this.lblLogLimitVerbose, 0, 5);
            this.tableLogLimits.Controls.Add(this.nudLogLimitVerbose, 1, 5);
            this.tableLogLimits.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLogLimits.Location = new System.Drawing.Point(0, 9);
            this.tableLogLimits.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLogLimits.Name = "tableLogLimits";
            this.tableLogLimits.RowCount = 6;
            this.tableLogLimits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLogLimits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLogLimits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLogLimits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLogLimits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLogLimits.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLogLimits.Size = new System.Drawing.Size(856, 186);
            this.tableLogLimits.TabIndex = 0;
            //
            // lblLogLimitError
            //
            this.lblLogLimitError.AutoEllipsis = true;
            this.lblLogLimitError.AutoSize = true;
            this.lblLogLimitError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogLimitError.Location = new System.Drawing.Point(4, 0);
            this.lblLogLimitError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogLimitError.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblLogLimitError.Name = "lblLogLimitError";
            this.lblLogLimitError.Size = new System.Drawing.Size(200, 31);
            this.lblLogLimitError.TabIndex = 0;
            this.lblLogLimitError.Text = "Error";
            this.lblLogLimitError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // nudLogLimitError
            //
            this.nudLogLimitError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLogLimitError.Location = new System.Drawing.Point(212, 3);
            this.nudLogLimitError.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudLogLimitError.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudLogLimitError.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudLogLimitError.Name = "nudLogLimitError";
            this.nudLogLimitError.Size = new System.Drawing.Size(640, 25);
            this.nudLogLimitError.TabIndex = 1;
            this.nudLogLimitError.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudLogLimitError.Value = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            //
            // lblLogLimitWarning
            //
            this.lblLogLimitWarning.AutoEllipsis = true;
            this.lblLogLimitWarning.AutoSize = true;
            this.lblLogLimitWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogLimitWarning.Location = new System.Drawing.Point(4, 31);
            this.lblLogLimitWarning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogLimitWarning.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblLogLimitWarning.Name = "lblLogLimitWarning";
            this.lblLogLimitWarning.Size = new System.Drawing.Size(200, 31);
            this.lblLogLimitWarning.TabIndex = 2;
            this.lblLogLimitWarning.Text = "Warning";
            this.lblLogLimitWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // nudLogLimitWarning
            //
            this.nudLogLimitWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLogLimitWarning.Location = new System.Drawing.Point(212, 34);
            this.nudLogLimitWarning.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudLogLimitWarning.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudLogLimitWarning.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudLogLimitWarning.Name = "nudLogLimitWarning";
            this.nudLogLimitWarning.Size = new System.Drawing.Size(640, 25);
            this.nudLogLimitWarning.TabIndex = 3;
            this.nudLogLimitWarning.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudLogLimitWarning.Value = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            //
            // lblLogLimitCustom
            //
            this.lblLogLimitCustom.AutoEllipsis = true;
            this.lblLogLimitCustom.AutoSize = true;
            this.lblLogLimitCustom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogLimitCustom.Location = new System.Drawing.Point(4, 62);
            this.lblLogLimitCustom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogLimitCustom.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblLogLimitCustom.Name = "lblLogLimitCustom";
            this.lblLogLimitCustom.Size = new System.Drawing.Size(200, 31);
            this.lblLogLimitCustom.TabIndex = 4;
            this.lblLogLimitCustom.Text = "Custom";
            this.lblLogLimitCustom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // nudLogLimitCustom
            //
            this.nudLogLimitCustom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLogLimitCustom.Location = new System.Drawing.Point(212, 65);
            this.nudLogLimitCustom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudLogLimitCustom.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudLogLimitCustom.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudLogLimitCustom.Name = "nudLogLimitCustom";
            this.nudLogLimitCustom.Size = new System.Drawing.Size(640, 25);
            this.nudLogLimitCustom.TabIndex = 5;
            this.nudLogLimitCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudLogLimitCustom.Value = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            //
            // lblLogLimitCustom2
            //
            this.lblLogLimitCustom2.AutoEllipsis = true;
            this.lblLogLimitCustom2.AutoSize = true;
            this.lblLogLimitCustom2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogLimitCustom2.Location = new System.Drawing.Point(4, 93);
            this.lblLogLimitCustom2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogLimitCustom2.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblLogLimitCustom2.Name = "lblLogLimitCustom2";
            this.lblLogLimitCustom2.Size = new System.Drawing.Size(200, 31);
            this.lblLogLimitCustom2.TabIndex = 6;
            this.lblLogLimitCustom2.Text = "Custom 2";
            this.lblLogLimitCustom2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // nudLogLimitCustom2
            //
            this.nudLogLimitCustom2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLogLimitCustom2.Location = new System.Drawing.Point(212, 96);
            this.nudLogLimitCustom2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudLogLimitCustom2.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudLogLimitCustom2.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudLogLimitCustom2.Name = "nudLogLimitCustom2";
            this.nudLogLimitCustom2.Size = new System.Drawing.Size(640, 25);
            this.nudLogLimitCustom2.TabIndex = 7;
            this.nudLogLimitCustom2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudLogLimitCustom2.Value = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            //
            // lblLogLimitInfo
            //
            this.lblLogLimitInfo.AutoEllipsis = true;
            this.lblLogLimitInfo.AutoSize = true;
            this.lblLogLimitInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogLimitInfo.Location = new System.Drawing.Point(4, 124);
            this.lblLogLimitInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogLimitInfo.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblLogLimitInfo.Name = "lblLogLimitInfo";
            this.lblLogLimitInfo.Size = new System.Drawing.Size(200, 31);
            this.lblLogLimitInfo.TabIndex = 8;
            this.lblLogLimitInfo.Text = "Info";
            this.lblLogLimitInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // nudLogLimitInfo
            //
            this.nudLogLimitInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLogLimitInfo.Location = new System.Drawing.Point(212, 127);
            this.nudLogLimitInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudLogLimitInfo.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudLogLimitInfo.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudLogLimitInfo.Name = "nudLogLimitInfo";
            this.nudLogLimitInfo.Size = new System.Drawing.Size(640, 25);
            this.nudLogLimitInfo.TabIndex = 9;
            this.nudLogLimitInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudLogLimitInfo.Value = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            //
            // lblLogLimitVerbose
            //
            this.lblLogLimitVerbose.AutoEllipsis = true;
            this.lblLogLimitVerbose.AutoSize = true;
            this.lblLogLimitVerbose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLogLimitVerbose.Location = new System.Drawing.Point(4, 155);
            this.lblLogLimitVerbose.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogLimitVerbose.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblLogLimitVerbose.Name = "lblLogLimitVerbose";
            this.lblLogLimitVerbose.Size = new System.Drawing.Size(200, 31);
            this.lblLogLimitVerbose.TabIndex = 10;
            this.lblLogLimitVerbose.Text = "Verbose";
            this.lblLogLimitVerbose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // nudLogLimitVerbose
            //
            this.nudLogLimitVerbose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLogLimitVerbose.Location = new System.Drawing.Point(212, 158);
            this.nudLogLimitVerbose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudLogLimitVerbose.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudLogLimitVerbose.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudLogLimitVerbose.Name = "nudLogLimitVerbose";
            this.nudLogLimitVerbose.Size = new System.Drawing.Size(640, 25);
            this.nudLogLimitVerbose.TabIndex = 11;
            this.nudLogLimitVerbose.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudLogLimitVerbose.Value = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            // 
            // tabShortcuts
            // 
            this.tabShortcuts.AutoScroll = true;
            this.tabShortcuts.Controls.Add(this.pnlShortcutTemplates);
            this.tabShortcuts.Controls.Add(this.capExpressions);
            this.tabShortcuts.Location = new System.Drawing.Point(4, 28);
            this.tabShortcuts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabShortcuts.Name = "tabShortcuts";
            this.tabShortcuts.Size = new System.Drawing.Size(856, 597);
            this.tabShortcuts.TabIndex = 3;
            this.tabShortcuts.Text = "Shortcuts";
            this.tabShortcuts.UseVisualStyleBackColor = true;
            // 
            // pnlShortcutTemplates
            // 
            this.pnlShortcutTemplates.AutoSize = true;
            this.pnlShortcutTemplates.Controls.Add(this.tableLayoutPanelShortCutExpression);
            this.pnlShortcutTemplates.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlShortcutTemplates.Location = new System.Drawing.Point(0, 35);
            this.pnlShortcutTemplates.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlShortcutTemplates.Name = "pnlShortcutTemplates";
            this.pnlShortcutTemplates.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.pnlShortcutTemplates.Size = new System.Drawing.Size(856, 96);
            this.pnlShortcutTemplates.TabIndex = 9;
            // 
            // tableLayoutPanelShortCutExpression
            // 
            this.tableLayoutPanelShortCutExpression.AutoSize = true;
            this.tableLayoutPanelShortCutExpression.ColumnCount = 2;
            this.tableLayoutPanelShortCutExpression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelShortCutExpression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelShortCutExpression.Controls.Add(this.lblShortcutWrapTextWhenSelected, 0, 2);
            this.tableLayoutPanelShortCutExpression.Controls.Add(this.lblShortcutUseAbbrevInTemplates, 0, 1);
            this.tableLayoutPanelShortCutExpression.Controls.Add(this.lblShortcutEnableTemplates, 0, 0);
            this.tableLayoutPanelShortCutExpression.Controls.Add(this.chkShortcutEnableTemplates, 1, 0);
            this.tableLayoutPanelShortCutExpression.Controls.Add(this.chkShortcutUseAbbrevInTemplates, 1, 1);
            this.tableLayoutPanelShortCutExpression.Controls.Add(this.chkShortcutWrapTextWhenSelected, 1, 2);
            this.tableLayoutPanelShortCutExpression.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelShortCutExpression.Location = new System.Drawing.Point(0, 9);
            this.tableLayoutPanelShortCutExpression.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanelShortCutExpression.Name = "tableLayoutPanelShortCutExpression";
            this.tableLayoutPanelShortCutExpression.RowCount = 3;
            this.tableLayoutPanelShortCutExpression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelShortCutExpression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelShortCutExpression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelShortCutExpression.Size = new System.Drawing.Size(856, 87);
            this.tableLayoutPanelShortCutExpression.TabIndex = 1;
            // 
            // lblShortcutWrapTextWhenSelected
            // 
            this.lblShortcutWrapTextWhenSelected.AutoEllipsis = true;
            this.lblShortcutWrapTextWhenSelected.AutoSize = true;
            this.lblShortcutWrapTextWhenSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShortcutWrapTextWhenSelected.Location = new System.Drawing.Point(4, 58);
            this.lblShortcutWrapTextWhenSelected.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblShortcutWrapTextWhenSelected.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblShortcutWrapTextWhenSelected.Name = "lblShortcutWrapTextWhenSelected";
            this.lblShortcutWrapTextWhenSelected.Size = new System.Drawing.Size(423, 29);
            this.lblShortcutWrapTextWhenSelected.TabIndex = 5;
            this.lblShortcutWrapTextWhenSelected.Text = "Wrap selected text in template expressions";
            this.lblShortcutWrapTextWhenSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblShortcutUseAbbrevInTemplates
            // 
            this.lblShortcutUseAbbrevInTemplates.AutoEllipsis = true;
            this.lblShortcutUseAbbrevInTemplates.AutoSize = true;
            this.lblShortcutUseAbbrevInTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShortcutUseAbbrevInTemplates.Location = new System.Drawing.Point(4, 29);
            this.lblShortcutUseAbbrevInTemplates.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblShortcutUseAbbrevInTemplates.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblShortcutUseAbbrevInTemplates.Name = "lblShortcutUseAbbrevInTemplates";
            this.lblShortcutUseAbbrevInTemplates.Size = new System.Drawing.Size(423, 29);
            this.lblShortcutUseAbbrevInTemplates.TabIndex = 4;
            this.lblShortcutUseAbbrevInTemplates.Text = "Use abbreviation expressions in template expressions";
            this.lblShortcutUseAbbrevInTemplates.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblShortcutEnableTemplates
            // 
            this.lblShortcutEnableTemplates.AutoEllipsis = true;
            this.lblShortcutEnableTemplates.AutoSize = true;
            this.lblShortcutEnableTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblShortcutEnableTemplates.Location = new System.Drawing.Point(4, 0);
            this.lblShortcutEnableTemplates.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblShortcutEnableTemplates.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblShortcutEnableTemplates.Name = "lblShortcutEnableTemplates";
            this.lblShortcutEnableTemplates.Size = new System.Drawing.Size(423, 29);
            this.lblShortcutEnableTemplates.TabIndex = 3;
            this.lblShortcutEnableTemplates.Text = "Enable shortcuts to input template expressions";
            this.lblShortcutEnableTemplates.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkShortcutEnableTemplates
            // 
            this.chkShortcutEnableTemplates.AutoSize = true;
            this.chkShortcutEnableTemplates.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShortcutEnableTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShortcutEnableTemplates.Location = new System.Drawing.Point(435, 3);
            this.chkShortcutEnableTemplates.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkShortcutEnableTemplates.Name = "chkShortcutEnableTemplates";
            this.chkShortcutEnableTemplates.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkShortcutEnableTemplates.Size = new System.Drawing.Size(417, 23);
            this.chkShortcutEnableTemplates.TabIndex = 0;
            this.chkShortcutEnableTemplates.TabStop = false;
            this.chkShortcutEnableTemplates.UseVisualStyleBackColor = true;
            // 
            // chkShortcutUseAbbrevInTemplates
            // 
            this.chkShortcutUseAbbrevInTemplates.AutoSize = true;
            this.chkShortcutUseAbbrevInTemplates.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShortcutUseAbbrevInTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShortcutUseAbbrevInTemplates.Location = new System.Drawing.Point(435, 32);
            this.chkShortcutUseAbbrevInTemplates.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkShortcutUseAbbrevInTemplates.Name = "chkShortcutUseAbbrevInTemplates";
            this.chkShortcutUseAbbrevInTemplates.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkShortcutUseAbbrevInTemplates.Size = new System.Drawing.Size(417, 23);
            this.chkShortcutUseAbbrevInTemplates.TabIndex = 1;
            this.chkShortcutUseAbbrevInTemplates.TabStop = false;
            this.chkShortcutUseAbbrevInTemplates.UseVisualStyleBackColor = true;
            // 
            // chkShortcutWrapTextWhenSelected
            // 
            this.chkShortcutWrapTextWhenSelected.AutoSize = true;
            this.chkShortcutWrapTextWhenSelected.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShortcutWrapTextWhenSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShortcutWrapTextWhenSelected.Location = new System.Drawing.Point(435, 61);
            this.chkShortcutWrapTextWhenSelected.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkShortcutWrapTextWhenSelected.Name = "chkShortcutWrapTextWhenSelected";
            this.chkShortcutWrapTextWhenSelected.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkShortcutWrapTextWhenSelected.Size = new System.Drawing.Size(417, 23);
            this.chkShortcutWrapTextWhenSelected.TabIndex = 2;
            this.chkShortcutWrapTextWhenSelected.TabStop = false;
            this.chkShortcutWrapTextWhenSelected.UseVisualStyleBackColor = true;
            // 
            // capExpressions
            // 
            this.capExpressions.Caption = "Expressions";
            this.capExpressions.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capExpressions.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capExpressions.Collapsed = false;
            this.capExpressions.Collapsee = null;
            this.capExpressions.Collapsible = false;
            this.capExpressions.Dock = System.Windows.Forms.DockStyle.Top;
            this.capExpressions.Location = new System.Drawing.Point(0, 0);
            this.capExpressions.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capExpressions.Name = "capExpressions";
            this.capExpressions.Size = new System.Drawing.Size(856, 35);
            this.capExpressions.TabIndex = 8;
            // 
            // tabCaching
            // 
            this.tabCaching.AutoScroll = true;
            this.tabCaching.Controls.Add(this.pnlCacheFile);
            this.tabCaching.Controls.Add(this.capDownloads);
            this.tabCaching.Controls.Add(this.pnlCacheRepo);
            this.tabCaching.Controls.Add(this.capRepoBackups);
            this.tabCaching.Controls.Add(this.pnlCacheJson);
            this.tabCaching.Controls.Add(this.capJason);
            this.tabCaching.Controls.Add(this.pnlCacheSound);
            this.tabCaching.Controls.Add(this.capSoundFiles);
            this.tabCaching.Controls.Add(this.pnlCacheImage);
            this.tabCaching.Controls.Add(this.capImageFiles);
            this.tabCaching.Location = new System.Drawing.Point(4, 28);
            this.tabCaching.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabCaching.Name = "tabCaching";
            this.tabCaching.Size = new System.Drawing.Size(856, 597);
            this.tabCaching.TabIndex = 4;
            this.tabCaching.Text = "Caching";
            this.tabCaching.UseVisualStyleBackColor = true;
            // 
            // pnlCacheFile
            // 
            this.pnlCacheFile.AutoSize = true;
            this.pnlCacheFile.Controls.Add(this.tableLayoutPanelDownloads);
            this.pnlCacheFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCacheFile.Location = new System.Drawing.Point(0, 783);
            this.pnlCacheFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlCacheFile.Name = "pnlCacheFile";
            this.pnlCacheFile.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.pnlCacheFile.Size = new System.Drawing.Size(835, 143);
            this.pnlCacheFile.TabIndex = 27;
            // 
            // tableLayoutPanelDownloads
            // 
            this.tableLayoutPanelDownloads.AutoSize = true;
            this.tableLayoutPanelDownloads.ColumnCount = 3;
            this.tableLayoutPanelDownloads.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDownloads.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDownloads.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDownloads.Controls.Add(this.btnCacheFileBrowse, 2, 3);
            this.tableLayoutPanelDownloads.Controls.Add(this.btnCacheFileClear, 1, 3);
            this.tableLayoutPanelDownloads.Controls.Add(this.txtCacheFileSize, 1, 2);
            this.tableLayoutPanelDownloads.Controls.Add(this.lblCacheFileSize, 0, 2);
            this.tableLayoutPanelDownloads.Controls.Add(this.lblCacheFileCount, 0, 1);
            this.tableLayoutPanelDownloads.Controls.Add(this.lblCacheFileExpiry, 0, 0);
            this.tableLayoutPanelDownloads.Controls.Add(this.nudCacheFileExpiry, 1, 0);
            this.tableLayoutPanelDownloads.Controls.Add(this.txtCacheFileCount, 1, 1);
            this.tableLayoutPanelDownloads.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelDownloads.Location = new System.Drawing.Point(0, 9);
            this.tableLayoutPanelDownloads.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanelDownloads.Name = "tableLayoutPanelDownloads";
            this.tableLayoutPanelDownloads.RowCount = 4;
            this.tableLayoutPanelDownloads.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDownloads.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDownloads.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDownloads.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDownloads.Size = new System.Drawing.Size(835, 134);
            this.tableLayoutPanelDownloads.TabIndex = 1;
            // 
            // btnCacheFileBrowse
            // 
            this.btnCacheFileBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheFileBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheFileBrowse.Image")));
            this.btnCacheFileBrowse.Location = new System.Drawing.Point(533, 96);
            this.btnCacheFileBrowse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCacheFileBrowse.Name = "btnCacheFileBrowse";
            this.btnCacheFileBrowse.Size = new System.Drawing.Size(298, 35);
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
            this.btnCacheFileClear.Location = new System.Drawing.Point(227, 96);
            this.btnCacheFileClear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCacheFileClear.Name = "btnCacheFileClear";
            this.btnCacheFileClear.Size = new System.Drawing.Size(298, 35);
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
            this.tableLayoutPanelDownloads.SetColumnSpan(this.txtCacheFileSize, 2);
            this.txtCacheFileSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheFileSize.Location = new System.Drawing.Point(227, 65);
            this.txtCacheFileSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCacheFileSize.Name = "txtCacheFileSize";
            this.txtCacheFileSize.ReadOnly = true;
            this.txtCacheFileSize.Size = new System.Drawing.Size(604, 25);
            this.txtCacheFileSize.TabIndex = 13;
            this.txtCacheFileSize.Text = "0";
            this.txtCacheFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCacheFileSize
            // 
            this.lblCacheFileSize.AutoEllipsis = true;
            this.lblCacheFileSize.AutoSize = true;
            this.lblCacheFileSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheFileSize.Location = new System.Drawing.Point(4, 62);
            this.lblCacheFileSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheFileSize.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheFileSize.Name = "lblCacheFileSize";
            this.lblCacheFileSize.Size = new System.Drawing.Size(215, 31);
            this.lblCacheFileSize.TabIndex = 14;
            this.lblCacheFileSize.Text = "Current disk size in bytes";
            this.lblCacheFileSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheFileCount
            // 
            this.lblCacheFileCount.AutoEllipsis = true;
            this.lblCacheFileCount.AutoSize = true;
            this.lblCacheFileCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheFileCount.Location = new System.Drawing.Point(4, 31);
            this.lblCacheFileCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheFileCount.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheFileCount.Name = "lblCacheFileCount";
            this.lblCacheFileCount.Size = new System.Drawing.Size(215, 31);
            this.lblCacheFileCount.TabIndex = 15;
            this.lblCacheFileCount.Text = "Current item count";
            this.lblCacheFileCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheFileExpiry
            // 
            this.lblCacheFileExpiry.AutoEllipsis = true;
            this.lblCacheFileExpiry.AutoSize = true;
            this.lblCacheFileExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheFileExpiry.Location = new System.Drawing.Point(4, 0);
            this.lblCacheFileExpiry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheFileExpiry.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheFileExpiry.Name = "lblCacheFileExpiry";
            this.lblCacheFileExpiry.Size = new System.Drawing.Size(215, 31);
            this.lblCacheFileExpiry.TabIndex = 16;
            this.lblCacheFileExpiry.Text = "Expiration in minutes";
            this.lblCacheFileExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudCacheFileExpiry
            // 
            this.tableLayoutPanelDownloads.SetColumnSpan(this.nudCacheFileExpiry, 2);
            this.nudCacheFileExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCacheFileExpiry.Location = new System.Drawing.Point(227, 3);
            this.nudCacheFileExpiry.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudCacheFileExpiry.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudCacheFileExpiry.Name = "nudCacheFileExpiry";
            this.nudCacheFileExpiry.Size = new System.Drawing.Size(604, 25);
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
            this.tableLayoutPanelDownloads.SetColumnSpan(this.txtCacheFileCount, 2);
            this.txtCacheFileCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheFileCount.Location = new System.Drawing.Point(227, 34);
            this.txtCacheFileCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCacheFileCount.Name = "txtCacheFileCount";
            this.txtCacheFileCount.ReadOnly = true;
            this.txtCacheFileCount.Size = new System.Drawing.Size(604, 25);
            this.txtCacheFileCount.TabIndex = 11;
            this.txtCacheFileCount.Text = "0";
            this.txtCacheFileCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // capDownloads
            // 
            this.capDownloads.Caption = "File downloads";
            this.capDownloads.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capDownloads.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capDownloads.Collapsed = false;
            this.capDownloads.Collapsee = null;
            this.capDownloads.Collapsible = false;
            this.capDownloads.Dock = System.Windows.Forms.DockStyle.Top;
            this.capDownloads.Location = new System.Drawing.Point(0, 748);
            this.capDownloads.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capDownloads.Name = "capDownloads";
            this.capDownloads.Size = new System.Drawing.Size(835, 35);
            this.capDownloads.TabIndex = 26;
            // 
            // pnlCacheRepo
            // 
            this.pnlCacheRepo.AutoSize = true;
            this.pnlCacheRepo.Controls.Add(this.tableLayoutPanelRepo);
            this.pnlCacheRepo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCacheRepo.Location = new System.Drawing.Point(0, 596);
            this.pnlCacheRepo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlCacheRepo.Name = "pnlCacheRepo";
            this.pnlCacheRepo.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlCacheRepo.Size = new System.Drawing.Size(835, 152);
            this.pnlCacheRepo.TabIndex = 25;
            // 
            // tableLayoutPanelRepo
            // 
            this.tableLayoutPanelRepo.AutoSize = true;
            this.tableLayoutPanelRepo.ColumnCount = 3;
            this.tableLayoutPanelRepo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelRepo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRepo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelRepo.Controls.Add(this.btnCacheRepoBrowse, 2, 3);
            this.tableLayoutPanelRepo.Controls.Add(this.btnCacheRepoClear, 1, 3);
            this.tableLayoutPanelRepo.Controls.Add(this.txtCacheRepoSize, 1, 2);
            this.tableLayoutPanelRepo.Controls.Add(this.lblCacheRepoSize, 0, 2);
            this.tableLayoutPanelRepo.Controls.Add(this.lblCacheRepoCount, 0, 1);
            this.tableLayoutPanelRepo.Controls.Add(this.lblCacheRepoExpiry, 0, 0);
            this.tableLayoutPanelRepo.Controls.Add(this.nudCacheRepoExpiry, 1, 0);
            this.tableLayoutPanelRepo.Controls.Add(this.txtCacheRepoCount, 1, 1);
            this.tableLayoutPanelRepo.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelRepo.Location = new System.Drawing.Point(0, 9);
            this.tableLayoutPanelRepo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanelRepo.Name = "tableLayoutPanelRepo";
            this.tableLayoutPanelRepo.RowCount = 4;
            this.tableLayoutPanelRepo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRepo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRepo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRepo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRepo.Size = new System.Drawing.Size(835, 134);
            this.tableLayoutPanelRepo.TabIndex = 1;
            // 
            // btnCacheRepoBrowse
            // 
            this.btnCacheRepoBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheRepoBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheRepoBrowse.Image")));
            this.btnCacheRepoBrowse.Location = new System.Drawing.Point(533, 96);
            this.btnCacheRepoBrowse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCacheRepoBrowse.Name = "btnCacheRepoBrowse";
            this.btnCacheRepoBrowse.Size = new System.Drawing.Size(298, 35);
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
            this.btnCacheRepoClear.Location = new System.Drawing.Point(227, 96);
            this.btnCacheRepoClear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCacheRepoClear.Name = "btnCacheRepoClear";
            this.btnCacheRepoClear.Size = new System.Drawing.Size(298, 35);
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
            this.tableLayoutPanelRepo.SetColumnSpan(this.txtCacheRepoSize, 2);
            this.txtCacheRepoSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheRepoSize.Location = new System.Drawing.Point(227, 65);
            this.txtCacheRepoSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCacheRepoSize.Name = "txtCacheRepoSize";
            this.txtCacheRepoSize.ReadOnly = true;
            this.txtCacheRepoSize.Size = new System.Drawing.Size(604, 25);
            this.txtCacheRepoSize.TabIndex = 13;
            this.txtCacheRepoSize.Text = "0";
            this.txtCacheRepoSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCacheRepoSize
            // 
            this.lblCacheRepoSize.AutoEllipsis = true;
            this.lblCacheRepoSize.AutoSize = true;
            this.lblCacheRepoSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheRepoSize.Location = new System.Drawing.Point(4, 62);
            this.lblCacheRepoSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheRepoSize.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheRepoSize.Name = "lblCacheRepoSize";
            this.lblCacheRepoSize.Size = new System.Drawing.Size(215, 31);
            this.lblCacheRepoSize.TabIndex = 14;
            this.lblCacheRepoSize.Text = "Current disk size in bytes";
            this.lblCacheRepoSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheRepoCount
            // 
            this.lblCacheRepoCount.AutoEllipsis = true;
            this.lblCacheRepoCount.AutoSize = true;
            this.lblCacheRepoCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheRepoCount.Location = new System.Drawing.Point(4, 31);
            this.lblCacheRepoCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheRepoCount.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheRepoCount.Name = "lblCacheRepoCount";
            this.lblCacheRepoCount.Size = new System.Drawing.Size(215, 31);
            this.lblCacheRepoCount.TabIndex = 15;
            this.lblCacheRepoCount.Text = "Current item count";
            this.lblCacheRepoCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheRepoExpiry
            // 
            this.lblCacheRepoExpiry.AutoEllipsis = true;
            this.lblCacheRepoExpiry.AutoSize = true;
            this.lblCacheRepoExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheRepoExpiry.Location = new System.Drawing.Point(4, 0);
            this.lblCacheRepoExpiry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheRepoExpiry.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheRepoExpiry.Name = "lblCacheRepoExpiry";
            this.lblCacheRepoExpiry.Size = new System.Drawing.Size(215, 31);
            this.lblCacheRepoExpiry.TabIndex = 16;
            this.lblCacheRepoExpiry.Text = "Expiration in minutes";
            this.lblCacheRepoExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudCacheRepoExpiry
            // 
            this.tableLayoutPanelRepo.SetColumnSpan(this.nudCacheRepoExpiry, 2);
            this.nudCacheRepoExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCacheRepoExpiry.Location = new System.Drawing.Point(227, 3);
            this.nudCacheRepoExpiry.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudCacheRepoExpiry.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudCacheRepoExpiry.Name = "nudCacheRepoExpiry";
            this.nudCacheRepoExpiry.Size = new System.Drawing.Size(604, 25);
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
            this.tableLayoutPanelRepo.SetColumnSpan(this.txtCacheRepoCount, 2);
            this.txtCacheRepoCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheRepoCount.Location = new System.Drawing.Point(227, 34);
            this.txtCacheRepoCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCacheRepoCount.Name = "txtCacheRepoCount";
            this.txtCacheRepoCount.ReadOnly = true;
            this.txtCacheRepoCount.Size = new System.Drawing.Size(604, 25);
            this.txtCacheRepoCount.TabIndex = 11;
            this.txtCacheRepoCount.Text = "0";
            this.txtCacheRepoCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // capRepoBackups
            // 
            this.capRepoBackups.Caption = "Repository backups";
            this.capRepoBackups.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capRepoBackups.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capRepoBackups.Collapsed = false;
            this.capRepoBackups.Collapsee = null;
            this.capRepoBackups.Collapsible = false;
            this.capRepoBackups.Dock = System.Windows.Forms.DockStyle.Top;
            this.capRepoBackups.Location = new System.Drawing.Point(0, 561);
            this.capRepoBackups.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capRepoBackups.Name = "capRepoBackups";
            this.capRepoBackups.Size = new System.Drawing.Size(835, 35);
            this.capRepoBackups.TabIndex = 24;
            // 
            // pnlCacheJson
            // 
            this.pnlCacheJson.AutoSize = true;
            this.pnlCacheJson.Controls.Add(this.tableLayoutPanelJason);
            this.pnlCacheJson.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCacheJson.Location = new System.Drawing.Point(0, 409);
            this.pnlCacheJson.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlCacheJson.Name = "pnlCacheJson";
            this.pnlCacheJson.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlCacheJson.Size = new System.Drawing.Size(835, 152);
            this.pnlCacheJson.TabIndex = 23;
            // 
            // tableLayoutPanelJason
            // 
            this.tableLayoutPanelJason.AutoSize = true;
            this.tableLayoutPanelJason.ColumnCount = 3;
            this.tableLayoutPanelJason.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelJason.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelJason.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelJason.Controls.Add(this.nudCacheJsonExpiry, 1, 0);
            this.tableLayoutPanelJason.Controls.Add(this.btnCacheJsonBrowse, 2, 3);
            this.tableLayoutPanelJason.Controls.Add(this.btnCacheJsonClear, 1, 3);
            this.tableLayoutPanelJason.Controls.Add(this.txtCacheJsonSize, 1, 2);
            this.tableLayoutPanelJason.Controls.Add(this.lblCacheJsonSize, 0, 2);
            this.tableLayoutPanelJason.Controls.Add(this.lblCacheJsonCount, 0, 1);
            this.tableLayoutPanelJason.Controls.Add(this.lblCacheJsonExpiry, 0, 0);
            this.tableLayoutPanelJason.Controls.Add(this.txtCacheJsonCount, 1, 1);
            this.tableLayoutPanelJason.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelJason.Location = new System.Drawing.Point(0, 9);
            this.tableLayoutPanelJason.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanelJason.Name = "tableLayoutPanelJason";
            this.tableLayoutPanelJason.RowCount = 4;
            this.tableLayoutPanelJason.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelJason.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelJason.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelJason.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelJason.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanelJason.Size = new System.Drawing.Size(835, 134);
            this.tableLayoutPanelJason.TabIndex = 1;
            // 
            // nudCacheJsonExpiry
            // 
            this.tableLayoutPanelJason.SetColumnSpan(this.nudCacheJsonExpiry, 2);
            this.nudCacheJsonExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCacheJsonExpiry.Location = new System.Drawing.Point(227, 3);
            this.nudCacheJsonExpiry.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudCacheJsonExpiry.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudCacheJsonExpiry.Name = "nudCacheJsonExpiry";
            this.nudCacheJsonExpiry.Size = new System.Drawing.Size(604, 25);
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
            this.btnCacheJsonBrowse.Location = new System.Drawing.Point(533, 96);
            this.btnCacheJsonBrowse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCacheJsonBrowse.Name = "btnCacheJsonBrowse";
            this.btnCacheJsonBrowse.Size = new System.Drawing.Size(298, 35);
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
            this.btnCacheJsonClear.Location = new System.Drawing.Point(227, 96);
            this.btnCacheJsonClear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCacheJsonClear.Name = "btnCacheJsonClear";
            this.btnCacheJsonClear.Size = new System.Drawing.Size(298, 35);
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
            this.tableLayoutPanelJason.SetColumnSpan(this.txtCacheJsonSize, 2);
            this.txtCacheJsonSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheJsonSize.Location = new System.Drawing.Point(227, 65);
            this.txtCacheJsonSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCacheJsonSize.Name = "txtCacheJsonSize";
            this.txtCacheJsonSize.ReadOnly = true;
            this.txtCacheJsonSize.Size = new System.Drawing.Size(604, 25);
            this.txtCacheJsonSize.TabIndex = 13;
            this.txtCacheJsonSize.Text = "0";
            this.txtCacheJsonSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCacheJsonSize
            // 
            this.lblCacheJsonSize.AutoEllipsis = true;
            this.lblCacheJsonSize.AutoSize = true;
            this.lblCacheJsonSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheJsonSize.Location = new System.Drawing.Point(4, 62);
            this.lblCacheJsonSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheJsonSize.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheJsonSize.Name = "lblCacheJsonSize";
            this.lblCacheJsonSize.Size = new System.Drawing.Size(215, 31);
            this.lblCacheJsonSize.TabIndex = 14;
            this.lblCacheJsonSize.Text = "Current disk size in bytes";
            this.lblCacheJsonSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheJsonCount
            // 
            this.lblCacheJsonCount.AutoEllipsis = true;
            this.lblCacheJsonCount.AutoSize = true;
            this.lblCacheJsonCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheJsonCount.Location = new System.Drawing.Point(4, 31);
            this.lblCacheJsonCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheJsonCount.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheJsonCount.Name = "lblCacheJsonCount";
            this.lblCacheJsonCount.Size = new System.Drawing.Size(215, 31);
            this.lblCacheJsonCount.TabIndex = 15;
            this.lblCacheJsonCount.Text = "Current item count";
            this.lblCacheJsonCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheJsonExpiry
            // 
            this.lblCacheJsonExpiry.AutoEllipsis = true;
            this.lblCacheJsonExpiry.AutoSize = true;
            this.lblCacheJsonExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheJsonExpiry.Location = new System.Drawing.Point(4, 0);
            this.lblCacheJsonExpiry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheJsonExpiry.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheJsonExpiry.Name = "lblCacheJsonExpiry";
            this.lblCacheJsonExpiry.Size = new System.Drawing.Size(215, 31);
            this.lblCacheJsonExpiry.TabIndex = 16;
            this.lblCacheJsonExpiry.Text = "Expiration in minutes";
            this.lblCacheJsonExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCacheJsonCount
            // 
            this.tableLayoutPanelJason.SetColumnSpan(this.txtCacheJsonCount, 2);
            this.txtCacheJsonCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheJsonCount.Location = new System.Drawing.Point(227, 34);
            this.txtCacheJsonCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCacheJsonCount.Name = "txtCacheJsonCount";
            this.txtCacheJsonCount.ReadOnly = true;
            this.txtCacheJsonCount.Size = new System.Drawing.Size(604, 25);
            this.txtCacheJsonCount.TabIndex = 11;
            this.txtCacheJsonCount.Text = "0";
            this.txtCacheJsonCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // capJason
            // 
            this.capJason.Caption = "JSON responses";
            this.capJason.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capJason.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capJason.Collapsed = false;
            this.capJason.Collapsee = null;
            this.capJason.Collapsible = false;
            this.capJason.Dock = System.Windows.Forms.DockStyle.Top;
            this.capJason.Location = new System.Drawing.Point(0, 374);
            this.capJason.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capJason.Name = "capJason";
            this.capJason.Size = new System.Drawing.Size(835, 35);
            this.capJason.TabIndex = 22;
            // 
            // pnlCacheSound
            // 
            this.pnlCacheSound.AutoSize = true;
            this.pnlCacheSound.Controls.Add(this.tableLayoutPanelSound);
            this.pnlCacheSound.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCacheSound.Location = new System.Drawing.Point(0, 222);
            this.pnlCacheSound.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlCacheSound.Name = "pnlCacheSound";
            this.pnlCacheSound.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlCacheSound.Size = new System.Drawing.Size(835, 152);
            this.pnlCacheSound.TabIndex = 21;
            // 
            // tableLayoutPanelSound
            // 
            this.tableLayoutPanelSound.AutoSize = true;
            this.tableLayoutPanelSound.ColumnCount = 3;
            this.tableLayoutPanelSound.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSound.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSound.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSound.Controls.Add(this.btnCacheSoundBrowse, 2, 3);
            this.tableLayoutPanelSound.Controls.Add(this.btnCacheSoundClear, 1, 3);
            this.tableLayoutPanelSound.Controls.Add(this.txtCacheSoundSize, 1, 2);
            this.tableLayoutPanelSound.Controls.Add(this.lblCacheSoundSize, 0, 2);
            this.tableLayoutPanelSound.Controls.Add(this.lblCacheSoundCount, 0, 1);
            this.tableLayoutPanelSound.Controls.Add(this.lblCacheSoundExpiry, 0, 0);
            this.tableLayoutPanelSound.Controls.Add(this.nudCacheSoundExpiry, 1, 0);
            this.tableLayoutPanelSound.Controls.Add(this.txtCacheSoundCount, 1, 1);
            this.tableLayoutPanelSound.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelSound.Location = new System.Drawing.Point(0, 9);
            this.tableLayoutPanelSound.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanelSound.Name = "tableLayoutPanelSound";
            this.tableLayoutPanelSound.RowCount = 4;
            this.tableLayoutPanelSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSound.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSound.Size = new System.Drawing.Size(835, 134);
            this.tableLayoutPanelSound.TabIndex = 1;
            // 
            // btnCacheSoundBrowse
            // 
            this.btnCacheSoundBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheSoundBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheSoundBrowse.Image")));
            this.btnCacheSoundBrowse.Location = new System.Drawing.Point(533, 96);
            this.btnCacheSoundBrowse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCacheSoundBrowse.Name = "btnCacheSoundBrowse";
            this.btnCacheSoundBrowse.Size = new System.Drawing.Size(298, 35);
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
            this.btnCacheSoundClear.Location = new System.Drawing.Point(227, 96);
            this.btnCacheSoundClear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCacheSoundClear.Name = "btnCacheSoundClear";
            this.btnCacheSoundClear.Size = new System.Drawing.Size(298, 35);
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
            this.tableLayoutPanelSound.SetColumnSpan(this.txtCacheSoundSize, 2);
            this.txtCacheSoundSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheSoundSize.Location = new System.Drawing.Point(227, 65);
            this.txtCacheSoundSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCacheSoundSize.Name = "txtCacheSoundSize";
            this.txtCacheSoundSize.ReadOnly = true;
            this.txtCacheSoundSize.Size = new System.Drawing.Size(604, 25);
            this.txtCacheSoundSize.TabIndex = 13;
            this.txtCacheSoundSize.Text = "0";
            this.txtCacheSoundSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCacheSoundSize
            // 
            this.lblCacheSoundSize.AutoEllipsis = true;
            this.lblCacheSoundSize.AutoSize = true;
            this.lblCacheSoundSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheSoundSize.Location = new System.Drawing.Point(4, 62);
            this.lblCacheSoundSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheSoundSize.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheSoundSize.Name = "lblCacheSoundSize";
            this.lblCacheSoundSize.Size = new System.Drawing.Size(215, 31);
            this.lblCacheSoundSize.TabIndex = 14;
            this.lblCacheSoundSize.Text = "Current disk size in bytes";
            this.lblCacheSoundSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheSoundCount
            // 
            this.lblCacheSoundCount.AutoEllipsis = true;
            this.lblCacheSoundCount.AutoSize = true;
            this.lblCacheSoundCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheSoundCount.Location = new System.Drawing.Point(4, 31);
            this.lblCacheSoundCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheSoundCount.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheSoundCount.Name = "lblCacheSoundCount";
            this.lblCacheSoundCount.Size = new System.Drawing.Size(215, 31);
            this.lblCacheSoundCount.TabIndex = 15;
            this.lblCacheSoundCount.Text = "Current item count";
            this.lblCacheSoundCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheSoundExpiry
            // 
            this.lblCacheSoundExpiry.AutoEllipsis = true;
            this.lblCacheSoundExpiry.AutoSize = true;
            this.lblCacheSoundExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheSoundExpiry.Location = new System.Drawing.Point(4, 0);
            this.lblCacheSoundExpiry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheSoundExpiry.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheSoundExpiry.Name = "lblCacheSoundExpiry";
            this.lblCacheSoundExpiry.Size = new System.Drawing.Size(215, 31);
            this.lblCacheSoundExpiry.TabIndex = 16;
            this.lblCacheSoundExpiry.Text = "Expiration in minutes";
            this.lblCacheSoundExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudCacheSoundExpiry
            // 
            this.tableLayoutPanelSound.SetColumnSpan(this.nudCacheSoundExpiry, 2);
            this.nudCacheSoundExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCacheSoundExpiry.Location = new System.Drawing.Point(227, 3);
            this.nudCacheSoundExpiry.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudCacheSoundExpiry.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudCacheSoundExpiry.Name = "nudCacheSoundExpiry";
            this.nudCacheSoundExpiry.Size = new System.Drawing.Size(604, 25);
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
            this.tableLayoutPanelSound.SetColumnSpan(this.txtCacheSoundCount, 2);
            this.txtCacheSoundCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheSoundCount.Location = new System.Drawing.Point(227, 34);
            this.txtCacheSoundCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCacheSoundCount.Name = "txtCacheSoundCount";
            this.txtCacheSoundCount.ReadOnly = true;
            this.txtCacheSoundCount.Size = new System.Drawing.Size(604, 25);
            this.txtCacheSoundCount.TabIndex = 11;
            this.txtCacheSoundCount.Text = "0";
            this.txtCacheSoundCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // capSoundFiles
            // 
            this.capSoundFiles.Caption = "Sound files";
            this.capSoundFiles.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capSoundFiles.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capSoundFiles.Collapsed = false;
            this.capSoundFiles.Collapsee = null;
            this.capSoundFiles.Collapsible = false;
            this.capSoundFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.capSoundFiles.Location = new System.Drawing.Point(0, 187);
            this.capSoundFiles.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capSoundFiles.Name = "capSoundFiles";
            this.capSoundFiles.Size = new System.Drawing.Size(835, 35);
            this.capSoundFiles.TabIndex = 20;
            // 
            // pnlCacheImage
            // 
            this.pnlCacheImage.AutoSize = true;
            this.pnlCacheImage.Controls.Add(this.tableLayoutPanelImages);
            this.pnlCacheImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCacheImage.Location = new System.Drawing.Point(0, 35);
            this.pnlCacheImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlCacheImage.Name = "pnlCacheImage";
            this.pnlCacheImage.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlCacheImage.Size = new System.Drawing.Size(835, 152);
            this.pnlCacheImage.TabIndex = 19;
            // 
            // tableLayoutPanelImages
            // 
            this.tableLayoutPanelImages.AutoSize = true;
            this.tableLayoutPanelImages.ColumnCount = 3;
            this.tableLayoutPanelImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelImages.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelImages.Controls.Add(this.btnCacheImageBrowse, 2, 3);
            this.tableLayoutPanelImages.Controls.Add(this.txtCacheImageSize, 1, 2);
            this.tableLayoutPanelImages.Controls.Add(this.lblCacheImageSize, 0, 2);
            this.tableLayoutPanelImages.Controls.Add(this.lblCacheImageCount, 0, 1);
            this.tableLayoutPanelImages.Controls.Add(this.lblCacheImageExpiry, 0, 0);
            this.tableLayoutPanelImages.Controls.Add(this.nudCacheImageExpiry, 1, 0);
            this.tableLayoutPanelImages.Controls.Add(this.txtCacheImageCount, 1, 1);
            this.tableLayoutPanelImages.Controls.Add(this.btnCacheImageClear, 1, 3);
            this.tableLayoutPanelImages.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelImages.Location = new System.Drawing.Point(0, 9);
            this.tableLayoutPanelImages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanelImages.Name = "tableLayoutPanelImages";
            this.tableLayoutPanelImages.RowCount = 4;
            this.tableLayoutPanelImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImages.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelImages.Size = new System.Drawing.Size(835, 134);
            this.tableLayoutPanelImages.TabIndex = 1;
            // 
            // btnCacheImageBrowse
            // 
            this.btnCacheImageBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheImageBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheImageBrowse.Image")));
            this.btnCacheImageBrowse.Location = new System.Drawing.Point(533, 96);
            this.btnCacheImageBrowse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCacheImageBrowse.Name = "btnCacheImageBrowse";
            this.btnCacheImageBrowse.Size = new System.Drawing.Size(298, 35);
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
            this.tableLayoutPanelImages.SetColumnSpan(this.txtCacheImageSize, 2);
            this.txtCacheImageSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheImageSize.Location = new System.Drawing.Point(227, 65);
            this.txtCacheImageSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCacheImageSize.Name = "txtCacheImageSize";
            this.txtCacheImageSize.ReadOnly = true;
            this.txtCacheImageSize.Size = new System.Drawing.Size(604, 25);
            this.txtCacheImageSize.TabIndex = 13;
            this.txtCacheImageSize.Text = "0";
            this.txtCacheImageSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCacheImageSize
            // 
            this.lblCacheImageSize.AutoEllipsis = true;
            this.lblCacheImageSize.AutoSize = true;
            this.lblCacheImageSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheImageSize.Location = new System.Drawing.Point(4, 62);
            this.lblCacheImageSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheImageSize.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheImageSize.Name = "lblCacheImageSize";
            this.lblCacheImageSize.Size = new System.Drawing.Size(215, 31);
            this.lblCacheImageSize.TabIndex = 14;
            this.lblCacheImageSize.Text = "Current disk size in bytes";
            this.lblCacheImageSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheImageCount
            // 
            this.lblCacheImageCount.AutoEllipsis = true;
            this.lblCacheImageCount.AutoSize = true;
            this.lblCacheImageCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheImageCount.Location = new System.Drawing.Point(4, 31);
            this.lblCacheImageCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheImageCount.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheImageCount.Name = "lblCacheImageCount";
            this.lblCacheImageCount.Size = new System.Drawing.Size(215, 31);
            this.lblCacheImageCount.TabIndex = 15;
            this.lblCacheImageCount.Text = "Current item count";
            this.lblCacheImageCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCacheImageExpiry
            // 
            this.lblCacheImageExpiry.AutoEllipsis = true;
            this.lblCacheImageExpiry.AutoSize = true;
            this.lblCacheImageExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCacheImageExpiry.Location = new System.Drawing.Point(4, 0);
            this.lblCacheImageExpiry.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCacheImageExpiry.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblCacheImageExpiry.Name = "lblCacheImageExpiry";
            this.lblCacheImageExpiry.Size = new System.Drawing.Size(215, 31);
            this.lblCacheImageExpiry.TabIndex = 16;
            this.lblCacheImageExpiry.Text = "Expiration in minutes";
            this.lblCacheImageExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudCacheImageExpiry
            // 
            this.tableLayoutPanelImages.SetColumnSpan(this.nudCacheImageExpiry, 2);
            this.nudCacheImageExpiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudCacheImageExpiry.Location = new System.Drawing.Point(227, 3);
            this.nudCacheImageExpiry.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudCacheImageExpiry.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudCacheImageExpiry.Name = "nudCacheImageExpiry";
            this.nudCacheImageExpiry.Size = new System.Drawing.Size(604, 25);
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
            this.tableLayoutPanelImages.SetColumnSpan(this.txtCacheImageCount, 2);
            this.txtCacheImageCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCacheImageCount.Location = new System.Drawing.Point(227, 34);
            this.txtCacheImageCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtCacheImageCount.Name = "txtCacheImageCount";
            this.txtCacheImageCount.ReadOnly = true;
            this.txtCacheImageCount.Size = new System.Drawing.Size(604, 25);
            this.txtCacheImageCount.TabIndex = 11;
            this.txtCacheImageCount.Text = "0";
            this.txtCacheImageCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnCacheImageClear
            // 
            this.btnCacheImageClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCacheImageClear.Image = ((System.Drawing.Image)(resources.GetObject("btnCacheImageClear.Image")));
            this.btnCacheImageClear.Location = new System.Drawing.Point(227, 96);
            this.btnCacheImageClear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCacheImageClear.Name = "btnCacheImageClear";
            this.btnCacheImageClear.Size = new System.Drawing.Size(298, 35);
            this.btnCacheImageClear.TabIndex = 18;
            this.btnCacheImageClear.TabStop = false;
            this.btnCacheImageClear.Text = "Clear cache";
            this.btnCacheImageClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCacheImageClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCacheImageClear.UseMnemonic = false;
            this.btnCacheImageClear.UseVisualStyleBackColor = true;
            this.btnCacheImageClear.Click += new System.EventHandler(this.btnCacheImageClear_Click);
            // 
            // capImageFiles
            // 
            this.capImageFiles.Caption = "Image files";
            this.capImageFiles.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capImageFiles.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capImageFiles.Collapsed = false;
            this.capImageFiles.Collapsee = null;
            this.capImageFiles.Collapsible = false;
            this.capImageFiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.capImageFiles.Location = new System.Drawing.Point(0, 0);
            this.capImageFiles.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capImageFiles.Name = "capImageFiles";
            this.capImageFiles.Size = new System.Drawing.Size(835, 35);
            this.capImageFiles.TabIndex = 18;
            // 
            // tabUpdates
            // 
            this.tabUpdates.AutoScroll = true;
            this.tabUpdates.Controls.Add(this.pnlUpdateChannel);
            this.tabUpdates.Controls.Add(this.capUpdateChannel);
            this.tabUpdates.Location = new System.Drawing.Point(4, 28);
            this.tabUpdates.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabUpdates.Name = "tabUpdates";
            this.tabUpdates.Size = new System.Drawing.Size(856, 597);
            this.tabUpdates.TabIndex = 5;
            this.tabUpdates.Text = "Updates";
            this.tabUpdates.UseVisualStyleBackColor = true;
            // 
            // pnlUpdateChannel
            // 
            this.pnlUpdateChannel.AutoSize = true;
            this.pnlUpdateChannel.Controls.Add(this.tableUpdateChannel);
            this.pnlUpdateChannel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUpdateChannel.Location = new System.Drawing.Point(0, 35);
            this.pnlUpdateChannel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlUpdateChannel.Name = "pnlUpdateChannel";
            this.pnlUpdateChannel.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.pnlUpdateChannel.Size = new System.Drawing.Size(856, 526);
            this.pnlUpdateChannel.TabIndex = 20;
            // 
            // tableUpdateChannel
            // 
            this.tableUpdateChannel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableUpdateChannel.ColumnCount = 2;
            this.tableUpdateChannel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableUpdateChannel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableUpdateChannel.Controls.Add(this.chkUpdates, 1, 3);
            this.tableUpdateChannel.Controls.Add(this.lblUpdates, 0, 3);
            this.tableUpdateChannel.Controls.Add(this.nudUpdateInterval, 1, 4);
            this.tableUpdateChannel.Controls.Add(this.lblUpdateInterval, 0, 4);
            this.tableUpdateChannel.Controls.Add(this.lblExternalUpdateWarn, 1, 1);
            this.tableUpdateChannel.Controls.Add(this.txtUpdateChannelUrl, 1, 2);
            this.tableUpdateChannel.Controls.Add(this.cbxUpdateMethod, 1, 0);
            this.tableUpdateChannel.Controls.Add(this.lblUpdateChannelUrl, 0, 2);
            this.tableUpdateChannel.Controls.Add(this.lblUpdateMethod, 0, 0);
            this.tableUpdateChannel.Controls.Add(this.btnUpdateCheck, 1, 5);
            this.tableUpdateChannel.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableUpdateChannel.Location = new System.Drawing.Point(0, 9);
            this.tableUpdateChannel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableUpdateChannel.Name = "tableUpdateChannel";
            this.tableUpdateChannel.RowCount = 6;
            this.tableUpdateChannel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUpdateChannel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUpdateChannel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUpdateChannel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUpdateChannel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUpdateChannel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableUpdateChannel.Size = new System.Drawing.Size(856, 517);
            this.tableUpdateChannel.TabIndex = 1;
            // 
            // chkUpdates
            // 
            this.chkUpdates.AutoSize = true;
            this.chkUpdates.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkUpdates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkUpdates.Location = new System.Drawing.Point(212, 178);
            this.chkUpdates.Margin = new System.Windows.Forms.Padding(4, 10, 4, 10);
            this.chkUpdates.Name = "chkUpdates";
            this.chkUpdates.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkUpdates.Size = new System.Drawing.Size(640, 23);
            this.chkUpdates.TabIndex = 34;
            this.chkUpdates.TabStop = false;
            this.chkUpdates.UseVisualStyleBackColor = true;
            // 
            // lblUpdates
            // 
            this.lblUpdates.AutoEllipsis = true;
            this.lblUpdates.AutoSize = true;
            this.lblUpdates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUpdates.Location = new System.Drawing.Point(4, 168);
            this.lblUpdates.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpdates.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblUpdates.Name = "lblUpdates";
            this.lblUpdates.Size = new System.Drawing.Size(200, 43);
            this.lblUpdates.TabIndex = 33;
            this.lblUpdates.Text = "Auto Check";
            this.lblUpdates.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudUpdateInterval
            // 
            this.nudUpdateInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudUpdateInterval.Increment = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudUpdateInterval.Location = new System.Drawing.Point(212, 221);
            this.nudUpdateInterval.Margin = new System.Windows.Forms.Padding(4, 10, 4, 10);
            this.nudUpdateInterval.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            0});
            this.nudUpdateInterval.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudUpdateInterval.Name = "nudUpdateInterval";
            this.nudUpdateInterval.Size = new System.Drawing.Size(640, 25);
            this.nudUpdateInterval.TabIndex = 32;
            this.nudUpdateInterval.TabStop = false;
            this.nudUpdateInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudUpdateInterval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // lblUpdateInterval
            // 
            this.lblUpdateInterval.AutoEllipsis = true;
            this.lblUpdateInterval.AutoSize = true;
            this.lblUpdateInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUpdateInterval.Location = new System.Drawing.Point(4, 211);
            this.lblUpdateInterval.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpdateInterval.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblUpdateInterval.Name = "lblUpdateInterval";
            this.lblUpdateInterval.Size = new System.Drawing.Size(200, 45);
            this.lblUpdateInterval.TabIndex = 31;
            this.lblUpdateInterval.Text = "Check Interval (min)";
            this.lblUpdateInterval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExternalUpdateWarn
            // 
            this.lblExternalUpdateWarn.AutoSize = true;
            this.lblExternalUpdateWarn.BackColor = System.Drawing.SystemColors.Info;
            this.lblExternalUpdateWarn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblExternalUpdateWarn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExternalUpdateWarn.Location = new System.Drawing.Point(212, 43);
            this.lblExternalUpdateWarn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExternalUpdateWarn.Name = "lblExternalUpdateWarn";
            this.lblExternalUpdateWarn.Padding = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.lblExternalUpdateWarn.Size = new System.Drawing.Size(640, 80);
            this.lblExternalUpdateWarn.TabIndex = 29;
            this.lblExternalUpdateWarn.Text = resources.GetString("lblExternalUpdateWarn.Text");
            this.lblExternalUpdateWarn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtUpdateChannelUrl
            // 
            this.txtUpdateChannelUrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtUpdateChannelUrl.Location = new System.Drawing.Point(212, 133);
            this.txtUpdateChannelUrl.Margin = new System.Windows.Forms.Padding(4, 10, 4, 10);
            this.txtUpdateChannelUrl.Name = "txtUpdateChannelUrl";
            this.txtUpdateChannelUrl.Size = new System.Drawing.Size(640, 25);
            this.txtUpdateChannelUrl.TabIndex = 22;
            // 
            // cbxUpdateMethod
            // 
            this.cbxUpdateMethod.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxUpdateMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxUpdateMethod.FormattingEnabled = true;
            this.cbxUpdateMethod.Items.AddRange(new object[] {
            "Built-in Github check (legacy)",
            "Official (recommended)",
            "External third party"});
            this.cbxUpdateMethod.Location = new System.Drawing.Point(212, 10);
            this.cbxUpdateMethod.Margin = new System.Windows.Forms.Padding(4, 10, 4, 10);
            this.cbxUpdateMethod.Name = "cbxUpdateMethod";
            this.cbxUpdateMethod.Size = new System.Drawing.Size(640, 23);
            this.cbxUpdateMethod.TabIndex = 12;
            this.cbxUpdateMethod.SelectedIndexChanged += new System.EventHandler(this.cbxUpdateChannel_SelectedIndexChanged);
            // 
            // lblUpdateChannelUrl
            // 
            this.lblUpdateChannelUrl.AutoEllipsis = true;
            this.lblUpdateChannelUrl.AutoSize = true;
            this.lblUpdateChannelUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUpdateChannelUrl.Location = new System.Drawing.Point(4, 123);
            this.lblUpdateChannelUrl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpdateChannelUrl.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblUpdateChannelUrl.Name = "lblUpdateChannelUrl";
            this.lblUpdateChannelUrl.Size = new System.Drawing.Size(200, 45);
            this.lblUpdateChannelUrl.TabIndex = 11;
            this.lblUpdateChannelUrl.Text = "Channel URI";
            this.lblUpdateChannelUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUpdateMethod
            // 
            this.lblUpdateMethod.AutoEllipsis = true;
            this.lblUpdateMethod.AutoSize = true;
            this.lblUpdateMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUpdateMethod.Location = new System.Drawing.Point(4, 0);
            this.lblUpdateMethod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpdateMethod.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblUpdateMethod.Name = "lblUpdateMethod";
            this.lblUpdateMethod.Size = new System.Drawing.Size(200, 43);
            this.lblUpdateMethod.TabIndex = 9;
            this.lblUpdateMethod.Text = "Update method";
            this.lblUpdateMethod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUpdateCheck
            // 
            this.btnUpdateCheck.AutoSize = true;
            this.btnUpdateCheck.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUpdateCheck.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUpdateCheck.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateCheck.Image")));
            this.btnUpdateCheck.Location = new System.Drawing.Point(212, 266);
            this.btnUpdateCheck.Margin = new System.Windows.Forms.Padding(4, 10, 4, 10);
            this.btnUpdateCheck.Name = "btnUpdateCheck";
            this.btnUpdateCheck.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.btnUpdateCheck.Size = new System.Drawing.Size(640, 29);
            this.btnUpdateCheck.TabIndex = 30;
            this.btnUpdateCheck.Text = "Check for updates";
            this.btnUpdateCheck.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdateCheck.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdateCheck.UseVisualStyleBackColor = true;
            this.btnUpdateCheck.Click += new System.EventHandler(this.btnUpdateCheck_Click);
            // 
            // capUpdateChannel
            // 
            this.capUpdateChannel.Caption = "Update channel";
            this.capUpdateChannel.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capUpdateChannel.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capUpdateChannel.Collapsed = false;
            this.capUpdateChannel.Collapsee = null;
            this.capUpdateChannel.Collapsible = false;
            this.capUpdateChannel.Dock = System.Windows.Forms.DockStyle.Top;
            this.capUpdateChannel.Location = new System.Drawing.Point(0, 0);
            this.capUpdateChannel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capUpdateChannel.Name = "capUpdateChannel";
            this.capUpdateChannel.Size = new System.Drawing.Size(856, 35);
            this.capUpdateChannel.TabIndex = 19;
            // 
            // tabEndpoint
            // 
            this.tabEndpoint.AutoScroll = true;
            this.tabEndpoint.Controls.Add(this.pnlEndpointHistory);
            this.tabEndpoint.Controls.Add(this.capEndpointHistory);
            this.tabEndpoint.Controls.Add(this.pnlEndpointSettings);
            this.tabEndpoint.Controls.Add(this.capEndpointSettings);
            this.tabEndpoint.Controls.Add(this.pnlEndpointStatus);
            this.tabEndpoint.Controls.Add(this.capEndpointStatus);
            this.tabEndpoint.Location = new System.Drawing.Point(4, 28);
            this.tabEndpoint.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabEndpoint.Name = "tabEndpoint";
            this.tabEndpoint.Size = new System.Drawing.Size(856, 597);
            this.tabEndpoint.TabIndex = 6;
            this.tabEndpoint.Text = "Endpoint";
            this.tabEndpoint.UseVisualStyleBackColor = true;
            // 
            // pnlEndpointHistory
            // 
            this.pnlEndpointHistory.AutoSize = true;
            this.pnlEndpointHistory.Controls.Add(this.dgvEndpointHistory);
            this.pnlEndpointHistory.Controls.Add(this.tlsEndpointHistory);
            this.pnlEndpointHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEndpointHistory.Location = new System.Drawing.Point(0, 318);
            this.pnlEndpointHistory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlEndpointHistory.MinimumSize = new System.Drawing.Size(0, 173);
            this.pnlEndpointHistory.Name = "pnlEndpointHistory";
            this.pnlEndpointHistory.Size = new System.Drawing.Size(856, 279);
            this.pnlEndpointHistory.TabIndex = 26;
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
            this.dgvEndpointHistory.Location = new System.Drawing.Point(0, 39);
            this.dgvEndpointHistory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvEndpointHistory.Name = "dgvEndpointHistory";
            this.dgvEndpointHistory.RowHeadersVisible = false;
            this.dgvEndpointHistory.RowHeadersWidth = 51;
            this.dgvEndpointHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEndpointHistory.Size = new System.Drawing.Size(856, 240);
            this.dgvEndpointHistory.TabIndex = 2;
            this.dgvEndpointHistory.TabStop = false;
            this.dgvEndpointHistory.VirtualMode = true;
            this.dgvEndpointHistory.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvEndpointHistory_CellValueNeeded);
            // 
            // colEndpointHistoryTimestamp
            // 
            this.colEndpointHistoryTimestamp.Frozen = true;
            this.colEndpointHistoryTimestamp.HeaderText = "Timestamp";
            this.colEndpointHistoryTimestamp.MinimumWidth = 6;
            this.colEndpointHistoryTimestamp.Name = "colEndpointHistoryTimestamp";
            this.colEndpointHistoryTimestamp.ReadOnly = true;
            this.colEndpointHistoryTimestamp.Width = 150;
            // 
            // colEndpointHistoryData
            // 
            this.colEndpointHistoryData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colEndpointHistoryData.HeaderText = "Data";
            this.colEndpointHistoryData.MinimumWidth = 6;
            this.colEndpointHistoryData.Name = "colEndpointHistoryData";
            this.colEndpointHistoryData.ReadOnly = true;
            // 
            // tlsEndpointHistory
            // 
            this.tlsEndpointHistory.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsEndpointHistory.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tlsEndpointHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEndpointHistUpdate,
            this.tlsEndpointHistoryRecv,
            this.tlsEndpointHistoryCount});
            this.tlsEndpointHistory.Location = new System.Drawing.Point(0, 0);
            this.tlsEndpointHistory.Name = "tlsEndpointHistory";
            this.tlsEndpointHistory.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlsEndpointHistory.Size = new System.Drawing.Size(856, 39);
            this.tlsEndpointHistory.TabIndex = 3;
            // 
            // btnEndpointHistUpdate
            // 
            this.btnEndpointHistUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnEndpointHistUpdate.Image")));
            this.btnEndpointHistUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEndpointHistUpdate.Name = "btnEndpointHistUpdate";
            this.btnEndpointHistUpdate.Padding = new System.Windows.Forms.Padding(4);
            this.btnEndpointHistUpdate.Size = new System.Drawing.Size(95, 32);
            this.btnEndpointHistUpdate.Text = "Update";
            this.btnEndpointHistUpdate.Click += new System.EventHandler(this.btnEndpointHistUpdate_Click);
            // 
            // tlsEndpointHistoryRecv
            // 
            this.tlsEndpointHistoryRecv.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tlsEndpointHistoryRecv.Name = "tlsEndpointHistoryRecv";
            this.tlsEndpointHistoryRecv.Size = new System.Drawing.Size(159, 32);
            this.tlsEndpointHistoryRecv.Text = "telegram(s) received";
            // 
            // tlsEndpointHistoryCount
            // 
            this.tlsEndpointHistoryCount.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tlsEndpointHistoryCount.Name = "tlsEndpointHistoryCount";
            this.tlsEndpointHistoryCount.Size = new System.Drawing.Size(18, 32);
            this.tlsEndpointHistoryCount.Text = "0";
            // 
            // capEndpointHistory
            // 
            this.capEndpointHistory.Caption = "History";
            this.capEndpointHistory.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capEndpointHistory.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capEndpointHistory.Collapsed = false;
            this.capEndpointHistory.Collapsee = null;
            this.capEndpointHistory.Collapsible = false;
            this.capEndpointHistory.Dock = System.Windows.Forms.DockStyle.Top;
            this.capEndpointHistory.Location = new System.Drawing.Point(0, 283);
            this.capEndpointHistory.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capEndpointHistory.Name = "capEndpointHistory";
            this.capEndpointHistory.Size = new System.Drawing.Size(856, 35);
            this.capEndpointHistory.TabIndex = 25;
            // 
            // pnlEndpointSettings
            // 
            this.pnlEndpointSettings.AutoScroll = true;
            this.pnlEndpointSettings.AutoSize = true;
            this.pnlEndpointSettings.Controls.Add(this.tableEndpointSettings);
            this.pnlEndpointSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEndpointSettings.Location = new System.Drawing.Point(0, 176);
            this.pnlEndpointSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlEndpointSettings.Name = "pnlEndpointSettings";
            this.pnlEndpointSettings.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlEndpointSettings.Size = new System.Drawing.Size(856, 107);
            this.pnlEndpointSettings.TabIndex = 24;
            // 
            // tableEndpointSettings
            // 
            this.tableEndpointSettings.AutoSize = true;
            this.tableEndpointSettings.ColumnCount = 2;
            this.tableEndpointSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableEndpointSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableEndpointSettings.Controls.Add(this.lblEndpointLog, 0, 2);
            this.tableEndpointSettings.Controls.Add(this.lblEndpointStartup, 0, 1);
            this.tableEndpointSettings.Controls.Add(this.chkEndpointStartup, 1, 1);
            this.tableEndpointSettings.Controls.Add(this.chkEndpointLog, 1, 2);
            this.tableEndpointSettings.Controls.Add(this.txtEndpoint, 1, 0);
            this.tableEndpointSettings.Controls.Add(this.lblEndpoint, 0, 0);
            this.tableEndpointSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableEndpointSettings.Location = new System.Drawing.Point(0, 9);
            this.tableEndpointSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableEndpointSettings.Name = "tableEndpointSettings";
            this.tableEndpointSettings.RowCount = 3;
            this.tableEndpointSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEndpointSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEndpointSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableEndpointSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableEndpointSettings.Size = new System.Drawing.Size(856, 89);
            this.tableEndpointSettings.TabIndex = 2;
            // 
            // lblEndpointLog
            // 
            this.lblEndpointLog.AutoEllipsis = true;
            this.lblEndpointLog.AutoSize = true;
            this.lblEndpointLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEndpointLog.Location = new System.Drawing.Point(4, 60);
            this.lblEndpointLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEndpointLog.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblEndpointLog.Name = "lblEndpointLog";
            this.lblEndpointLog.Size = new System.Drawing.Size(200, 29);
            this.lblEndpointLog.TabIndex = 10;
            this.lblEndpointLog.Text = "Log received data";
            this.lblEndpointLog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEndpointStartup
            // 
            this.lblEndpointStartup.AutoEllipsis = true;
            this.lblEndpointStartup.AutoSize = true;
            this.lblEndpointStartup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEndpointStartup.Location = new System.Drawing.Point(4, 31);
            this.lblEndpointStartup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEndpointStartup.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblEndpointStartup.Name = "lblEndpointStartup";
            this.lblEndpointStartup.Size = new System.Drawing.Size(200, 29);
            this.lblEndpointStartup.TabIndex = 9;
            this.lblEndpointStartup.Text = "Start endpoint on launch";
            this.lblEndpointStartup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkEndpointStartup
            // 
            this.chkEndpointStartup.AutoSize = true;
            this.chkEndpointStartup.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEndpointStartup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkEndpointStartup.Location = new System.Drawing.Point(212, 34);
            this.chkEndpointStartup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkEndpointStartup.Name = "chkEndpointStartup";
            this.chkEndpointStartup.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkEndpointStartup.Size = new System.Drawing.Size(640, 23);
            this.chkEndpointStartup.TabIndex = 0;
            this.chkEndpointStartup.TabStop = false;
            this.chkEndpointStartup.UseVisualStyleBackColor = true;
            // 
            // chkEndpointLog
            // 
            this.chkEndpointLog.AutoSize = true;
            this.chkEndpointLog.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEndpointLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkEndpointLog.Location = new System.Drawing.Point(212, 63);
            this.chkEndpointLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkEndpointLog.Name = "chkEndpointLog";
            this.chkEndpointLog.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkEndpointLog.Size = new System.Drawing.Size(640, 23);
            this.chkEndpointLog.TabIndex = 1;
            this.chkEndpointLog.TabStop = false;
            this.chkEndpointLog.UseVisualStyleBackColor = true;
            // 
            // txtEndpoint
            // 
            this.txtEndpoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEndpoint.Location = new System.Drawing.Point(212, 3);
            this.txtEndpoint.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtEndpoint.Name = "txtEndpoint";
            this.txtEndpoint.Size = new System.Drawing.Size(640, 25);
            this.txtEndpoint.TabIndex = 7;
            // 
            // lblEndpoint
            // 
            this.lblEndpoint.AutoEllipsis = true;
            this.lblEndpoint.AutoSize = true;
            this.lblEndpoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEndpoint.Location = new System.Drawing.Point(4, 0);
            this.lblEndpoint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEndpoint.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblEndpoint.Name = "lblEndpoint";
            this.lblEndpoint.Size = new System.Drawing.Size(200, 31);
            this.lblEndpoint.TabIndex = 8;
            this.lblEndpoint.Text = "HTTP endpoint";
            this.lblEndpoint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // capEndpointSettings
            // 
            this.capEndpointSettings.Caption = "Settings";
            this.capEndpointSettings.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capEndpointSettings.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capEndpointSettings.Collapsed = false;
            this.capEndpointSettings.Collapsee = null;
            this.capEndpointSettings.Collapsible = false;
            this.capEndpointSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.capEndpointSettings.Location = new System.Drawing.Point(0, 141);
            this.capEndpointSettings.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capEndpointSettings.Name = "capEndpointSettings";
            this.capEndpointSettings.Size = new System.Drawing.Size(856, 35);
            this.capEndpointSettings.TabIndex = 23;
            // 
            // pnlEndpointStatus
            // 
            this.pnlEndpointStatus.AutoSize = true;
            this.pnlEndpointStatus.Controls.Add(this.tableEndpointStatus);
            this.pnlEndpointStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEndpointStatus.Location = new System.Drawing.Point(0, 35);
            this.pnlEndpointStatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlEndpointStatus.Name = "pnlEndpointStatus";
            this.pnlEndpointStatus.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlEndpointStatus.Size = new System.Drawing.Size(856, 106);
            this.pnlEndpointStatus.TabIndex = 22;
            // 
            // tableEndpointStatus
            // 
            this.tableEndpointStatus.AutoSize = true;
            this.tableEndpointStatus.ColumnCount = 2;
            this.tableEndpointStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableEndpointStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableEndpointStatus.Controls.Add(this.btnEndpointStart, 1, 0);
            this.tableEndpointStatus.Controls.Add(this.btnEndpointStop, 1, 1);
            this.tableEndpointStatus.Controls.Add(this.txtEndpointStatus, 0, 0);
            this.tableEndpointStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableEndpointStatus.Location = new System.Drawing.Point(0, 9);
            this.tableEndpointStatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableEndpointStatus.Name = "tableEndpointStatus";
            this.tableEndpointStatus.RowCount = 2;
            this.tableEndpointStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableEndpointStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableEndpointStatus.Size = new System.Drawing.Size(856, 88);
            this.tableEndpointStatus.TabIndex = 1;
            // 
            // btnEndpointStart
            // 
            this.btnEndpointStart.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEndpointStart.Image = ((System.Drawing.Image)(resources.GetObject("btnEndpointStart.Image")));
            this.btnEndpointStart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEndpointStart.Location = new System.Drawing.Point(745, 3);
            this.btnEndpointStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnEndpointStart.Name = "btnEndpointStart";
            this.btnEndpointStart.Size = new System.Drawing.Size(107, 38);
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
            this.btnEndpointStop.Location = new System.Drawing.Point(745, 47);
            this.btnEndpointStop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnEndpointStop.Name = "btnEndpointStop";
            this.btnEndpointStop.Size = new System.Drawing.Size(107, 38);
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
            this.txtEndpointStatus.Location = new System.Drawing.Point(4, 3);
            this.txtEndpointStatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtEndpointStatus.Multiline = true;
            this.txtEndpointStatus.Name = "txtEndpointStatus";
            this.txtEndpointStatus.ReadOnly = true;
            this.tableEndpointStatus.SetRowSpan(this.txtEndpointStatus, 2);
            this.txtEndpointStatus.Size = new System.Drawing.Size(733, 82);
            this.txtEndpointStatus.TabIndex = 2;
            // 
            // capEndpointStatus
            // 
            this.capEndpointStatus.Caption = "Status";
            this.capEndpointStatus.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capEndpointStatus.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capEndpointStatus.Collapsed = false;
            this.capEndpointStatus.Collapsee = null;
            this.capEndpointStatus.Collapsible = false;
            this.capEndpointStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.capEndpointStatus.Location = new System.Drawing.Point(0, 0);
            this.capEndpointStatus.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capEndpointStatus.Name = "capEndpointStatus";
            this.capEndpointStatus.Size = new System.Drawing.Size(856, 35);
            this.capEndpointStatus.TabIndex = 21;
            // 
            // tabConstants
            // 
            this.tabConstants.AutoScroll = true;
            this.tabConstants.Controls.Add(this.dgvConstVariables);
            this.tabConstants.Controls.Add(this.tlsConstant);
            this.tabConstants.Location = new System.Drawing.Point(4, 28);
            this.tabConstants.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabConstants.Name = "tabConstants";
            this.tabConstants.Size = new System.Drawing.Size(856, 597);
            this.tabConstants.TabIndex = 7;
            this.tabConstants.Text = "Constants";
            this.tabConstants.UseVisualStyleBackColor = true;
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
            this.dgvConstVariables.Location = new System.Drawing.Point(0, 39);
            this.dgvConstVariables.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvConstVariables.Name = "dgvConstVariables";
            this.dgvConstVariables.RowHeadersVisible = false;
            this.dgvConstVariables.RowHeadersWidth = 51;
            this.dgvConstVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConstVariables.ShowCellErrors = false;
            this.dgvConstVariables.ShowEditingIcon = false;
            this.dgvConstVariables.ShowRowErrors = false;
            this.dgvConstVariables.Size = new System.Drawing.Size(856, 558);
            this.dgvConstVariables.TabIndex = 2;
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
            this.colScalarName.MinimumWidth = 6;
            this.colScalarName.Name = "colScalarName";
            this.colScalarName.ReadOnly = true;
            this.colScalarName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colScalarName.Width = 200;
            // 
            // colScalarValue
            // 
            this.colScalarValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colScalarValue.HeaderText = "Value";
            this.colScalarValue.MinimumWidth = 6;
            this.colScalarValue.Name = "colScalarValue";
            this.colScalarValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tlsConstant
            // 
            this.tlsConstant.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsConstant.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tlsConstant.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConstAdd,
            this.btnConstEdit,
            this.btnConstRemove});
            this.tlsConstant.Location = new System.Drawing.Point(0, 0);
            this.tlsConstant.Name = "tlsConstant";
            this.tlsConstant.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlsConstant.Size = new System.Drawing.Size(856, 39);
            this.tlsConstant.TabIndex = 3;
            // 
            // btnConstAdd
            // 
            this.btnConstAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnConstAdd.Image")));
            this.btnConstAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConstAdd.Name = "btnConstAdd";
            this.btnConstAdd.Padding = new System.Windows.Forms.Padding(4);
            this.btnConstAdd.Size = new System.Drawing.Size(139, 32);
            this.btnConstAdd.Text = "Add constant";
            this.btnConstAdd.Click += new System.EventHandler(this.btnConstAdd_Click);
            // 
            // btnConstEdit
            // 
            this.btnConstEdit.Enabled = false;
            this.btnConstEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnConstEdit.Image")));
            this.btnConstEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConstEdit.Name = "btnConstEdit";
            this.btnConstEdit.Padding = new System.Windows.Forms.Padding(4);
            this.btnConstEdit.Size = new System.Drawing.Size(136, 32);
            this.btnConstEdit.Text = "Edit constant";
            this.btnConstEdit.Click += new System.EventHandler(this.btnConstEdit_Click);
            // 
            // btnConstRemove
            // 
            this.btnConstRemove.Enabled = false;
            this.btnConstRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnConstRemove.Image")));
            this.btnConstRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConstRemove.Name = "btnConstRemove";
            this.btnConstRemove.Padding = new System.Windows.Forms.Padding(4);
            this.btnConstRemove.Size = new System.Drawing.Size(168, 32);
            this.btnConstRemove.Text = "Remove constant";
            this.btnConstRemove.ToolTipText = "Remove value";
            this.btnConstRemove.Click += new System.EventHandler(this.btnConstRemove_Click);
            // 
            // tabSubs
            // 
            this.tabSubs.AutoScroll = true;
            this.tabSubs.Controls.Add(this.dgvSubstitutions);
            this.tabSubs.Controls.Add(this.tlsSubstitutions);
            this.tabSubs.Location = new System.Drawing.Point(4, 28);
            this.tabSubs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabSubs.Name = "tabSubs";
            this.tabSubs.Size = new System.Drawing.Size(856, 597);
            this.tabSubs.TabIndex = 11;
            this.tabSubs.Text = "Substitutions";
            this.tabSubs.UseVisualStyleBackColor = true;
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
            this.dgvSubstitutions.Location = new System.Drawing.Point(0, 39);
            this.dgvSubstitutions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvSubstitutions.Name = "dgvSubstitutions";
            this.dgvSubstitutions.RowHeadersVisible = false;
            this.dgvSubstitutions.RowHeadersWidth = 51;
            this.dgvSubstitutions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSubstitutions.ShowCellErrors = false;
            this.dgvSubstitutions.ShowEditingIcon = false;
            this.dgvSubstitutions.ShowRowErrors = false;
            this.dgvSubstitutions.Size = new System.Drawing.Size(856, 558);
            this.dgvSubstitutions.TabIndex = 2;
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
            this.colSubSearchFor.MinimumWidth = 6;
            this.colSubSearchFor.Name = "colSubSearchFor";
            this.colSubSearchFor.ReadOnly = true;
            this.colSubSearchFor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSubSearchFor.Width = 150;
            // 
            // colSubReplaceWith
            // 
            this.colSubReplaceWith.HeaderText = "Replace with";
            this.colSubReplaceWith.MinimumWidth = 6;
            this.colSubReplaceWith.Name = "colSubReplaceWith";
            this.colSubReplaceWith.ReadOnly = true;
            this.colSubReplaceWith.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSubReplaceWith.Width = 150;
            // 
            // colSubScope
            // 
            this.colSubScope.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSubScope.HeaderText = "Scope";
            this.colSubScope.MinimumWidth = 6;
            this.colSubScope.Name = "colSubScope";
            this.colSubScope.ReadOnly = true;
            this.colSubScope.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tlsSubstitutions
            // 
            this.tlsSubstitutions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsSubstitutions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tlsSubstitutions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSubAdd,
            this.btnSubEdit,
            this.btnSubRemove});
            this.tlsSubstitutions.Location = new System.Drawing.Point(0, 0);
            this.tlsSubstitutions.Name = "tlsSubstitutions";
            this.tlsSubstitutions.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlsSubstitutions.Size = new System.Drawing.Size(856, 39);
            this.tlsSubstitutions.TabIndex = 3;
            // 
            // btnSubAdd
            // 
            this.btnSubAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnSubAdd.Image")));
            this.btnSubAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSubAdd.Name = "btnSubAdd";
            this.btnSubAdd.Padding = new System.Windows.Forms.Padding(4);
            this.btnSubAdd.Size = new System.Drawing.Size(163, 32);
            this.btnSubAdd.Text = "Add substitution";
            this.btnSubAdd.Click += new System.EventHandler(this.btnSubAdd_Click);
            // 
            // btnSubEdit
            // 
            this.btnSubEdit.Enabled = false;
            this.btnSubEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnSubEdit.Image")));
            this.btnSubEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSubEdit.Name = "btnSubEdit";
            this.btnSubEdit.Padding = new System.Windows.Forms.Padding(4);
            this.btnSubEdit.Size = new System.Drawing.Size(160, 32);
            this.btnSubEdit.Text = "Edit substitution";
            this.btnSubEdit.Click += new System.EventHandler(this.btnSubEdit_Click);
            // 
            // btnSubRemove
            // 
            this.btnSubRemove.Enabled = false;
            this.btnSubRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnSubRemove.Image")));
            this.btnSubRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSubRemove.Name = "btnSubRemove";
            this.btnSubRemove.Padding = new System.Windows.Forms.Padding(4);
            this.btnSubRemove.Size = new System.Drawing.Size(192, 32);
            this.btnSubRemove.Text = "Remove substitution";
            this.btnSubRemove.Click += new System.EventHandler(this.btnSubRemove_Click);
            // 
            // tabGameSpecific
            // 
            this.tabGameSpecific.AutoScroll = true;
            this.tabGameSpecific.Controls.Add(this.tbcGames);
            this.tabGameSpecific.Location = new System.Drawing.Point(4, 28);
            this.tabGameSpecific.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabGameSpecific.Name = "tabGameSpecific";
            this.tabGameSpecific.Size = new System.Drawing.Size(856, 597);
            this.tabGameSpecific.TabIndex = 8;
            this.tabGameSpecific.Text = "Game-specific";
            this.tabGameSpecific.UseVisualStyleBackColor = true;
            // 
            // tbcGames
            // 
            this.tbcGames.Controls.Add(this.tabFFXIV);
            this.tbcGames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcGames.Location = new System.Drawing.Point(0, 0);
            this.tbcGames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbcGames.Name = "tbcGames";
            this.tbcGames.SelectedIndex = 0;
            this.tbcGames.Size = new System.Drawing.Size(856, 597);
            this.tbcGames.TabIndex = 0;
            // 
            // tabFFXIV
            // 
            this.tabFFXIV.Controls.Add(this.pnlFfxivLogging);
            this.tabFFXIV.Controls.Add(this.capFfxivLogging);
            this.tabFFXIV.Location = new System.Drawing.Point(4, 25);
            this.tabFFXIV.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabFFXIV.Name = "tabFFXIV";
            this.tabFFXIV.Padding = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.tabFFXIV.Size = new System.Drawing.Size(848, 568);
            this.tabFFXIV.TabIndex = 0;
            this.tabFFXIV.Text = "Final Fantasy XIV";
            this.tabFFXIV.UseVisualStyleBackColor = true;
            // 
            // pnlFfxivLogging
            // 
            this.pnlFfxivLogging.AutoSize = true;
            this.pnlFfxivLogging.Controls.Add(this.tableFfxivLogging);
            this.pnlFfxivLogging.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFfxivLogging.Location = new System.Drawing.Point(11, 44);
            this.pnlFfxivLogging.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlFfxivLogging.Name = "pnlFfxivLogging";
            this.pnlFfxivLogging.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlFfxivLogging.Size = new System.Drawing.Size(826, 47);
            this.pnlFfxivLogging.TabIndex = 21;
            // 
            // tableFfxivLogging
            // 
            this.tableFfxivLogging.AutoSize = true;
            this.tableFfxivLogging.ColumnCount = 2;
            this.tableFfxivLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableFfxivLogging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableFfxivLogging.Controls.Add(this.lblFfxivLogNetwork, 0, 1);
            this.tableFfxivLogging.Controls.Add(this.chkFfxivLogNetwork, 1, 1);
            this.tableFfxivLogging.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableFfxivLogging.Location = new System.Drawing.Point(0, 9);
            this.tableFfxivLogging.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableFfxivLogging.Name = "tableFfxivLogging";
            this.tableFfxivLogging.RowCount = 2;
            this.tableFfxivLogging.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableFfxivLogging.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableFfxivLogging.Size = new System.Drawing.Size(826, 29);
            this.tableFfxivLogging.TabIndex = 1;
            // 
            // lblFfxivLogNetwork
            // 
            this.lblFfxivLogNetwork.AutoEllipsis = true;
            this.lblFfxivLogNetwork.AutoSize = true;
            this.lblFfxivLogNetwork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFfxivLogNetwork.Location = new System.Drawing.Point(4, 0);
            this.lblFfxivLogNetwork.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFfxivLogNetwork.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblFfxivLogNetwork.Name = "lblFfxivLogNetwork";
            this.lblFfxivLogNetwork.Size = new System.Drawing.Size(200, 29);
            this.lblFfxivLogNetwork.TabIndex = 10;
            this.lblFfxivLogNetwork.Text = "Log network events";
            this.lblFfxivLogNetwork.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkFfxivLogNetwork
            // 
            this.chkFfxivLogNetwork.AutoSize = true;
            this.chkFfxivLogNetwork.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkFfxivLogNetwork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkFfxivLogNetwork.Location = new System.Drawing.Point(212, 3);
            this.chkFfxivLogNetwork.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkFfxivLogNetwork.Name = "chkFfxivLogNetwork";
            this.chkFfxivLogNetwork.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.chkFfxivLogNetwork.Size = new System.Drawing.Size(610, 23);
            this.chkFfxivLogNetwork.TabIndex = 0;
            this.chkFfxivLogNetwork.TabStop = false;
            this.chkFfxivLogNetwork.UseVisualStyleBackColor = true;
            // 
            // capFfxivLogging
            // 
            this.capFfxivLogging.Caption = "Logging";
            this.capFfxivLogging.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capFfxivLogging.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capFfxivLogging.Collapsed = false;
            this.capFfxivLogging.Collapsee = null;
            this.capFfxivLogging.Collapsible = false;
            this.capFfxivLogging.Dock = System.Windows.Forms.DockStyle.Top;
            this.capFfxivLogging.Location = new System.Drawing.Point(11, 9);
            this.capFfxivLogging.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capFfxivLogging.Name = "capFfxivLogging";
            this.capFfxivLogging.Size = new System.Drawing.Size(826, 35);
            this.capFfxivLogging.TabIndex = 20;
            // 
            // tabSecurity
            // 
            this.tabSecurity.AutoScroll = true;
            this.tabSecurity.Controls.Add(this.pnlScriptingAccess);
            this.tabSecurity.Controls.Add(this.capScripting);
            this.tabSecurity.Location = new System.Drawing.Point(4, 28);
            this.tabSecurity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabSecurity.Name = "tabSecurity";
            this.tabSecurity.Size = new System.Drawing.Size(856, 597);
            this.tabSecurity.TabIndex = 10;
            this.tabSecurity.Text = "Security";
            this.tabSecurity.UseVisualStyleBackColor = true;
            // 
            // pnlScriptingAccess
            // 
            this.pnlScriptingAccess.Controls.Add(this.dgvAdditionalFeatures);
            this.pnlScriptingAccess.Controls.Add(this.pnlSecurityFeaturesSpacing);
            this.pnlScriptingAccess.Controls.Add(this.dgvApiAccess);
            this.pnlScriptingAccess.Controls.Add(this.pnlSecurityUnlockSpacing);
            this.pnlScriptingAccess.Controls.Add(this.btnUnlockSecurity);
            this.pnlScriptingAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScriptingAccess.Location = new System.Drawing.Point(0, 35);
            this.pnlScriptingAccess.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlScriptingAccess.Name = "pnlScriptingAccess";
            this.pnlScriptingAccess.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.pnlScriptingAccess.Size = new System.Drawing.Size(856, 562);
            this.pnlScriptingAccess.TabIndex = 10;
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
            this.dgvAdditionalFeatures.Location = new System.Drawing.Point(0, 293);
            this.dgvAdditionalFeatures.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvAdditionalFeatures.Name = "dgvAdditionalFeatures";
            this.dgvAdditionalFeatures.ReadOnly = true;
            this.dgvAdditionalFeatures.RowHeadersVisible = false;
            this.dgvAdditionalFeatures.RowHeadersWidth = 51;
            this.dgvAdditionalFeatures.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdditionalFeatures.ShowCellErrors = false;
            this.dgvAdditionalFeatures.ShowEditingIcon = false;
            this.dgvAdditionalFeatures.ShowRowErrors = false;
            this.dgvAdditionalFeatures.Size = new System.Drawing.Size(856, 269);
            this.dgvAdditionalFeatures.TabIndex = 5;
            this.dgvAdditionalFeatures.TabStop = false;
            this.dgvAdditionalFeatures.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAdditionalFeatures_CellContentClick);
            this.dgvAdditionalFeatures.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAdditionalFeatures_CellContentDoubleClick);
            this.dgvAdditionalFeatures.SelectionChanged += new System.EventHandler(this.dgvAdditionalFeatures_SelectionChanged);
            // 
            // colFeatureName
            // 
            this.colFeatureName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFeatureName.HeaderText = "Additional features";
            this.colFeatureName.MinimumWidth = 6;
            this.colFeatureName.Name = "colFeatureName";
            this.colFeatureName.ReadOnly = true;
            this.colFeatureName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colFeatureName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colFeatureAllowLocal
            // 
            this.colFeatureAllowLocal.HeaderText = "Allow for local triggers";
            this.colFeatureAllowLocal.MinimumWidth = 6;
            this.colFeatureAllowLocal.Name = "colFeatureAllowLocal";
            this.colFeatureAllowLocal.ReadOnly = true;
            this.colFeatureAllowLocal.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colFeatureAllowLocal.Width = 125;
            // 
            // colFeatureAllowRemote
            // 
            this.colFeatureAllowRemote.HeaderText = "Allow for remote triggers";
            this.colFeatureAllowRemote.MinimumWidth = 6;
            this.colFeatureAllowRemote.Name = "colFeatureAllowRemote";
            this.colFeatureAllowRemote.ReadOnly = true;
            this.colFeatureAllowRemote.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colFeatureAllowRemote.Width = 125;
            // 
            // colFeatureAllowAdmin
            // 
            this.colFeatureAllowAdmin.HeaderText = "Allow if running as admin";
            this.colFeatureAllowAdmin.MinimumWidth = 6;
            this.colFeatureAllowAdmin.Name = "colFeatureAllowAdmin";
            this.colFeatureAllowAdmin.ReadOnly = true;
            this.colFeatureAllowAdmin.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colFeatureAllowAdmin.Width = 125;
            // 
            // pnlSecurityFeaturesSpacing
            // 
            this.pnlSecurityFeaturesSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSecurityFeaturesSpacing.Location = new System.Drawing.Point(0, 284);
            this.pnlSecurityFeaturesSpacing.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlSecurityFeaturesSpacing.Name = "pnlSecurityFeaturesSpacing";
            this.pnlSecurityFeaturesSpacing.Size = new System.Drawing.Size(856, 9);
            this.pnlSecurityFeaturesSpacing.TabIndex = 6;
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
            this.dgvApiAccess.Location = new System.Drawing.Point(0, 53);
            this.dgvApiAccess.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dgvApiAccess.Name = "dgvApiAccess";
            this.dgvApiAccess.ReadOnly = true;
            this.dgvApiAccess.RowHeadersVisible = false;
            this.dgvApiAccess.RowHeadersWidth = 51;
            this.dgvApiAccess.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvApiAccess.ShowCellErrors = false;
            this.dgvApiAccess.ShowEditingIcon = false;
            this.dgvApiAccess.ShowRowErrors = false;
            this.dgvApiAccess.Size = new System.Drawing.Size(856, 231);
            this.dgvApiAccess.TabIndex = 7;
            this.dgvApiAccess.TabStop = false;
            this.dgvApiAccess.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvApiAccess_CellContentClick);
            this.dgvApiAccess.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvApiAccess_CellContentDoubleClick);
            this.dgvApiAccess.SelectionChanged += new System.EventHandler(this.dgvApiAccess_SelectionChanged);
            // 
            // colApiNamespace
            // 
            this.colApiNamespace.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colApiNamespace.HeaderText = "Namespace";
            this.colApiNamespace.MinimumWidth = 6;
            this.colApiNamespace.Name = "colApiNamespace";
            this.colApiNamespace.ReadOnly = true;
            this.colApiNamespace.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colApiNamespace.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colApiAllowLocal
            // 
            this.colApiAllowLocal.HeaderText = "Allow for local triggers";
            this.colApiAllowLocal.MinimumWidth = 6;
            this.colApiAllowLocal.Name = "colApiAllowLocal";
            this.colApiAllowLocal.ReadOnly = true;
            this.colApiAllowLocal.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colApiAllowLocal.Width = 125;
            // 
            // colApiAllowRemote
            // 
            this.colApiAllowRemote.HeaderText = "Allow for remote triggers";
            this.colApiAllowRemote.MinimumWidth = 6;
            this.colApiAllowRemote.Name = "colApiAllowRemote";
            this.colApiAllowRemote.ReadOnly = true;
            this.colApiAllowRemote.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colApiAllowRemote.Width = 125;
            // 
            // colApiAllowAdmin
            // 
            this.colApiAllowAdmin.HeaderText = "Allow if running as admin";
            this.colApiAllowAdmin.MinimumWidth = 6;
            this.colApiAllowAdmin.Name = "colApiAllowAdmin";
            this.colApiAllowAdmin.ReadOnly = true;
            this.colApiAllowAdmin.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colApiAllowAdmin.Width = 125;
            // 
            // pnlSecurityUnlockSpacing
            // 
            this.pnlSecurityUnlockSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSecurityUnlockSpacing.Location = new System.Drawing.Point(0, 44);
            this.pnlSecurityUnlockSpacing.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlSecurityUnlockSpacing.Name = "pnlSecurityUnlockSpacing";
            this.pnlSecurityUnlockSpacing.Size = new System.Drawing.Size(856, 9);
            this.pnlSecurityUnlockSpacing.TabIndex = 8;
            // 
            // btnUnlockSecurity
            // 
            this.btnUnlockSecurity.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUnlockSecurity.Image = ((System.Drawing.Image)(resources.GetObject("btnUnlockSecurity.Image")));
            this.btnUnlockSecurity.Location = new System.Drawing.Point(0, 9);
            this.btnUnlockSecurity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUnlockSecurity.Name = "btnUnlockSecurity";
            this.btnUnlockSecurity.Size = new System.Drawing.Size(856, 35);
            this.btnUnlockSecurity.TabIndex = 9;
            this.btnUnlockSecurity.TabStop = false;
            this.btnUnlockSecurity.Text = "Unlock security settings for editing";
            this.btnUnlockSecurity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUnlockSecurity.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUnlockSecurity.UseVisualStyleBackColor = true;
            this.btnUnlockSecurity.Click += new System.EventHandler(this.btnUnlockSecurity_Click);
            // 
            // capScripting
            // 
            this.capScripting.Caption = "Scripting API access";
            this.capScripting.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capScripting.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capScripting.Collapsed = false;
            this.capScripting.Collapsee = null;
            this.capScripting.Collapsible = false;
            this.capScripting.Dock = System.Windows.Forms.DockStyle.Top;
            this.capScripting.Location = new System.Drawing.Point(0, 0);
            this.capScripting.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capScripting.Name = "capScripting";
            this.capScripting.Size = new System.Drawing.Size(856, 35);
            this.capScripting.TabIndex = 9;
            // 
            // tabMiscellaneous
            // 
            this.tabMiscellaneous.AutoScroll = true;
            this.tabMiscellaneous.Controls.Add(this.pnlAuraManagement);
            this.tabMiscellaneous.Controls.Add(this.capAuraManagement);
            this.tabMiscellaneous.Controls.Add(this.pnlAutosave);
            this.tabMiscellaneous.Controls.Add(this.capAutosaving);
            this.tabMiscellaneous.Controls.Add(this.pnlTriggerTemplate);
            this.tabMiscellaneous.Controls.Add(this.capTemplate);
            this.tabMiscellaneous.Location = new System.Drawing.Point(4, 28);
            this.tabMiscellaneous.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabMiscellaneous.Name = "tabMiscellaneous";
            this.tabMiscellaneous.Size = new System.Drawing.Size(856, 597);
            this.tabMiscellaneous.TabIndex = 9;
            this.tabMiscellaneous.Text = "Miscellaneous";
            this.tabMiscellaneous.UseVisualStyleBackColor = true;
            // 
            // pnlAuraManagement
            // 
            this.pnlAuraManagement.AutoSize = true;
            this.pnlAuraManagement.Controls.Add(this.tableAuraManagement);
            this.pnlAuraManagement.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAuraManagement.Location = new System.Drawing.Point(0, 234);
            this.pnlAuraManagement.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlAuraManagement.Name = "pnlAuraManagement";
            this.pnlAuraManagement.Padding = new System.Windows.Forms.Padding(0, 9, 0, 0);
            this.pnlAuraManagement.Size = new System.Drawing.Size(856, 69);
            this.pnlAuraManagement.TabIndex = 27;
            // 
            // tableAuraManagement
            // 
            this.tableAuraManagement.AutoSize = true;
            this.tableAuraManagement.ColumnCount = 2;
            this.tableAuraManagement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAuraManagement.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAuraManagement.Controls.Add(this.lblEnableHwAccel, 0, 1);
            this.tableAuraManagement.Controls.Add(this.txtMonitorWindow, 1, 0);
            this.tableAuraManagement.Controls.Add(this.lblMonitorWindow, 0, 0);
            this.tableAuraManagement.Controls.Add(this.cbxEnableHwAccel, 1, 1);
            this.tableAuraManagement.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableAuraManagement.Location = new System.Drawing.Point(0, 9);
            this.tableAuraManagement.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableAuraManagement.Name = "tableAuraManagement";
            this.tableAuraManagement.RowCount = 2;
            this.tableAuraManagement.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAuraManagement.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAuraManagement.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableAuraManagement.Size = new System.Drawing.Size(856, 60);
            this.tableAuraManagement.TabIndex = 1;
            // 
            // lblEnableHwAccel
            // 
            this.lblEnableHwAccel.AutoEllipsis = true;
            this.lblEnableHwAccel.AutoSize = true;
            this.lblEnableHwAccel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEnableHwAccel.Location = new System.Drawing.Point(4, 31);
            this.lblEnableHwAccel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEnableHwAccel.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblEnableHwAccel.Name = "lblEnableHwAccel";
            this.lblEnableHwAccel.Size = new System.Drawing.Size(343, 29);
            this.lblEnableHwAccel.TabIndex = 11;
            this.lblEnableHwAccel.Text = "Enable overlay hardware acceleration";
            this.lblEnableHwAccel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMonitorWindow
            // 
            this.txtMonitorWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMonitorWindow.Location = new System.Drawing.Point(355, 3);
            this.txtMonitorWindow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtMonitorWindow.Name = "txtMonitorWindow";
            this.txtMonitorWindow.Size = new System.Drawing.Size(497, 25);
            this.txtMonitorWindow.TabIndex = 8;
            // 
            // lblMonitorWindow
            // 
            this.lblMonitorWindow.AutoEllipsis = true;
            this.lblMonitorWindow.AutoSize = true;
            this.lblMonitorWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMonitorWindow.Location = new System.Drawing.Point(4, 0);
            this.lblMonitorWindow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMonitorWindow.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblMonitorWindow.Name = "lblMonitorWindow";
            this.lblMonitorWindow.Size = new System.Drawing.Size(343, 31);
            this.lblMonitorWindow.TabIndex = 9;
            this.lblMonitorWindow.Text = "Monitor window for showing/hiding overlays";
            this.lblMonitorWindow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxEnableHwAccel
            // 
            this.cbxEnableHwAccel.AutoSize = true;
            this.cbxEnableHwAccel.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxEnableHwAccel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxEnableHwAccel.Location = new System.Drawing.Point(355, 34);
            this.cbxEnableHwAccel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxEnableHwAccel.Name = "cbxEnableHwAccel";
            this.cbxEnableHwAccel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.cbxEnableHwAccel.Size = new System.Drawing.Size(497, 23);
            this.cbxEnableHwAccel.TabIndex = 10;
            this.cbxEnableHwAccel.TabStop = false;
            this.cbxEnableHwAccel.UseVisualStyleBackColor = true;
            // 
            // capAuraManagement
            // 
            this.capAuraManagement.Caption = "Overlay management";
            this.capAuraManagement.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capAuraManagement.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capAuraManagement.Collapsed = false;
            this.capAuraManagement.Collapsee = null;
            this.capAuraManagement.Collapsible = false;
            this.capAuraManagement.Dock = System.Windows.Forms.DockStyle.Top;
            this.capAuraManagement.Location = new System.Drawing.Point(0, 199);
            this.capAuraManagement.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capAuraManagement.Name = "capAuraManagement";
            this.capAuraManagement.Size = new System.Drawing.Size(856, 35);
            this.capAuraManagement.TabIndex = 26;
            // 
            // pnlAutosave
            // 
            this.pnlAutosave.AutoSize = true;
            this.pnlAutosave.Controls.Add(this.tableAutosave);
            this.pnlAutosave.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAutosave.Location = new System.Drawing.Point(0, 121);
            this.pnlAutosave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlAutosave.Name = "pnlAutosave";
            this.pnlAutosave.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlAutosave.Size = new System.Drawing.Size(856, 78);
            this.pnlAutosave.TabIndex = 25;
            // 
            // tableAutosave
            // 
            this.tableAutosave.AutoSize = true;
            this.tableAutosave.ColumnCount = 2;
            this.tableAutosave.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAutosave.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAutosave.Controls.Add(this.lblAutosaveConfig, 0, 0);
            this.tableAutosave.Controls.Add(this.lblAutosaveInterval, 0, 1);
            this.tableAutosave.Controls.Add(this.cbxAutosaveConfig, 1, 0);
            this.tableAutosave.Controls.Add(this.nudAutosaveMinutes, 1, 1);
            this.tableAutosave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableAutosave.Location = new System.Drawing.Point(0, 9);
            this.tableAutosave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableAutosave.Name = "tableAutosave";
            this.tableAutosave.RowCount = 2;
            this.tableAutosave.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutosave.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutosave.Size = new System.Drawing.Size(856, 60);
            this.tableAutosave.TabIndex = 1;
            // 
            // lblAutosaveConfig
            // 
            this.lblAutosaveConfig.AutoEllipsis = true;
            this.lblAutosaveConfig.AutoSize = true;
            this.lblAutosaveConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAutosaveConfig.Location = new System.Drawing.Point(4, 0);
            this.lblAutosaveConfig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAutosaveConfig.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblAutosaveConfig.Name = "lblAutosaveConfig";
            this.lblAutosaveConfig.Size = new System.Drawing.Size(287, 29);
            this.lblAutosaveConfig.TabIndex = 8;
            this.lblAutosaveConfig.Text = "Enable configuration auto-save";
            this.lblAutosaveConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAutosaveInterval
            // 
            this.lblAutosaveInterval.AutoEllipsis = true;
            this.lblAutosaveInterval.AutoSize = true;
            this.lblAutosaveInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAutosaveInterval.Location = new System.Drawing.Point(4, 29);
            this.lblAutosaveInterval.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAutosaveInterval.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblAutosaveInterval.Name = "lblAutosaveInterval";
            this.lblAutosaveInterval.Size = new System.Drawing.Size(287, 31);
            this.lblAutosaveInterval.TabIndex = 2;
            this.lblAutosaveInterval.Text = "Autosaving time interval in minutes";
            this.lblAutosaveInterval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxAutosaveConfig
            // 
            this.cbxAutosaveConfig.AutoSize = true;
            this.cbxAutosaveConfig.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAutosaveConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxAutosaveConfig.Location = new System.Drawing.Point(299, 3);
            this.cbxAutosaveConfig.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxAutosaveConfig.Name = "cbxAutosaveConfig";
            this.cbxAutosaveConfig.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.cbxAutosaveConfig.Size = new System.Drawing.Size(553, 23);
            this.cbxAutosaveConfig.TabIndex = 3;
            this.cbxAutosaveConfig.TabStop = false;
            this.cbxAutosaveConfig.UseVisualStyleBackColor = true;
            this.cbxAutosaveConfig.CheckedChanged += new System.EventHandler(this.cbxAutosaveConfig_CheckedChanged);
            // 
            // nudAutosaveMinutes
            // 
            this.nudAutosaveMinutes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudAutosaveMinutes.Location = new System.Drawing.Point(299, 32);
            this.nudAutosaveMinutes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
            this.nudAutosaveMinutes.Size = new System.Drawing.Size(553, 25);
            this.nudAutosaveMinutes.TabIndex = 7;
            this.nudAutosaveMinutes.TabStop = false;
            this.nudAutosaveMinutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudAutosaveMinutes.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // capAutosaving
            // 
            this.capAutosaving.Caption = "Autosaving";
            this.capAutosaving.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capAutosaving.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capAutosaving.Collapsed = false;
            this.capAutosaving.Collapsee = null;
            this.capAutosaving.Collapsible = false;
            this.capAutosaving.Dock = System.Windows.Forms.DockStyle.Top;
            this.capAutosaving.Location = new System.Drawing.Point(0, 86);
            this.capAutosaving.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capAutosaving.Name = "capAutosaving";
            this.capAutosaving.Size = new System.Drawing.Size(856, 35);
            this.capAutosaving.TabIndex = 24;
            // 
            // pnlTriggerTemplate
            // 
            this.pnlTriggerTemplate.AutoSize = true;
            this.pnlTriggerTemplate.Controls.Add(this.tableTriggerTemplate);
            this.pnlTriggerTemplate.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTriggerTemplate.Location = new System.Drawing.Point(0, 35);
            this.pnlTriggerTemplate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlTriggerTemplate.Name = "pnlTriggerTemplate";
            this.pnlTriggerTemplate.Padding = new System.Windows.Forms.Padding(0, 9, 0, 9);
            this.pnlTriggerTemplate.Size = new System.Drawing.Size(856, 51);
            this.pnlTriggerTemplate.TabIndex = 23;
            // 
            // tableTriggerTemplate
            // 
            this.tableTriggerTemplate.AutoSize = true;
            this.tableTriggerTemplate.ColumnCount = 3;
            this.tableTriggerTemplate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableTriggerTemplate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableTriggerTemplate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 267F));
            this.tableTriggerTemplate.Controls.Add(this.lblTriggerTemplate, 0, 0);
            this.tableTriggerTemplate.Controls.Add(this.cbxTriggerTemplate, 1, 0);
            this.tableTriggerTemplate.Controls.Add(this.btnTriggerTemplate, 2, 0);
            this.tableTriggerTemplate.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableTriggerTemplate.Location = new System.Drawing.Point(0, 9);
            this.tableTriggerTemplate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableTriggerTemplate.Name = "tableTriggerTemplate";
            this.tableTriggerTemplate.RowCount = 2;
            this.tableTriggerTemplate.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTriggerTemplate.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTriggerTemplate.Size = new System.Drawing.Size(856, 33);
            this.tableTriggerTemplate.TabIndex = 1;
            // 
            // lblTriggerTemplate
            // 
            this.lblTriggerTemplate.AutoEllipsis = true;
            this.lblTriggerTemplate.AutoSize = true;
            this.lblTriggerTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTriggerTemplate.Location = new System.Drawing.Point(4, 0);
            this.lblTriggerTemplate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTriggerTemplate.MinimumSize = new System.Drawing.Size(200, 0);
            this.lblTriggerTemplate.Name = "lblTriggerTemplate";
            this.lblTriggerTemplate.Size = new System.Drawing.Size(319, 33);
            this.lblTriggerTemplate.TabIndex = 9;
            this.lblTriggerTemplate.Text = "Use template trigger for default values";
            this.lblTriggerTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxTriggerTemplate
            // 
            this.cbxTriggerTemplate.AutoSize = true;
            this.cbxTriggerTemplate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxTriggerTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxTriggerTemplate.Location = new System.Drawing.Point(331, 3);
            this.cbxTriggerTemplate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbxTriggerTemplate.Name = "cbxTriggerTemplate";
            this.cbxTriggerTemplate.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.cbxTriggerTemplate.Size = new System.Drawing.Size(254, 27);
            this.cbxTriggerTemplate.TabIndex = 0;
            this.cbxTriggerTemplate.TabStop = false;
            this.cbxTriggerTemplate.UseVisualStyleBackColor = true;
            // 
            // btnTriggerTemplate
            // 
            this.btnTriggerTemplate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTriggerTemplate.Location = new System.Drawing.Point(593, 3);
            this.btnTriggerTemplate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnTriggerTemplate.Name = "btnTriggerTemplate";
            this.btnTriggerTemplate.Size = new System.Drawing.Size(259, 27);
            this.btnTriggerTemplate.TabIndex = 1;
            this.btnTriggerTemplate.TabStop = false;
            this.btnTriggerTemplate.Text = "Edit template trigger";
            this.btnTriggerTemplate.UseVisualStyleBackColor = true;
            this.btnTriggerTemplate.Click += new System.EventHandler(this.btnTriggerTemplate_Click);
            // 
            // capTemplate
            // 
            this.capTemplate.Caption = "Templates";
            this.capTemplate.CaptionHighlightColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.capTemplate.CaptionShadowColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.capTemplate.Collapsed = false;
            this.capTemplate.Collapsee = null;
            this.capTemplate.Collapsible = false;
            this.capTemplate.Dock = System.Windows.Forms.DockStyle.Top;
            this.capTemplate.Location = new System.Drawing.Point(0, 0);
            this.capTemplate.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.capTemplate.Name = "capTemplate";
            this.capTemplate.Size = new System.Drawing.Size(856, 35);
            this.capTemplate.TabIndex = 22;
            // 
            // btnSecurity
            // 
            this.btnSecurity.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSecurity.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnSecurity.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSecurity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSecurity.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSecurity.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnSecurity.IconCharacter = "d";
            this.btnSecurity.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnSecurity.Location = new System.Drawing.Point(0, 506);
            this.btnSecurity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSecurity.Name = "btnSecurity";
            this.btnSecurity.Size = new System.Drawing.Size(293, 46);
            this.btnSecurity.TabControl = this.tbcMain;
            this.btnSecurity.TabIndex = 13;
            this.btnSecurity.TabNumber = 11;
            this.btnSecurity.Text = "Security";
            this.btnSecurity.UseVisualStyleBackColor = false;
            // 
            // btnGameSpecific
            // 
            this.btnGameSpecific.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnGameSpecific.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnGameSpecific.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGameSpecific.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGameSpecific.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGameSpecific.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnGameSpecific.IconCharacter = "Ä";
            this.btnGameSpecific.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnGameSpecific.Location = new System.Drawing.Point(0, 460);
            this.btnGameSpecific.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnGameSpecific.Name = "btnGameSpecific";
            this.btnGameSpecific.Size = new System.Drawing.Size(293, 46);
            this.btnGameSpecific.TabControl = this.tbcMain;
            this.btnGameSpecific.TabIndex = 12;
            this.btnGameSpecific.TabNumber = 10;
            this.btnGameSpecific.Text = "Game-specific";
            this.btnGameSpecific.UseVisualStyleBackColor = false;
            // 
            // btnSubstitutions
            // 
            this.btnSubstitutions.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSubstitutions.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnSubstitutions.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSubstitutions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubstitutions.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubstitutions.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnSubstitutions.IconCharacter = ">";
            this.btnSubstitutions.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnSubstitutions.Location = new System.Drawing.Point(0, 414);
            this.btnSubstitutions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSubstitutions.Name = "btnSubstitutions";
            this.btnSubstitutions.Size = new System.Drawing.Size(293, 46);
            this.btnSubstitutions.TabControl = this.tbcMain;
            this.btnSubstitutions.TabIndex = 15;
            this.btnSubstitutions.TabNumber = 9;
            this.btnSubstitutions.Text = "Substitutions";
            this.btnSubstitutions.UseVisualStyleBackColor = false;
            // 
            // btnConstants
            // 
            this.btnConstants.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnConstants.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnConstants.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnConstants.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConstants.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConstants.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnConstants.IconCharacter = "Ï";
            this.btnConstants.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnConstants.Location = new System.Drawing.Point(0, 368);
            this.btnConstants.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnConstants.Name = "btnConstants";
            this.btnConstants.Size = new System.Drawing.Size(293, 46);
            this.btnConstants.TabControl = this.tbcMain;
            this.btnConstants.TabIndex = 11;
            this.btnConstants.TabNumber = 8;
            this.btnConstants.Text = "Constants";
            this.btnConstants.UseVisualStyleBackColor = false;
            // 
            // btnEndpoint
            // 
            this.btnEndpoint.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnEndpoint.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnEndpoint.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEndpoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEndpoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEndpoint.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnEndpoint.IconCharacter = "Â";
            this.btnEndpoint.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnEndpoint.Location = new System.Drawing.Point(0, 322);
            this.btnEndpoint.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnEndpoint.Name = "btnEndpoint";
            this.btnEndpoint.Size = new System.Drawing.Size(293, 46);
            this.btnEndpoint.TabControl = this.tbcMain;
            this.btnEndpoint.TabIndex = 10;
            this.btnEndpoint.TabNumber = 7;
            this.btnEndpoint.Text = "Endpoint";
            this.btnEndpoint.UseVisualStyleBackColor = false;
            // 
            // btnUpdates
            // 
            this.btnUpdates.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnUpdates.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnUpdates.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUpdates.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdates.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdates.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnUpdates.IconCharacter = "q";
            this.btnUpdates.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnUpdates.Location = new System.Drawing.Point(0, 276);
            this.btnUpdates.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUpdates.Name = "btnUpdates";
            this.btnUpdates.Size = new System.Drawing.Size(293, 46);
            this.btnUpdates.TabControl = this.tbcMain;
            this.btnUpdates.TabIndex = 9;
            this.btnUpdates.TabNumber = 6;
            this.btnUpdates.Text = "Updates";
            this.btnUpdates.UseVisualStyleBackColor = false;
            // 
            // btnCaching
            // 
            this.btnCaching.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnCaching.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnCaching.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCaching.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCaching.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCaching.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnCaching.IconCharacter = "Í";
            this.btnCaching.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnCaching.Location = new System.Drawing.Point(0, 230);
            this.btnCaching.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCaching.Name = "btnCaching";
            this.btnCaching.Size = new System.Drawing.Size(293, 46);
            this.btnCaching.TabControl = this.tbcMain;
            this.btnCaching.TabIndex = 8;
            this.btnCaching.TabNumber = 5;
            this.btnCaching.Text = "Caching";
            this.btnCaching.UseVisualStyleBackColor = false;
            // 
            // btnShortcuts
            // 
            this.btnShortcuts.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnShortcuts.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnShortcuts.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnShortcuts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShortcuts.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShortcuts.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnShortcuts.IconCharacter = "i";
            this.btnShortcuts.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnShortcuts.Location = new System.Drawing.Point(0, 184);
            this.btnShortcuts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnShortcuts.Name = "btnShortcuts";
            this.btnShortcuts.Size = new System.Drawing.Size(293, 46);
            this.btnShortcuts.TabControl = this.tbcMain;
            this.btnShortcuts.TabIndex = 7;
            this.btnShortcuts.TabNumber = 4;
            this.btnShortcuts.Text = "Shortcuts";
            this.btnShortcuts.UseVisualStyleBackColor = false;
            // 
            // btnLogging
            // 
            this.btnLogging.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnLogging.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnLogging.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLogging.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogging.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogging.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnLogging.IconCharacter = "¥";
            this.btnLogging.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnLogging.Location = new System.Drawing.Point(0, 138);
            this.btnLogging.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLogging.Name = "btnLogging";
            this.btnLogging.Size = new System.Drawing.Size(293, 46);
            this.btnLogging.TabControl = this.tbcMain;
            this.btnLogging.TabIndex = 6;
            this.btnLogging.TabNumber = 3;
            this.btnLogging.Text = "Logging";
            this.btnLogging.UseVisualStyleBackColor = false;
            // 
            // btnAudio
            // 
            this.btnAudio.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnAudio.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnAudio.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAudio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAudio.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAudio.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnAudio.IconCharacter = "X";
            this.btnAudio.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnAudio.Location = new System.Drawing.Point(0, 92);
            this.btnAudio.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAudio.Name = "btnAudio";
            this.btnAudio.Size = new System.Drawing.Size(293, 46);
            this.btnAudio.TabControl = this.tbcMain;
            this.btnAudio.TabIndex = 5;
            this.btnAudio.TabNumber = 2;
            this.btnAudio.Text = "Audio";
            this.btnAudio.UseVisualStyleBackColor = false;
            // 
            // btnUserInterface
            // 
            this.btnUserInterface.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnUserInterface.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnUserInterface.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUserInterface.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserInterface.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUserInterface.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnUserInterface.IconCharacter = "2";
            this.btnUserInterface.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnUserInterface.Location = new System.Drawing.Point(0, 46);
            this.btnUserInterface.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUserInterface.Name = "btnUserInterface";
            this.btnUserInterface.Size = new System.Drawing.Size(293, 46);
            this.btnUserInterface.TabControl = this.tbcMain;
            this.btnUserInterface.TabIndex = 16;
            this.btnUserInterface.TabNumber = 1;
            this.btnUserInterface.Text = "User interface";
            this.btnUserInterface.UseVisualStyleBackColor = false;
            // 
            // btnStartup
            // 
            this.btnStartup.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnStartup.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnStartup.Checked = true;
            this.btnStartup.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStartup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartup.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartup.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnStartup.IconCharacter = "~";
            this.btnStartup.IconFont = new System.Drawing.Font("Webdings", 18F);
            this.btnStartup.Location = new System.Drawing.Point(0, 0);
            this.btnStartup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnStartup.Name = "btnStartup";
            this.btnStartup.Size = new System.Drawing.Size(293, 46);
            this.btnStartup.TabControl = this.tbcMain;
            this.btnStartup.TabIndex = 4;
            this.btnStartup.TabNumber = 0;
            this.btnStartup.TabStop = true;
            this.btnStartup.Text = "Startup";
            this.btnStartup.UseVisualStyleBackColor = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.Window;
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(293, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(886, 58);
            this.pnlHeader.TabIndex = 1;
            // 
            // lblHeader
            // 
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.lblHeader.Size = new System.Drawing.Size(886, 58);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Configuration";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlContent
            // 
            this.pnlContent.AutoScroll = true;
            this.pnlContent.Controls.Add(this.tbcMain);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(293, 58);
            this.pnlContent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.pnlContent.Size = new System.Drawing.Size(886, 647);
            this.pnlContent.TabIndex = 3;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnOk);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(293, 705);
            this.pnlButtons.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(11, 0, 11, 9);
            this.pnlButtons.Size = new System.Drawing.Size(886, 58);
            this.pnlButtons.TabIndex = 4;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(555, 0);
            this.btnOk.Margin = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(160, 49);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(715, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 49);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // toolTipSettings
            // 
            this.toolTipSettings.AutoPopDelay = 60000;
            this.toolTipSettings.InitialDelay = 500;
            this.toolTipSettings.ReshowDelay = 500;
            this.toolTipSettings.ShowAlways = true;
            // 
            // dlgUiFont
            // 
            this.dlgUiFont.AllowScriptChange = false;
            // 
            // ConfigurationForm2
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1179, 763);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(794, 454);
            this.Name = "ConfigurationForm2";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            this.Shown += new System.EventHandler(this.ConfigurationForm2_Shown);
            this.pnlMenu.ResumeLayout(false);
            this.tbcMain.ResumeLayout(false);
            this.tabStartup.ResumeLayout(false);
            this.tabStartup.PerformLayout();
            this.pnlStartupTrigger.ResumeLayout(false);
            this.pnlStartupTrigger.PerformLayout();
            this.ctxStartupTrigger.ResumeLayout(false);
            this.tlsStartupTrigger.ResumeLayout(false);
            this.tlsStartupTrigger.PerformLayout();
            this.pnlStartup.ResumeLayout(false);
            this.pnlStartup.PerformLayout();
            this.tableStartup.ResumeLayout(false);
            this.tableStartup.PerformLayout();
            this.tabUserInterface.ResumeLayout(false);
            this.tabUserInterface.PerformLayout();
            this.pnlUiOptions.ResumeLayout(false);
            this.pnlUiOptions.PerformLayout();
            this.tableUiOptions.ResumeLayout(false);
            this.tableUiOptions.PerformLayout();
            this.pnlUiFont.ResumeLayout(false);
            this.pnlUiFont.PerformLayout();
            this.tableUiFont.ResumeLayout(false);
            this.tableUiFont.PerformLayout();
            this.tabAudio.ResumeLayout(false);
            this.tabAudio.PerformLayout();
            this.panelAudioTts.ResumeLayout(false);
            this.panelAudioTts.PerformLayout();
            this.tableAudioTts.ResumeLayout(false);
            this.tableAudioTts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbTtsRepetition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbTtsVolume)).EndInit();
            this.pnlTtsWarnings.ResumeLayout(false);
            this.panelAudioSound.ResumeLayout(false);
            this.panelAudioSound.PerformLayout();
            this.tableAudioSound.ResumeLayout(false);
            this.tableAudioSound.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbSoundRepetition)).EndInit();
            this.pnlExternalSoundWarning.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trbSoundVolume)).EndInit();
            this.panelAudioBeep.ResumeLayout(false);
            this.panelAudioBeep.PerformLayout();
            this.tableAudioBeep.ResumeLayout(false);
            this.tableAudioBeep.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbBeepVolume)).EndInit();
            this.tabLogging.ResumeLayout(false);
            this.tabLogging.PerformLayout();
            this.pnlLoggingGeneral.ResumeLayout(false);
            this.pnlLoggingGeneral.PerformLayout();
            this.tableLoggingGeneral.ResumeLayout(false);
            this.tableLoggingGeneral.PerformLayout();
            this.panelLogLimits.ResumeLayout(false);
            this.panelLogLimits.PerformLayout();
            this.tableLogLimits.ResumeLayout(false);
            this.tableLogLimits.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitCustom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitCustom2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogLimitVerbose)).EndInit();
            this.tabShortcuts.ResumeLayout(false);
            this.tabShortcuts.PerformLayout();
            this.pnlShortcutTemplates.ResumeLayout(false);
            this.pnlShortcutTemplates.PerformLayout();
            this.tableLayoutPanelShortCutExpression.ResumeLayout(false);
            this.tableLayoutPanelShortCutExpression.PerformLayout();
            this.tabCaching.ResumeLayout(false);
            this.tabCaching.PerformLayout();
            this.pnlCacheFile.ResumeLayout(false);
            this.pnlCacheFile.PerformLayout();
            this.tableLayoutPanelDownloads.ResumeLayout(false);
            this.tableLayoutPanelDownloads.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheFileExpiry)).EndInit();
            this.pnlCacheRepo.ResumeLayout(false);
            this.pnlCacheRepo.PerformLayout();
            this.tableLayoutPanelRepo.ResumeLayout(false);
            this.tableLayoutPanelRepo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheRepoExpiry)).EndInit();
            this.pnlCacheJson.ResumeLayout(false);
            this.pnlCacheJson.PerformLayout();
            this.tableLayoutPanelJason.ResumeLayout(false);
            this.tableLayoutPanelJason.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheJsonExpiry)).EndInit();
            this.pnlCacheSound.ResumeLayout(false);
            this.pnlCacheSound.PerformLayout();
            this.tableLayoutPanelSound.ResumeLayout(false);
            this.tableLayoutPanelSound.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheSoundExpiry)).EndInit();
            this.pnlCacheImage.ResumeLayout(false);
            this.pnlCacheImage.PerformLayout();
            this.tableLayoutPanelImages.ResumeLayout(false);
            this.tableLayoutPanelImages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCacheImageExpiry)).EndInit();
            this.tabUpdates.ResumeLayout(false);
            this.tabUpdates.PerformLayout();
            this.pnlUpdateChannel.ResumeLayout(false);
            this.tableUpdateChannel.ResumeLayout(false);
            this.tableUpdateChannel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudUpdateInterval)).EndInit();
            this.tabEndpoint.ResumeLayout(false);
            this.tabEndpoint.PerformLayout();
            this.pnlEndpointHistory.ResumeLayout(false);
            this.pnlEndpointHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEndpointHistory)).EndInit();
            this.tlsEndpointHistory.ResumeLayout(false);
            this.tlsEndpointHistory.PerformLayout();
            this.pnlEndpointSettings.ResumeLayout(false);
            this.pnlEndpointSettings.PerformLayout();
            this.tableEndpointSettings.ResumeLayout(false);
            this.tableEndpointSettings.PerformLayout();
            this.pnlEndpointStatus.ResumeLayout(false);
            this.pnlEndpointStatus.PerformLayout();
            this.tableEndpointStatus.ResumeLayout(false);
            this.tableEndpointStatus.PerformLayout();
            this.tabConstants.ResumeLayout(false);
            this.tabConstants.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConstVariables)).EndInit();
            this.tlsConstant.ResumeLayout(false);
            this.tlsConstant.PerformLayout();
            this.tabSubs.ResumeLayout(false);
            this.tabSubs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubstitutions)).EndInit();
            this.tlsSubstitutions.ResumeLayout(false);
            this.tlsSubstitutions.PerformLayout();
            this.tabGameSpecific.ResumeLayout(false);
            this.tbcGames.ResumeLayout(false);
            this.tabFFXIV.ResumeLayout(false);
            this.tabFFXIV.PerformLayout();
            this.pnlFfxivLogging.ResumeLayout(false);
            this.pnlFfxivLogging.PerformLayout();
            this.tableFfxivLogging.ResumeLayout(false);
            this.tableFfxivLogging.PerformLayout();
            this.tabSecurity.ResumeLayout(false);
            this.pnlScriptingAccess.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdditionalFeatures)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApiAccess)).EndInit();
            this.tabMiscellaneous.ResumeLayout(false);
            this.tabMiscellaneous.PerformLayout();
            this.pnlAuraManagement.ResumeLayout(false);
            this.pnlAuraManagement.PerformLayout();
            this.tableAuraManagement.ResumeLayout(false);
            this.tableAuraManagement.PerformLayout();
            this.pnlAutosave.ResumeLayout(false);
            this.pnlAutosave.PerformLayout();
            this.tableAutosave.ResumeLayout(false);
            this.tableAutosave.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAutosaveMinutes)).EndInit();
            this.pnlTriggerTemplate.ResumeLayout(false);
            this.pnlTriggerTemplate.PerformLayout();
            this.tableTriggerTemplate.ResumeLayout(false);
            this.tableTriggerTemplate.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tabStartup;
        private System.Windows.Forms.TabPage tabAudio;
        private System.Windows.Forms.Panel pnlContent;
        private CustomControls.MenuButton btnLogging;
        private CustomControls.MenuButton btnAudio;
        private CustomControls.MenuButton btnStartup;
        private CustomControls.MenuButton btnConstants;
        private CustomControls.MenuButton btnEndpoint;
        private CustomControls.MenuButton btnUpdates;
        private CustomControls.MenuButton btnCaching;
        private CustomControls.MenuButton btnShortcuts;
        private CustomControls.MenuButton btnMisc;
        private CustomControls.MenuButton btnSecurity;
        private CustomControls.MenuButton btnGameSpecific;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabPage tabLogging;
        private System.Windows.Forms.TabPage tabShortcuts;
        private System.Windows.Forms.TabPage tabCaching;
        private System.Windows.Forms.TabPage tabUpdates;
        private System.Windows.Forms.TabPage tabEndpoint;
        private System.Windows.Forms.TabPage tabConstants;
        private System.Windows.Forms.TabPage tabGameSpecific;
        private System.Windows.Forms.TabPage tabSecurity;
        private System.Windows.Forms.TabPage tabMiscellaneous;
        private System.Windows.Forms.TabPage tabSubs;
        private CustomControls.MenuButton btnSubstitutions;
        private CustomControls.DataGridViewEx dgvConstVariables;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScalarName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScalarValue;
        private System.Windows.Forms.ToolStrip tlsConstant;
        private System.Windows.Forms.ToolStripButton btnConstAdd;
        private System.Windows.Forms.ToolStripButton btnConstEdit;
        private System.Windows.Forms.ToolStripButton btnConstRemove;
        private CustomControls.PrettyCaption capStartupTrigger;
        private System.Windows.Forms.Panel pnlStartup;
        private System.Windows.Forms.TableLayoutPanel tableStartup;
        private System.Windows.Forms.CheckBox chkWarnAdmin;
        private System.Windows.Forms.CheckBox chkWelcome;
        private CustomControls.PrettyCaption capStartupOptions;
        private System.Windows.Forms.Panel pnlStartupTrigger;
        internal System.Windows.Forms.TreeView trvTrigger;
        private System.Windows.Forms.ToolStrip tlsStartupTrigger;
        private System.Windows.Forms.ToolStripButton btnClearSelection;
        private System.Windows.Forms.ToolStripLabel lblFolderReminder;
        private System.Windows.Forms.Panel panelAudioSound;
        private System.Windows.Forms.TableLayoutPanel tableAudioSound;
        private CustomControls.PrettyCaption capAudioSound;
        private System.Windows.Forms.Panel panelAudioBeep;
        private System.Windows.Forms.TableLayoutPanel tableAudioBeep;
        private System.Windows.Forms.Button btnBeepTest;
        private System.Windows.Forms.Label lblBeepVolP;
        private System.Windows.Forms.TrackBar trbBeepVolume;
        private System.Windows.Forms.Label lblBeepVolume;
        private System.Windows.Forms.CheckBox chkUseSimulatedBeep;
        private System.Windows.Forms.Label lblUseSimulatedBeep;
        private CustomControls.PrettyCaption capAudioBeep;
        private System.Windows.Forms.Panel pnlShortcutTemplates;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelShortCutExpression;
        private System.Windows.Forms.CheckBox chkShortcutEnableTemplates;
        private System.Windows.Forms.CheckBox chkShortcutUseAbbrevInTemplates;
        private System.Windows.Forms.CheckBox chkShortcutWrapTextWhenSelected;
        private CustomControls.PrettyCaption capExpressions;
        private CustomControls.PrettyCaption capImageFiles;
        private System.Windows.Forms.Panel pnlCacheImage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelImages;
        private System.Windows.Forms.Button btnCacheImageBrowse;
        private System.Windows.Forms.TextBox txtCacheImageSize;
        private System.Windows.Forms.Label lblCacheImageSize;
        private System.Windows.Forms.Label lblCacheImageCount;
        private System.Windows.Forms.Label lblCacheImageExpiry;
        private System.Windows.Forms.NumericUpDown nudCacheImageExpiry;
        private System.Windows.Forms.TextBox txtCacheImageCount;
        private System.Windows.Forms.Button btnCacheImageClear;
        private CustomControls.PrettyCaption capSoundFiles;
        private System.Windows.Forms.Panel pnlCacheSound;
        private CustomControls.PrettyCaption capJason;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSound;
        private System.Windows.Forms.Button btnCacheSoundBrowse;
        private System.Windows.Forms.Button btnCacheSoundClear;
        private System.Windows.Forms.TextBox txtCacheSoundSize;
        private System.Windows.Forms.Label lblCacheSoundSize;
        private System.Windows.Forms.Label lblCacheSoundCount;
        private System.Windows.Forms.Label lblCacheSoundExpiry;
        private System.Windows.Forms.NumericUpDown nudCacheSoundExpiry;
        private System.Windows.Forms.TextBox txtCacheSoundCount;
        private System.Windows.Forms.Panel pnlCacheRepo;
        private CustomControls.PrettyCaption capRepoBackups;
        private System.Windows.Forms.Panel pnlCacheJson;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelJason;
        private System.Windows.Forms.NumericUpDown nudCacheJsonExpiry;
        private System.Windows.Forms.Button btnCacheJsonBrowse;
        private System.Windows.Forms.Button btnCacheJsonClear;
        private System.Windows.Forms.TextBox txtCacheJsonSize;
        private System.Windows.Forms.Label lblCacheJsonSize;
        private System.Windows.Forms.Label lblCacheJsonCount;
        private System.Windows.Forms.Label lblCacheJsonExpiry;
        private System.Windows.Forms.TextBox txtCacheJsonCount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRepo;
        private System.Windows.Forms.Button btnCacheRepoBrowse;
        private System.Windows.Forms.Button btnCacheRepoClear;
        private System.Windows.Forms.TextBox txtCacheRepoSize;
        private System.Windows.Forms.Label lblCacheRepoSize;
        private System.Windows.Forms.Label lblCacheRepoCount;
        private System.Windows.Forms.Label lblCacheRepoExpiry;
        private System.Windows.Forms.NumericUpDown nudCacheRepoExpiry;
        private System.Windows.Forms.TextBox txtCacheRepoCount;
        private System.Windows.Forms.Panel pnlCacheFile;
        private CustomControls.PrettyCaption capDownloads;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDownloads;
        private System.Windows.Forms.Button btnCacheFileBrowse;
        private System.Windows.Forms.Button btnCacheFileClear;
        private System.Windows.Forms.TextBox txtCacheFileSize;
        private System.Windows.Forms.Label lblCacheFileSize;
        private System.Windows.Forms.Label lblCacheFileCount;
        private System.Windows.Forms.Label lblCacheFileExpiry;
        private System.Windows.Forms.NumericUpDown nudCacheFileExpiry;
        private System.Windows.Forms.TextBox txtCacheFileCount;
        private System.Windows.Forms.Panel pnlScriptingAccess;
        private CustomControls.DataGridViewEx dgvAdditionalFeatures;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFeatureName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFeatureAllowLocal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFeatureAllowRemote;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colFeatureAllowAdmin;
        private System.Windows.Forms.Panel pnlSecurityFeaturesSpacing;
        private CustomControls.DataGridViewEx dgvApiAccess;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApiNamespace;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colApiAllowLocal;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colApiAllowRemote;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colApiAllowAdmin;
        private System.Windows.Forms.Panel pnlSecurityUnlockSpacing;
        private System.Windows.Forms.Button btnUnlockSecurity;
        private CustomControls.PrettyCaption capScripting;
        private System.Windows.Forms.Panel panelAudioTts;
        private System.Windows.Forms.TableLayoutPanel tableAudioTts;
        private System.Windows.Forms.Label lblTtsVolP;
        private System.Windows.Forms.TrackBar trbTtsVolume;
        private System.Windows.Forms.Label lblTtsVolume;
        private CustomControls.PrettyCaption capAudioTts;
        private System.Windows.Forms.Label lblSoundVolP;
        private System.Windows.Forms.TrackBar trbSoundVolume;
        private System.Windows.Forms.Label lblSoundVolume;
        private System.Windows.Forms.Label lblTtsMethod;
        private System.Windows.Forms.Label lblSoundPath;
        private System.Windows.Forms.Label lblSoundMethod;
        private System.Windows.Forms.ComboBox cbxSoundMethod;
        private System.Windows.Forms.Button btnSoundPath;
        private System.Windows.Forms.TextBox txtSoundPath;
        private System.Windows.Forms.Button btnTtsPath;
        private System.Windows.Forms.TextBox txtTtsPath;
        private System.Windows.Forms.ComboBox cbxTtsMethod;
        private System.Windows.Forms.Label lblTtsPath;
        private System.Windows.Forms.Panel pnlUpdateChannel;
        private System.Windows.Forms.TableLayoutPanel tableUpdateChannel;
        private System.Windows.Forms.TextBox txtUpdateChannelUrl;
        private System.Windows.Forms.ComboBox cbxUpdateMethod;
        private System.Windows.Forms.Label lblUpdateChannelUrl;
        private System.Windows.Forms.Label lblUpdateMethod;
        private CustomControls.PrettyCaption capUpdateChannel;
        private System.Windows.Forms.Panel pnlLoggingGeneral;
        private System.Windows.Forms.TableLayoutPanel tableLoggingGeneral;
        private System.Windows.Forms.CheckBox chkLogNormalEvents;
        private System.Windows.Forms.Label lblLoggingLevel;
        private Triggernometry.UI.CustomControls.PrettyCaption capLogLimits;
        private System.Windows.Forms.Panel panelLogLimits;
        private System.Windows.Forms.TableLayoutPanel tableLogLimits;
        private System.Windows.Forms.Label lblLogLimitError;
        private System.Windows.Forms.NumericUpDown nudLogLimitError;
        private System.Windows.Forms.Label lblLogLimitWarning;
        private System.Windows.Forms.NumericUpDown nudLogLimitWarning;
        private System.Windows.Forms.Label lblLogLimitCustom;
        private System.Windows.Forms.NumericUpDown nudLogLimitCustom;
        private System.Windows.Forms.Label lblLogLimitCustom2;
        private System.Windows.Forms.NumericUpDown nudLogLimitCustom2;
        private System.Windows.Forms.Label lblLogLimitInfo;
        private System.Windows.Forms.NumericUpDown nudLogLimitInfo;
        private System.Windows.Forms.Label lblLogLimitVerbose;
        private System.Windows.Forms.NumericUpDown nudLogLimitVerbose;
        private System.Windows.Forms.ComboBox cbxLoggingLevel;
        private CustomControls.PrettyCaption capLogging;
        private System.Windows.Forms.TabControl tbcGames;
        private System.Windows.Forms.TabPage tabFFXIV;
        private CustomControls.PrettyCaption capFfxivLogging;
        private System.Windows.Forms.Panel pnlFfxivLogging;
        private System.Windows.Forms.TableLayoutPanel tableFfxivLogging;
        private System.Windows.Forms.CheckBox chkFfxivLogNetwork;
        private System.Windows.Forms.Panel pnlEndpointHistory;
        private CustomControls.DataGridViewEx dgvEndpointHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndpointHistoryTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndpointHistoryData;
        private System.Windows.Forms.ToolStrip tlsEndpointHistory;
        private System.Windows.Forms.ToolStripButton btnEndpointHistUpdate;
        private System.Windows.Forms.ToolStripLabel tlsEndpointHistoryRecv;
        private System.Windows.Forms.ToolStripLabel tlsEndpointHistoryCount;
        private CustomControls.PrettyCaption capEndpointHistory;
        private System.Windows.Forms.Panel pnlEndpointSettings;
        private CustomControls.PrettyCaption capEndpointSettings;
        private System.Windows.Forms.Panel pnlEndpointStatus;
        private System.Windows.Forms.TableLayoutPanel tableEndpointStatus;
        private System.Windows.Forms.Button btnEndpointStart;
        private System.Windows.Forms.Button btnEndpointStop;
        private System.Windows.Forms.TextBox txtEndpointStatus;
        private CustomControls.PrettyCaption capEndpointStatus;
        private System.Windows.Forms.TableLayoutPanel tableEndpointSettings;
        private System.Windows.Forms.CheckBox chkEndpointStartup;
        private System.Windows.Forms.CheckBox chkEndpointLog;
        private System.Windows.Forms.TextBox txtEndpoint;
        private System.Windows.Forms.Label lblEndpoint;
        private System.Windows.Forms.Panel pnlTriggerTemplate;
        private System.Windows.Forms.TableLayoutPanel tableTriggerTemplate;
        private System.Windows.Forms.CheckBox cbxTriggerTemplate;
        private System.Windows.Forms.Button btnTriggerTemplate;
        private CustomControls.PrettyCaption capTemplate;
        private System.Windows.Forms.Panel pnlAuraManagement;
        private System.Windows.Forms.TableLayoutPanel tableAuraManagement;
        private System.Windows.Forms.TextBox txtMonitorWindow;
        private System.Windows.Forms.Label lblMonitorWindow;
        private System.Windows.Forms.CheckBox cbxEnableHwAccel;
        private CustomControls.PrettyCaption capAuraManagement;
        private System.Windows.Forms.Panel pnlAutosave;
        private System.Windows.Forms.TableLayoutPanel tableAutosave;
        private System.Windows.Forms.Label lblAutosaveInterval;
        private System.Windows.Forms.CheckBox cbxAutosaveConfig;
        private System.Windows.Forms.NumericUpDown nudAutosaveMinutes;
        private CustomControls.PrettyCaption capAutosaving;
        private System.Windows.Forms.Panel pnlExternalSoundWarning;
        private System.Windows.Forms.Label lblExternalSoundWarn;
        private CustomControls.DataGridViewEx dgvSubstitutions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubSearchFor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubReplaceWith;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubScope;
        private System.Windows.Forms.ToolStrip tlsSubstitutions;
        private System.Windows.Forms.ToolStripButton btnSubAdd;
        private System.Windows.Forms.ToolStripButton btnSubEdit;
        private System.Windows.Forms.ToolStripButton btnSubRemove;
        private System.Windows.Forms.ToolTip toolTipSettings;
        private System.Windows.Forms.ContextMenuStrip ctxStartupTrigger;
        private System.Windows.Forms.ToolStripMenuItem ctxClearSelection;
        private System.Windows.Forms.Label lblExternalUpdateWarn;
        private System.Windows.Forms.TextBox txtTtsPathArgs;
        private System.Windows.Forms.Label lblTtsPathArgs;
        private System.Windows.Forms.TextBox txtSoundPathArgs;
        private System.Windows.Forms.Label lblSoundPathArgs;
        private CustomControls.MenuButton btnUserInterface;
        private System.Windows.Forms.TabPage tabUserInterface;
        private System.Windows.Forms.Panel pnlUiFont;
        private CustomControls.PrettyCaption capAppearance;
        private System.Windows.Forms.Panel pnlUiOptions;
        private System.Windows.Forms.TableLayoutPanel tableUiOptions;
        private CustomControls.PrettyCaption capBehavior;
        private System.Windows.Forms.TableLayoutPanel tableUiFont;
        private System.Windows.Forms.CheckBox cbxUiFontDefault;
        private System.Windows.Forms.Label lblUiFont;
        private System.Windows.Forms.TextBox txtUiFontDesc;
        private System.Windows.Forms.Button btnUiFont;
        private System.Windows.Forms.FontDialog dlgUiFont;
        private System.Windows.Forms.CheckBox cbxAutoComplete;
        private System.Windows.Forms.CheckBox cbxDevMode;
        private System.Windows.Forms.CheckBox cbxActionAsync;
        private System.Windows.Forms.CheckBox cbxTestIgnoreConditions;
        private System.Windows.Forms.Panel pnlTtsWarnings;
        private System.Windows.Forms.Label lblExternalTtsWarn;
        private System.Windows.Forms.Label lblActTtsWarn;
        private System.Windows.Forms.Label lblLogNormalEvents;
        private System.Windows.Forms.Label lblLogVariableExpansions;
        private System.Windows.Forms.CheckBox chkLogVariableExpansions;
        private System.Windows.Forms.Label lblUiFontDefault;
        private System.Windows.Forms.Label lblDevMode;
        private System.Windows.Forms.Label lblAutoComplete;
        private System.Windows.Forms.Label lblActionAsync;
        private System.Windows.Forms.Label lblTestIgnoreConditions;
        private System.Windows.Forms.Label lblWarnAdmin;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblShortcutWrapTextWhenSelected;
        private System.Windows.Forms.Label lblShortcutUseAbbrevInTemplates;
        private System.Windows.Forms.Label lblShortcutEnableTemplates;
        private System.Windows.Forms.Label lblEndpointStartup;
        private System.Windows.Forms.Label lblEndpointLog;
        private System.Windows.Forms.Label lblFfxivLogNetwork;
        private System.Windows.Forms.Label lblEnableHwAccel;
        private System.Windows.Forms.Label lblAutosaveConfig;
        private System.Windows.Forms.Label lblTriggerTemplate;
        private System.Windows.Forms.Button btnUpdateCheck;
        private System.Windows.Forms.Label lblSoundRepetition;
        private System.Windows.Forms.TrackBar trbTtsRepetition;
        private System.Windows.Forms.Label lblTtsRepetition;
        private System.Windows.Forms.Label lblSoundRepV;
        private System.Windows.Forms.TrackBar trbSoundRepetition;
        private System.Windows.Forms.Label lblTtsRepV;
        private System.Windows.Forms.NumericUpDown nudUpdateInterval;
        private System.Windows.Forms.Label lblUpdateInterval;
        private System.Windows.Forms.Label lblUpdates;
        private System.Windows.Forms.CheckBox chkUpdates;
    }
}