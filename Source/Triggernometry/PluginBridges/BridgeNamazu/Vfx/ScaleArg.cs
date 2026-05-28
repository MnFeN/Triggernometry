using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Triggernometry.Expressions.String.Utils;

namespace Triggernometry.PluginBridges.BridgeNamazu.Vfx
{
    internal sealed class ScaleArg
    {
        private readonly ScaleTerm X;
        private readonly ScaleTerm Y;
        private readonly ScaleTerm Z;

        public bool HasDistanceToken => X.HasDistanceToken || Y.HasDistanceToken || Z.HasDistanceToken;

        private ScaleArg(ScaleTerm x, ScaleTerm y, ScaleTerm z)
        {
            X = x ?? throw new ArgumentNullException(nameof(x));
            Y = y ?? throw new ArgumentNullException(nameof(y));
            Z = z ?? throw new ArgumentNullException(nameof(z));
        }

        public static ScaleArg FromVector3(Vector3 scale)
        {
            return new ScaleArg(
                new ScaleTerm(scale.X),
                new ScaleTerm(scale.Y),
                new ScaleTerm(scale.Z));
        }

        public static ScaleArg Parse(string raw)
        {
            var (x, y, z) = raw.ParseArgs<string, string, string>(
                (0, "1"),
                (1, null),
                (2, null));

            return new ScaleArg(
                ScaleTerm.Parse(x),
                ScaleTerm.Parse(y ?? x),
                ScaleTerm.Parse(z ?? "1"));
        }

        public static ScaleArg ParseCyl(string raw)
        {
            var (x, z) = raw.ParseArgs<string, string>(
                (0, "1"),
                (1, null));

            return new ScaleArg(
                ScaleTerm.Parse(x),
                ScaleTerm.Parse(x),
                ScaleTerm.Parse(z ?? x));
        }

        public Vector3 Resolve(double distance)
        {
            return new Vector3(
                (float)X.Resolve(distance),
                (float)Y.Resolve(distance),
                (float)Z.Resolve(distance));
        }


        private sealed class ScaleTerm
        {
            private static readonly Regex DistanceTokenRegex =
                new Regex(@"\b_d\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

            private readonly bool _hasDistanceToken;
            private readonly double _value;
            private readonly string _expression;
            private readonly Func<double, string> _replaceDistance;

            public bool HasDistanceToken => _hasDistanceToken;

            internal ScaleTerm(double value)
            {
                _hasDistanceToken = false;
                _value = value;
            }

            private ScaleTerm(string expression)
            {
                if (string.IsNullOrWhiteSpace(expression))
                    throw new ArgumentException("Dynamic scale expression cannot be empty.", nameof(expression));

                _hasDistanceToken = true;
                _expression = expression.Trim();
                _replaceDistance = distance => DistanceTokenRegex.Replace(_expression, distance.ToDataString());
            }

            public static ScaleTerm Parse(string raw)
            {
                if (string.IsNullOrWhiteSpace(raw))
                    return new ScaleTerm(1.0);

                raw = raw.Trim();

                if (!DistanceTokenRegex.IsMatch(raw))
                    return new ScaleTerm(raw.ParseData<double>());

                return new ScaleTerm(raw);
            }

            public double Resolve(double distance) => _hasDistanceToken
                ? _replaceDistance(distance).ParseData<double>()
                : _value;
        }
    }

}
