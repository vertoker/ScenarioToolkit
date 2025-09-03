using NaughtyAttributes;
using UnityEngine;
using VRF.DataSources;
using VRF.DataSources.CommandLine;
using VRF.DataSources.Config;
using VRF.DataSources.Scriptables;
using VRF.Players.Core;
using VRF.Players.Models;
using VRF.Players.Models.Player;
using VRF.Utilities;
using Zenject;

namespace VRF.Players.Init
{
    public class PlayersInitInstaller : MonoInstaller
    {
        [SerializeField] private DataSourceType[] sourcesEditor = 
            { DataSourceType.LocalCache, DataSourceType.Scriptable, };
        [SerializeField] private DataSourceType[] sourcesRuntime = 
            { DataSourceType.LocalCache, DataSourceType.Scriptable, };
        [Space]
        [SerializeField] private Transform spawnPoint;
        [Required, Expandable]
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private bool bindDefaultCmdParser = true;
        
        #region Preset Editor
        private bool PresetIsNull => !playerConfig;
        
        [Button, ShowIf(nameof(PresetIsNull))]
        private void CreateLoadPlayerConfig()
        {
            ScriptableTools.CreatePresetEditor(ref playerConfig, "Assets/Configs/VRF/", "PlayerConfig");
            playerConfig.SetModel(new PlayerModel { PriorityControlMode = PlayerControlModes.VR });
        }
        [ContextMenu(nameof(CreateStreamingConfig))]
        private void CreateStreamingConfig()
        {
            var dataSource = new ConfigDataSource(new ConfigDataSourceSettings());
            dataSource.Save(playerConfig ? playerConfig.GetModel() : new PlayerModel 
                { PriorityControlMode = PlayerControlModes.VR });
        }
        
        //[ShowNativeProperty, ShowIf(nameof(IsPlaying))]
        //private PlayerControlModes CurrentMode => Container.Resolve<PlayersContainer>().CurrentMode;
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void ChangeToVR()
        {
            Container.Resolve<PlayersContainer>().UpdatePlayerMode(PlayerControlModes.VR);
        }
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void ChangeToWASD()
        {
            Container.Resolve<PlayersContainer>().UpdatePlayerMode(PlayerControlModes.WASD);
        }
        #endregion
        
        public override void InstallBindings()
        {
            var commandLineSource = Container.Resolve<CommandLineDataSource>();
            var scriptableSource = Container.Resolve<ScriptableDataSource>();

            if (bindDefaultCmdParser) commandLineSource.Add(new PlayerModelCmdParser());
            if (playerConfig) scriptableSource.Add(playerConfig);
            
            var sources = DataSourceStatic.GetSources(sourcesEditor, sourcesRuntime);
            
            Container.BindInterfacesAndSelfTo<PlayersLauncher>().AsSingle()
                .WithArguments(Container, spawnPoint, sources);
            Container.BindInterfacesAndSelfTo<PlayerBuilderUpdater>().AsSingle();
        }
    }
}