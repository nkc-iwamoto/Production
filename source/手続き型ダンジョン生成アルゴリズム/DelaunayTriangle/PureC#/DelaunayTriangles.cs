using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;

namespace C__Graphics
{
    public static class DelaunayTriangles
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start">矩形の左上座標</param>
        /// <param name="end">矩形の右下座標</param>
        /// <returns></returns>
        public static Triangle GetSuperTriangle(Vector3 start, Vector3 end)
        {

            /*
             * 与えられた矩形を包含する円を求める
             *      円の中心　center = 矩形の中心
             *      円の半径　radius = |p - center| + p
             *     ただし、pは与えられた矩形の任意の頂点　pは正数
             */
            Vector3 center = (end - start) / 2;
            float radius = (center - start).magnitude;

            Vector3 vertex1 = new Vector3
                (
                    // √3ｘ半径
                    x: Mathf.Sqrt(3) * radius - center.x,
                    y: center.y - radius,
                    z: 0
                );
            Vector3 vertex2 = new Vector3
                (
                    x: Mathf.Sqrt(3) * radius + center.x,
                    y: center.y - radius,
                    z: 0
                );
            Vector3 vertex3 = new Vector3
                (
                    x: center.x,
                    y: center.y - radius,
                    z: 0
                );

            return new Triangle(vertex1, vertex2, vertex3);
        }
        public static List<Triangle> Generate(List<Vector3> pointArray)
        {
            // 一番遠い点
            float maxDistanceSqr = pointArray.Max(x => x.sqrMagnitude);
            // ↑の点を含む円の直径
            float radius = Mathf.Sqrt(maxDistanceSqr) * 2;
            // ↑の円に外接する三角形の頂点
            float dividedThree = Mathf.PI * 2.0f / 3.0f;

            // 三角形の頂点
            Vector3[] superTriangleVertex = new Vector3[3];

            for (int i = 0; i < superTriangleVertex.Length; ++i)
            {
                float rad = dividedThree * (i + 1.0f) + (Mathf.PI / 2.0f);
                superTriangleVertex[i] = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
            }

            // 巨大な三角形を生成
            Triangle superTriangle = new Triangle(superTriangleVertex[0], superTriangleVertex[1], superTriangleVertex[2]);

            List<Triangle> triangles = new List<Triangle>
            {
                superTriangle
            };
            Split(pointArray, triangles);
            DeleteSuperTriangle(superTriangle, triangles);
            return triangles;

        }

        private static void DeleteSuperTriangle(Triangle superTriangle, List<Triangle> triangles)
        {
            foreach (Triangle item in triangles.ToArray())
            {
                if (item.IsIncludeCommponPoint(superTriangle))
                {
                    triangles.Remove(item);
                }
            }
        }
        private static void Split(List<Vector3> pointArray, List<Triangle> triangles)
        {
            Dictionary<Triangle, bool> tmpHash = new Dictionary<Triangle, bool>();
            Stack<LineSegment> lineStack = new Stack<LineSegment>();
            for (int i = 0; i < pointArray.Count; i++)
            {
                Vector2 newPoint = pointArray[i];
                tmpHash.Clear();

                foreach (var currentTri in triangles.ToArray())
                {
                    lineStack.Clear();
                    var circle = currentTri.GetCircumscribedCircle();
                    // この三角形が点を含まない
                    if (!circle.DotInclude(newPoint)) { continue; }

                    Vector3[] verts = currentTri.GetVertex();
                    triangles.Remove(currentTri);

                    Triangle[] newTris = new Triangle[3];
                    newTris[0] = new Triangle(verts[0], verts[1], newPoint);
                    newTris[1] = new Triangle(verts[1], verts[2], newPoint);
                    newTris[2] = new Triangle(verts[2], verts[0], newPoint);

                    for (int k = 0; k < newTris.Length; k++)
                    {
                        if (tmpHash.ContainsKey(newTris[k]))
                        {
                            tmpHash[newTris[k]] = true;
                            continue;
                        }

                        tmpHash.Add(newTris[k], false);
                    }
                }

                // 重複してない三角形を取り出す
                var triEnumerator = tmpHash
                    .Where((x) => !x.Value)
                    .Select((x) => x.Key);

                // 追加する
                foreach (var tri in triEnumerator)
                {
                    triangles.Add(tri);
                }
            }
        }
    }
}