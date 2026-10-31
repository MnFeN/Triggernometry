﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.Core.Variables;
using Triggernometry.Localization;

namespace Triggernometry.UI.Forms
{

    public partial class StateForm : MemoryForm<StateForm>
    {

        internal RealPlugin plug;

        public StateForm()
        {
            InitializeComponent();
            Shown += VariableForm_Shown;
            RestoredSavedDimensions();
            sortedVariableKeys = new List<string>();
            sortType = "name";
            sortAsc = true;
            dgvScalarVariables.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
            SetAllHeadersMinWidth(this);
            foreach (var grid in new[] { dgvListVariables, dgvPeListVariables, dgvDictVariables, dgvPeDictVariables })
                grid.Columns[2].CellTemplate = new VariablePreviewCell();
        }

        private void VariableForm_Shown(object sender, EventArgs e)
        {
            RefreshScalarVariables(plug.GetVariableStore(false), dgvScalarVariables);
        }

        private void tbcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            sortType = "name";
            sortAsc = true;
            switch (tbcMain.SelectedIndex)
            {
                case 0:
                    tbcScalar.SelectedIndex = 0;
                    dgvScalarVariables.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    RefreshScalarVariables(plug.GetVariableStore(false), dgvScalarVariables);
                    break;
                case 1:
                    tbcList.SelectedIndex = 0;
                    dgvListVariables.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    RefreshListVariables(plug.GetVariableStore(false), dgvListVariables);
                    break;
                case 2:
                    tbcTable.SelectedIndex = 0;
                    dgvTableVariables.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    RefreshTableVariables(plug.GetVariableStore(false), dgvTableVariables);
                    break;
                case 3:
                    tbcDict.SelectedIndex = 0;
                    dgvDictVariables.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    RefreshDictVariables(plug.GetVariableStore(false), dgvDictVariables);
                    break;
                case 4:
                    dgvRegisteredVariables.Columns[0].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                    RefreshRegisteredVariables();
                    break;
                case 5:
                    RefreshMutexes();
                    break;
                case 6:
                    RefreshImageAuras();
                    break;
                case 7:
                    RefreshTextAuras();
                    break;
                case 8:
                    RefreshNamedCallbacks();
                    break;
            }
        }

