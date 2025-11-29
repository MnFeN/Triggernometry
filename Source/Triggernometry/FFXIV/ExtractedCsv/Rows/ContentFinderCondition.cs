using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class ContentFinderCondition : TypedCsvRow
    {

        // "AAC Cruiserweight M1"
        public override string Name => Get("Name");
        public byte ContentTypeId => Get<byte>("ContentType"); // ContentType
        // Raids
        public ContentType ContentType => GetRow<ContentType>(ContentTypeId);

        // many other boolean fields
    }

}