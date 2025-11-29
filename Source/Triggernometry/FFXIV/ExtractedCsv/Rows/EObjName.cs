using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class EObjName : TypedCsvRow
    {
        public override string Name => Get(1);

    }

}