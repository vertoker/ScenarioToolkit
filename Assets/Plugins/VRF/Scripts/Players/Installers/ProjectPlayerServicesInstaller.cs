using NaughtyAttributes;
using UnityEngine;
using VRF.Players.Scriptables;
using VRF.Players.Services;
using VRF.Players.Services.Settings;
using VRF.Players.Services.Views;
using VRF.Utilities;
using Zenject;

namespace VRF.Players.Installers
{
    public class ProjectPlayerServicesInstaller : MonoInstaller
    {
        [SerializeField, Required] private ProjectPlayerSpawnConfig projectSpawnConfig;
        
        #region Preset Editor
        private bool ProjectSpawnConfigIsNull => !projectSpawnConfig;
        
        [Button, ShowIf(nameof(ProjectSpawnConfigIsNull))]
        private void CreateProjectSpawnConfig()
        {
            ScriptableTools.CopyPresetEditor(ref projectSpawnConfig, 
                "Assets/Modules/VRF/Configs/Default/Default_ProjectPlayerSpawnConfig.asset", 
                "Assets/Configs/VRF/ProjectPlayerSpawnConfig.asset");
        }
        #endregion
        
        public override void InstallBindings()
        {
            Container.EnsureBind(projectSpawnConfig);
            
            // Views
            Container.Bind<ViewsSpawnedContainer>().AsSingle();
            Container.Bind<ProjectViewSpawnerService>().AsSingle();
            
            // Services
            Container.BindInterfacesAndSelfTo<MouseSensitivityParameter>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputAudioDeviceParameter>().AsSingle();
            Container.BindInterfacesAndSelfTo<CursorHideoutService>().AsSingle();
        }
    }
}