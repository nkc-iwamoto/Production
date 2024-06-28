using UnityEngine;

namespace C__Graphics
{
    public struct Circle
    {
        // 中心座標
        public Vector3 center { get; private set; }
        public float radius { get; private set; }

        public Circle(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public bool DotInclude(Vector3 point)
        {
            float distance = (point - center).magnitude;
            return distance < radius;
        }
    }
}
