using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using VRF.Components.Items.Views;

namespace VRF.Inventory.Scriptables
{
    /// <summary>
    /// Вся информация о предмете, в том числе исходные объекты для создания
    /// </summary>
    [CreateAssetMenu(fileName = nameof(InventoryItemConfig), menuName = "VRF/Inventory/" + nameof(InventoryItemConfig))]
    public class InventoryItemConfig : ScriptableObject
    {
        [SerializeField] private bool active = true;
        [SerializeField] private string itemName;
        [SerializeField] private ItemView itemView;

        [Header("Life Cycle")]
        [SerializeField] private bool limitInstances = true;
        [SerializeField, ShowIf(nameof(limitInstances)), Min(1)] private int instancesCount = 1;
        [SerializeField] private bool disableOnRelease = false;
        [SerializeField] private bool overridePoolSize = false;
        [SerializeField, ShowIf(nameof(overridePoolSize)), Min(1)] private int poolSize = 1;
        
        [Header("Inventory Displaying")]
        [SerializeField] private Vector3 positionOffset = Vector3.zero;
        [SerializeField] private Vector3 localRotationOffset = Vector3.zero;
        
#if UNITY_EDITOR
        [SerializeField, HideInInspector] private List<InventoryItemConfigList> configsUsage;
#endif

        /// <summary> Фильтрует все объекты в списке зарегистрированных в игре </summary>
        public bool Active => active;
        public string ItemName => itemName;
        public ItemView ItemView => itemView;
        
        public bool LimitInstances => limitInstances;
        public int InstancesCount => instancesCount;
        public bool DisableOnRelease => disableOnRelease;
        public bool OverridePoolSize => overridePoolSize;
        public int PoolSize => poolSize;
        
        public Vector3 PositionOffset => positionOffset;
        public Vector3 LocalRotationOffset => localRotationOffset;

        private void OnValidate()
        {
            UpdateUsages();
        }
        
        private void UpdateUsages()
        {
#if UNITY_EDITOR
            var items = configsUsage.ToArray();
            foreach (var sample in items)
            {
                if (sample != null)
                    sample.UpdateActiveItems();
            }
#endif
        }

#if UNITY_EDITOR
        public void AddUsage(InventoryItemConfigList config)
        {
            if (!configsUsage.Contains(config))
                configsUsage.Add(config);
        }
        public void RemoveUsage(InventoryItemConfigList config) => configsUsage.Remove(config);
#endif

    }
}