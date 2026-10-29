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
                if (Callback is Action<object, string> callback)
                {
                    try 
                    { 
                        callback(Obj, val); 
                    }
                    catch (Exception ex) 
                    { 
                        throw new System.Reflection.TargetInvocationException(ex); 
                    }
                }
                else // actually does not support other types for now
                {
                    Callback.DynamicInvoke(new object[] { Obj, val });
                }
                LastInvoked = DateTime.Now;
            }

        }

        internal Dictionary<int, NamedCallback> callbacksById = new Dictionary<int, NamedCallback>();
        internal Dictionary<string, NamedCallback[]> callbacksByName = new Dictionary<string, NamedCallback[]>(StringComparer.OrdinalIgnoreCase);

        public void InvokeNamedCallback(string name, string val)
        {
            NamedCallback[] callbacks;
            lock (callbacksById)
            {
                if (!callbacksByName.TryGetValue(name, out callbacks)) return;
            }
            // 注册和注销只替换数组；回调在锁外执行，并继续使用本次取得的快照。
            foreach (NamedCallback callback in callbacks)
            {
                callback.Invoke(val);
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
                callbacksByName.TryGetValue(name, out NamedCallback[] callbacks);
                int count = callbacks?.Length ?? 0;
                Array.Resize(ref callbacks, count + 1);
                callbacks[count] = nc;
                callbacksByName[name] = callbacks;
            }
        }

        /// <summary>
        /// Registers a named callback and returns a unique ID. <br />
        /// If <paramref name="allowDuplicatedName"/> is false, all callbacks with the same name are removed before registration. <br />
        /// Used in ProxyPlugin, and can also be invoked by Triggernometry user scripts.
        /// </summary>
        public int RegisterNamedCallback(string name, Delegate callback, object o = null, bool allowDuplicatedName = false, string registrant = "Triggernometry Script")
        {
            lock (callbacksById)
            {
                // 替换同名注册、分配 ID 和写入必须在同一临界区，避免并发替换留下重复项。
                if (!allowDuplicatedName)
                    UnregisterNamedCallback(name);

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
                if (!callbacksById.TryGetValue(id, out NamedCallback callback)) return;
                callbacksById.Remove(id);
                NamedCallback[] callbacks = callbacksByName[callback.Name];
                int index = Array.IndexOf(callbacks, callback);
                if (index < 0) return;
                if (callbacks.Length == 1)
                {
                    callbacksByName.Remove(callback.Name);
                }
                else
                {
                    var remaining = new NamedCallback[callbacks.Length - 1];
                    Array.Copy(callbacks, 0, remaining, 0, index);
                    Array.Copy(callbacks, index + 1, remaining, index, remaining.Length - index);
                    callbacksByName[callback.Name] = remaining;
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
                if (!callbacksByName.TryGetValue(name, out NamedCallback[] callbacks)) return;
                foreach (NamedCallback nc in callbacks)
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
