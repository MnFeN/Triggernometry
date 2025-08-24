using System;
using static System.Math;

namespace Triggernometry.Utilities.Math
{
    public class CartesianCoord : XIVCoord
    {
        public double X;
        public double Y;
        public double Z;

        public string X_3 => X.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
        public string Y_3 => Y.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
        public string Z_3 => Z.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);

        public CartesianCoord(double x, double y, double z)
        {
            X = x; Y = y; Z = z;
        }

        public override XIVCoord Duplicate() => new CartesianCoord(X, Y, Z);

        public override XIVCoord RotateTo(double θ)
        {
            var sin = Sin(θ);
            var cos = Cos(θ);
            (X, Y) = (-X * cos - Y * sin, X * sin - Y * cos);
            return this;
        }

        public override XIVCoord MoveTo(double dx, double dy, double dz)
        {
            X += dx;
            Y += dy;
            Z += dz;
            return this;
        }

        public override XIVCoord ScaleBy(double scaleX, double scaleY, double scaleZ)
        {
            X *= scaleX;
            Y *= scaleY;
            Z *= scaleZ;
            return this;
        }

        public override CartesianCoord ToCartesian() => new CartesianCoord(X, Y, Z);

        public override PolarCoord ToPolar()
        {
            double r = Sqrt(X * X + Y * Y);
            double θ = Atan2(X, Y);
            return new PolarCoord(r, θ, Z);
        }

        public override double Length => Sqrt(X * X + Y * Y + Z * Z);

        public override string ToString() => $"({X_3}, {Y_3}, {Z_3})";

        public override string Jsonify() => $"\"X\": {X_3}, \"Z\": {Y_3}, \"Y\": {Z_3}, \"Active\": true";

        public static CartesianCoord Parse(params string[] coords)
        {
            switch (coords.Length)
            {
                case 2:
                    return ParseCoordsString(coords[0], coords[1]);
                case 3:
                    return ParseCoordsString(coords[0], coords[1], coords[2]);
                default:
                    throw Context.ArgCountError("CartesianCoord: 坐标构造函数", "2-3", coords.Length, "[" + string.Join("], [", coords) + "]");
            }
        }

        private static CartesianCoord ParseCoordsString(string rawX, string rawY, string rawZ = null)
        {
            try
            {
                return new CartesianCoord(
                    MathParser.Parse(rawX),
                    MathParser.Parse(rawY),
                    rawZ == null ? 0 : MathParser.Parse(rawZ));
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"直角坐标解析错误：{ex.Message}\n" +
                    $"原始数据：\nx = ({rawX}), \ny = ({rawY}), \nz = ({rawZ ?? "null"})");
            }
        }
    }

}
