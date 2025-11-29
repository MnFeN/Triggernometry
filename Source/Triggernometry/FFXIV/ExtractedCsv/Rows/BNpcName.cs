using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class BNpcName : TypedCsvRow
    {
        public override string Name => Get(1);
    }

}