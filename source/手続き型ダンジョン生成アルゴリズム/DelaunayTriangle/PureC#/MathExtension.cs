using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration.Diagram
{
    public static class MathExtension
    {
        public static float Pow(this float value, float power)
        {
            return Mathf.Pow(value, power);
        }

        public static float Pow2(this float value)
        {
            return Mathf.Pow(value, 2.0f);
        }
    }
}