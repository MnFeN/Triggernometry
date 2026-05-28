using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Triggernometry.Expressions.String.Models;
using Triggernometry.Expressions.String.Utils;
using Triggernometry.PluginBridges.BridgeNamazu.Modules;

namespace Triggernometry.PluginBridges.BridgeNamazu.Vfx
{
    public enum StaticVfxAction
    {
        Create,
        Modify,
        Remove,
        Triangulate,
        ExaFlare,
    }

    public sealed class StaticVfxArgs
    {
        internal MultiLineRawArgs Raw;

        internal StaticVfxAction Action = StaticVfxAction.Create;
        internal string VfxPath;

        /// <summary>
        /// 参数中显式指定的 Tag。null 表示没有指定。<br />
        /// Create 时使用 GetCreateTag() 获取默认值；Modify/Remove 时 null 表示不过滤 Tag。
        /// </summary>
        internal string Tag;
        public string CreateTag => Tag ?? Vfx.DefaultTag;

        internal string Regex;

        internal double? Delay;
        internal double? Time;

        internal Vector4? Color;

        /// <summary>
        /// 相对坐标，也可以作为两点位姿模式的起点。可以是固定坐标或实体 id。
        /// </summary>
        internal DynamicCoordArg Pos;

        /// <summary>
        /// 两点位姿模式的终点。可以是固定坐标或实体 id。
        /// </summary>
        internal DynamicCoordArg Target;

        /// <summary>
        /// 相对角度 (θ, θx, θy)。
        /// </summary>
        internal Vector3? Angle3D;

        /// <summary>
        /// VFX 自身缩放参数。允许使用 _d 表示 Pos-Target 距离。
        /// </summary>
        internal ScaleArg Scale;

        /// <summary>
        /// 线性变换坐标系中心 O / Center。可以是固定坐标或实体 id。
        /// </summary>
        internal DynamicCoordArg TransformCenter;

        /// <summary>
        /// 线性变换固定旋转角。来自 θ 或 dirX。
        /// </summary>
        internal double? TransformNorthAngle;

        /// <summary>
        /// 线性变换坐标系正北指向的坐标。可以是固定坐标或实体 id。
        /// </summary>
        internal DynamicCoordArg TransformNorthTarget;

        internal bool? TransformKeepX;
        internal bool? TransformKeepY;

        public bool HasPose =>
            Pos != null ||
            Target != null ||
            Angle3D.HasValue;

        public bool HasTransform =>
            TransformCenter != null ||
            TransformNorthAngle.HasValue ||
            TransformNorthTarget != null ||
            TransformKeepX.HasValue ||
            TransformKeepY.HasValue;

        public bool RequiresRefresh =>
            Pos?.IsDynamic == true ||
            Target?.IsDynamic == true ||
            TransformCenter?.IsDynamic == true ||
            TransformNorthTarget?.IsDynamic == true;

        public Func<string, bool> TagFilter()
        {
            if (!string.IsNullOrEmpty(Tag))
            {
                return tag => string.Equals(tag, Tag, StringComparison.OrdinalIgnoreCase);
            }

            if (!string.IsNullOrEmpty(Regex))
            {
                var re = new Regex(Regex, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                return tag => tag != null && re.IsMatch(tag);
            }

            return tag => true;
        }

    }

}