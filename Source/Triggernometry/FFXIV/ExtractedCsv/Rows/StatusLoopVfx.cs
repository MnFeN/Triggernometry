using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class StatusLoopVfx : TypedCsvRow
    {
        public override string Name => $"StatusLoopVfx#{Index}";

        public ushort Vfx1Id => Get<ushort>(1);
        public Vfx Vfx1 => GetRow<Vfx>(Vfx1Id);
        public byte Vfx1Param => Get<byte>(2);

        public ushort Vfx2Id => Get<ushort>(3);
        public Vfx Vfx2 => GetRow<Vfx>(Vfx2Id);
        public byte Vfx2Param => Get<byte>(4);

        public ushort Vfx3Id => Get<ushort>(5);
        public Vfx Vfx3 => GetRow<Vfx>(Vfx3Id);
        public byte Vfx3Param => Get<byte>(6);
    }

}