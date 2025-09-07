using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRF.Utilities
{
    /// <summary>
    /// Операции и расширения над Vector4
    /// </summary>
    public static class VrfVector4
    {
        public static readonly Vector4 Max = new(float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue);
        public static readonly Vector4 Min = new(float.MinValue, float.MinValue, float.MinValue, float.MinValue);
        
        public static Vector4 Average(Vector4 vector1, Vector4 vector2)
            => (vector1 + vector2) / 2f;
        public static Vector4 Average(Vector4 vector1, Vector4 vector2, Vector4 vector3)
            => (vector1 + vector2 + vector3) / 3f;
        public static Vector4 Average(Vector4 vector1, Vector4 vector2, Vector4 vector3, Vector4 vector4)
            => (vector1 + vector2 + vector3 + vector4) / 4f;

        public static Vector4 Aggregate(params Vector4[] vectors) 
            => vectors.Aggregate();
        
        public static float SquaredDistance(Vector4 a, Vector4 b)
        {
            var x = a.x - b.x;
            var y = a.y - b.y;
            var z = a.z - b.z;
            var w = a.w - b.w;
            return x * x + y * y + z * z + w * w;
        }

        #region Extensions

        public const float VectorEpsilon = 0.001f;
        
        public static bool IsZero(this Vector4 vector, float epsilon = VectorEpsilon) 
            => vector.x * vector.x + vector.y * vector.y + vector.z * vector.z + vector.w * vector.w < epsilon;
        
        public static bool IsZero(this Vector4 vector, bool maskX, bool maskY, bool maskZ, bool maskW, float epsilon = VectorEpsilon)
        {
            var result = 0f;
            if (maskX) result += vector.x * vector.x;
            if (maskY) result += vector.y * vector.y;
            if (maskZ) result += vector.z * vector.z;
            if (maskW) result += vector.w * vector.w;
            return result < epsilon;
        }
        
        public static Vector4 Aggregate(this IEnumerable<Vector4> vectors) 
            => vectors.Aggregate(Vector4.zero, (current, vector) => current + vector);

        public static float ComponentsAverage(this Vector4 vector) 
            => (vector.x + vector.y + vector.z + vector.w) / 4f;
        
        public static Vector4 ClampToOnlySign(this Vector4 vector)
        {
            var x = vector.x < 0 ? -1 : 1;
            var y = vector.y < 0 ? -1 : 1;
            var z = vector.z < 0 ? -1 : 1;
            var w = vector.w < 0 ? -1 : 1;
            return new Vector4(x, y, z, w);
        }
        
        public static Vector4 MultiplyBy(this Vector4 a, Vector4 b) 
            => new(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        
        public static Vector4 DivideBy(this Vector4 a, Vector4 b) 
            => new(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);
        
        public static Vector4 Invert(this Vector4 a) 
            => new(1f / a.x, 1f / a.y, 1f / a.z, 1f / a.w);

        #endregion
    }
}