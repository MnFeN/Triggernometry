using System;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using Triggernometry.Core;
using Triggernometry.Expressions.String.Evaluators;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.FFXIV;
using static Triggernometry.Expressions.String.Utils.ArgHelper;
using static Triggernometry.Expressions.String.Utils.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class EntityModule : ModuleBase
    {
        public IntPtr TargetSystemPtr;
        // public IntPtr HasStatusPtr;
        public IntPtr GetStatusIndexPtr;
        public IntPtr RemoveStatusPtr;

        /// <summary> 实体初始坐标相对于实体地址的偏移。</summary>
        public Func<int> DefaultPosOffset = () => 0x10;
        /// <summary> 实体 ID 相对于实体地址的偏移。</summary>
        public Func<int> IdOffset = () => Plugin.IsCN ? 0x78 : 0x78; // 6.0 / 7.3
        /// <summary> 实体坐标相对于实体地址的偏移。</summary>
        public Func<int> PosOffset = () => Plugin.IsCN ? 0xB0 : 0xB0; // 7.2 / 7.3
        /// <summary> 实体缩放倍率相对于实体地址的偏移。</summary>
        public Func<int> ScaleOffset = () => Plugin.IsCN ? 0xC4 : 0xC4; // 7.2 / 7.3
        /// <summary> 实体模型的相对偏移相对于实体地址的偏移。相对坐标偏移会影响实体绘制的模型显示的位置。</summary>
        public Func<int> ModelRelPosOffset = () => Plugin.IsCN ? 0xE0 : 0xE0; // 7.2 / 7.3
        /// <summary> 实体模型（DrawObject*）相对于实体地址的偏移。</summary>
        public Func<int> ModelOffset = () => Plugin.IsCN ? 0x100 : 0x100; // 7.2 / 7.3
        /// <summary> 实体 StatusLoopVfx ID 相对于实体地址的偏移。</summary>
        public Func<int> StatusLoopVfxOffset = () => Plugin.IsCN ? 0x1C8 : 0x1C8; // 7.2 / 7.3
        /// <summary> 实体透明度相对于实体地址的偏移。</summary>
        public Func<int> OpacityOffset = () => Plugin.IsCN ? 0x22D8 : 0x22D8; // 7.2 / 7.3 (尚未确认)
        /// <summary> 实体 ModelStatus (RenderFlags) 相对于实体地址的偏移。</summary>
        /// https://github.com/xivdev/Penumbra/blob/master/Penumbra/Interop/Structs/DrawState.cs
        public Func<int> ModelStatusOffset = () => Plugin.IsCN ? 0x118 : 0x118; // 7.2 / 7.3

        /// <summary> 硬目标地址相对于实体 TargetSystem 地址的偏移（SoftTarget 地址在此基础上 +0x8）。</summary>
        public Func<int> HardTargetOffset = () => 0x80; // 7.0

        /// <summary> 模型坐标相对于模型地址的偏移。</summary>
        public Func<int> ModelPosOffset = () => 0x50; // 6.0
        /// <summary> 模型缩放倍率相对于模型地址的偏移。</summary>
        public Func<int> ModelScaleOffset = () => 0x70; // 6.0

        /// <summary> EnableDraw 虚函数索引。</summary>
        public Func<int> EnableDrawVTableIdx = () => 12; // 7.0
        /// <summary> DisableDraw 虚函数索引。</summary> 
        public Func<int> DisableDrawVTableIdx = () => 13; // 7.0
        /// <summary> SetHighlightColor 虚函数索引。</summary>
        public Func<int> SetHighlightColorVTableIdx = () => 26; // 7.0
        /// <summary> GetStatusManager 虚函数索引。</summary>
        public Func<int> GetStatusManagerVTableIdx = () => 77; // 7.0

        public EntityModule()
        {
            ScanMethod = () =>
            {
                // FFXIVClientStructs/FFXIV/Client/Game/Control/TargetSystem.cs
                TargetSystemPtr = Scanner.TryScan(
                    "48 8D 0D * * * * E8 ? ? ? ? 48 3B C6 0F 95 C0", nameof(TargetSystemPtr)); // 7.0

                // FFXIVClientStructs/FFXIV/Client/Game/StatusManager.cs
                GetStatusIndexPtr = Scanner.TryScanMultiple(new string[] {
                    "E8 * * * * 85 C0 79 ? 4C 8B 15", // 7.3
                }, nameof(GetStatusIndexPtr));
                RemoveStatusPtr = Scanner.TryScanMultiple(new string[] {
                    "83 FA 3C 73 ?? 53 48 83 EC 30 48 8B D9", // 7.2
                }, nameof(RemoveStatusPtr));
            };
        }

        [CallbackMethod("InvokeOnMultipleEntities")]
        internal void CbInvokeOnMultipleEntities(string cmd)
        {
            CheckBeforeExecution(cmd);
            var cmds = cmd.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            // 首行是实体过滤器
            var filter = XivEntityFilterEvaluator.CreateFilter(cmds[0]);
            foreach (IntPtr address in Entity.GetEntities().Where(filter).Select(e => e.Address))
            {
                var strAddress = address.ToString();
                var hexId = Memory.Read<uint>(address + IdOffset()).ToString("X8");
                // 后续行是回调名称和参数，实体地址用 _address 替换
                foreach (var cbPair in cmds.Skip(1).Select(c => c.Split(new[] { ',' }, 2)))
                {
                    if (cbPair.Length == 1) throw new Exception($"批量调用回调时未提供回调参数：{cbPair[0]}");
                    var cbName = cbPair[0].Trim();
                    var cbRawParams = cbPair[1].Replace("_address", strAddress);
                    Task.Run(() =>
                    {
                        try
                        {
                            RealPlugin.Instance.InvokeNamedCallback(cbName, cbRawParams);
                        }
                        catch (Exception ex)
                        {
                            WarningLog($"对实体 0x{hexId} 调用回调 {cbName}: {cbRawParams} 时失败：\n{ex}");
                        }
                    });
                }
            }
        }

        [CallbackMethod("SetDefaultPos")]
        internal void CbSetDefaultPos(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, x, y, z) = cmd.ParseArgs<IntPtr, float, float, float>();
            Memory.ExecuteWithLock(() => SetDefaultPos(objectPtr, x, y, z));
        }

        [CallbackMethod("SetPos", tag: "Kairos")]
        internal void CbSetPos(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, x, y, z) = cmd.ParseArgs<IntPtr, float, float, float>();
            Memory.ExecuteWithLock(() => SetPos(objectPtr, x, y, z));
        }

        [CallbackMethod("SetModelRelPos")]
        internal void CbSetModelRelPos(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, dx, dy, dz) = cmd.ParseArgs<IntPtr, float, float, float>();
            Memory.ExecuteWithLock(() => SetModelRelPos(objectPtr, dx, dy, dz));
        }

        [CallbackMethod("Teleport", tag: "Kairos")]
        internal void CbTeleport(string cmd)
        {
            CheckBeforeExecution(cmd);
            var objectPtr = Triggernometry.FFXIV.Entity.GetMyself().Address;
            var (x, y, z) = cmd.ParseArgs<float, float, float>();
            Memory.ExecuteWithLock(() => SetPos(objectPtr, x, y, z));
        }

        [CallbackMethod("SetDefaultHeading")]
        internal void CbSetDefaultHeading(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, heading) = cmd.ParseArgs<IntPtr, float>();
            Memory.ExecuteWithLock(() => SetDefaultHeading(objectPtr, heading));
        }

        [CallbackMethod("SetHeading")]
        internal void CbSetHeading(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, heading) = cmd.ParseArgs<IntPtr, float>();
            Memory.ExecuteWithLock(() => SetHeading(objectPtr, heading));
        }

        [CallbackMethod("Target")]
        internal void CbTarget(string cmd)
        {
            CheckBeforeExecution(cmd);
            (uint id, bool hard, bool soft) = cmd.ParseArgs<HexOrDecId, bool, bool>((1, true), (2, true));
            IntPtr objectPtr = default;

            if (id != HexOrDecId.Default)
            {
                var entity = Triggernometry.FFXIV.Entity.GetEntityByID(id);
                if (entity.Exist)
                    objectPtr = entity.Address;
                else
                    WarningLog($"[鲶鱼精邮差扩展] [Target] 未找到实体：0x{id:X8}");
            }
            Target(objectPtr, hard, soft);
        }

        [CallbackMethod("SetModelStatus")]
        internal void CbSetModelStatus(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, modelStatus) = cmd.ParseArgs<IntPtr, int>();
            Memory.ExecuteWithLock(() => SetModelStatus(objectPtr, modelStatus));
        }

        // 新方法 直接修改实体参数并重绘
        [CallbackMethod("SetObjectScale")]
        internal void CbSetObjectScale(string cmd)
        {
            CheckBeforeExecution(cmd);
            if (GetConfig<bool>("ObjectScale") == false) return; // ignored
            var (objectPtr, scale) = cmd.ParseArgs<IntPtr, float>();
            Memory.ExecuteWithLock(() => SetObjectScale(objectPtr, scale));
        }

        // 旧方法 临时修改已经绘制生成的实体模型
        [CallbackMethod("ObjectScaling")]
        internal void CbObjectScaling(string cmd)
        {
            CheckBeforeExecution(cmd);
            if (GetConfig<bool>("ObjectScale") == false) return; // ignored
            var (objectPtr, scaleX, scaleY, scaleZ) = cmd.ParseArgs<IntPtr, float, float?, float?>((2, null), (3, null));
            Memory.ExecuteWithLock(() => SetObjectScaleTemp(objectPtr, scaleX, scaleY ?? scaleX, scaleZ ?? scaleX));
        }

        [CallbackMethod("SetOpacity")]
        internal void CbSetOpacity(string cmd)
        {
            CheckBeforeExecution(cmd);
            if (GetConfig<bool>("Opacity") == false) return; // ignored
            var (objectPtr, opacity) = cmd.ParseArgs<IntPtr, float>();
            Memory.ExecuteWithLock(() => SetOpacity(objectPtr, opacity));
        }

        [CallbackMethod("SetStatusLoopVfx")]
        internal void CbSetStatusLoopVfx(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, vfxId) = cmd.ParseArgs<IntPtr, ushort>();
            Memory.ExecuteWithLock(() => {
                SetStatusLoopVfx(objectPtr, vfxId);
                ReDraw(objectPtr);
            });
        }

        [CallbackMethod("Redraw")]
        internal void CbRedraw(string cmd)
        {
            CheckBeforeExecution(cmd);
            var objectPtr = cmd.ParseData<IntPtr>();
            Memory.ExecuteWithLock(() => ReDraw(objectPtr));
        }

        [CallbackMethod("SetHighlightColor")]
        internal void CbSetHighlightColor(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, color) = cmd.ParseArgs<IntPtr, byte>();
            Memory.ExecuteWithLock(() => SetHighlightColor(objectPtr, color));
        }

        [CallbackMethod("RemoveStatus")]
        internal void CbRemoveStatus(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, statusId) = cmd.ParseArgs<IntPtr, ushort>();
            Memory.ExecuteWithLock(() => RemoveStatus(objectPtr, statusId));
        }

        public void SetPos(IntPtr objectAddress, float x, float y, float z)
        {
            Vector3 pos = new Vector3(x, z, y); // 注意 Y Z 轴交换
            IntPtr modelAddress = Memory.Read<IntPtr>(objectAddress + ModelOffset());
            Memory.Write(objectAddress + PosOffset(), pos);
            if (modelAddress != IntPtr.Zero)
                Memory.Write(modelAddress + ModelPosOffset(), pos);
        }

        public void SetDefaultPos(IntPtr objectAddress, float x, float y, float z)
        {
            Vector3 pos = new Vector3(x, z, y); // 注意 Y Z 轴交换
            Memory.Write(objectAddress + DefaultPosOffset(), pos);
        }

        public void SetModelRelPos(IntPtr objectAddress, float dx, float dy, float dz)
        {
            Vector3 relPos = new Vector3(dx, dz, dy); // 注意 Y Z 轴交换
            Memory.Write(objectAddress + ModelRelPosOffset(), relPos);
        }

        public void SetHeading(IntPtr objectAddress, float h)
        {
            IntPtr modelAddress = Memory.Read<IntPtr>(objectAddress + ModelOffset());
            Memory.Write(objectAddress + PosOffset() + 0x10, h);
            // 四元数
            Memory.Write(modelAddress + ModelPosOffset() + 0x14, (float)Math.Sin(h / 2));
            Memory.Write(modelAddress + ModelPosOffset() + 0x1C, (float)Math.Cos(h / 2));
        }

        public void SetDefaultHeading(IntPtr objectAddress, float h)
        {
            Memory.Write(objectAddress + DefaultPosOffset() + 0x10, h);
        }

        public void Target(IntPtr address, bool hard = true, bool soft = true)
        {   // FFXIVClientStructs/FFXIV/Client/Game/Control/TargetSystem.cs
            CheckIfAnyZeroPtr(TargetSystemPtr);
            if (hard) Memory.Write(TargetSystemPtr + HardTargetOffset(), address);
            if (soft) Memory.Write(TargetSystemPtr + HardTargetOffset() + 8, address);
        }

        /// <summary> 见 status 参数描述 </summary>
        /// <param name="status">
        /// 0: "visible" 正常状态 <br />
        /// 512/1024: 玩家切换地图时会经历的两种状态，类似 16384 <br />
        /// 2048: 不重绘: 有模型无名牌、列表可选；重绘：恢复 0 <br />
        /// 4096: 不重绘: 有模型无名牌、列表可选；重绘：不变，刷新模型 <br />
        /// 8192: 不重绘：有模型无名牌、不可选；重绘/移动/攻击：恢复 0 <br />
        /// 16384: 不重绘：有模型无名牌、不可选；重绘：不变，刷新模型 <br />
        /// </param>
        public void SetModelStatus(IntPtr objectAddress, int status)
        {
            Memory.Write(objectAddress + ModelStatusOffset(), status);
        }

        public void SetObjectScaleTemp(IntPtr objectAddress, float scaleX, float scaleY, float scaleZ)
        {
            IntPtr drawObjectAddress = Memory.Read<IntPtr>(objectAddress + ModelOffset());
            Memory.Write<Vector3>(drawObjectAddress + ModelScaleOffset(), new Vector3(scaleX, scaleZ, scaleY));
        }

        public void SetObjectScale(IntPtr objectAddress, float scale)
        {
            Memory.Write<float>(objectAddress + ScaleOffset(), scale);
            ReDraw(objectAddress);
        }

        // FFXIVClientStructs/FFXIV/Client/Game/Character/Character.cs    public float Alpha;
        public void SetOpacity(IntPtr objectAddress, float opacity)
        {
            Memory.Write<float>(objectAddress + OpacityOffset(), opacity);
        }

        public void SetStatusLoopVfx(IntPtr objectAddress, ushort id)
        {
            Memory.Write<ushort>(objectAddress + StatusLoopVfxOffset(), id);
            ReDraw(objectAddress);
        }

        public void EnableDraw(IntPtr address)
            => CallEntityVirtualFunction(address, EnableDrawVTableIdx());

        public void DisableDraw(IntPtr address)
            => CallEntityVirtualFunction(address, DisableDrawVTableIdx());

        public void ReDraw(IntPtr address)
        {
            DisableDraw(address);
            EnableDraw(address);
        }

        public void SetHighlightColor(IntPtr address, byte color)
            => CallEntityVirtualFunction(address, SetHighlightColorVTableIdx(), color);

        // FFXIVClientStructs/FFXIV/Client/Game/Character/Character.cs
        // The GameObject must be a Character!
        public IntPtr GetStatusManagerPtr(IntPtr address)
            => CallEntityVirtualFunction<IntPtr>(address, GetStatusManagerVTableIdx());

        public T CallEntityVirtualFunction<T>(IntPtr entityAddress, int vFuncIndex, params object[] args) where T : struct
        {
            if ((long)entityAddress < 0xFFFF)
                throw new Exception($"[鲶鱼精邮差扩展] 传入实体虚函数 {vFuncIndex} 的地址 {entityAddress} 无效。");
            return Memory.CallVirtualFunction<T>(entityAddress, vFuncIndex, args);
        }

        public void CallEntityVirtualFunction(IntPtr entityAddress, int vFuncIndex, params object[] args)
            => CallEntityVirtualFunction<IntPtr>(entityAddress, vFuncIndex, args);

        public void RemoveStatus(IntPtr address, ushort statusId)
        {
            CheckIfAnyZeroPtr(GetStatusIndexPtr, RemoveStatusPtr);
            IntPtr statusManagerPtr = GetStatusManagerPtr(address);
            if (statusManagerPtr == IntPtr.Zero)
            {
                throw new Exception($"StatusManagerPtr is null for entity at {(long)address:X}");
            }
            int statusIndex = Memory.CallInjected64<int>(GetStatusIndexPtr, statusManagerPtr, statusId, 0xE0000000);
            if (statusIndex < 0)
            {
                return;
                // throw new Exception($"Status 0x{statusId:X} does not exist for entity at {(long)address:X}");
            }
            Memory.CallInjected64(RemoveStatusPtr, statusManagerPtr, statusIndex, 0);
        }
    }
}
