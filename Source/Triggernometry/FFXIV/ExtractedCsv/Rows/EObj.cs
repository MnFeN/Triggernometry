using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class EObj : TypedCsvRow
    {
        public override string Name => EObjName.Name;
        public EObjName EObjName => GetRow<EObjName>((int)Index);
        public uint Data => Get<uint>("Data");
        public byte Invisibility => Get<byte>("Invisibility");
        public bool Target => Get<bool>("Target");

    }

}