using UnityEngine;
using VRF.Components.Items.Views;

namespace VRF.Inventory.Scriptables
{
    /// <summary>
    /// Глобальные настройки инвентаря
    /// </summary>
    [CreateAssetMenu(fileName  = nameof(InventoryProjectConfig), menuName = "VRF/Inventory/" + nameof(InventoryProjectConfig))]
    public class InventoryProjectConfig : ScriptableObject
    {
        [SerializeField] private float inventoryScaleCoefficient = 0.1f;
        [SerializeField] private ItemViewUI viewUI;

        public float InventoryScaleCoefficient => inventoryScaleCoefficient;
        public ItemViewUI ViewUI => viewUI;
    }
}