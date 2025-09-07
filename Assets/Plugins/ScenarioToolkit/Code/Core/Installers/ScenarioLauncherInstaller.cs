using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Core.Services;
using ScenarioToolkit.Core.World;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Core.Installers
{
    public class ScenarioLauncherInstaller : MonoInstaller, IScenarioWorldID
    {
        [SerializeField] private bool playOnInitialize = true;
        [ContextMenuItem(nameof(RefreshID), nameof(EditorRefreshID))]
        // ReSharper disable once InconsistentNaming
        [SerializeField] private string ID;
        
        // [Space]
        // [SerializeField] private DataSourceType[] sourcesEditor = DataSourceStatic.DefaultEditor;
        // [SerializeField] private DataSourceType[] sourcesRuntime = DataSourceStatic.DefaultRuntime;
        [Space]
        [SerializeField] private ScenarioLaunchConfig launchConfig;

        public ScenarioLaunchConfig LaunchConfig
        {
            get => launchConfig;
            set => launchConfig = value;
        }
        
        public void Reset() => RefreshID();
        
        public override void InstallBindings()
        {
            /*var commandLineSource = Container.Resolve<CommandLineDataSource>();
            var scriptableSource = Container.Resolve<ScriptableDataSource>();
            var disposeService = Container.Resolve<DataSourceDisposeService>();

            if (bindDefaultCommandLineParser)
            {
                var defaultParser = new ScenarioLaunchModelCmdParser();
                commandLineSource.Add(defaultParser);
                disposeService.Add(commandLineSource.GetRemovePromise(defaultParser));
            }
            if (launchConfig)
            {
                scriptableSource.Add(launchConfig);
                disposeService.Add(scriptableSource.GetRemovePromise(launchConfig));
            }
            
            var sources = DataSourceStatic.GetSources(sourcesEditor, sourcesRuntime);*/
            Container.BindInterfacesAndSelfTo<ScenarioLauncherService>().AsSingle()
                .WithArguments(playOnInitialize, launchConfig);
            
            Container.BindInterfacesAndSelfTo<DynamicScenarioService>().AsSingle();
        }

        public string GetID() => ID;
        public void SetID(string newID) => ID = newID;
        
        [ContextMenu(nameof(RefreshID))]
        public void EditorRefreshID() => IScenarioWorldID.EditorRefreshID(this, this);
        public void RefreshID() => IScenarioWorldID.RefreshID(this, this);
    }
}