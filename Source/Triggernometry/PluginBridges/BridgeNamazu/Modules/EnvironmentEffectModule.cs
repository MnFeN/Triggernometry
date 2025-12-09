using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Triggernometry.Expressions.String.Utils;
using static Triggernometry.Expressions.String.Utils.DataStringHelper;

namespace Triggernometry.PluginBridges.BridgeNamazu.Modules
{
    public class EnvironmentEffectModule : ModuleBase
    {
        public IntPtr MapEffectOldFunctionPtr;
        public IntPtr MapEffectFunctionPtr;
        public IntPtr EventFrameworkPtrPtr;
        public IntPtr EnvManagerPtrPtr;

        // Hyperborea/Hyperborea/Utils.cs
        // GetMapEffectModule() => *(nint*)(((nint)EventFramework.Instance()) + 344);
        // FFCS: ContentDirector (没提供偏移)
        /// <summary> 可能为 0，代表当前地图不存在 Director </summary>
        public IntPtr ContentDirectorPtr => Memory.Read<IntPtr>(Memory.Read<IntPtr>(EventFrameworkPtrPtr) + 0x158);

        public EnvironmentEffectModule()
        {
            ScanMethod = () =>
            {
                // ECommons/ECommons/Hooks/MapEffect.cs
                // 原始 sig: 48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 8B FA 41 0F B7 E8
                MapEffectOldFunctionPtr = Scanner.TryScan("44 0F B7 40 ? E9 * * * * C3", nameof(MapEffectOldFunctionPtr));
                // 上面函数的下一层
                // 原始 sig：48 89 6C 24 ?? 56 48 83 EC ?? 8B C2 41 0F B7 E8 45 33 C0 48 8D 14 40 48 8B 81 ?? ?? ?? ?? B1 ??
                MapEffectFunctionPtr = Scanner.TryScan("E8 * * * * 3C ? 75 ? 80 64 B3 ? ?", nameof(MapEffectFunctionPtr));
                // ffcs EventFramework.Instance
                EventFrameworkPtrPtr = Scanner.TryScan("48 83 3D * * * * * 74 ? E8 ? ? ? ? 48 8B C8 E8 ? ? ? ? 3C", nameof(EventFrameworkPtrPtr)); // 7.3 兼容7.2
                EnvManagerPtrPtr = Scanner.TryScan("0F 28 F2 48 8B 05 * * * *", nameof(EnvManagerPtrPtr));
            };
        }

        private static readonly Regex _mapEffectRegex = new Regex(
            @"^(?<flag>[0-9A-Fa-f]{4})(?<unknownFlag>[0-9A-Fa-f]{4})?[:|](?<index>[0-9A-Fa-f]{1,8})$",
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
            var args = new List<(uint, ushort?, ushort)>();
            foreach (var command in cmds)
            {
                try
                {
                    if (command.Contains(","))
                    {
                        var (index, unknownFlag, flag) = command.ParseArgs<uint, ushort?, ushort?>((2, null));
                        // 支持的参数格式如 (index, unknownFlag, flag)，或 (index, flag)，因为游戏中实际并未使用 unknownFlag
                        if (flag == null)
                            (unknownFlag, flag) = (flag, unknownFlag);
                        args.Add((index, unknownFlag, flag.Value));
                    }
                    else // 支持格式如 00020001:0F, 00020001|0F   或省略未使用的参数，如 0002:0F
                    {
                        var match = _mapEffectRegex.Match(command);
                        if (match.Success)
                        {
                            var flag = ushort.Parse(match.Groups["flag"].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                            var unknownFlag = match.Groups["unknownFlag"].Success 
                                ? ushort.Parse(match.Groups["unknownFlag"].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture)
                                : (ushort?)null;
                            var index = uint.Parse(match.Groups["index"].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                            args.Add((index, unknownFlag, flag));
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
                foreach (var (index, unknownFlag, flag) in args)
                {
                    if (!unknownFlag.HasValue)
                    {
                        NamazuLog($"[MapEffect] index = {index}, flag = {flag} ({flag:X4}????:{index:X2})");
                        MapEffect(index, flag);
                    }
                    else
                    {
                        NamazuLog($"[MapEffect] index = {index}, flag = {flag} ({flag:X4}{unknownFlag:X4}:{index:X2})");
#pragma warning disable CS0618 // 使用弃用方法的警告
                        MapEffectOld(index, unknownFlag.Value, flag);
#pragma warning restore CS0618
                    }
                }
            });
        }

        /// <summary> MapEffect 底层函数。 </summary>
        /// <returns> 是否调用成功。</returns>
        public bool MapEffect(uint index, ushort flag)
        {
            CheckIfAnyZeroPtr(MapEffectFunctionPtr, EventFrameworkPtrPtr);
            var contentDirectorPtr = ContentDirectorPtr;
            if (contentDirectorPtr != IntPtr.Zero)
            {
                bool success = Memory.CallInjected64<bool>(MapEffectFunctionPtr, contentDirectorPtr, index, flag);
                if (!success)
                {
                    WarningLog($"[鲶鱼精邮差扩展] 当前地图 {BridgeFFXIV.ZoneID} 中 MapEffect ({index}, {flag}) 调用失败。");
                }
                return success;
            }
            else
            {
                ErrorLog($"[鲶鱼精邮差扩展] 当前地图 {BridgeFFXIV.ZoneID} 不存在 Director，无法调用 MapEffect ({index}, {flag})。");
                return false;
            }
        }

        /// <summary> <see cref="MapEffect" /> 的上一层函数，第二个参数并未实际使用。 </summary>
        [Obsolete("Use MapEffect(uint index, ushort flag)")]
        public void MapEffectOld(uint index, ushort unknownFlag, ushort flag)
        {
            
            CheckIfAnyZeroPtr(MapEffectOldFunctionPtr, EventFrameworkPtrPtr);
            var contentDirectorPtr = ContentDirectorPtr;
            if (contentDirectorPtr != IntPtr.Zero)
            {
                Memory.CallInjected64<IntPtr>(MapEffectOldFunctionPtr, contentDirectorPtr, index, unknownFlag, flag);
            }
            else
            {
                ErrorLog($"[鲶鱼精邮差扩展] 当前地图 {BridgeFFXIV.ZoneID} 不存在 Director，无法调用 MapEffect (Old) ({index}, {unknownFlag}, {flag})。");
            }
        }

        [CallbackMethod("ChangeWeather")]
        internal void CbChangeWeather(string command)
        {
            var weatherId = command.ParseData<byte>();
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
