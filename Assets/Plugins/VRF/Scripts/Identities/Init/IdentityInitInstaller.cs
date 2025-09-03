using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using VRF.DataSources;
using VRF.DataSources.CommandLine;
using VRF.DataSources.Config;
using VRF.DataSources.Scriptables;
using VRF.Identities.Models;
using VRF.Utilities;
using Zenject;

namespace VRF.Identities.Init
{
    /// <summary>
    /// Инициализатор сетевой роли, работает как в SceneContext, так и в ProjectContext
    /// </summary>
    public class IdentityInitInstaller : MonoInstaller
    {
        [SerializeField] private bool initialize = true;
        [SerializeField] private bool dispose = true;
        
        [Space]
        [SerializeField] private DataSourceType[] identitySourcesEditor = 
            { DataSourceType.LocalCache, DataSourceType.Scriptable, };
        [SerializeField] private DataSourceType[] identitySourcesRuntime = 
            { DataSourceType.LocalCache, DataSourceType.Scriptable, };
        
        [Space]
        [Expandable] [SerializeField, Required] private AuthIdentityConfig authIdentityConfig;
        [SerializeField] private bool bindDefaultIdentityParser = true;
        
        #region Preset Editor
        private bool PresetIsNull => !authIdentityConfig;
        
        [Button, ShowIf(nameof(PresetIsNull))]
        private void CreateDefaultIdentity()
        {
            var all = ScriptableTools.CreatePresetEditor<AuthIdentityConfig>("Assets/Configs/VRF/Auth/", "AuthAll");
            var vr = ScriptableTools.CreatePresetEditor<AuthIdentityConfig>("Assets/Configs/VRF/Auth/", "AuthVR");
            var wasd = ScriptableTools.CreatePresetEditor<AuthIdentityConfig>("Assets/Configs/VRF/Auth/", "AuthWASD");
            var spectator = ScriptableTools.CreatePresetEditor<AuthIdentityConfig>("Assets/Configs/VRF/Auth/", "AuthSpectator");
            
            ScriptableTools.SetModel(all, new AuthIdentityModel("all"));
            ScriptableTools.SetModel(vr, new AuthIdentityModel("vr"));
            ScriptableTools.SetModel(wasd, new AuthIdentityModel("wasd"));
            ScriptableTools.SetModel(spectator, new AuthIdentityModel("spectator"));
            
            ScriptableTools.CreatePresetEditor(ref authIdentityConfig, "Assets/Configs/VRF/Auth/", "AuthAll");
        }
        [ContextMenu(nameof(CreateStreamingConfig))]
        private void CreateStreamingConfig()
        {
            var dataSource = new ConfigDataSource(new ConfigDataSourceSettings());
            dataSource.Save(authIdentityConfig ? authIdentityConfig.GetModel() : new AuthIdentityModel("vr"));
        }
        #endregion
        
        public override void InstallBindings()
        {
            if (bindDefaultIdentityParser)
            {
                var commandLineSource = Container.Resolve<CommandLineDataSource>();
                commandLineSource.Add(new AuthIdentityModelCmdParser());
            }
            if (authIdentityConfig)
            {
                var scriptableSource = Container.Resolve<ScriptableDataSource>();
                scriptableSource.Add(authIdentityConfig);
            }
            
            var identitySources = DataSourceStatic.GetSources(identitySourcesEditor, identitySourcesRuntime);
            
            Container.BindInterfacesAndSelfTo<IdentityLauncher>().AsSingle()
                .WithArguments(identitySources, initialize, dispose);
        }
    }
}