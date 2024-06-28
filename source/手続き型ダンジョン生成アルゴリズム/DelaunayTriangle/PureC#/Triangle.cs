using DungeonGeneration.Diagram;
using System;
using System.Linq;
using UnityEngine;
namespace C__Graphics
{
    public struct Triangle
    {
        public Vector3 p1 { get; private set; }
        public Vector3 p2 { get; private set; }
        public Vector3 p3 { get; private set; }

        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }

        public override bool Equals(object other)
        {
            return other is Triangle tri &&
               (
                   (this.p1 == tri.p1 && this.p2 == tri.p2 && this.p3 == tri.p3)
                   || (this.p1 == tri.p1 && this.p2 == tri.p3 && this.p3 == tri.p2)

                   || (this.p1 == tri.p2 && this.p2 == tri.p3 && this.p3 == tri.p1)
                   || (this.p1 == tri.p2 && this.p2 == tri.p1 && this.p3 == tri.p3)

                   || (this.p1 == tri.p3 && this.p2 == tri.p1 && this.p3 == tri.p2)
                   || (this.p1 == tri.p3 && this.p2 == tri.p2 && this.p3 == tri.p1)
               );
        }

        public bool HasCommonPoints(Triangle t)
        {
            return (p1.Equals(t.p1) || p1.Equals(t.p2) && p1.Equals(t.p3) ||
                    p2.Equals(t.p1) || p2.Equals(t.p2) || p2.Equals(t.p3) ||
                    p3.Equals(t.p1) || p3.Equals(t.p2) || p3.Equals(t.p3));
        }

        public bool IsIncludeCommponPoint(Triangle triangle)
        {
            var selfVertex = this.GetVertex();
            var targetVertex = triangle.GetVertex();

            return selfVertex.Contains(targetVertex[0])
                || selfVertex.Contains(targetVertex[1])
                || selfVertex.Contains(targetVertex[2]);
        }

        // 外接円を求める
        public Circle GetCircumscribedCircle()
        {
            /*
                この方程式を x と y について解くと、
                x = { (y3 - y1)(x22 - x12 + y22 - y12) + (y1 - y2)(x32 - x12 + y32 - y12) } / c
                y = { (x1 - x3)(x22 - x12 + y22 - y12) + (x2 - x1)(x32 - x12 + y32 - y12) } / c
                となり、中心座標が得られる。ただし
                c = 2 { (x2 - x1)(y3 - y1) - (y2 - y1)(x3 - x1) }
             */

            float x1 = p1.x;
            float y1 = p1.y;
            float x2 = p2.x;
            float y2 = p2.y;
            float x3 = p3.x;
            float y3 = p3.y;

            float c = 2 * ((x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1));

            float x = ((y3 - y1) * (x2.Pow2() - x1.Pow2() + y2.Pow2() - y1.Pow2()) + (y1 - y2) * (x3.Pow2() - x1.Pow2() + y3.Pow2() - y1.Pow2())) / c;
            float y = ((x1 - x3) * (x2.Pow2() - x1.Pow2() + y2.Pow2() - y1.Pow2()) + (x2 - x1) * (x3.Pow2() - x1.Pow2() + y3.Pow2() - y1.Pow2())) / c;

            Vector3 center = new Vector2(x, y);
            float radius = (p1 - center).magnitude;

            return new Circle(center, radius);
        }

    


        public Vector3[] GetVertex()
        {
            return new Vector3[] { p1, p2, p3 };
        }


        public override int GetHashCode()
        {
            int hashCode = Mathf.FloorToInt(p1.x * p2.x * p3.x + p1.y * p2.y * p3.y);
            return hashCode;
        }
        public static bool operator ==(Triangle x, Triangle y)
        {
            return x.Equals(y);
        }
        public static bool operator !=(Triangle x, Triangle y)
        {
            return !x.Equals(y);
        }
    }
}
