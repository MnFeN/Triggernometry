namespace Triggernometry.UI.Forms
{
    partial class ActionForm
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>释放设计器创建的组件。</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>创建并布局动作编辑窗口。</summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.layout = new System.Windows.Forms.TableLayoutPanel();
            this.lblActionType = new System.Windows.Forms.Label();
            this.btnActionType = new Triggernometry.UI.CustomControls.MenuComboBox();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabActionSettings = new System.Windows.Forms.TabPage();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tabActionCondition = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tabDescription = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel23 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDescription = new Triggernometry.UI.CustomControls.EnhancedTextBox();
            this.chkOverrideDesc = new System.Windows.Forms.CheckBox();
            this.lblDescBgColor = new System.Windows.Forms.Label();
            this.lblDescTextColor = new System.Windows.Forms.Label();
            this.expDescBgColor = new Triggernometry.UI.CustomControls.ExpressionTextBox();
            this.expDescTextColor = new Triggernometry.UI.CustomControls.ExpressionTextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnTest = new Triggernometry.UI.CustomControls.SplitButton();
            this.testMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.actionTypeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.layout.SuspendLayout();
            this.tabs.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabDescription.SuspendLayout();
            this.tableLayoutPanel23.SuspendLayout();
            this.SuspendLayout();
            //
            // layout
            //
            this.layout.ColumnCount = 2;
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layout.Controls.Add(this.lblActionType, 0, 0);
            this.layout.Controls.Add(this.btnActionType, 1, 0);
            this.layout.Controls.Add(this.tabs, 0, 1);
            this.layout.Controls.Add(this.panel3, 0, 2);
            this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layout.Name = "layout";
            this.layout.Padding = new System.Windows.Forms.Padding(8);
            this.layout.RowCount = 3;
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.layout.Size = new System.Drawing.Size(860, 620);
            //
            // lblActionType
            //
            this.lblActionType.AutoSize = true;
            this.lblActionType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblActionType.Margin = new System.Windows.Forms.Padding(4, 7, 8, 7);
            this.lblActionType.Name = "lblActionType";
            this.lblActionType.Text = "Action type";
            this.lblActionType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnActionType
            //
            this.btnActionType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnActionType.DropDown = this.actionTypeMenu;
            this.btnActionType.Name = "btnActionType";
            this.btnActionType.TabIndex = 0;
            //
            // tabs
            //
            this.layout.SetColumnSpan(this.tabs, 2);
            this.tabs.Controls.Add(this.tabActionSettings);
            this.tabs.Controls.Add(this.tabActionCondition);
            this.tabs.Controls.Add(this.tabGeneral);
            this.tabs.Controls.Add(this.tabDescription);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Name = "tabs";
            this.tabs.TabIndex = 1;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.Tabs_SelectedIndexChanged);
            //
            // tabActionSettings
            //
            this.tabActionSettings.Name = "tabActionSettings";
            this.tabActionSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabActionSettings.Text = "Action-specific settings";
            this.tabActionSettings.UseVisualStyleBackColor = true;
            //
            // tabGeneral
            //
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Text = "General settings";
            this.tabGeneral.UseVisualStyleBackColor = true;
            //
            // tabActionCondition
            //
            this.tabActionCondition.Name = "tabActionCondition";
            this.tabActionCondition.Padding = new System.Windows.Forms.Padding(3);
            this.tabActionCondition.Text = "Action condition";
            this.tabActionCondition.UseVisualStyleBackColor = true;
            //
            // tabDescription
            // 
            this.tabDescription.Controls.Add(this.tableLayoutPanel23);
            this.tabDescription.Location = new System.Drawing.Point(4, 22);
            this.tabDescription.Name = "tabDescription";
            this.tabDescription.Padding = new System.Windows.Forms.Padding(7);
            this.tabDescription.Size = new System.Drawing.Size(756, 449);
            this.tabDescription.TabIndex = 4;
            this.tabDescription.Text = "Description";
            this.tabDescription.UseVisualStyleBackColor = true;
            //
            // tableLayoutPanel23
            // 
            this.tableLayoutPanel23.AutoSize = true;
            this.tableLayoutPanel23.ColumnCount = 4;
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel23.Controls.Add(this.chkOverrideDesc, 0, 2);
            this.tableLayoutPanel23.Controls.Add(this.txtDescription, 0, 0);
            this.tableLayoutPanel23.Controls.Add(this.lblDescBgColor, 0, 1);
            this.tableLayoutPanel23.Controls.Add(this.expDescBgColor, 1, 1);
            this.tableLayoutPanel23.Controls.Add(this.lblDescTextColor, 2, 1);
            this.tableLayoutPanel23.Controls.Add(this.expDescTextColor, 3, 1);
            this.tableLayoutPanel23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel23.Location = new System.Drawing.Point(7, 7);
            this.tableLayoutPanel23.Name = "tableLayoutPanel23";
            this.tableLayoutPanel23.RowCount = 3;
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel23.Size = new System.Drawing.Size(742, 435);
            this.tableLayoutPanel23.TabIndex = 0;
            //
            // txtDescription
            // 
            this.txtDescription.AcceptsReturn = true;
            this.txtDescription.AcceptsTab = true;
            this.tableLayoutPanel23.SetColumnSpan(this.txtDescription, 4);
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Location = new System.Drawing.Point(3, 3);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDescription.Size = new System.Drawing.Size(736, 376);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.WordWrap = false;
            //
            // chkOverrideDesc
            // 
            this.chkOverrideDesc.AutoSize = true;
            this.chkOverrideDesc.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableLayoutPanel23.SetColumnSpan(this.chkOverrideDesc, 4);
            this.chkOverrideDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkOverrideDesc.Location = new System.Drawing.Point(3, 413);
            this.chkOverrideDesc.Margin = new System.Windows.Forms.Padding(3, 5, 2, 5);
            this.chkOverrideDesc.Name = "chkOverrideDesc";
            this.chkOverrideDesc.Size = new System.Drawing.Size(737, 17);
            this.chkOverrideDesc.TabIndex = 0;
            this.chkOverrideDesc.TabStop = false;
            this.chkOverrideDesc.Text = "Override autogenerated action description";
            this.chkOverrideDesc.UseVisualStyleBackColor = true;
            //
            // lblDescBgColor
            // 
            this.lblDescBgColor.AutoSize = true;
            this.lblDescBgColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescBgColor.Location = new System.Drawing.Point(3, 382);
            this.lblDescBgColor.Name = "lblDescBgColor";
            this.lblDescBgColor.Size = new System.Drawing.Size(142, 26);
            this.lblDescBgColor.TabIndex = 2;
            this.lblDescBgColor.Text = "Background Color";
            this.lblDescBgColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblDescTextColor
            // 
            this.lblDescTextColor.AutoSize = true;
            this.lblDescTextColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescTextColor.Location = new System.Drawing.Point(373, 382);
            this.lblDescTextColor.Name = "lblDescTextColor";
            this.lblDescTextColor.Size = new System.Drawing.Size(142, 26);
            this.lblDescTextColor.TabIndex = 5;
            this.lblDescTextColor.Text = "Text Color";
            this.lblDescTextColor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // expDescBgColor
            // 
            this.expDescBgColor.AutocompleteAvailable = true;
            this.expDescBgColor.AutofillType = Triggernometry.UI.CustomControls.ExpressionTextBox.AutofillTypeEnum.None;
            this.expDescBgColor.AutoSize = true;
            this.expDescBgColor.Dock = System.Windows.Forms.DockStyle.Top;
            this.expDescBgColor.Expression = "";
            this.expDescBgColor.ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum.Color;
            this.expDescBgColor.IsPersistent = false;
            this.expDescBgColor.Location = new System.Drawing.Point(151, 385);
            this.expDescBgColor.Name = "expDescBgColor";
            this.expDescBgColor.ReadOnly = false;
            this.expDescBgColor.Size = new System.Drawing.Size(216, 20);
            this.expDescBgColor.TabIndex = 4;
            //
            // expDescTextColor
            // 
            this.expDescTextColor.AutocompleteAvailable = true;
            this.expDescTextColor.AutofillType = Triggernometry.UI.CustomControls.ExpressionTextBox.AutofillTypeEnum.None;
            this.expDescTextColor.AutoSize = true;
            this.expDescTextColor.Dock = System.Windows.Forms.DockStyle.Top;
            this.expDescTextColor.Expression = "";
            this.expDescTextColor.ExpressionType = Triggernometry.UI.CustomControls.ExpressionTextBox.SupportedExpressionTypeEnum.Color;
            this.expDescTextColor.IsPersistent = false;
            this.expDescTextColor.Location = new System.Drawing.Point(521, 385);
            this.expDescTextColor.Name = "expDescTextColor";
            this.expDescTextColor.ReadOnly = false;
            this.expDescTextColor.Size = new System.Drawing.Size(218, 20);
            this.expDescTextColor.TabIndex = 6;
            //
            // panel3
            // 
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnOk);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(10, 616);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(764, 35);
            this.panel3.TabIndex = 6;
            //
            // panel6
            // 
            this.panel6.Controls.Add(this.btnTest);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(150, 0);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.panel6.Size = new System.Drawing.Size(464, 35);
            this.panel6.TabIndex = 2;
            //
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(614, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 35);
            this.btnCancel.TabIndex = 3;
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
            this.btnOk.TabIndex = 4;
            this.btnOk.TabStop = false;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            //
            // btnTest
            // 
            this.btnTest.AutoSize = true;
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTest.Location = new System.Drawing.Point(10, 0);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(444, 35);
            this.btnTest.TabIndex = 0;
            this.btnTest.TabStop = false;
            this.btnTest.Text = "Test action";
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.BtnTest_Click);
            this.btnTest.ContextMenuStrip = this.testMenu;
            this.layout.SetColumnSpan(this.panel3, 2);
            //
            // ActionForm
            //
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 620);
            this.Controls.Add(this.layout);
            this.MinimumSize = new System.Drawing.Size(620, 400);
            this.Name = "ActionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit action";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ActionForm_FormClosing);
            this.layout.ResumeLayout(false);
            this.layout.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.tabDescription.ResumeLayout(false);
            this.tabDescription.PerformLayout();
            this.tableLayoutPanel23.ResumeLayout(false);
            this.tableLayoutPanel23.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layout;
        private System.Windows.Forms.Label lblActionType;
        private Triggernometry.UI.CustomControls.MenuComboBox btnActionType;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabActionSettings;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabActionCondition;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TabPage tabDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel23;
        private Triggernometry.UI.CustomControls.EnhancedTextBox txtDescription;
        private System.Windows.Forms.CheckBox chkOverrideDesc;
        private System.Windows.Forms.Label lblDescBgColor;
        private System.Windows.Forms.Label lblDescTextColor;
        private Triggernometry.UI.CustomControls.ExpressionTextBox expDescBgColor;
        private Triggernometry.UI.CustomControls.ExpressionTextBox expDescTextColor;
        private System.Windows.Forms.ContextMenuStrip testMenu;
        private System.Windows.Forms.ContextMenuStrip actionTypeMenu;
        internal System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private Triggernometry.UI.CustomControls.SplitButton btnTest;
    }
}
