using Triggernometry.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Triggernometry.Expressions.Maths;
using static System.Math;

namespace Triggernometry.Utilities.Maths
{
    public abstract class XIVCoord
    {
        public abstract XIVCoord Duplicate();

        /// <summary>
        /// 将初始坐标视为相对坐标。<br/>
        /// 将相对坐标系的正北（θ = ±pi）在平面内旋转至给定方向 <paramref str="θ"/>。<br/><br/>
        /// 方向角度 <paramref str="θ"/> 为游戏内标准，如：<br/>
        /// · 正北（不旋转）= ±pi；<br/>
        /// · 正南（旋转 180 度）= 0；<br/>
        /// · 正东（顺时针旋转 90 度）= pi/2。
        /// </summary>
        /// <param str="theta">将初始相对坐标系的正北（-pi）旋转到的方向角度。</param>
        public abstract XIVCoord RotateTo(double θ);
        public abstract XIVCoord MoveTo(double dx, double dy, double dz);
        public XIVCoord MoveTo(XIVCoord center) => this + center;
        public abstract XIVCoord ScaleBy(double scaleX, double scaleY, double scaleZ);
        public abstract CartesianCoord ToCartesian();
        public abstract PolarCoord ToPolar();
        public abstract double Length { get; }
        public abstract string Jsonify();
        public abstract override string ToString();

        public static CartesianCoord operator +(XIVCoord a, XIVCoord b)
        {
            CartesianCoord cartesianA = a.ToCartesian();
            CartesianCoord cartesianB = b.ToCartesian();

            return new CartesianCoord(
                cartesianA.X + cartesianB.X,
                cartesianA.Y + cartesianB.Y,
                cartesianA.Z + cartesianB.Z);
        }

        public static CartesianCoord operator -(XIVCoord a, XIVCoord b)
        {
            CartesianCoord cartesianA = a.ToCartesian();
            CartesianCoord cartesianB = b.ToCartesian();

            return new CartesianCoord(
                cartesianA.X - cartesianB.X,
                cartesianA.Y - cartesianB.Y,
                cartesianA.Z - cartesianB.Z);
        }

        public static XIVCoord operator -(XIVCoord a)
        {
            if (a is CartesianCoord cartesianA)
            {
                return new CartesianCoord(-cartesianA.X, -cartesianA.Y, -cartesianA.Z);
            }
            else
            {
                PolarCoord polarA = (PolarCoord)a;
                return new PolarCoord(polarA.R, polarA.θ + PI, -polarA.Z);
            }
        }

        public static XIVCoord operator *(XIVCoord a, double n)
        {
            if (a is CartesianCoord cartesianA)
            {
                return new CartesianCoord(cartesianA.X * n, cartesianA.Y * n, cartesianA.Z * n);
            }
            else
            {
                PolarCoord polarA = (PolarCoord)a;
                return new PolarCoord(polarA.R * n, polarA.θ, polarA.Z * n);
            }
        }

        public static XIVCoord operator *(double n, XIVCoord a) => a * n;

        public static XIVCoord operator /(XIVCoord a, double n) => a * (1.0 / n);

        public static explicit operator Vector3(XIVCoord coord)
        {
            var cart = coord.ToCartesian();
            return new Vector3((float)cart.X, (float)cart.Y, (float)cart.Z);
        }

