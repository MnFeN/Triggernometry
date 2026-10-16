using System;
using System.Text.RegularExpressions;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.Utilities.Maths;

namespace Triggernometry.PluginBridges.BridgeNamazu.Vfx
{
    internal enum CoordArgKind
    {
        Fixed,
        EntityId,
        CoordinateSelector,
    }

    internal enum EntitySelectMode
    {
        Direct,
        Target,
        Near,
        Far,
    }

    internal enum EntitySelectType
    {
        Player,
        Tank,
        Healer,
        NonTank,
        DPS,
        Enemy,
    }

    internal sealed class DynamicCoordArg
    {
        // 只识别位于整个表达式末尾的选择器，避免将坐标中的小数点当作分隔符。
        private static readonly Regex EntitySelectorRegex = new Regex(
            @"\.\s*(?:(?<target>Target)|(?<order>Near|Far)\s*(?<type>NonTank|Player|Tank|Healer|DPS|Enemy)\s*(?<index>\d+))$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        public CoordArgKind Kind { get; private set; }

        // Fixed 时表示固定位置；CoordinateSelector 时表示用于距离排序的绝对参考坐标。
        public XIVCoord FixedCoord { get; private set; }

        // EntityId 始终保存参考实体 ID，不会替换成最终选中的实体 ID。
        public uint EntityId { get; private set; }
        public EntitySelectMode SelectMode { get; private set; } = EntitySelectMode.Direct;
        public EntitySelectType? SelectType { get; private set; }
        public int SelectIndex { get; private set; } = 1;

        public double? OverrideX { get; private set; }
        public double? OverrideY { get; private set; }
        public double? OverrideZ { get; private set; }

        // 每个参数独立使用的运行时缓存，Duplicate() 不复制这些字段。
        internal uint? CachedSelectedId;
        internal DateTime NextSelectUtc;

        public bool IsFixed => Kind == CoordArgKind.Fixed;
        public bool IsDynamic => Kind != CoordArgKind.Fixed;

        private DynamicCoordArg()
        {
        }

        public static DynamicCoordArg FromCoord(XIVCoord coord)
        {
            if (coord == null)
                throw new ArgumentNullException(nameof(coord));

            return new DynamicCoordArg
            {
                Kind = CoordArgKind.Fixed,
                FixedCoord = coord,
            };
        }

        public static DynamicCoordArg FromEntityId(uint entityId)
        {
            return new DynamicCoordArg
            {
                Kind = CoordArgKind.EntityId,
                EntityId = entityId,
            };
        }

        private static DynamicCoordArg FromCoordinateSelector(XIVCoord coord)
        {
            if (coord == null)
                throw new ArgumentNullException(nameof(coord));

            return new DynamicCoordArg
            {
                Kind = CoordArgKind.CoordinateSelector,
                FixedCoord = coord,
            };
        }

        public static DynamicCoordArg Parse(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            raw = raw.Trim();

            // 仅在坐标轴覆盖参数之前分割，其余逗号仍交给坐标解析器处理。
            var parts = Regex.Split(raw, @",\s*(?=[xyz]\s*=)", RegexOptions.IgnoreCase);
            var source = parts[0].Trim();
            DynamicCoordArg result;

            // 先从末尾提取选择器，再分别判断参考对象是实体 ID 还是固定坐标。
            var match = EntitySelectorRegex.Match(source);
            if (match.Success)
            {
                var reference = source.Substring(0, match.Index).Trim();

                if (TryParseEntityId(reference, out var referenceId))
                {
                    result = FromEntityId(referenceId);
                }
                else
                {
                    if (match.Groups["target"].Success)
                        throw new ArgumentException($"[PictoACT] Target 选择器必须以实体 ID 为参考：{source}");

                    result = FromCoordinateSelector(XIVCoord.ParseRawData(reference));
                }

                if (match.Groups["target"].Success)
                {
                    result.SelectMode = EntitySelectMode.Target;
                }
                else
                {
                    result.SelectMode = match.Groups["order"].Value.Equals("Near", StringComparison.OrdinalIgnoreCase)
                        ? EntitySelectMode.Near
                        : EntitySelectMode.Far;
                    result.SelectType = (EntitySelectType)Enum.Parse(typeof(EntitySelectType),
                        match.Groups["type"].Value, true);

                    if (!int.TryParse(match.Groups["index"].Value, out var index) || index <= 0)
                        throw new ArgumentException($"[PictoACT] 实体距离排名必须是正整数：{source}");

                    result.SelectIndex = index;
                }
            }
            else if (TryParseEntityId(source, out var entityId))
            {
                result = FromEntityId(entityId);
            }
            else
            {
                // 没有选择器时，保持普通固定坐标的原有解析行为。
                return FromCoord(XIVCoord.ParseRawData(raw));
            }

            // 坐标覆盖应用于最终选中实体的位置，不改变距离排序的参考点。
            for (int i = 1; i < parts.Length; i++)
            {
                int eq = parts[i].IndexOf('=');
                string axis = parts[i].Substring(0, eq).Trim().ToLowerInvariant();
                double value = parts[i].Substring(eq + 1).Trim().ParseData<double>();

                switch (axis)
                {
                    case "x": result.OverrideX = value; break;
                    case "y": result.OverrideY = value; break;
                    case "z": result.OverrideZ = value; break;
                }
            }

            return result;
        }

        public DynamicCoordArg Duplicate()
        {
            if (Kind == CoordArgKind.Fixed)
                return FromCoord(FixedCoord.Duplicate());

            return new DynamicCoordArg
            {
                Kind = Kind,
                FixedCoord = FixedCoord?.Duplicate(),
                EntityId = EntityId,
                SelectMode = SelectMode,
                SelectType = SelectType,
                SelectIndex = SelectIndex,
                OverrideX = OverrideX,
                OverrideY = OverrideY,
                OverrideZ = OverrideZ,
            };
        }

        internal static bool TryParseEntityId(string raw, out uint entityId)
        {
            entityId = 0;

            if (string.IsNullOrWhiteSpace(raw))
                return false;

            raw = raw.Trim();

            if (raw.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                raw = raw.Substring(2);

            if (!raw.TryParseHexUInt(out entityId))
                return false;

            return
                (entityId >= 0x10000000 && entityId <= 0x10FFFFFF) ||
                (entityId >= 0x40000000 && entityId <= 0x40FFFFFF);
        }
    }
}