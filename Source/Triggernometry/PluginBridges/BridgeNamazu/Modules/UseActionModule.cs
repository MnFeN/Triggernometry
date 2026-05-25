using System;
using System.Numerics;
using Triggernometry.Expressions.String.Utils;
using static Triggernometry.Expressions.String.Utils.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class UseActionModule : ModuleBase
    {
        public IntPtr UseActionPtr;
        public IntPtr UseActionLocationPtr;
        public IntPtr ActionManagerPtr;
        public IntPtr MouseToWorldPtr;

        public UseActionModule()
        {
            ScanMethod = () =>
            {
                // https://github.com/aers/FFXIVClientStructs/blob/main/FFXIVClientStructs/FFXIV/Client/Game/ActionManager.cs
                UseActionPtr = Scanner.TryScan(
                    "E8 * * * * B0 01 EB B6", nameof(UseActionPtr));
                
                UseActionLocationPtr = Scanner.TryScanMultiple(new string[] {
                    "E8 * * * * 48 8B BC 24 ? ? ? ? 44 0F B6 F8 B0", // 7.4
                    "E8 * * * * 40 3A C7 0F 85", // 7.3
                    "E8 * * * * 41 3A C5 0F 85", // 7.2 TC
                }, nameof(UseActionLocationPtr));

                ActionManagerPtr = Scanner.TryScan(
                    "48 8D 0D * * * * F3 0F 10 13", nameof(ActionManagerPtr));
                // https://github.com/zfxsquare/PostNamazuFra/blob/main/PostNamazu/Actions/Tp.cs
                MouseToWorldPtr = Scanner.TryScan(
                    "4C 8B DC 49 89 5B ?? 49 89 6B ?? 49 89 73 ?? 57 48 81 EC ?? ?? ?? ?? 33 C0", nameof(MouseToWorldPtr));
            };
        }

        [CallbackMethod("UseAction")]
        internal void CbUseAction(string command)
        {
            CheckBeforeExecution(command);
            var (actionType, actionId, targetId, mode)
                = command.ParseArgs<ActionType, uint, uint, UseActionMode>(
                    (2, 0xE0000000),
                    (3, UseActionMode.None)
                );
            UseAction(actionType, actionId, targetId, mode);
        }

        public bool UseAction(ActionType actionType, uint actionId, uint targetId, UseActionMode mode = UseActionMode.None)
        {
            CheckIfAnyZeroPtr(UseActionPtr, ActionManagerPtr);
            uint extraParam = (uint)(actionType == ActionType.Item ? 0xFFFF : 0);
            uint comboRouteID = 0;
            var result = Plugin.Call<bool>(UseActionPtr, ActionManagerPtr, (int)actionType, actionId, targetId, extraParam, (int)mode, comboRouteID, 0);
            if (result)
            {
                NamazuLog($"[UseAction] {actionType} ({(int)actionType}), action = {actionId} (0x{actionId:X}), target = {targetId:X}, mode = {mode} ({(int)mode})");
            }
            return result;
        }

        [CallbackMethod("UseActionLocation")]
        internal void CbUseActionLocation(string command)
        {
            CheckBeforeExecution(command);
            var (actionType, actionId, x, y, z, extraParam)
                = command.ParseArgs<ActionType, uint, float, float, float, uint>(
                    (1, 0xE0000000),
                    (2, 0), (3, 0), (4, 0),
                    (5, 0)
                );
            UseActionLocation(actionType, actionId, x, y, z, extraParam);
        }

        public bool UseActionLocation(ActionType actionType, uint actionId, float x, float y, float z, uint extraParam = 0)
        {
            CheckIfAnyZeroPtr(UseActionLocationPtr, ActionManagerPtr);
            uint targetId = HexOrDecId.Default;
            IntPtr posPtr = default;
            bool result = default;
            try
            {
                posPtr = Memory.AllocateMemory(0x10);
                Memory.Write(posPtr, new Vector3(x, z, y));
                result = Plugin.Call<bool>(UseActionLocationPtr, ActionManagerPtr, 
                    (byte)actionType, actionId, targetId, posPtr, extraParam, /*unknown bool*/(byte)0);
            }
            finally
            {
                if (posPtr != IntPtr.Zero)
                    Memory.FreeMemory(posPtr);
            }
            if (result)
            {
                NamazuLog($"[UseActionLocation]: {actionType} ({(byte)actionType}); action = {actionId} (0x{actionId:X}) @ ({x:0.##}, {y:0.##}, {z:0.##})");
            }
            return result;
        }

        // 似乎是借用尝试放置标点时调用的函数获取鼠标位置
        [ScriptingMethod("MouseToWorld")]
        public Vector3? MouseToWorld()
        {
            CheckIfAnyZeroPtr(MouseToWorldPtr, ActionManagerPtr);
            uint actionId = 0xFFFFFFFF;
            ActionType actionType = ActionType.Waymark;
            IntPtr resultPtr = IntPtr.Zero;
            try
            {
                resultPtr = Memory.AllocateMemory(0x20);
                Plugin.Call<long>(MouseToWorldPtr, ActionManagerPtr, actionId, (byte)actionType, resultPtr);
                // bool canUseAction = Memory.Read<bool>(resultPtr + 0x1); 似乎代表这个位置是否视线未遮挡
                if (Memory.Read<bool>(resultPtr + 0x0)) // 代表确实指向了一个有效位置（一定距离内已加载的有碰撞的模型）
                {
                    var pos = Memory.Read<Vector3>(resultPtr + 0x10);
                    return new Vector3(pos.X, pos.Z, pos.Y);
                }
                else return null;
            }
            finally
            {
                if (resultPtr != IntPtr.Zero)
                    Memory.FreeMemory(resultPtr);
            }
        }

        [ScriptingMethod("IsMouseInSight")]
        public bool IsMouseInSight()
        {
            CheckIfAnyZeroPtr(MouseToWorldPtr, ActionManagerPtr);
            uint actionId = 0xFFFFFFFF;
            ActionType actionType = ActionType.Waymark;
            IntPtr resultPtr = IntPtr.Zero;
            try
            {
                resultPtr = Memory.AllocateMemory(0x20);
                Plugin.Call<long>(MouseToWorldPtr, ActionManagerPtr, actionId, (byte)actionType, resultPtr);
                return Memory.Read<bool>(resultPtr + 0x1);
            }
            finally
            {
                if (resultPtr != IntPtr.Zero)
                    Memory.FreeMemory(resultPtr);
            }
        }
    }

    public enum ActionType : byte
    {
        None = 0,
        Normal = 1, Action = 1, // Spell, Weaponskill, Ability
        Item = 2,
        KeyItem = 3,
        Ability = 4, // Not in UseActionHelper (??)
        General = 5, GeneralAction = 5,
        Buddy = 6, BuddyAction = 6,
        Main = 7, MainCommand = 7,
        Companion = 8,
        Craft = 9, CraftAction = 9,
        Unk_10 = 10, // Fishing per Sapphire? Something to do with items.
        Pet = 11, PetAction = 11,
        Unk_12 = 12, // Not in UseActionHelper. Sapphire says CompanyAction, but not actually triggered.
        Mount = 13,
        PvP = 14, PvPAction = 14,
        Waymark = 15, FieldMarker = 15,
        ChocoboRaceAbility = 16,
        ChocoboRaceItem = 17,
        Unk_18 = 18, // Not in UseActionHelper (?)
        BgcArmyAction = 0x19,
        Ornament = 0x20,
    }

    public enum UseActionMode
    {
        None = 0, // usual action execution, e.g. a hotbar button press
        Queue = 1, // previously queued action is now ready and is being executed (=> will ignore queue)
        Macro = 2, // action execution originating from a macro (=> won't be queued)
        Combo = 3, // action execution is from a single-button combo
    }
}
