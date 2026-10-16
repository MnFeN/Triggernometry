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
    }

    internal sealed class DynamicCoordArg
    {
        public CoordArgKind Kind { get; private set; }

        public XIVCoord FixedCoord { get; private set; }

        public uint EntityId { get; private set; }
        public double? OverrideX { get; private set; }
        public double? OverrideY { get; private set; }
        public double? OverrideZ { get; private set; }

        public bool IsFixed => Kind == CoordArgKind.Fixed;
        public bool IsDynamic => Kind == CoordArgKind.EntityId;

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

        public static DynamicCoordArg Parse(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return null;

            raw = raw.Trim();

            // 纯实体 ID
            if (TryParseEntityId(raw, out var entityId))
                return FromEntityId(entityId);

            // 实体 ID + 指定坐标分量，如 10001234, y=10, z = 0
            var parts = Regex.Split(raw, @",\s*(?=[xyz]\s*=)", RegexOptions.IgnoreCase);
            if (parts.Length > 1 && TryParseEntityId(parts[0], out entityId))
            {
                var result = FromEntityId(entityId);

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

            // 普通坐标
            return FromCoord(XIVCoord.ParseRawData(raw));
        }

        public DynamicCoordArg Duplicate()
        {
            if (Kind == CoordArgKind.Fixed)
                return FromCoord(FixedCoord.Duplicate());

            return new DynamicCoordArg
            {
                Kind = Kind,
                EntityId = EntityId,
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