using KBCore.Refs;
using NaughtyAttributes;
using UnityEngine;
using VRF.Utilities.Extensions;

namespace VRF.Utils.Identifying
{
    /// <summary>
    /// Реализация IAssetIdentity для MonoBehaviour объекта,
    /// для идентификации ассета считает корневой префаб
    /// </summary>
    public abstract class IdentifiedMonoBehaviour : MonoBehaviour, IAssetIdentity
    {
        [SerializeField, ReadOnly] private int assetHashCode;

        public int AssetHashCode => assetHashCode;
        public int RuntimeHashCode => GetHashCode();

        protected virtual void OnValidate()
        {
            this.ValidateRefs(); // Для работы KBCore.Refs
            if (gameObject.GetAssetHashCode(out var hashCode))
                assetHashCode = hashCode;
        }
    }
}