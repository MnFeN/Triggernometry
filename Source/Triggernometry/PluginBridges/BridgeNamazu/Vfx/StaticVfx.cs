using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using Triggernometry.FFXIV.ExtractedCsv.Rows;
using Triggernometry.Utilities.Maths;

namespace Triggernometry.PluginBridges.BridgeNamazu.Vfx
{
    public class StaticVfx : Vfx
    {
        /// <summary> 脏状态标记，Modify 后等待 VFX 循环统一刷新并 Update。 </summary>
        internal bool PendingUpdate;

        /// <summary> 当前设置的相对坐标参数，可以是固定坐标或实体 id </summary>
        internal DynamicCoordArg PosArg;

        /// <summary> 当前设置的两点位姿终点参数，可以是固定坐标或实体 id </summary>
        internal DynamicCoordArg TargetArg;

        /// <summary> 当前设置的相对朝向参数；如果 TargetArg 非空，则优先使用两点位姿模式 </summary>
        internal Vector3? Angle3DArg;

        /// <summary> 当前设置的 VFX 自身缩放参数；允许使用 _d 表示 Pos-Target 距离 </summary>
        internal ScaleArg ScaleArg;

        /// <summary> 当前设置的线性变换坐标系中心 O / Center，可以是固定坐标或实体 id </summary>
        internal DynamicCoordArg TransformCenterArg;

        /// <summary> 当前设置的线性变换固定旋转角，来自 θ 或 dirX </summary>
        internal double? TransformNorthAngle;

        /// <summary> 当前设置的线性变换坐标系正北指向的坐标，可以是固定坐标或实体 id </summary>
        internal DynamicCoordArg TransformNorthCoordArg;

        /// <summary> 当前设置的 X 是否不翻转 </summary>
        internal bool? TransformKeepX;

        /// <summary> 当前设置的 Y 是否不翻转 </summary>
        internal bool? TransformKeepY;

        internal bool HasTransformArgs =>
            TransformCenterArg != null ||
            TransformNorthAngle.HasValue ||
            TransformNorthCoordArg != null ||
            TransformKeepX.HasValue ||
            TransformKeepY.HasValue;

        public bool RequiresRefresh =>
            PosArg?.IsDynamic == true ||
            TargetArg?.IsDynamic == true ||
            TransformCenterArg?.IsDynamic == true ||
            TransformNorthCoordArg?.IsDynamic == true;

        /// <summary> 注意 lock </summary>
        public static IReadOnlyDictionary<IntPtr, StaticVfx> Storage
            => VfxManager.StaticVfxs;

        public override bool TryRemove()
            => VfxManager.Remove(this);

        /// <summary>
        /// 将本次解析到的参数合并到当前 VFX 的参数源状态上。Create 时会补齐默认位姿；Modify 时全部增量式覆盖。
        /// </summary>
        public void ApplyArgs(StaticVfxArgs newArgs, bool isCreate)
        {
            if (newArgs == null)
                throw new ArgumentNullException(nameof(newArgs));

            if (isCreate)
            {
                // 如果是创建，并且本次涉及位姿或线性变换，或 Scale 涉及距离，则初始化默认位姿，供后续刷新和修改使用
                if (newArgs.HasPose || newArgs.HasTransform || newArgs.Scale?.HasDistanceToken == true)
                {
                    PosArg = newArgs.Pos?.Duplicate() ?? DynamicCoordArg.FromCoord(new CartesianCoord(0, 0, 0));
                    TargetArg = newArgs.Target?.Duplicate();

                    // Target 存在时使用两点位姿模式；否则使用显式 Angle3D 或默认朝向
                    Angle3DArg = newArgs.Target == null
                        ? (newArgs.Angle3D ?? new Vector3((float)Math.PI, 0, 0))
                        : (Vector3?)null;
                }
            }
            else
            {
                // 若指定了 Pos，则写入当前参数源
                if (newArgs.Pos != null)
                    PosArg = newArgs.Pos.Duplicate();

                // 若指定了 Target，则启用两点位姿模式；若此前没有 Pos，则使用原点作为默认起点。
                if (newArgs.Target != null)
                {
                    if (PosArg == null)
                        PosArg = DynamicCoordArg.FromCoord(new CartesianCoord(0, 0, 0));

                    TargetArg = newArgs.Target.Duplicate();
                    Angle3DArg = null;
                }

                // 若指定了 Angle3D，则启用单点位姿模式
                if (newArgs.Angle3D.HasValue)
                {
                    Angle3DArg = newArgs.Angle3D.Value;
                    TargetArg = null;
                }
            }

            // 更新 VFX 自身缩放参数。未指定时保持原值不变。
            if (newArgs.Scale != null)
            {
                ScaleArg = newArgs.Scale;
            }

            // 更新颜色，非动态参数，直接写入 VFX 属性。
            if (newArgs.Color.HasValue)
                Color = newArgs.Color.Value;

            // 更新线性变换参数源。未指定的字段保持原值不变。
            if (newArgs.TransformCenter != null)
                TransformCenterArg = newArgs.TransformCenter.Duplicate();

            if (newArgs.TransformNorthAngle.HasValue)
            {
                var angle = newArgs.TransformNorthAngle.Value;

                // 哨兵值，清空固定角度，方向与 O 指定的实体同步
                if (double.IsNaN(angle))
                {
                    TransformNorthAngle = null;
                    TransformNorthCoordArg = null;
                }
                else
                {
                    TransformNorthAngle = angle;
                    TransformNorthCoordArg = null;
                }
            }
            else if (newArgs.TransformNorthTarget != null)
            {
                TransformNorthCoordArg = newArgs.TransformNorthTarget.Duplicate();
                TransformNorthAngle = null;
            }

            if (newArgs.TransformKeepX.HasValue)
                TransformKeepX = newArgs.TransformKeepX.Value;

            if (newArgs.TransformKeepY.HasValue)
                TransformKeepY = newArgs.TransformKeepY.Value;

            // validate
            if (ScaleArg?.HasDistanceToken == true && TargetArg == null)
                throw new ArgumentException("[PictoACT] Scale 使用 _d 时，当前 VFX 必须已有 Target，或本次指定 Target。");
        }
    }
}