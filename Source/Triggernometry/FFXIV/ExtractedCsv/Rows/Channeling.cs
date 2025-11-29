using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class Channeling : TypedCsvRow
    {

        /// <summary> VFX name (path), e.g. "dk01rf_atk0h" </summary>
        public override string Name => Get("File");
        public byte WidthScale => Get<byte>("WidthScale");
    }

}