namespace Triggernometry.UI.CustomControls
{
    partial class ActionEditor
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>释放设计器创建的组件。</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>创建并布局动作参数编辑控件。</summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.helpLayout = new System.Windows.Forms.TableLayoutPanel();
            this.configurationEditor = new Triggernometry.UI.CustomControls.ConfigEditor();
            this.tools = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            //
            // configurationEditor
            //
            this.configurationEditor.Controls.Add(this.helpLayout);
            this.configurationEditor.Controls.SetChildIndex(this.helpLayout, 0);
            this.configurationEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configurationEditor.Location = new System.Drawing.Point(0, 36);
            this.configurationEditor.Name = "configurationEditor";
            this.configurationEditor.Size = new System.Drawing.Size(760, 424);
            this.configurationEditor.TabIndex = 1;
            //
            // tools
            //
            this.tools.AutoSize = true;
            this.tools.Dock = System.Windows.Forms.DockStyle.Top;
            this.tools.Location = new System.Drawing.Point(0, 0);
            this.tools.Name = "tools";
            this.tools.Size = new System.Drawing.Size(760, 36);
            this.tools.TabIndex = 0;
            this.tools.Visible = false;
            //
            // helpLayout
            //
            this.helpLayout.AutoSize = true;
            this.helpLayout.ColumnCount = 1;
            this.helpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.helpLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.helpLayout.Name = "helpLayout";
            this.helpLayout.Padding = new System.Windows.Forms.Padding(6);
            this.helpLayout.Visible = false;
            //
            // ActionEditor
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.configurationEditor);
            this.Controls.Add(this.tools);
            this.Name = "ActionEditor";
            this.Size = new System.Drawing.Size(760, 460);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel helpLayout;
        private ConfigEditor configurationEditor;
        private System.Windows.Forms.FlowLayoutPanel tools;
    }
}
