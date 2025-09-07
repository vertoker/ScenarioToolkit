using System.Collections.Generic;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF.Utilities.Extensions
{
    public static class VrfBoundsExtensions
    {
        /// <summary>
        /// From local space to world space
        /// </summary>
        public static Bounds GetBounds(this Transform self)
        {
            return new Bounds(self.position, self.lossyScale);
        }
        
        // https://discussions.unity.com/t/can-39-t-convert-bounds-from-world-coordinates-to-local-coordinates/57667/4

        /// <summary>
        /// From local space to world space
        /// </summary>
        public static Bounds TransformBounds(this Transform self, Bounds bounds)
        {
            var center = self.TransformPoint(bounds.center);
            var points = bounds.GetCorners();

            var result = new Bounds(center, Vector3.zero);
            foreach (var point in points)
                result.Encapsulate(self.TransformPoint(point));
            return result;
        }

        /// <summary>
        /// From world space to local space
        /// </summary>
        public static Bounds InverseTransformBounds(this Transform self, Bounds bounds)
        {
            var points = bounds.GetCorners();

            var result = new Bounds(self.InverseTransformPoint(points[0]), Vector3.zero);
            for (var i = 1; i < 8; i++)
                result.Encapsulate(self.InverseTransformPoint(points[i]));

            return result;
        }

        public static List<Vector3> GetCorners(this Bounds obj, bool includePosition = true)
        {
            var result = new List<Vector3>(8);
            for (var x = -1; x <= 1; x += 2)
            for (var y = -1; y <= 1; y += 2)
            for (var z = -1; z <= 1; z += 2)
                result.Add((includePosition ? obj.center : Vector3.zero)
                           + Vector3.Scale(obj.size / 2, new Vector3(x, y, z)));
            return result;
        }

        /// <summary> Transform center and extents of the given bounds transformed by the transformation matrix passed. </summary>
        public static Bounds ToMatrix(this Bounds bounds, Matrix4x4 matrix)
        {
            var xa = matrix.GetColumn(0) * bounds.min.x;
            var xb = matrix.GetColumn(0) * bounds.max.x;

            var ya = matrix.GetColumn(1) * bounds.min.y;
            var yb = matrix.GetColumn(1) * bounds.max.y;

            var za = matrix.GetColumn(2) * bounds.min.z;
            var zb = matrix.GetColumn(2) * bounds.max.z;

            var col4Pos = matrix.GetColumn(3);

            var min = new Vector3
            {
                x = Mathf.Min(xa.x, xb.x) + Mathf.Min(ya.x, yb.x) + Mathf.Min(za.x, zb.x) + col4Pos.x,
                y = Mathf.Min(xa.y, xb.y) + Mathf.Min(ya.y, yb.y) + Mathf.Min(za.y, zb.y) + col4Pos.y,
                z = Mathf.Min(xa.z, xb.z) + Mathf.Min(ya.z, yb.z) + Mathf.Min(za.z, zb.z) + col4Pos.z
            };
            var max = new Vector3
            {
                x = Mathf.Max(xa.x, xb.x) + Mathf.Max(ya.x, yb.x) + Mathf.Max(za.x, zb.x) + col4Pos.x,
                y = Mathf.Max(xa.y, xb.y) + Mathf.Max(ya.y, yb.y) + Mathf.Max(za.y, zb.y) + col4Pos.y,
                z = Mathf.Max(xa.z, xb.z) + Mathf.Max(ya.z, yb.z) + Mathf.Max(za.z, zb.z) + col4Pos.z
            };

            bounds.SetMinMax(min, max);
            return bounds;
        }
    }
}