using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Triggernometry.PluginBridges.BridgeNamazu.Modules;
using Triggernometry.Utilities.Maths;

namespace Triggernometry.PluginBridges.BridgeNamazu.Vfx
{
    public class StaticVfx : Vfx
    {

        /// <summary> 上一次设置的坐标系中心 </summary>
        internal XIVCoord PrevCenter;
        /// <summary> 上一次设置的坐标系角度 </summary>
        internal double PrevRotation;
        /// <summary> 上一次设置的 X 是否不翻转 </summary>
        internal bool PrevKeepX;
        /// <summary> 上一次设置的 Y 是否不翻转 </summary>
        internal bool PrevKeepY;
        /// <summary> 上一次设置的相对坐标 </summary>
        internal XIVCoord PrevPos;
        /// <summary> 上一次设置的相对朝向 </summary>
        internal Vector3 PrevAngles; // 似乎应该是 V3? 而不是 V3
        /// <summary> 注意 lock </summary>
        public static IReadOnlyDictionary<IntPtr, StaticVfx> Storage 
            => VfxManager.StaticVfxs;

        public override bool TryRemove()
            => VfxManager.Remove(this);

    }
}
