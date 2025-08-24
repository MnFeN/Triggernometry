using System;
using System.Numerics;
using static Triggernometry.Utilities.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class ExecuteCommandModule : ModuleBase
    {
        public IntPtr ExecuteCommandPtr;
        public IntPtr ExecuteCommandTgtPtr;
        public IntPtr ExecuteCommandPosPtr;

        public ExecuteCommandModule()
        {
            ScanMethod = () =>
            {
                ExecuteCommandPtr = Scanner.TryScan(
                    "E8 * * * * 48 83 C4 ? C3 CC CC CC CC CC CC CC CC CC CC CC CC 48 83 EC ? 45 0F B6 C0", nameof(ExecuteCommandPtr));
                ExecuteCommandTgtPtr = Scanner.TryScan(
                    "E8 * * * * 80 7D ? ? 74 ? 41 0F B6 45", nameof(ExecuteCommandTgtPtr));
                ExecuteCommandPosPtr = Scanner.TryScan( /*"E8 * * * * EB ? 48 8B 53 ? 33 C0" 7.2 */
                    "E8 * * * * 0F 28 74 24 ?? 0F 28 7C 24 ?? 48 8B 74 24 ??", nameof(ExecuteCommandPosPtr)); // 7.3 兼容 7.2
            };
        }

        [CallbackMethod("Exec", tag: "Kairos+")]
        internal void CbExecuteCommand(string command)
        {
            CheckBeforeExecution(command);
            var (cmd, a1, a2, a3, a4) = ParseArgs<ExecuteCommandFlag, uint, uint, uint, uint>(command, (1, 0), (2, 0), (3, 0), (4, 0));
            Memory.ExecuteWithLock(() => ExecuteCommand(cmd, a1, a2, a3, a4));
        }

        [CallbackMethod("StatusOff")]
        internal void CbStatusOff(string command)
        {
            CheckBeforeExecution(command);
            var (statusID, srcID) = ParseArgs<ushort, uint>(command, (1, 0xE0000000));
            StatusOff(statusID, srcID);
        }

        [CallbackMethod("ExecTgt", tag: "Kairos+")]
        internal void CbExecuteCommandTgt(string command)
        {
            CheckBeforeExecution(command);
            var (cmd, tgt, a1, a2, a3, a4) = ParseArgs<ExecuteCommandComplexFlag, HexOrDecId, uint, uint, uint, uint>(
                command, (1, 0xE0000000), (2, 0), (3, 0), (4, 0), (5, 0)
            );
            Memory.ExecuteWithLock(() => ExecuteCommandTgt(cmd, tgt, a1, a2, a3, a4));
        }

        [CallbackMethod("ExecPos", tag: "Kairos+")]
        internal void CbExecuteCommandPos(string command)
        {
            CheckBeforeExecution(command);
            var (cmd, x, y, z, a1, a2, a3, a4) = ParseArgs<ExecuteCommandComplexFlag, double, double, double, uint, uint, uint, uint>(
                command, (1, 0.0), (2, 0.0), (3, 0.0), (4, 0), (5, 0), (6, 0), (7, 0)
            );
            Memory.ExecuteWithLock(() => ExecuteCommandPos(cmd, x, y, z, a1, a2, a3, a4));
        }

        [CallbackMethod("TeleportDive", tag: "Kairos+")]
        internal void CbTeleportDive(string command)
        {
            CheckBeforeExecution(command);
            double x, y, z;
            if (string.IsNullOrWhiteSpace(command))
            {
                var me = Triggernometry.FFXIV.Entity.GetMyself();
                (x, y, z) = (me.PosX, me.PosY, me.PosZ);
            }
            else
            {
                (x, y, z) = ParseArgs<double, double, double>(command, (2, 0.0));
            }
            Memory.ExecuteWithLock(() => ExecuteCommandPos(ExecuteCommandComplexFlag.DiveEnd, x, y, z));
        }

        // bool 似乎都应该是 void？

        public bool ExecuteCommand(ExecuteCommandFlag command, 
            uint a1 = 0, uint a2 = 0, uint a3 = 0, uint a4 = 0, bool log = true)
        {
            CheckIfAnyZeroPtr(ExecuteCommandPtr);
            var result = Memory.CallInjected64<bool>(ExecuteCommandPtr, (uint)command, a1, a2, a3, a4);
            if (log)
            {
                string cmdName = Enum.GetName(typeof(ExecuteCommandFlag), command) ?? "Unknown";
                NamazuLog($"[ExecCmd] {(uint)command} ({cmdName}), {FormatParam(a1)}, {FormatParam(a2)}, {FormatParam(a3)}, {FormatParam(a4)}");
            }
            return result;
        }

        public void StatusOff(uint statusID, uint srcID = 0xE0000000)
            => ExecuteCommand(ExecuteCommandFlag.StatusOff, statusID, 0, srcID, 0);

        public bool ExecuteCommandTgt(ExecuteCommandComplexFlag command, uint targetId = 0xE0000000, 
            uint a1 = 0, uint a2 = 0, uint a3 = 0, uint a4 = 0, bool log = true)
        {
            CheckIfAnyZeroPtr(ExecuteCommandTgtPtr);
            var result = Memory.CallInjected64<bool>(ExecuteCommandTgtPtr, (uint)command, targetId, a1, a2, a3, a4);
            if (log)
            {
                string cmdName = Enum.GetName(typeof(ExecuteCommandComplexFlag), command) ?? "Unknown";
                NamazuLog($"[ExecCmdTgt] {(uint)command} ({cmdName}) => 0x{targetId:X8}, {FormatParam(a1)}, {FormatParam(a2)}, {FormatParam(a3)}, {FormatParam(a4)}");
            }
            return result;
        }

        public bool ExecuteCommandPos(ExecuteCommandComplexFlag command, Vector3 pos, 
            uint a1 = 0, uint a2 = 0, uint a3 = 0, uint a4 = 0, bool log = true)
            => ExecuteCommandPos(command, pos.X, pos.Y, pos.Z, a1, a2, a3, a4, log);
        
        public bool ExecuteCommandPos(ExecuteCommandComplexFlag command, double x = 0, double y = 0, double z = 0, 
            uint a1 = 0, uint a2 = 0, uint a3 = 0, uint a4 = 0, bool log = true)
        {
            CheckIfAnyZeroPtr(ExecuteCommandPosPtr);
            var pos = new Vector3((float)x, (float)z, (float)y);
            IntPtr posPtr = default;
            bool result = default;
            try
            {
                posPtr = Memory.AllocateMemory(0x10);
                Memory.Write(posPtr, pos);
                result = Memory.CallInjected64<bool>(ExecuteCommandPosPtr, (uint)command, posPtr, a1, a2, a3, a4);
            }
            finally
            { 
                if (posPtr != default)
                    Memory.FreeMemory(posPtr);
            }
            if (log)
            {
                string cmdName = Enum.GetName(typeof(ExecuteCommandComplexFlag), command) ?? "Unknown";
                NamazuLog($"[ExecPos] {(uint)command} ({cmdName}) @ ({x:0.###}, {y:0.###}, {z:0.###}), {FormatParam(a1)}, {FormatParam(a2)}, {FormatParam(a3)}, {FormatParam(a4)}");
            }
            return result;
        }

        public bool TeleportDive()
        {
            var me = Triggernometry.FFXIV.Entity.GetMyself();
            return TeleportDive(me.PosX, me.PosY, me.PosZ);
        }

        public bool TeleportDive(double x, double y, double z)
            => ExecuteCommandPos(ExecuteCommandComplexFlag.DiveEnd, x, y, z);

        private string FormatParam(uint p) => p > 9 ? $"{p} (0x{p:X})" : $"{p}";

    }

    public enum ExecuteCommandFlag
    {
        /// <summary>
        /// 拔出/收回武器
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 1 - 拔出, 0 - 收回</para>
        /// <para><c>param2</c>: 未知, 固定为 1</para>
        /// </remarks>
        DrawOrSheatheWeapon = 1,

        /// <summary>
        /// 自动攻击
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 是否开启自动攻击 (0 - 否, 1 - 是)</para>
        /// <para><c>param2</c>: 目标对象ID</para>
        /// <para><c>param3</c>: 是否为手动操作 (0 - 否, 1 - 是)</para>
        /// </remarks>
        AutoAttack = 2,

        /// <summary>
        /// 选中目标
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 目标 Object ID (无目标为: -536870912, 即 int.MinValue / 4)</para>
        /// </remarks>
        Target = 3,

        /// <summary>
        /// 下坐骑
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 0 - 不进入队列; 1 - 进入队列</para>
        /// </remarks>
        Dismount = 101,

        /// <summary>
        /// 召唤宠物
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 宠物 ID</para>
        /// </remarks>
        SummonPet = 102,

        /// <summary>
        /// 收回宠物
        /// </summary>
        WithdrawPet = 103,

        /// <summary>
        /// 取消身上指定的状态效果
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Status ID</para>
        /// <para><c>param3</c>: 自身的 GameObjectID</para>
        /// </remarks>
        StatusOff = 104,

        /// <summary>
        /// 中断咏唱
        /// </summary>
        CancelCast = 105,

        /// <summary>
        /// 共同骑乘
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 目标 ID</para>
        /// <para><c>param2</c>: 位置索引</para>
        /// </remarks>
        RidePillion = 106,

        /// <summary>
        /// 收起时尚配饰
        /// </summary>
        WithdrawParasol109 = 109,

        /// <summary>
        /// 收起时尚配饰
        /// </summary>
        WithdrawParasol110 = 110,

        /// <summary>
        /// 复活
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 操作 (5 - 接受复活; 8 - 确认返回返回点)</para>
        /// </remarks>
        Revive = 200,

        /// <summary>
        /// 区域变更
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 变更方式</para>
        /// <list type="table">
        ///     <item>
        ///         <term>1</term>
        ///         <description>NPC 传送</description>
        ///     </item>
        ///     <item>
        ///         <term>3</term>
        ///         <description>边界过图</description>
        ///     </item>
        ///     <item>
        ///         <term>4</term>
        ///         <description>正常传送</description>
        ///     </item>
        ///     <item>
        ///         <term>7</term>
        ///         <description>返回</description>
        ///     </item>
        ///     <item>
        ///         <term>15</term>
        ///         <description>城内以太之晶</description>
        ///     </item>
        ///     <item>
        ///         <term>20</term>
        ///         <description>房区</description>
        ///     </item>
        /// </list>
        /// <para><c>param2</c>: 区域内位置变更方式</para>
        /// <list type="table">
        ///     <item>
        ///         <term>1</term>
        ///         <description>剧情转移</description>
        ///     </item>
        ///     <item>
        ///         <term>2</term>
        ///         <description>返回到安全区</description>
        ///     </item>
        ///     <item>
        ///         <term>25</term>
        ///         <description>副本内过图</description>
        ///     </item>
        ///     <item>
        ///         <term>26</term>
        ///         <description>潜水</description>
        ///     </item>
        /// </list>
        /// </remarks>
        TerritoryTransport = 201,

        /// <summary>
        /// 传送至指定的以太之光
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 以太之光 ID</para>
        /// <para><c>param2</c>: 是否使用传送券 (0 - 否, 1 - 是)</para>
        /// <para><c>param3</c>: 以太之光 Sub ID</para>
        /// </remarks>
        Teleport = 202,

        /// <summary>
        /// 接受传送邀请
        /// </summary>
        AcceptTeleportOffer = 203,

        /// <summary>
        /// 请求好友房屋传送信息
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 未知</para>
        /// <para><c>param2</c>: 未知</para>
        /// </remarks>
        RequestFriendHouseTeleport = 210,

        /// <summary>
        /// 传送至好友房屋
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 未知</para>
        /// <para><c>param2</c>: 未知</para>
        /// <para><c>param3</c>: 以太之光 ID (个人房屋 - 61, 部队房屋 - 57)</para>
        /// <para><c>param4</c>: 以太之光 Sub ID (疑似恒定为 1)</para>
        /// </remarks>
        TeleportToFriendHouse = 211,

        /// <summary>
        /// 若当前种族不是拉拉菲尔族, 则返回至当前地图的最近安全点
        /// </summary>
        ReturnIfNotLalafell = 213,

        /// <summary>
        /// 立即返回至返回点, 若在副本内则返回至副本内重生点
        /// </summary>
        InstantReturn = 214,

        /// <summary>
        /// 检查指定玩家
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 待对象 Object ID</para>
        /// </remarks>
        Inspect = 300,

        /// <summary>
        /// 更改佩戴的称号
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 称号 ID</para>
        /// </remarks>
        ChangeTitle = 302,

        /// <summary>
        /// 请求过场剧情数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 过场剧情在 Cutscene.csv 中的对应索引</para>
        /// </remarks>
        RequestCutscene307 = 307,

        /// <summary>
        /// 请求挑战笔记具体类别下数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 类别索引 (从 1 开始)</para>
        /// </remarks>
        RequestContentsNoteCategory = 310,

        /// <summary>
        /// 清除场地标点
        /// </summary>
        ClearFieldMarkers = 313,

        /// <summary>
        /// 将青魔法师技能交换或应用于有效技能
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 类型 (0 为应用有效技能, 1 为交换有效技能)</para>
        /// <para><c>param2</c>: 格子序号 (从 0 开始, 小于 24)</para>
        /// <para><c>param3</c>: 技能 ID / 格子序号 (从 0 开始, 小于 24)</para>
        /// </remarks>
        AssignBLUActionToSlot = 315,

        /// <summary>
        /// 请求跨界传送数据
        /// </summary>
        RequestWorldTravel = 316,

        /// <summary>
        /// 放置场地标点
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 标点索引</para>
        /// <para><c>param2</c>: 坐标 X * 1000</para>
        /// <para><c>param3</c>: 坐标 Y * 1000</para>
        /// <para><c>param4</c>: 坐标 Z * 1000</para>
        /// </remarks>
        PlaceFieldMarker = 317,

        /// <summary>
        /// 移除场地标点
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 标点索引</para>
        /// </remarks>
        RemoveFieldMarker = 318,

        /// <summary>
        /// 清除来自木人的仇恨
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 木人的 Object ID</para>
        /// </remarks>
        ResetStrikingDummy = 319,

        /// <summary>
        /// 请求指定物品栏数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: (int)InventoryType</para>
        /// </remarks>
        RequestInventory = 405,

        /// <summary>
        /// 进入镶嵌魔晶石状态
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 物品 ID</para>
        /// </remarks>
        EnterMateriaAttachState = 408,

        /// <summary>
        /// 退出镶嵌魔晶石状态
        /// </summary>
        LeaveMateriaAttachState = 410,

        /// <summary>
        /// 取消魔晶石镶嵌委托
        /// </summary>
        CancelMateriaMeldRequest = 419,

        /// <summary>
        /// 请求收藏柜的数据
        /// </summary>
        RequestCabinet = 424,

        /// <summary>
        /// 存入物品至收藏柜
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 物品在 Cabinet.csv 中的对应索引</para>
        /// </remarks>
        StoreToCabinet = 425,

        /// <summary>
        /// 从收藏柜中取回物品
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 物品在 Cabinet.csv 中的对应索引</para>
        /// </remarks>
        RestoreFromCabinet = 426,

        /// <summary>
        /// 维修装备
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Inventory Type</para>
        /// <para><c>param2</c>: Inventory Slot</para>
        /// <para><c>param3</c>: Item ID</para>
        /// </remarks>
        RepairItem = 434,

        /// <summary>
        /// 批量维修装备中装备
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Inventory Type (固定为 1000)</para>
        /// </remarks>
        RepairEquippedItems = 435,

        /// <summary>
        /// 批量维修装备
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 分类 (0 - 主手/副手; 1 - 头部/身体/手臂; 2 - 腿部/脚部; 3 - 耳部;颈部; 4 - 腕部;戒指; 5 - 物品)</para>
        /// </remarks>
        RepairAllItems = 436,

        /// <summary>
        /// 精制魔晶石
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Inventory Type</para>
        /// <para><c>param1</c>: Inventory Slot</para>
        /// </remarks>
        ExtractMateria = 437,

        /// <summary>
        /// 更换套装
        /// </summary>
        GearsetChange = 441,

        /// <summary>
        /// 请求陆行鸟鞍囊的数据
        /// </summary>
        RequestSaddleBag = 444,

        /// <summary>
        /// 打断当前正在进行的情感动作
        /// </summary>
        InterruptEmote = 502,

        /// <summary>
        /// 打断当前正在进行的特殊情感动作
        /// </summary>
        InterruptEmoteSpecial = 503,

        /// <summary>
        /// 更改闲置状态姿势
        /// </summary>
        /// <remarks>
        /// <para><c>param2</c>: 姿势索引</para>
        /// </remarks>
        IdlePostureChange = 505,

        /// <summary>
        /// 进入闲置状态姿势
        /// </summary>
        /// <remarks>
        /// <para><c>param2</c>: 姿势索引</para>
        /// </remarks>
        IdlePostureEnter = 506,

        /// <summary>
        /// 退出闲置状态姿势
        /// </summary>
        IdlePostureExit = 507,

        /// <summary>
        /// 进入游泳状态 (也会强制下坐骑)
        /// </summary>
        EnterSwim = 608,

        /// <summary>
        /// 退出游泳状态
        /// </summary>
        LeaveSwim = 609,

        /// <summary>
        /// 赋予/取消禁止骑乘坐骑状态
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 0 - 取消; 1 - 赋予</para>
        /// </remarks>
        DisableMounting = 612,

        /// <summary>
        /// 进入飞行状态
        /// </summary>
        EnterFlight = 616,

        /// <summary>
        /// 生产
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 类型 (0 - 普通制作, 1 - 简易制作; 2 - 制作练习)</para>
        /// <para><c>param2</c>: 配方 ID (在 Recipe.csv 中)</para>
        /// <para><c>param3</c>: 额外参数 (简易制作 - 数量, 最多 255 个)</para>
        /// </remarks>
        Craft = 700,

        /// <summary>
        /// 钓鱼
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 动作 (0 - 抛竿, 1 - 收杆, 2 - 提钩, 4 - 换饵, 10 - 强力提杆, 11 - 精准提钩, 13 - 耐心, 14 - 耐心2, 24 - 熟练妙招, 25 - 游动饵)
        /// </para>
        /// <para><c>param2</c>: 额外参数 (若为换饵, 则为物品 ID; 若为游动饵, 则为饵索引)</para>
        /// </remarks>
        Fish = 701,

        /// <summary>
        /// 加载制作笔记数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 职业索引 (从左至右, 从 0 开始, 至 7 结束)</para>
        /// </remarks>
        LoadCraftLog = 710,

        /// <summary>
        /// 结束制作
        /// </summary>
        ExitCraft = 711,

        /// <summary>
        /// 放弃任务
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 任务 ID (非 RowID)</para>
        /// </remarks>
        AbandonQuest = 800,

        /// <summary>
        /// 刷新理符任务状态
        /// </summary>
        RefreshLeveQuest = 801,

        /// <summary>
        /// 开始理符任务
        /// <remarks>
        /// <para><c>param1</c>: 理符任务 ID</para>
        /// <para><c>param2</c>: 要提高的等级数</para>
        /// </remarks>
        /// </summary>
        StartLeveQuest = 804,

        /// <summary>
        /// 副本相关
        /// </summary>
        Content = 808,

        /// <summary>
        /// 开始指定的临危受命任务
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: FATE ID</para>
        /// <para><c>param2</c>: 目标 Object ID</para>
        /// </remarks>
        FateStart = 809,

        /// <summary>
        /// 加载临危受命信息
        /// (在切换地图时会一次性加载完地图内所有 FATE 信息)
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: FATE ID</para>
        /// </remarks>
        FateLoad = 810,

        /// <summary>
        /// 进入 临危受命 范围 (若 FATE 在脚底下生成则不会发送该命令)
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: FATE ID</para>
        /// </remarks>
        FateEnter = 812,

        /// <summary>
        /// 为 临危受命 等级同步
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: FATE ID</para>
        /// <para><c>param2</c>: 是否等级同步 (0 - 否, 1 - 是)</para>
        /// </remarks>
        FateLevelSync = 813,

        /// <summary>
        /// 临危受命 野怪生成
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Object ID</para>
        /// </remarks>
        FateMobSpawn = 814,

        /// <summary>
        /// 区域变更完成
        /// </summary>
        TerritoryTransportFinish = 816,

        /// <summary>
        /// 离开副本
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 类型 (0 - 正常退本, 1 - 一段时间未操作)</para>
        /// </remarks>
        LeaveDuty = 819,

        /// <summary>
        /// 昔日重现模式
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: QuestRedo.csv 中对应的昔日重现章节序号 (0 - 退出昔日重现)</para>
        /// </remarks>
        QuestRedo = 824,

        /// <summary>
        /// 刷新物品栏
        /// </summary>
        InventoryRefresh = 830,

        /// <summary>
        /// 请求过场剧情数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 过场剧情在 Cutscene.csv 中的对应索引</para>
        /// </remarks>
        RequestCutscene831 = 831,

        /// <summary>
        /// 请求成就进度数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 成就在 Achievement.csv 中的对应索引</para>
        /// </remarks>
        RequestAchievement = 1000,

        /// <summary>
        /// 请求所有成就概览 (不含具体成就内容)
        /// </summary>
        RequestAllAchievement = 1001,

        /// <summary>
        /// 请求接近达成成就概览 (不含具体成就内容)
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 未知, 固定为 1</para>
        /// </remarks>
        RequestNearCompletionAchievement = 1002,

        /// <summary>
        /// 请求抽选数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Territory Type</para>
        /// <para><c>param2</c>: 地皮对应索引</para>
        /// <code>
        /// <![CDATA[
        /// const int HousesPerArea = 60;
        /// const int AreaOffset = 256;
        /// 
        /// // 第 1 区 第 41 号
        /// var wardID = 0;
        /// var districtOffset = wardID * AreaOffset;
        /// var houseID = 40;
        /// var position = districtOffset + houseID]]>
        /// </code>
        /// </remarks>
        RequestLotteryData = 1105,

        /// <summary>
        /// 请求门牌数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Territory Type</para>
        /// <para><c>param2</c>: 地皮对应索引</para>
        /// <code>
        /// <![CDATA[
        /// const int HousesPerArea = 60;
        /// const int AreaOffset = 256;
        /// 
        /// // 第 1 区 第 41 号
        /// var wardID = 0;
        /// var districtOffset = wardID * AreaOffset;
        /// var houseID = 40;
        /// var position = districtOffset + houseID]]>
        /// </code>
        /// </remarks>
        RequestPlacardData = 1106,

        /// <summary>
        /// 请求住宅区数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Territory Type</para>
        /// <para><c>param2</c>: 分区索引</para>
        /// </remarks>
        RequestHousingAreaData = 1107,

        /// <summary>
        /// 向房屋仓库存入指定的物品
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: HouseManager 相关区域的 HouseID 地址的高 32 位</para>
        /// <code>*(long*)((nint)HousingManager.Instance()->IndoorTerritory + 38560) >> 32</code>
        /// <para><c>param2</c>: HouseManager 相关区域的 HouseID</para>
        /// <para><c>param3</c>: InventoryType</para>
        /// <para><c>param4</c>: InventorySlot</para>
        /// </remarks>
        StoreFurniture = 1112,

        /// <summary>
        /// 从房屋中取回指定的家具
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: HouseManager 相关区域的 HouseID 地址的高 32 位</para>
        /// <code>(long)((nint)HousingManager.Instance()->IndoorTerritory + 38560) >> 32</code>
        /// <para><c>param2</c>: HouseManager 相关区域的 HouseID</para>
        /// <para><c>param3</c>: InventoryType (25000 至 25010 / 27000 至 27008)</para>
        /// <para><c>param4</c>: InventorySlot (若 >65535 则将 slot 为 (i - 65536) 的家具收入仓库)</para>
        /// </remarks>
        RestoreFurniture = 1113,

        /// <summary>
        /// 请求房屋名称设置数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: HouseManager 相关区域的 HouseID 地址的高 32 位</para>
        /// <code>*(long*)((nint)HousingManager.Instance()->IndoorTerritory + 38560) >> 32</code>
        /// <para><c>param2</c>: HouseManager 相关区域的 HouseID</para>
        /// </remarks>
        RequestHousingName = 1114,

        /// <summary>
        /// 请求房屋问候语设置数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: HouseManager 相关区域的 HouseID 地址的高 32 位</para>
        /// <code>*(long*)((nint)HousingManager.Instance()->IndoorTerritory + 38560) >> 32</code>
        /// <para><c>param2</c>: HouseManager 相关区域的 HouseID</para>
        /// </remarks>
        RequestHousingGreeting = 1115,

        /// <summary>
        /// 请求房屋访客权限设置数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: HouseManager 相关区域的 HouseID 地址的高 32 位</para>
        /// <code>*(long*)((nint)HousingManager.Instance()->IndoorTerritory + 38560) >> 32</code>
        /// <para><c>param2</c>: HouseManager 相关区域的 HouseID</para>
        /// </remarks>
        RequestHousingGuestAccess = 1117,

        /// <summary>
        /// 保存房屋访客权限设置
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: HouseManager 相关区域的 HouseID 地址的高 32 位</para>
        /// <code>*(long*)((nint)HousingManager.Instance()->IndoorTerritory + 38560) >> 32</code>
        /// <para><c>param2</c>: HouseManager 相关区域的 HouseID</para>
        /// <para><c>param3</c>: 设置枚举值组合 (已知: 1 - 传送权限; 65536 - 进入权限)</para>
        /// </remarks>
        SaveHousingGuestAccess = 1118,

        /// <summary>
        /// 请求房屋宣传设置数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: HouseManager 相关区域的 HouseID 地址的高 32 位</para>
        /// <code>*(long*)((nint)HousingManager.Instance()->IndoorTerritory + 38560) >> 32</code>
        /// <para><c>param2</c>: HouseManager 相关区域的 HouseID</para>
        /// </remarks>
        RequestHousingEstateTag = 1119,

        /// <summary>
        /// 保存房屋宣传设置
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: HouseManager 相关区域的 HouseID 地址的高 32 位</para>
        /// <code>*(long*)((nint)HousingManager.Instance()->IndoorTerritory + 38560) >> 32</code>
        /// <para><c>param2</c>: HouseManager 相关区域的 HouseID</para>
        /// <para><c>param3</c>: 设置枚举值组合 (注: 即使是相同名称的 Tag 在不同位置上对应的枚举值也不同)</para>
        /// </remarks>
        SaveHousingEstateTag = 1120,

        /// <summary>
        /// 移动到庭院门前
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 地块索引</para>
        /// </remarks>
        MoveToHouseFrontGate = 1122,

        /// <summary>
        /// 进入到"布置家具/庭具"状态
        /// </summary>
        /// <remarks>
        /// <para><c>param2</c>: 房屋地块索引 (公寓为 0)</para>
        /// </remarks>
        FurnishState = 1123,

        /// <summary>
        /// 查看房屋详情
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Territory Type</para>
        /// <para><c>param2</c>: 地皮对应索引</para>
        /// <code>
        /// <![CDATA[
        /// const int HousesPerArea = 60;
        /// const int AreaOffset = 256;
        /// 
        /// // 第 1 区 第 41 号
        /// var wardID = 0;
        /// var districtOffset = wardID * AreaOffset;
        /// var houseID = 40;
        /// var position = districtOffset + houseID]]>
        /// </code>
        /// <para><c>param3</c>: (若有)公寓房间索引</para>
        /// </remarks>
        ViewHouseDetail = 1126,

        /// <summary>
        /// 调整房间亮度
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 亮度等级 (最亮为 0, 最暗为 5)</para>
        /// </remarks>
        AdjustHouseLight = 1137,

        /// <summary>
        /// 刷新部队合建物品交纳信息
        /// </summary>
        RefreshFCMaterialDelivery = 1143,

        /// <summary>
        /// 设置房屋背景音乐
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 管弦乐曲在 Orchestrion.csv 中的对应索引</para>
        /// </remarks>
        SetHouseBackgroundMusic = 1145,

        /// <summary>
        /// 从房屋仓库中取出布置指定物品
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: HouseManager 相关区域的 HouseID 地址的高 32 位</para>
        /// <code>*(long*)((nint)HousingManager.Instance()->IndoorTerritory + 38560) >> 32</code>
        /// <para><c>param2</c>: HouseManager 相关区域的 HouseID</para>
        /// <para><c>param3</c>: InventoryType (25000 至 25010 / 27000 至 27008)</para>
        /// <para><c>param4</c>: InventorySlot</para>
        /// </remarks>
        Furnish = 1150,

        /// <summary>
        /// 修理潜水艇部件
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 潜水艇索引</para>
        /// <para><c>param2</c>: 潜水艇部件索引</para>
        /// </remarks>
        RepairSubmarinePart = 1153,

        /// <summary>
        /// 领取战利水晶
        /// </summary>
        CollectTrophyCrystal = 1200,

        /// <summary>
        /// 请求挑战笔记数据
        /// </summary>
        RequestContentsNote = 1301,

        /// <summary>
        /// 在 NPC 处维修装备
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Inventory Type</para>
        /// <para><c>param2</c>: Inventory Slot</para>
        /// <para><c>param3</c>: Item ID</para>
        /// </remarks>
        RepairItemNPC = 1600,

        /// <summary>
        /// 在 NPC 处批量维修装备
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 分类 (0 - 主手/副手; 1 - 头部/身体/手臂; 2 - 腿部/脚部; 3 - 耳部;颈部; 4 - 腕部;戒指; 5 - 物品)</para>
        /// </remarks>
        RepairAllItemsNPC = 1601,

        /// <summary>
        /// 在 NPC 处批量维修装备中装备
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Inventory Type (固定为 1000)</para>
        /// </remarks>
        RepairEquippedItemsNPC = 1602,

        /// <summary>
        /// 切换陆行鸟作战风格
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: BuddyAction.csv 中的对应索引</para>
        /// </remarks>
        BuddyAction = 1700,

        /// <summary>
        /// 陆行鸟装甲
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 部位 (0 - 头部, 1 - 身体, 2 - 腿部)</para>
        /// <para><c>param2</c>: 在 BuddyEquip.csv 中对应的装备索引 (0 - 卸下装备)</para>
        /// </remarks>
        BuddyEquip = 1701,

        /// <summary>
        /// 陆行鸟学习技能
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: Skill 索引</para>
        /// </remarks>
        BuddyLearnSkill = 1702,

        /// <summary>
        /// 请求金碟游乐场面板 整体 信息
        /// </summary>
        RequestGSGeneral = 1850,

        /// <summary>
        /// 请求金碟游乐场面板 萌宠之王 信息
        /// </summary>
        RequestGSLordofVerminion = 2010,

        /// <summary>
        /// 启用/解除自动加入新人频道设置
        /// </summary>
        EnableAutoJoinNoviceNetwork = 2102,

        /// <summary>
        /// 发起决斗
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 被决斗对象的 GameObject ID</para>
        /// </remarks>
        SendDuel = 2200,

        /// <summary>
        /// 确认决斗申请
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 0 - 确认; 1 - 取消</para>
        /// </remarks>
        RequestDuel = 2201,

        /// <summary>
        /// 同意决斗
        /// </summary>
        ConfirmDuel = 2202,

        /// <summary>
        /// 确认天书奇谈副本结果
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 索引 (从左到右从上到下, 从 0 开始)</para>
        /// </remarks>
        WondrousTailsConfirm = 2253,

        /// <summary>
        /// 天书奇谈其他操作
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 操作 (0 - 再想想)</para>
        /// <para><c>param2</c>: 索引 (从左到右从上到下, 从 0 开始)</para>
        /// </remarks>
        WondrousTailsOperate = 2253,

        /// <summary>
        /// 请求投影台数据
        /// </summary>
        RequestPrismBox = 2350,

        /// <summary>
        /// 取出投影台物品
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 投影台内部物品 ID (MirageManager.Instance().PrismBoxItemIds)</para>
        /// </remarks>
        RestorePrsimBoxItem = 2352,

        /// <summary>
        /// 请求投影模板数据
        /// </summary>
        RequestGlamourPlates = 2355,

        /// <summary>
        /// 进入/退出投影模板选择状态
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 0 - 退出, 1 - 进入</para>
        /// <para><c>param2</c>: 未知, 可能为 0 或 1</para>
        /// </remarks>
        EnterGlamourPlateState = 2356,

        /// <summary>
        /// 应用投影模板 (需要先进入投影模板选择状态)
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 投影模板索引</para>
        /// </remarks>
        ApplyGlamourPlate = 2357,

        /// <summary>
        /// 请求金碟游乐场面板 多玛方城战 信息
        /// </summary>
        RequestGSMahjong = 2550,

        /// <summary>
        /// 请求青魔法书数据
        /// </summary>
        RequstAOZNotebook = 2601,

        /// <summary>
        /// 请求亲信战友数据
        /// </summary>
        RequestTrustedFriend = 2651,

        /// <summary>
        /// 请求剧情辅助器数据
        /// </summary>
        RequestDutySupport = 2653,

        /// <summary>
        /// 分解指定的物品 / 回收指定物品的魔晶石 / 精选指定物品
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 分解: 3735552; 回收魔晶石: 3735553; 精选: 3735554</para>
        /// <para><c>param2</c>: Inventory Type</para>
        /// <para><c>param3</c>: Inventory Slot</para>
        /// <para><c>param4</c>: 物品 ID</para>
        /// </remarks>
        Desynthesize = 2800,

        /// <summary>
        /// 请求肖像列表数据
        /// </summary>
        RequestPortraits = 3200,

        /// <summary>
        /// 请求无人岛工房排班数据
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 具体天数 (0 为本周期第一天, 7 为下周期第一天)</para>
        /// </remarks>
        MJIWorkshopRequest = 3254,

        /// <summary>
        /// 请求无人岛工房排班物品数据
        /// </summary>
        MJIWorkshopRequestItem = 3258,

        /// <summary>
        /// 无人岛工房排班
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 物品和排班时间段: (8 * (startingHour | (32 * craftObjectId)))</para>
        /// <para><c>param2</c>: 具体天数 (0 - 本周期第一天, 7 - 下周期第一天)</para>
        /// <para><c>param4</c>: 添加/删除 (0 - 添加, 1 - 删除)</para>
        /// </remarks>
        MJIWorkshopAssign = 3259,

        /// <summary>
        /// 收取无人岛屯货仓库探索结果
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 仓库索引</para>
        /// </remarks>
        MJIGranaryCollect = 3262,

        /// <summary>
        /// 无人岛屯货仓库派遣探险
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 仓库索引</para>
        /// <para><c>param2</c>: 目的地索引</para>
        /// <para><c>param3</c>: 探索天数</para>
        /// </remarks>
        MJIGranaryAssign = 3264,

        /// <summary>
        /// 领取无人岛牧场托管结果
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: MJIManager.Instance()->PastureHandler->AvailableMammetLeavings</para>
        /// 需要依次遍历该 Map 并用每个键值对的值来执行指令
        /// </remarks>
        MJIPastureCollect = 3271,

        /// <summary>
        /// 托管单块无人岛耕地
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 耕地索引</para>
        /// <para><c>param2</c>: 种子物品 ID</para>
        /// </remarks>
        MJIFarmEntrustSingle = 3279,

        /// <summary>
        /// 取消托管单块无人岛耕地
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 耕地索引</para>
        /// </remarks>
        MJIFarmDismiss = 3280,

        /// <summary>
        /// 收取单块无人岛耕地
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 耕地索引</para>
        /// <para><c>param2</c>: 收取后是否取消托管 (0 - 否, 1 - 是)</para>
        /// </remarks>
        MJIFarmCollectSingle = 3281,

        /// <summary>
        /// 收取全部无人岛耕地
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: *(int*)MJIManager.Instance()->GranariesState</para>
        /// </remarks>
        MJIFarmCollectAll = 3282,

        /// <summary>
        /// 请求无人岛工房需求数据
        /// </summary>
        MJIFavorStateRequest = 3292,

        /// <summary>
        /// 掷骰子
        /// </summary>
        /// <remarks>
        /// <para><c>param1</c>: 类型 (固定为 0)</para>
        /// <para><c>param2</c>: 最大值</para>
        /// </remarks>
        RollDice = 9000,

        /// <summary>
        /// 雇员
        /// </summary>
        Retainer = 9003,
    }

    public enum ExecuteCommandComplexFlag
    {
        /// <summary>
        /// 低空飞行下坐骑 (场地)
        /// </summary>
        /// <remarks>
        /// <para><c>location</c>: 目标位置</para>
        /// <para><c>param1</c>: 玩家旋转角度</para>
        /// <para><c>param2</c>: 未知, 一直为 1</para>
        /// <para><c>param2</c>: 未知, 一直为 0</para>
        /// </remarks>
        Dismount = 101,

        /// <summary>
        /// 未知
        /// </summary>
        Unk208 = 208,

        /// <summary>
        /// 潜水通过 (场地)
        /// </summary>
        /// <remarks>
        /// <para><c>location</c>: 目标位置</para>
        /// <para><c>param1</c>: 玩家旋转角度</para>
        /// </remarks>
        DiveThrough = 209,

        /// <summary>
        /// 未知
        /// </summary>
        Unk212 = 212,

        /// <summary>
        /// 放置目标标记
        /// </summary>
        /// <remarks>
        /// <para><c>target</c>: 目标 Entity ID</para>
        /// <para><c>param1</c>: 目标标记索引 (从 0 开始)</para>
        /// </remarks>
        PlaceMarker = 301,

        /// <summary>
        /// 使用情感动作
        /// </summary>
        /// <remarks>
        /// <para><c>target</c>: 目标 Entity ID</para>
        /// <para><c>param1</c>: Emote ID</para>
        /// <para><c>param3</c>: 是否发送情感动作消息 (1 - 不发送, 0 - 发送)</para>
        /// </remarks>
        Emote = 500,

        /// <summary>
        /// 使用情感动作 (场地)
        /// </summary>
        /// <remarks>
        /// <para><c>location</c>: 目标位置</para>
        /// <para><c>param1</c>: Emote ID</para>
        /// <para><c>param2</c>: 角度</para>
        /// <para><c>param4</c>: 玩家旋转角度</para>
        /// </remarks>
        EmoteLocation = 501,

        /// <summary>
        /// 打断当前情感动作 (场地)
        /// </summary>
        /// <para><c>location</c>: 目标位置</para>
        /// <para><c>param2</c>: Rotation Packet</para>
        EmoteInterruptLocation = 504,

        /// <summary>
        /// 未知 (场地)
        /// </summary>
        /// <para><c>location</c>: 位置</para>
        /// <para><c>param1</c>: Rotation Packet</para>
        /// <para><c>param2</c>: 未知</para>
        /// <para><c>param3</c>: 未知</para>
        Unk603 = 603,

        /// <summary>
        /// 潜水结束 (场地)
        /// </summary>
        /// <remarks>
        /// <para><c>location</c>: 目标位置</para>
        /// <para><c>param1</c>: Rotation Packet</para>
        /// <para><c>param2</c>: 玩家是否在坐骑上 (1 - 是, 0 - 否)</para>
        /// </remarks>
        DiveEnd = 607,

        /// <summary>
        /// 非法潜水 → 回到当前地图的出生点
        /// </summary>
        /// <para><c>location</c>: 当前位置 (似乎不影响)</para>
        /// <para><c>param1</c>: 未知 (似乎不影响)</para>
        DiveInvalid = 610,

        /// <summary>
        /// 召唤物技能
        /// </summary>
        /// <remarks>
        /// <para><c>target/location</c>: 0xE0000000 / 目的地位置 (仅移动)</para>
        /// <para><c>param1</c>: Pet Action ID</para>
        /// </remarks>
        PetAction = 1800,

        /// <summary>
        /// 冒险者分队技能
        /// </summary>
        /// <remarks>
        /// <para><c>target</c>: 目标 Entity ID</para>
        /// <para><c>param1</c>: BgcArmyAction ID</para>
        /// </remarks>
        BgcArmyAction = 1810,

        /// <summary>
        /// 未知 (场地)
        /// </summary>
        /// <para><c>location</c>: 位置</para>
        /// <para><c>param1</c>: Entity ID</para>
        Unk2000 = 2000,
    }

}
