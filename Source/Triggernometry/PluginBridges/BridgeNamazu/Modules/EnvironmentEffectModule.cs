using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static Triggernometry.Utilities.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class EnvironmentEffectModule : ModuleBase
    {
        public IntPtr MapEffectFunctionPtr;
        public IntPtr EventFrameworkPtrPtr;
        public IntPtr EnvManagerPtrPtr;

        public EnvironmentEffectModule()
        {
            ScanMethod = () =>
            {
                // ECommons/ECommons/Hooks/MapEffect.cs
                // 原始 sig: 48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 8B FA 41 0F B7 E8
                MapEffectFunctionPtr = Scanner.TryScan("44 0F B7 40 ? E9 * * * * C3", nameof(MapEffectFunctionPtr));
                // ffcs EventFramework.Instance
                // EventFrameworkPtrPtr = Scanner.TryScan("4C 39 2D * * * * 74 14", nameof(EventFrameworkPtrPtr)); 7.2=
                EventFrameworkPtrPtr = Scanner.TryScan("48 83 3D * * * * * 74 ? E8 ? ? ? ? 48 8B C8 E8 ? ? ? ? 3C", nameof(EventFrameworkPtrPtr)); // 7.3 兼容7.2
                EnvManagerPtrPtr = Scanner.TryScan("0F 28 F2 48 8B 05 * * * *", nameof(EnvManagerPtrPtr));
            };
        }

        private static readonly Regex _mapEffectRegex = new Regex(
            @"^(?<data2>[0-9A-Fa-f]{4})(?<data1>[0-9A-Fa-f]{4})[:|](?<position>[0-9A-Fa-f]{1,8})$",
            RegexOptions.Compiled);

        [CallbackMethod("MapEffect")]
        internal void CbMapEffect(string multiLineCmd)
        {
            CheckBeforeExecution(multiLineCmd);
            if (GetConfig<bool>("MapEffect") == false) return; // ignored
            var cmds = multiLineCmd
                .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s) && !s.StartsWith("//"))
                .Select(s => s.Trim());
            var args = new List<(uint, ushort)>();
            foreach (var command in cmds)
            {
                try
                {
                    if (command.Contains(","))
                    {
                        var (position, item2, item3) = ParseArgs<uint, ushort, ushort?>(command, (2, null));
                        // 支持的参数格式如 (pos, data1, data2)，或 (pos, data2)，因为游戏中实际并未使用 data1
                        // 所以解析参数时忽略 data1，在提供两项时 data2 赋值为第二项，提供三项时 data2 赋值为第三项
                        var data2 = item3 ?? item2;
                        args.Add((position, data2));
                    }
                    else // given act or network log format
                    {
                        var match = _mapEffectRegex.Match(command);
                        if (match.Success)
                        {
                            var data2 = ushort.Parse(match.Groups["data2"].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                            var position = uint.Parse(match.Groups["position"].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                            args.Add((position, data2));
                        }
                        else
                        {
                            throw new Exception($"{command} 参数格式无法识别");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog($"[鲶鱼精邮差扩展] MapEffect 参数错误：{ex.Message}");
                    continue;
                }
            }
            Memory.ExecuteWithLock(() => {
                foreach (var (position, data2) in args)
                {
                    NamazuLog($"[MapEffect] position = {position}, data2 = {data2} ({data2:X4}XXXX:{position:X2})");
                    MapEffect(position, data2);
                }
            });
        }

        // 优化：获取当前地图是否有 EnvControl Instance 防止炸游戏
        public void MapEffect(uint position, ushort data1, ushort data2)
        {
            // Hyperborea/Hyperborea/Utils.cs
            // GetMapEffectModule() => *(nint*)(((nint)EventFramework.Instance()) + 344);
            CheckIfAnyZeroPtr(MapEffectFunctionPtr, EventFrameworkPtrPtr);
            var mapEffectModulePtr = Memory.Read<IntPtr>(Memory.Read<IntPtr>(EventFrameworkPtrPtr) + 0x158);
            Memory.CallInjected64<IntPtr>(MapEffectFunctionPtr, mapEffectModulePtr, position, data1, data2);
        }

        // data1 实际上没有使用
        // 更新：data1 为 0 一般情况下没问题，但在用 hyperborea 插件本地进入区域时无效。
        // 此时使用非 0 的 data1 执行一次之后就有效了，似乎和加载资源有关？ 改成 1 了
        public void MapEffect(uint position, ushort data2) => MapEffect(position, 1, data2);

        [CallbackMethod("ChangeWeather")]
        internal void CbChangeWeather(string command)
        {
            var weatherId = ParseArgs<byte>(command);
            CheckBeforeExecution(command);
            NamazuLog($"[ChangeWeather] {weatherId}");
            Memory.ExecuteWithLock(() => ChangeWeather(weatherId));
        }

        // FFXIVClientStructs/FFXIV/Client/Graphics/Environment/EnvManager.cs
        public void ChangeWeather(byte weatherId)
        {
            CheckIfAnyZeroPtr(EnvManagerPtrPtr);
            var envManagerPtr = Memory.Read<IntPtr>(EnvManagerPtrPtr);
            Memory.Write<byte>(envManagerPtr + 0x27, weatherId); // ActiveWeather
            Memory.Write<float>(envManagerPtr + 0x28, 1); // TransitionTime
        }

    }

}
