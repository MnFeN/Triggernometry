using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Triggernometry.Core;
using Triggernometry.Core.Variables;
using Triggernometry.Localization;
using Triggernometry.UI.Forms;

namespace Triggernometry.UI.CustomControls
{
    public partial class RegisteredVariableViewer : UserControl
    {
        private bool isReadOnly;
        private enum RowMove { Up, Down, Top, Bottom }

        internal Folder Folder { get; set; }
        internal RealPlugin Plugin { get; set; }
        private RealPlugin CurrentPlugin => Plugin ?? RealPlugin.Instance;

        public RegisteredVariableViewer()
        {
            InitializeComponent();
            colLoadWhenActive.DisplayMember = "Value";
            colLoadWhenActive.ValueMember = "Key";
            colLoadWhenActive.DataSource = new[]
            {
                new KeyValuePair<bool, string>(false, I18n.Translate("internal/RegisteredVariableViewer/alwaysactive", "Always active")),
                new KeyValuePair<bool, string>(true, I18n.Translate("internal/RegisteredVariableViewer/folderenabled", "When folder enabled"))
            };
            UpdateButtons();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetInitialColumnWidths();
        }

        /// <summary> Fits the initial columns to their contents while reserving half the available space for values. </summary>
        private void SetInitialColumnWidths()
        {
            colEnabled.Width = colEnabled.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
            int available = dgvVariables.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - colEnabled.Width - 2;
            int valueWidth = Math.Max(colValue.MinimumWidth, available / 2);
            int policyWidth = colLoadWhenActive.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
            foreach (var option in (KeyValuePair<bool, string>[])colLoadWhenActive.DataSource)
            {
                policyWidth = Math.Max(policyWidth, TextRenderer.MeasureText(option.Value, dgvVariables.Font).Width
                    + SystemInformation.HorizontalScrollBarThumbWidth + 12);
            }
            policyWidth = Math.Max(policyWidth, TextRenderer.MeasureText(colLoadWhenActive.HeaderText, dgvVariables.Font).Width + 20);
            colLoadWhenActive.Width = Math.Min(policyWidth, Math.Max(colLoadWhenActive.MinimumWidth,
                available - valueWidth - colName.MinimumWidth));
            colName.Width = Math.Min(colName.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true),
                Math.Max(colName.MinimumWidth, available - valueWidth - colLoadWhenActive.Width));
        }

        /// <summary> Loads independent editor rows, retaining saved names for rename confirmation. </summary>
        internal void SetVariables(IEnumerable<RegisteredVariable> variables)
        {
            dgvVariables.Rows.Clear();
            foreach (RegisteredVariable variable in variables ?? Enumerable.Empty<RegisteredVariable>())
            {
                InsertRow(dgvVariables.Rows.Count, variable, true);
            }
            dgvVariables.ClearSelection();
            UpdateButtons();
        }

        /// <summary> Keeps remote definitions read-only while allowing copying, viewing and runtime application. </summary>
        internal void SetReadOnly()
        {
            isReadOnly = true;
            dgvVariables.ReadOnly = true;
            foreach (DataGridViewRow row in dgvVariables.Rows) UpdateRowStyle(row);
            UpdateButtons();
        }

        private List<DataGridViewRow> SelectedRows()
        {
            return dgvVariables.SelectedRows.Cast<DataGridViewRow>().OrderBy(row => row.Index).ToList();
        }

        private void InsertRow(int index, RegisteredVariable variable, bool keepSavedName)
        {
            dgvVariables.Rows.Insert(index, variable.Enabled, variable.IsComment ? "" : variable.Name,
                variable.Value, variable.LoadWhenActive);
            DataGridViewRow row = dgvVariables.Rows[index];
            row.Tag = variable.Duplicate();
            if (!keepSavedName) ((RegisteredVariable)row.Tag).Name = "";
            UpdateRowStyle(row);
        }

