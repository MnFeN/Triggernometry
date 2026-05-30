using System;
using System.Collections.Generic;
using System.Numerics;
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
        /// 将本次解析到的参数合并到当前 VFX 的参数源状态上。Create 时会补齐默认位姿；Modify 时仅覆盖显式指定的参数。
        /// </summary>
        public void ApplyArgs(StaticVfxArgs newArgs, bool isCreate)
        {
            if (newArgs == null)
                throw new ArgumentNullException(nameof(newArgs));

            ApplyPosArgs(newArgs, isCreate);
            ApplyAngleOrTargetArgs(newArgs, isCreate);
            ApplyScaleArgs(newArgs);
            ApplyColorArgs(newArgs);
            ApplyTransformArgs(newArgs);
            Validate();
        }

        private static DynamicCoordArg DefaultPos() 
            => DynamicCoordArg.FromCoord(new CartesianCoord(0, 0, 0));
        private static Vector3 DefaultAngle() 
            => new Vector3((float)Math.PI, 0, 0);

        private void ApplyPosArgs(StaticVfxArgs newArgs, bool isCreate)
        {
            // 若指定了 Pos，则写入当前参数源
            if (newArgs.Pos != null)
                PosArg = newArgs.Pos.Duplicate();

            // 若未指定，且为 Create，则使用默认值
            else if (isCreate)
                PosArg = DefaultPos();
        }

        private void ApplyAngleOrTargetArgs(StaticVfxArgs newArgs, bool isCreate)
        {
            // 若指定了 Target，则切换至两点位姿模式
            if (newArgs.Target != null)
                SwitchToPosTargetMode(newArgs.Target);

            // 若指定了 Angle3D，则切换至单点位姿模式
            else if (newArgs.Angle3D.HasValue)
                SwitchToPosAngleMode(newArgs.Angle3D.Value);

            // 若均未指定，且为 Create，则使用默认值
            else if (isCreate)
                SwitchToPosAngleMode(DefaultAngle());
        }

        private void SwitchToPosTargetMode(DynamicCoordArg target)
        {
            TargetArg = target.Duplicate();
            Angle3DArg = null;
        }

        private void SwitchToPosAngleMode(Vector3 angle)
        {
            Angle3DArg = angle;
            TargetArg = null;
        }

        private void ApplyScaleArgs(StaticVfxArgs newArgs)
        {
            if (newArgs.Scale != null)
            {
                ScaleArg = newArgs.Scale.Duplicate();
            }
        }

        private void ApplyColorArgs(StaticVfxArgs newArgs)
        {
            if (newArgs.Color.HasValue)
            {
                Color = newArgs.Color.Value;
            }
        }

        private void ApplyTransformArgs(StaticVfxArgs newArgs)
        {
            // 平移变换：坐标系中心
            if (newArgs.TransformCenter != null)
                TransformCenterArg = newArgs.TransformCenter.Duplicate();

            // 旋转变换：坐标系正北
            if (newArgs.TransformNorthAngle.HasValue)
            {
                var angle = newArgs.TransformNorthAngle.Value;

                // 哨兵值，清空固定角度，方向与 O 指定的实体同步
                if (double.IsNaN(angle))
                {
                    TransformNorthAngle = null;
                    TransformNorthCoordArg = null;
                }
                // 指定角度
                else
                {
                    TransformNorthAngle = angle;
                    TransformNorthCoordArg = null;
                }
            }
            // 指定正北的坐标或实体
            else if (newArgs.TransformNorthTarget != null)
            {
                TransformNorthCoordArg = newArgs.TransformNorthTarget.Duplicate();
                TransformNorthAngle = null;
            }

            // 伸缩变换：XY 翻转选项
            if (newArgs.TransformKeepX.HasValue)
                TransformKeepX = newArgs.TransformKeepX;

            if (newArgs.TransformKeepY.HasValue)
                TransformKeepY = newArgs.TransformKeepY;
        }

        private void Validate()
        {
            // 当前状态校验：_d 依赖两点距离，因此最终状态中必须有 Target。
            if (ScaleArg?.HasDistanceToken == true && TargetArg == null)
                throw new ArgumentException("[PictoACT] Scale 使用 _d 时，当前 VFX 必须已有 Target，或本次指定 Target。");
        }
    
    }
}