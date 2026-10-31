using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Triggernometry.Core;
using Triggernometry.Core.Variables;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Localization;

namespace Triggernometry.UI.CustomControls
{
    public class PartyListPanel : TableLayoutPanel
    {
        List<PlayerLabel> _players = new List<PlayerLabel>();
        private readonly VarNames varNames;
        private static readonly string[] Role8 = { "MT", "ST", "H1", "H2", "D1", "D2", "D3", "D4" };
        public int PlayerCount;
        public string[] PlayerDescriptions;

        public class VarNames
        {
            public string MyIndexVar = "myIdx";

            /// <summary>
            /// 4 人小队：T H D D
            /// 8 人小队：T T H H D D D D
            /// </summary>
            public string IsStandardVar = "partyIsStandard";

            public string IdsList = "party";
            public string NamesList = "partyName";
            public string JobsList = "partyJob";

            public string IdToNameDict = "partyIdToName";
            public string IdToRole8Dict = "partyIdToRole8";
            public string IdToRole8FullWidthDict = "partyIdToRole８";
            public string IdToJobDict = "partyIdToJob";

            public static VarNames Default = new VarNames();
        }

        public PartyListPanel(string[] playerDescriptions, VarNames varNamesOverride = null)
        {
            SuspendLayout();

            varNames = varNamesOverride ?? VarNames.Default;

            if ((playerDescriptions?.Length ?? 0) == 0)
                playerDescriptions = new string[] { "[Undefined]" };
            PlayerCount = playerDescriptions.Length;
            PlayerDescriptions = playerDescriptions;

            Dock = DockStyle.Fill;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Set DragDrop Events
            DragEnter += new DragEventHandler(PartyListPanel_DragEnter);
            DragDrop += new DragEventHandler(PartyListPanel_DragDrop);
            AllowDrop = true;

            // Deternime Row and Column count based on PlayerCount
            int rowCount = (PlayerCount + 3) / 4;
            int colCount = Math.Min(PlayerCount, 4);
            RowCount = rowCount;
            ColumnCount = colCount;
            for (int i = 0; i < rowCount; i++)
            {
                RowStyles.Add(new RowStyle(SizeType.Percent, 100F / rowCount));
            }
            for (int i = 0; i < colCount; i++)
            {
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / colCount));
            }

            // Read Current Entities and Create PlayerLabels
            var entities = GetSortedPartyMembers();
            for (int i = 0; i < PlayerCount; i++)
            {
                PlayerLabel player = new PlayerLabel(this, entities[i], i);
                _players.Add(player);
            }