        private RegisteredVariable ReadRow(DataGridViewRow row)
        {
            bool isComment = ((RegisteredVariable)row.Tag).IsComment;
            return new RegisteredVariable
            {
                Enabled = row.Cells[colEnabled.Index].Value is bool enabled && enabled,
                IsComment = isComment,
                Name = isComment ? "" : row.Cells[colName.Index].Value?.ToString() ?? "",
                Value = row.Cells[colValue.Index].Value?.ToString() ?? "",
                LoadWhenActive = !isComment && row.Cells[colLoadWhenActive.Index].Value is bool loadWhenActive && loadWhenActive
            };
        }

        /// <summary> Commits the current cell and validates only enabled variable definitions. </summary>
        internal bool TryGetVariables(out List<RegisteredVariable> variables)
        {
            variables = null;
            if (!dgvVariables.EndEdit()) return false;
            return TryReadRows(dgvVariables.Rows.Cast<DataGridViewRow>(), out variables);
        }

        private bool TryReadRows(IEnumerable<DataGridViewRow> rows, out List<RegisteredVariable> variables)
        {
            variables = new List<RegisteredVariable>();
            foreach (DataGridViewRow row in rows)
            {
                RegisteredVariable variable = ReadRow(row);
                if (variable.Enabled && !variable.IsComment && string.IsNullOrWhiteSpace(variable.Name))
                {
                    if (Parent is TabPage tab && tab.Parent is TabControl tabs) tabs.SelectedTab = tab;
                    dgvVariables.CurrentCell = row.Cells[colName.Index];
                    MessageBox.Show(this,
                        I18n.Translate("internal/RegisteredVariableViewer/namerequired", "Enter a key for each enabled registered variable."),
                        I18n.Translate("internal/FolderForm/registeredvariables", "Registered variables"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                variables.Add(variable);
            }
            return true;
        }

        private void UpdateRowStyle(DataGridViewRow row)
        {
            if (!(row.Tag is RegisteredVariable definition)) return;
            RegisteredVariable variable = ReadRow(row);
            row.DefaultCellStyle.BackColor = variable.IsComment ? SystemColors.InactiveCaption
                : variable.LoadWhenActive ? Color.FromArgb(255, 225, 255) : Color.White;
            row.DefaultCellStyle.ForeColor = !variable.Enabled ? SystemColors.GrayText
                : variable.IsComment ? SystemColors.InactiveCaptionText : dgvVariables.DefaultCellStyle.ForeColor;
            row.DefaultCellStyle.SelectionForeColor = variable.Enabled ? SystemColors.HighlightText : SystemColors.GrayText;
            row.Cells[colEnabled.Index].ReadOnly = isReadOnly || definition.IsComment;
            row.Cells[colName.Index].ReadOnly = isReadOnly || definition.IsComment;
            row.Cells[colLoadWhenActive.Index].ReadOnly = isReadOnly || definition.IsComment;
            // Multiline values are edited in the expanded editor so inline editing cannot flatten them.
            row.Cells[colValue.Index].ReadOnly = isReadOnly || definition.IsComment || variable.Value.Contains("\n") || variable.Value.Contains("\r");
            // The comment spans cells, so changing its value must repaint the whole row.
            dgvVariables.InvalidateRow(row.Index);
        }

        private void UpdateButtons()
        {
            var selected = SelectedRows();
            bool canMutate = !isReadOnly && selected.Count > 0;
            btnAddVariable.Enabled = btnAddComment.Enabled = !isReadOnly;
            ctxAddVariable.Enabled = !isReadOnly;
            ctxAddComment.Enabled = !isReadOnly;
            btnExpandEdit.Enabled = ctxExpandEdit.Enabled = selected.Count == 1;
            btnRemove.Enabled = ctxRemove.Enabled = canMutate;
            btnUp.Enabled = ctxUp.Enabled = canMutate;
            btnDown.Enabled = ctxDown.Enabled = canMutate;
            btnTop.Enabled = ctxTop.Enabled = canMutate;
            btnBottom.Enabled = ctxBottom.Enabled = canMutate;
            ctxCopy.Enabled = selected.Count > 0;
            ctxPaste.Enabled = !isReadOnly;
            btnApply.Enabled = ctxApply.Enabled = selected.Any(IsApplicable);
            btnApplyAll.Enabled = ctxApplyAll.Enabled = dgvVariables.Rows.Cast<DataGridViewRow>().Any(IsApplicable);
        }

        private bool IsApplicable(DataGridViewRow row)
        {
            return row.Tag is RegisteredVariable definition && !definition.IsComment
                && row.Cells[colEnabled.Index].Value is bool enabled && enabled;
        }

        private void AddRow(bool isComment)
        {
            if (isReadOnly || !dgvVariables.EndEdit()) return;
            int index = SelectedRows().Select(row => row.Index).DefaultIfEmpty(dgvVariables.RowCount - 1).Max() + 1;
            InsertRow(index, new RegisteredVariable { IsComment = isComment }, false);
            dgvVariables.ClearSelection();
            dgvVariables.Rows[index].Selected = true;
            dgvVariables.CurrentCell = dgvVariables.Rows[index].Cells[isComment ? colValue.Index : colName.Index];
            if (isComment) ExpandEdit();
            else dgvVariables.BeginEdit(true);
        }

        private void AddVariable_Click(object sender, EventArgs e) => AddRow(false);
        private void AddComment_Click(object sender, EventArgs e) => AddRow(true);

        private void ExpandEdit()
        {
            if (!dgvVariables.EndEdit()) return;
            var selected = SelectedRows();
            if (selected.Count != 1) return;
            DataGridViewRow row = selected[0];
            RegisteredVariable variable = ReadRow(row);
            string title = variable.IsComment
                ? I18n.Translate("internal/RegisteredVariableViewer/editcomment", "Expand comment")
                : I18n.Translate("internal/RegisteredVariableViewer/editvalue", "Expand value: {0}", variable.Name);
            using (var form = new SimpleInputForm(title, ExpressionTextBox.SupportedExpressionTypeEnum.String, variable.Value, multiline: true))
            {
                form.ReadOnly = isReadOnly;
                string value = form.GetInput(FindForm());
                if (value == null || isReadOnly) return;
                row.Cells[colValue.Index].Value = value;
                UpdateRowStyle(row);
            }
        }

        private void ExpandEdit_Click(object sender, EventArgs e) => ExpandEdit();

        /// <summary> Applies enabled selected rows or all rows without saving folder definitions. </summary>
        private void ApplyRows(bool all)
        {
            if (!dgvVariables.EndEdit()) return;
            var rows = all ? dgvVariables.Rows.Cast<DataGridViewRow>().ToList() : SelectedRows();
            if (!TryReadRows(rows, out List<RegisteredVariable> variables)) return;
            for (int i = 0; i < rows.Count; i++)
            {
                RegisteredVariable variable = variables[i];
                if (!variable.Enabled || variable.IsComment) continue;
                CurrentPlugin.RegisterVariable(Folder.LogName, variable);
                var original = (RegisteredVariable)rows[i].Tag;
                if (string.IsNullOrEmpty(original.Name)) original.Name = variable.Name;
            }
        }

        private void Apply_Click(object sender, EventArgs e) => ApplyRows(false);
        private void ApplyAll_Click(object sender, EventArgs e) => ApplyRows(true);

        /// <summary> Offers to remove renamed runtime keys when the enclosing form is saved. </summary>
        internal void ConfirmRenamedVariables()
        {
            foreach (DataGridViewRow row in dgvVariables.Rows)
            {
                RegisteredVariable variable = ReadRow(row);
                string oldName = ((RegisteredVariable)row.Tag).Name;
                if (!variable.Enabled || variable.IsComment || string.IsNullOrEmpty(oldName) || oldName == variable.Name) continue;
                string oldValue;
                lock (CurrentPlugin.RegisteredVariables)
                {
                    if (!CurrentPlugin.RegisteredVariables.TryGetValue(oldName, out VariableScalar previous)) continue;
                    oldValue = previous.Value;
                }
                if (MessageBox.Show(this,
                    I18n.Translate("internal/FolderForm/deleterenamedregisteredvariable",
                        "Registered variable '{0}' was renamed to '{1}'.\nOld runtime value: '{2}'\nDelete the old key-value pair?", oldName, variable.Name, oldValue),
                    I18n.Translate("internal/FolderForm/registeredvariables", "Registered variables"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    lock (CurrentPlugin.RegisteredVariables) CurrentPlugin.RegisteredVariables.Remove(oldName);
                }
            }
        }

        /// <summary> Moves selected rows as one ordered block, retaining comment rows and rename metadata. </summary>
        private void MoveRows(RowMove move)
        {
            if (isReadOnly || !dgvVariables.EndEdit()) return;
            var selected = SelectedRows();
            if (selected.Count == 0) return;
            var remaining = dgvVariables.Rows.Cast<DataGridViewRow>().Except(selected).ToList();
            int index;
            switch (move)
            {
                case RowMove.Up: index = Math.Max(0, selected[0].Index - 1); break;
                case RowMove.Down: index = Math.Min(remaining.Count, selected.Last().Index - selected.Count + 2); break;
                case RowMove.Top: index = 0; break;
                default: index = remaining.Count; break;
            }
            remaining.InsertRange(index, selected);
            dgvVariables.Rows.Clear();
            dgvVariables.Rows.AddRange(remaining.ToArray());
            dgvVariables.CurrentCell = selected[0].Cells[colValue.Index];
            dgvVariables.ClearSelection();
            foreach (DataGridViewRow row in selected) row.Selected = true;
            UpdateButtons();
        }

        private void Up_Click(object sender, EventArgs e) => MoveRows(RowMove.Up);
        private void Down_Click(object sender, EventArgs e) => MoveRows(RowMove.Down);
        private void Top_Click(object sender, EventArgs e) => MoveRows(RowMove.Top);
        private void Bottom_Click(object sender, EventArgs e) => MoveRows(RowMove.Bottom);

        private void Remove_Click(object sender, EventArgs e)
        {
            if (isReadOnly || !dgvVariables.EndEdit()) return;
            var selected = SelectedRows();
            if (selected.Count == 0) return;
            if (MessageBox.Show(this,
                I18n.Translate("internal/RegisteredVariableViewer/confirmremove", "Remove the selected rows?"),
                I18n.Translate("internal/RegisteredVariableViewer/removal", "Confirm removal"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            foreach (DataGridViewRow row in selected) dgvVariables.Rows.Remove(row);
            UpdateButtons();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (!dgvVariables.EndEdit()) return;
            var variables = SelectedRows().Select(ReadRow).ToList();
            if (variables.Count == 0) return;
            try
            {
                var serializer = new XmlSerializer(typeof(List<RegisteredVariable>), new XmlRootAttribute("RegisteredVariables"));
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, variables);
                    Clipboard.SetText(writer.ToString());
                }
            }
            catch (Exception ex) { ShowClipboardError(ex); }
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            if (isReadOnly || !dgvVariables.EndEdit() || !Clipboard.ContainsText()) return;
            try
            {
                var serializer = new XmlSerializer(typeof(List<RegisteredVariable>), new XmlRootAttribute("RegisteredVariables"));
                List<RegisteredVariable> variables;
                using (var reader = new StringReader(Clipboard.GetText()))
                {
                    variables = (List<RegisteredVariable>)serializer.Deserialize(reader);
                }
                int index = SelectedRows().Select(row => row.Index).DefaultIfEmpty(-1).Max() + 1;
                dgvVariables.ClearSelection();
                foreach (RegisteredVariable variable in variables) InsertRow(index++, variable, false);
                for (int i = index - variables.Count; i < index; i++) dgvVariables.Rows[i].Selected = true;
                UpdateButtons();
            }
            catch (Exception ex) { ShowClipboardError(ex); }
        }

        private void ShowClipboardError(Exception ex)
        {
            MessageBox.Show(this,
                I18n.Translate("internal/RegisteredVariableViewer/clipboarderror", "Could not copy or paste registered variables:\n{0}", ex.Message),
                I18n.Translate("internal/FolderForm/registeredvariables", "Registered variables"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void dgvVariables_SelectionChanged(object sender, EventArgs e) => UpdateButtons();

        private void dgvVariables_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvVariables.IsCurrentCellDirty && (dgvVariables.CurrentCell is DataGridViewCheckBoxCell
                || dgvVariables.CurrentCell is DataGridViewComboBoxCell))
                dgvVariables.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvVariables_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            UpdateRowStyle(dgvVariables.Rows[e.RowIndex]);
            UpdateButtons();
        }

        private void dgvVariables_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colValue.Index || e.Value == null) return;
            e.Value = e.Value.ToString().Replace("\r\n", " ⏎ ").Replace("\r", " ⏎ ").Replace("\n", " ⏎ ");
            e.FormattingApplied = true;
        }

        private void dgvVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == colValue.Index
                || ((RegisteredVariable)dgvVariables.Rows[e.RowIndex].Tag).IsComment)) ExpandEdit();
        }

        /// <summary> Draws comment text across the entire row without individual cell borders or controls. </summary>
        private void dgvVariables_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow row = dgvVariables.Rows[e.RowIndex];
            if (!(row.Tag is RegisteredVariable definition) || !definition.IsComment) return;
            Rectangle bounds = dgvVariables.GetRowDisplayRectangle(e.RowIndex, false);
            bounds.Width = dgvVariables.Columns.GetColumnsWidth(DataGridViewElementStates.Visible);
            bounds.Height--;
            DataGridViewCellStyle style = row.DefaultCellStyle;
            Color background = row.Selected ? dgvVariables.DefaultCellStyle.SelectionBackColor : style.BackColor;
            Color foreground = row.Selected ? style.SelectionForeColor : style.ForeColor;
            using (var brush = new SolidBrush(background)) e.Graphics.FillRectangle(brush, bounds);
            bounds.Inflate(-4, 0);
            string text = row.Cells[colValue.Index].Value?.ToString() ?? "";
            text = text.Replace("\r\n", " ⏎ ").Replace("\r", " ⏎ ").Replace("\n", " ⏎ ");
            TextRenderer.DrawText(e.Graphics, text, dgvVariables.Font, bounds, foreground,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine
                | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        private void dgvVariables_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            int index = dgvVariables.HitTest(e.X, e.Y).RowIndex;
            if (index < 0 || dgvVariables.Rows[index].Selected) return;
            dgvVariables.ClearSelection();
            dgvVariables.CurrentCell = dgvVariables.Rows[index].Cells[colValue.Index];
            dgvVariables.Rows[index].Selected = true;
        }

        private void dgvVariables_MouseUp(object sender, MouseEventArgs e)
        {
            if (dgvVariables.HitTest(e.X, e.Y).RowIndex >= 0) return;
            // Clear after the grid finishes its own mouse-down selection handling.
            dgvVariables.CurrentCell = null;
            dgvVariables.ClearSelection();
        }

        private void ctxVariables_Opening(object sender, CancelEventArgs e) => UpdateButtons();

        private void dgvVariables_KeyDown(object sender, KeyEventArgs e)
        {
            // Keep normal text clipboard shortcuts while a cell is being edited.
            if (dgvVariables.IsCurrentCellInEditMode) return;
            if (e.KeyData == (Keys.Control | Keys.C)) Copy_Click(sender, e);
            else if (e.KeyData == (Keys.Control | Keys.V)) Paste_Click(sender, e);
            else if (e.KeyData == Keys.Delete) Remove_Click(sender, e);
            else if (e.KeyData == (Keys.Control | Keys.Q)) Apply_Click(sender, e);
            else return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }
}
