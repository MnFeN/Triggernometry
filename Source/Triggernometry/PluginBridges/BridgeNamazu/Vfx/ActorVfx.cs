using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Triggernometry.PluginBridges.BridgeNamazu.Modules;
using Triggernometry.Utilities;

namespace Triggernometry.PluginBridges.BridgeNamazu.Vfx
{
    public class ActorVfx : Vfx
    {
        /// <summary> 注意 lock </summary>
        public static IReadOnlyDictionary<IntPtr, ActorVfx> Storage => VfxModule.ActorVfxs;

        public static ActorVfx Create(IntPtr srcAddress, IntPtr tgtAddress, string fullPath, string tag = null)
            => Module.ActorVfxCreate(srcAddress, tgtAddress, fullPath, tag);

        public override bool TryRemove()
            => Module.TryActorVfxRemove(Ptr);

    }
}
