using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Triggernometry.PluginBridges
{
    [OverlayModule]
    internal static class ModuleAlliance
    {
        public static bool Ready; 
        private static object _partyMemoryManager;
        private static MethodInfo _getPartyListsMethod;

        static ModuleAlliance()
        {
            try
            {
                _partyMemoryManager = BridgeOverlay.Container.Resolve($"RainbowMage.OverlayPlugin.MemoryProcessors.Party.IPartyMemory, OverlayPlugin.Core");
                _getPartyListsMethod = _partyMemoryManager.GetType().GetMethod("GetPartyLists", BindingFlags.Public | BindingFlags.Instance)
                    ?? throw new Exception("GetPartyListsMethod not found");
                Ready = true;
            }
            catch (Exception ex)
            {
                RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Error,
                    I18n.Translate("internal/BridgeOverlay/initfail", "OverlayPlugin initialization failed due to: {0}", ex.ToString())
                );
                Ready = false;
            }
        }

        #region PartyList

        public static object GetPartyLists()
        {
            if (!Ready)
            {
                RealPlugin.plug.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Warning, "OverlayPlugin not ready");
                return new object();
            }
            var o = _getPartyListsMethod.Invoke(_partyMemoryManager, null);
            return o;
        }

        public class PartyLists
        {

        }

        #endregion PartyList

    }
}