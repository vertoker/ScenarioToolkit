using NaughtyAttributes;
using UnityEngine;
using VRF.Inventory.Core;
using VRF.Inventory.Equipment;
using VRF.Inventory.Scriptables;
using VRF.Utilities;
using Zenject;

namespace VRF.Inventory.Installers
{
    /// <summary> Установщик сцены для инвентаря </summary>
    public class InventorySceneInstaller : MonoInstaller
    {
        [Expandable]
        [SerializeField] private InventoryItemConfigList config;
        
        public InventoryItemConfigList Config => config;
        
        #region Preset Editor
        private bool PresetIsNull => !config;
        #if UNITY_EDITOR
        [Button, ShowIf(nameof(PresetIsNull))]
        private void CreateItemsInventoryPreset()
        {
            ScriptableTools.CreatePresetEditor(ref config, "Assets/Configs/VRF/Systems/", "ItemsInventory");
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
        #endregion

        public override void InstallBindings()
        {
            Container.EnsureBind(config);
            // Пул полностью локальный для сцены и не может находиться в ProjectContext
            Container.BindInterfacesAndSelfTo<ItemContainerPool>().AsSingle().WithArguments(transform);

            Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryDirectHandController>().AsSingle();
            Container.BindInterfacesAndSelfTo<ActiveItemsContainer>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<HeadEquipmentService>().AsSingle();
        }
    }
}