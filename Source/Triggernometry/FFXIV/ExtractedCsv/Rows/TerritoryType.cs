using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class TerritoryType : TypedCsvRow
    {
        // "x6d3"
        public override string Name => Get("Name");

        // region: The Northern Empty
        public ushort RegionPlaceNameId => Get<ushort>("PlaceName{Region}"); // PlaceName
        public PlaceName RegionPlaceName => GetRow<PlaceName>(RegionPlaceNameId);

        // Zone: Labyrinthos
        public ushort ZonePlaceNameId => Get<ushort>("PlaceName{Zone}"); // PlaceName
        public PlaceName ZonePlaceName => GetRow<PlaceName>(ZonePlaceNameId);

        // place: The Dæmons' Nest
        public ushort PlaceNameId => Get<ushort>("PlaceName"); // PlaceName
        public PlaceName PlaceName => GetRow<PlaceName>(PlaceNameId);

        // map: n5ra/00
        public ushort MapId => Get<ushort>("Map"); // Map
        public Map Map => GetRow<Map>(MapId);

        // content: Anabaseios: The Tenth Circle
        public ushort ContentFinderConditionId => Get<ushort>("ContentFinderCondition"); // ContentFinderCondition
        public ContentFinderCondition ContentFinderCondition => GetRow<ContentFinderCondition>(ContentFinderConditionId);
    }

}