            // Adjust Label Size after the correct font is applied
            _players[0].HandleCreated += (sender, e) => AdjustLabelSizes();
            ResumeLayout(true);
        }

        private void AdjustLabelSizes()
        {
            double lblWidth = 50;
            double lblHeight = 30;
            using (Graphics g = CreateGraphics())
            {
                var font = _players[0].Font;
                foreach (var label in _players)
                {
                    SizeF size = g.MeasureString(label.Text, font);
                    lblWidth = Math.Max(lblWidth, size.Width);
                    lblHeight = Math.Max(lblHeight, size.Height);
                }
            }
            lblWidth += 20;
            lblHeight += 20;
            foreach (var label in _players)
            {
                label.Width = (int)lblWidth;
                label.Height = (int)lblHeight;
            }
        }

        List<FFXIV.Entity> GetSortedPartyMembers()
        {
            List<FFXIV.Entity> entities = FFXIV.Entity.GetEntities()
                .Where(e => e.HexID.StartsWith("10"))       // is player
                .OrderByDescending(e => e.InParty)          // is party member
                .ThenBy(e => e.Job.DefaultOrder)            // 与实体表达式使用相同的职业默认顺序
                .ThenBy(e => e.Name, StringComparer.Ordinal)
                .Take(PlayerCount)
                .ToList();

            while (entities.Count < PlayerCount)
            {
                entities.Add(FFXIV.Entity.NullEntity());
            }

            if (entities.Count == 8     // Double Caster => D2 / D4
                && entities[5].Job.SubRole == FFXIV.Job.RoleType.PhysicalRanged
                && entities[6].Job.SubRole == FFXIV.Job.RoleType.MagicalRanged
                && entities[7].Job.SubRole == FFXIV.Job.RoleType.MagicalRanged)
            {
                (entities[5], entities[6]) = (entities[6], entities[5]);
                if (entities[7].Job.JobType == FFXIV.JobEnum.BLM) // with BLM: BLM D2
                {
                    (entities[5], entities[7]) = (entities[7], entities[5]);
                }
            }
            return entities;
        }

        private void PartyListPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(PlayerLabel)) is PlayerLabel label && label.ParentTable == this)
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        /// <summary> Get dragged label and target label, then update Order.</summary>
        private void PartyListPanel_DragDrop(object sender, DragEventArgs e)
        {
            var draggedLabel = e.Data.GetData(typeof(PlayerLabel)) as PlayerLabel;
            if (draggedLabel?.ParentTable != this)
                return;
            Point clientPoint = PointToClient(new Point(e.X, e.Y));
            Control control = GetChildAtPoint(clientPoint);
            if (control != null && control is PlayerLabel targetLabel && draggedLabel != targetLabel)
            {
                SuspendLayout();
                try
                {
                    SwapLabels(draggedLabel, targetLabel);
                }
                finally
                {
                    ResumeLayout(true);
                }
            }
        }

        private void SwapLabels(PlayerLabel draggedLabel, PlayerLabel targetLabel)
        {
            (draggedLabel.Order, targetLabel.Order) = (targetLabel.Order, draggedLabel.Order);
        }

        /// <summary>仅在已保存 ID 与当前队员完整匹配时恢复顺序。</summary>
        public void LoadFromConfig()
        {
            var savedList = RealPlugin.Instance.GetVariableStore(false).GetListVariable(varNames.IdsList);
            if (savedList == null || savedList.Size != PlayerCount)
                return;

            List<string> storedPlayerIDs = savedList.Values.Select(var => var.ToString()).ToList();

            List<int> indices = new List<int>();
            foreach (var playerLabel in _players)
            {
                int index = storedPlayerIDs.IndexOf(playerLabel.HexID);
                if (index >= 0 && index < PlayerCount)
                {
                    indices.Add(index);
                }
                else return;
            }
            HashSet<int> expectedIndices = new HashSet<int>(Enumerable.Range(0, PlayerCount));
            if (new HashSet<int>(indices).SetEquals(expectedIndices))
            {
                SuspendLayout();
                try
                {
                    for (int i = 0; i < PlayerCount; i++)
                    {
                        _players[i].Order = indices[i];
                    }
                }
                finally
                {
                    ResumeLayout(true);
                }
            }
        }

        /// <summary>按当前队员顺序生成临时变量；空位只保留列表位置，不加入 ID 字典。</summary>
        public void SaveToConfig()
        {
            if (_players.Count <= 1)
                return;

            var store = RealPlugin.Instance.GetVariableStore(false);
            var players = _players.OrderBy(p => p.Order).ToArray();
            var names = new VariableDictionary();
            var jobs = new VariableDictionary();
            var roles = new VariableDictionary();
            var fullWidthRoles = new VariableDictionary();
            const string changer = "PartyListPanel";
            uint myId = FFXIV.Entity.GetMyself().ID;
            int myIndex = 0;

            foreach (var player in players)
            {
                if (player.ID == 0)
                    continue;

                names.SetValue(player.HexID, player.PlayerName, changer);
                jobs.SetValue(player.HexID, player.JobName, changer);
                if (PlayerCount == 8)
                {
                    string role = Role8[player.Order];
                    roles.SetValue(player.HexID, role, changer);
                    fullWidthRoles.SetValue(player.HexID, role.ToFullWidth(), changer);
                }
                if (player.ID == myId)
                    myIndex = player.Order + 1;
            }

            var mainRoles = players.Where(p => p.ID != 0).Select(p => p.SubRole & FFXIV.Job.RoleType.MainRole).ToArray();
            int tankCount = mainRoles.Count(role => role == FFXIV.Job.RoleType.Tank);
            int healerCount = mainRoles.Count(role => role == FFXIV.Job.RoleType.Healer);
            int dpsCount = mainRoles.Count(role => role == FFXIV.Job.RoleType.DPS);
            // 只判断实际职业构成，拖拽调整位置不会改变是否为标准小队。
            bool isStandard = PlayerCount == 4 && tankCount == 1 && healerCount == 1 && dpsCount == 2
                || PlayerCount == 8 && tankCount == 2 && healerCount == 2 && dpsCount == 4;

            lock (store.Scalar)
            {
                store.Scalar[varNames.MyIndexVar] = new VariableScalar(myIndex);
                store.Scalar[varNames.IsStandardVar] = new VariableScalar(isStandard ? 1 : 0);
            }
            lock (store.List)
            {
                store.List[varNames.IdsList] = new VariableList(players.Select(p => p.HexID)) { LastChanger = changer };
                store.List[varNames.NamesList] = new VariableList(players.Select(p => p.PlayerName)) { LastChanger = changer };
                store.List[varNames.JobsList] = new VariableList(players.Select(p => p.ID != 0 ? p.JobName : "")) { LastChanger = changer };
            }
            lock (store.Dict)
            {
                store.Dict[varNames.IdToNameDict] = names;
                store.Dict[varNames.IdToJobDict] = jobs;
                if (PlayerCount == 8)
                {
                    store.Dict[varNames.IdToRole8Dict] = roles;
                    store.Dict[varNames.IdToRole8FullWidthDict] = fullWidthRoles;
                }
                else
                {
                    store.Dict.Remove(varNames.IdToRole8Dict);
                    store.Dict.Remove(varNames.IdToRole8FullWidthDict);
                }
            }
        }

        public class PlayerLabel : Label
        {
            public PartyListPanel ParentTable;
            internal readonly uint ID;
            public string PlayerName;
            public FFXIV.Job.RoleType SubRole;
            public string JobName;
            public string HexID;
            private Label _draggingClone;

            private int _order;
            /// <summary> Start from 0. </summary>
            public int Order
            {
                get => _order;
                set
                {
                    _order = value;
                    Text = $"[{ParentTable.PlayerDescriptions[_order]}] {JobName}\n" + PlayerName.Replace(" ", "\n");
                    RefreshLocation();
                }
            }

            private Color GetForeColorByRole()
            {
                switch (SubRole & FFXIV.Job.RoleType.MainRole)
                {
                    case FFXIV.Job.RoleType.Tank: return Color.FromArgb(16, 72, 144);
                    case FFXIV.Job.RoleType.Healer: return Color.FromArgb(16, 144, 72);
                    case FFXIV.Job.RoleType.DPS:
                        switch (SubRole)
                        {
                            case FFXIV.Job.RoleType.StrengthMelee:
                            case FFXIV.Job.RoleType.DexterityMelee: return Color.FromArgb(160, 64, 0);
                            case FFXIV.Job.RoleType.PhysicalRanged: return Color.FromArgb(160, 0, 0);
                            case FFXIV.Job.RoleType.MagicalRanged: return Color.FromArgb(160, 0, 96);
                            default: return Color.FromArgb(128, 128, 128);
                        }
                    default: return Color.FromArgb(128, 128, 128);
                }
            }

            public PlayerLabel(PartyListPanel parent, FFXIV.Entity entity, int order)
            {
                ParentTable = parent;
                ID = entity.Exist ? entity.ID : 0;
                PlayerName = entity.Name;
                SubRole = entity.Job.SubRole;
                JobName = I18n.IsChineseEnvironment
                    ? entity.Job.NameCN2
                    : entity.Job.NameEN3;
                HexID = entity.HexID;
                Order = order;
                ForeColor = GetForeColorByRole();
                Margin = new Padding(10);
                AutoSize = false;
                Anchor = AnchorStyles.None;
                TextAlign = ContentAlignment.MiddleCenter;
                Cursor = Cursors.SizeAll;
                MouseDown += new MouseEventHandler(PlayerLabel_MouseDown);
                MouseMove += new MouseEventHandler(PlayerLabel_MouseMove);
                MouseUp += new MouseEventHandler(PlayerLabel_MouseUp);
            }

            /// <summary> Set the label to the correct position in the parent table according to Order.  </summary>
            public void RefreshLocation()
            {
                int colCount = Math.Min(ParentTable.PlayerCount, 4);
                int row = Order / colCount;
                int col = Order % colCount;
                if (Parent == ParentTable)
                    ParentTable.SetCellPosition(this, new TableLayoutPanelCellPosition(col, row));
                else
                    ParentTable.Controls.Add(this, col, row);
            }

            private void PlayerLabel_MouseDown(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    /*  To-Do
                    _draggingClone = new Label
                    {
                        Text = this.Text,
                        Size = this.Size,
                        BackColor = Color.FromArgb(128, this.BackColor),
                        ForeColor = Color.FromArgb(128, this.ForeColor),
                        Font = this.Font,
                        TextAlign = this.TextAlign,
                    };
                    ParentTable.Parent.Controls.Add(_draggingClone);
                    _draggingClone.BringToFront();
                    _draggingClone.Location = this.Location;
                    */
                    DoDragDrop(this, DragDropEffects.Move);
                }
            }

            private void PlayerLabel_MouseMove(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left && _draggingClone != null)
                {
                    Point newLocation = ParentTable.PointToClient(Cursor.Position);
                    newLocation.Offset(-_draggingClone.Width / 2, -_draggingClone.Height / 2);
                    _draggingClone.Location = newLocation;
                }
            }

            private void PlayerLabel_MouseUp(object sender, MouseEventArgs e)
            {
                if (_draggingClone != null)
                {
                    ParentTable.Controls.Remove(_draggingClone);
                    _draggingClone.Dispose();
                    _draggingClone = null;
                }
            }
        }

    }
}