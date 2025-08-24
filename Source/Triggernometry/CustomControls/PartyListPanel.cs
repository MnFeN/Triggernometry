using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Triggernometry.PluginBridges;
using Triggernometry.Variables;

namespace Triggernometry.CustomControls
{
    public class PartyListPanel : TableLayoutPanel
    {
        List<PlayerLabel> _players = new List<PlayerLabel>();
        public int PlayerCount;
        public string[] PlayerDescriptions;

        public string PlayerNamesLvarName;
        public string PlayerIdsLvarName;
        public string PlayerIdxVarName;

        public PartyListPanel(
            string[] playerDescriptions, 
            string playerNamesLvarName = "pname", 
            string playerIdsLvarName = "party", 
            string PplayerIdxVarName = "myIdx")
        {
            SuspendLayout();

            PlayerNamesLvarName = playerNamesLvarName;
            PlayerIdsLvarName = playerIdsLvarName;
            PlayerIdxVarName = PplayerIdxVarName;

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
            int rowCount = PlayerCount <= 4 ? 1 : 2;
            int colCount = Math.Min(PlayerCount, 4);
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

        private static readonly string[] jobOrder = {
            "WAR", "MRD", "PLD", "GLA", "DRK", "GNB",
            "WHM", "CNJ", "AST", "SGE", "SCH",
            "SAM", "MNK", "PGL", "DRG", "LNC", "NIN", "ROG", "RPR", "VPR",
            "BRD", "ARC", "MCH", "DNC", "BLM", "THM", "PCT", "RDM", "SMN", "ACN", "BLU"
        };

        List<FFXIV.Entity> GetSortedPartyMembers()
        {
            List<FFXIV.Entity> entities = FFXIV.Entity.GetEntities()
                .Where(e => e.HexID.StartsWith("10"))       // is player
                .OrderByDescending(e => e.InParty)          // is party member
                .ThenBy(e => e.Job.SubRole == FFXIV.Job.RoleType.None ? 99 : (int)e.Job.SubRole)  // sort by subrole id
                .ThenBy(e => Array.IndexOf(jobOrder, e.Job.NameEN3))   // customized job order
                .ThenBy(e => e.Job.JobID)                   // unknown jobs: sort by job id
                .ThenBy(e => e.Name)
                .Take(PlayerCount)
                .ToList();

            while (entities.Count < PlayerCount)
            {
                entities.Add(new FFXIV.Entity());
            }

            if (entities.Count == 8     // Double Caster => D2 / D4
                && entities[5].Job.SubRole == FFXIV.Job.RoleType.PhysicalRanged
                && entities[6].Job.SubRole == FFXIV.Job.RoleType.MagicalRanged
                && entities[7].Job.SubRole == FFXIV.Job.RoleType.MagicalRanged)
            {
                (entities[5], entities[6]) = (entities[6], entities[5]);
                if (entities[7].Job.NameEN3 == "BLM") // with BLM: BLM D2
                {
                    (entities[5], entities[7]) = (entities[7], entities[5]);
                }
            }
            return entities;
        }

        private void PartyListPanel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PlayerLabel)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        /// <summary> Get dragged label and target label, then update Order.</summary>
        private void PartyListPanel_DragDrop(object sender, DragEventArgs e)
        {
            PlayerLabel draggedLabel = (PlayerLabel)e.Data.GetData(typeof(PlayerLabel));
            Point clientPoint = PointToClient(new Point(e.X, e.Y));
            Control control = GetChildAtPoint(clientPoint);
            if (control != null && control is PlayerLabel targetLabel && draggedLabel != targetLabel)
            {
                Parent?.SuspendLayout();
                SwapLabels(draggedLabel, targetLabel);
                Parent?.ResumeLayout(false);
            }
        }

        private void SwapLabels(PlayerLabel draggedLabel, PlayerLabel targetLabel)
        {
            (draggedLabel.Order, targetLabel.Order) = (targetLabel.Order, draggedLabel.Order);
        }

        public void LoadFromConfig()
        {
            if (!RealPlugin.plug.sessionvars.List.TryGetValue(PlayerIdsLvarName, out VariableList savedList) || savedList.Size != PlayerCount)
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
                for (int i = 0; i < PlayerCount; i++)
                {
                    _players[i].Order = indices[i];
                }
            }
        }

        public void SaveToConfig()
        {
            if (_players.Count <= 1) return;

            _players = _players.OrderBy(p => p.Order).ToList();

            VariableList hexIDList = new VariableList();
            VariableList nameList = new VariableList();
            VariableDictionary hexIDDict = new VariableDictionary();
            VariableDictionary nameDict = new VariableDictionary();
            string changer = "PartyList";

            foreach (var label in _players)
            {
                Variable hexID = new VariableScalar(label.HexID);
                Variable name = new VariableScalar(label.PlayerName);
                string description = PlayerDescriptions[label.Order];

                hexIDList.Push(hexID, changer);
                nameList.Push(name, changer);
                hexIDDict.SetValue(description, hexID, changer);
                nameDict.SetValue(description, name, changer);

                if (BridgeFFXIV.PlayerHexId == label.HexID) // var:myIdx
                {
                    var idx = label.Order + 1;
                    RealPlugin.plug.sessionvars.Scalar[PlayerIdxVarName] = new VariableScalar(idx);
                }
            }
            RealPlugin.plug.sessionvars.List[PlayerIdsLvarName] = hexIDList;
            RealPlugin.plug.sessionvars.List[PlayerNamesLvarName] = nameList;
        }

        public class PlayerLabel : Label
        {
            public PartyListPanel ParentTable;
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
                PlayerName = entity.Name;
                SubRole = entity.Job.SubRole;
                JobName = CultureInfo.CurrentCulture.Name.StartsWith("zh-")
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