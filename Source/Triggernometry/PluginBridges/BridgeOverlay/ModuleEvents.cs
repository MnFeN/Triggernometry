using Newtonsoft.Json.Linq;
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
    public static class ModuleEvents
    {
        public static bool Ready;
        private static object _eventDispatcher;

        private static MethodInfo _callHandlerMethod;

        static ModuleEvents()
        {
            Initialize();
        }

        internal static void Initialize()
        {
            try
            {
                _eventDispatcher = BridgeOverlay.Container.Resolve($"RainbowMage.OverlayPlugin.EventDispatcher, OverlayPlugin.Core");
                _callHandlerMethod = _eventDispatcher.GetType().GetMethod("CallHandler", BindingFlags.Public | BindingFlags.Instance)
                    ?? throw new ReflectionNotFoundException("EventDispatcher.CallHandler");
                Ready = true;
            }
            catch (Exception ex)
            {
                RealPlugin.Instance.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Error,
                    I18n.Translate("internal/BridgeOverlay/failInitModule",
                    "OverlayPlugin {1} module initialization failed due to: {0}",
                    ex.ToString(), "Combatant")
                );
                Ready = false;
                return;
            }
            Ready = true;
        }

        public static JToken CallOverlayHandler(JObject jObject)
            => CallOverlayHandler((object)jObject);

        public static JToken CallOverlayHandler(object jObject)
        {
            return (JToken)_callHandlerMethod.Invoke(_eventDispatcher, new object[] { jObject });
        }

    }
}