        private static readonly Regex rexOpKeywords = new Regex(
            @"\b(plus|minus|polar|minuspolar|polardirN?\d+)\b",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private static readonly Regex rexDirKeyword = new Regex(
            @"^polardir(?<isNegative>N)?(?<segments>\d+)$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

        /// <summary>
        /// 将直角坐标、极坐标及离散方向坐标解析并叠加。<br/><br/>
        /// 例如：<br/>
        /// 10, -10, 0<br/>
        /// polar 20, -45°, 0<br/>
        /// 100, 100, 0 polardirN4 10, 2（从给定点向右下偏移 10）<br/>
        /// polardir8 10, 1（按八方向中的第 1 个方向偏移 10）<br/><br/>
        /// 各组坐标可使用 plus、minus、polar、minuspolar、polardirN? 等关键字连接。
        /// polardirN? 与 polardir? 的方向规则一致，但序号增加 0.5，序号以 0 为起点。
        /// </summary>
        public static XIVCoord ParseRawData(string rawCoords)
        {
            // 例：x, y, z plus x2, y2 minus x3, y3, z3 polar r, θ polardirN4 r2, index2

            List<string> parts = new List<string>();
            string currentPart = "";
            int depth = 0;
            foreach (char c in rawCoords)
            {
                switch (c)
                {
                    case ',':
                        if (depth == 0)
                        {
                            parts.Add(currentPart);
                            currentPart = "";
                            continue;
                        }
                        break;
                    case '(': depth++; break;
                    case ')': depth--; break;
                }
                currentPart += c;
            }
            parts.Add(currentPart);

            if (depth != 0)
            {
                throw new Exception(I18n.Translate("internal/XIVCoord/unmatchedparentheses", "Coordinate expression contains {0} unmatched parentheses ({1}). Expression: {2}", Abs(depth), depth > 0 ? "(" : ")", rawCoords));
            }

            List<XIVCoord> coords = new List<XIVCoord>();
            bool isCurrentPolar = false;
            bool isCurrentPlus = true;
            int currentDirSegments = 0;
            bool currentDirNegative = false;
            List<string> currentParams = new List<string>();

            foreach (string part in parts)
            {
                string[] splitParts = rexOpKeywords.Split(part);

                if (splitParts.Length == 3)
                {
                    string beforeOp = splitParts[0].Trim();
                    string operation = splitParts[1].Trim();
                    string afterOp = splitParts[2].Trim();

                    // 先完成前一组坐标的解析，再切换到新的坐标类型。
                    if (currentParams.Count != 0 || !string.IsNullOrEmpty(beforeOp))
                    {
                        currentParams.Add(beforeOp);
                        XIVCoord coord = ParseCoordPart(currentParams, isCurrentPolar, currentDirSegments, currentDirNegative);
                        coords.Add(isCurrentPlus ? coord : -coord);
                        currentParams.Clear();
                    }

                    var dirMatch = rexDirKeyword.Match(operation);
                    if (dirMatch.Success)
                    {
                        // polardirN4 r, 2 等价于 polar r, π/4；负向分段的序号偏移 0.5。
                        if (!int.TryParse(dirMatch.Groups["segments"].Value, out currentDirSegments) || currentDirSegments <= 0)
                            throw new ArgumentException(I18n.Translate("internal/XIVCoord/invalidsegments", "Coordinate direction segment count must be a positive integer: {0}", operation));

                        currentDirNegative = dirMatch.Groups["isNegative"].Success;
                        isCurrentPolar = true;
                        isCurrentPlus = true;
                    }
                    else
                    {
                        currentDirSegments = 0;
                        currentDirNegative = false;
                        isCurrentPlus = !operation.StartsWith("minus", StringComparison.OrdinalIgnoreCase);
                        isCurrentPolar = operation.EndsWith("polar", StringComparison.OrdinalIgnoreCase);
                    }

                    currentParams.Add(afterOp);
                }
                else if (splitParts.Length == 1)
                {
                    currentParams.Add(splitParts[0].Trim());
                }
                else
                {
                    throw new Exception(I18n.Translate("internal/XIVCoord/missingarguments", "Not enough arguments between coordinate keywords.\nExpression: {0};\nLocation: {1}", rawCoords, part));
                }
            }

            XIVCoord finalCoord = ParseCoordPart(currentParams, isCurrentPolar, currentDirSegments, currentDirNegative);
            coords.Add(isCurrentPlus ? finalCoord : -finalCoord);

            return coords.Aggregate((c1, c2) => c1 + c2);
        }

        private static XIVCoord ParseCoordPart(List<string> parameters, bool isPolar, int dirSegments, bool dirNegative)
        {
            string[] args = parameters.ToArray();
            if (dirSegments > 0 && (args.Length == 2 || args.Length == 3))
            {
                double index = MathParser.Parse(args[1]);
                if (dirNegative)
                    index += 0.5;

                index = (index % dirSegments + dirSegments) % dirSegments;
                double θ = index / dirSegments * 2 * PI - PI;
                args[1] = θ.ToString("R", CultureInfo.InvariantCulture);
            }

            return isPolar ? PolarCoord.Parse(args) : (XIVCoord)CartesianCoord.Parse(args);
        }
    }
}
