using System;
using Triggernometry.Expressions.String.Utils;
using static Triggernometry.Expressions.String.Utils.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class MovementModule : ModuleBase
    {
        public IntPtr MoveSpeedPtr;
        public IntPtr JumpHeightPtr;

        public MovementModule()
        {
            ScanMethod = () => 
            {
                MoveSpeedPtr = Scanner.TryScanMultiple(new[] {
                    "48 8D 0D * * * * E8 ? ? ? ? 84 C0 75 4F", // 7.0
                    "48 8D 0D * * * * 4C 8B CF 48 89 5C 24 ? 48 8B D3" // 7.0
                }, nameof(MoveSpeedPtr)) + 0x58;
                JumpHeightPtr = Scanner.TryScanMultiple(new[] {
                    "48 8D 0D * * * * E8 ? ? ? ? EB ? 48 8B 0D ? ? ? ? B2 ?" // 7.0
                }, nameof(JumpHeightPtr)) + 0x54;
            };
        }

        [CallbackMethod("SetMoveSpeedMultiplier", "Kairos")]
        internal void CbSetMoveSpeedMultiplier(string cmd)
        {
            CheckBeforeExecution(cmd);
            var multiplier = cmd.ParseDataOrDefault(1.0f);
            SetMoveSpeedMultiplier(multiplier);
        }

        [CallbackMethod("SetJumpHeightMultiplier", "Kairos")]
        internal void CbSetJumpHeightMultiplier(string cmd)
        {
            CheckBeforeExecution(cmd);
            var multiplier = cmd.ParseDataOrDefault(1.0f);
            SetJumpHeightMultiplier(multiplier);
        }

        public void SetMoveSpeedMultiplier(float multiplier)
        {
            CheckIfAnyZeroPtr(MoveSpeedPtr);
            Memory.Write(MoveSpeedPtr, 6f * multiplier);
        }

        public void SetJumpHeightMultiplier(float multiplier)
        {
            CheckIfAnyZeroPtr(JumpHeightPtr);
            Memory.Write(JumpHeightPtr, 10.4f * multiplier);
        }

    }
}
