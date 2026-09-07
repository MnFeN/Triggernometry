using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Triggernometry.Core;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.FFXIV.LogTranscribe
{
    internal static class LogTranscriber
    {
        private static readonly object StateLock = new object();
        private static int _generation = 0;

        internal static void Reset()
        {
            lock (StateLock)
            {
                unchecked
                {
                    _generation++;
                }

                _territoryId = "";
                _territoryName = "";
                CombatantSeenUntil.Clear();
            }
        }

        private static bool IsCurrentGeneration(int generation)
        {
            lock (StateLock)
            {
                return generation == _generation;
            }
        }

        internal static void Process(string logLine, string zone)
        {
            if (string.IsNullOrEmpty(logLine))
                return;

            try
            {
                string logNumber = GetLogNumber(logLine);

                switch (logNumber)
                {
                    case "01": ProcessTerritory(logLine, zone); break;
                    case "1B": ProcessTargetIcon(logLine, zone); break;
                    case "22": ProcessNameToggle(logLine, zone); break;
                    case "23": ProcessTether(logLine, zone); break;
                    case "105": ProcessCombatant(logLine, zone); break;
                    case "10F": ProcessActorSetPos(logLine, zone); break;
                    case "110": ProcessActorSpawnExtra(logLine, zone); break;
                    case "111": ProcessActorControl(logLine, zone); break;
                }
            }
            catch (Exception ex)
            {
                RealPlugin.Instance.FilteredAddToLog(
                    RealPlugin.DebugLevelEnum.Error,
                    $"Exception while transcribing FFXIV log line ({logLine}): {ex}");
            }
        }

        /// <summary> e.g. "1B" "10F"; null if failed </summary>
        private static string GetLogNumber(string logLine)
        {
            // [00:00:00.000] TargetIcon 1B:...
            if (logLine.Length <= 15)
                return null;

            // TargetIcon ←
            int nameEnd = logLine.IndexOf(' ', 15);
            if (nameEnd < 0 || nameEnd + 1 >= logLine.Length)
                return null;

            // →1B:
            int numStart = nameEnd + 1;
            int searchLength = Math.Min(5, logLine.Length - numStart);
            // 1B:←
            int numEnd = logLine.IndexOf(':', numStart, searchLength);

            if (numEnd < 0)
                return null;

            return logLine.Substring(nameEnd + 1, numEnd - nameEnd - 1);
        }

        #region 01

        private static string _territoryId = "";
        private static string _territoryName = "";

        private static readonly Regex TerritoryRegex = new Regex(
            @"^(?<time>.{14}) \S+ 01:(?<id>[^:]*):(?<name>[^:]*)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static void ProcessTerritory(string logLine, string zone)
        {
            Match m = TerritoryRegex.Match(logLine);
            if (!m.Success)
                return;

            string id = m.Groups["id"].Value;
            string name = m.Groups["name"].Value;

            string previousId;
            string previousName;

            lock (StateLock)
            {
                previousId = _territoryId;
                previousName = _territoryName;

                _territoryId = id;
                _territoryName = name;
            }

            // 同一 Territory 重复出现时不转录。
            if (string.Equals(id, previousId, StringComparison.OrdinalIgnoreCase))
                return;

            QueueLog(
                $"{m.Groups["time"].Value} _Territory 1001:" +
                $"{id.ParseHexInt()}:{name}:{previousId.ParseHexInt()}:{previousName}",
                zone);
        }

        #endregion

        #region 1B
        private static readonly Regex TargetIconRegex = new Regex(
            @"^(?<time>.{14}) \S+ 1B:(?<sid>.{8}):[^:]*:[^:]*:[^:]*:(?<type>[^:]+):",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static void ProcessTargetIcon(string logLine, string zone)
        {
            Match m = TargetIconRegex.Match(logLine);
            if (!m.Success)
                return;

            string sid = m.Groups["sid"].Value;
            Entity entity = GetEntity(sid);

            // _TargetIcon AAA:1B:01A0:40123456:<entity>
            QueueLog(
                $"{m.Groups["time"].Value} _TargetIcon 101B:" +
                $"{m.Groups["type"].Value}:{sid}:{FormatEntityFull(entity)}",
                zone);
        }
        #endregion

        #region 22

        private static readonly Regex NameToggleRegex = new Regex(
           @"^(?<time>.{14}) \S+ 22:(?<id>4.{7}):[^:]*:[^:]*:[^:]*:(?<targetable>0?[01])",
           RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static void ProcessNameToggle(string logLine, string zone)
        {
            Match m = NameToggleRegex.Match(logLine);
            if (!m.Success)
                return;

            string id = m.Groups["id"].Value;
            Entity entity = GetEntity(id);

            if (!m.Groups["targetable"].Value.TryParseData(out int targetable))
            {
                return;
            }

            QueueLog(
                $"{m.Groups["time"].Value} _NameToggle 1022:" +
                $"{targetable}:{id}:{FormatEntityFull(entity)}",
                zone);
        }
        #endregion

        #region 23
        private static readonly Regex TetherRegex = new Regex(
            @"^(?<time>.{14}) \S+ 23:(?<sid>.{8}):[^:]*:(?<tid>.{8}):[^:]*:[^:]*:[^:]*:(?<type>[^:]*)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static void ProcessTether(string logLine, string zone)
        {
            Match m = TetherRegex.Match(logLine);
            if (!m.Success)
                return;

            string sid = m.Groups["sid"].Value;
            string tid = m.Groups["tid"].Value;

            // 只处理至少一端为 40xxxxxx 的连线，忽略玩家之间的连线。
            if (!sid.StartsWith("40", StringComparison.OrdinalIgnoreCase) &&
                !tid.StartsWith("40", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            Entity source = GetEntity(sid);
            Entity target = GetEntity(tid);

            QueueLog(
                $"{m.Groups["time"].Value} _Tether 1023:" +
                $"{m.Groups["type"].Value}:" +
                $"{sid}:{FormatEntityFull(source)}:" +
                $"{tid}:{FormatEntityFull(target)}",
                zone);
        }
        #endregion

        #region 105

        private static readonly Dictionary<string, DateTime> CombatantSeenUntil =
            new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

        private static readonly Regex CombatantRegex = new Regex(
            @"^(?<time>.{14}) \S+ 105:Add:(?<id>4.{7}):(?<data>.*Type:(?<type>\d+).*)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static void ProcessCombatant(string logLine, string zone)
        {
            Match m = CombatantRegex.Match(logLine);
            if (!m.Success)
                return;

            // bypass PC and BattleNPC
            if (!m.Groups["type"].Value.TryParseInt(out var objectKind) || objectKind <= 2)
            {
                return;
            }

            string id = m.Groups["id"].Value;

            if (!TryMarkCombatantSeen(id))
                return;

            var data = Parse105KVPairs(m.Groups["data"].Value);

            string name = data.TryGetValue("Name", out var rawName) ? rawName : "";
            string bnpcId = data.TryGetValue("BNpcID", out var rawBnpcId) ? rawBnpcId : "0";
            string x = data.TryGetValue("PosX", out var rawX) ? FormatF4(rawX) : "0.0000";
            string y = data.TryGetValue("PosY", out var rawY) ? FormatF4(rawY) : "0.0000";
            string z = data.TryGetValue("PosZ", out var rawZ) ? FormatF4(rawZ) : "0.0000";
            string h = data.TryGetValue("Heading", out var rawH) ? FormatF4(rawH) : "0.0000";

            QueueLog(
                $"{m.Groups["time"].Value} _SpawnObject 1105:" +
                $"{id}:{name}:{bnpcId}:{x}:{y}:{z}:{h}:{objectKind}",
                zone);
        }

        private static Dictionary<string, string> Parse105KVPairs(string data)
        {
            string[] parts = data.Split(':');

            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            // in case there are fields containing ':' like "...:Name:XXX:YYY:PosX:0.000:..."
            for (int i = 0; i + 1 < parts.Length; i++)
            {
                result[parts[i]] = parts[i + 1];
            }
            return result;
        }

        private static bool TryMarkCombatantSeen(string id)
        {
            DateTime now = DateTime.UtcNow;

            lock (StateLock)
            {
                if (CombatantSeenUntil.TryGetValue(id, out DateTime until) && until > now)
                {
                    return false;
                }

                CombatantSeenUntil[id] = now.AddSeconds(2);

                // 清理已过期记录，避免长期运行后字典持续增长。
                if (CombatantSeenUntil.Count > 200)
                {
                    List<string> expired = new List<string>();

                    foreach (KeyValuePair<string, DateTime> pair in CombatantSeenUntil)
                    {
                        if (pair.Value <= now)
                            expired.Add(pair.Key);
                    }

                    foreach (string key in expired)
                        CombatantSeenUntil.Remove(key);
                }

                return true;
            }
        }

        #endregion

        #region 10F
        private static readonly Regex ActorSetPosRegex = new Regex(
            @"^(?<time>.{14}) \S+ 10F:(?<id>4.{7}):(?<h>[^:]+):[^:]+:[^:]+:(?<xyz>[^:]+:[^:]+:[^:]+)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static void ProcessActorSetPos(string logLine, string zone)
        {
            Match m = ActorSetPosRegex.Match(logLine);
            if (!m.Success)
                return;

            string id = m.Groups["id"].Value;
            Entity entity = GetEntity(id);

            QueueLog(
                $"{m.Groups["time"].Value} _ActorSetPos 110F:" +
                $"{id}:{FormatEntityIdentity(entity)}:{m.Groups["xyz"].Value}:{m.Groups["h"].Value}",
                zone);
        }
        #endregion

        #region 110
        // 忽略不携带任何有效额外信息的 “E0000000:0000:00”
        private static readonly Regex ActorSpawnExtraRegex = new Regex(
            @"^(?<time>.{14}) \S+ 110:(?<id>4.{7}):(?!E0000000:0000:00)(?<params>.+)$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static void ProcessActorSpawnExtra(string logLine, string zone)
        {
            int generation;

            lock (StateLock)
            {
                generation = _generation;
            }

            Match m = ActorSpawnExtraRegex.Match(logLine);
            if (!m.Success)
                return;

            string time = m.Groups["time"].Value;
            string id = m.Groups["id"].Value;
            string parameters = m.Groups["params"].Value;

            // 原触发器在 GetEntity 前延迟 100 ms，
            // 给新生成的实体留出进入实体表的时间。
            Task.Delay(100).ContinueWith(_ =>
            {
                try
                {
                    if (!IsCurrentGeneration(generation)) return;
                    Entity entity = GetEntity(id);

                    if (!IsCurrentGeneration(generation)) return;
                    QueueLog(
                        $"{time} _ActorSpawnExtra 1110:" +
                        $"{id}:{FormatEntityFull(entity)}:{parameters}",
                        zone);
                }
                catch (Exception ex)
                {
                    RealPlugin.Instance.FilteredAddToLog(
                        RealPlugin.DebugLevelEnum.Error,
                        $"Exception while transcribing ActorSpawnExtra ({logLine}): {ex}");
                }
            });
        }
        #endregion

        #region 111
        private readonly struct ActorControlData
        {
            public readonly string Name;
            public readonly string HexNumber;
            public ActorControlData(string name, string hexNumber)
            {
                Name = name;
                HexNumber = hexNumber;
            }
        }

        private static readonly Regex ActorControlRegex = new Regex(
            @"^(?<time>.{14}) \S+ 111:(?<id>4.{7}):(?<type>[^:]*):(?<a1234>[^:]*:[^:]*:[^:]*:[^:]*)",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly Dictionary<string, ActorControlData> ActorControlCategories =
            new Dictionary<string, ActorControlData>(StringComparer.OrdinalIgnoreCase)
            {
                { "0031", new ActorControlData("Unknown49", "10031") },
                { "003E", new ActorControlData("AnimationState", "1003E") },
                { "003F", new ActorControlData("WeaponId", "1003F") },
                { "0197", new ActorControlData("PlayActionTimeline", "10197") },
                { "019D", new ActorControlData("EObjAnimation", "1019D") },
                { "01F8", new ActorControlData("StatusUpdate", "101F8") },
                { "00B8", new ActorControlData("TargetVfx", "100B8") },
            };

        private static void ProcessActorControl(string logLine, string zone)
        {
            Match m = ActorControlRegex.Match(logLine);
            if (!m.Success)
                return;

            string type = m.Groups["type"].Value;
            if (!ActorControlCategories.TryGetValue(type, out ActorControlData data))
                return;

            string id = m.Groups["id"].Value;

            Entity entity = GetEntity(id);
            bool forceBNpcNameIdZero = entity.BNpcID >= 2000000;

            QueueLog(
                $"{m.Groups["time"].Value} _{data.Name} {data.HexNumber}:" +
                $"{id}:{FormatEntityFull(entity, forceBNpcNameIdZero)}:" +
                $"{m.Groups["a1234"].Value}",
                zone);
        }
        #endregion

        private static Entity GetEntity(string hexId)
        {
            if (!hexId.TryParseHexUInt(out uint id))
            {
                RealPlugin.Instance.FilteredAddToLog(
                    RealPlugin.DebugLevelEnum.Warning,
                    $"转录日志时未找到实体：{hexId}");
                return Entity.NullEntity();
            }
            return Entity.GetEntityByID(id);
        }

        private static string FormatEntityFull(Entity entity, bool forceBNpcNameIdZero = false)
        {
            uint bnpcNameId = forceBNpcNameIdZero ? 0 : entity.BNpcNameID;

            return $"{entity.Name}:{bnpcNameId}:{entity.BNpcID}:" +
                   $"{FormatF4(entity.PosX)}:{FormatF4(entity.PosY)}:{FormatF4(entity.PosZ)}:{FormatF4(entity.Heading)}";
        }

        private static string FormatEntityIdentity(Entity entity, bool forceBNpcNameIdZero = false)
        {
            uint bnpcNameId = forceBNpcNameIdZero ? 0 : entity.BNpcNameID;
            return $"{entity.Name}:{bnpcNameId}:{entity.BNpcID}";
        }

        private static string FormatF4(float value)
        {
            return value.ToString("F4", CultureInfo.InvariantCulture);
        }

        private static string FormatF4(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "0.0000";

            return value.ParseFloat().ToString("F4", CultureInfo.InvariantCulture);
        }

        private static void QueueLog(
            string message,
            string zone,
            bool addToACTEncounter = true)
        {
            RealPlugin plugin = RealPlugin.Instance;

            plugin.LogLineQueuer(
                message,
                zone ?? "",
                LogEvent.SourceEnum.Log);

            if (addToACTEncounter)
                plugin.ACTEncounterLogHook?.Invoke(message);
        }
    }
}