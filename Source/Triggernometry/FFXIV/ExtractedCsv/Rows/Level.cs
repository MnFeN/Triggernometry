using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class LevelEntry : TypedCsvRow
    {
        public override string Name => $"Object#{ObjectId}";

        public float X => Get<float>("X");
        public float Y => Get<float>("Z"); // reversed
        public float Z => Get<float>("Y"); // reversed
        public float Heading => Get<float>("Yaw");
        public float Radius => Get<float>("Radius");
        public byte Type => Get<byte>("Type");
        public byte ObjectId => Get<byte>("Object"); // BNpc/ENpc/EObj
        public ushort MapId => Get<ushort>("Map"); // Map
        public ushort TerritoryId => Get<ushort>("Territory"); // TerritoryType
    }

}