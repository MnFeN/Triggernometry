using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class Map : TypedCsvRow
    {
        // "x6d3/02"
        public override string Name => Get("Id");
        // 400
        public ushort SizeFactor => Get<ushort>("SizeFactor");
        // -100
        public ushort OffsetX => Get<ushort>("Offset{X}");
        // -100
        public ushort OffsetY => Get<ushort>("Offset{Y}");
        // region: Unlost World
        public ushort RegionPlaceNameId => Get<ushort>("PlaceName{Region}"); // PlaceName
        // place: Alexandria
        public ushort PlaceNameId => Get<ushort>("PlaceName"); // PlaceName
        // sub: Containment Area III
        public ushort SubPlaceNameId => Get<ushort>("PlaceName{Sub}"); // PlaceName
    }

}