using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class Omen : TypedCsvRow
    {
        /// <summary> VFX name (path), e.g. "dk01rf_atk0h" </summary>
        public override string Name => Get("Path");
        public byte Type => Get<byte>("Type");
        public bool RestrictYScale => Get<bool>("RestrictYScale");
        public bool LargeScale => Get<bool>("LargeScale");
    }

}