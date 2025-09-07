using UnityEngine;

namespace ScenarioToolkit.Shared.VRF
{
    /// <summary>
    /// Реализация IAssetIdentity для Scriptable объекта
    /// </summary>
    public abstract class IdentifiedScriptableObject : ScriptableObject
    {
        [SerializeField] private int assetHashCode;

        public int AssetHashCode => assetHashCode;
        public int RuntimeHashCode => GetHashCode();
    }
}