using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRF.Utilities
{
    /// <summary>
    /// Операции и расширения над Vector2
    /// </summary>
    public static class VrfVector2
    {
        public static readonly Vector2 Max = new(float.MaxValue, float.MaxValue);
        public static readonly Vector2 Min = new(float.MinValue, float.MinValue);
        
        public static Vector2 Average(Vector2 vector1, Vector2 vector2)
            => (vector1 + vector2) / 2f;
        public static Vector2 Average(Vector2 vector1, Vector2 vector2, Vector2 vector3)
            => (vector1 + vector2 + vector3) / 3f;
        public static Vector2 Average(Vector2 vector1, Vector2 vector2, Vector2 vector3, Vector2 vector4)
            => (vector1 + vector2 + vector3 + vector4) / 4f;

        public static Vector2 Aggregate(params Vector2[] vectors) 
            => vectors.Aggregate();
        
        public static float SquaredDistance(Vector2 a, Vector2 b)
        {
            var x = a.x - b.x;
            var y = a.y - b.y;
            return x * x + y * y;
        }

        public static Vector2 RotateDeg(this Vector2 vector, float angleDeg)
            => vector.RotateRad(angleDeg * Mathf.Deg2Rad);
        
        public static Vector2 RotateRad(this Vector2 vector, float angleRad)
        {
            var sin = Mathf.Sin(angleRad);
            var cos = Mathf.Cos(angleRad);
            return RotateRad(vector, cos, sin);
        }
        
        private static Vector2 RotateRad(Vector2 vector, float cos, float sin)
        {
            return new Vector2(vector.x * cos - vector.y * sin, vector.x * sin + vector.y * cos);
        }

        #region Extensions

        public const float VectorEpsilon = 0.001f;
        
        public static bool IsZero(this Vector2 vector, float epsilon = VectorEpsilon) 
            => vector.x * vector.x + vector.y * vector.y < epsilon;

        public static bool IsZero(this Vector2 vector, bool maskX, bool maskY, float epsilon = VectorEpsilon)
        {
            var result = 0f;
            if (maskX) result += vector.x * vector.x;
            if (maskY) result += vector.y * vector.y;
            return result < epsilon;
        }
        
        public static Vector2 Aggregate(this IEnumerable<Vector2> vectors) 
            => vectors.Aggregate(Vector2.zero, (current, vector) => current + vector);
        
        public static float ComponentsAverage(this Vector2 vector) 
            => (vector.x + vector.y) / 2f;
        
        public static Vector2 ClampToOnlySign(this Vector2 vector)
        {
            var x = vector.x < 0 ? -1 : 1;
            var y = vector.y < 0 ? -1 : 1;
            return new Vector2(x, y);
        }
        
        public static Vector2 MultiplyBy(this Vector2 a, Vector2 b) 
            => new(a.x * b.x, a.y * b.y);
        
        public static Vector2 DivideBy(this Vector2 a, Vector2 b) 
            => new(a.x / b.x, a.y / b.y);
        
        public static Vector2 Invert(this Vector2 a) 
            => new(1f / a.x, 1f / a.y);
        
        public static void SetMagnitude(this ref Vector2 vector, float magnitude)
        {
            if (vector == Vector2.zero) return;
            var currentMagnitude = vector.magnitude;
            vector *= magnitude / currentMagnitude;
        }
        public static Vector2 GetMagnitude(this Vector2 vector, float magnitude)
        {
            if (vector == Vector2.zero) return Vector2.zero;
            var currentMagnitude = vector.magnitude;
            return vector * (magnitude / currentMagnitude);
        }

        #endregion
    }
}