using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;
using static Triggernometry.Core.Conditions.ConditionGroup;
using Triggernometry.Localization;
using Triggernometry.Core;
using Triggernometry.Core.Conditions;

namespace Triggernometry.UI.CustomControls
{

    public partial class ConditionViewer : UserControl
    {

        private Dictionary<ConditionComponent, TreeNode> _reverseNodeLookup = new Dictionary<ConditionComponent, TreeNode>();
        private bool isReadOnly;
        private bool isLoadingCondition;
        private Color disabledNodeColor;

        /// <summary> Set by parent form. </summary>
        internal Context UiContext { get; set; }
        private Trigger Trigger => UiContext?.Trigger;
        private RealPlugin Plugin => UiContext?.Plugin;

        private bool _PanelStateFromOption;
        private bool PanelStateFromOption
        {
            get
            {
                return _PanelStateFromOption;
            }
            set
            {
                _PanelStateFromOption = value;
                splitContainer1.Panel2Collapsed = (_PanelStateFromSelection == false || _PanelStateFromOption == false);
            }
        }

        private bool _PanelStateFromSelection;
        private bool PanelStateFromSelection
        {
            get
            {
                return _PanelStateFromSelection;
            }
            set
            {
                _PanelStateFromSelection = value;
                splitContainer1.Panel2Collapsed = (_PanelStateFromSelection == false || _PanelStateFromOption == false);
            }
        }

        private ConditionGroup _ConditionToEdit = null;
        public ConditionGroup ConditionToEdit
        {
            get
            {
                return _ConditionToEdit;
            }
            set
            {
                isLoadingCondition = true;
                trvNodes.BeginUpdate();
                try
                {
                    ClearReverseLookup();
                    trvNodes.Nodes.Clear();
                    _ConditionToEdit = value;
                    if (value == null)
                        UpdateEditorToConditionComponent(null);
                    else
                    {
                        UpdateEditorToCondition(value);
                        RecolorStartingFromNode(trvNodes.Nodes[0], value.Enabled);
                        trvNodes.ExpandAll();
                    }
                }
                finally
                {
                    isLoadingCondition = false;
                    trvNodes.EndUpdate();
                }
            }
        }

        /// <summary>解绑旧模型事件，避免已关闭的编辑器继续被条件对象引用。</summary>
        private void ClearReverseLookup()
        {
            foreach (var condition in _reverseNodeLookup.Keys) condition.OnPropertyChange -= Re_OnPropertyChange;
            _reverseNodeLookup.Clear();
        }

        private void RemoveReverseLookup(ConditionComponent cc)
        {
            if (_reverseNodeLookup.ContainsKey(cc) == true)
            {
                cc.OnPropertyChange -= Re_OnPropertyChange;
                _reverseNodeLookup.Remove(cc);
            }
        }

        private void SetReverseLookup(ConditionComponent cc, TreeNode tn)
        {
            if (!_reverseNodeLookup.ContainsKey(cc))
                cc.OnPropertyChange += Re_OnPropertyChange;
            _reverseNodeLookup[cc] = tn;
        }

        private TreeNode GetReverseLookup(ConditionComponent cc)
        {
            if (_reverseNodeLookup.ContainsKey(cc) == true)
            {
                return _reverseNodeLookup[cc];
            }
            return null;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            // ToolStrip uses its own default font instead of inheriting the viewer font.
            if (toolStrip1 != null)
                toolStrip1.Font = Font;
            if (contextMenuStrip1 != null)
                contextMenuStrip1.Font = Font;
        }

        public ConditionViewer()
        {
            InitializeComponent();
            // todo resize doesn't follow i18n when it's done here
            ResizeCombobox(cbxExpLType);
            ResizeCombobox(cbxExpRType);
            ResizeCombobox(cbxGroupingType);
            ResizeCombobox(cbxOpType);
            DoubleBuffered = true;
            disabledNodeColor = Color.Red;
            trvNodes.BeforeCheck += trvNodes_BeforeCheck;
            pnlEditorGroup.Visible = false;
            pnlEditorSingle.Visible = false;
            pnlEditorComment.Visible = false;
            PanelStateFromSelection = false;
            expLeft.textBox1.TextChanged += expLeft_TextChanged;
            expRight.textBox1.TextChanged += expRight_TextChanged;
            _PanelStateFromOption = btnProperties.Checked;
        }

