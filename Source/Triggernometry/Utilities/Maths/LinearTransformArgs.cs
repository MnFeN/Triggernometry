using System;
using System.Numerics;

namespace Triggernometry.Utilities.Maths
{
    public sealed class LinearTransformArgs
    {
        /// <summary>
        /// false：对象模式，只允许 KeepX / KeepY 镜像，不允许任意缩放。<br />
        /// true：点集模式，允许 ScaleX / ScaleY / ScaleZ 进行缩放。
        /// </summary>
        public readonly bool IsScalingMode = false;

        private XIVCoord _center = new CartesianCoord(0, 0, 0);
        public XIVCoord Center
        {
            get
            {
                _center = _center ?? new CartesianCoord(0, 0, 0);
                return _center;
            }
            set => _center = value ?? new CartesianCoord(0, 0, 0);
        }

        public double Rotation = Math.PI;

        public bool KeepX = true;
        public bool KeepY = true;

        public double ScaleX = 1.0;
        public double ScaleY = 1.0;
        public double ScaleZ = 1.0;

        public LinearTransformArgs(bool isScalingMode = false)
        {
            IsScalingMode = isScalingMode;
        }

        public LinearTransformArgs Duplicate()
        {
            var duplicated = new LinearTransformArgs(IsScalingMode)
            {
                Center = Center,
                Rotation = Rotation,
            };
            if (IsScalingMode)
            {
                duplicated.ScaleX = ScaleX;
                duplicated.ScaleY = ScaleY;
                duplicated.ScaleZ = ScaleZ;
            }
            else
            {
                duplicated.KeepX = KeepX;
                duplicated.KeepY = KeepY;
            }
            return duplicated;
        }

        public XIVCoord TransformCoord(XIVCoord pos)
        {
            if (pos == null)
                throw new ArgumentNullException(nameof(pos));

            double scaleX, scaleY, scaleZ;
            if (IsScalingMode)
            {
                scaleX = ScaleX;
                scaleY = ScaleY;
                scaleZ = ScaleZ;
            }
            else
            {
                scaleX = KeepX ? 1.0 : -1.0;
                scaleY = KeepY ? 1.0 : -1.0;
                scaleZ = 1.0;
            }

            return pos.Duplicate()
                .ScaleBy(scaleX, scaleY, scaleZ)
                .RotateTo(Rotation)
                .MoveTo(Center);
        }

        public Vector3 TransformAngle3D(Vector3 angle3D)
        {
            if (IsScalingMode)
                throw new InvalidOperationException("TransformAngle3D does not support scaling mode.");

            var theta = angle3D.X;

            if (!KeepX)
                theta *= -1;

            if (!KeepY)
                theta = (float)Math.PI - theta;

            theta += (float)(Rotation - Math.PI);

            return new Vector3(theta, angle3D.Y, angle3D.Z);
        }

        public float TransformAngle(float θ)
        {
            if (IsScalingMode)
                throw new InvalidOperationException("TransformAngle does not support scaling mode.");

            if (!KeepX)
                θ *= -1;

            if (!KeepY)
                θ = (float)Math.PI - θ;

            θ += (float)(Rotation - Math.PI);

            return θ;
        }
    }
}