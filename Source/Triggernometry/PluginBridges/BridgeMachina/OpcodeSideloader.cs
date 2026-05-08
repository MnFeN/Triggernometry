using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Triggernometry.Core;

namespace Triggernometry.PluginBridges.BridgeMachina
{
    public static class OpcodeSideloader
    {
        public static void Callback(object _, string rawOpcodes)
        {
            try
            {
                SideloadOpcodes(rawOpcodes);
            }
            catch (Exception ex)
            {
                LogError("Failed to sideload opcodes: " + ex.Message);
            }
        }

        private static void Log(string message)
            => RealPlugin.Instance.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Custom,
                $"[{nameof(OpcodeSideloader)}] {message}");

        private static void LogError(string message)
            => RealPlugin.Instance.UnfilteredAddToLog(RealPlugin.DebugLevelEnum.Error,
                $"[{nameof(OpcodeSideloader)}] {message}");

        private static Exception ReflectFail(string name)
            => new Exception($"Reflection lookup failed: {name}");

        public static void SideloadOpcodes(string rawSideloadOpcodes)
        {
            var original = GetCurrentOpcodes() ?? throw ReflectFail("CurrentOpcodes");
            var sideload = GetSideloadOpcodes(rawSideloadOpcodes);

            var updated = new Dictionary<string, (ushort, ushort)>(StringComparer.OrdinalIgnoreCase);
            var extra = new Dictionary<string, ushort>(StringComparer.OrdinalIgnoreCase);

            foreach (var sideloadKv in sideload)
            {
                // 忽略大小写匹配 key
                var matchedKey = original.Keys.FirstOrDefault(k => string.Equals(k, sideloadKv.Key, StringComparison.OrdinalIgnoreCase));
                if (matchedKey != null)
                {
                    var oldValue = original[matchedKey];
                    var newValue = sideloadKv.Value;
                    original[matchedKey] = newValue;
                    updated[matchedKey] = (oldValue, newValue);
                }
                else
                {
                    extra[sideloadKv.Key] = sideloadKv.Value;
                }
            }

            var untouched = original.Where(kv => !updated.ContainsKey(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value);

            // ---- 输出对比报告 ----
            var sb = new StringBuilder();
            sb.AppendLine("Opcodes sideload initiated successfully:\n");

            if (updated.Count > 0)
            {
                sb.AppendLine($"Updated opcodes × {updated.Count}:");
                foreach (var kv in updated
                    .Select(kv => new { kv, match = Regex.Match(kv.Key, @"^(.+?)(\d*)$") })
                    .OrderBy(x => x.match.Groups[1].Value, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(x => int.Parse("0" + x.match.Groups[2].Value)) // 这样可以让 XXX4 XXX8 排在 XXX16 前面
                    .Select(x => x.kv))
                {
                    ushort oldVal = kv.Value.Item1;
                    ushort newVal = kv.Value.Item2;
                    sb.AppendLine($"  - {kv.Key}: 0x{oldVal:X} => 0x{newVal:X}");
                }
                sb.AppendLine();
            }

            if (untouched.Count > 0)
            {
                sb.AppendLine($"Untouched opcodes × {untouched.Count}:");
                foreach (var kv in untouched
                    .Select(kv => new { kv, match = Regex.Match(kv.Key, @"^(.+?)(\d*)$") })
                    .OrderBy(x => x.match.Groups[1].Value, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(x => int.Parse("0" + x.match.Groups[2].Value))
                    .Select(x => x.kv))
                    sb.AppendLine($"  - {kv.Key}: 0x{kv.Value:X}");
                sb.AppendLine();
            }

            if (extra.Count > 0)
            {
                sb.AppendLine($"Extra opcodes × {extra.Count}:");
                foreach (var kv in extra
                    .Select(kv => new { kv, match = Regex.Match(kv.Key, @"^(.+?)(\d*)$") })
                    .OrderBy(x => x.match.Groups[1].Value, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(x => int.Parse("0" + x.match.Groups[2].Value))
                    .Select(x => x.kv))
                    sb.AppendLine($"  - {kv.Key}: 0x{kv.Value:X}");
                sb.AppendLine();
            }

            Log(sb.ToString());

            // ---- 真正让修改生效 ----
            ApplyOpcodes(original);

            Log("Machina opcode structures updated successfully.");
        }

        /// <summary>
        /// 接受输入如： <br />
        /// ActorCast | 36F <br />
        /// ActorControl | 1CD <br />
        /// ...
        /// </summary>
        public static Dictionary<string, ushort> GetSideloadOpcodes(string rawOpcodes)
        {
            if (string.IsNullOrWhiteSpace(rawOpcodes))
                throw new Exception("No opcode data provided to OpcodeSideloader callback.");

            var dict = new Dictionary<string, ushort>(StringComparer.OrdinalIgnoreCase);
            var lines = rawOpcodes
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .Where(line => line.Length > 0 && !line.StartsWith("//") && !line.StartsWith("#"));

            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length != 2)
                    throw new Exception("Invalid line format: " + line);

                var key = parts[0].Trim();
                var valStr = parts[1].Trim();

                if (key.Length == 0 || valStr.Length == 0)
                    throw new Exception("Invalid line (empty key or value): " + line);

                if (!ushort.TryParse(valStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ushort value))
                    throw new Exception("Unable to parse hex value: " + valStr);

                dict[key] = value;
            }

            return dict;
        }

        #region Machina

        private static Assembly MachinaAssembly =>
            AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "Machina.FFXIV")
            ?? throw ReflectFail("Machina.FFXIV Assembly");

        private static Assembly NetworkAssembly =>
            AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "FFXIV_ACT_Plugin.Network")
            ?? throw ReflectFail("FFXIV_ACT_Plugin.Network Assembly");