        internal event EventHandler ConditionsUpdated;

        /// <summary> Keeps condition navigation and copying available without allowing edits. </summary>
        internal void SetReadOnly()
        {
            isReadOnly = true;
            expLeft.ReadOnly = expRight.ReadOnly = true;
            txtComment.ReadOnly = true;
            cbxExpLType.Enabled = cbxExpRType.Enabled = cbxOpType.Enabled = cbxGroupingType.Enabled = false;
            btnAdd.Enabled = btnAddGroup.Enabled = btnAddCondition.Enabled = btnAddComment.Enabled = btnDelete.Enabled = false;
            trvNodes.AllowDrop = false;
        }

        private void trvNodes_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if (isReadOnly || e.Node.Tag is ConditionComment)
                e.Cancel = true;
        }

        internal void OnConditionsUpdated()
        {
            if (!isLoadingCondition)
                ConditionsUpdated?.Invoke(this, EventArgs.Empty);
        }
        private void expLeft_TextChanged(object sender, EventArgs e)
        {
            if (isLoadingCondition)
                return;
            TreeNode tn = trvNodes.SelectedNode;
            ConditionSingle re = (ConditionSingle)tn.Tag;
            re.ExpressionL = expLeft.Expression;
        }

        private void expRight_TextChanged(object sender, EventArgs e)
        {
            if (isLoadingCondition)
                return;
            TreeNode tn = trvNodes.SelectedNode;
            ConditionSingle re = (ConditionSingle)tn.Tag;
            re.ExpressionR = expRight.Expression;
        }

        private void expLeft_LostFocus(object sender, EventArgs e)
        {
            OnConditionsUpdated();
        }

        private void expRight_LostFocus(object sender, EventArgs e)
        {
            OnConditionsUpdated();
        }

        private void ResizeCombobox(ComboBox cbx)
        {
            int mw = cbx.DropDownWidth;
            using (Graphics gr = toolStrip1.CreateGraphics())
            {
                foreach (string s in cbx.Items)
                {
                    int nw = (int)Math.Ceiling(gr.MeasureString(s, cbx.Font).Width);
                    if (nw > mw)
                    {
                        mw = nw;
                    }
                }
            }
            cbx.DropDownWidth = mw;
        }

        /// <summary> Uses row edges for sibling order and a group's center to move conditions into it. </summary>
        private bool TryGetDropTarget(DragEventArgs e, out TreeNode dragged, out TreeNode target, out int index)
        {
            dragged = e.Data?.GetData(typeof(TreeNode)) as TreeNode;
            target = null;
            index = -1;
            if (isReadOnly || dragged?.Parent == null || dragged.TreeView != trvNodes
                || (e.AllowedEffect & DragDropEffects.Move) == 0)
                return false;

            Point point = trvNodes.PointToClient(new Point(e.X, e.Y));
            if (!trvNodes.ClientRectangle.Contains(point))
                return false;
            target = trvNodes.GetNodeAt(point) ?? trvNodes.Nodes[0];
            if (target == dragged)
                return false;
            Rectangle bounds = target.Bounds;
            if (!(target.Tag is ConditionGroup) || (target.Parent != null
                && (point.Y < bounds.Top + bounds.Height / 4 || point.Y >= bounds.Bottom - bounds.Height / 4)))
            {
                index = target.Index + (point.Y < bounds.Top + bounds.Height / 2 ? 0 : 1);
            }
            TreeNode parent = index < 0 ? target : target.Parent;
            if (parent == null)
                return false;
            for (TreeNode ancestor = parent; ancestor != null; ancestor = ancestor.Parent)
            {
                if (ancestor == dragged)
                    return false;
            }
            return true;
        }

        internal void trvNodes_DragOver(object sender, DragEventArgs e)
        {
            if (TryGetDropTarget(e, out _, out TreeNode target, out int index))
            {
                e.Effect = DragDropEffects.Move;
                trvNodes.SelectedNode = target;
                trvNodes.SetInsertionMark(index < 0 ? null : target, index > target.Index);
                if (index < 0)
                    target.Expand();
            }
            else
            {
                e.Effect = DragDropEffects.None;
                trvNodes.SetInsertionMark(null, false);
            }
        }

        internal void trvNodes_DragEnter(object sender, DragEventArgs e)
        {
            trvNodes_DragOver(sender, e);
        }

