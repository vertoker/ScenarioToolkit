using NaughtyAttributes;
using UnityEngine;

namespace VRF.Utils.Identifying
{
    /// <summary>
    /// Реализация IAssetIdentity для Scriptable объекта
    /// </summary>
    public abstract class IdentifiedScriptableObject : ScriptableObject
    {
        [SerializeField, ReadOnly] private int assetHashCode;

        public int AssetHashCode => assetHashCode;
        public int RuntimeHashCode => GetHashCode();
    }
}