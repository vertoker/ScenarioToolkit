using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Core.Player.Roles;
using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Core.Serialization;
using ScenarioToolkit.Core.Services;
using ScenarioToolkit.Core.World;
using ScenarioToolkit.Shared.VRF.Utilities.VRF;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Core.Installers
{
    public class ScenarioInstaller : MonoInstaller
    {
        [SerializeField] private ScenarioModules modulesConfig;
        [SerializeField] private ScenarioSceneProvider sceneProvider;
        [SerializeField] private ScenarioSerializationSettings serializationSettings;
        

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