        private static Type OpcodeManagerType =>
            MachinaAssembly.GetType("Machina.FFXIV.Headers.Opcodes.OpcodeManager")
            ?? throw ReflectFail("OpcodeManager");

        private static Type ServerMessageType =>
            MachinaAssembly.GetType("Machina.FFXIV.Headers.Server_MessageType")
            ?? throw ReflectFail("Server_MessageType");

        private static Type GameRegionType =>
            MachinaAssembly.GetType("Machina.FFXIV.GameRegion")
            ?? throw ReflectFail("GameRegion");

        private static Type PacketHandlerMediatorType =>
            NetworkAssembly.GetType("FFXIV_ACT_Plugin.Network.PacketHandlerMediator")
            ?? throw ReflectFail("PacketHandlerMediator");

        public static Dictionary<string, ushort> GetCurrentOpcodes()
        {
            var instanceProp = OpcodeManagerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)
                ?? throw ReflectFail("OpcodeManager.Instance Property");

            var instance = instanceProp.GetValue(null)
                ?? throw ReflectFail("OpcodeManager.Instance Value");

            var currentProp = OpcodeManagerType.GetProperty("CurrentOpcodes", BindingFlags.Public | BindingFlags.Instance)
                ?? throw ReflectFail("OpcodeManager.CurrentOpcodes Property");

            var rawOpcodes = currentProp.GetValue(instance) as IDictionary
                ?? throw ReflectFail("OpcodeManager.CurrentOpcodes Value");

            var result = new Dictionary<string, ushort>(StringComparer.OrdinalIgnoreCase);
            foreach (DictionaryEntry entry in rawOpcodes)
            {
                if (entry.Key == null || entry.Value == null)
                    continue;

                result[entry.Key.ToString()] = Convert.ToUInt16(entry.Value, CultureInfo.InvariantCulture);
            }

