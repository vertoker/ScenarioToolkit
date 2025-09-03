using System.Linq;
using NaughtyAttributes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VRF.Inventory.Scriptables
{
    /// <summary>
    /// Специальное расширение для списка предметов, предназначенное для сбора
    /// всей информации о всех предметах
    /// </summary>
    [CreateAssetMenu(fileName  = nameof(InventoryItemMainList), menuName = "VRF/Inventory/" + nameof(InventoryItemMainList))]
    public class InventoryItemMainList : InventoryItemConfigList
    {
#if UNITY_EDITOR
        [Button]
        private void SearchItems()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(InventoryItemConfig)}");
            var configs = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<InventoryItemConfig>)
                .ToList();
            UpdateItems(configs);
        }
#endif
    }
}