using System;
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

            if (TryParseEntityId(raw, out var entityId))
                return FromEntityId(entityId);

            return FromCoord(XIVCoord.ParseRawData(raw));
        }

        public DynamicCoordArg Duplicate()
        {
            if (Kind == CoordArgKind.Fixed)
                return FromCoord(FixedCoord.Duplicate());

            return FromEntityId(EntityId);
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