            return result;
        }

        public static void ApplyOpcodes(Dictionary<string, ushort> replaceDict)
        {
            var instanceProp = OpcodeManagerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)
                ?? throw ReflectFail("OpcodeManager.Instance Property");

            var opcodeManagerInstance = instanceProp.GetValue(null)
                ?? throw ReflectFail("OpcodeManager.Instance Value");

            var regionName = GetCurrentMachinaRegionName(opcodeManagerInstance);

            // 更新 OpcodeManager._opcodes[当前区域]
            UpdateOpcodeManagerBackingStore(opcodeManagerInstance, regionName, replaceDict);

            // 让 OpcodeManager.CurrentOpcodes 切回当前区域，并读到刚写入的值
            SetOpcodeManagerRegion(opcodeManagerInstance, regionName);

            // 更新 Server_MessageType 静态字段
            var internalValueProp = ServerMessageType.GetProperty("InternalValue", BindingFlags.Public | BindingFlags.Instance)
                ?? throw ReflectFail("Server_MessageType.InternalValue Property");

            foreach (var field in ServerMessageType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                var isInitOnly = typeof(FieldInfo).GetField("m_isInitOnly", BindingFlags.NonPublic | BindingFlags.Instance);
                isInitOnly?.SetValue(field, false);

                var instance = Activator.CreateInstance(ServerMessageType);
                if (replaceDict.TryGetValue(field.Name, out ushort newVal))
                    internalValueProp.SetValue(instance, newVal);

                field.SetValue(null, instance);
            }

            // 不要手动重建 _packetHandlers；让 FFXIV_ACT_Plugin 自己按当前版本结构重载
            RefreshPacketHandlers();
        }

        private static string GetCurrentMachinaRegionName(object opcodeManagerInstance)
        {
            var gameRegionProperty = OpcodeManagerType.GetProperty("GameRegion", BindingFlags.Public | BindingFlags.Instance)
                ?? throw ReflectFail("OpcodeManager.GameRegion Property");

            var gameRegion = gameRegionProperty.GetValue(opcodeManagerInstance)
                ?? throw ReflectFail("OpcodeManager.GameRegion Value");

            return gameRegion.ToString();
        }

        private static object GetMachinaRegion(string regionName)
        {
            try
            {
                return Enum.Parse(GameRegionType, regionName);
            }
            catch (Exception ex)
            {
                throw new Exception($"[OpcodeSideloader] Unsupported Machina GameRegion: {regionName}", ex);
            }
        }

        private static void SetOpcodeManagerRegion(object opcodeManagerInstance, string regionName)
        {
            var setRegionMethod = OpcodeManagerType.GetMethod("SetRegion", new[] { GameRegionType })
                ?? throw ReflectFail("OpcodeManager.SetRegion Method");

            setRegionMethod.Invoke(opcodeManagerInstance, new[] { GetMachinaRegion(regionName) });
        }

        private static void UpdateOpcodeManagerBackingStore(object opcodeManagerInstance, string regionName, Dictionary<string, ushort> opcodes)
        {
            var opcodesField = OpcodeManagerType.GetField("_opcodes", BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw ReflectFail("OpcodeManager._opcodes Field");

            var regionOpcodes = opcodesField.GetValue(opcodeManagerInstance) as IDictionary
                ?? throw ReflectFail("OpcodeManager._opcodes Value");

            regionOpcodes[GetMachinaRegion(regionName)] = new Dictionary<string, ushort>(opcodes);
        }

        private static void RefreshPacketHandlers()
        {
            var mediatorInstance = GetPacketHandlerMediator();

            var loadPacketHandlers = mediatorInstance.GetType()
                .GetMethod("LoadPacketHandlers", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw ReflectFail("PacketHandlerMediator.LoadPacketHandlers Method");

            loadPacketHandlers.Invoke(mediatorInstance, null);
        }

        private static object GetPacketHandlerMediator()
        {
            var actPluginInstance = BridgeFFXIV.GetInstance()
                ?? throw ReflectFail("BridgeFFXIV Instance");

            // 新版 FFXIV_ACT_Plugin：_dataCollection -> _scanPackets -> _packetHandlerMediator
            var dataCollection = actPluginInstance.GetType()
                .GetField("_dataCollection", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(actPluginInstance);

            if (dataCollection != null)
            {
                var scanPackets = dataCollection.GetType()
                    .GetField("_scanPackets", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.GetValue(dataCollection);

                if (scanPackets != null)
                {
                    var mediator = scanPackets.GetType()
                        .GetField("_packetHandlerMediator", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.GetValue(scanPackets);

                    if (mediator != null)
                        return mediator;
                }
            }

            // 兼容旧逻辑：_iocContainer.GetService(PacketHandlerMediatorType)
            var iocContainer = actPluginInstance.GetType()
                .GetField("_iocContainer", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(actPluginInstance)
                ?? throw ReflectFail("iocContainer Field");

            var iocGetService = iocContainer.GetType().GetMethod("GetService")
                ?? throw ReflectFail("iocContainer.GetService Method");

            return iocGetService.Invoke(iocContainer, new object[] { PacketHandlerMediatorType })
                ?? throw ReflectFail("PacketHandlerMediator Instance");
        }

        #endregion Machina

    }
}