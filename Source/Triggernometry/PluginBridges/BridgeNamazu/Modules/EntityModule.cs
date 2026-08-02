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
        public IntPtr EObjAnimationPtr;
        public IntPtr PlayActionTimelinePtr;
        public int TimelineContainerOffset;

        /*Plugin.IsTC ? TC : Global*/
        /// <summary> 实体初始坐标相对于实体地址的偏移。</summary>
        public Func<int> DefaultPosOffset = () => Plugin.IsTC ? 0x10 : 0x10;
        /// <summary> 实体 ID 相对于实体地址的偏移。</summary>
        public Func<int> IdOffset = () => Plugin.IsTC ? 0x78 : 0x78; // 6.0 / 7.3
        /// <summary> 实体坐标相对于实体地址的偏移。</summary>
        public Func<int> PosOffset = () => Plugin.IsTC ? 0xB0 : 0xB0; // 7.2 / 7.3
        /// <summary> 实体缩放倍率相对于实体地址的偏移。</summary>
        public Func<int> ScaleOffset = () => Plugin.IsTC ? 0xC4 : 0xC4; // 7.2 / 7.3
        /// <summary> 实体模型的相对偏移相对于实体地址的偏移。相对坐标偏移会影响实体绘制的模型显示的位置。</summary>
        public Func<int> ModelRelPosOffset = () => Plugin.IsTC ? 0xE0 : 0xE0; // 7.2 / 7.3
        /// <summary> 实体模型（DrawObject*）相对于实体地址的偏移。</summary>
        public Func<int> ModelOffset = () => Plugin.IsTC ? 0x100 : 0x100; // 7.2 / 7.3
        /// <summary> 实体 StatusLoopVfx ID 相对于实体地址的偏移。</summary>
        public Func<int> StatusLoopVfxOffset = () => Plugin.IsTC ? 0x1C8 : 0x1C8; // 7.2 / 7.3
        /// <summary> 实体透明度相对于实体地址的偏移。</summary>
        public Func<int> OpacityOffset = () => Plugin.IsTC ? 0x22D8 : 0x22E8; // 7.4; 7.3 0x22D8 
        /// <summary> 实体 ModelStatus (RenderFlags) 相对于实体地址的偏移。</summary>
        /// https://github.com/xivdev/Penumbra/blob/master/Penumbra/Interop/Structs/DrawState.cs
        public Func<int> ModelStatusOffset = () => Plugin.IsTC ? 0x118 : 0x118; // 7.2 / 7.3

        /// <summary> 硬目标地址相对于实体 TargetSystem 地址的偏移（SoftTarget 地址在此基础上 +0x8）。</summary>
        public Func<int> HardTargetOffset = () => Plugin.IsTC ? 0x80 : 0x80; // 7.0

        /// <summary> 模型坐标相对于模型地址的偏移。</summary>
        public Func<int> ModelPosOffset = () => Plugin.IsTC ? 0x50 : 0x50; // 6.0
        /// <summary> 模型缩放倍率相对于模型地址的偏移。</summary>
        public Func<int> ModelScaleOffset = () => Plugin.IsTC ? 0x70 : 0x70; // 6.0

        /// <summary> EnableDraw 虚函数索引。</summary>
        public Func<int> EnableDrawVTableIdx = () => 12; // 7.0
        /// <summary> DisableDraw 虚函数索引。</summary> 
        public Func<int> DisableDrawVTableIdx = () => 13; // 7.0
        /// <summary> SetHighlightColor 虚函数索引。</summary>
        public Func<int> SetHighlightColorVTableIdx = () => 26; // 7.0
        /// <summary> GetStatusManager 虚函数索引。</summary>
        public Func<int> GetStatusManagerVTableIdx = () => Plugin.IsTC ? 77 : 78; // 7.0

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

                // 收包函数 case ActorControl -> case EObjAnimation (0x1A2) 调用的函数
                EObjAnimationPtr = Scanner.TryScanMultiple(new string[] {
                    "45 33 C9 0F B7 54 24 ?? 48 8B CB E8 * * * *", // 7.4
                }, nameof(EObjAnimationPtr));

                /* 收包函数 case ActorControl -> case PlayActionTimeline (0x197) 调用的函数
                 
                case 0x197u:
                case 0x19Cu:
                    if (v15)
                        sub_1408929F0(v15 + 2608, (unsigned __int16)v971, 0, 0);
                    return;
                
                4D 85 F6;                test r14, r14
                0F 84 A7 41 00 00;       jz + 0x41A7
                0F B7 54 24 70;          movzx edx, word ptr[rsp + 70h]
                49 8D 8E 30 0A 00 00;    lea rcx, [r14 + 0A30h]
                45 33 C9;                xor r9d, r9d
                45 33 C0;                xor r8d, r8d
                E8 F9 E4 CF FF;          call<rel32>
                E9 8B 41 00 00;          jmp + 0x418B
                */
                var playActionTimelineCasePtr = Scanner.TryScanMultiple(new string[] {
                    "0F B7 54 24 ? 49 8D 8E ? ? ? ? 45 33 C9 45 33 C0", // 7.4
                }, nameof(PlayActionTimelinePtr));
                // 49 8D 8E ? ? ? ? 的偏移
                TimelineContainerOffset = Memory.Read<int>(playActionTimelineCasePtr + 8);
                // E8 * * * * 的相对寻址
                PlayActionTimelinePtr = playActionTimelineCasePtr + 23 + Memory.Read<int>(playActionTimelineCasePtr + 19);
                /* FFXIVClientStructs/FFXIV/Client/Game/Character/TimelineContainer.cs
                PlayActionTimelinePtr = Scanner.TryScanMultiple(new string[] {
                    "E8 * * * * 48 8D 8F ? ? ? ? B2 12", // 7.4
                }, nameof(PlayActionTimelinePtr));
                */
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
            SetDefaultPos(objectPtr, x, y, z);
        }

        [CallbackMethod("SetPos", tag: "Kairos")]
        internal void CbSetPos(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, x, y, z) = cmd.ParseArgs<IntPtr, float, float, float>();
            SetPos(objectPtr, x, y, z);
        }

        [CallbackMethod("SetModelRelPos")]
        internal void CbSetModelRelPos(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, dx, dy, dz) = cmd.ParseArgs<IntPtr, float, float, float>();
            SetModelRelPos(objectPtr, dx, dy, dz);
        }

        [CallbackMethod("Teleport", tag: "Kairos")]
        internal void CbTeleport(string cmd)
        {
            CheckBeforeExecution(cmd);
            var objectPtr = Triggernometry.FFXIV.Entity.GetMyself().Address;
            var (x, y, z) = cmd.ParseArgs<float, float, float>();
            SetPos(objectPtr, x, y, z);
        }

        [CallbackMethod("SetDefaultHeading")]
        internal void CbSetDefaultHeading(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, heading) = cmd.ParseArgs<IntPtr, float>();
            SetDefaultHeading(objectPtr, heading);
        }

        [CallbackMethod("SetHeading")]
        internal void CbSetHeading(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, heading) = cmd.ParseArgs<IntPtr, float>();
            SetHeading(objectPtr, heading);
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
            SetModelStatus(objectPtr, modelStatus);
        }

        // 新方法 直接修改实体参数并重绘
        [CallbackMethod("SetObjectScale")]
        internal void CbSetObjectScale(string cmd)
        {
            CheckBeforeExecution(cmd);
            if (GetConfig<bool>("ObjectScale") == false) return; // ignored
            var (objectPtr, scale) = cmd.ParseArgs<IntPtr, float>();
            SetObjectScale(objectPtr, scale);
        }

        // 旧方法 临时修改已经绘制生成的实体模型
        [CallbackMethod("ObjectScaling")]
        internal void CbObjectScaling(string cmd)
        {
            CheckBeforeExecution(cmd);
            if (GetConfig<bool>("ObjectScale") == false) return; // ignored
            var (objectPtr, scaleX, scaleY, scaleZ) = cmd.ParseArgs<IntPtr, float, float?, float?>((2, null), (3, null));
            SetObjectScaleTemp(objectPtr, scaleX, scaleY ?? scaleX, scaleZ ?? scaleX);
        }

        [CallbackMethod("SetOpacity")]
        internal void CbSetOpacity(string cmd)
        {
            CheckBeforeExecution(cmd);
            if (GetConfig<bool>("Opacity") == false) return; // ignored
            var (objectPtr, opacity) = cmd.ParseArgs<IntPtr, float>();
            SetOpacity(objectPtr, opacity);
        }

        [CallbackMethod("SetStatusLoopVfx")]
        internal void CbSetStatusLoopVfx(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, vfxId) = cmd.ParseArgs<IntPtr, ushort>();
            SetStatusLoopVfx(objectPtr, vfxId);
            ReDraw(objectPtr);
        }

        [CallbackMethod("Redraw")]
        internal void CbRedraw(string cmd)
        {
            CheckBeforeExecution(cmd);
            var objectPtr = cmd.ParseData<IntPtr>();
            ReDraw(objectPtr);
        }

        [CallbackMethod("SetHighlightColor")]
        internal void CbSetHighlightColor(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, color) = cmd.ParseArgs<IntPtr, byte>();
            SetHighlightColor(objectPtr, color);
        }

        [CallbackMethod("RemoveStatus")]
        internal void CbRemoveStatus(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, statusId) = cmd.ParseArgs<IntPtr, ushort>();
            RemoveStatus(objectPtr, statusId);
        }

        [CallbackMethod("EObjAnimation")]
        internal void CbEObjAnimation(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, animationId, slotMask, context) = cmd.ParseArgs<IntPtr, ushort, ushort, long>((3, 0L));
            EObjAnimation(objectPtr, animationId, slotMask, context);
        }

        [CallbackMethod("PlayActionTimeline")]
        internal void CbPlayActionTimeline(string cmd)
        {
            CheckBeforeExecution(cmd);
            var (objectPtr, timelineId, a3, a4) = cmd.ParseArgs<IntPtr, ushort, long, bool>((2, 0L), (3, false));
            PlayActionTimeline(objectPtr, timelineId, a3, a4);
        }

        public void SetPos(IntPtr objectPtr, float x, float y, float z)
        {
            if (!CheckIfValidEntityPtr(objectPtr, nameof(SetPos))) return;
            Vector3 pos = new Vector3(x, z, y); // 注意 Y Z 轴交换
            IntPtr modelAddress = Memory.Read<IntPtr>(objectPtr + ModelOffset());
            Memory.Write(objectPtr + PosOffset(), pos);
            if (modelAddress != IntPtr.Zero)
                Memory.Write(modelAddress + ModelPosOffset(), pos);
        }

        public void SetDefaultPos(IntPtr objectPtr, float x, float y, float z)
        {
            if (!CheckIfValidEntityPtr(objectPtr, nameof(SetDefaultPos))) return;
            Vector3 pos = new Vector3(x, z, y); // 注意 Y Z 轴交换
            Memory.Write(objectPtr + DefaultPosOffset(), pos);
        }

        public void SetModelRelPos(IntPtr objectPtr, float dx, float dy, float dz)
        {
            if (!CheckIfValidEntityPtr(objectPtr, nameof(SetModelRelPos))) return;
            Vector3 relPos = new Vector3(dx, dz, dy); // 注意 Y Z 轴交换
            Memory.Write(objectPtr + ModelRelPosOffset(), relPos);
        }

        public void SetHeading(IntPtr objectPtr, float h)
        {
            if (!CheckIfValidEntityPtr(objectPtr, nameof(SetHeading))) return;
            IntPtr modelAddress = Memory.Read<IntPtr>(objectPtr + ModelOffset());
            Memory.Write(objectPtr + PosOffset() + 0x10, h);
            if (modelAddress != IntPtr.Zero)
            {
                // 四元数
                Memory.Write(modelAddress + ModelPosOffset() + 0x14, (float)Math.Sin(h / 2));
                Memory.Write(modelAddress + ModelPosOffset() + 0x1C, (float)Math.Cos(h / 2));
            }
        }

        public void SetDefaultHeading(IntPtr objectPtr, float h)
        {
            if (!CheckIfValidEntityPtr(objectPtr, nameof(SetDefaultHeading))) return;
            Memory.Write(objectPtr + DefaultPosOffset() + 0x10, h);
        }

        public void Target(IntPtr objectPtr, bool hard = true, bool soft = true)
        {   // FFXIVClientStructs/FFXIV/Client/Game/Control/TargetSystem.cs
            CheckIfAnyZeroPtr(TargetSystemPtr);
            if (!CheckIfValidEntityPtr(objectPtr, nameof(Target))) return;

            if (hard) Memory.Write(TargetSystemPtr + HardTargetOffset(), objectPtr);
            if (soft) Memory.Write(TargetSystemPtr + HardTargetOffset() + 8, objectPtr);
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
        public void SetModelStatus(IntPtr objectPtr, int status)
        {
            if (!CheckIfValidEntityPtr(objectPtr, nameof(SetModelStatus))) return;
            Memory.Write(objectPtr + ModelStatusOffset(), status);
        }

        public void SetObjectScaleTemp(IntPtr objectPtr, float scaleX, float scaleY, float scaleZ)
        {
            if (!CheckIfValidEntityPtr(objectPtr, nameof(SetObjectScaleTemp))) return;
            IntPtr drawObjectPtr = Memory.Read<IntPtr>(objectPtr + ModelOffset());
            CheckIfAnyZeroPtr(drawObjectPtr);
            Memory.Write<Vector3>(drawObjectPtr + ModelScaleOffset(), new Vector3(scaleX, scaleZ, scaleY));
        }

        public void SetObjectScale(IntPtr objectPtr, float scale)
        {
            if (!CheckIfValidEntityPtr(objectPtr, nameof(SetObjectScale))) return;
            var scalePtr = objectPtr + ScaleOffset();
            var old = Memory.Read<float>(scalePtr);
            Memory.Write<float>(scalePtr, scale);
            ReDraw(objectPtr);
            Custom2Log($"SetObjectScale: {old} → {scale} @ {(long)objectPtr:X}");
        }

        // FFXIVClientStructs/FFXIV/Client/Game/Character/Character.cs    public float Alpha;
        public void SetOpacity(IntPtr objectPtr, float opacity)
        {
            if (!CheckIfValidEntityPtr(objectPtr, nameof(SetOpacity))) return;
            Memory.Write<float>(objectPtr + OpacityOffset(), opacity);
        }

        public void SetStatusLoopVfx(IntPtr objectPtr, ushort id)
        {
            if (!CheckIfValidEntityPtr(objectPtr, nameof(SetStatusLoopVfx))) return;
            Memory.Write<ushort>(objectPtr + StatusLoopVfxOffset(), id);
            ReDraw(objectPtr);
        }

        public void EnableDraw(IntPtr objectPtr)
            => CallEntityVirtualFunction(objectPtr, EnableDrawVTableIdx());

        public void DisableDraw(IntPtr objectPtr)
            => CallEntityVirtualFunction(objectPtr, DisableDrawVTableIdx());

        public void ReDraw(IntPtr objectPtr)
        {
            if (!CheckIfValidEntityPtr(objectPtr, nameof(ReDraw))) return;
            DisableDraw(objectPtr);
            EnableDraw(objectPtr);
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
            return Plugin.CallVirtualFunction<T>(entityAddress, vFuncIndex, args);
        }

        public void CallEntityVirtualFunction(IntPtr entityAddress, int vFuncIndex, params object[] args)
            => CallEntityVirtualFunction<IntPtr>(entityAddress, vFuncIndex, args);

        public void RemoveStatus(IntPtr objectPtr, ushort statusId)
        {
            CheckIfAnyZeroPtr(objectPtr, GetStatusIndexPtr, RemoveStatusPtr);
            if (!CheckIfValidEntityPtr(objectPtr, nameof(RemoveStatus))) return;

            IntPtr statusManagerPtr = GetStatusManagerPtr(objectPtr);
            if (statusManagerPtr == IntPtr.Zero)
            {
                throw new Exception($"StatusManagerPtr is null for entity at {(long)objectPtr:X}");
            }
            int statusIndex = Plugin.Call<int>(GetStatusIndexPtr, statusManagerPtr, statusId, 0xE0000000);
            if (statusIndex < 0)
            {
                return;
            }
            Plugin.Call(RemoveStatusPtr, statusManagerPtr, statusIndex, 0);
        }

        public void EObjAnimation(IntPtr objectPtr, ushort animationId, ushort slotMask, long context = 0)
        {
            CheckIfAnyZeroPtr(EObjAnimationPtr);
            if (!CheckIfValidEntityPtr(objectPtr, nameof(EObjAnimation))) return;

            var obj = Entity.GetEntities(e => e.Address == objectPtr).FirstOrDefault();
            if (obj == null)
            {
                WarningLog("[EObjAnimation] 未找到对应的实体");
                return;
            }
            if (obj.Type != EntityType.EventObj)
            {
                throw new Exception($"[EObjAnimation] 指定实体 \"{obj.Name}\" ({obj.ID:X8}) @ {(long)objectPtr:X} 类型 {obj.Type} 不是 EventObject");
            }
            _ = Plugin.Call<IntPtr>(EObjAnimationPtr, objectPtr, animationId, slotMask, context);
        }

        public bool PlayActionTimeline(IntPtr objectPtr, ushort timelineId, long a3 = 0, bool a4 = false)
        {
            // a4: should skip mount/submodel timeline
            // 原函数是 实体->TimelineContainer 的方法，这里封装改用了实体本身的地址
            CheckIfAnyZeroPtr(PlayActionTimelinePtr);
            if (!CheckIfValidEntityPtr(objectPtr, nameof(PlayActionTimeline))) return false;

            var timelineContainerPtr = objectPtr + TimelineContainerOffset;
            var byte_a4 = a4 ? (byte)1 : (byte)0;
            return Plugin.Call<bool>(PlayActionTimelinePtr, timelineContainerPtr, timelineId, a3, byte_a4);
        }

        public bool CheckIfValidEntityPtr(IntPtr objectPtr, string funcName)
        {
            var isValid = objectPtr != IntPtr.Zero;
            if (!isValid)
            {
                WarningLog($"[鲶鱼精邮差扩展] 调用 {funcName} 时实体地址为空。");
            }
            return isValid;
        }
    }
}
