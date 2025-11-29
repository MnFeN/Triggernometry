using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class Status : TypedCsvRow
    {
        public override string Name => Get("Name");
        public string Description => Get("Description");
        public uint IconId => Get<uint>("Icon");
        public byte StatusCategory => Get<byte>("StatusCategory");
        public ushort StatusLoopVfxId => Get<ushort>("VFX"); // StatusLoopVfx
        public bool LockMovement => Get<bool>("LockMovement");
        public bool LockActions => Get<bool>("LockActions");
        public bool LockControl => Get<bool>("LockControl");
        public bool Transfiguration => Get<bool>("Transfiguration");
        public bool IsGaze => Get<bool>("IsGaze");
        public bool InflictedByActor => Get<bool>("InflictedByActor");
        public bool IsPermanent => Get<bool>("IsPermanent");
        public byte Priority => Get<byte>("PartyListPriority");
        public bool CanStatusOff => Get<bool>("CanStatusOff");

    }

}