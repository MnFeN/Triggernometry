using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Triggernometry.Core;
using Triggernometry.Localization;

namespace Triggernometry.PluginBridges
{
    [OverlayModule]
    internal static class ModuleAtkStageMemory
    {
        public static bool Ready;
        public static object AtkStageMemoryManager;
        private static MethodInfo _getAddonMethod;
        private static MethodInfo _getAddonAddressMethod;

        static ModuleAtkStageMemory()
        {
            try
            {
                AtkStageMemoryManager = BridgeOverlay.Container.Resolve($"RainbowMage.OverlayPlugin.MemoryProcessors.AtkStage.IAtkStageMemory, OverlayPlugin.Core")
                    ?? throw new Exception(I18n.Translate("internal/BridgeOverlay/objectnotfound", "Object '{0}' was not found.", "AtkStageMemoryManager"));
                _getAddonMethod = AtkStageMemoryManager.GetType().GetMethod("GetAddon", BindingFlags.Public | BindingFlags.Instance)
                    ?? throw new Exception(I18n.Translate("internal/BridgeOverlay/objectnotfound", "Object '{0}' was not found.", "GetAddonMethod"));
                _getAddonAddressMethod = AtkStageMemoryManager.GetType().GetMethod("GetAddonAddress", BindingFlags.Public | BindingFlags.Instance)
                    ?? throw new Exception(I18n.Translate("internal/BridgeOverlay/objectnotfound", "Object '{0}' was not found.", "GetAddonAddressMethod"));
                Ready = true;
            }
            catch (Exception ex)
            {
                RealPlugin.Instance.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Error,
                    I18n.Translate("internal/BridgeOverlay/initfail", "OverlayPlugin initialization failed due to: {0}", ex.ToString())
                );
                Ready = false;
            }
        }

        #region AtkStageMemory

        public static IntPtr GetAddonAddress(string name)
        {
            return (IntPtr)_getAddonAddressMethod.Invoke(AtkStageMemoryManager, new object[] { name });
        }

        public static object GetAddon(string name)
        {
            return _getAddonMethod.Invoke(AtkStageMemoryManager, new object[] { name });
        }

        #endregion AtkStageMemory
    }
}