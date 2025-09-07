using UnityEngine;

namespace VRF.Utilities
{
    /// <summary>
    /// Класс с различными математическими методами
    /// </summary>
    public static class VrfMath
    {
        /// <summary>
        /// Вращает вектор на указанную величину и определяет его радиус
        /// </summary>
        /// <param name="angleRad">Угол в радианах, на который нужно повернуть вектор</param>
        /// <param name="radius">Радиус финального вектора</param>
        /// <returns>Вектор, повернутый на andgeRad радиан и имеющий радиус radius</returns>
        public static Vector3 RotateVector(Vector3 vector, float angleRad, float radius)
        {
            var direction = vector.IsZero() ? Quaternion.identity : Quaternion.LookRotation(vector);
            return direction * new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * radius;
        }
        
        public static int ArrayOverlap(int value, int length) 
            => value == length ? 0 : value;
    }
}