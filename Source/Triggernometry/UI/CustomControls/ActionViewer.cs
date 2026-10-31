using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.Core.Actions;
using Triggernometry.Core.Conditions;
using Triggernometry.Localization;
using Triggernometry.UI.Forms;

namespace Triggernometry.UI.CustomControls
{

    public partial class ActionViewer : UserControl
    {
        private bool IsReadonly { get; set; } = false;

        /// <summary> Set by parent form. </summary>
        internal Context UiContext { get; set; }
        private Trigger Trigger => UiContext?.Trigger;
        private RealPlugin Plugin => UiContext?.Plugin;

        internal List<ActionBase> Actions { get; set; } = new List<ActionBase>();
        internal ImageList Images { get; set; }
        internal TreeView TreeView { get; set; }
        internal List<ActionBase> PrevActions { get; set; }
        internal List<int> PrevSelectedIndices { get; set; }
        private static ConditionGroup copiedCondition;

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (tlsActions != null)
                tlsActions.Font = Font;
            if (ctxAction != null)
                ctxAction.Font = Font;
        }

        public ActionViewer()
        {
            InitializeComponent();
        }

        internal event EventHandler ActionsUpdated;

        internal void OnActionsUpdated()
        {
            ActionsUpdated?.Invoke(this, EventArgs.Empty);
        }

        internal void RefreshDgv()
        {
            dgvActions.RowCount = Actions.Count;
        }

        internal void SetReadOnly()
        {
            IsReadonly = true;
            dgvActions.ReadOnly = true;
            btnAddAction.Enabled = false;
            btnActionUp.Enabled = false;
            btnActionDown.Enabled = false;
            btnActionTop.Enabled = false;
            btnActionBottom.Enabled = false;
            btnRemoveAction.Enabled = false;
            btnUndo.Enabled = false;
        }

        private void dgvActions_SelectionChanged(object sender, EventArgs e)
        {
            btnEditAction.Enabled = (dgvActions.SelectedRows.Count == 1);
            bool allowMoveAndRemove = IsReadonly == false && (dgvActions.SelectedRows.Count > 0);
            btnRemoveAction.Enabled = allowMoveAndRemove;
            btnActionUp.Enabled = allowMoveAndRemove;
            btnActionDown.Enabled = allowMoveAndRemove;
            btnActionTop.Enabled = allowMoveAndRemove;
            btnActionBottom.Enabled = allowMoveAndRemove;
        }

        internal List<Core.ActionBase> SelectedActions()
        {
            return dgvActions.SelectedRows
                             .Cast<DataGridViewRow>()
                             .OrderBy(row => row.Index)
                             .Select(row => Actions[row.Index])
                             .ToList();
        }

        private void dgvActions_Leave(object sender, EventArgs e)
        {
            dgvActions.ClearSelection();
        }

