using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Triggernometry.Core;
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

        internal List<ActionOld> Actions { get; set; } = new List<ActionOld>();
        internal ImageList Images { get; set; }
        internal TreeView TreeView { get; set; }
        internal List<ActionOld> PrevActions { get; set; }
        internal List<int> PrevSelectedIndices { get; set; }
        private static ConditionGroup copiedCondition;

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

        internal List<Core.ActionOld> SelectedActions()
        {
            return dgvActions.SelectedRows
                             .Cast<DataGridViewRow>()
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
            var a = new ActionOld();
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
                    af.SettingsToAction(a);
                    a.Enabled = true;
                    int insertIndex = (dgvActions.Rows.Count > 0 && dgvActions.SelectedRows.Count > 0) ? (dgvActions.SelectedRows[0].Index + 1) : dgvActions.Rows.Count;
                    Actions.Insert(insertIndex, a);
                    a.OrderNumber = insertIndex + 1;
                    for (int i = insertIndex + 1; i < Actions.Count; i++) { Actions[i].OrderNumber++; }
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
            Context ctx = new Context(UiContext.Trigger);
            var action = Actions[rowIndex];
            using (ActionForm af = new ActionForm(action, UiContext, TreeView, Images))
            {
                if (IsReadonly == true)
                {
                    af.SetReadOnly();
                }
                af.Text = I18n.Translate("internal/TriggerForm/editaction", "Edit action '{0}'", action.GetDescription(ctx));
                af.btnOk.Text = I18n.Translate("internal/TriggerForm/savechanges", "Save changes");
                if (af.ShowDialog() == DialogResult.OK)
                {
                    StoreActions();
                    af.SettingsToAction(action);
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
                default: throw new Exception($"Wrong moveType argument {moveType}");
            }

            List<int> ordMap = new List<int>(new int[length]);

            for (int i = 0; i < length; i++)
            {
                if (i >= start && i <= end)
                {
                    ordMap[i] = selectedRowIndices[0] + 1;
                    selectedRowIndices.RemoveAt(0);
                }
                else
                {
                    ordMap[i] = unselectedRowIndices[0] + 1;
                    unselectedRowIndices.RemoveAt(0);
                }
            }

            for (int i = 0; i < length; i++)
            {
                Actions[ordMap[i] - 1].OrderNumber = i + 1;
            }

            Actions.Sort((a, b) => a.OrderNumber.CompareTo(b.OrderNumber));

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
                    foreach (Core.ActionOld a in SelectedActions())
                    {
                        Actions.Remove(a);
                        List<Core.ActionOld> px = new List<Core.ActionOld>();
                        px.AddRange(from ax in Actions where ax.OrderNumber > a.OrderNumber select ax);
                        foreach (Core.ActionOld aaa in px)
                        {
                            aaa.OrderNumber--;
                        }
                    }
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
            PrevActions = new List<Core.ActionOld>();
            foreach (var originalAction in Actions)
            {
                Core.ActionOld copiedAction = new Core.ActionOld();
                originalAction.CopySettingsTo(copiedAction);
                PrevActions.Add(copiedAction);
            }

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
                    e.Value = Actions[e.RowIndex].GetDescription(UiContext);
                    break;
            }
        }

        private void dgvActions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= Actions.Count)
            {
                return;
            }
            Core.ActionOld a = Actions[e.RowIndex];

            // set a warning color when a delay (not zero) is hidden under the description
            string delay = "0" + a.ExecutionDelayExpression.Trim();
            if (a.Enabled && a.DescriptionOverride && !( double.TryParse(delay, out double result) && result == 0 ))
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
                    string rawBgColor = UiContext.ExpandVariables(null, null, false, a.DescBgColor);
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
                if (a.ActionType == Core.ActionOld.ActionTypeEnum.Placeholder)
                {
                    e.CellStyle.BackColor = (bgColor != Color.Empty) ? bgColor : SystemColors.InactiveCaption;
                    e.CellStyle.ForeColor = (textColor != Color.Empty) ? textColor : SystemColors.InactiveCaptionText;
                }
                else
                {
                    e.CellStyle.BackColor = (bgColor != Color.Empty) ? bgColor : dgvActions.DefaultCellStyle.BackColor;
                    e.CellStyle.ForeColor = (textColor != Color.Empty) ? textColor :
                                            (a.Enabled) ? dgvActions.DefaultCellStyle.ForeColor : Color.FromArgb(176, 192, 208);
                }
            }
        }

        private void dgvActions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= Actions.Count)
            {
                return;
            }
            if (e.ColumnIndex != 0)
            {
                return;
            }
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
            Actions[e.RowIndex].Enabled = (Actions[e.RowIndex].Enabled == false);
        }

        private void CopySelectedActions()
        {
            try
            {
                if (btnRemoveAction.Enabled == true)
                {
                    var selectedActions = SelectedActions();
                    var xmlData = Core.ActionOld.ActionBundle.ActionsToXml(selectedActions);
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
            StoreActions();
            string data = System.Windows.Forms.Clipboard.GetText(TextDataFormat.UnicodeText);
            try
            {
                // Parse the XML and collect the actions into a list
                List<Core.ActionOld> pastedActions = Core.ActionOld.ActionBundle.XmlToActions(data);
                if ((pastedActions?.Count ?? 0) == 0) return;

                // Set ParentTrigger for all pasted actions
                pastedActions.ForEach(a => a.ParentTrigger = Trigger);

                // Decide insert index:
                // If there are selected rows: insert after the last selected row
                // If nothing is selected: insert at the top (index 0)
                int insertIndex = dgvActions.SelectedRows.Cast<DataGridViewRow>().Select(r => r.Index).DefaultIfEmpty(-1).Max() + 1;

                // Insert/append all new actions at the determined position
                foreach (var act in pastedActions)
                {
                    Actions.Insert(insertIndex++, act);
                }

                // Re-number all current Actions consecutively (1..N)
                for (int i = 0; i < Actions.Count; i++)
                {
                    Actions[i].OrderNumber = i + 1;
                }

                // Update UI: row count, selection, refresh
                dgvActions.SuspendLayout();
                dgvActions.RowCount = Actions.Count;
                dgvActions.ClearSelection();
                foreach (var pastedAction in pastedActions)
                {
                    int idx = Actions.IndexOf(pastedAction);
                    if (idx >= 0)
                        dgvActions.Rows[idx].Selected = true;
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
            foreach (Core.ActionOld action in Actions)
            {
                descriptions.Add(action.GetDescription(UiContext));
            }
            return descriptions;
        }

        public double GetActionTotalDelay()
        {   // return the total delay, or NaN if at least one of the expressions is not numeric
            double totalDelay = 0;
            foreach (var action in Actions)
            {
                if (!action.Enabled) { continue; }
                string delay = action.ExecutionDelayExpression;
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

        private void ctxTest_Click(object sender, EventArgs e)
        {
            Core.ActionOld selectedAction = SelectedActions().FirstOrDefault();
            if (selectedAction == null)
                return;
            Core.ActionOld a = new Core.ActionOld();
            selectedAction.CopySettingsTo(a);
            Context ctx = new Context(a.ParentTrigger);
            ctx.soundhook = RealPlugin.Instance.SoundPlaybackSmart;
            ctx.ttshook = RealPlugin.Instance.TtsPlaybackSmart;

            var item = (ToolStripMenuItem)sender;
            switch (item.Name)
            {
                case "ctxTestAction":
                    ctx.testByPlaceholder = RealPlugin.Instance.cfg.TestLiveByDefault == false;
                    if (Plugin.cfg.TestIgnoreConditionsByDefault)
                        a.Condition = new ConditionGroup();
                    ctxAction.Close();
                    break;
                case "ctxTestPlaceholder":
                    ctx.testByPlaceholder = true;
                    break;
                case "ctxTestLive":
                    ctx.testByPlaceholder = false;
                    break;
                case "ctxTestLiveIgnoreCnd":
                    ctx.testByPlaceholder = false;
                    if (Plugin.cfg.TestIgnoreConditionsByDefault)
                        a.Condition = new ConditionGroup();
                    break;
            }
            
            a.Execute(null, ctx);
        }

        private void ctxEditPropCopyCnd_Click(object sender, EventArgs e)
        {
            ActionViewer.copiedCondition = (ConditionGroup)SelectedActions().FirstOrDefault()?.Condition.Duplicate();
        }

        private void ctxEditPropPasteCnd_Click(object sender, EventArgs e)
        {
            if (ActionViewer.copiedCondition == null) 
                return;
            foreach (Core.ActionOld a in SelectedActions())
            {
                a.Condition = (ConditionGroup)ActionViewer.copiedCondition.Duplicate();
            }
            dgvActions.Refresh();
            OnActionsUpdated();
        }

        private void ctxEditPropRemoveCnd_Click(object sender, EventArgs e)
        {
            foreach (Core.ActionOld a in SelectedActions())
            {
                a.Condition = new ConditionGroup();
                a.Condition.Enabled = false;
            }
            dgvActions.Refresh();
            OnActionsUpdated();
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
            foreach (Core.ActionOld a in SelectedActions())
            {
                a.Asynchronous = true;
            }
            dgvActions.Refresh();
            OnActionsUpdated();
        }

        private void ctxEditPropAsyncOff_Click(object sender, EventArgs e)
        {
            foreach (Core.ActionOld a in SelectedActions())
            {
                a.Asynchronous = false;
            }
            dgvActions.Refresh();
            OnActionsUpdated();
        }

        private void ctxEditPropDelay_Click(object sender, EventArgs e)
        {
            string value = new SimpleInputForm(
                I18n.Translate("internal/ActionViewer/setDelay", "Set Core.Action Delay To (ms)"),
                ExpressionTextBox.SupportedExpressionTypeEnum.Numeric
                ).GetInput();
            if (value != null)
            {
                foreach (Core.ActionOld a in SelectedActions())
                {
                    a.ExecutionDelayExpression = value;
                }
            }
            dgvActions.Refresh();
            OnActionsUpdated();
        }

        private void ctxEditPropBgColor_Click(object sender, EventArgs e)
        {
            string value = new SimpleInputForm(
                I18n.Translate("internal/ActionViewer/setBgColor", "Set Description Background Color To"),
                ExpressionTextBox.SupportedExpressionTypeEnum.Color
                ).GetInput();
            if (value != null)
            {
                foreach (Core.ActionOld a in SelectedActions())
                {
                    a.DescBgColor = value;
                }
            }
            dgvActions.Refresh();
            OnActionsUpdated();
        }

        private void ctxEditPropTextColor_Click(object sender, EventArgs e)
        {
            string value = new SimpleInputForm(
                I18n.Translate("internal/ActionViewer/setTextColor", "Set Description Text Color To"), 
                ExpressionTextBox.SupportedExpressionTypeEnum.Color
                ).GetInput();
            if (value != null)
            {
                foreach (Core.ActionOld a in SelectedActions())
                {
                    a.DescTextColor = value;
                }
            }
            dgvActions.Refresh();
            OnActionsUpdated();
        }

        private void ctxEditPropRemoveDesc_Click(object sender, EventArgs e)
        {
            foreach (Core.ActionOld a in SelectedActions())
            {
                a.DescriptionOverride = false;
                a.Description = "";
            }
            dgvActions.Refresh();
            OnActionsUpdated();
        }

        private void SetCndGroupingType(ConditionGroup.CndGroupingEnum cndGroupingType)
        {
            foreach (Core.ActionOld a in SelectedActions())
            {
                if (a.Condition == null) 
                { 
                    a.Condition = new ConditionGroup();
                    a.Condition.Enabled = false;
                }
                a.Condition.Grouping = cndGroupingType;
            }
            dgvActions.Refresh();
            OnActionsUpdated();
        }

        private void ctxAction_Opening(object sender, CancelEventArgs e)
        {
            bool isSingleActionSelected = dgvActions.SelectedRows.Count == 1;
            bool hasSelection = dgvActions.SelectedRows.Count > 0;
            ctxAddAction.Enabled = isSingleActionSelected;
            ctxEditAction.Enabled = isSingleActionSelected;
            ctxEditPropCopyCnd.Enabled = isSingleActionSelected;
            ctxEditPropPasteCnd.Enabled = ActionViewer.copiedCondition != null;
            ctxTestAction.Enabled = isSingleActionSelected && SelectedActions()[0].ActionType != Core.ActionOld.ActionTypeEnum.Placeholder;
            
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
            string data = System.Windows.Forms.Clipboard.GetText(TextDataFormat.UnicodeText);
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