        private void trvNodes_DragLeave(object sender, EventArgs e)
        {
            trvNodes.SetInsertionMark(null, false);
        }

        internal void FocusTree()
        {
            trvNodes.Focus();
        }

        internal void trvNodes_DragDrop(object sender, DragEventArgs e)
        {
            trvNodes.SetInsertionMark(null, false);
            if (e.Effect != DragDropEffects.Move || !TryGetDropTarget(e, out TreeNode dragged, out TreeNode target, out int index))
                return;
            TreeNode parent = index < 0 ? target : target.Parent;
            if (index < 0)
                index = parent.Nodes.Count;
            if (dragged.Parent == parent && dragged.Index < index)
                index--;
            if (dragged.Parent == parent && dragged.Index == index)
                return;

            // Tree order is also the serialized and short-circuit evaluation order.
            ConditionComponent condition = (ConditionComponent)dragged.Tag;
            condition.Parent.Children.Remove(condition);
            condition.Parent = (ConditionGroup)parent.Tag;
            condition.Parent.Children.Insert(index, condition);
            dragged.Remove();
            parent.Nodes.Insert(index, dragged);
            parent.Expand();
            trvNodes.SelectedNode = dragged;
            RecolorStartingFromNode(dragged, parent.Checked);
            OnConditionsUpdated();
        }

