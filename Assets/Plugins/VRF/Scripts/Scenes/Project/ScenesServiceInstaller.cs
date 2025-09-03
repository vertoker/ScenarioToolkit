using NaughtyAttributes;
using UnityEngine;
using VRF.Scenes.Scriptables;
using VRF.Utilities;
using Zenject;

namespace VRF.Scenes.Project
{
    public class ScenesServiceInstaller : MonoInstaller
    {
        [SerializeField, Required] private ClientScenesConfig scenesConfig;

        public ClientScenesConfig ScenesConfig => scenesConfig;
        
        #region Preset Editor
        private bool PresetIsNull => !scenesConfig;
#if UNITY_EDITOR
        [Button, ShowIf(nameof(PresetIsNull))]
        private void CreateScenePreset()
        {
            ScriptableTools.CreatePresetEditor(ref scenesConfig, "Assets/Configs/VRF/", "ClientScenes");
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        #endregion

        public override void InstallBindings()
        {
            var config = Container.EnsureBind(scenesConfig);
            
            if (!config) Debug.LogWarning($"Couldn't resolve <b>{nameof(ClientScenesConfig)}</b>, add to the ProjectContext");
            
            Container.BindInterfacesAndSelfTo<ScenesService>().AsSingle();
        }
    }
}