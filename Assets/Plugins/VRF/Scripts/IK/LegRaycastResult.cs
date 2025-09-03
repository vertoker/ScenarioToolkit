using UnityEngine;

namespace VRF.IK
{
    /// <summary>
    /// Информация о результате лучей ног
    /// </summary>
    public struct LegRaycastResult
    {
        /// <summary> Точка на поверхности </summary>
        public Vector2 PointXZ;
        /// <summary> Глобальная высота ноги </summary>
        public float PointY;
        /// <summary> Нормаль объекта, которого нога касается </summary>
        private readonly Vector3 normal;

        public LegRaycastResult(Vector3 legRaycastOrigin, float maxDistance, LayerMask mask)
        {
            var ray = new Ray(legRaycastOrigin, Vector3.down);
            Vector3 point;
            
            if (Physics.Raycast(ray, out var hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
            {
                point = hit.point;
                PointXZ = new Vector2(point.x, point.z);
                PointY = point.y;
                normal = hit.normal;
                return;
            }
            
            point = ray.GetPoint(maxDistance);
            PointXZ = new Vector2(point.x, point.z);
            PointY = point.y;
            normal = Vector3.up;
        }
        
        /// <summary> Итоговая точки ноги, учитывает высчитанную высоту </summary>
        public Vector3 GetPoint()
        {
            return new Vector3(PointXZ.x, PointY, PointXZ.y);
        }
        
        /// <summary> Взгляд ступней, то есть всегда наверх относительно поверхности </summary>
        public Quaternion LookFeet(Transform player)
        {
            return Quaternion.LookRotation(player.forward, normal);
        }
    }
}