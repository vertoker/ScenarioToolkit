using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRF.Utilities
{
    /// <summary>
    /// Операции и расширения над Vector3
    /// </summary>
    public static class VrfVector3
    {
        public static readonly Vector3 Max = new(float.MaxValue, float.MaxValue, float.MaxValue);
        public static readonly Vector3 Min = new(float.MinValue, float.MinValue, float.MinValue);
        
        public static Vector3 Average(Vector3 vector1, Vector3 vector2)
            => (vector1 + vector2) / 2f;
        public static Vector3 Average(Vector3 vector1, Vector3 vector2, Vector3 vector3)
            => (vector1 + vector2 + vector3) / 3f;
        public static Vector3 Average(Vector3 vector1, Vector3 vector2, Vector3 vector3, Vector3 vector4)
            => (vector1 + vector2 + vector3 + vector4) / 4f;

        public static Vector3 Aggregate(params Vector3[] vectors) 
            => vectors.Aggregate();
        
        public static float SquaredDistance(Vector3 a, Vector3 b)
        {
            var x = a.x - b.x;
            var y = a.y - b.y;
            var z = a.z - b.z;
            return x * x + y * y + z * z;
        }
        
        private static Vector3 RotateZRad(Vector3 vector, float cos, float sin)
        {
            return new Vector3(vector.x * cos - vector.z * sin, vector.y, vector.x * sin + vector.z * cos);
        }

        #region Extensions

        public const float VectorEpsilon = 0.001f;

        public static Vector3 GetWithX(this Vector3 value, float x) => new(x, value.y, value.z);
        public static Vector3 GetWithY(this Vector3 value, float y) => new(value.x, y, value.z);
        public static Vector3 GetWithZ(this Vector3 value, float z) => new(value.x, value.y, z);
        
        public static Vector3 GetWithX(this Vector3 value, Vector3 vector) => new(vector.x, value.y, value.z);
        public static Vector3 GetWithY(this Vector3 value, Vector3 vector) => new(value.x, vector.y, value.z);
        public static Vector3 GetWithZ(this Vector3 value, Vector3 vector) => new(value.x, value.y, vector.z);

        public static Vector3 GetWithXY(this Vector3 value, float x, float y) => new(x, y, value.z);
        public static Vector3 GetWithXZ(this Vector3 value, float x, float z) => new(x, value.y, z);
        public static Vector3 GetWithYZ(this Vector3 value, float y, float z) => new(value.x, y, z);
        
        public static Vector3 GetWithXY(this Vector3 value, Vector2 vector) => new(vector.x, vector.y, value.z);
        public static Vector3 GetWithXZ(this Vector3 value, Vector2 vector) => new(vector.x, value.y, vector.y);
        public static Vector3 GetWithYZ(this Vector3 value, Vector2 vector) => new(value.x, vector.x, vector.y);

        public static Vector3 GetWithXY(this Vector3 value, Vector3 vector) => new(vector.x, vector.y, value.z);
        public static Vector3 GetWithXZ(this Vector3 value, Vector3 vector) => new(vector.x, value.y, vector.z);
        public static Vector3 GetWithYZ(this Vector3 value, Vector3 vector) => new(value.x, vector.y, vector.z);

        public static bool IsZero(this Vector3 vector, float epsilon = VectorEpsilon) 
            => vector.x * vector.x + vector.y * vector.y + vector.z * vector.z < epsilon;
        
        public static bool IsZero(this Vector3 vector, bool maskX, bool maskY, bool maskZ, float epsilon = VectorEpsilon)
        {
            var result = 0f;
            if (maskX) result += vector.x * vector.x;
            if (maskY) result += vector.y * vector.y;
            if (maskZ) result += vector.z * vector.z;
            return result < epsilon;
        }
        
        public static Vector3 Aggregate(this IEnumerable<Vector3> vectors) 
            => vectors.Aggregate(Vector3.zero, (current, vector) => current + vector);
        
        public static float ComponentsAverage(this Vector3 vector) 
            => (vector.x + vector.y + vector.z) / 3f;

        public static Vector3 ClampToOnlySign(this Vector3 vector)
        {
            var x = vector.x < 0 ? -1 : 1;
            var y = vector.y < 0 ? -1 : 1;
            var z = vector.z < 0 ? -1 : 1;
            return new Vector3(x, y, z);
        }
        
        public static Vector3 MultiplyBy(this Vector3 a, Vector3 b) 
            => new(a.x * b.x, a.y * b.y, a.z * b.z);
        
        public static Vector3 DivideBy(this Vector3 a, Vector3 b) 
            => new(a.x / b.x, a.y / b.y, a.z / b.z);

        public static Vector3 Invert(this Vector3 a) 
            => new(1f / a.x, 1f / a.y, 1f / a.z);

        public static Vector3 RotateZDeg(this Vector3 vector, float angleDeg)
            => vector.RotateZRad(angleDeg * Mathf.Deg2Rad);
        
        public static Vector3 RotateZRad(this Vector3 vector, float angleRad)
        {
            var sin = Mathf.Sin(angleRad);
            var cos = Mathf.Cos(angleRad);
            return RotateZRad(vector, cos, sin);
        }
        
        public static Vector3 RotateVectorXZ(this Vector3 value, float angle)
        {
            var x = Mathf.Cos(angle) * value.x - Mathf.Sin(angle) * value.z;
            var z = Mathf.Sin(angle) * value.x + Mathf.Cos(angle) * value.z;
            return new Vector3(x, value.y, z);
        }
        
        public static void SetMagnitude(this ref Vector3 vector, float magnitude)
        {
            if (vector == Vector3.zero) return;
            var currentMagnitude = vector.magnitude;
            vector *= magnitude / currentMagnitude;
        }
        public static Vector3 GetMagnitude(this Vector3 vector, float magnitude)
        {
            if (vector == Vector3.zero) return Vector3.zero;
            var currentMagnitude = vector.magnitude;
            return vector * (magnitude / currentMagnitude);
        }
        
        #endregion
    }
}