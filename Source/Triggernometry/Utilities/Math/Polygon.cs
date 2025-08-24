using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using static Triggernometry.Utilities.DataStringHelper;

namespace Triggernometry.Utilities.Math
{
    public class Polygon
    {
        /// <summary> Clockwise (in-game coordination system). </summary>
        public List<Vector2> VertexList { get; private set; }
        public int N => VertexList?.Count ?? 0;

        public Polygon(params Vector2[] vertices) : this(vertices?.ToList()) { }
        public Polygon(IEnumerable<Vector2> vertices) : this(vertices?.ToList()) { }
        public Polygon(List<Vector2> vertexList)
        {
            if (vertexList == null || vertexList.Count < 3)
                throw new ArgumentException("多边形至少需要 3 个顶点。");
            int n = vertexList.Count;
            /*
            // 确保各边未相交
            for (int i = 0; i < n; i++)
            {
                var a1 = vertexList[i];
                var a2 = vertexList[(i + 1) % n];
                for (int j = i + 2; j < (i > 0 ? n : n - 1); j++) // 跳过相邻顶点和前面的顶点（n-1 是 0 的相邻顶点）
                {
                    var b1 = vertexList[j];
                    var b2 = vertexList[(j + 1) % n];
                    if (CheckSegmentsIntersect(a1, a2, b1, b2))
                        throw new ArgumentException($"检测到相交的边：({a1.X}, {a1.Y}) - ({a2.X}, {a2.Y}) | ({b1.X}, {b1.Y}) - ({b2.X}, {b2.Y})");
                }
            }
            */
            // 确保顶点为顺时针
            double doubleArea = 0;
            for (int i = 0; i < n; i++)
            {
                var v1 = vertexList[i];
                var v2 = vertexList[(i + 1) % n];
                doubleArea += (v2.X + v1.X) * (v2.Y - v1.Y);
            }
            if (doubleArea > 0)
                VertexList = vertexList.ToList();
            else
                VertexList = vertexList.AsEnumerable().Reverse().ToList();
        }

        /// <summary>
        /// 格式：x1, y1; x2, y2; ..
        /// </summary>
        public static Polygon Parse(string rawExpr)
        {
            List<Vector2> vertices = rawExpr
                .Split(';')
                .Select(rawXY => ParseArgs<float, float>(rawXY))
                .Select(tuple => new Vector2(tuple.Item1, tuple.Item2))
                .ToList();
            return new Polygon(vertices);
        }

        /// <summary> 检查 A1-A2、B1-B2 是否相交。共顶点不视为相交。</summary>
        private static bool CheckSegmentsIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            Vector2 A1A2 = a2 - a1;
            Vector2 B1B2 = b2 - b1;
            Vector2 A1B1 = b1 - a1;
            Vector2 A1B2 = b2 - a1;
            Vector2 B1A1 = -A1B1;
            Vector2 B1A2 = a2 - b1;
            var tolerance = 1e-5;
            return Cross(A1A2, A1B1) * Cross(A1A2, A1B2) < -tolerance &&    // b1、b2 在 a1-a2 的两侧
                   Cross(B1B2, B1A1) * Cross(B1B2, B1A2) < -tolerance;     // a1、a2 在 b1-b2 的两侧
        }

        private static float Cross(Vector2 a, Vector2 b) => -a.X * b.Y + b.X * a.Y; // 左手系

        public List<Polygon> Triangulate(double tolerance = 0.05)
        { 
            var vertices = VertexList.ToList();
            var output = new List<Polygon>();
            while (vertices.Count > 3)
            {
                var n = vertices.Count;
                var minIdx = vertices
                    .Select((v, i) => (idx: i, angle: GetAngle(vertices, i)) )
                    .Where(tuple => tuple.angle >= 0) // 过滤掉凹顶点
                    .OrderBy(tuple => tuple.angle) // 取最小的有向角度
                    .First().idx;
                var v1 = vertices[(minIdx - 1 + n) % n];
                var v2 = vertices[minIdx];
                var v3 = vertices[(minIdx + 1) % n];
                var err = tolerance * tolerance;
                if (Vector2.DistanceSquared(v1, v2) <= err || Vector2.DistanceSquared(v2, v3) <= err || Vector2.DistanceSquared(v3, v1) <= err) 
                    continue; // 跳过任意边小于容差的三角形
                output.Add(new Polygon(v1, v2, v3));
                vertices.RemoveAt(minIdx);
            }
            output.Add(new Polygon(vertices));
            return output;
        }

