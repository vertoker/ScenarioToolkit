using NaughtyAttributes;
using SimpleUI.Scriptables.Manager;
using UnityEngine;
using VRF.Inventory.Scriptables;
using VRF.Utilities;
using Zenject;

namespace VRF.Inventory.Installers
{
    /// <summary> Проектный установщик для инвентаря </summary>
    public class InventoryProjectInstaller : MonoInstaller
    {
        [SerializeField, Required] private InventoryProjectConfig projectConfig;
        [SerializeField, Required] private InventoryItemMainList poolConfig;

        public InventoryProjectConfig ProjectConfig => projectConfig;
        public InventoryItemMainList PoolConfig => poolConfig;
        
        #region Preset Editor
        private bool ProjectIsNull => !projectConfig;
        private bool PoolIsNull => !poolConfig;
#if UNITY_EDITOR
        [Button, ShowIf(nameof(ProjectIsNull))]
        private void CreateInventoryProjectConfig()
        {
            ScriptableTools.CopyPresetEditor(ref projectConfig, 
                "Assets/Modules/VRF/Configs/Default/Default_InventoryProjectConfig.asset", 
                "Assets/Configs/VRF/Systems/InventoryProjectConfig.asset");
            UnityEditor.EditorUtility.SetDirty(this);
        }
        [Button, ShowIf(nameof(PoolIsNull))]
        private void CreateItemsAllPreset()
        {
            ScriptableTools.CreatePresetEditor(ref poolConfig, "Assets/Configs/VRF/Systems/", "ItemsPool_All");
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        #endregion
        
        public override void InstallBindings()
        {
            Container.EnsureBind(projectConfig);
            Container.EnsureBind(poolConfig);
        }
    }
}