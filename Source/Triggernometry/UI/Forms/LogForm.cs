using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Triggernometry.UI.CustomControls;
using Triggernometry.Localization;
using Triggernometry.UI.Forms;
using Triggernometry.Core;

namespace Triggernometry.UI.Forms
{

    public partial class LogForm : MemoryForm<LogForm>
    {

        private static readonly ComponentResourceManager actionViewerResources = new ComponentResourceManager(typeof(ActionViewer));

        internal RealPlugin plug = RealPlugin.Instance;

        internal List<InternalLog> logData;
        internal List<InternalLog> virtualData;
        internal bool startWithErrorSearch = false;

        public LogForm()
        {
            InitializeComponent();
            btnSortOrder.Image = (Image)actionViewerResources.GetObject("btnActionBottom.Image");
            logData = new List<InternalLog>();
            Shown += LogForm_Shown;
            RestoredSavedDimensions();
            dgvLog.CellValueNeeded += DgvLog_CellValueNeeded;
            dgvLog.CellFormatting += DgvLog_CellFormatting;
            rexSearch.OnEnterKeyHit += RexSearch_OnEnter;
            dgvLog.CellMouseClick += DgvLog_CellMouseClick;
            dgvLog.CellMouseDoubleClick += DgvLog_CellMouseDoubleClick;
            dgvLog.SelectionChanged += DgvLog_SelectionChanged;
            dgvLog.KeyDown += DgvLog_KeyDown;
        }

        private void RexSearch_OnEnter()
        {
            btnSearch_Click(null, null);
        }

        internal static Color BgRed = Color.FromArgb(255, 210, 210);  
        internal static Color BgYellow = Color.FromArgb(255, 240, 195);  
        internal static Color BgGreen = Color.FromArgb(195, 255, 225);
        internal static Color BgBlue = Color.FromArgb(195, 225, 255);
        internal static Color BgGray = Color.FromArgb(225, 225, 225); 

        private void DgvLog_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            InternalLog il = virtualData[e.RowIndex];
            switch (il.Level)
            {
                case RealPlugin.DebugLevelEnum.Verbose:
                    e.CellStyle.BackColor = BgGray;
                    break;
                case RealPlugin.DebugLevelEnum.Info:
                    e.CellStyle.BackColor = Color.White;
                    break;
                case RealPlugin.DebugLevelEnum.Warning:
                    e.CellStyle.BackColor = BgYellow; 
                    break;
                case RealPlugin.DebugLevelEnum.Error:
                    e.CellStyle.BackColor = BgRed;
                    break;
                case RealPlugin.DebugLevelEnum.Custom:
                    e.CellStyle.BackColor = BgGreen;
                    break;
                case RealPlugin.DebugLevelEnum.Custom2:
                    e.CellStyle.BackColor = BgBlue;
                    break;
            }
            e.CellStyle.ForeColor = Color.Black;
            e.FormattingApplied = true;
        }

