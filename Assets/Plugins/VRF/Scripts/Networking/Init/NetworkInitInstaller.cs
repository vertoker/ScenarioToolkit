using System;
using Mirror;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using VRF.DataSources;
using VRF.DataSources.CommandLine;
using VRF.DataSources.Config;
using VRF.DataSources.Scriptables;
using VRF.Networking.Models;
using VRF.Utilities;
using Zenject;

namespace VRF.Networking.Init
{
    /// <summary>
    /// Инициализатор сети, работает как в SceneContext, так и в ProjectContext
    /// </summary>
    public class NetworkInitInstaller : MonoInstaller
    {
        [SerializeField] private bool initialize = true;
        [SerializeField] private bool dispose = true;
        
        [Space]
        [SerializeField] private DataSourceType[] networkSourcesEditor = 
            { DataSourceType.LocalCache, DataSourceType.Scriptable, };
        [SerializeField] private DataSourceType[] networkSourcesRuntime = 
            { DataSourceType.LocalCache, DataSourceType.Scriptable, };
        
        [Space]
        [Expandable] [SerializeField] private NetConfig customNetConfig;
        [SerializeField] private bool bindDefaultNetParser = true;
        
        #region Preset Editor
        private bool CreateScriptable => !customNetConfig;
#if UNITY_EDITOR
        [Button, ShowIf(nameof(CreateScriptable))]
        private void CreateServer()
        {
            CreateDefaultConfigs();
            ScriptableTools.CreatePresetEditor(ref customNetConfig, "Assets/Configs/VRF/Net/", "NetServer");
            UnityEditor.EditorUtility.SetDirty(this);
        }
        [Button, ShowIf(nameof(CreateScriptable))]
        private void CreateClient()
        {
            CreateDefaultConfigs();
            ScriptableTools.CreatePresetEditor(ref customNetConfig, "Assets/Configs/VRF/Net/", "NetClient");
            UnityEditor.EditorUtility.SetDirty(this);
        }
        [Button, ShowIf(nameof(CreateScriptable))]
        private void CreateHost()
        {
            CreateDefaultConfigs();
            ScriptableTools.CreatePresetEditor(ref customNetConfig, "Assets/Configs/VRF/Net/", "NetHost");
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
        private void CreateDefaultConfigs()
        {
            var server = ScriptableTools.CreatePresetEditor<NetConfig>("Assets/Configs/VRF/Net/", "NetServer");
            var client = ScriptableTools.CreatePresetEditor<NetConfig>("Assets/Configs/VRF/Net/", "NetClient");
            var host = ScriptableTools.CreatePresetEditor<NetConfig>("Assets/Configs/VRF/Net/", "NetHost");
            
            ScriptableTools.SetModel(server, new NetModel 
                { NetMode = NetworkManagerMode.ServerOnly, Address = "localhost", Port = 7777, });
            ScriptableTools.SetModel(client, new NetModel 
                { NetMode = NetworkManagerMode.ClientOnly, Address = "192.168.0.1", Port = 7777, });
            ScriptableTools.SetModel(host, new NetModel 
                { NetMode = NetworkManagerMode.Host, Address = "localhost", Port = 7777, });
        }
        [ContextMenu(nameof(CreateStreamingConfig))]
        private void CreateStreamingConfig()
        {
            var dataSource = new ConfigDataSource(new ConfigDataSourceSettings());
            dataSource.Save(customNetConfig ? customNetConfig.GetModel() : new NetModel
            {
                NetMode = NetworkManagerMode.Host,
                Address = "localhost",
                Port = 7777,
            });
        }
        #endregion
        
        public override void InstallBindings()
        {
            if (bindDefaultNetParser)
            {
                var commandLineSource = Container.Resolve<CommandLineDataSource>();
                commandLineSource.Add(new NetModelCmdParser());
            }
            if (customNetConfig)
            {
                var scriptableSource = Container.Resolve<ScriptableDataSource>();
                scriptableSource.Add(customNetConfig);
            }
            
            var networkSources = DataSourceStatic.GetSources(networkSourcesEditor, networkSourcesRuntime);
            
            Container.BindInterfacesAndSelfTo<NetworkLauncher>().AsSingle()
                .WithArguments(networkSources, initialize, dispose);
        }
    }
}