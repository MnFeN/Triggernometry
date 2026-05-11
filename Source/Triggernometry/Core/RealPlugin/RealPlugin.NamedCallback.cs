using System;
using System.Collections.Generic;
using System.Linq;
using Triggernometry.PluginBridges;
using Triggernometry.PluginBridges.BridgeMachina;
using Triggernometry.UI.Forms;
using Triggernometry.Utilities;

namespace Triggernometry.Core
{

    public partial class RealPlugin
    {
        public class NamedCallback
        {

            public int Id { get; set; }
            public string Name { get; set; }
            public Delegate Callback { get; set; }
            public object Obj { get; set; }
            public string Registrant { get; set; }
            public DateTime RegistrationTime { get; set; }
            public DateTime? LastInvoked { get; set; }

            public void Invoke(string val)
            {
                Callback.DynamicInvoke(new object[] { Obj, val });
                LastInvoked = DateTime.Now;
            }

        }

        internal Dictionary<int, NamedCallback> callbacksById = new Dictionary<int, NamedCallback>();
        internal Dictionary<string, List<NamedCallback>> callbacksByName = new Dictionary<string, List<NamedCallback>>(StringComparer.OrdinalIgnoreCase);

        public void InvokeNamedCallback(string name, string val)
        {
            List<NamedCallback> cbs = new List<NamedCallback>();
            lock (callbacksByName)
            {
                if (callbacksByName.ContainsKey(name) == true)
                {
                    cbs.AddRange(callbacksByName[name]);
                }
            }
            foreach (NamedCallback nc in cbs)
            {
                nc.Invoke(val);
                /*
                try
                {
                    nc.Invoke(val);
                }
                catch (Exception ex)
                {
                    Exception inner = ex;
                    while (inner.InnerException != null)
                    {
                        inner = inner.InnerException;
                    }
                    FilteredAddToLog(DebugLevelEnum.Error, I18n.Translate("internal/NamedCallback/exception",
                        "Exception occurred when invoking named callback {0}:\n {1}", name, inner.ToString()));
                }
                 */
            }
        }

        /// <summary>
        /// Registers a <see cref="NamedCallback" /> using the provided ID. <br />
        /// This method is used automatically via reflection when ProxyPlugin processes callbacks that were registered too early (before RealPlugin was ready). <br />
        /// Not intended for use in user scripts or external plugins. <br />
        /// </summary>
        private void RegisterNamedCallback(int id, string name, Delegate callback, object o, string registrant)
        {
            NamedCallback nc = new NamedCallback
            {
                Id = id,
                Callback = callback,
                Obj = o,
                Name = name,
                Registrant = registrant,
                RegistrationTime = DateTime.Now,
            };
            lock (callbacksById)
            {
                callbacksById[id] = nc;
                if (callbacksByName.ContainsKey(name) == false)
                {
                    callbacksByName[name] = new List<NamedCallback>();
                }
                callbacksByName[name].Add(nc);
            }
        }

        /// <summary>
        /// Registers a named callback and returns a unique ID. <br />
        /// If <paramref name="allowDuplicatedName"/> is false, all callbacks with the same name are removed before registration. <br />
        /// Used in ProxyPlugin, and can also be invoked by Triggernometry user scripts.
        /// </summary>
        public int RegisterNamedCallback(string name, Delegate callback, object o = null, bool allowDuplicatedName = false, string registrant = "Triggernometry Script")
        {
            if (!allowDuplicatedName)
            {
                UnregisterNamedCallback(name);
            }

            lock (callbacksById)
            {
                // Find the first free positive integer ID
                int id = Enumerable.Range(1, int.MaxValue).Where(n => !callbacksById.ContainsKey(n)).First();
                RegisterNamedCallback(id, name, callback, o, registrant);
                return id;
            }
        }

        /// <summary>
        /// Unregisters the callback with the specified ID.
        /// This method is used by ProxyPlugin via reflection. <br />
        /// Not intended for use in user scripts or external plugins. <br />
        /// </summary>
        private void UnregisterNamedCallback(int id)
        {
            lock (callbacksById)
            {
                NamedCallback nc = null;
                if (callbacksById.ContainsKey(id) == false)
                {
                    return;
                }
                nc = callbacksById[id];
                callbacksById.Remove(id);
                callbacksByName[nc.Name].Remove(nc);
                if (callbacksByName[nc.Name].Count == 0)
                {
                    callbacksByName.Remove(nc.Name);
                }
            }
        }

        /// <summary>
        /// Unregisters all callbacks with the given name.
        /// </summary>
        public void UnregisterNamedCallback(string name)
        {
            lock (callbacksById)
            {
                if (!callbacksByName.ContainsKey(name))
                {
                    return;
                }
                foreach (NamedCallback nc in callbacksByName[name])
                {
                    callbacksById.Remove(nc.Id);
                }
                callbacksByName.Remove(name);
            }
        }

        private void RegisterDefaultNamedCallbacks()
        {
            _ = RegisterNamedCallback("UploadText", (Action<object, string>)UploadTextHelper.UploadTextV1Callback, registrant: nameof(RealPlugin));
            _ = RegisterNamedCallback("UploadTextV2", (Action<object, string>)UploadTextHelper.UploadTextV2Callback, registrant: nameof(RealPlugin));
            _ = RegisterNamedCallback("DisableCactbotTriggerSetsTts", (Action<object, string>)BridgeCactbot.DisableTriggerSetsTtsCallback, registrant: nameof(RealPlugin));
            _ = RegisterNamedCallback("RegisterActorControlCategories", (Action<object, string>)ActorControlPatcher.RegisterCategoriesCallback, registrant: nameof(RealPlugin));
            _ = RegisterNamedCallback("SideloadMachinaOpcodes", (Action<object, string>)OpcodeSideloader.Callback, registrant: nameof(RealPlugin));
            _ = RegisterNamedCallback("TraySliderInfo", (Action<object, string>)TraySlider.CallbackInfo, registrant: nameof(RealPlugin));
            _ = RegisterNamedCallback("TraySliderWarning", (Action<object, string>)TraySlider.CallbackWarning, registrant: nameof(RealPlugin));
            _ = RegisterNamedCallback("TraySliderError", (Action<object, string>)TraySlider.CallbackError, registrant: nameof(RealPlugin));
        }

    }

}
