using System;
using System.Numerics;
using static System.Math;

namespace Triggernometry.Utilities.Maths
{
    /// <summary>
    /// 用于 Omen 绘制
    /// </summary>
    public class IsoscelesTriangle
    {
        public XIVCoord Center { get; set; }
        public float θ { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }

        /// <summary> v1 v2 为两个相等的角对应的坐标，传入之前需要确保为等腰三角形 </summary>
        public IsoscelesTriangle(Vector2 v0, Vector2 v1, Vector2 v2)
        {
            Center = new CartesianCoord(v0.X, v0.Y, 0);
            var m = (v1 + v2) / 2; // 底边中点
            ScaleX = Vector2.Distance(v1, v2) / 2;
            ScaleY = Vector2.Distance(v0, m);
            var vec = m - v0; // 顶点到底边中点的向量
            θ = (float)Atan2(vec.X, vec.Y);
        }
    }
}
