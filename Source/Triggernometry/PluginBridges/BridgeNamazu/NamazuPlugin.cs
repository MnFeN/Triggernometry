using System;
using System.Collections.Generic;
using System.Reflection;

namespace Triggernometry.PluginBridges.BridgeNamazu
{
    /// <summary>
    /// Wrapper for PostNamazu.PostNamazu
    /// </summary>
    public class NamazuPlugin
    {
        private readonly dynamic _plugin;

        private GreyMagicExternalProcessMemory _Memory;
        public GreyMagicExternalProcessMemory Memory
        {
            get
            {
                var current = _plugin.Memory;
                if (_Memory?.RawMemory != current)
                { 
                    _Memory = current == null ? null : new GreyMagicExternalProcessMemory(current);
                }
                return _Memory;
            }
        }

        private NamazuScanner _SigScanner;
        public NamazuScanner SigScanner
        {
            get
            {
                var current = _plugin.SigScanner;
                if (_SigScanner?.RawScanner != current)
                {
                    _SigScanner = current == null ? null : new NamazuScanner(current);
                }
                return _SigScanner;
            }
        }

        public bool IsReady => Triggernometry.Utilities.Memory.XivProc != null && Memory != null;

        private Func<object> _getNamazuUi;
        public object PluginUI => _getNamazuUi();

        public NamazuPlugin(object plugin)
        {
            _plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
            var pluginType = plugin.GetType();
            var fi = pluginType.GetField("PluginUi", BindingFlags.Instance | BindingFlags.NonPublic) 
                ?? pluginType.GetField("PluginUI", BindingFlags.Instance | BindingFlags.NonPublic) // 改过名
                ?? throw new MissingFieldException("PluginUi");
            _getNamazuUi = () => fi.GetValue(_plugin);
        }

        // Actions
        // public T GetModuleInstance<T>() where T : NamazuModule
        public static dynamic GetModuleInstance(string moduleName)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, bool> ActionEnabled => _plugin.ActionEnabled;

        public void DoAction(string command, string payload) 
            => _plugin.DoAction(command, payload);

        /// <summary>
        /// Force an action to be executed, bypassing the user config checks.
        /// </summary>
        /// <returns>If the user allows the action to be executed.</returns>
        public void DoActionForce(string command, string payload, string moduleName = null)
        {
            if (moduleName == null && !_commandToModuleNames.TryGetValue(command, out moduleName))
            {
                throw new ArgumentException($"Command '{command}' does not map to a module name.", nameof(command));
            }
            ExecuteWithForcedModuleState(moduleName, () => DoAction(command, payload));
        }

        public void ExecuteWithForcedModuleState(string moduleName, System.Action visitor)
        {
            if (!ActionEnabled.TryGetValue(moduleName, out bool enabled))
            {
                throw new KeyNotFoundException($"Module '{moduleName}' not found.");
            }
            try
            {
                ActionEnabled[moduleName] = true;
                visitor();
            }
            finally
            {
                ActionEnabled[moduleName] = enabled;
            }
        }

        public T ExecuteWithForcedModuleState<T>(string moduleName, Func<T> visitor)
        {
            if (!ActionEnabled.TryGetValue(moduleName, out bool enabled))
            {
                throw new KeyNotFoundException($"Module '{moduleName}' not found.");
            }
            try
            {
                ActionEnabled[moduleName] = true;
                return visitor();
            }
            finally
            {
                ActionEnabled[moduleName] = enabled;
            }
        }

        private static readonly IReadOnlyDictionary<string, string> _commandToModuleNames 
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "command", "Command" },
            { "DoTextCommand", "Command" },
            { "mark", "Mark" },
            { "normalcommand", "NormalCommand" },
            { "DoNormalTextCommand", "NormalCommand" },
            { "preset", "Preset" },
            { "DoInsertPreset", "Preset" },
            { "queue", "Queue" },
            { "DoQueueActions", "Queue" },
            { "sendkey", "DoSendKey" },
            { "place", "WayMark" },
            { "DoWaymarks", "WayMark" },
        };

        // Region detection
        public bool IsCN => _plugin.IsCN;
        public IntPtr FrameworkPtr => _plugin.FrameworkPtr;

    }
}