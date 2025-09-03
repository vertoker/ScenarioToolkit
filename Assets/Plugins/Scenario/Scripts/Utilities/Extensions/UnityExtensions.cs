using UnityEngine;

namespace Scenario.Utilities.Extensions
{
    public static class UnityExtensions
    {
        public static void SetLossyScale(this Transform transform, Vector3 scale)
        {
            transform.localScale = Vector3.one; // v1
            var matrix = transform.worldToLocalMatrix;
            matrix.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));

            /*var local = transform.localScale; // v2
            var localInvMatrix = new Matrix4x4
            {
                m00 = local.x, m10 = 0, m20 = 0, m30 = 0,
                m01 = 0, m11 = local.y, m21 = 0, m31 = 0,
                m02 = 0, m12 = 0, m22 = local.z, m32 = 0,
                m03 = 0, m13 = 0, m23 = 0, m33 = 1
            }.inverse;
            matrix *= localInvMatrix;*/
            
            transform.localScale = matrix.MultiplyPoint(scale);
        }
    }
}