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

            };
        }

    }

}
