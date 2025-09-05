using UnityEngine;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы с LayerMask
    /// </summary>
    public static class VrfLayerMaskExtensions
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}