using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
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
                return new PolarCoord(polarA.R, polarA.θ + PI, polarA.Z);
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
                return new PolarCoord(polarA.R * n, polarA.θ, polarA.Z);
            }
        }

        public static XIVCoord operator *(double n, XIVCoord a) => a * n;

        public static XIVCoord operator /(XIVCoord a, double n) => a * (1.0 / n);

        public static explicit operator Vector3(XIVCoord coord)
        {
            var cart = coord.ToCartesian();
            return new Vector3((float)cart.X, (float)cart.Y, (float)cart.Z);
        }

        private static Regex rexOpKeywords = new Regex(@"\b(plus|minus|polar|minuspolar)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// 将一串直角坐标、极坐标、或混合方式指定的坐标解析并叠加，如：<br /><br />
        /// <paramref str="A"/>: 10, -10, 0 <br />
        /// <paramref str="A"/>: <paramref str="polar"/> 20, -45°, 0<br />
        /// <paramref str="A"/>: 10, -10, 0 <paramref str="polar"/> 20, -45°（在直角坐标基础上叠加极坐标结果）<br /><br />
        /// 字符串格式详见 <paramref str="rawCoords"/>。
        /// </summary>
        /// <param str="rawCoords">
        /// 一串坐标字符串，可包含多组坐标。<br />
        /// 每组坐标之间以关键字连接，坐标分量之间以逗号连接。如：<br /><br />
        /// <paramref str="A"/>:     x, y, z  
        /// <paramref str="plus"/>   x2, y2  
        /// <paramref str="minus"/>  x3, y3, z3  
        /// <paramref str="polar"/>  r1, θ1
        /// <paramref str="minuspolar"/> r2, θ2, z2<br />
        /// </param>
        public static XIVCoord ParseRawData(string rawCoords)
        {
            // 例：x, y, z plus x2, y2 minus x3, y3, z3 polar r1, θ1 minuspolar r2, θ2, z2

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
                throw new Exception($"坐标参数存在 {Abs(depth)} 个未闭合的{(depth > 0 ? "左" : "右")}括号。表达式：{rawCoords}");
            }

            // 此时：[x] [y] [z plus x2] [y2 minus x3] [y3] [z3 polar r] [θ] [z]

            List<XIVCoord> coords = new List<XIVCoord>();
            bool isCurrentPolar = false;
            bool isCurrentPlus = true;
            List<string> currentParams = new List<string>();

            foreach (string part in parts)
            {
                string[] splitParts = rexOpKeywords.Split(part);

                if (splitParts.Length == 3) // 找到操作符，拆分解析
                {
                    string beforeOp = splitParts[0].Trim();
                    string operation = splitParts[1].Trim();
                    string afterOp = splitParts[2].Trim();

                    // 处理前部分
                    if (currentParams.Count != 0 || !string.IsNullOrEmpty(beforeOp)) // 不是形如 "polar ..." 的字符串开始位置
                    {
                        currentParams.Add(beforeOp);
                        XIVCoord coord = isCurrentPolar
                            ? PolarCoord.Parse(currentParams.ToArray())
                            : (XIVCoord)CartesianCoord.Parse(currentParams.ToArray());
                        coords.Add(isCurrentPlus ? coord : -coord);
                        currentParams.Clear();
                    }

                    // 处理操作符：是否是加法/极坐标操作
                    isCurrentPlus = !operation.StartsWith("minus");
                    isCurrentPolar = operation.EndsWith("polar");

                    // 处理后部分
                    currentParams.Add(afterOp);
                }
                else if (splitParts.Length == 1) // 未找到操作符，直接添加
                {
                    currentParams.Add(splitParts[0].Trim());
                }
                else // 偷个懒，坐标最少两个参数，而只要有两个就会被逗号预先拆分，所以正常不会出现 1 3 以外的情况
                {
                    throw new Exception($"坐标参数解析时，关键字之间参数过少。\n表达式：{rawCoords}；\n出错位置：{part}");
                }
            }
            XIVCoord finalCoord = isCurrentPolar
                ? PolarCoord.Parse(currentParams.ToArray())
                : (XIVCoord)CartesianCoord.Parse(currentParams.ToArray());
            coords.Add(isCurrentPlus ? finalCoord : -finalCoord);

            // 此时：[Cartesian1] [Cartesian2] [Cartesian3] [Polar1]
            return coords.Aggregate((c1, c2) => c1 + c2);
        }

    }

}
