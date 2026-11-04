using System;
using System.Linq;
using System.Runtime.InteropServices;
using Triggernometry.Expressions.String.Utils;
using static Triggernometry.Expressions.String.Utils.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class InstanceAfkTimerModule : ModuleBase
    {
        // FFXIVClientStructs/FFXIV/Client/UI/Misc/InputTimerModule.cs
        // [FieldOffset(0x14)] public float ContentInputTimer;
        // [FieldOffset(0x18)] public float InputTimer;
        // [FieldOffset(0x1C)] private float Unk1C; // used in a couple different ways, no idea

        // 补丁对应的不是 timer 地址，而是自动累加计时器的 movss 指令地址
        public IntPtr PatchPtr;
        // F3 0F11 53 1C         - movss [rbx+1C],xmm2  未知用途的计时器
        // F3 0F11 43 14         - movss [rbx+14],xmm0  ContentInputTimer 副本内无手动输入时长的计时器
        // F3 0F11 4B 18         - movss [rbx+18],xmm1  InputTimer 无手动输入时长的计时器
        public static readonly byte[] PatchedBytes = Enumerable.Repeat((byte)0x90, 15).ToArray();
        public static readonly byte[] OriginalBytes = new byte[] { 
            0xF3, 0x0F, 0x11, 0x53, 0x1C, 
            0xF3, 0x0F, 0x11, 0x43, 0x14,
            0xF3, 0x0F, 0x11, 0x4B, 0x18 
        };

        public InstanceAfkTimerModule()
        {
            ScanMethod = () =>
            {
                // E9 在此仅用于定位 不跳转！
                PatchPtr = Scanner.ScanText("E9 ? ? ? ? F3 0F 10 53 1C", nameof(PatchPtr)) + 0x23;
            };
        }

        [CallbackMethod("DisableInstanceAfkTimer")]
        internal void CbDisableInstanceAfkTimer(string cmd)
        {
            CheckBeforeExecution(cmd);
            if (GetConfig<bool>("InstanceAfkTimer") == false) return; // ignored
            var shouldDisable = cmd.ParseDataOrDefault(true);
            DisableInstanceTimer(shouldDisable);
        }

        public void DisableInstanceTimer(bool shouldDisable)
        {
            CheckIfAnyZeroPtr(PatchPtr);
            var currentBytes = Memory.ReadBytes(PatchPtr, OriginalBytes.Length);
            bool? isDisabled = currentBytes.SequenceEqual(PatchedBytes) ? true :
                               currentBytes.SequenceEqual(OriginalBytes) ? false : (bool?)null;  
            if (isDisabled == null)
            {
                WarningLog("[鲶鱼精邮差扩展] 当前副本计时疑似被其他插件修改，无法禁用副本计时器。");
                return;
            }

            if (isDisabled == shouldDisable)
                return;

            Memory.WriteBytes(PatchPtr, shouldDisable ? PatchedBytes : OriginalBytes);
            CustomLog(shouldDisable ? "[鲶鱼精邮差扩展] 已禁用副本计时器。" : "[鲶鱼精邮差扩展] 已恢复副本计时器。");
        }
    }

}
