using System;
using Triggernometry.Expressions.String.Utils;
using static Triggernometry.Expressions.String.Utils.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class QuitInstanceModule : ModuleBase
    {
        public IntPtr QuitInstancePtr;

        public QuitInstanceModule()
        {
            ScanMethod = () =>
            {
                QuitInstancePtr = Scanner.TryScan("48 83 EC ?? 0F B6 D1 45 33 C9", nameof(QuitInstancePtr));
            };
        }

        [CallbackMethod("QuitInstance")]
        internal void CbQuitInstance(string cmd)
        {
            CheckBeforeExecution(cmd);
            var shouldForceQuit = cmd.ParseDataOrDefault(false);
            Memory.ExecuteWithLock(() => QuitInstance(shouldForceQuit));
        }

        public void QuitInstance(bool shouldForceQuit)
        {
            CheckIfAnyZeroPtr(QuitInstancePtr);
            Memory.CallInjected64(QuitInstancePtr, shouldForceQuit ? 1 : 0);
        }
    }
}
