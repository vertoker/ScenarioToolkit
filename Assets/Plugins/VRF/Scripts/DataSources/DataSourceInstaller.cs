using NaughtyAttributes;
using UnityEngine;
using VRF.DataSources.CommandLine;
using VRF.DataSources.CommandLine.Scriptables;
using VRF.DataSources.Config;
using VRF.DataSources.LocalCache;
using VRF.DataSources.Scriptables;
using VRF.Utilities;
using Zenject;

namespace VRF.DataSources
{
    /// <summary> Инсталлер для всех основных источников данных </summary>
    public class DataSourceInstaller : MonoInstaller
    {
        [SerializeField] private ContainerDataSourceSettings containerSettings;
        [Space]
        [SerializeField] private ConfigDataSourceSettings configSettings;
        [SerializeField] private CommandLineDataSourceSettings commandLineSettings;
        [SerializeField] private ScriptableDataSourceSettings scriptableSettings;
        [SerializeField] private LocalCacheDataSourceSettings localCacheSettings;

        public override void InstallBindings()
        {
            var config = new ConfigDataSource(configSettings);
            var commandLine = new CommandLineDataSource(commandLineSettings);
            var scriptable = new ScriptableDataSource(scriptableSettings);
            var localCache = new LocalCacheDataSource(localCacheSettings);

            var container = new ContainerDataSource(containerSettings);
            container.AddDataSources(config, commandLine, scriptable, localCache);
            
            // Non Lazy нужен для того, чтобы создавать бинды внутри контроллеров
            Container.BindInstance(config).AsSingle();
            Container.BindInstance(commandLine).AsSingle();
            Container.BindInstance(scriptable).AsSingle();
            Container.BindInstance(localCache).AsSingle();

            Container.BindInterfacesAndSelfTo<ContainerDataSource>().FromInstance(container).AsSingle();
        }
        
        #region Preset Editor
        private bool PresetIsNull => !commandLineSettings.CommandLineConfig;
        [Button, ShowIf(nameof(PresetIsNull))]
        private void CreateEditorArgs()
        {
            CommandLineArgsConfig argsConfig = null;
            ScriptableTools.CreatePresetEditor(ref argsConfig, "Assets/Configs/VRF/", "EditorArgs");
            commandLineSettings.Set(argsConfig);
        }
        #endregion
    }
}