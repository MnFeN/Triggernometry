using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class PlaceName : TypedCsvRow
    {
        public override string Name => Get("Name");
    }

}