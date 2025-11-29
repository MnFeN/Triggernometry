using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class ActionCastTimeline : TypedCsvRow
    {
        public override string Name => ActionTimeline.Name;
        public ushort ActionTimelineId => Get<ushort>(1); // ActionTimeline
        public ActionTimeline ActionTimeline => GetRow<ActionTimeline>(ActionTimelineId);
        public ushort VfxId => Get<ushort>("VFX"); // Vfx
        public Vfx Vfx => GetRow<Vfx>(VfxId);
    }

}