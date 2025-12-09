using System;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class AbilityRangeCheckModule : ModuleBase
    {
        public IntPtr PatchPtr;
        public byte[] PatchedBytes = new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00 };
        public byte[] OriginalBytes = new byte[] { 0xB8, 0x36, 0x02, 0x00, 0x00 };

        public AbilityRangeCheckModule()
        {
            ScanMethod = () =>
            {
                // E8 在此仅用于定位 不跳转！
                PatchPtr = Scanner.ScanText("E8 ? ? ? ? 85 C0 75 02 33 C0", nameof(PatchPtr)) + 0x4B;
            };
        }

        [CallbackMethod("DisableAbilityRangeCheck", "Kairos")]
        internal void CbDisableAbilityRangeCheck(string cmd)
        {
            CheckBeforeExecution(cmd);
            var shouldDisable = cmd.ParseDataOrDefault(true);
            DisableAbilityRangeCheck(shouldDisable);
        }

        public void DisableAbilityRangeCheck(bool shouldDisable)
        {
            CheckIfAnyZeroPtr(PatchPtr);
            Memory.WriteBytes(PatchPtr, shouldDisable ? PatchedBytes : OriginalBytes);
            CustomLog(shouldDisable ? "[Kairos] 开启屏蔽技能距离检测。" : "[Kairos] 已恢复技能距离检测。");
        }
    }
}