        private void tbcScalar_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tbcScalar.SelectedIndex)
            {
                case 0:
                    RefreshScalarVariables(plug.GetVariableStore(false), dgvScalarVariables);
                    break;
                case 1:
                    RefreshScalarVariables(plug.GetVariableStore(true), dgvPeScalarVariables);
                    break;
            }
        }

        private void tbcList_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tbcList.SelectedIndex)
            {
                case 0:
                    RefreshListVariables(plug.GetVariableStore(false), dgvListVariables);
                    break;
                case 1:
                    RefreshListVariables(plug.GetVariableStore(true), dgvPeListVariables);
                    break;
            }
        }

        private void tbcTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tbcTable.SelectedIndex)
            {
                case 0:
                    RefreshTableVariables(plug.GetVariableStore(false), dgvTableVariables);
                    break;
                case 1:
                    RefreshTableVariables(plug.GetVariableStore(true), dgvPeTableVariables);
                    break;
            }
        }

        private void tbcDict_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tbcDict.SelectedIndex)
            {
                case 0:
                    RefreshDictVariables(plug.GetVariableStore(false), dgvDictVariables);
                    break;
                case 1:
                    RefreshDictVariables(plug.GetVariableStore(true), dgvPeDictVariables);
                    break;
            }
        }

        #region Generic

        private void ctxState_Opening(object sender, CancelEventArgs e)
        {
            var dgv = ctxState.SourceControl as DataGridView;
            bool hasSource = dgv != null && dgv != dgvMutexes && dgv != dgvCallback;
            ctxLocateSource.Visible = hasSource;
            ctxLocateSource.Enabled = hasSource && dgv.SelectedRows.Count == 1;
        }

        private void ctxLocateSource_Click(object sender, EventArgs e)
        {
            if (!(ctxState.SourceControl is DataGridView dgv) || dgv.SelectedRows.Count != 1) return;
            string source = dgv.SelectedRows[0].Cells[dgv.ColumnCount - 1].Value?.ToString() ?? "";
            // Log names parenthesize the source ID; aura contexts also contain a separate execution GUID.
            Match match = Regex.Match(source, @"\((?<remote>@)?(?<id>[0-9a-fA-F]{8}(?:-[0-9a-fA-F]{4}){3}-[0-9a-fA-F]{12})\)");
            if (match.Success)
            {
                TreeNode root = plug.ui.treeView.Nodes[match.Groups["remote"].Success ? 1 : 0];
                TreeNode node = FindSourceNode(root, new Guid(match.Groups["id"].Value));
                if (node != null)
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    plug.ui.LocateTreeNode(node);
                    return;
                }
            }
            System.Media.SystemSounds.Hand.Play();
        }

        /// <summary> Finds a trigger or registered-variable folder by its source GUID within the selected tree. </summary>
        private TreeNode FindSourceNode(TreeNode node, Guid id)
        {
            if (node.Tag is Trigger trigger && trigger.Id == id || node.Tag is Folder folder && folder.Id == id) return node;
            foreach (TreeNode child in node.Nodes)
            {
                TreeNode found = FindSourceNode(child, id);
                if (found != null) return found;
            }
            return null;
        }

        private List<string> mutexKeys = new List<string>();
        private List<string> imageAuraKeys = new List<string>();
        private List<string> textAuraKeys = new List<string>();

        private List<string> sortedVariableKeys;
        private string sortType;
        private bool sortAsc;
        private static readonly IComparer<string> textSortComparer = StringComparer.Ordinal;
        private static readonly IComparer<Variable> variableSortComparer = Comparer<Variable>.Create(CompareVariableValues);

        /// <summary>保留排序键的原类型，并始终按名称或 ID 升序处理相同排序值。</summary>
        private List<TKey> SortKeys<TKey, TValue, TSort>(Dictionary<TKey, TValue> variables,
            Func<KeyValuePair<TKey, TValue>, TSort> selector)
        {
            lock (variables)
            {
                IComparer<TSort> comparer = typeof(TSort) == typeof(string) ? (IComparer<TSort>)textSortComparer
                    : typeof(Variable).IsAssignableFrom(typeof(TSort)) ? (IComparer<TSort>)variableSortComparer
                    : Comparer<TSort>.Default;
                IComparer<TKey> keyComparer = typeof(TKey) == typeof(string) ? (IComparer<TKey>)textSortComparer : Comparer<TKey>.Default;
                var ordered = sortAsc ? variables.OrderBy(selector, comparer) : variables.OrderByDescending(selector, comparer);
                return ordered.ThenBy(pair => pair.Key, keyComparer).Select(pair => pair.Key).ToList();
            }
        }

        /// <summary>变量值保留原有类型和大小优先级，内部文本统一按 Ordinal 比较。</summary>
        private static int CompareVariableValues(Variable left, Variable right)
        {
            if (ReferenceEquals(left, right)) return 0;
            if (left == null) return -1;
            if (right == null) return 1;
            if (left is VariableScalar leftScalar && right is VariableScalar rightScalar)
                return textSortComparer.Compare(leftScalar.Value, rightScalar.Value);
            if (left is VariableList leftList && right is VariableList rightList)
            {
                int result = leftList.Size.CompareTo(rightList.Size);
                if (result != 0) return result;
                for (int i = 0; i < leftList.Size; i++)
                {
                    result = CompareVariableValues(leftList.Values[i], rightList.Values[i]);
                    if (result != 0) return result;
                }
                return 0;
            }
            if (left is VariableDictionary leftDict && right is VariableDictionary rightDict)
            {
                int result = leftDict.Size.CompareTo(rightDict.Size);
                if (result != 0) return result;
                var leftKeys = leftDict.Values.Keys.OrderBy(key => key, textSortComparer).ToArray();
                var rightKeys = rightDict.Values.Keys.OrderBy(key => key, textSortComparer).ToArray();
                for (int i = 0; i < leftKeys.Length; i++)
                {
                    result = textSortComparer.Compare(leftKeys[i], rightKeys[i]);
                    if (result != 0) return result;
                    result = CompareVariableValues(leftDict.Values[leftKeys[i]], rightDict.Values[rightKeys[i]]);
                    if (result != 0) return result;
                }
                return 0;
            }
            if (left is VariableTable leftTable && right is VariableTable rightTable)
            {
                int result = leftTable.Height.CompareTo(rightTable.Height);
                if (result != 0) return result;
                result = leftTable.Width.CompareTo(rightTable.Width);
                if (result != 0) return result;
                for (int y = 1; y <= leftTable.Height; y++)
                    for (int x = 1; x <= leftTable.Width; x++)
                    {
                        result = CompareVariableValues(leftTable.Peek(x, y), rightTable.Peek(x, y));
                        if (result != 0) return result;
                    }
                return 0;
            }
            return left.CompareTo(right);
        }

        private const int PreviewTextLimit = 1024;

        /// <summary>只生成有限长度的预览，嵌套列表和字典也在达到上限时停止。</summary>
        private static string GetVariablePreview(Variable value)
        {
            string full = GetShortListPreview(value);
            if (full != null) return full;
            var text = new StringBuilder();
            AppendVariablePreview(text, value, true);
            if (text.Length > PreviewTextLimit)
            {
                text.Length = PreviewTextLimit;
                if (char.IsHighSurrogate(text[text.Length - 1])) text.Length--;
                text.Append('…');
            }
            return text.ToString();
        }

        /// <summary>能直接完整显示的短列表复用原拼接，避免增加预览分配。</summary>
        private static string GetShortListPreview(Variable value)
        {
            if (value is VariableList list && list.Size <= 32)
            {
                int length = Math.Max(0, list.Size - 1) * 2;
                foreach (Variable item in list.Values)
                {
                    if (item != null && item.GetType() != typeof(VariableScalar)) return null;
                    length += ((VariableScalar)item)?.Value?.Length ?? 0;
                    if (length > PreviewTextLimit) return null;
                }
                return list.Join(", ");
            }
            return null;
        }

        /// <summary>沿用原来的列表和字典分隔符，避免先构造完整的嵌套文本。</summary>
        private static void AppendVariablePreview(StringBuilder text, Variable value, bool isRoot = false)
        {
            if (text.Length > PreviewTextLimit) return;
            if (value is VariableList list && (isRoot || value.GetType() == typeof(VariableList)))
            {
                bool isFirst = true;
                foreach (Variable item in list.Values)
                {
                    if (!isFirst) AppendPreviewText(text, isRoot ? ", " : ",");
                    isFirst = false;
                    AppendVariablePreview(text, item);
                    if (text.Length > PreviewTextLimit) break;
                }
            }
            else if (value is VariableDictionary dict)
            {
                bool isFirst = true;
                foreach (var pair in dict.Values)
                {
                    if (!isFirst) AppendPreviewText(text, isRoot ? ", " : ",");
                    isFirst = false;
                    AppendPreviewText(text, pair.Key);
                    AppendPreviewText(text, isRoot ? ":" : "=");
                    AppendVariablePreview(text, pair.Value);
                    if (text.Length > PreviewTextLimit) break;
                }
            }
            else
            {
                // 自定义变量子类仍使用自己的 ToString；普通标量不产生额外的完整副本。
                AppendPreviewText(text, value?.ToString());
            }
        }

        /// <summary>最多保留一个超出上限的字符，用于判断是否需要省略号。</summary>
        private static void AppendPreviewText(StringBuilder text, string value)
        {
            if (value != null && text.Length <= PreviewTextLimit)
                text.Append(value, 0, Math.Min(value.Length, PreviewTextLimit + 1 - text.Length));
        }

        /// <summary>复制时才读取完整内容，不把界面中的省略号写入剪贴板。</summary>
        private string GetFullVariableText(DataGridView grid, int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= sortedVariableKeys.Count) return null;
            string key = sortedVariableKeys[rowIndex];
            var store = plug.GetVariableStore(grid == dgvPeListVariables || grid == dgvPeDictVariables);
            if (grid == dgvListVariables || grid == dgvPeListVariables)
            {
                lock (store.List)
                    return store.List.TryGetValue(key, out var list) ? list.Join(", ") : null;
            }
            lock (store.Dict)
                return store.Dict.TryGetValue(key, out var dict) ? dict.JoinAll(":", ", ") : null;
        }

        /// <summary>普通显示使用短预览；保留表格原有文本、CSV 和 HTML 的完整复制流程。</summary>
        private sealed class VariablePreviewCell : DataGridViewTextBoxCell
        {
            public VariablePreviewCell() { }

            protected override object GetFormattedValue(object value, int rowIndex,
                ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter,
                TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
            {
                if ((context & DataGridViewDataErrorContexts.ClipboardContent) != 0
                    && DataGridView?.FindForm() is StateForm form)
                    value = form.GetFullVariableText(DataGridView, rowIndex);
                return base.GetFormattedValue(value, rowIndex, ref cellStyle,
                    valueTypeConverter, formattedValueTypeConverter, context);
            }
        }

        private Variable OpenVariableEditor(Variable v, ref string name, bool isNew)
        {
            using (VariableEditorForm vef = new VariableEditorForm())
            {
                vef.VariableName = name;
                vef.VariableToEdit = v.Duplicate();
                vef.IsNew = isNew;
                if (vef.ShowDialog() == DialogResult.OK)
                {
                    name = vef.VariableName;
                    vef.VariableToEdit.LastChanged = DateTime.Now;
                    vef.VariableToEdit.LastChanger = I18n.Translate("internal/VariableForm/variableeditortag", "Variable editor");
                    return vef.VariableToEdit;
                }
            }
            return null;
        }

        private void UpdateSortType(string newType, DataGridView dgv, int colIndex)
        {
            if (sortType == newType)
            {
                sortAsc = !sortAsc;
            }
            else
            {
                sortType = newType;
                sortAsc = true;
            }
            // update sort glyph
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            dgv.Columns[colIndex].HeaderCell.SortGlyphDirection = (sortAsc) ? SortOrder.Ascending : SortOrder.Descending;
        }

        private void dgvCommon_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string newType = "";
            DataGridView dgv = sender as DataGridView;
            VariableStore vs = plug.GetVariableStore(
                dgv == dgvPeScalarVariables || 
                dgv == dgvPeListVariables ||
                dgv == dgvPeTableVariables || 
                dgv == dgvPeDictVariables);
            switch (dgv.Name)
            {
                case nameof(dgvScalarVariables):
                case nameof(dgvPeScalarVariables):
                    switch (e.ColumnIndex)
                    {
                        case 0: newType = "name"; break;
                        case 1: newType = "value"; break;
                        case 2: newType = "lastchanged"; break;
                        case 3: newType = "lastchanger"; break;
                    }
                    UpdateSortType(newType, dgv, e.ColumnIndex);
                    RefreshScalarVariables(vs, dgv);
                    break;
                case nameof(dgvListVariables):
                case nameof(dgvPeListVariables):
                    switch (e.ColumnIndex)
                    {
                        case 0: newType = "name"; break;
                        case 1: newType = "size"; break;
                        case 2: newType = "value"; break;
                        case 3: newType = "lastchanged"; break;
                        case 4: newType = "lastchanger"; break;
                    }
                    UpdateSortType(newType, dgv, e.ColumnIndex);
                    RefreshListVariables(vs, dgv);
                    break;
                case nameof(dgvTableVariables):
                case nameof(dgvPeTableVariables):
                    switch (e.ColumnIndex)
                    {
                        case 0: newType = "name"; break;
                        case 1: newType = "width"; break;
                        case 2: newType = "height"; break;
                        case 3: newType = "lastchanged"; break;
                        case 4: newType = "lastchanger"; break;
                    }
                    UpdateSortType(newType, dgv, e.ColumnIndex);
                    RefreshTableVariables(vs, dgv);
                    break;
                case nameof(dgvDictVariables):
                case nameof(dgvPeDictVariables):
                    switch (e.ColumnIndex)
                    {
                        case 0: newType = "name"; break;
                        case 1: newType = "size"; break;
                        case 2: return;
                        case 3: newType = "lastchanged"; break;
                        case 4: newType = "lastchanger"; break;
                    }
                    UpdateSortType(newType, dgv, e.ColumnIndex);
                    RefreshDictVariables(vs, dgv);
                    break;
                case nameof(dgvRegisteredVariables):
                    switch (e.ColumnIndex)
                    {
                        case 0: newType = "name"; break;
                        case 1: newType = "value"; break;
                        case 2: newType = "lastchanged"; break;
                        case 3: newType = "lastchanger"; break;
                    }
                    UpdateSortType(newType, dgv, e.ColumnIndex);
                    RefreshRegisteredVariables();
                    break;
                case nameof(dgvMutexes):
                case nameof(dgvImage):
                case nameof(dgvText):
                    break;
                case nameof(dgvCallback):
                    switch (e.ColumnIndex)
                    {
                        case 0: newType = "id"; break;
                        case 1: newType = "name"; break;
                        case 2: newType = "registrant"; break;
                        case 3: newType = "lastRegistered"; break;
                        case 4: newType = "lastInvoked"; break;
                    }
                    UpdateSortType(newType, dgv, e.ColumnIndex);
                    RefreshNamedCallbacks();
                    break;
                default:
                    return;
            }
        }

        private void SetHeadersMinWidth(DataGridView dgv)
        {
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                Size textSize = TextRenderer.MeasureText(column.HeaderText, dgv.Font);
                column.MinimumWidth = textSize.Width + TextRenderer.MeasureText("M", dgv.Font).Width * 3; //  margin and sorting glyph width 
            }
        }

        private void SetAllHeadersMinWidth(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                if (child is DataGridView dgv)
                {
                    SetHeadersMinWidth(dgv);
                }
                SetAllHeadersMinWidth(child);
            }
        }

        #endregion

        #region Scalar variables

        private void dgvScalarVariables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (btnScalarRemove.Enabled == true)
                {
                    btnScalarRemove_Click(this, null);
                }
            }
        }

        private void dgvPeScalarVariables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (btnPeScalarRemove.Enabled == true)
                {
                    btnPeScalarRemove_Click(this, null);
                }
            }
        }

        private void dgvScalarVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            dgvScalarVariables.ClearSelection();
            dgvScalarVariables.Rows[e.RowIndex].Selected = true;
            btnScalarEdit_Click(sender, null);
        }

        private void dgvPeScalarVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            dgvPeScalarVariables.ClearSelection();
            dgvPeScalarVariables.Rows[e.RowIndex].Selected = true;
            btnPeScalarEdit_Click(sender, null);
        }

        private void dgvScalarVariables_SelectionChanged(object sender, EventArgs e)
        {
            btnScalarEdit.Enabled = (dgvScalarVariables.SelectedRows.Count == 1);
            btnScalarRemove.Enabled = (dgvScalarVariables.SelectedRows.Count > 0);
        }

        private void dgvPeScalarVariables_SelectionChanged(object sender, EventArgs e)
        {
            btnPeScalarEdit.Enabled = (dgvPeScalarVariables.SelectedRows.Count == 1);
            btnPeScalarRemove.Enabled = (dgvPeScalarVariables.SelectedRows.Count > 0);
        }

        private void SortScalarVariables(VariableStore vs)
        {
            var variables = vs.Scalar;
            switch (sortType)
            {
                case "value":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value); break;
                case "lastchanged":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.LastChanged); break;
                case "lastchanger":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.LastChanger); break;
                default:
                    sortedVariableKeys = SortKeys(variables, pair => pair.Key); break;
            }
        }

        private void ScalarCellValueNeeded(VariableStore vs, DataGridViewCellValueEventArgs e)
        {
            lock (vs.Scalar)
            {
                if (e.RowIndex < 0 || e.RowIndex >= sortedVariableKeys.Count) 
                    return;

                string key = sortedVariableKeys[e.RowIndex];
                if (!vs.Scalar.TryGetValue(key, out var varValue)) 
                    return;

                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = key;
                        break;
                    case 1:
                        e.Value = varValue.Value;
                        break;
                    case 2:
                        e.Value = varValue.LastChanged.ToString();
                        break;
                    case 3:
                        e.Value = varValue.LastChanger;
                        break;
                }
            }
        }

        private void dgvScalarVariables_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            ScalarCellValueNeeded(plug.GetVariableStore(false), e);
        }

        private void dgvPeScalarVariables_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            ScalarCellValueNeeded(plug.GetVariableStore(true), e);
        }

        private void RefreshScalarVariables(VariableStore vs, DataGridView dgv)
        {
            lock (vs.Scalar)
            {
                dgv.RowCount = vs.Scalar.Count;
                SortScalarVariables(vs);
            }
            Refresh();
        }

        private void AddScalarVariable(VariableStore vs, DataGridView dgv)
        {
            VariableScalar v = new VariableScalar();
            string varname = "";
            v = (VariableScalar)OpenVariableEditor(v, ref varname, true);
            if (v != null)
            {
                lock (vs.Scalar)
                {
                    vs.Scalar[varname] = v;
                }
                RefreshScalarVariables(vs, dgv);
            }
        }

        private void EditScalarVariable(VariableStore vs, DataGridView dgv)
        {
            string varname = "";
            foreach (DataGridViewRow r in dgv.SelectedRows)
            {
                varname = r.Cells[0].Value.ToString();
            }
            VariableScalar v = null;
            lock (vs.Scalar)
            {
                if (vs.Scalar.ContainsKey(varname) == true)
                {
                    v = vs.Scalar[varname];
                }
            }
            if (v == null)
            {
                v = new VariableScalar();
            }
            string varname2 = varname;
            v = (VariableScalar)OpenVariableEditor(v, ref varname2, false);
            if (v != null)
            {
                lock (vs.Scalar)
                {
                    if (varname != varname2)
                    {
                        if (vs.Scalar.ContainsKey(varname) == true)
                        {
                            vs.Scalar.Remove(varname);
                        }
                    }
                    vs.Scalar[varname2] = v;
                }
                RefreshScalarVariables(vs, dgv);
            }
        }

        private void RemoveScalarVariable(VariableStore vs, DataGridView dgv)
        {
            string temp;
            if (dgv.SelectedRows.Count > 1)
            {
                temp = I18n.Translate("internal/VariableForm/areyousureplural", "Are you sure you want to remove the selected variables?");
            }
            else
            {
                temp = I18n.Translate("internal/VariableForm/areyousuresingular", "Are you sure you want to remove the selected variable?");
            }
            switch (MessageBox.Show(this, temp, I18n.Translate("internal/VariableForm/confirmremoval", "Confirm removal"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                case DialogResult.Yes:
                    List<string> varnames = new List<string>();
                    foreach (DataGridViewRow r in dgv.SelectedRows)
                    {
                        varnames.Add(r.Cells[0].Value.ToString());
                    }
                    lock (vs.Scalar)
                    {
                        foreach (string varname in varnames)
                        {   // Deleted the key check: Remove() would check if the key exists
                            vs.Scalar.Remove(varname); 
                        }
                        dgv.RowCount = vs.Scalar.Count;
                    }
                    dgv.ClearSelection();
                    RefreshScalarVariables(vs, dgv);
                    break;
            }
        }

        private void RemoveAllScalarVariables(VariableStore vs, DataGridView dgv)
        {
            lock (vs.Scalar)
            {
                vs.Scalar.Clear();
            }
            RefreshScalarVariables(vs, dgv);
        }

        private void btnScalarAdd_Click(object sender, EventArgs e)
        {
            AddScalarVariable(plug.GetVariableStore(false), dgvScalarVariables);
        }

        private void btnScalarEdit_Click(object sender, EventArgs e)
        {
            EditScalarVariable(plug.GetVariableStore(false), dgvScalarVariables);
        }

        private void btnScalarRemove_Click(object sender, EventArgs e)
        {
            RemoveScalarVariable(plug.GetVariableStore(false), dgvScalarVariables);
        }

        private void btnRefreshScalar_ButtonClick(object sender, EventArgs e)
        {
            RefreshScalarVariables(plug.GetVariableStore(false), dgvScalarVariables);
        }

        private void btnRemoveAllScalar_Click(object sender, EventArgs e)
        {
            RemoveAllScalarVariables(plug.GetVariableStore(false), dgvScalarVariables);
        }

        private void btnPeScalarAdd_Click(object sender, EventArgs e)
        {
            AddScalarVariable(plug.GetVariableStore(true), dgvPeScalarVariables);
        }

        private void btnPeScalarEdit_Click(object sender, EventArgs e)
        {
            EditScalarVariable(plug.GetVariableStore(true), dgvPeScalarVariables);
        }

        private void btnPeScalarRemove_Click(object sender, EventArgs e)
        {
            RemoveScalarVariable(plug.GetVariableStore(true), dgvPeScalarVariables);
        }

        private void btnPeScalarRefresh_ButtonClick(object sender, EventArgs e)
        {
            RefreshScalarVariables(plug.GetVariableStore(true), dgvPeScalarVariables);
        }

        private void btnPeScalarRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveAllScalarVariables(plug.GetVariableStore(true), dgvPeScalarVariables);
        }

        #endregion

        #region List variables

        private void dgvListVariables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (btnListRemove.Enabled == true)
                {
                    btnListRemove_Click(this, null);
                }
            }
        }

        private void dgvPeListVariables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (btnPeListRemove.Enabled == true)
                {
                    btnPeListRemove_Click(this, null);
                }
            }
        }

        private void dgvListVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            dgvListVariables.ClearSelection();
            dgvListVariables.Rows[e.RowIndex].Selected = true;
            btnListEdit_Click(sender, null);
        }

        private void dgvPeListVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            dgvPeListVariables.ClearSelection();
            dgvPeListVariables.Rows[e.RowIndex].Selected = true;
            btnPeListEdit_Click(sender, null);
        }

        private void SortListVariables(VariableStore vs)
        {
            var variables = vs.List;
            switch (sortType)
            {
                case "size":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.Size); break;
                case "value":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value); break;
                case "lastchanged":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.LastChanged); break;
                case "lastchanger":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.LastChanger); break;
                default:
                    sortedVariableKeys = SortKeys(variables, pair => pair.Key); break;
            }
        }

        private void ListCellValueNeeded(VariableStore vs, DataGridViewCellValueEventArgs e)
        {
            lock (vs.List)
            {
                if (e.RowIndex < 0 || e.RowIndex >= sortedVariableKeys.Count) 
                    return;

                string key = sortedVariableKeys[e.RowIndex];
                if (!vs.List.TryGetValue(key, out var varValue)) 
                    return;

                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = key;
                        break;
                    case 1:
                        e.Value = varValue.Size;
                        break;
                    case 2:
                        e.Value = GetVariablePreview(varValue);
                        break;
                    case 3:
                        e.Value = varValue.LastChanged.ToString();
                        break;
                    case 4:
                        e.Value = varValue.LastChanger;
                        break;
                }
            }
        }

        private void dgvListVariables_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            ListCellValueNeeded(plug.GetVariableStore(false), e);
        }

        private void dgvPeListVariables_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            ListCellValueNeeded(plug.GetVariableStore(true), e);
        }

        private void dgvListVariables_SelectionChanged(object sender, EventArgs e)
        {
            btnListEdit.Enabled = (dgvListVariables.SelectedRows.Count == 1);
            btnListRemove.Enabled = (dgvListVariables.SelectedRows.Count > 0);
        }

        private void dgvPeListVariables_SelectionChanged(object sender, EventArgs e)
        {
            btnPeListEdit.Enabled = (dgvPeListVariables.SelectedRows.Count == 1);
            btnPeListRemove.Enabled = (dgvPeListVariables.SelectedRows.Count > 0);
        }

        private void RefreshListVariables(VariableStore vs, DataGridView dgv)
        {
            lock (vs.List)
            {
                dgv.RowCount = vs.List.Count;
            }
            SortListVariables(vs);
            Refresh();
        }

        private void AddListVariable(VariableStore vs, DataGridView dgv)
        {
            VariableList v = new VariableList();
            string varname = "";
            v = (VariableList)OpenVariableEditor(v, ref varname, true);
            if (v != null)
            {
                lock (vs.List)
                {
                    vs.List[varname] = v;
                }
                RefreshListVariables(vs, dgv);
            }
        }

        private void EditListVariable(VariableStore vs, DataGridView dgv)
        {
            string varname = "";
            foreach (DataGridViewRow r in dgv.SelectedRows)
            {
                varname = r.Cells[0].Value.ToString();
            }
            VariableList v = null;
            lock (vs.List)
            {
                if (vs.List.ContainsKey(varname) == true)
                {
                    v = vs.List[varname];
                }
            }
            if (v == null)
            {
                v = new VariableList();
            }
            string varname2 = varname;
            v = (VariableList)OpenVariableEditor(v, ref varname2, false);
            if (v != null)
            {
                lock (vs.List)
                {
                    if (varname != varname2)
                    {
                        if (vs.List.ContainsKey(varname) == true)
                        {
                            vs.List.Remove(varname);
                        }
                    }
                    vs.List[varname2] = v;
                }
                RefreshListVariables(vs, dgv);
            }
        }

        private void RemoveListVariable(VariableStore vs, DataGridView dgv)
        {
            string temp;
            if (dgv.SelectedRows.Count > 1)
            {
                temp = I18n.Translate("internal/VariableForm/areyousureplural", "Are you sure you want to remove the selected variables?");
            }
            else
            {
                temp = I18n.Translate("internal/VariableForm/areyousuresingular", "Are you sure you want to remove the selected variable?");
            }
            switch (MessageBox.Show(this, temp, I18n.Translate("internal/VariableForm/confirmremoval", "Confirm removal"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                case DialogResult.Yes:
                    List<string> varnames = new List<string>();
                    foreach (DataGridViewRow r in dgv.SelectedRows)
                    {
                        varnames.Add(r.Cells[0].Value.ToString());
                    }
                    lock (vs.List)
                    {
                        foreach (string varname in varnames)
                        {
                            vs.List.Remove(varname);
                        }
                        dgv.RowCount = vs.List.Count;
                    }
                    dgv.ClearSelection();
                    RefreshListVariables(vs, dgv);
                    break;
            }
        }

        private void RemoveAllListVariables(VariableStore vs, DataGridView dgv)
        {
            lock (vs.List)
            {
                vs.List.Clear();
            }
            RefreshListVariables(vs, dgv);
        }

        private void btnListAdd_Click(object sender, EventArgs e)
        {
            AddListVariable(plug.GetVariableStore(false), dgvListVariables);
        }

        private void btnListEdit_Click(object sender, EventArgs e)
        {
            EditListVariable(plug.GetVariableStore(false), dgvListVariables);
        }

        private void btnListRemove_Click(object sender, EventArgs e)
        {
            RemoveListVariable(plug.GetVariableStore(false), dgvListVariables);
        }

        private void btnListRefresh_ButtonClick(object sender, EventArgs e)
        {
            RefreshListVariables(plug.GetVariableStore(false), dgvListVariables);
        }

        private void btnListRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveAllListVariables(plug.GetVariableStore(false), dgvListVariables);
        }

        private void btnPeListAdd_Click(object sender, EventArgs e)
        {
            AddListVariable(plug.GetVariableStore(true), dgvPeListVariables);
        }

        private void btnPeListEdit_Click(object sender, EventArgs e)
        {
            EditListVariable(plug.GetVariableStore(true), dgvPeListVariables);
        }

        private void btnPeListRemove_Click(object sender, EventArgs e)
        {
            RemoveListVariable(plug.GetVariableStore(true), dgvPeListVariables);
        }

        private void btnPeListRefresh_ButtonClick(object sender, EventArgs e)
        {
            RefreshListVariables(plug.GetVariableStore(true), dgvPeListVariables);
        }

        private void btnPeListRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveAllListVariables(plug.GetVariableStore(true), dgvPeListVariables);
        }

        #endregion

        #region Table variables

        private void dgvTableVariables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (btnTableRemove.Enabled == true)
                {
                    btnTableRemove_Click(this, null);
                }
            }
        }

        private void dgvPeTableVariables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (btnPeTableRemove.Enabled == true)
                {
                    btnPeTableRemove_Click(this, null);
                }
            }
        }

        private void dgvTableVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            dgvTableVariables.ClearSelection();
            dgvTableVariables.Rows[e.RowIndex].Selected = true;
            btnTableEdit_Click(sender, null);
        }

        private void dgvPeTableVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            dgvPeTableVariables.ClearSelection();
            dgvPeTableVariables.Rows[e.RowIndex].Selected = true;
            btnPeTableEdit_Click(sender, null);
        }

        private void SortTableVariables(VariableStore vs)
        {
            var variables = vs.Table;
            switch (sortType)
            {
                case "width":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.Width); break;
                case "height":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.Height); break;
                case "lastchanged":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.LastChanged); break;
                case "lastchanger":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.LastChanger); break;
                default:
                    sortedVariableKeys = SortKeys(variables, pair => pair.Key); break;
            }
        }

        private void TableCellValueNeeded(VariableStore vs, DataGridViewCellValueEventArgs e)
        {
            lock (vs.Table)
            {
                if (e.RowIndex < 0 || e.RowIndex >= sortedVariableKeys.Count) 
                    return;

                string key = sortedVariableKeys[e.RowIndex];
                if (!vs.Table.TryGetValue(key, out var varValue)) 
                    return;

                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = key;
                        break;
                    case 1:
                        e.Value = varValue.Width;
                        break;
                    case 2:
                        e.Value = varValue.Height;
                        break;
                    case 3:
                        e.Value = varValue.LastChanged.ToString();
                        break;
                    case 4:
                        e.Value = varValue.LastChanger;
                        break;
                }
            }
        }

        private void dgvTableVariables_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            TableCellValueNeeded(plug.GetVariableStore(false), e);
        }

        private void dgvPeTableVariables_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            TableCellValueNeeded(plug.GetVariableStore(true), e);
        }

        private void dgvTableVariables_SelectionChanged(object sender, EventArgs e)
        {
            btnTableEdit.Enabled = (dgvTableVariables.SelectedRows.Count == 1);
            btnTableRemove.Enabled = (dgvTableVariables.SelectedRows.Count > 0);
        }

        private void dgvPeTableVariables_SelectionChanged(object sender, EventArgs e)
        {
            btnPeTableEdit.Enabled = (dgvPeTableVariables.SelectedRows.Count == 1);
            btnPeTableRemove.Enabled = (dgvPeTableVariables.SelectedRows.Count > 0);
        }

        private void RefreshTableVariables(VariableStore vs, DataGridView dgv)
        {
            lock (vs.Table)
            {
                dgv.RowCount = vs.Table.Count;
                SortTableVariables(vs);
            }
            Refresh();
        }

        private void AddTableVariable(VariableStore vs, DataGridView dgv)
        {
            VariableTable v = new VariableTable();
            string varname = "";
            v = (VariableTable)OpenVariableEditor(v, ref varname, true);
            if (v != null)
            {
                lock (vs.Table)
                {
                    vs.Table[varname] = v;
                }
                RefreshTableVariables(vs, dgv);
            }
        }

        private void EditTableVariable(VariableStore vs, DataGridView dgv)
        {
            string varname = "";
            foreach (DataGridViewRow r in dgv.SelectedRows)
            {
                varname = r.Cells[0].Value.ToString();
            }
            VariableTable v = null;
            lock (vs.Table)
            {
                if (vs.Table.ContainsKey(varname) == true)
                {
                    v = vs.Table[varname];
                }
            }
            if (v == null)
            {
                v = new VariableTable();
            }
            string varname2 = varname;
            v = (VariableTable)OpenVariableEditor(v, ref varname2, false);
            if (v != null)
            {
                lock (vs.Table)
                {
                    if (varname != varname2)
                    {
                        if (vs.Table.ContainsKey(varname) == true)
                        {
                            vs.Table.Remove(varname);
                        }
                    }
                    vs.Table[varname2] = v;
                }
                RefreshTableVariables(vs, dgv);
            }
        }

        private void RemoveTableVariable(VariableStore vs, DataGridView dgv)
        {
            string temp;
            if (dgv.SelectedRows.Count > 1)
            {
                temp = I18n.Translate("internal/VariableForm/areyousureplural", "Are you sure you want to remove the selected variables?");
            }
            else
            {
                temp = I18n.Translate("internal/VariableForm/areyousuresingular", "Are you sure you want to remove the selected variable?");
            }
            switch (MessageBox.Show(this, temp, I18n.Translate("internal/VariableForm/confirmremoval", "Confirm removal"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                case DialogResult.Yes:
                    List<string> varnames = new List<string>();
                    foreach (DataGridViewRow r in dgv.SelectedRows)
                    {
                        varnames.Add(r.Cells[0].Value.ToString());
                    }
                    lock (vs.Table)
                    {
                        foreach (string varname in varnames)
                        {
                            vs.Table.Remove(varname);
                        }
                        dgv.RowCount = vs.Table.Count;
                    }
                    dgv.ClearSelection();
                    RefreshTableVariables(vs, dgv);
                    break;
            }
        }

        private void RemoveAllTableVariables(VariableStore vs, DataGridView dgv)
        {
            lock (vs.Table)
            {
                vs.Table.Clear();
            }
            RefreshTableVariables(vs, dgv);
        }

        private void btnTableAdd_Click(object sender, EventArgs e)
        {
            AddTableVariable(plug.GetVariableStore(false), dgvTableVariables);
        }

        private void btnTableEdit_Click(object sender, EventArgs e)
        {
            EditTableVariable(plug.GetVariableStore(false), dgvTableVariables);
        }

        private void btnTableRemove_Click(object sender, EventArgs e)
        {
            RemoveTableVariable(plug.GetVariableStore(false), dgvTableVariables);
        }

        private void btnTableRefresh_ButtonClick(object sender, EventArgs e)
        {
            RefreshTableVariables(plug.GetVariableStore(false), dgvTableVariables);
        }

        private void btnTableRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveAllTableVariables(plug.GetVariableStore(false), dgvTableVariables);
        }

        private void btnPeTableAdd_Click(object sender, EventArgs e)
        {
            AddTableVariable(plug.GetVariableStore(true), dgvPeTableVariables);
        }

        private void btnPeTableEdit_Click(object sender, EventArgs e)
        {
            EditTableVariable(plug.GetVariableStore(true), dgvPeTableVariables);
        }

        private void btnPeTableRemove_Click(object sender, EventArgs e)
        {
            RemoveTableVariable(plug.GetVariableStore(true), dgvPeTableVariables);
        }

        private void btnPeTableRefresh_ButtonClick(object sender, EventArgs e)
        {
            RefreshTableVariables(plug.GetVariableStore(true), dgvPeTableVariables);
        }

        private void btnPeTableRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveAllTableVariables(plug.GetVariableStore(true), dgvPeTableVariables);
        }

        #endregion

        #region Dict variables

        private void dgvDictVariables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (btnDictRemove.Enabled == true)
                {
                    btnDictRemove_Click(this, null);
                }
            }
        }

        private void dgvPeDictVariables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (btnPeDictRemove.Enabled == true)
                {
                    btnPeDictRemove_Click(this, null);
                }
            }
        }

        private void dgvDictVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            dgvDictVariables.ClearSelection();
            dgvDictVariables.Rows[e.RowIndex].Selected = true;
            btnDictEdit_Click(sender, null);
        }

        private void dgvPeDictVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            dgvPeDictVariables.ClearSelection();
            dgvPeDictVariables.Rows[e.RowIndex].Selected = true;
            btnPeDictEdit_Click(sender, null);
        }

        private void SortDictVariables(VariableStore vs)
        {
            var variables = vs.Dict;
            switch (sortType)
            {
                case "size":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.Size); break;
                case "lastchanged":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.LastChanged); break;
                case "lastchanger":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.LastChanger); break;
                default:
                    sortedVariableKeys = SortKeys(variables, pair => pair.Key); break;
            }
        }

        private void DictCellValueNeeded(VariableStore vs, DataGridViewCellValueEventArgs e)
        {
            lock (vs.Dict)
            {
                // #83：索引保护必须在访问键列表之前，取值只查一次字典。
                if (e.RowIndex < 0 || e.RowIndex >= sortedVariableKeys.Count) 
                    return;

                string key = sortedVariableKeys[e.RowIndex];
                if (!vs.Dict.TryGetValue(key, out var varDict)) 
                    return;

                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = key;
                        break;
                    case 1:
                        e.Value = varDict.Size;
                        break;
                    case 2:
                        e.Value = GetVariablePreview(varDict);
                        break;
                    case 3:
                        e.Value = varDict.LastChanged.ToString();
                        break;
                    case 4:
                        e.Value = varDict.LastChanger;
                        break;
                }
            }
        }

        private void dgvDictVariables_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            DictCellValueNeeded(plug.GetVariableStore(false), e);
        }

        private void dgvPeDictVariables_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            DictCellValueNeeded(plug.GetVariableStore(true), e);
        }

        private void dgvDictVariables_SelectionChanged(object sender, EventArgs e)
        {
            btnDictEdit.Enabled = (dgvDictVariables.SelectedRows.Count == 1);
            btnDictRemove.Enabled = (dgvDictVariables.SelectedRows.Count > 0);
        }

        private void dgvPeDictVariables_SelectionChanged(object sender, EventArgs e)
        {
            btnPeDictEdit.Enabled = (dgvPeDictVariables.SelectedRows.Count == 1);
            btnPeDictRemove.Enabled = (dgvPeDictVariables.SelectedRows.Count > 0);
        }

        private void RefreshDictVariables(VariableStore vs, DataGridView dgv)
        {
            lock (vs.Dict)
            {
                dgv.RowCount = vs.Dict.Count;
                SortDictVariables(vs);
            }
            Refresh();
        }

        private void AddDictVariable(VariableStore vs, DataGridView dgv)
        {
            VariableDictionary v = new VariableDictionary();
            string varname = "";
            v = (VariableDictionary)OpenVariableEditor(v, ref varname, true);
            if (v != null)
            {
                lock (vs.Dict)
                {
                    vs.Dict[varname] = v;
                }
                RefreshDictVariables(vs, dgv);
            }
        }

        private void EditDictVariable(VariableStore vs, DataGridView dgv)
        {
            string varname = "";
            foreach (DataGridViewRow r in dgv.SelectedRows)
            {
                varname = r.Cells[0].Value.ToString();
            }
            VariableDictionary v = null;
            lock (vs.Dict)
            {
                if (vs.Dict.ContainsKey(varname) == true)
                {
                    v = vs.Dict[varname];
                }
            }
            if (v == null)
            {
                v = new VariableDictionary();
            }
            string varname2 = varname;
            v = (VariableDictionary)OpenVariableEditor(v, ref varname2, false);
            if (v != null)
            {
                lock (vs.Dict)
                {
                    if (varname != varname2)
                    {
                        if (vs.Dict.ContainsKey(varname) == true)
                        {
                            vs.Dict.Remove(varname);
                        }
                    }
                    vs.Dict[varname2] = v;
                }
                RefreshDictVariables(vs, dgv);
            }
        }

        private void RemoveDictVariable(VariableStore vs, DataGridView dgv)
        {
            string temp;
            if (dgv.SelectedRows.Count > 1)
            {
                temp = I18n.Translate("internal/VariableForm/areyousureplural", "Are you sure you want to remove the selected variables?");
            }
            else
            {
                temp = I18n.Translate("internal/VariableForm/areyousuresingular", "Are you sure you want to remove the selected variable?");
            }
            switch (MessageBox.Show(this, temp, I18n.Translate("internal/VariableForm/confirmremoval", "Confirm removal"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                case DialogResult.Yes:
                    List<string> varnames = new List<string>();
                    foreach (DataGridViewRow r in dgv.SelectedRows)
                    {
                        varnames.Add(r.Cells[0].Value.ToString());
                    }
                    lock (vs.Dict)
                    {
                        foreach (string varname in varnames)
                        {
                            vs.Dict.Remove(varname);
                        }
                        dgv.RowCount = vs.Dict.Count;
                    }
                    dgv.ClearSelection();
                    RefreshDictVariables(vs, dgv);
                    break;
            }
        }

        private void RemoveAllDictVariables(VariableStore vs, DataGridView dgv)
        {
            lock (vs.Dict)
            {
                vs.Dict.Clear();
            }
            RefreshDictVariables(vs, dgv);
        }

        private void btnDictAdd_Click(object sender, EventArgs e)
        {
            AddDictVariable(plug.GetVariableStore(false), dgvDictVariables);
        }

        private void btnDictEdit_Click(object sender, EventArgs e)
        {
            EditDictVariable(plug.GetVariableStore(false), dgvDictVariables);
        }

        private void btnDictRemove_Click(object sender, EventArgs e)
        {
            RemoveDictVariable(plug.GetVariableStore(false), dgvDictVariables);
        }

        private void btnDictRefresh_ButtonClick(object sender, EventArgs e)
        {
            RefreshDictVariables(plug.GetVariableStore(false), dgvDictVariables);
        }

        private void btnDictRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveAllDictVariables(plug.GetVariableStore(false), dgvDictVariables);
        }

        private void btnPeDictAdd_Click(object sender, EventArgs e)
        {
            AddDictVariable(plug.GetVariableStore(true), dgvPeDictVariables);
        }

        private void btnPeDictEdit_Click(object sender, EventArgs e)
        {
            EditDictVariable(plug.GetVariableStore(true), dgvPeDictVariables);
        }

        private void btnPeDictRemove_Click(object sender, EventArgs e)
        {
            RemoveDictVariable(plug.GetVariableStore(true), dgvPeDictVariables);
        }

        private void btnPeDictRefresh_ButtonClick(object sender, EventArgs e)
        {
            RefreshDictVariables(plug.GetVariableStore(true), dgvPeDictVariables);
        }

        private void btnPeDictRemoveAll_Click(object sender, EventArgs e)
        {
            RemoveAllDictVariables(plug.GetVariableStore(true), dgvPeDictVariables);
        }

        #endregion

        #region Registered variables

        private void dgvRegisteredVariables_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (btnRegisteredRemove.Enabled == true)
                {
                    btnRegisteredRemove_Click(this, null);
                }
            }
        }

        private void dgvRegisteredVariables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            dgvRegisteredVariables.ClearSelection();
            dgvRegisteredVariables.Rows[e.RowIndex].Selected = true;
            btnRegisteredEdit_Click(sender, null);
        }

        private void dgvRegisteredVariables_SelectionChanged(object sender, EventArgs e)
        {
            btnRegisteredEdit.Enabled = (dgvRegisteredVariables.SelectedRows.Count == 1);
            btnRegisteredRemove.Enabled = (dgvRegisteredVariables.SelectedRows.Count > 0);
        }

        private void SortRegisteredVariables()
        {
            var variables = plug.RegisteredVariables;
            switch (sortType)
            {
                case "value":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value); break;
                case "lastchanged":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.LastChanged); break;
                case "lastchanger":
                    sortedVariableKeys = SortKeys(variables, pair => pair.Value.LastChanger); break;
                default:
                    sortedVariableKeys = SortKeys(variables, pair => pair.Key); break;
            }
        }

        private void dgvRegisteredVariables_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            lock (plug.RegisteredVariables)
            {
                if (e.RowIndex < 0 || e.RowIndex >= sortedVariableKeys.Count) 
                    return;

                string key = sortedVariableKeys[e.RowIndex];
                // Keep the displayed key available if its runtime value was removed.
                if (e.ColumnIndex == 0)
                {
                    e.Value = key;
                    return;
                }
                if (!plug.RegisteredVariables.TryGetValue(key, out VariableScalar varValue)) return;
                switch (e.ColumnIndex)
                {
                    case 1:
                        e.Value = varValue.Value;
                        break;
                    case 2:
                        e.Value = varValue.LastChanged.ToString();
                        break;
                    case 3:
                        e.Value = varValue.LastChanger;
                        break;
                }
            }
        }

        private void RefreshRegisteredVariables()
        {
            lock (plug.RegisteredVariables)
            {
                SortRegisteredVariables();
                dgvRegisteredVariables.RowCount = plug.RegisteredVariables.Count;
            }
            Refresh();
        }

        // Runtime edits use the scalar editor without changing folder declarations.
        private void AddRegisteredVariable()
        {
            VariableScalar v = new VariableScalar();
            string varname = "";
            v = (VariableScalar)OpenVariableEditor(v, ref varname, true);
            if (v != null)
            {
                lock (plug.RegisteredVariables)
                {
                    plug.RegisteredVariables[varname] = v;
                }
                RefreshRegisteredVariables();
            }
        }

        private void EditRegisteredVariable()
        {
            string varname = "";
            foreach (DataGridViewRow r in dgvRegisteredVariables.SelectedRows)
            {
                varname = r.Cells[0].Value.ToString();
            }
            VariableScalar v = null;
            lock (plug.RegisteredVariables)
            {
                if (plug.RegisteredVariables.ContainsKey(varname) == true)
                {
                    v = plug.RegisteredVariables[varname];
                }
            }
            if (v == null)
            {
                v = new VariableScalar();
            }
            string varname2 = varname;
            v = (VariableScalar)OpenVariableEditor(v, ref varname2, false);
            if (v != null)
            {
                lock (plug.RegisteredVariables)
                {
                    if (varname != varname2)
                    {
                        if (plug.RegisteredVariables.ContainsKey(varname) == true)
                        {
                            plug.RegisteredVariables.Remove(varname);
                        }
                    }
                    plug.RegisteredVariables[varname2] = v;
                }
                RefreshRegisteredVariables();
            }
        }

        private void RemoveRegisteredVariable()
        {
            string temp;
            if (dgvRegisteredVariables.SelectedRows.Count > 1)
            {
                temp = I18n.Translate("internal/VariableForm/areyousureplural", "Are you sure you want to remove the selected variables?");
            }
            else
            {
                temp = I18n.Translate("internal/VariableForm/areyousuresingular", "Are you sure you want to remove the selected variable?");
            }
            switch (MessageBox.Show(this, temp, I18n.Translate("internal/VariableForm/confirmremoval", "Confirm removal"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                case DialogResult.Yes:
                    List<string> varnames = new List<string>();
                    foreach (DataGridViewRow r in dgvRegisteredVariables.SelectedRows)
                    {
                        varnames.Add(r.Cells[0].Value.ToString());
                    }
                    lock (plug.RegisteredVariables)
                    {
                        foreach (string varname in varnames)
                        {
                            plug.RegisteredVariables.Remove(varname);
                        }
                    }
                    dgvRegisteredVariables.ClearSelection();
                    RefreshRegisteredVariables();
                    break;
            }
        }

        private void RemoveAllRegisteredVariables()
        {
            lock (plug.RegisteredVariables)
            {
                plug.RegisteredVariables.Clear();
            }
            RefreshRegisteredVariables();
        }

        private void btnRegisteredAdd_Click(object sender, EventArgs e)
        {
            AddRegisteredVariable();
        }

        private void btnRegisteredEdit_Click(object sender, EventArgs e)
        {
            EditRegisteredVariable();
        }

        private void btnRegisteredRemove_Click(object sender, EventArgs e)
        {
            RemoveRegisteredVariable();
        }

        private void btnRefreshRegistered_ButtonClick(object sender, EventArgs e)
        {
            RefreshRegisteredVariables();
        }

        private void btnRemoveAllRegistered_Click(object sender, EventArgs e)
        {
            RemoveAllRegisteredVariables();
        }

        #endregion

        #region Mutexes

        private void dgvMutexes_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            lock (plug.mutexes)
            {
                if (e.RowIndex < 0 || e.RowIndex >= mutexKeys.Count) return;
                string key = mutexKeys[e.RowIndex];
                if (e.ColumnIndex == 0)
                {
                    e.Value = key;
                    return;
                }
                if (!plug.mutexes.TryGetValue(key, out var mutex)) return;
                lock (mutex)
                {
                    switch (e.ColumnIndex)
                    {
                        case 1:
                            e.Value = mutex.OwnerContext != null ? mutex.OwnerContext.ToString() : "Not owned";
                            break;
                        case 2:
                            e.Value = mutex.RefCount.ToString();
                            break;
                        case 3:
                            e.Value = string.Join(", ", mutex.AcquireQueue.Select(ticket => ticket.Context.ToString()));
                            break;
                    }
                }
            }
        }

        private void RefreshMutexes()
        {
            lock (plug.mutexes)
            {
                mutexKeys = plug.mutexes.Keys.ToList();
                dgvMutexes.RowCount = mutexKeys.Count;
            }
            Refresh();
        }

        private void btnMutexRefresh_Click(object sender, EventArgs e)
        {
            RefreshMutexes();
        }

        private void btnMutexForce_Click(object sender, EventArgs e)
        {
            List<string> mnames = new List<string>();
            foreach (DataGridViewRow r in dgvMutexes.SelectedRows)
            {
                mnames.Add(r.Cells[0].Value.ToString());
            }
            lock (plug.mutexes)
            {
                foreach (string mname in mnames)
                {
                    if (plug.mutexes.ContainsKey(mname) == true)
                    {
                        plug.mutexes[mname].ForceRelease();
                    }
                }                
            }
            btnMutexRefresh_Click(sender, e);
        }

        private void dgvMutexes_SelectionChanged(object sender, EventArgs e)
        {
            btnMutexForce.Enabled = (dgvMutexes.SelectedRows.Count > 0);
        }

        #endregion

        /// <summary>由绘图线程关闭悬浮窗，整批完成后再通知界面刷新。</summary>
        private void QueueAuraClosures(List<string> names, Scarborough.Scarborough.ItemAction.ItemTypeEnum type, Action refresh)
        {
            if (names.Count == 0) return;
            Action completed = () =>
            {
                if (IsDisposed || !IsHandleCreated) return;
                try { BeginInvoke(refresh); }
                catch (InvalidOperationException) { } // 窗体可能在提交刷新前已经关闭。
            };
            for (int i = 0; i < names.Count; i++)
                plug.sc.Deactivate(names[i], type, i == names.Count - 1 ? completed : null);
        }

        #region Image auras
        private void RefreshImageAuras()
        {

            if (plug.sc != null)
            {
                lock (plug.sc.imageitems)
                {
                    imageAuraKeys = plug.sc.imageitems.Keys.ToList();
                    dgvImage.RowCount = imageAuraKeys.Count;
                }
            }
            else
            {
                lock (plug.imageauras)
                {
                    imageAuraKeys = plug.imageauras.Keys.ToList();
                    dgvImage.RowCount = imageAuraKeys.Count;
                }
            }
            Refresh();
        }

        private void btnImageRefresh_Click(object sender, EventArgs e)
        {
            dgvImage.ClearSelection();
            RefreshImageAuras();
        }

        private void dgvImage_SelectionChanged(object sender, EventArgs e)
        {
            btnImageForce.Enabled = (dgvImage.SelectedRows.Count > 0);
        }

        private void btnImageForce_Click(object sender, EventArgs e)
        {
            List<string> anames = new List<string>();
            foreach (DataGridViewRow r in dgvImage.SelectedRows)
            {
                anames.Add(r.Cells[0].Value.ToString());
            }
            if (plug.sc != null)
            {
                dgvImage.ClearSelection();
                QueueAuraClosures(anames, Scarborough.Scarborough.ItemAction.ItemTypeEnum.Image, RefreshImageAuras);
                return;
            }
            else
            {
                foreach (string aname in anames)
                {
                    lock (plug.imageauras)
                    {
                        if (plug.imageauras.ContainsKey(aname) == true)
                        {
                            AuraContainerForm acf = plug.imageauras[aname];
                            acf.AuraDeactivate();
                        }
                    }
                }
            }
            btnImageRefresh_Click(sender, e);
        }

        /// <summary>按刷新时的名称快照读取，后台增删不会把同一行的列映射到不同项目。</summary>
        private static void AuraCellValueNeeded<T>(Dictionary<string, T> items, List<string> keys,
            DataGridViewCellValueEventArgs e, Func<T, Context> context)
        {
            lock (items)
            {
                if (e.RowIndex < 0 || e.RowIndex >= keys.Count) return;
                string key = keys[e.RowIndex];
                if (e.ColumnIndex == 0) e.Value = key;
                else if (e.ColumnIndex == 1 && items.TryGetValue(key, out T item))
                    e.Value = context(item)?.ToString() ?? "(unknown)";
            }
        }

        private void dgvImage_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (plug.sc != null)
                AuraCellValueNeeded(plug.sc.imageitems, imageAuraKeys, e, item => item.ctx);
            else
                AuraCellValueNeeded(plug.imageauras, imageAuraKeys, e, item => item.ctx);
        }
        #endregion

        #region Text auras
        private void RefreshTextAuras()
        {
            if (plug.sc != null)
            {
                lock (plug.sc.textitems)
                {
                    textAuraKeys = plug.sc.textitems.Keys.ToList();
                    dgvText.RowCount = textAuraKeys.Count;
                }
            }
            else
            {
                lock (plug.textauras)
                {
                    textAuraKeys = plug.textauras.Keys.ToList();
                    dgvText.RowCount = textAuraKeys.Count;
                }
            }
            Refresh();
        }

        private void btnTextRefresh_Click(object sender, EventArgs e)
        {
            dgvText.ClearSelection();
            RefreshTextAuras();
        }

        private void dgvText_SelectionChanged(object sender, EventArgs e)
        {
            btnTextForce.Enabled = (dgvText.SelectedRows.Count > 0);
        }

        private void btnTextForce_Click(object sender, EventArgs e)
        {
            List<string> anames = new List<string>();
            foreach (DataGridViewRow r in dgvText.SelectedRows)
            {
                anames.Add(r.Cells[0].Value.ToString());
            }
            if (plug.sc != null)
            {
                dgvText.ClearSelection();
                QueueAuraClosures(anames, Scarborough.Scarborough.ItemAction.ItemTypeEnum.Text, RefreshTextAuras);
                return;
            }
            else
            {
                foreach (string aname in anames)
                {
                    lock (plug.textauras)
                    {
                        if (plug.textauras.ContainsKey(aname) == true)
                        {
                            Forms.AuraContainerForm acf = plug.textauras[aname];
                            acf.AuraDeactivate();
                        }
                    }
                }
            }
            btnTextRefresh_Click(sender, e);
        }

        private void dgvText_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (plug.sc != null)
                AuraCellValueNeeded(plug.sc.textitems, textAuraKeys, e, item => item.ctx);
            else
                AuraCellValueNeeded(plug.textauras, textAuraKeys, e, item => item.ctx);
        }
        #endregion

        #region Named callbacks
        private void RefreshNamedCallbacks()
        {
            lock (plug.callbacksById)
            {
                SortCallbacks();
                dgvCallback.RowCount = sortedCallbackKeys.Count;
            }
            Refresh();
        }

        private void btnCallbackRefresh_Click(object sender, EventArgs e)
        {
            RefreshNamedCallbacks();
        }

        private void dgvCallback_SelectionChanged(object sender, EventArgs e)
        {
            dgvCallback.ClearSelection();
        }

        private void dgvCallback_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            lock (plug.callbacksById)
            {

                if (e.RowIndex < 0 || e.RowIndex >= sortedCallbackKeys.Count
                    || !plug.callbacksById.TryGetValue(sortedCallbackKeys[e.RowIndex], out RealPlugin.NamedCallback cb))
                    return;

                switch (e.ColumnIndex)
                {
                    case 0:
                        e.Value = cb.Id.ToString(CultureInfo.InvariantCulture);
                        break;
                    case 1:
                        e.Value = cb.Name; break;
                    case 2:
                        e.Value = cb.Registrant; break;
                    case 3:
                        e.Value = cb.RegistrationTime.ToString(); break;
                    case 4:
                        e.Value = cb.LastInvoked?.ToString() ?? ""; break;
                }
            }
        }

        private List<int> sortedCallbackKeys = new List<int>();
        private void SortCallbacks()
        {
            var variables = plug.callbacksById;
            switch (sortType)
            {
                case "id":
                    sortedCallbackKeys = SortKeys(variables, pair => pair.Value.Id); break;
                case "name":
                    sortedCallbackKeys = SortKeys(variables, pair => pair.Value.Name); break;
                case "registrant":
                    sortedCallbackKeys = SortKeys(variables, pair => pair.Value.Registrant ?? ""); break;
                case "lastRegistered":
                    sortedCallbackKeys = SortKeys(variables, pair => pair.Value.RegistrationTime); break;
                case "lastInvoked":
                    sortedCallbackKeys = SortKeys(variables, pair => pair.Value.LastInvoked ?? DateTime.MinValue); break;
                default:
                    sortedCallbackKeys = SortKeys(variables, pair => pair.Key); break;
            }
        }

        #endregion

    }

}
