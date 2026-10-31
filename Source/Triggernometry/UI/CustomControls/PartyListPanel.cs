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
    public class PartyListPanel : ReorderableTableLayoutPanel
    {
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

            // 队员面板默认每行最多显示四人。
            SetGridSize((PlayerCount + 3) / 4, Math.Min(PlayerCount, 4));

            // Read Current Entities and Create PlayerLabels
            var entities = GetSortedPartyMembers();
            for (int i = 0; i < PlayerCount; i++)
            {
                PlayerLabel player = new PlayerLabel(this, entities[i], i);
                AddLabel(player);
            }

            ResumeLayout(true);
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

        /// <summary>位置变化后刷新职能描述，序号直接从网格读取。</summary>
        protected override void OnOrderChanged(EventArgs e)
        {
            foreach (PlayerLabel player in GetLabels())
                player.UpdateText(GetOrder(player));
            base.OnOrderChanged(e);
        }

        /// <summary>仅在已保存 ID 与当前队员完整匹配时恢复顺序。</summary>
        public void LoadFromConfig()
        {
            var savedList = RealPlugin.Instance.GetVariableStore(false).GetListVariable(varNames.IdsList);
            if (savedList == null || savedList.Size != PlayerCount)
                return;

            List<string> storedPlayerIDs = savedList.Values.Select(var => var.ToString()).ToList();

            var ordered = new PlayerLabel[PlayerCount];
            foreach (PlayerLabel player in GetLabels())
            {
                int index = storedPlayerIDs.IndexOf(player.HexID);
                if (index < 0 || ordered[index] != null)
                    return;
                ordered[index] = player;
            }
            SetOrder(ordered);
        }

        /// <summary>按当前队员顺序生成临时变量；空位只保留列表位置，不加入 ID 字典。</summary>
        public void SaveToConfig()
        {
            var players = GetLabels().Cast<PlayerLabel>().ToArray();
            if (players.Length <= 1)
                return;

            var store = RealPlugin.Instance.GetVariableStore(false);
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

            // 按当前位置逐位匹配 4 人 THDD 或 8 人 TTHHDDDD，空位也参与校验。
            var mainRoles = players.Select(p => p.ID != 0 ? p.SubRole & FFXIV.Job.RoleType.MainRole : FFXIV.Job.RoleType.None);
            int roleCount = PlayerCount / 4;
            var expectedRoles = Enumerable.Repeat(FFXIV.Job.RoleType.Tank, roleCount)
                .Concat(Enumerable.Repeat(FFXIV.Job.RoleType.Healer, roleCount))
                .Concat(Enumerable.Repeat(FFXIV.Job.RoleType.DPS, roleCount * 2));
            bool isStandard = (PlayerCount == 4 || PlayerCount == 8) && mainRoles.SequenceEqual(expectedRoles);

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

            /// <summary>当前网格中的序号，不再另存一份位置状态。</summary>
            public int Order => ParentTable.GetOrder(this);

            /// <summary>职能描述属于固定位置，交换队员后随新位置刷新。</summary>
            internal void UpdateText(int order)
                => Text = $"[{ParentTable.PlayerDescriptions[order]}] {JobName}\n" + PlayerName.Replace(" ", "\n");

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
                UpdateText(order);
                ForeColor = GetForeColorByRole();
                MinimumSize = new Size(70, 50);
            }
        }

    }
}
