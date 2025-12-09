using System;
using Triggernometry.Expressions.Maths;
using Triggernometry.Expressions.String.Utils;
using static System.Math;

namespace Triggernometry.Utilities.Maths
{
   
    public class PolarCoord : XIVCoord
    {
        public double R;
        public double θ;
        public double Z;

        public PolarCoord(double r, double θ, double z)
        {
            R = r; this.θ = θ; Z = z;
        }

        public override XIVCoord Duplicate() => new PolarCoord(R, θ, Z);

        public override XIVCoord RotateTo(double θ)
        {
            this.θ += θ + PI;
            return this;
        }

        public override XIVCoord MoveTo(double dx, double dy, double dz)
            => ToCartesian().MoveTo(dx, dy, dz);

        public override XIVCoord ScaleBy(double scaleX, double scaleY, double scaleZ)
        {
            if (Abs(scaleX - scaleY) < 1e-5 && scaleX >= 1e-4)
            {
                R *= scaleX;
                Z *= scaleZ;
                return this;
            }
            else return ToCartesian().ScaleBy(scaleX, scaleY, scaleZ);
        }

        public override CartesianCoord ToCartesian()
        {
            double x = R * Sin(θ);
            double y = R * Cos(θ);
            return new CartesianCoord(x, y, Z);
        }

        public override PolarCoord ToPolar() => new PolarCoord(R, θ, Z);

        public override double Length => Sqrt(R * R + Z * Z);

        public override string ToString() => $"(R={R}, θ={θ}, Z={Z})";

        public override string Jsonify() => ToCartesian().Jsonify();

        public static PolarCoord Parse(params string[] coords)
        {
            switch (coords.Length)
            {
                case 2:
                    return ParsePolarCoordsString(coords[0], coords[1]);
                case 3:
                    return ParsePolarCoordsString(coords[0], coords[1], coords[2]);
                default:
                    throw ErrorHelper.ArgumentCountError("极坐标构造函数", "2-3", coords.Length, "[" + string.Join("], [", coords) + "]");
            }
        }

        private static PolarCoord ParsePolarCoordsString(string rawR, string rawθ, string rawZ = null)
        {
            try
            {
                return new PolarCoord(
                    MathParser.Parse(rawR),
                    MathParser.Parse(rawθ),
                    rawZ == null ? 0 : MathParser.Parse(rawZ));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"极坐标解析错误：{ex.Message}\n\n" +
                    $"原始数据：\nr = ({rawR}), \nθ = ({rawθ}), \nz = ({rawZ})");
            }
        }

    }
}
