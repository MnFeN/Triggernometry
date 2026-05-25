using System;
using System.Text;
using Triggernometry.Expressions.Maths;
using static Triggernometry.Expressions.String.Utils.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class ShowTextGimmickHintModule : ModuleBase
    {
        public IntPtr GetUiModulePtr;
        public IntPtr FrameworkPtrPtr;
        public IntPtr ShowTextGimmickHintPtr;

        public ShowTextGimmickHintModule()
        {
            ScanMethod = () =>
            {
                // 国际服：FFXIVClientStructs/FFXIV/Client/System/Framework/Framework.cs
                FrameworkPtrPtr = Scanner.TryScanMultiple(new string[] {
                    "49 8B C4 48 8B 0D ? ? ? ? 48 8D 15 ? ? ? ? 48 89 05 * * * *", // 7.0 CN
                    "49 8B DC 48 89 1D * * * *" // 7.0 global
                }, nameof(FrameworkPtrPtr));
                // FFXIVClientStructs/FFXIV/Client/System/Framework/Framework.cs
                GetUiModulePtr = Scanner.TryScan(
                    "E8 * * * * 80 7B 1D 01", nameof(GetUiModulePtr));
                // FFXIVClientStructs/FFXIV/Client/UI/RaptureAtkModule.cs
                ShowTextGimmickHintPtr = Scanner.TryScanMultiple(new string[] {
                    "48 85 D2 0F 84 ?? ?? ?? ?? 4C 8B DC 49 89 6B ?? 56", // 7.3 原始
                    "44 8B CB 45 33 C0 48 8B D0 48 8B CF E8 * * * * E9", // 7.3
                    "44 8B CB 41 B0 01 48 8B D0 48 8B CF E8 * * * * E9", // 7.3 备用签名
                    "44 8B CE 48 8B C8 48 8B D7 E8 * * * * E9 ? ? ? ? 66 0F BA E6 0A 0F 83 ? ? ? ?", // 7.2TC
                }, nameof(ShowTextGimmickHintPtr));
            };
        }

        [CallbackMethod("Hint")]
        internal void CbHint(string command) => ShowTextGimmickHintRaw(true, command);

        [CallbackMethod("Warn")]
        internal void CbWarn(string command) => ShowTextGimmickHintRaw(false, command);

        private void ShowTextGimmickHintRaw(bool isHint, string command)
        {
            CheckBeforeExecution(command);
            var lines = command.Split(new[] { '\n' }, 2);
            string rawTime = lines[0].Trim();
            string text = lines.Length > 1 ? lines[1] : "";

            int timeIn100Ms = Math.Max(0, (int)(MathParser.Parse(rawTime) * 10));
            NamazuLog((isHint ? "[Hint]" : "[Warn]") + $": ({timeIn100Ms / 10.0:F1} s) {text}");

            ShowTextGimmickHint(isHint, text, timeIn100Ms);
        }

        public void ShowTextGimmickHint(bool isHint, string text, int timeIn100Ms)
        {
            CheckIfAnyZeroPtr(FrameworkPtrPtr, GetUiModulePtr, ShowTextGimmickHintPtr);
            var frameworkPtr = Memory.Read<IntPtr>(FrameworkPtrPtr);
            var uiModulePtr = Plugin.Call<IntPtr>(GetUiModulePtr, frameworkPtr);
            var raptureAtkModulePtr = Plugin.CallVirtualFunction<IntPtr>(uiModulePtr, 7);
            Memory.WithAllocatedString(text, Encoding.UTF8, stringPtr => 
            {
                Plugin.Call(ShowTextGimmickHintPtr, raptureAtkModulePtr, stringPtr, isHint ? 1 : 0, timeIn100Ms);
            });
        }
    }
}