        /// <summary> 计算有向角度，正值代表游戏坐标系下这个点前后连接为顺时针方向。 </summary>
        public /*private*/ double GetAngle(List<Vector2> vertices, int idx)
        {
            int n = vertices.Count;
            Vector2 v1 = vertices[(idx - 1 + n) % n] - vertices[idx];   // 当前点 -> 前一个点
            Vector2 v2 = vertices[(idx + 1) % n] - vertices[idx];       // 当前点 -> 后一个点

            return Atan2(Cross(v1, v2), Vector2.Dot(v1, v2));    // 有符号角度 [-π, π]
        }

        public List<IsoscelesTriangle> IsoscelesTriangulate(double tolerance = 0.05)
        {
            if (N != 3)
                throw new InvalidOperationException("只能对三角形进行等腰子三角剖分。");

            var result = new List<IsoscelesTriangle>();

            // 三个顶点
            var (A, B, C) = (VertexList[0], VertexList[1], VertexList[2]);
            // 三个边长
            var (a, b, c) = (Vector2.Distance(B, C), Vector2.Distance(C, A), Vector2.Distance(A, B));
            // 调整顺序至 A <= B <= C
            var sort = new[] { (a, A), (b, B), (c, C) }.OrderBy(pair => pair.Item1).ToList();
            ((a, A), (b, B), (c, C)) = (sort[0], sort[1], sort[2]);
            // 检查等腰三角形特殊情况
            if (Abs(a - b) <= tolerance && c > tolerance)
            {
                result.Add(new IsoscelesTriangle(C, A, B));
                return result;
            }
            if (Abs(b - c) <= tolerance && a > tolerance)
            {
                result.Add(new IsoscelesTriangle(A, B, C));
                return result;
            }
            if (Abs(c - a) <= tolerance && b > tolerance)
            {
                result.Add(new IsoscelesTriangle(B, C, A));
                return result;
            }
            // 不等边三角形：判断三角形类型
            var difference = a * a + b * b - c * c;
            var err = Sqrt(2) * tolerance * Max(a, b); // 这个误差可保证视为直角三角形时顶点的偏移 ~ tolerance
            if (difference > err) // 锐角：外心三分
            {
                var (xA, yA) = (A.X, A.Y);
                var (xB, yB) = (B.X, B.Y);
                var (xC, yC) = (C.X, C.Y);
                var d = 2 * (xA * (yB - yC) + xB * (yC - yA) + xC * (yA - yB));
                var x = ((xA * xA + yA * yA) * (yB - yC) +
                          (xB * xB + yB * yB) * (yC - yA) +
                          (xC * xC + yC * yC) * (yA - yB)) / d;
                var y = ((xA * xA + yA * yA) * (xC - xB) +
                          (xB * xB + yB * yB) * (xA - xC) +
                          (xC * xC + yC * yC) * (xB - xA)) / d;
                var M = new Vector2((float)x, (float)y); // 外心

                result.Add(new IsoscelesTriangle(M, A, B));
                result.Add(new IsoscelesTriangle(M, B, C));
                result.Add(new IsoscelesTriangle(M, C, A));
            }
            else if (difference >= -err) // 直角：斜边中点二分
            {
                var M = (A + B) / 2;
                result.Add(new IsoscelesTriangle(M, C, A));
                result.Add(new IsoscelesTriangle(M, C, B));
            }
            else // 钝角
            {
                // 从钝角做垂线 AH，分为两个直角三角形
                var dx = A.X - B.X;
                var dy = A.Y - B.Y;
                var t = ((C.X - B.X) * dx + (C.Y - B.Y) * dy) / (dx * dx + dy * dy);
                var H = new Vector2((float)(B.X + t * dx), (float)(B.Y + t * dy));
                // 按直角处理
                var Mb = (C + A) / 2;
                var Ma = (C + B) / 2;
                result.Add(new IsoscelesTriangle(Ma, H, B));
                result.Add(new IsoscelesTriangle(Ma, H, C));
                result.Add(new IsoscelesTriangle(Mb, H, C));
                result.Add(new IsoscelesTriangle(Mb, H, A));
            }
            return result;
        }
    }

}
