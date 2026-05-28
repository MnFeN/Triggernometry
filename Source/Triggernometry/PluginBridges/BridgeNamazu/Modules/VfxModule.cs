using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Triggernometry.Core;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.PluginBridges.BridgeNamazu.Vfx;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class VfxModule : ModuleBase
    {
        public IntPtr ActorVfxCreatePtr;
        public IntPtr ActorVfxRemovePtr;

        public IntPtr StaticVfxCreatePtr;
        public IntPtr StaticVfxRunPtr;
        public IntPtr StaticVfxFadeOutPtr;

        public VfxModule()
        {
            ScanMethod = () =>
            {
                VfxManager.Clear();

                // 40 53 55 56 57 48 81 EC 08 02 00 00 0F 29 B4 24 F0 01 00 00 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 E0 01 00 00 0F B6 ??
                ActorVfxCreatePtr = Scanner.TryScan(
                    "E8 * * * * 48 8B D8 48 85 C0 74 ?? 0F B6 57 ?? 48 8B C8 C0 EA 02 80 E2 01", nameof(ActorVfxCreatePtr));

                // 48 89 5C 24 10 57 48 83 EC 20 48 8D 05 ?? ?? ?? ?? 48 8B D9 48 89 01 8B FA 48 8D 05 ?? ?? ?? ?? 48 89 81 B0 01 00 00 48
                var actorVfxRemovePtrPtr = Scanner.TryScan(
                    "0F 11 48 10 48 8D 05 * * * *", nameof(ActorVfxRemovePtr));
                if (actorVfxRemovePtrPtr != IntPtr.Zero)
                    ActorVfxRemovePtr = Memory.Read<IntPtr>(actorVfxRemovePtrPtr);

                // 48 89 5C 24 08 57 48 83 EC 20 48 8B 05 ?? ?? ?? ?? 48 8B F9 BA 80 03 00 00 41 B8 10 00 00 00 48 8B 48 30 48 8B 01 FF 50 ??
                StaticVfxCreatePtr = Scanner.TryScan(
                    "E8 * * * * F3 0F 10 35 ?? ?? ?? ?? 48 89 43 08", nameof(StaticVfxCreatePtr));

                // 48 89 5C 24 10 48 89 74 24 20 57 48 81 EC 90 00 00 00 0F 29 B4 24 80 00 00 00 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 44 24 ??
                StaticVfxRunPtr = Scanner.TryScanMultiple(new string[] {
                    "e8 * * * * 0f ? ? ? ? ? ? 66 ? ? ? 74 ?", // 7.3
                    "E8 * * * * 8B 4B 7C 85 C9", // 7.0
                }, nameof(StaticVfxRunPtr));

                StaticVfxFadeOutPtr = Scanner.TryScanMultiple(new string[] {
                    "0F 28 CA E8 * * * * 80 A3", // 某个传参时间非 0 的 xref
                    "83 89 ? ? ? ? ? F3 0F 11 89",
                }, nameof(StaticVfxFadeOutPtr));
            };
        }

        #region ActorVfx

        /// <summary> 点名特效 </summary>
        [CallbackMethod("LockOn")]
        internal void CbLockOn(string cmd)
        {
            CheckBeforeExecution(cmd);
            if (GetConfig<bool>("ActorVfx") == false) return; // ignored
            var (tgtAddress, vfxName, duration) = cmd.ParseArgs<IntPtr, string, double>((2, -1.0)); // 默认不移除
            CheckIfVfxNameTooShort(vfxName, "LockOn");
            var vfx = LockOnCreate(tgtAddress, vfxName);
            if (vfx == null) return;
            IntPtr vfxPtr = vfx.Ptr;
            ScheduleActorVfxRemove(vfxPtr, duration, true);
        }

        /// <summary> 连线特效 </summary>
        [CallbackMethod("Channeling")]
        internal void CbChanneling(string cmd)
        {
            CheckBeforeExecution(cmd);
            if (GetConfig<bool>("ActorVfx") == false) return; // ignored
            var (srcAddress, tgtAddress, vfxName, duration) = cmd.ParseArgs<IntPtr, IntPtr, string, double>((3, 3.0)); // 默认持续时间 3 秒
            CheckIfVfxNameTooShort(vfxName, "Channeling");
            var vfx = ChannelingCreate(srcAddress, tgtAddress, vfxName);
            if (vfx == null) return;
            IntPtr vfxPtr = vfx.Ptr;
            ScheduleActorVfxRemove(vfxPtr, duration);
        }

        /// <summary> 咏唱特效 </summary>
        [CallbackMethod("CastVfx")]
        internal void CbCastVfx(string cmd)
        {
            CheckBeforeExecution(cmd);
            if (GetConfig<bool>("ActorVfx") == false) return; // ignored
            var (srcAddress, vfxName, duration) = cmd.ParseArgs<IntPtr, string, double>((2, 3.0)); // 默认持续时间 3 秒
            CheckIfVfxNameTooShort(vfxName, "CastVfx");
            var vfx = CastVfxCreate(srcAddress, vfxName);
            if (vfx == null) return;
            IntPtr vfxPtr = vfx.Ptr;
            ScheduleActorVfxRemove(vfxPtr, duration);
        }

        /// <summary> 通用 ActorVfx </summary>
        [CallbackMethod("ActorVfx")]
        internal void CbActorVfx(string cmd)
        {
            CheckBeforeExecution(cmd);
            if (GetConfig<bool>("ActorVfx") == false) return; // ignored
            var (srcAddress, tgtAddress, vfxName, duration) = cmd.ParseArgs<IntPtr, IntPtr, string, double>((3, 3.0)); // 默认持续时间 3 秒
            CheckIfVfxNameTooShort(vfxName, "ActorVfx");
            var vfx = ActorVfxCreate(srcAddress, tgtAddress, vfxName);
            if (vfx == null) return;
            IntPtr vfxPtr = vfx.Ptr;
            ScheduleActorVfxRemove(vfxPtr, duration);
        }

        public ActorVfx LockOnCreate(IntPtr tgtAddress, string vfxName, string tag = Vfx.Vfx.DefaultTag)
            => ActorVfxCreate(tgtAddress, tgtAddress, $"vfx/lockon/eff/{vfxName}.avfx", tag, true);

        public ActorVfx ChannelingCreate(IntPtr srcAddress, IntPtr tgtAddress, string vfxName, string tag = Vfx.Vfx.DefaultTag)
            => ActorVfxCreate(srcAddress, tgtAddress, $"vfx/channeling/eff/{vfxName}.avfx", tag);

        public ActorVfx CastVfxCreate(IntPtr srcAddress, string vfxName, string tag = Vfx.Vfx.DefaultTag)
            => ActorVfxCreate(srcAddress, srcAddress, $"vfx/common/eff/{vfxName}.avfx", tag);

        public ActorVfx ActorVfxCreate(IntPtr srcAddress, IntPtr tgtAddress, string fullPath, string tag = Vfx.Vfx.DefaultTag, bool scheduleRemovalByGame = false, float unknownParamTest = -1f)
        {
            CheckIfAnyZeroPtr(ActorVfxCreatePtr);
            if (!scheduleRemovalByGame) CheckIfAnyZeroPtr(ActorVfxRemovePtr);
            CheckIfVfxPathValid(fullPath);
            if (srcAddress == IntPtr.Zero || tgtAddress == IntPtr.Zero)
            {
                WarningLog($"[鲶鱼精邮差扩展] ActorVfxCreate ({fullPath}) 实体地址无效：src = {(long)srcAddress:X}, tgt = {(long)tgtAddress:X}");
                return null;
            }
            if ((long)srcAddress <= 0xFFFF || (long)tgtAddress <= 0xFFFF)
                throw new Exception($"[鲶鱼精邮差扩展] ActorVfxCreate ({fullPath}) 实体地址无效：src = {(long)srcAddress:X}, tgt = {(long)tgtAddress:X}");
            var vfxPtr = Memory.WithAllocatedString(fullPath, Encoding.UTF8, pathPtr => Plugin.Call<IntPtr>(
                ActorVfxCreatePtr, pathPtr, srcAddress, tgtAddress, /*-1f*/unknownParamTest, (byte)0, 0, (byte)0
            ));
            var vfx = new ActorVfx()
            {
                Ptr = vfxPtr,
                Path = fullPath,
                Tag = tag
            };
            if (!scheduleRemovalByGame) // 临时应对方式，暂时未能检测 LockOn 是否已经被移除，所以不主动注册
            {
                VfxManager.Register(vfx);
            }
            Custom2Log($"[ActorVfxCreate] {fullPath} @ {(long)vfxPtr:X}");
            return vfx;
        }

        public bool TryActorVfxRemove(IntPtr vfxPtr, bool scheduleRemovalByGame = false) // 待优化：判断是否存在 vfx
        {
            CheckIfAnyZeroPtr(ActorVfxRemovePtr);

            var found = VfxManager.TryUnregisterActor(vfxPtr, out var vfx);

            if (!found && !scheduleRemovalByGame)
            {
                Custom2Log($"[ActorVfx] 移除特效：（已移除）@{(long)vfxPtr:X}");
                return false;
            }

            try
            {
                Plugin.Call(ActorVfxRemovePtr, vfxPtr, (byte)1); // a2: bool freeMemory
            }
            finally
            {
                Custom2Log($"[ActorVfx] 移除特效：{vfx?.Path ?? "unknown"} @ {(long)vfxPtr:X}");
            }

            return true;
        }

        public void ScheduleActorVfxRemove(IntPtr vfxPtr, double duration, bool scheduleRemovalByGame = false)
        {
            if (duration >= 0 && vfxPtr != IntPtr.Zero)
            {
                Task.Delay((int)(duration * 1000)).ContinueWith(_ => TryActorVfxRemove(vfxPtr, scheduleRemovalByGame));
            }
        }

        #endregion ActorVfx

        [CallbackMethod("Omen")]
        internal void CbOmen(string command) 
            => ProcessStaticVfx(command, "vfx/omen/eff/{0}.avfx");

        [CallbackMethod("StaticVfx")]
        internal void CbStaticVfx(string command)
            => ProcessStaticVfx(command);

        private void ProcessStaticVfx(string rawArgs, string nameFormatTemplate = null)
        {
            CheckBeforeExecution(rawArgs);
            if (GetConfig<bool>("StaticVfx") == false) return; // ignored
            var (vfxName, t, x, y, z, h, scaleX, rawScaleY, rawScaleZ, r, g, b, a) 
                = rawArgs.ParseArgs<string, float, float, float, float, float, float, float?, float?, float, float, float, float>(
                    (6, 1), (7, null), (8, null), (9, 1), (10, 1), (11, 1), (12, 1));

            CheckIfVfxNameTooShort(vfxName, "StaticVfx");
            var vfxPath = nameFormatTemplate == null ? vfxName : string.Format(nameFormatTemplate, vfxName);
            var pos = new Vector3(x, y, z);
            var scales = new Vector3(scaleX, rawScaleY ?? scaleX, rawScaleZ ?? scaleX);
            var color = new Vector4(r, g, b, a);

            var vfx = VfxManager.InitStatic(vfxPath, Vfx.Vfx.DefaultTag, v =>
            {
                v.Pos = pos;
                v.Angle = h;

                if (scales != Vector3.One)
                    v.Scales = scales;

                if (color != Vector4.One)
                    v.Color = color;
            });

            vfx.ScheduleRemove(t);
        }

        public IntPtr StaticVfxCreate(string fullPath)
        {
            CheckIfAnyZeroPtr(StaticVfxCreatePtr, StaticVfxRunPtr, StaticVfxFadeOutPtr);
            CheckIfVfxPathValid(fullPath);

            const string pool = "Client.System.Scheduler.Instance.VfxObject";
            var vfxPtr = Memory.WithAllocatedString2(fullPath, pool, Encoding.UTF8,
                (fullPathPtr, poolPtr) => Plugin.Call<IntPtr>(StaticVfxCreatePtr, fullPathPtr, poolPtr)
            );

            Custom2Log($"[StaticVfxCreate] {fullPath} @ {(long)vfxPtr:X}");
            return vfxPtr;
        }

        public void StaticVfxRun(IntPtr vfxPtr)
        {
            CheckIfAnyZeroPtr(StaticVfxRunPtr);
            Plugin.Call<IntPtr>(StaticVfxRunPtr, vfxPtr, 0.0f, -1);
        }

        /// <summary>
        /// fadeFrames60: 1f = one 60 FPS frame ≈ 0.0167 s.
        /// </summary>
        private void StaticVfxFadeout(IntPtr vfxPtr, float fadeFrames60)
        {
            CheckIfAnyZeroPtr(StaticVfxFadeOutPtr);
            _ = Plugin.Call<IntPtr>(StaticVfxFadeOutPtr, vfxPtr, fadeFrames60);
        }

        public bool TryStaticVfxRemove(IntPtr vfxPtr)
        {
            CheckIfAnyZeroPtr(StaticVfxFadeOutPtr);

            if (!VfxManager.TryUnregisterStatic(vfxPtr, out var vfx))
            {
                Custom2Log($"[StaticVfx] 移除特效：（已移除）@{(long)vfxPtr:X}");
                return false;
            }

            StaticVfxFadeout(vfxPtr, 10f);
            Custom2Log($"[StaticVfx] 已淡出特效记录：{vfx.Path} @ {(long)vfxPtr:X}");
            return true;
        }

        private void CheckIfVfxNameTooShort(string vfxName, string methodName)
        { 
            if (vfxName.Length <= 8)
                throw new Exception($"[鲶鱼精邮差扩展] {methodName} vfxName 参数过短：{vfxName}");
        }

        private void CheckIfVfxPathValid(string vfxPath)
        {
            if (!vfxPath.EndsWith(".avfx", StringComparison.OrdinalIgnoreCase))
                throw new Exception($"[鲶鱼精邮差扩展] vfxName 不以 \".avfx\" 结尾：{vfxPath}");
            if (Regex.IsMatch(vfxPath, @"\.(?!avfx)", RegexOptions.IgnoreCase))
                throw new Exception($"[鲶鱼精邮差扩展] vfxName 包含错误的扩展名：{vfxPath}");
        }

    }
}
