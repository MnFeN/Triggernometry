namespace Triggernometry.UI.Forms
{
    partial class TriggerForm
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
            this.pnlTopSpacing = new System.Windows.Forms.Panel();
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.tableTriggerGeneral = new System.Windows.Forms.TableLayoutPanel();
            this.lblRegexp = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new Triggernometry.UI.CustomControls.EnhancedTextBox();
            this.txtRegexp = new Triggernometry.UI.CustomControls.ExpressionTextBox();
            this.pnlButtonsSpacing = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTriggerDesc = new System.Windows.Forms.Label();
            this.tbcMain = new System.Windows.Forms.TabControl();
            this.tabTriggerActions = new System.Windows.Forms.TabPage();
            this.actionViewer = new Triggernometry.UI.CustomControls.ActionViewer();
            this.tabTriggerCondition = new System.Windows.Forms.TabPage();
            this.cndCondition = new Triggernometry.UI.CustomControls.ConditionViewer();
            this.tabScheduling = new System.Windows.Forms.TabPage();
            this.tableScheduling = new System.Windows.Forms.TableLayoutPanel();
            this.expMutexName = new Triggernometry.UI.CustomControls.ExpressionTextBox();
            this.lblMutexCapture = new System.Windows.Forms.Label();
            this.expTag = new Triggernometry.UI.CustomControls.ExpressionTextBox();
            this.lblTag = new System.Windows.Forms.Label();
            this.cbxSequential = new System.Windows.Forms.CheckBox();
            this.cbxEditAutofire = new System.Windows.Forms.CheckBox();
            this.cbxEditAutofireAllowCondition = new System.Windows.Forms.CheckBox();
            this.cbxTriggerSource = new System.Windows.Forms.ComboBox();
            this.lblTriggerSource = new System.Windows.Forms.Label();
            this.cbxRefireWithinPeriod = new System.Windows.Forms.ComboBox();
            this.lblRefireWithinPeriod = new System.Windows.Forms.Label();
            this.cbxRefireOption2 = new System.Windows.Forms.ComboBox();
            this.lblScheduleFrom = new System.Windows.Forms.Label();
            this.lblRefirePeriod = new System.Windows.Forms.Label();
            this.cbxScheduleFrom = new System.Windows.Forms.ComboBox();
            this.cbxRefireOption1 = new System.Windows.Forms.ComboBox();
            this.lblRefireOption1 = new System.Windows.Forms.Label();
            this.expRefirePeriod = new Triggernometry.UI.CustomControls.ExpressionTextBox();
            this.tabDebugging = new System.Windows.Forms.TabPage();
            this.tableDebugging = new System.Windows.Forms.TableLayoutPanel();
            this.cbxLoggingLevel = new System.Windows.Forms.ComboBox();
            this.lblLoggingLevel = new System.Windows.Forms.Label();
            this.txtEvent = new Triggernometry.UI.CustomControls.EnhancedTextBox();
            this.lblEvent = new System.Windows.Forms.Label();
            this.tableDebugReserved = new System.Windows.Forms.TableLayoutPanel();
            this.tabDescription = new System.Windows.Forms.TabPage();
            this.tableDescription = new System.Windows.Forms.TableLayoutPanel();
            this.chkReadmeTrigger = new System.Windows.Forms.CheckBox();
            this.txtDescription = new Triggernometry.UI.CustomControls.EnhancedTextBox();
            this.pnlReadOnlyHint = new System.Windows.Forms.Panel();
            this.lblReadOnly = new System.Windows.Forms.Label();
            this.grpGeneral.SuspendLayout();
            this.tableTriggerGeneral.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.tableLayoutPanelButtons.SuspendLayout();
            this.tbcMain.SuspendLayout();
            this.tabTriggerActions.SuspendLayout();
            this.tabTriggerCondition.SuspendLayout();
            this.tabScheduling.SuspendLayout();
            this.tableScheduling.SuspendLayout();
            this.tabDebugging.SuspendLayout();
            this.tableDebugging.SuspendLayout();
            this.tabDescription.SuspendLayout();
            this.tableDescription.SuspendLayout();
            this.pnlReadOnlyHint.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTopSpacing
            // 
            this.pnlTopSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopSpacing.Location = new System.Drawing.Point(10, 95);
            this.pnlTopSpacing.Name = "pnlTopSpacing";
            this.pnlTopSpacing.Size = new System.Drawing.Size(664, 10);
            this.pnlTopSpacing.TabIndex = 9;
            // 
            // grpGeneral
            // 
            this.grpGeneral.AutoSize = true;
            this.grpGeneral.Controls.Add(this.tableTriggerGeneral);
            this.grpGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpGeneral.Location = new System.Drawing.Point(10, 10);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Padding = new System.Windows.Forms.Padding(10);
            this.grpGeneral.Size = new System.Drawing.Size(664, 85);
            this.grpGeneral.TabIndex = 8;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = " General settings ";
            // 
            // tableTriggerGeneral
            // 
            this.tableTriggerGeneral.AutoSize = true;
            this.tableTriggerGeneral.ColumnCount = 2;
            this.tableTriggerGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableTriggerGeneral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableTriggerGeneral.Controls.Add(this.lblRegexp, 0, 1);
            this.tableTriggerGeneral.Controls.Add(this.lblName, 0, 0);
            this.tableTriggerGeneral.Controls.Add(this.txtName, 1, 0);
            this.tableTriggerGeneral.Controls.Add(this.txtRegexp, 1, 1);
            this.tableTriggerGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableTriggerGeneral.Location = new System.Drawing.Point(10, 23);
            this.tableTriggerGeneral.Name = "tableTriggerGeneral";
            this.tableTriggerGeneral.RowCount = 2;
            this.tableTriggerGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTriggerGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableTriggerGeneral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableTriggerGeneral.Size = new System.Drawing.Size(644, 52);
            this.tableTriggerGeneral.TabIndex = 0;
            // 
            // lblRegexp
            // 
            this.lblRegexp.AutoEllipsis = true;
            this.lblRegexp.AutoSize = true;
            this.lblRegexp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRegexp.Location = new System.Drawing.Point(3, 26);
            this.lblRegexp.Name = "lblRegexp";
            this.lblRegexp.Size = new System.Drawing.Size(97, 26);
            this.lblRegexp.TabIndex = 2;
            this.lblRegexp.Text = "Regular expression";
            this.lblRegexp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblName
            // 
            this.lblName.AutoEllipsis = true;
            this.lblName.AutoSize = true;
            this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblName.Location = new System.Drawing.Point(3, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(97, 26);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Trigger name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtName.Location = new System.Drawing.Point(106, 3);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(535, 20);
            this.txtName.TabIndex = 1;
            // 
            // txtRegexp
            // 
            this.txtRegexp.AutocompleteAvailable = true;
            this.txtRegexp.AutofillType = Triggernometry.UI.CustomControls.ExpressionTextBox.AutofillTypeEnum.None;
            this.txtRegexp.AutoSize = true;
            this.txtRegexp.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtRegexp.Expression = "";
            this.txtRegexp.ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum.Regex;
            this.txtRegexp.IsPersistent = false;
            this.txtRegexp.Location = new System.Drawing.Point(106, 29);
            this.txtRegexp.Name = "txtRegexp";
            this.txtRegexp.ReadOnly = false;
            this.txtRegexp.RegexSupportsExpressions = false;
            this.txtRegexp.Size = new System.Drawing.Size(535, 20);
            this.txtRegexp.TabIndex = 3;
            // 
            // pnlButtonsSpacing
            // 
            this.pnlButtonsSpacing.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonsSpacing.Location = new System.Drawing.Point(10, 501);
            this.pnlButtonsSpacing.Name = "pnlButtonsSpacing";
            this.pnlButtonsSpacing.Size = new System.Drawing.Size(664, 10);
            this.pnlButtonsSpacing.TabIndex = 11;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.tableLayoutPanelButtons);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(10, 511);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(664, 40);
            this.pnlButtons.TabIndex = 15;
            // 
            // tableLayoutPanelButtons
            // 
            this.tableLayoutPanelButtons.ColumnCount = 3;
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanelButtons.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.btnCancel, 2, 0);
            this.tableLayoutPanelButtons.Controls.Add(this.lblTriggerDesc, 1, 0);
            this.tableLayoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelButtons.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelButtons.Name = "tableLayoutPanelButtons";
            this.tableLayoutPanelButtons.RowCount = 1;
            this.tableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelButtons.Size = new System.Drawing.Size(664, 40);
            this.tableLayoutPanelButtons.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Location = new System.Drawing.Point(3, 3);
            this.btnOk.MinimumSize = new System.Drawing.Size(0, 35);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(144, 35);
            this.btnOk.TabIndex = 0;
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Location = new System.Drawing.Point(517, 3);
            this.btnCancel.MinimumSize = new System.Drawing.Size(0, 35);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 35);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblTriggerDesc
            // 
            this.lblTriggerDesc.AutoSize = true;
            this.lblTriggerDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTriggerDesc.Location = new System.Drawing.Point(153, 0);
            this.lblTriggerDesc.Name = "lblTriggerDesc";
            this.lblTriggerDesc.Size = new System.Drawing.Size(358, 40);
            this.lblTriggerDesc.TabIndex = 2;
            this.lblTriggerDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbcMain
            // 
            this.tbcMain.Controls.Add(this.tabTriggerActions);
            this.tbcMain.Controls.Add(this.tabTriggerCondition);
            this.tbcMain.Controls.Add(this.tabScheduling);
            this.tbcMain.Controls.Add(this.tabDebugging);
            this.tbcMain.Controls.Add(this.tabDescription);
            this.tbcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcMain.Location = new System.Drawing.Point(10, 156);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.SelectedIndex = 0;
            this.tbcMain.Size = new System.Drawing.Size(664, 345);
            this.tbcMain.TabIndex = 13;
            // 
            // tabTriggerActions
            // 
            this.tabTriggerActions.Controls.Add(this.actionViewer);
            this.tabTriggerActions.Location = new System.Drawing.Point(4, 22);
            this.tabTriggerActions.Name = "tabTriggerActions";
            this.tabTriggerActions.Padding = new System.Windows.Forms.Padding(3);
            this.tabTriggerActions.Size = new System.Drawing.Size(656, 319);
            this.tabTriggerActions.TabIndex = 0;
            this.tabTriggerActions.Text = "Trigger actions";
            this.tabTriggerActions.UseVisualStyleBackColor = true;
            // 
            // actionViewer
            // 
            this.actionViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionViewer.Location = new System.Drawing.Point(3, 3);
            this.actionViewer.Name = "actionViewer";
            this.actionViewer.Size = new System.Drawing.Size(650, 313);
            this.actionViewer.TabIndex = 2;
            // 
            // tabTriggerCondition
            // 
            this.tabTriggerCondition.Controls.Add(this.cndCondition);
            this.tabTriggerCondition.Location = new System.Drawing.Point(4, 22);
            this.tabTriggerCondition.Name = "tabTriggerCondition";
            this.tabTriggerCondition.Padding = new System.Windows.Forms.Padding(3);
            this.tabTriggerCondition.Size = new System.Drawing.Size(656, 319);
            this.tabTriggerCondition.TabIndex = 5;
            this.tabTriggerCondition.Text = "Trigger condition";
            this.tabTriggerCondition.UseVisualStyleBackColor = true;
            // 
            // cndCondition
            // 
            this.cndCondition.ConditionToEdit = null;
            this.cndCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cndCondition.Location = new System.Drawing.Point(3, 3);
            this.cndCondition.Name = "cndCondition";
            this.cndCondition.Size = new System.Drawing.Size(650, 313);
            this.cndCondition.TabIndex = 4;
            // 
            // tabScheduling
            // 
            this.tabScheduling.Controls.Add(this.tableScheduling);
            this.tabScheduling.Location = new System.Drawing.Point(4, 22);
            this.tabScheduling.Name = "tabScheduling";
            this.tabScheduling.Padding = new System.Windows.Forms.Padding(7);
            this.tabScheduling.Size = new System.Drawing.Size(656, 319);
            this.tabScheduling.TabIndex = 2;
            this.tabScheduling.Text = "Scheduling";
            this.tabScheduling.UseVisualStyleBackColor = true;
            // 
            // tableScheduling
            // 
            this.tableScheduling.AutoSize = true;
            this.tableScheduling.ColumnCount = 3;
            this.tableScheduling.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableScheduling.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableScheduling.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableScheduling.Controls.Add(this.lblTriggerSource, 0, 0);
            this.tableScheduling.Controls.Add(this.cbxTriggerSource, 1, 0);
            this.tableScheduling.Controls.Add(this.lblTag, 0, 1);
            this.tableScheduling.Controls.Add(this.expTag, 1, 1);
            this.tableScheduling.Controls.Add(this.lblRefireOption1, 0, 2);
            this.tableScheduling.Controls.Add(this.cbxRefireOption1, 1, 2);
            this.tableScheduling.Controls.Add(this.cbxRefireOption2, 1, 3);
            this.tableScheduling.Controls.Add(this.lblScheduleFrom, 0, 4);
            this.tableScheduling.Controls.Add(this.cbxScheduleFrom, 1, 4);
            this.tableScheduling.Controls.Add(this.lblRefireWithinPeriod, 0, 5);
            this.tableScheduling.Controls.Add(this.cbxRefireWithinPeriod, 1, 5);
            this.tableScheduling.Controls.Add(this.lblRefirePeriod, 0, 6);
            this.tableScheduling.Controls.Add(this.expRefirePeriod, 1, 6);
            this.tableScheduling.Controls.Add(this.lblMutexCapture, 0, 7);
            this.tableScheduling.Controls.Add(this.expMutexName, 1, 7);
            this.tableScheduling.Controls.Add(this.cbxEditAutofire, 0, 8);
            this.tableScheduling.Controls.Add(this.cbxEditAutofireAllowCondition, 0, 9);
            this.tableScheduling.Controls.Add(this.cbxSequential, 0, 10);
            this.tableScheduling.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableScheduling.Location = new System.Drawing.Point(7, 7);
            this.tableScheduling.Name = "tableScheduling";
            this.tableScheduling.RowCount = 11;
            this.tableScheduling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableScheduling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableScheduling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableScheduling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableScheduling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableScheduling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableScheduling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableScheduling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableScheduling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableScheduling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableScheduling.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableScheduling.Size = new System.Drawing.Size(642, 256);
            this.tableScheduling.TabIndex = 6;
            // 
            // expMutexName
            // 
            this.expMutexName.AutocompleteAvailable = true;
            this.expMutexName.AutofillType = Triggernometry.UI.CustomControls.ExpressionTextBox.AutofillTypeEnum.None;
            this.expMutexName.AutoSize = true;
            this.tableScheduling.SetColumnSpan(this.expMutexName, 2);
            this.expMutexName.Dock = System.Windows.Forms.DockStyle.Top;
            this.expMutexName.Expression = "";
            this.expMutexName.ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum.String;
            this.expMutexName.IsPersistent = false;
            this.expMutexName.Location = new System.Drawing.Point(282, 164);
            this.expMutexName.Name = "expMutexName";
            this.expMutexName.ReadOnly = false;
            this.expMutexName.Size = new System.Drawing.Size(357, 20);
            this.expMutexName.TabIndex = 18;
            // 
            // lblMutexCapture
            // 
            this.lblMutexCapture.AutoSize = true;
            this.lblMutexCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMutexCapture.Location = new System.Drawing.Point(3, 161);
            this.lblMutexCapture.Name = "lblMutexCapture";
            this.lblMutexCapture.Size = new System.Drawing.Size(273, 26);
            this.lblMutexCapture.Text = "Mutex to capture on fire";
            this.lblMutexCapture.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // expTag
            // 
            this.expTag.AutocompleteAvailable = true;
            this.expTag.AutofillType = Triggernometry.UI.CustomControls.ExpressionTextBox.AutofillTypeEnum.None;
            this.expTag.AutoSize = true;
            this.tableScheduling.SetColumnSpan(this.expTag, 2);
            this.expTag.Dock = System.Windows.Forms.DockStyle.Top;
            this.expTag.Expression = "";
            this.expTag.ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum.String;
            this.expTag.IsPersistent = false;
            this.expTag.Name = "expTag";
            this.expTag.ReadOnly = false;
            this.expTag.TabIndex = 17;
            // 
            // lblTag
            // 
            this.lblTag.AutoSize = true;
            this.lblTag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTag.Name = "lblTag";
            this.lblTag.TabIndex = 17;
            this.lblTag.Text = "Tag";
            this.lblTag.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxSequential
            // 
            this.cbxSequential.AutoSize = true;
            this.cbxSequential.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableScheduling.SetColumnSpan(this.cbxSequential, 3);
            this.cbxSequential.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxSequential.Location = new System.Drawing.Point(3, 236);
            this.cbxSequential.Name = "cbxSequential";
            this.cbxSequential.Size = new System.Drawing.Size(636, 17);
            this.cbxSequential.TabIndex = 16;
            this.cbxSequential.Text = "Sequential execution";
            this.cbxSequential.UseVisualStyleBackColor = true;
            // 
            // cbxEditAutofire
            // 
            this.cbxEditAutofire.AutoSize = true;
            this.cbxEditAutofire.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableScheduling.SetColumnSpan(this.cbxEditAutofire, 3);
            this.cbxEditAutofire.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxEditAutofire.Location = new System.Drawing.Point(3, 190);
            this.cbxEditAutofire.Name = "cbxEditAutofire";
            this.cbxEditAutofire.Size = new System.Drawing.Size(636, 17);
            this.cbxEditAutofire.TabIndex = 15;
            this.cbxEditAutofire.Text = "Autofire trigger after it has been edited";
            this.cbxEditAutofire.UseVisualStyleBackColor = true;
            // 
            // cbxEditAutofireAllowCondition
            // 
            this.cbxEditAutofireAllowCondition.AutoSize = true;
            this.cbxEditAutofireAllowCondition.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableScheduling.SetColumnSpan(this.cbxEditAutofireAllowCondition, 3);
            this.cbxEditAutofireAllowCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxEditAutofireAllowCondition.Location = new System.Drawing.Point(3, 213);
            this.cbxEditAutofireAllowCondition.Name = "cbxEditAutofireAllowCondition";
            this.cbxEditAutofireAllowCondition.Size = new System.Drawing.Size(636, 17);
            this.cbxEditAutofireAllowCondition.TabIndex = 15;
            this.cbxEditAutofireAllowCondition.Text = "Enable conditions for autofiring";
            this.cbxEditAutofireAllowCondition.UseVisualStyleBackColor = true;
            // 
            // cbxTriggerSource
            // 
            this.tableScheduling.SetColumnSpan(this.cbxTriggerSource, 2);
            this.cbxTriggerSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxTriggerSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTriggerSource.FormattingEnabled = true;
            this.cbxTriggerSource.Items.AddRange(new object[] {
            "Normal log lines",
            "FFXIV network events",
            "None",
            "ACT events",
            "Endpoint"});
            this.cbxTriggerSource.Location = new System.Drawing.Point(282, 3);
            this.cbxTriggerSource.Name = "cbxTriggerSource";
            this.cbxTriggerSource.Size = new System.Drawing.Size(357, 21);
            this.cbxTriggerSource.TabIndex = 14;
            // 
            // lblTriggerSource
            // 
            this.lblTriggerSource.AutoSize = true;
            this.lblTriggerSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTriggerSource.Location = new System.Drawing.Point(3, 0);
            this.lblTriggerSource.Name = "lblTriggerSource";
            this.lblTriggerSource.Size = new System.Drawing.Size(273, 27);
            this.lblTriggerSource.TabIndex = 13;
            this.lblTriggerSource.Text = "Trigger event source";
            this.lblTriggerSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxRefireWithinPeriod
            // 
            this.tableScheduling.SetColumnSpan(this.cbxRefireWithinPeriod, 2);
            this.cbxRefireWithinPeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxRefireWithinPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxRefireWithinPeriod.FormattingEnabled = true;
            this.cbxRefireWithinPeriod.Items.AddRange(new object[] {
            "Allow the trigger to fire again",
            "Do not allow the trigger to fire again"});
            this.cbxRefireWithinPeriod.Location = new System.Drawing.Point(282, 111);
            this.cbxRefireWithinPeriod.Name = "cbxRefireWithinPeriod";
            this.cbxRefireWithinPeriod.Size = new System.Drawing.Size(357, 21);
            this.cbxRefireWithinPeriod.TabIndex = 12;
            // 
            // lblRefireWithinPeriod
            // 
            this.lblRefireWithinPeriod.AutoSize = true;
            this.lblRefireWithinPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRefireWithinPeriod.Location = new System.Drawing.Point(3, 108);
            this.lblRefireWithinPeriod.Name = "lblRefireWithinPeriod";
            this.lblRefireWithinPeriod.Size = new System.Drawing.Size(273, 27);
            this.lblRefireWithinPeriod.TabIndex = 11;
            this.lblRefireWithinPeriod.Text = "If the trigger fires within the refire period";
            this.lblRefireWithinPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxRefireOption2
            // 
            this.tableScheduling.SetColumnSpan(this.cbxRefireOption2, 2);
            this.cbxRefireOption2.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxRefireOption2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxRefireOption2.FormattingEnabled = true;
            this.cbxRefireOption2.Items.AddRange(new object[] {
            "...and allow the trigger to fire again",
            "...and do not allow the trigger to fire again"});
            this.cbxRefireOption2.Location = new System.Drawing.Point(282, 57);
            this.cbxRefireOption2.Name = "cbxRefireOption2";
            this.cbxRefireOption2.Size = new System.Drawing.Size(357, 21);
            this.cbxRefireOption2.TabIndex = 10;
            // 
            // lblScheduleFrom
            // 
            this.lblScheduleFrom.AutoSize = true;
            this.lblScheduleFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblScheduleFrom.Location = new System.Drawing.Point(3, 81);
            this.lblScheduleFrom.Name = "lblScheduleFrom";
            this.lblScheduleFrom.Size = new System.Drawing.Size(273, 27);
            this.lblScheduleFrom.TabIndex = 9;
            this.lblScheduleFrom.Text = "Schedule trigger actions starting from";
            this.lblScheduleFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRefirePeriod
            // 
            this.lblRefirePeriod.AutoSize = true;
            this.lblRefirePeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRefirePeriod.Location = new System.Drawing.Point(3, 135);
            this.lblRefirePeriod.Name = "lblRefirePeriod";
            this.lblRefirePeriod.Size = new System.Drawing.Size(273, 26);
            this.lblRefirePeriod.TabIndex = 6;
            this.lblRefirePeriod.Text = "Trigger refire period";
            this.lblRefirePeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxScheduleFrom
            // 
            this.tableScheduling.SetColumnSpan(this.cbxScheduleFrom, 2);
            this.cbxScheduleFrom.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxScheduleFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxScheduleFrom.FormattingEnabled = true;
            this.cbxScheduleFrom.Items.AddRange(new object[] {
            "The firing time",
            "The last queued action from this trigger",
            "The end of the trigger refire period"});
            this.cbxScheduleFrom.Location = new System.Drawing.Point(282, 84);
            this.cbxScheduleFrom.Name = "cbxScheduleFrom";
            this.cbxScheduleFrom.Size = new System.Drawing.Size(357, 21);
            this.cbxScheduleFrom.TabIndex = 4;
            // 
            // cbxRefireOption1
            // 
            this.tableScheduling.SetColumnSpan(this.cbxRefireOption1, 2);
            this.cbxRefireOption1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbxRefireOption1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxRefireOption1.FormattingEnabled = true;
            this.cbxRefireOption1.Items.AddRange(new object[] {
            "Keep all actions previously queued from this trigger...",
            "Interrupt all actions previously queued from this trigger..."});
            this.cbxRefireOption1.Location = new System.Drawing.Point(282, 30);
            this.cbxRefireOption1.Name = "cbxRefireOption1";
            this.cbxRefireOption1.Size = new System.Drawing.Size(357, 21);
            this.cbxRefireOption1.TabIndex = 3;
            // 
            // lblRefireOption1
            // 
            this.lblRefireOption1.AutoSize = true;
            this.lblRefireOption1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRefireOption1.Location = new System.Drawing.Point(3, 27);
            this.lblRefireOption1.Name = "lblRefireOption1";
            this.lblRefireOption1.Size = new System.Drawing.Size(273, 27);
            this.lblRefireOption1.TabIndex = 2;
            this.lblRefireOption1.Text = "If the trigger fires while any of its actions are still in queue";
            this.lblRefireOption1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // expRefirePeriod
            // 
            this.expRefirePeriod.AutocompleteAvailable = true;
            this.expRefirePeriod.AutofillType = Triggernometry.UI.CustomControls.ExpressionTextBox.AutofillTypeEnum.None;
            this.expRefirePeriod.AutoSize = true;
            this.tableScheduling.SetColumnSpan(this.expRefirePeriod, 2);
            this.expRefirePeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.expRefirePeriod.Expression = "";
            this.expRefirePeriod.ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum.Numeric;
            this.expRefirePeriod.IsPersistent = false;
            this.expRefirePeriod.Location = new System.Drawing.Point(282, 138);
            this.expRefirePeriod.Name = "expRefirePeriod";
            this.expRefirePeriod.ReadOnly = false;
            this.expRefirePeriod.Size = new System.Drawing.Size(357, 20);
            this.expRefirePeriod.TabIndex = 7;
            // 
            // tabDebugging
            // 
            this.tabDebugging.Controls.Add(this.tableDebugging);
            this.tabDebugging.Controls.Add(this.tableDebugReserved);
            this.tabDebugging.Location = new System.Drawing.Point(4, 22);
            this.tabDebugging.Name = "tabDebugging";
            this.tabDebugging.Padding = new System.Windows.Forms.Padding(7);
            this.tabDebugging.Size = new System.Drawing.Size(656, 319);
            this.tabDebugging.TabIndex = 3;
            this.tabDebugging.Text = "Debugging";
            this.tabDebugging.UseVisualStyleBackColor = true;
            // 
            // tableDebugging
            // 
            this.tableDebugging.AutoSize = true;
            this.tableDebugging.ColumnCount = 3;
            this.tableDebugging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableDebugging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDebugging.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableDebugging.Controls.Add(this.cbxLoggingLevel, 1, 0);
            this.tableDebugging.Controls.Add(this.lblLoggingLevel, 0, 0);
            this.tableDebugging.Controls.Add(this.txtEvent, 1, 1);
            this.tableDebugging.Controls.Add(this.lblEvent, 0, 1);
            this.tableDebugging.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableDebugging.Location = new System.Drawing.Point(7, 7);
            this.tableDebugging.Name = "tableDebugging";
            this.tableDebugging.RowCount = 2;
            this.tableDebugging.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDebugging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDebugging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableDebugging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableDebugging.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableDebugging.Size = new System.Drawing.Size(642, 305);
            this.tableDebugging.TabIndex = 4;
            // 
            // cbxLoggingLevel
            // 
            this.tableDebugging.SetColumnSpan(this.cbxLoggingLevel, 2);
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
            "Verbose debug",
            "(inherit from configuration)"});
            this.cbxLoggingLevel.Location = new System.Drawing.Point(115, 3);
            this.cbxLoggingLevel.Name = "cbxLoggingLevel";
            this.cbxLoggingLevel.Size = new System.Drawing.Size(524, 21);
            this.cbxLoggingLevel.TabIndex = 3;
            // 
            // lblLoggingLevel
            // 
            this.lblLoggingLevel.AutoSize = true;
            this.lblLoggingLevel.Location = new System.Drawing.Point(3, 0);
            this.lblLoggingLevel.Name = "lblLoggingLevel";
            this.lblLoggingLevel.Size = new System.Drawing.Size(106, 13);
            this.lblLoggingLevel.TabIndex = 2;
            this.lblLoggingLevel.Text = "Logging filtering level";
            this.lblLoggingLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEvent
            // 
            this.txtEvent.AcceptsReturn = true;
            this.txtEvent.AcceptsTab = true;
            this.tableDebugging.SetColumnSpan(this.txtEvent, 2);
            this.txtEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEvent.Location = new System.Drawing.Point(115, 30);
            this.txtEvent.Multiline = true;
            this.txtEvent.Name = "txtEvent";
            this.txtEvent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEvent.Size = new System.Drawing.Size(524, 272);
            this.txtEvent.TabIndex = 9;
            this.txtEvent.WordWrap = false;
            // 
            // lblEvent
            // 
            this.lblEvent.AutoEllipsis = true;
            this.lblEvent.AutoSize = true;
            this.lblEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEvent.Location = new System.Drawing.Point(3, 27);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.lblEvent.Size = new System.Drawing.Size(106, 278);
            this.lblEvent.TabIndex = 8;
            this.lblEvent.Text = "Test input lines";
            // 
            // tableDebugReserved
            // 
            this.tableDebugReserved.AutoSize = true;
            this.tableDebugReserved.ColumnCount = 2;
            this.tableDebugReserved.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableDebugReserved.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDebugReserved.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableDebugReserved.Location = new System.Drawing.Point(7, 7);
            this.tableDebugReserved.Name = "tableDebugReserved";
            this.tableDebugReserved.RowCount = 1;
            this.tableDebugReserved.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDebugReserved.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableDebugReserved.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableDebugReserved.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableDebugReserved.Size = new System.Drawing.Size(642, 0);
            this.tableDebugReserved.TabIndex = 3;
            // 
            // tabDescription
            // 
            this.tabDescription.Controls.Add(this.tableDescription);
            this.tabDescription.Location = new System.Drawing.Point(4, 22);
            this.tabDescription.Name = "tabDescription";
            this.tabDescription.Padding = new System.Windows.Forms.Padding(7);
            this.tabDescription.Size = new System.Drawing.Size(656, 319);
            this.tabDescription.TabIndex = 4;
            this.tabDescription.Text = "Description";
            this.tabDescription.UseVisualStyleBackColor = true;
            // 
            // tableDescription
            // 
            this.tableDescription.AutoSize = true;
            this.tableDescription.ColumnCount = 1;
            this.tableDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableDescription.Controls.Add(this.chkReadmeTrigger, 0, 1);
            this.tableDescription.Controls.Add(this.txtDescription, 0, 0);
            this.tableDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableDescription.Location = new System.Drawing.Point(7, 7);
            this.tableDescription.Name = "tableDescription";
            this.tableDescription.RowCount = 2;
            this.tableDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDescription.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDescription.Size = new System.Drawing.Size(642, 305);
            this.tableDescription.TabIndex = 3;
            // 
            // chkReadmeTrigger
            // 
            this.chkReadmeTrigger.AutoSize = true;
            this.chkReadmeTrigger.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkReadmeTrigger.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkReadmeTrigger.Location = new System.Drawing.Point(3, 283);
            this.chkReadmeTrigger.Margin = new System.Windows.Forms.Padding(3, 5, 2, 5);
            this.chkReadmeTrigger.Name = "chkReadmeTrigger";
            this.chkReadmeTrigger.Size = new System.Drawing.Size(637, 17);
            this.chkReadmeTrigger.TabIndex = 8;
            this.chkReadmeTrigger.Text = "Specify trigger as readme for repositories";
            this.chkReadmeTrigger.UseVisualStyleBackColor = true;
            // 
            // txtDescription
            // 
            this.txtDescription.AcceptsReturn = true;
            this.txtDescription.AcceptsTab = true;
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Location = new System.Drawing.Point(3, 3);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDescription.Size = new System.Drawing.Size(636, 272);
            this.txtDescription.TabIndex = 0;
            this.txtDescription.WordWrap = false;
            // 
            // pnlReadOnlyHint
            // 
            this.pnlReadOnlyHint.BackColor = System.Drawing.SystemColors.Control;
            this.pnlReadOnlyHint.Controls.Add(this.lblReadOnly);
            this.pnlReadOnlyHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReadOnlyHint.Location = new System.Drawing.Point(10, 105);
            this.pnlReadOnlyHint.Name = "pnlReadOnlyHint";
            this.pnlReadOnlyHint.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.pnlReadOnlyHint.Size = new System.Drawing.Size(664, 51);
            this.pnlReadOnlyHint.TabIndex = 14;
            this.pnlReadOnlyHint.Visible = false;
            // 
            // lblReadOnly
            // 
            this.lblReadOnly.AutoEllipsis = true;
            this.lblReadOnly.BackColor = System.Drawing.SystemColors.Info;
            this.lblReadOnly.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblReadOnly.Location = new System.Drawing.Point(0, 0);
            this.lblReadOnly.Name = "lblReadOnly";
            this.lblReadOnly.Size = new System.Drawing.Size(664, 41);
            this.lblReadOnly.TabIndex = 0;
            this.lblReadOnly.Text = "You are in read-only mode, as the configuration of remote triggers can\'t be edite" +
    "d locally. If you wish to edit the trigger, you will need to make a local copy o" +
    "f it.";
            this.lblReadOnly.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TriggerForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(684, 561);
            this.Controls.Add(this.tbcMain);
            this.Controls.Add(this.pnlReadOnlyHint);
            this.Controls.Add(this.pnlTopSpacing);
            this.Controls.Add(this.grpGeneral);
            this.Controls.Add(this.pnlButtonsSpacing);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(700, 600);
            this.Name = "TriggerForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Shown += new System.EventHandler(this.TriggerForm_Shown);
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            this.tableTriggerGeneral.ResumeLayout(false);
            this.tableTriggerGeneral.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.tableLayoutPanelButtons.ResumeLayout(false);
            this.tableLayoutPanelButtons.PerformLayout();
            this.tbcMain.ResumeLayout(false);
            this.tabTriggerActions.ResumeLayout(false);
            this.tabTriggerCondition.ResumeLayout(false);
            this.tabScheduling.ResumeLayout(false);
            this.tabScheduling.PerformLayout();
            this.tableScheduling.ResumeLayout(false);
            this.tableScheduling.PerformLayout();
            this.tabDebugging.ResumeLayout(false);
            this.tabDebugging.PerformLayout();
            this.tableDebugging.ResumeLayout(false);
            this.tableDebugging.PerformLayout();
            this.tabDescription.ResumeLayout(false);
            this.tabDescription.PerformLayout();
            this.tableDescription.ResumeLayout(false);
            this.tableDescription.PerformLayout();
            this.pnlReadOnlyHint.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlTopSpacing;
        private System.Windows.Forms.GroupBox grpGeneral;
        private System.Windows.Forms.TableLayoutPanel tableTriggerGeneral;
        private System.Windows.Forms.Label lblRegexp;
        private System.Windows.Forms.Label lblName;
        private Triggernometry.UI.CustomControls.EnhancedTextBox txtName;
        private System.Windows.Forms.Panel pnlButtonsSpacing;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelButtons;
        private System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnOk;
        internal System.Windows.Forms.Label lblTriggerDesc;
        private CustomControls.ExpressionTextBox txtRegexp;
        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tabTriggerActions;
        private System.Windows.Forms.TabPage tabScheduling;
        private System.Windows.Forms.TableLayoutPanel tableScheduling;
        private System.Windows.Forms.Label lblRefirePeriod;
        private System.Windows.Forms.ComboBox cbxScheduleFrom;
        private System.Windows.Forms.ComboBox cbxRefireOption1;
        private System.Windows.Forms.Label lblRefireOption1;
        private CustomControls.ExpressionTextBox expRefirePeriod;
        private System.Windows.Forms.Label lblScheduleFrom;
        private System.Windows.Forms.ComboBox cbxRefireOption2;
        private System.Windows.Forms.ComboBox cbxRefireWithinPeriod;
        private System.Windows.Forms.Label lblRefireWithinPeriod;
        private System.Windows.Forms.TabPage tabDebugging;
        private System.Windows.Forms.TableLayoutPanel tableDebugReserved;
        private System.Windows.Forms.ComboBox cbxLoggingLevel;
        private System.Windows.Forms.Label lblLoggingLevel;
        private System.Windows.Forms.TabPage tabDescription;
        private Triggernometry.UI.CustomControls.EnhancedTextBox txtDescription;
        private System.Windows.Forms.TabPage tabTriggerCondition;
        private CustomControls.ConditionViewer cndCondition;
        private System.Windows.Forms.ComboBox cbxTriggerSource;
        private System.Windows.Forms.Label lblTriggerSource;
        private System.Windows.Forms.Panel pnlReadOnlyHint;
        private System.Windows.Forms.Label lblReadOnly;
        private System.Windows.Forms.CheckBox cbxEditAutofire;
        private System.Windows.Forms.CheckBox cbxEditAutofireAllowCondition;
        private System.Windows.Forms.CheckBox cbxSequential;
        private CustomControls.ExpressionTextBox expMutexName;
        private System.Windows.Forms.Label lblMutexCapture;
        private CustomControls.ExpressionTextBox expTag;
        private System.Windows.Forms.Label lblTag;
        private System.Windows.Forms.TableLayoutPanel tableDescription;
        private System.Windows.Forms.CheckBox chkReadmeTrigger;
        private CustomControls.ActionViewer actionViewer;
        private System.Windows.Forms.TableLayoutPanel tableDebugging;
        internal Triggernometry.UI.CustomControls.EnhancedTextBox txtEvent;
        private System.Windows.Forms.Label lblEvent;
    }
}