        internal void trvNodes_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (!isReadOnly && e.Button == MouseButtons.Left && e.Item is TreeNode node && node.Parent != null)
            {
                DoDragDrop(node, DragDropEffects.Move);
                trvNodes.SetInsertionMark(null, false);
            }
        }

        private void UpdateNodeIcon(TreeNode tn)
        {
            ConditionComponent re = (ConditionComponent)tn.Tag;
            if (re is ConditionComment)
            {
                tn.ImageKey = "comment";
                tn.SelectedImageKey = "comment";
            }
            if (re is ConditionSingle)
            {
                tn.ImageKey = "function";
                tn.SelectedImageKey = "function";
            }
            if (re is ConditionGroup)
            {
                switch (((ConditionGroup)re).Grouping)
                {
                    case ConditionGroup.CndGroupingEnum.And:
                        tn.ImageKey = "group_and";
                        tn.SelectedImageKey = "group_and";
                        break;
                    case ConditionGroup.CndGroupingEnum.Or:
                        tn.ImageKey = "group_or";
                        tn.SelectedImageKey = "group_or";
                        break;
                    case ConditionGroup.CndGroupingEnum.Xor:
                        tn.ImageKey = "group_xor";
                        tn.SelectedImageKey = "group_xor";
                        break;
                    case ConditionGroup.CndGroupingEnum.Not:
                        tn.ImageKey = "group_not";
                        tn.SelectedImageKey = "group_not";
                        break;
                }
            }
        }

        private void AttachConditionComponentToNode(TreeNode tn, ConditionComponent re)
        {
            tn.Tag = re;
            tn.Text = re.ToString();
            tn.Checked = !(re is ConditionComment) && re.Enabled;
            if (re is ConditionComment)
                tn.ForeColor = disabledNodeColor;
            if (re != null)
            {
                SetReverseLookup(re, tn);
            }
            UpdateNodeIcon(tn);
        }

        private void Re_OnPropertyChange(ConditionComponent re)
        {
            TreeNode tn = GetReverseLookup(re);
            if (tn == null)
                return;
            UpdateNodeIcon(tn);
            tn.Text = re.ToString();
        }

        private void UpdateEditorToCondition(ConditionGroup r)
        {
            BuildTreeFromCondition(r);
            trvNodes.SelectedNode = trvNodes.Nodes[0];
            trvNodes.Nodes[0].Expand();
        }

        private void UpdateEditorToConditionComponent(ConditionComponent re)
        {
            bool wasLoading = isLoadingCondition;
            isLoadingCondition = true;
            try
            {
                pnlEditorComment.Visible = false;
                if (re is ConditionSingle)
                {
                    pnlEditorGroup.Visible = false;
                    PanelStateFromSelection = true;
                    ConditionSingle r = (ConditionSingle)re;
                    expLeft.Expression = r.ExpressionL;
                    expRight.Expression = r.ExpressionR;
                    switch (r.ExpressionTypeL)
                    {
                        case ConditionSingle.ExprTypeEnum.String:
                            cbxExpLType.SelectedIndex = 0;
                            break;
                        case ConditionSingle.ExprTypeEnum.Numeric:
                            cbxExpLType.SelectedIndex = 1;
                            break;
                    }
                    switch (r.ExpressionTypeR)
                    {
                        case ConditionSingle.ExprTypeEnum.String:
                            cbxExpRType.SelectedIndex = 0;
                            break;
                        case ConditionSingle.ExprTypeEnum.Numeric:
                            cbxExpRType.SelectedIndex = 1;
                            break;
                    }
                    cbxOpType.SelectedIndex = (int)r.ConditionType;
                    expLeft.ExpressionType = r.ExpressionTypeL == ConditionSingle.ExprTypeEnum.Numeric
                        ? ExpressionTextBox.SupportedExpressionTypeEnum.Numeric : ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    UpdateRightExpressionType();
                    capProperties.Caption = I18n.Translate("internal/ConditionViewer/condprops", "Condition properties");
                    pnlEditorSingle.Visible = true;
                }
                else if (re is ConditionGroup)
                {
                    pnlEditorSingle.Visible = false;
                    PanelStateFromSelection = true;
                    ConditionGroup r = (ConditionGroup)re;
                    switch (r.Grouping)
                    {
                        case ConditionGroup.CndGroupingEnum.And:
                            cbxGroupingType.SelectedIndex = 0;
                            break;
                        case ConditionGroup.CndGroupingEnum.Or:
                            cbxGroupingType.SelectedIndex = 1;
                            break;
                        case ConditionGroup.CndGroupingEnum.Xor:
                            cbxGroupingType.SelectedIndex = 2;
                            break;
                        case ConditionGroup.CndGroupingEnum.Not:
                            cbxGroupingType.SelectedIndex = 3;
                            break;
                    }
                    capProperties.Caption = I18n.Translate("internal/ConditionViewer/groupprops", "Group properties");
                    pnlEditorGroup.Visible = true;
                }
                else if (re is ConditionComment comment)
                {
                    pnlEditorSingle.Visible = false;
                    pnlEditorGroup.Visible = false;
                    PanelStateFromSelection = true;
                    txtComment.Text = comment.Text;
                    capProperties.Caption = I18n.Translate("internal/ConditionViewer/commentprops", "Comment properties");
                    pnlEditorComment.Visible = true;
                }
                else
                {
                    pnlEditorSingle.Visible = false;
                    pnlEditorGroup.Visible = false;
                    PanelStateFromSelection = false;
                }
                btnAddGroup.Enabled = btnAddCondition.Enabled = btnAddComment.Enabled = !isReadOnly && re != null;
                btnDelete.Enabled = !isReadOnly && re?.Parent != null;
            }
            finally { isLoadingCondition = wasLoading; }
        }

        private void trvNodes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateEditorToConditionComponent((ConditionComponent)e.Node.Tag);
        }

        private void BuildTreeFromCondition(ConditionGroup r)
        {
            BuildTreeFromConditionItem(null, r);
        }

        private TreeNode BuildTreeFromConditionItem(TreeNode parent, ConditionComponent r)
        {
            TreeNode tn = new TreeNode();
            if (parent == null)
            {
                trvNodes.Nodes.Add(tn);
            }
            else
            {
                parent.Nodes.Add(tn);
            }
            AttachConditionComponentToNode(tn, r);
            if (r is ConditionGroup)
            {
                ConditionGroup gr = (ConditionGroup)r;
                foreach (ConditionComponent cr in gr.Children)
                {
                    BuildTreeFromConditionItem(tn, cr);
                }
            }
            return tn;
        }

        internal int CountRootConditions()
        {   // Count the enabled conditions under root folder.
            // -1 if contains enabled subfolder.
            TreeNodeCollection nodes = trvNodes.Nodes;
            TreeNode rootNode = nodes[0];
            if (rootNode.Tag is ConditionGroup rootGroup)
            {
                if (!rootGroup.Enabled) { return 0; }

                int conditionCount = 0;
                foreach (TreeNode node in rootNode.Nodes)
                {
                    if (node.Tag is ConditionGroup group && group.Enabled)
                    {
                        return -1;
                    }
                    else if (node.Tag is ConditionSingle condition && condition.Enabled)
                    {
                        conditionCount++;
                    }
                }
                return conditionCount;
            }
            return 0;
        }

        internal CndGroupingEnum RootConditionType()
        {
            var root = (ConditionGroup)trvNodes.Nodes[0].Tag;
            return root.Grouping;
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            AddComponent(new ConditionGroup());
        }

        private void btnAddCondition_Click(object sender, EventArgs e)
        {
            AddComponent(new ConditionSingle { ConditionType = ConditionSingle.CndTypeEnum.StringEqualNocase });
            expLeft.Focus();
        }

        private void btnAddComment_Click(object sender, EventArgs e)
        {
            AddComponent(new ConditionComment());
            txtComment.Focus();
        }

        private void txtComment_TextChanged(object sender, EventArgs e)
        {
            if (isLoadingCondition)
                return;
            ConditionComment comment = (ConditionComment)trvNodes.SelectedNode.Tag;
            if (comment.Text == txtComment.Text)
                return;
            comment.Text = txtComment.Text;
            OnConditionsUpdated();
        }

        /// <summary> Appends an item to the selected group or the selected leaf's parent. </summary>
        private void AddComponent(ConditionComponent re)
        {
            TreeNode tn = new TreeNode();
            AttachConditionComponentToNode(tn, re);
            TreeNode tx = trvNodes.SelectedNode;
            if (!(tx.Tag is ConditionGroup))
                tx = tx.Parent;
            ((ConditionGroup)tx.Tag).AddChild(re);
            tx.Nodes.Add(tn);
            RecolorStartingFromNode(tx, tx.Checked);
            trvNodes.SelectedNode = tn;
            if (tx.IsExpanded == false)
            {
                tx.Expand();
            }
            OnConditionsUpdated();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            TreeNode tn = trvNodes.SelectedNode;
            ConditionComponent re = (ConditionComponent)tn.Tag;
            if (re is ConditionSingle || re is ConditionComment)
            {
                string prompt = re is ConditionComment
                    ? I18n.Translate("internal/ConditionViewer/areyousurecomment", "Are you sure you want to remove the selected comment?")
                    : I18n.Translate("internal/ConditionViewer/areyousuresingle", "Are you sure you want to remove the selected condition?");
                if (MessageBox.Show(this, prompt + Environment.NewLine + Environment.NewLine + re.ToString(), I18n.Translate("internal/ConditionViewer/confirm", "Confirm removal"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    RemoveReverseLookup(re);
                    re.Parent.Children.Remove(re);
                    tn.Parent.Nodes.Remove(tn);
                }
            }
            if (re is ConditionGroup)
            {
                int numgroups = 0, numconditions = 0;
                CountItems((ConditionGroup)re, ref numgroups, ref numconditions);
                string temp = "";
                if (numgroups > 0 || numconditions > 0)
                {
                    temp += Environment.NewLine + Environment.NewLine + I18n.Translate("internal/ConditionViewer/operationwillremove", "This operation will remove:") + Environment.NewLine;
                    if (numgroups > 1)
                    {
                        temp += Environment.NewLine + "∙ " + I18n.Translate("internal/ConditionViewer/groupplural", "{0} groups", numgroups);
                    }
                    else if (numgroups > 0)
                    {
                        temp += Environment.NewLine + "∙ " + I18n.Translate("internal/ConditionViewer/groupsingular", "{0} group", numgroups);
                    }
                    if (numconditions > 1)
                    {
                        temp += Environment.NewLine + "∙ " + I18n.Translate("internal/ConditionViewer/conditionplural", "{0} conditions", numconditions);
                    }
                    else if (numconditions > 0)
                    {
                        temp += Environment.NewLine + "∙ " + I18n.Translate("internal/ConditionViewer/conditionsingular", "{0} condition", numconditions);
                    }
                }
                if (MessageBox.Show(this, I18n.Translate("internal/ConditionViewer/areyousuregroup", "Are you sure you want to remove the selected condition group?") + temp, I18n.Translate("internal/ConditionViewer/confirm", "Confirm removal"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteGroup((ConditionGroup)re);
                    tn.Parent.Nodes.Remove(tn);
                }
            }
            OnConditionsUpdated();
        }

        private void DeleteGroup(ConditionGroup rge)
        {
            RemoveReverseLookup(rge);
            rge.Parent.Children.Remove(rge);
            rge.Parent = null;
            while (rge.Children.Count > 0)
            {
                ConditionComponent re = rge.Children[0];
                if (!(re is ConditionGroup))
                {
                    RemoveReverseLookup(re);
                    re.Parent = null;
                    rge.Children.RemoveAt(0);
                }
                if (re is ConditionGroup)
                {
                    DeleteGroup((ConditionGroup)re);
                }
            }
            OnConditionsUpdated();
        }

        internal void CountItems(ConditionGroup rge, ref int numgroups, ref int numrules)
        {
            numgroups++;
            foreach (ConditionComponent re in rge.Children)
            {
                if (re is ConditionSingle)
                {
                    numrules++;
                }
                if (re is ConditionGroup)
                {
                    CountItems((ConditionGroup)re, ref numgroups, ref numrules);
                }
            }
        }

        private void trvNodes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                CopySelected();
            }
            else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                PasteSelected(false);
            }
            else if (e.KeyCode == Keys.V && e.Modifiers == (Keys.Control | Keys.Shift))
            {
                PasteSelected(true);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (btnDelete.Enabled == true)
                {
                    btnDelete_Click(this, null);
                }
            }
            else if (e.KeyCode == Keys.Insert)
            {
                if ((e.Modifiers & Keys.Shift) != 0)
                {
                    if (btnAddGroup.Enabled == true)
                    {
                        btnAddGroup_Click(this, null);
                    }
                }
                else
                {
                    if (btnAddCondition.Enabled == true)
                    {
                        btnAddCondition_Click(this, null);
                    }
                }
            }
        }

        private void cbxExpRType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingCondition)
                return;
            TreeNode tn = trvNodes.SelectedNode;
            ConditionSingle re = (ConditionSingle)tn.Tag;
            switch (cbxExpRType.SelectedIndex)
            {
                case 0:
                    re.ExpressionTypeR = ConditionSingle.ExprTypeEnum.String;
                    break;
                case 1:
                    re.ExpressionTypeR = ConditionSingle.ExprTypeEnum.Numeric;
                    break;
            }
            UpdateRightExpressionType();
            OnConditionsUpdated();
        }

        /// <summary> Validates the right-hand string as a regex when the comparison uses one. </summary>
        private void UpdateRightExpressionType()
        {
            bool isRegex = cbxOpType.SelectedIndex == (int)ConditionSingle.CndTypeEnum.RegexMatch ||
                cbxOpType.SelectedIndex == (int)ConditionSingle.CndTypeEnum.RegexNotMatch;
            expRight.ExpressionType = cbxExpRType.SelectedIndex == 1
                ? ExpressionTextBox.SupportedExpressionTypeEnum.Numeric
                : isRegex ? ExpressionTextBox.SupportedExpressionTypeEnum.Regex
                : ExpressionTextBox.SupportedExpressionTypeEnum.String;
        }

        private void cbxExpLType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingCondition)
                return;
            TreeNode tn = trvNodes.SelectedNode;
            ConditionSingle re = (ConditionSingle)tn.Tag;
            switch (cbxExpLType.SelectedIndex)
            {
                case 0:
                    expLeft.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.String;
                    re.ExpressionTypeL = ConditionSingle.ExprTypeEnum.String;
                    break;
                case 1:
                    expLeft.ExpressionType = ExpressionTextBox.SupportedExpressionTypeEnum.Numeric;
                    re.ExpressionTypeL = ConditionSingle.ExprTypeEnum.Numeric;
                    break;
            }
            OnConditionsUpdated();
        }

        private void trvNodes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                trvNodes.SelectedNode = e.Node;
            }
        }

        private void trvNodes_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isLoadingCondition)
                return;
            RecolorStartingFromNode(e.Node, e.Node.Checked);
            OnConditionsUpdated();
        }

        private void RecolorStartingFromNode(TreeNode tn, bool pstate)
        {
            object o = tn.Tag;
            TreeNode tnx = tn.Parent;
            while (tnx != null && pstate == true)
            {
                pstate = tnx.Checked;
                tnx = tnx.Parent;
            }
            tn.ForeColor = (!(o is ConditionComment) && tn.Checked && pstate) ? trvNodes.ForeColor : disabledNodeColor;
            ConditionComponent cc = (ConditionComponent)o;
            if (!(cc is ConditionComment))
                cc.Enabled = tn.Checked;
            if (o is ConditionGroup)
            {
                foreach (TreeNode tnn in tn.Nodes)
                {
                    RecolorStartingFromNode(tnn, (tn.Checked == true && pstate == true));
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ctxAddCondition.Enabled = btnAddCondition.Enabled;
            ctxAddGroup.Enabled = btnAddGroup.Enabled;
            ctxAddComment.Enabled = btnAddComment.Enabled;
            ctxRemove.Enabled = btnDelete.Enabled;
            ctxCopy.Enabled = (trvNodes.SelectedNode != null);
            ctxPaste.Enabled = (btnAddCondition.Enabled == true && System.Windows.Forms.Clipboard.ContainsText() == true);
            ctxPasteOver.Enabled = ctxPaste.Enabled;
            TreeNode selectedNode = trvNodes.SelectedNode;
            ctxTest.Enabled = selectedNode != null && !(selectedNode.Tag is ConditionComment);
            ctxExpandAll.Enabled = selectedNode != null && selectedNode.Tag is ConditionGroup;
            ctxCollapseAll.Enabled = ctxExpandAll.Enabled;
        }

        private void ctxBtnConditionGroup_Click(object sender, EventArgs e)
        {
            btnAddGroup_Click(sender, e);
        }

        private void ctxBtnCondition_Click(object sender, EventArgs e)
        {
            btnAddCondition_Click(sender, e);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopySelected();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteSelected(false);
        }

        internal void CopySelected()
        {
            try
            {
                string data = ExportSelection();
                System.Windows.Forms.Clipboard.SetText(data);
            }
            catch (Exception ex)
            {
                Plugin.FilteredAddToLog(RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/ConditionViewer/copyfail", "Tree copy failed due to exception: {0}", ex.Message));
            }
        }

        internal void PasteSelected(bool overWrite)
        {
            if (isReadOnly)
                return;
            string data = null;
            try
            {
                data = System.Windows.Forms.Clipboard.GetText(TextDataFormat.UnicodeText);
                if (data != null && data.Length > 0)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(data);
                    XmlSerializer xs = null;
                    if (doc.DocumentElement.Name == "ConditionGroup")
                    {
                        xs = new XmlSerializer(typeof(ConditionGroup));
                    }
                    else if (doc.DocumentElement.Name == "ConditionSingle")
                    {
                        xs = new XmlSerializer(typeof(ConditionSingle));
                    }
                    else if (doc.DocumentElement.Name == "ConditionComment")
                    {
                        xs = new XmlSerializer(typeof(ConditionComment));
                    }
                    else
                    {
                        // doesn't look like valid paste
                        return;
                    }
                    ConditionComponent cx;
                    using (XmlNodeReader nr = new XmlNodeReader(doc.DocumentElement))
                    {
                        cx = (ConditionComponent)xs.Deserialize(nr);
                    }
                    // XML stores children, but internal parent links must be rebuilt after deserialization.
                    if (cx is ConditionGroup group)
                    {
                        ConditionGroup.RebuildParentage(group);
                    }
                    TreeNode tn = trvNodes.SelectedNode;
                    ConditionComponent target = (ConditionComponent)tn.Tag;
                    if (
                        (
                            (cx is ConditionGroup && target is ConditionGroup)
                            ||
                            (cx is ConditionSingle && target is ConditionSingle)
                            ||
                            (cx is ConditionComment && target is ConditionComment)
                        )
                        &&
                        overWrite == true
                    )
                    {
                        if (cx is ConditionGroup && target is ConditionGroup)
                        {
                            ConditionGroup sc = (ConditionGroup)cx;
                            ConditionGroup tc = (ConditionGroup)target;
                            tc.Children.AddRange(sc.Children);
                            foreach (ConditionComponent cc in sc.Children)
                            {
                                cc.Parent = tc;
                                BuildTreeFromConditionItem(tn, cc);
                            }
                            tc.Enabled = sc.Enabled;
                            tn.Checked = tc.Enabled;
                            tc.Grouping = sc.Grouping;
                            RecolorStartingFromNode(tn, tn.Checked);
                            UpdateEditorToConditionComponent(target);
                            tn.Expand();
                        }
                        else if (cx is ConditionSingle && target is ConditionSingle)
                        {
                            ConditionSingle sc = (ConditionSingle)cx;
                            ConditionSingle tc = (ConditionSingle)target;
                            tc.ConditionType = sc.ConditionType;
                            tc.Enabled = sc.Enabled;
                            tn.Checked = tc.Enabled;
                            tc.ExpressionL = sc.ExpressionL;
                            tc.ExpressionR = sc.ExpressionR;
                            tc.ExpressionTypeL = sc.ExpressionTypeL;
                            tc.ExpressionTypeR = sc.ExpressionTypeR;
                            RecolorStartingFromNode(tn, tn.Checked);
                            UpdateEditorToConditionComponent(target);
                        }
                        else if (cx is ConditionComment sc && target is ConditionComment tc)
                        {
                            tc.Enabled = sc.Enabled;
                            tc.Text = sc.Text;
                            UpdateEditorToConditionComponent(target);
                        }
                    }
                    else
                    {
                        if (!(target is ConditionGroup))
                        {
                            target = target.Parent;
                            tn = tn.Parent;
                        }
                        ConditionGroup tg = (ConditionGroup)target;
                        tg.Children.Add(cx);
                        cx.Parent = tg;
                        TreeNode ex = BuildTreeFromConditionItem(tn, cx);
                        RecolorStartingFromNode(tn, tn.Checked);
                        trvNodes.SelectedNode = ex;
                        ex.ExpandAll();
                    }
                }
                OnConditionsUpdated();
            }
            catch (Exception ex)
            {
                Plugin.FilteredAddToLog(RealPlugin.DebugLevelEnum.Error, I18n.Translate("internal/ConditionViewer/pastefail", "Tree paste failed due to exception: {0}", ex.Message));
            }
        }

        private void cbxGroupingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingCondition)
                return;
            TreeNode tn = trvNodes.SelectedNode;
            ConditionGroup re = (ConditionGroup)tn.Tag;
            switch (cbxGroupingType.SelectedIndex)
            {
                case 0:
                    re.Grouping = ConditionGroup.CndGroupingEnum.And;
                    break;
                case 1:
                    re.Grouping = ConditionGroup.CndGroupingEnum.Or;
                    break;
                case 2:
                    re.Grouping = ConditionGroup.CndGroupingEnum.Xor;
                    break;
                case 3:
                    re.Grouping = ConditionGroup.CndGroupingEnum.Not;
                    break;
            }
            UpdateNodeIcon(tn);
            OnConditionsUpdated();
        }

        private void cbxOpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingCondition)
                return;
            TreeNode tn = trvNodes.SelectedNode;
            ConditionSingle re = (ConditionSingle)tn.Tag;
            re.ConditionType = (ConditionSingle.CndTypeEnum)cbxOpType.SelectedIndex;
            UpdateRightExpressionType();
            OnConditionsUpdated();
        }

        private string ExportSelection()
        {
            TreeNode tn = trvNodes.SelectedNode;
            ConditionComponent cc = (ConditionComponent)tn.Tag;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            string test = "";
            ns.Add("", "");
            XmlSerializer xs = new XmlSerializer(cc.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                xs.Serialize(ms, cc, ns);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    test = sr.ReadToEnd();
                }
            }
            return test;
        }

        private void pasteOverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteSelected(true);
        }

        private void btnProperties_CheckedChanged(object sender, EventArgs e)
        {
            PanelStateFromOption = btnProperties.Checked;
        }

        private void testStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = trvNodes.SelectedNode;
            Context fakectx = new Context(Trigger);
            if (selectedNode.Tag is ConditionGroup group)
            {
                string result = group.Check(fakectx, null, null) ? "True" : "False";
                MessageBox.Show(result, I18n.Translate("internal/ConditionViewer/testtitle", "Test"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (selectedNode.Tag is ConditionSingle condition)
            {
                string result = condition.Check(fakectx, null, null) ? "True" : "False";
                string GetExprResult(Func<string> func)
                {
                    try { return func(); }
                    catch (Exception ex)
                    {
                        return "[Error] " + ex.Message;
                    }
                }
                string left = "L = \"" + GetExprResult(() => condition.GetExpressionResultL(fakectx, null, null)) + "\"";
                string right = "R = \"" + GetExprResult(() => condition.GetExpressionResultR(fakectx, null, null)) + "\"";
                MessageBox.Show($"{result}\n{left}\n{right}", I18n.Translate("internal/ConditionViewer/testtitle", "Test"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void expandAllStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = trvNodes.SelectedNode;
            if (selectedNode != null)
            {
                selectedNode.ExpandAll();
            }
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = trvNodes.SelectedNode;
            if (selectedNode != null)
            {
                selectedNode.Collapse(false);
            }
        }

    }

}
