using NaughtyAttributes;
using Scenario.Core.Player;
using Scenario.Core.Player.Roles;
using Scenario.Core.Scriptables;
using Scenario.Core.Serialization;
using Scenario.Core.Services;
using Scenario.Core.World;
using UnityEngine;
using VRF.Utilities;
using Zenject;

namespace Scenario.Core.Installers
{
    public class ScenarioInstaller : MonoInstaller
    {
        [SerializeField] private ScenarioModules modulesConfig;
        [SerializeField] private ScenarioSceneProvider sceneProvider;
        [SerializeField] private ScenarioSerializationSettings serializationSettings;
        
        #region Preset Editor
        private bool PresetIsNull => !modulesConfig;
        [Button, ShowIf(nameof(PresetIsNull))]
        private void CreateScenarioModules()
        {
            ScriptableTools.CreatePresetEditor(ref modulesConfig, "Assets/Configs/Scenario/", "ScenarioModules");
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endregion

        public override void InstallBindings()
        {
            Container.Bind<ScenarioSceneProvider>().FromInstance(sceneProvider);
            
            // Json/Loading
            Container.BindInstance(modulesConfig);
            Container.BindInstance(serializationSettings);
            Container.Bind<ScenarioSerializationService>().AsSingle();
            Container.Bind<ScenarioLoadService>().AsSingle();
            
            // Playing
            Container.Bind<RoleFilterService>().AsSingle();
            Container.Bind<ScenarioPlayer>().AsSingle();
            
            // Editor Reflection
            EditorDiContainerService.OnUpdateContainer(Container);
        }
        private void OnDestroy()
        {
            EditorDiContainerService.OnRemoveContainer();
        }
    }
}