        private void DgvLog_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0) return;
            InternalLog il = virtualData[e.RowIndex];
            switch (e.ColumnIndex)
            {
                case 0:
                    e.Value = RealPlugin.FormatDateTime(il.Timestamp);
                    break;
                case 1:
                    e.Value = I18n.Translate($"LogForm/chk{il.Level}", $"{il.Level}");
                    break;
                case 2:
                    e.Value = il.Message;
                    break;
            }
        }

        private void LogForm_Shown(object sender, EventArgs e)
        {
            ResetFilters(startWithErrorSearch);
            rexSearch.Focus();
        }

        /// <summary>恢复默认筛选，或仅显示错误与警告；适用于已打开的日志窗口。</summary>
        internal void ResetFilters(bool errorsAndWarningsOnly)
        {
            isChkProgrammaticChange = true;
            try
            {
                chkAll.Checked = false;
                chkError.Checked = true;
                chkWarning.Checked = true;
                chkInfo.Checked = !errorsAndWarningsOnly;
                chkVerbose.Checked = false;
                chkCustom.Checked = !errorsAndWarningsOnly;
                chkCustom2.Checked = !errorsAndWarningsOnly;
                rexSearch.Text = "";
            }
            finally
            {
                isChkProgrammaticChange = false;
            }
            RefreshLog();
        }

        private void RefreshLog()
        {
            logData.Clear();
            foreach (var pair in plug.log)
            {
                var queue = pair.Value;
                lock (queue)
                {
                    logData.AddRange(queue);
                }
            }
            RefreshViewVirtual();
        }

        private List<InternalLog> BuildDataset()
        {
            Regex rex = null;
            if (rexSearch.Text != null && rexSearch.Text.Trim().Length > 0)
            {
                try
                {
                    rex = new Regex(rexSearch.Text, RegexOptions.IgnoreCase);
                }
                catch (ArgumentException)
                {
                    return new List<InternalLog>();
                }
            }
            var filtered = logData.Where(ix =>
                (
                    chkAll.Checked ||
                    (chkError.Checked && ix.Level == RealPlugin.DebugLevelEnum.Error) ||
                    (chkWarning.Checked && ix.Level == RealPlugin.DebugLevelEnum.Warning) ||
                    (chkInfo.Checked && ix.Level == RealPlugin.DebugLevelEnum.Info) ||
                    (chkVerbose.Checked && ix.Level == RealPlugin.DebugLevelEnum.Verbose) ||
                    (chkCustom.Checked && ix.Level == RealPlugin.DebugLevelEnum.Custom) ||
                    (chkCustom2.Checked && ix.Level == RealPlugin.DebugLevelEnum.Custom2) 
                ) && (rex == null || rex.IsMatch(ix.Message))
            ).ToList();
            bool ascending = btnSortOrder.Checked;
            filtered.Sort((a, b) =>
            {
                int order = a.Timestamp.CompareTo(b.Timestamp);
                if (order == 0)
                    order = a.Sequence.CompareTo(b.Sequence);
                return ascending ? order : -order;
            });
            return filtered;
        }

        private void RefreshViewVirtual()
        {
            List<InternalLog> p1 = BuildDataset();
            dgvLog.RowCount = 0;
            if (virtualData != null)
            {
                virtualData.Clear();
                virtualData = null;
            }
            virtualData = p1;
            dgvLog.RowCount = p1.Count;
            dgvLog.Refresh();
            lblStatus.Text = I18n.Translate("internal/LogForm/displaying", "Displaying {0} out of {1}", p1.Count, logData.Count);
        }

        private DateTime lastDoubleClickTime = DateTime.MinValue;
        private InternalLog lastDoubleClickedLog = null;
        // double click on a log entry to jump to and select the trigger
        private void DgvLog_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.Button != MouseButtons.Left)
                return;
            InternalLog log = virtualData[e.RowIndex];
            if (log.SourceAction != null)
            {
                InternalLog.RecordedAction = log.SourceAction;
            }
            if (log.SourceTrigger != null)
            {
                var root = log.SourceTrigger.Repo == null ? plug.ui.treeView.Nodes[0] : plug.ui.treeView.Nodes[1];
                TreeNode node = RealPlugin.Instance.LocateNodeHostingTrigger(root, log.SourceTrigger);
                if (node != null)
                {
                    lastDoubleClickedLog = log;
                    lastDoubleClickTime = DateTime.Now;
                    System.Media.SystemSounds.Asterisk.Play();
                    RealPlugin.Instance.ui.LocateTreeNode(node);
                    return;
                }
            }
            System.Media.SystemSounds.Hand.Play();
        }

        // triple click on a log entry to jump to and edit the trigger
        private void DgvLog_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.Button != MouseButtons.Left)
                return;

            InternalLog currentLog = virtualData[e.RowIndex];
            if (currentLog == lastDoubleClickedLog && (DateTime.Now - lastDoubleClickTime).TotalMilliseconds < SystemInformation.DoubleClickTime)
            {
                RealPlugin.Instance.ui.btnEdit_Click(null, null);
                lastDoubleClickTime = DateTime.MinValue;
                lastDoubleClickedLog = null;
            }
        }

        /// <summary>按当前行号输出所选日志，保留筛选和排序后的显示顺序。</summary>
        private void CopySelectedLogs(bool messagesOnly)
        {
            if (dgvLog.SelectedRows.Count == 0)
                return;
            try
            {
                var selectedLogs = dgvLog.SelectedRows.Cast<DataGridViewRow>()
                    .OrderBy(row => row.Index)
                    .Select(row => virtualData[row.Index]);
                string text = string.Join(Environment.NewLine, selectedLogs.Select(log => messagesOnly
                    ? log.Message
                    : string.Join("\t", RealPlugin.FormatDateTime(log.Timestamp),
                        I18n.Translate($"LogForm/chk{log.Level}", $"{log.Level}"), log.Message)));
                if (text.Length == 0)
                    Clipboard.Clear();
                else
                    Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, I18n.Translate("internal/LogForm/exception", "Exception"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CopyRows_Click(object sender, EventArgs e) => CopySelectedLogs(false);

        private void CopyMessages_Click(object sender, EventArgs e) => CopySelectedLogs(true);

        private void DgvLog_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dgvLog.SelectedRows.Count > 0;
            btnCopyRows.Enabled = btnCopyMessages.Enabled = ctxCopyRows.Enabled = ctxCopyMessages.Enabled = hasSelection;
        }

        private void DgvLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.C))
            {
                e.SuppressKeyPress = true;
                CopySelectedLogs(true);
            }
        }

        private void SortOrder_CheckedChanged(object sender, EventArgs e)
        {
            btnSortOrder.Image = (Image)actionViewerResources.GetObject(btnSortOrder.Checked
                ? "btnActionUp.Image" : "btnActionBottom.Image");
            btnSortOrder.Text = btnSortOrder.Checked
                ? I18n.Translate("LogForm/btnSortOrder[Ascending]", "Ascending")
                : I18n.Translate("LogForm/btnSortOrder", "Descending");
            RefreshViewVirtual();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            plug.ClearLog();
            RefreshLog();
        }

        private void btnSearchOptions_CheckedChanged(object sender, EventArgs e)
        {
            // 同一轮布局中更新显示和跨列，避免自动列宽受中间状态影响。
            layoutMain.SuspendLayout();
            try
            {
                bool showOptions = btnSearchOptions.Checked;
                pnlSearchOptions.Visible = showOptions;
                layoutMain.SetColumnSpan(dgvLog, showOptions ? 1 : 2);
            }
            finally
            {
                layoutMain.ResumeLayout(true);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefreshLog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool isChkProgrammaticChange = false;

        private void ChkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (isChkProgrammaticChange) { return; }
            bool checkState = chkAll.Checked;

            isChkProgrammaticChange = true;
            chkError.Checked = checkState;
            chkWarning.Checked = checkState;
            chkInfo.Checked = checkState;
            chkVerbose.Checked = checkState;
            chkCustom.Checked = checkState;
            chkCustom2.Checked = checkState;
            isChkProgrammaticChange = false;
            RefreshLog();
        }

        private void ChkOther_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox currentChkBox = sender as CheckBox;
            if (isChkProgrammaticChange || currentChkBox == null) { return; }
            isChkProgrammaticChange = true;
            if (chkError.Checked && chkWarning.Checked && chkInfo.Checked && chkVerbose.Checked && chkCustom.Checked && chkCustom2.Checked)
            {
                chkAll.Checked = true;
            }
            else 
            {
                chkAll.Checked = false;
            }
            isChkProgrammaticChange = false;
            RefreshLog();
        }

        private void ChkAll_RightClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isChkProgrammaticChange = true;
                chkAll.Checked = true;
                chkError.Checked = true;
                chkWarning.Checked = true;
                chkInfo.Checked = true;
                chkVerbose.Checked = true;
                chkCustom.Checked = true;
                chkCustom2.Checked = true;
                isChkProgrammaticChange = false;
                RefreshLog();
            }
        }

        private void ChkOther_RightClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isChkProgrammaticChange = true;
                chkAll.Checked = false;
                chkError.Checked = false;
                chkWarning.Checked = false;
                chkInfo.Checked = false;
                chkVerbose.Checked = false;
                chkCustom.Checked = false;
                chkCustom2.Checked = false;
                CheckBox currentChkBox = sender as CheckBox;
                currentChkBox.Checked = true;
                isChkProgrammaticChange = false;
                RefreshLog();
            }
        }

    }

}
