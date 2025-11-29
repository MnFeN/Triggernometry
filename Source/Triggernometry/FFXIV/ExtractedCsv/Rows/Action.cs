using System;
using System.Collections.Generic;

namespace Triggernometry.FFXIV.ExtractedCsv.Rows
{
    public class Action : TypedCsvRow
    {
        public override string Name => Get("Name");
        public int IconId => Get<int>("Icon");

        public int CastVfxId => Get<int>("VFX"); // ActionCastVFX
        public ActionCastVfx CastVfx => GetRow<ActionCastVfx>(CastVfxId);

        public byte ActionStartId => Get<byte>("Animation{Start}"); //ActionCastTimeline
        public ActionCastTimeline ActionCastTimeline => GetRow<ActionCastTimeline>(ActionStartId);
        public ActionTimeline AnimationStart => ActionCastTimeline.ActionTimeline;
        public Vfx AnimationStartVfx => ActionCastTimeline.Vfx;

        public ushort AnimationEndId => Get<ushort>("Animation{End}"); // ActionTimeline
        public ActionTimeline AnimationEnd => GetRow<ActionTimeline>(AnimationEndId);

        public ushort ActionTimelineHitId => Get<ushort>("ActionTimeline{Hit}"); // ActionTimeline
        public ActionTimeline ActionTimelineHit => GetRow<ActionTimeline>(ActionTimelineHitId);

        public sbyte Range => Get<sbyte>("Range");
        public byte ShapeType => Get<byte>("CastType");
        public ShapeEnum Shape => ShapeMap[ShapeType];

        /// <summary> 技能范围，即圆/扇形技能的半径、矩形技能的半长，相当于特效的 y 参数。</summary>
        public byte ScaleY => Get<byte>("EffectRange");
        /// <summary> 非圆/扇形对称性的技能的宽度，相当于特效 x 参数的两倍。 </summary>
        public byte Scale2X => Get<byte>("XAxisModifier");
        public float ScaleX => Scale2X / 2f;

        public float CastTime => Get<ushort>("Cast<100ms>") / 10f;

        public ushort OmenId => Get<ushort>("Omen"); // Omen
        public Omen Omen => GetRow<Omen>(OmenId);

        public enum ShapeEnum : byte
        {
            None,
            Circle,
            Fan,
            Rect,
            RectTo, // y = 0
            RectThrough, // y = 0, 截至 7.3 只有 PVP 技能 必杀剑·早天
            Ring,
            Cross,
            Triangle,
        }

        public static Dictionary<byte, ShapeEnum> ShapeMap = new Dictionary<byte, ShapeEnum>()
        {
            [0] = ShapeEnum.None,
            [1] = ShapeEnum.None,
            [2] = ShapeEnum.Circle,
            [3] = ShapeEnum.Fan,
            [4] = ShapeEnum.Rect,
            [5] = ShapeEnum.Circle,
            [6] = ShapeEnum.Circle,
            [7] = ShapeEnum.Circle,
            [8] = ShapeEnum.RectTo,
            [9] = ShapeEnum.None, // never used
            [10] = ShapeEnum.Ring,
            [11] = ShapeEnum.Cross,
            [12] = ShapeEnum.Rect,
            [13] = ShapeEnum.Fan,
            [14] = ShapeEnum.Triangle,
            [15] = ShapeEnum.RectThrough,
        };
    }

}