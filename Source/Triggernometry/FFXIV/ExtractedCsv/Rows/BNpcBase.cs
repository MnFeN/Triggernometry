using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class BNpcBase : TypedCsvRow
    {
        public override string Name => $"BNpc#{Get(0)}";

        // OverlayPlugin: MonsterType, mostly 0/4
        public byte Battalion => Get<byte>("Battalion"); // Battalion
        public byte Rank => Get<byte>("Rank");
        public float Scale => Get<byte>("Scale");
        public ushort ModelCharaId => Get<ushort>("ModelChara"); // ModelChara
    }

}