using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Triggernometry.PluginBridges.BridgeNamazu
{
    /// <summary>
    /// Wrapper for PostNamazu.PostNamazu
    /// </summary>
    public class NamazuPlugin
    {
        private readonly dynamic _plugin;
        public object CommandModule => GetOriginalModuleByName("Command");
        public object MarkModule => GetOriginalModuleByName("Mark");
        public object NormalCommandModule => GetOriginalModuleByName("NormalCommand");
        public object PresetModule => GetOriginalModuleByName("Preset");
        public object QueueModule => GetOriginalModuleByName("Queue");
        public object SendKeyModule => GetOriginalModuleByName("SendKey");
        public object WayMarkModule => GetOriginalModuleByName("WayMark");

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

        public object GetOriginalModuleByName(string moduleName)
        {
            var modulesField = _plugin.GetType().GetField("Modules", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new Exception($"[鲶鱼精邮差扩展] 未找到 Modules 列表。");
            var modules = modulesField.GetValue(_plugin) as IList
                ?? throw new Exception($"[鲶鱼精邮差扩展] Modules 实例不是列表。");
            return modules.Cast<object>().FirstOrDefault(m => m.GetType().Name.Equals(moduleName, StringComparison.OrdinalIgnoreCase));
        }

        public Dictionary<string, bool> ActionEnabled => _plugin.ActionEnabled;

        public bool IsActionEnabled(string cmdOrModuleName)
        {
            if (!_commandToModuleNames.TryGetValue(cmdOrModuleName, out var moduleName))
            { 
                moduleName = cmdOrModuleName;
            }
            if (!ActionEnabled.TryGetValue(moduleName, out bool enabled))
            {
                throw new KeyNotFoundException($"Module '{moduleName}' not found.");
            }
            return enabled;
        }

        public void DoAction(string command, string payload) 
            => _plugin.DoAction(command, payload);

        /// <summary>
        /// Force an action to be executed, bypassing the user config checks.
        /// </summary>
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
        public bool IsTC => FFXIV.GameLanguage.Language == FFXIV.GameLanguageEnum.TCN;
        public IntPtr FrameworkPtr => _plugin.FrameworkPtr;

    }
}