        private void dgvActions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                CopySelectedActions();
            }
            else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                PasteSelectedActions();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (btnRemoveAction.Enabled == true)
                {
                    btnRemoveAction_Click(this, null);
                }
            }
        }

        private void btnAddAction_Click(object sender, EventArgs e)
        {
            ActionBase a = new ActionPlaceholder(); // default
            using (ActionForm af = new ActionForm(a, UiContext, TreeView, Images))
            {
                if (IsReadonly == true)
                {
                    af.SetReadOnly();
                }
                af.Text = I18n.Translate("internal/TriggerForm/addnewaction", "Add new action");
                af.btnOk.Text = I18n.Translate("internal/TriggerForm/add", "Add");
                if (af.ShowDialog() == DialogResult.OK)
                {
                    a = af.SettingsToAction();
                    a.Enabled = true;
                    int insertIndex = (dgvActions.Rows.Count > 0 && dgvActions.SelectedRows.Count > 0) ? (dgvActions.SelectedRows[0].Index + 1) : dgvActions.Rows.Count;
                    StoreActions();
                    Actions.Insert(insertIndex, a);
                    dgvActions.RowCount = Actions.Count;
                    dgvActions.ClearSelection();
                    dgvActions.Rows[insertIndex].Selected = true;
                    OnActionsUpdated();
                }
            }
        }

        private void btnEditAction_Click(object sender, EventArgs e)
        {
            if (dgvActions.SelectedRows.Count == 0) { return; }
            int rowIndex = dgvActions.SelectedRows[0].Index;
            if (rowIndex < 0 || rowIndex >= Actions.Count ) { return; }
            var action = Actions[rowIndex];
            using (ActionForm af = new ActionForm(action, UiContext, TreeView, Images))
            {
                if (IsReadonly == true)
                {
                    af.SetReadOnly();
                }
                af.Text = I18n.Translate("internal/TriggerForm/editaction", "Edit action '{0}'", action.Describe());
                af.btnOk.Text = I18n.Translate("internal/TriggerForm/savechanges", "Save changes");
                if (af.ShowDialog() == DialogResult.OK)
                {
                    StoreActions();
                    Actions[rowIndex] = af.SettingsToAction();
                    dgvActions.Refresh();
                    OnActionsUpdated();
                }
            }
        }

        private void MoveSelectedRows(string moveType)
        {
            int length = Actions.Count;
            List<int> selectedRowIndices = dgvActions.SelectedRows.Cast<DataGridViewRow>().Select(row => row.Index).ToList();
            List<int> unselectedRowIndices = Enumerable.Range(0, length).Except(selectedRowIndices).ToList();
            selectedRowIndices.Sort();
            int start, end;
            switch (moveType)
            {
                case "up":
                    start = Math.Max(selectedRowIndices[0] - 1, 0); // the first selected row moves to this index
                    end = start + selectedRowIndices.Count - 1;     // the last selected row moves to this index
                    break;
                case "top":
                    start = 0;
                    end = selectedRowIndices.Count - 1;
                    break;
                case "down":
                    end = Math.Min(selectedRowIndices[selectedRowIndices.Count - 1] + 1, length - 1);
                    start = end - selectedRowIndices.Count + 1;
                    break;
                case "bottom":
                    end = length - 1;
                    start = length - selectedRowIndices.Count;
                    break;
                default: throw new Exception(I18n.Translate("internal/ActionViewer/invalidmovetype", "Invalid moveType argument: {0}", moveType));
            }

            var reordered = new List<ActionBase>(length);
            int selectedIndex = 0, unselectedIndex = 0;
            for (int i = 0; i < length; i++)
            {
                int sourceIndex = i >= start && i <= end
                    ? selectedRowIndices[selectedIndex++]
                    : unselectedRowIndices[unselectedIndex++];
                reordered.Add(Actions[sourceIndex]);
            }
            Actions = reordered;

            dgvActions.ClearSelection();
            for (int i = start; i <= end; i++)
            {
                dgvActions.Rows[i].Selected = true;
            }

            dgvActions.Refresh();
        }

        private void btnActionUp_Click(object sender, EventArgs e)
        {
            StoreActions();
            MoveSelectedRows("up");
            // OnActionsUpdated();  not used since moving actions would not change trigger descriptions
        }

        private void btnActionDown_Click(object sender, EventArgs e)
        {
            StoreActions();
            MoveSelectedRows("down");
        }

        private void btnActionTop_Click(object sender, EventArgs e)
        {
            StoreActions();
            MoveSelectedRows("top");
        }

        private void btnActionBottom_Click(object sender, EventArgs e)
        {
            StoreActions();
            MoveSelectedRows("bottom");
        }

        private void btnRemoveAction_Click(object sender, EventArgs e)
        {
            string temp;
            if (dgvActions.SelectedRows.Count > 1)
            {
                temp = I18n.Translate("internal/TriggerForm/areyousureplural", "Are you sure you want to remove the selected actions?");
            }
            else
            {
                temp = I18n.Translate("internal/TriggerForm/areyousuresingular", "Are you sure you want to remove the selected action?");
            }
            switch (MessageBox.Show(this, temp, I18n.Translate("internal/TriggerForm/confirmremoval", "Confirm removal"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                case DialogResult.Yes:
                    StoreActions();
                    var selected = new HashSet<ActionBase>(SelectedActions());
                    Actions.RemoveAll(action => selected.Contains(action));
                    dgvActions.RowCount = Actions.Count;
                    dgvActions.ClearSelection();
                    dgvActions.Refresh();
                    OnActionsUpdated();
                    break;
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (PrevActions == null || PrevSelectedIndices == null) { return; }

            (Actions, PrevActions) = (PrevActions, Actions);

            var tempIndices = PrevSelectedIndices;
            PrevSelectedIndices = dgvActions.SelectedRows.Cast<DataGridViewRow>()
                .Select(row => row.Index).ToList();
            dgvActions.RowCount = Actions.Count;
            dgvActions.ClearSelection();
            dgvActions.Refresh();
            foreach (int index in tempIndices)
            {
                if (index < dgvActions.RowCount)
                {
                    dgvActions.Rows[index].Selected = true;
                }
            }
            OnActionsUpdated();
        }

        private void StoreActions()
        {
            PrevActions = Actions.Select(action => action.Copy()).ToList();

            PrevSelectedIndices = dgvActions.SelectedRows.Cast<DataGridViewRow>()
                .Select(row => row.Index).ToList();
            btnUndo.Enabled = true;
        }

        private void dgvActions_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= Actions.Count)
            {
                return;
            }
            switch (e.ColumnIndex)
            {
                case 0:
                    e.Value = Actions[e.RowIndex].Enabled;
                    break;
                case 1:
                    e.Value = Actions[e.RowIndex].Describe();
                    break;
            }
        }

        /// <summary>按动作状态及颜色表达式设置单元格配色。</summary>
        private void dgvActions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= Actions.Count)
            {
                return;
            }
            Core.ActionBase a = Actions[e.RowIndex];

            // set a warning color when a delay (not zero) is hidden under the description
            string delay = "0" + a.ExecDelay.Trim();
            if (a.Enabled && a.DescOverride && !( double.TryParse(delay, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double result) && result == 0 ))
            {
                e.CellStyle.BackColor = Color.FromArgb(240, 224, 128); // light yellow
                e.CellStyle.ForeColor = SystemColors.InactiveCaptionText;
            }
            else
            {
                // customized colors
                Color bgColor, textColor;
                try
                {
                    string rawBgColor = UiContext.ExpandVariables(null, null, false, a.DescBackColor);
                    bgColor = ExpressionTextBox.ParseColor(rawBgColor, Color.Empty);
                }
                catch { bgColor = Color.Empty; }

                try
                {
                    string rawTextColor = UiContext.ExpandVariables(null, null, false, a.DescTextColor);
                    textColor = ExpressionTextBox.ParseColor(rawTextColor, Color.Empty);
                }
                catch { textColor = Color.Empty; }

                // placeholder / normal color
                if (a is ActionPlaceholder)
                {
                    e.CellStyle.BackColor = (bgColor != Color.Empty) ? bgColor : SystemColors.InactiveCaption;
                    e.CellStyle.ForeColor = (textColor != Color.Empty) ? textColor : SystemColors.InactiveCaptionText;
                }
                else
                {
                    e.CellStyle.BackColor = (bgColor != Color.Empty)
                        ? bgColor : dgvActions.DefaultCellStyle.BackColor;
                    e.CellStyle.ForeColor = (textColor != Color.Empty)
                        ? textColor : (a.Enabled)
                            ? dgvActions.DefaultCellStyle.ForeColor : Color.FromArgb(176, 192, 208);
                }
            }
        }

        private void dgvActions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (IsReadonly || e.RowIndex < 0 || e.RowIndex >= Actions.Count)
            {
                return;
            }
            if (e.ColumnIndex != 0)
            {
                return;
            }
            StoreActions();
            Actions[e.RowIndex].Enabled = !Actions[e.RowIndex].Enabled;
            OnActionsUpdated();
        }

        private void dgvActions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= Actions.Count || dgvActions.SelectedRows.Count == 0)
            {
                return;
            }
            if (e.ColumnIndex == 1)
            {
                btnEditAction_Click(sender, null);
                return;
            }
            if (!IsReadonly)
            {
                StoreActions();
                Actions[e.RowIndex].Enabled = !Actions[e.RowIndex].Enabled;
                OnActionsUpdated();
            }
        }

        private void CopySelectedActions()
        {
            try
            {
                if (dgvActions.SelectedRows.Count > 0)
                {
                    var selectedActions = SelectedActions();
                    var xmlData = Core.ActionBase.ActionBundle.ActionsToXml(selectedActions);
                    System.Windows.Forms.Clipboard.SetText(xmlData);
                }
            }
            catch (Exception ex)
            {
                Plugin.FilteredAddToLog(
                    RealPlugin.DebugLevelEnum.Error,
                    I18n.Translate("internal/TriggerForm/actioncopyfail", "Core.Action copy failed due to exception: {0}", ex.Message),
                    this.Trigger);
            }
        }

        private void PasteSelectedActions()
        {
            if (OkToPasteAction() == false)
            {
                return;
            }
            PasteActions(System.Windows.Forms.Clipboard.GetText(TextDataFormat.UnicodeText));
        }

        private void PasteActions(string data)
        {
            if (IsReadonly)
                return;
            try
            {
                // Parse the XML and collect the actions into a list
                List<Core.ActionBase> pastedActions = Core.ActionBase.ActionBundle.XmlToActions(data);
                if ((pastedActions?.Count ?? 0) == 0)
                    return;

                // Set ParentTrigger for all pasted actions
                Core.ActionBase.RebuildParentage(pastedActions, Trigger);

                // Decide insert index:
                // If there are selected rows: insert after the last selected row
                // If nothing is selected: insert at the top (index 0)
                int insertIndex = dgvActions.SelectedRows.Cast<DataGridViewRow>().Select(r => r.Index).DefaultIfEmpty(-1).Max() + 1;

                // Insert/append all new actions at the determined position
                StoreActions();
                Actions.InsertRange(insertIndex, pastedActions);

                // Update UI: row count, selection, refresh
                dgvActions.SuspendLayout();
                dgvActions.RowCount = Actions.Count;
                dgvActions.ClearSelection();
                for (int i = insertIndex; i < insertIndex + pastedActions.Count; i++)
                {
                    dgvActions.Rows[i].Selected = true;
                }
                dgvActions.ResumeLayout();
                dgvActions.Invalidate();

                OnActionsUpdated();
            }
            catch (Exception ex)
            {
                System.Media.SystemSounds.Exclamation.Play();
                Plugin.FilteredAddToLog(
                    RealPlugin.DebugLevelEnum.Warning,
                    I18n.Translate("internal/TriggerForm/actionpastefail", "Core.Action paste failed due to exception: {0}", ex.Message),
                    this.Trigger);
            }
        }

        public List<string> GetActionDescriptions()
        {
            List<string> descriptions = new List<string>();
            foreach (Core.ActionBase action in Actions)
            {
                descriptions.Add(action.Describe());
            }
            return descriptions;
        }

        public double GetActionTotalDelay()
        {   // return the total delay, or NaN if at least one of the expressions is not numeric
            double totalDelay = 0;
            foreach (var action in Actions)
            {
                if (!action.Enabled) { continue; }
                string delay = action.ExecDelay;
                if (delay.Contains("$") || delay.ToLower().Contains("random"))
                {
                    return Double.NaN;
                }
                else
                {
                    try { totalDelay += UiContext.EvaluateNumericExpression(null, null, delay); }
                    catch { return Double.NaN; }
                }
            }
            return totalDelay;
        }

        private void ctxAddAction_Click(object sender, EventArgs e)
        {
            btnAddAction_Click(sender, e);
        }

        private void ctxEditAction_Click(object sender, EventArgs e)
        {
            btnEditAction_Click(sender, e);
        }

        private void ctxCopyAction_Click(object sender, EventArgs e)
        {
            CopySelectedActions();
        }

        private void ctxPasteAction_Click(object sender, EventArgs e)
        {
            PasteSelectedActions();
        }

        private void ctxMoveUp_Click(object sender, EventArgs e)
        {
            btnActionUp_Click(sender, e);
        }

        private void ctxMoveDown_Click(object sender, EventArgs e)
        {
            btnActionDown_Click(sender, e);
        }

        private void ctxMoveTop_Click(object sender, EventArgs e)
        {
            btnActionTop_Click(sender, e);
        }

        private void ctxMoveBottom_Click(object sender, EventArgs e)
        {
            btnActionBottom_Click(sender, e);
        }

        private void ctxRemoveAction_Click(object sender, EventArgs e)
        {
            btnRemoveAction_Click(sender, e);
        }

        private void ctxUndo_Click(object sender, EventArgs e)
        {
            btnUndo_Click(sender, e);
        }

        /// <summary>复制测试动作并固定本次测试选项，不修改编辑中的动作。</summary>
        internal static Action CreateTestExecution(ActionBase source, Context uiContext, bool useLiveValues, bool ignoreConditions)
        {
            var action = source.Copy();
            ActionBase.RebuildParentage(new[] { action }, uiContext?.Trigger ?? source.ParentTrigger);
            if (ignoreConditions)
                action.Condition = new ConditionGroup();
            var plugin = uiContext?.Plugin ?? RealPlugin.Instance;
            // 按键测试可能延后执行，上下文在实际执行时创建，保持时间变量的含义。
            return () => action.Execute(new Context(action.ParentTrigger)
            {
                testByPlaceholder = !useLiveValues,
                soundhook = plugin.SoundPlaybackSmart,
                ttshook = plugin.TtsPlaybackSmart
            });
        }

        private void ctxTest_Click(object sender, EventArgs e)
        {
            ActionBase action = SelectedActions().FirstOrDefault();
            if (action == null)
                return;
            var item = (ToolStripMenuItem)sender;
            // 直接点击父菜单沿用测试默认配置，与动作编辑窗口的测试按钮一致。
            bool useDefaults = item == ctxTestAction;
            bool useLiveValues = item != ctxTestPlaceholder;
            bool ignoreConditions = useDefaults ? Plugin.Config.TestIgnoreConditionsByDefault : item == ctxTestLiveIgnoreCnd;
            if (useDefaults)
                ctxAction.Close();
            CreateTestExecution(action, UiContext, useLiveValues, ignoreConditions)();
        }

        /// <summary>批量编辑前保存一次撤销快照，完成后统一刷新和通知。</summary>
        private void UpdateSelectedActions(Action<ActionBase> update)
        {
            if (IsReadonly)
                return;
            var selected = SelectedActions();
            if (selected.Count == 0)
                return;
            StoreActions();
            foreach (var action in selected) update(action);
            dgvActions.Refresh();
            OnActionsUpdated();
        }

        private void ctxEditPropCopyCnd_Click(object sender, EventArgs e)
        {
            ActionViewer.copiedCondition = (ConditionGroup)SelectedActions().FirstOrDefault()?.Condition.Duplicate();
        }

        private void ctxEditPropPasteCnd_Click(object sender, EventArgs e)
        {
            if (ActionViewer.copiedCondition == null)
                return;
            UpdateSelectedActions(a =>
            {
                a.Condition = (ConditionGroup)ActionViewer.copiedCondition.Duplicate();
            });
        }

        private void ctxEditPropRemoveCnd_Click(object sender, EventArgs e)
        {
            UpdateSelectedActions(a =>
            {
                a.Condition = new ConditionGroup { Enabled = false };
            });
        }

        private void ctxEditPropCndGroupingAnd_Click(object sender, EventArgs e)
        {
            SetCndGroupingType(ConditionGroup.CndGroupingEnum.And);
        }

        private void ctxEditPropCndGroupingOr_Click(object sender, EventArgs e)
        {
            SetCndGroupingType(ConditionGroup.CndGroupingEnum.Or);
        }

        private void ctxEditPropCndGroupingXor_Click(object sender, EventArgs e)
        {
            SetCndGroupingType(ConditionGroup.CndGroupingEnum.Xor);
        }

        private void ctxEditPropCndGroupingNot_Click(object sender, EventArgs e)
        {
            SetCndGroupingType(ConditionGroup.CndGroupingEnum.Not);
        }

        private void ctxEditPropAsyncOn_Click(object sender, EventArgs e)
        {
            UpdateSelectedActions(a =>
            {
                a.Async = true;
            });
        }

        private void ctxEditPropAsyncOff_Click(object sender, EventArgs e)
        {
            UpdateSelectedActions(a =>
            {
                a.Async = false;
            });
        }

        private void ctxEditPropDelay_Click(object sender, EventArgs e)
        {
            string value = SimpleInputForm.GetInput(
                I18n.Translate("internal/ActionViewer/setDelay", "Set Core.Action Delay To (ms)"),
                ExpressionTextBox.SupportedExpressionTypeEnum.Numeric
                );
            if (value == null)
                return;
            UpdateSelectedActions(a =>
            {
                a.ExecDelay = value;
            });
        }

        private void ctxEditPropBgColor_Click(object sender, EventArgs e)
        {
            string value = SimpleInputForm.GetInput(
                I18n.Translate("internal/ActionViewer/setBgColor", "Set Description Background Color To"),
                ExpressionTextBox.SupportedExpressionTypeEnum.Color
                );
            if (value == null)
                return;
            UpdateSelectedActions(a =>
            {
                a.DescBackColor = value;
            });
        }

        private void ctxEditPropTextColor_Click(object sender, EventArgs e)
        {
            string value = SimpleInputForm.GetInput(
                I18n.Translate("internal/ActionViewer/setTextColor", "Set Description Text Color To"),
                ExpressionTextBox.SupportedExpressionTypeEnum.Color
                );
            if (value == null)
                return;
            UpdateSelectedActions(a =>
            {
                a.DescTextColor = value;
            });
        }

        private void ctxEditPropRemoveDesc_Click(object sender, EventArgs e)
        {
            UpdateSelectedActions(a =>
            {
                a.DescOverride = false;
                a.Desc = "";
            });
        }

        private void SetCndGroupingType(ConditionGroup.CndGroupingEnum cndGroupingType)
        {
            UpdateSelectedActions(a =>
            {
                if (a.Condition == null)
                {
                    a.Condition = new ConditionGroup();
                    a.Condition.Enabled = false;
                }
                a.Condition.Grouping = cndGroupingType;
            });
        }

        private void ctxAction_Opening(object sender, CancelEventArgs e)
        {
            bool isSingleActionSelected = dgvActions.SelectedRows.Count == 1;
            bool hasSelection = dgvActions.SelectedRows.Count > 0;
            ctxAddAction.Enabled = !IsReadonly && isSingleActionSelected;
            ctxEditAction.Enabled = isSingleActionSelected;
            foreach (ToolStripItem item in ctxEditProp.DropDownItems)
            {
                if (item is ToolStripMenuItem)
                    item.Enabled = !IsReadonly && hasSelection;
            }
            ctxEditPropCopyCnd.Enabled = isSingleActionSelected;
            ctxEditPropPasteCnd.Enabled = !IsReadonly && hasSelection && ActionViewer.copiedCondition != null;
            ctxTestAction.Enabled = isSingleActionSelected && !(SelectedActions()[0] is ActionPlaceholder);

            bool allowMutate = IsReadonly == false && hasSelection;
            ctxCopyAction.Enabled = hasSelection;
            ctxMoveUp.Enabled = allowMutate;
            ctxMoveDown.Enabled = allowMutate;
            ctxMoveTop.Enabled = allowMutate;
            ctxMoveBottom.Enabled = allowMutate;
            ctxRemoveAction.Enabled = allowMutate;
            ctxUndo.Enabled = btnUndo.Enabled;
            ctxPasteAction.Enabled = OkToPasteAction();
        }

        private bool OkToPasteAction()
        {
            string data = Clipboard.GetText(TextDataFormat.UnicodeText);
            return IsReadonly == false && (data != null && data.Length > 0);
        }

        private void dgvActions_MouseClick(object sender, MouseEventArgs e)
        {   // clicking the grey region
            int rowIndex = dgvActions.HitTest(e.X, e.Y).RowIndex;
            if (rowIndex == -1 || rowIndex >= dgvActions.RowCount)
            {
                dgvActions.ClearSelection();
            }
        }

        private void dgvActions_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hitTestInfo = dgvActions.HitTest(e.X, e.Y);
                if (hitTestInfo.Type == DataGridViewHitTestType.Cell)
                {
                    int rowIndex = hitTestInfo.RowIndex;
                    if (!dgvActions.Rows[rowIndex].Selected) // if not right-clicking on a selected row: change selection
                    {
                        dgvActions.ClearSelection();
                        dgvActions.Rows[rowIndex].Selected = true;
                    }
                }
            }
        }

    }

}
