using Scenario.Core.DataSource;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Installers;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Core.Scriptables;
using ScenarioToolkit.Core.Serialization;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Core.Services
{
    /// <summary>
    /// Лаунчер плеера внутри контейнера, опционален
    /// </summary>
    public class ScenarioLauncherService : IInitializable
    {
        // private readonly ContainerDataSource dataSource;
        private readonly ScenarioModules modules;
        
        private readonly bool playOnInitialize;
        private readonly ScenarioLaunchConfig launchConfig;
        
        // private readonly DataSourceType[] scenarioSourcesOrder;
        private readonly ScenarioLoadService loadService;
        private readonly ScenarioPlayer scenarioPlayer;

        public ScenarioLauncherService(ScenarioModules modules,
            ScenarioLoadService loadService, ScenarioPlayer scenarioPlayer,
            bool playOnInitialize, [InjectOptional] ScenarioLaunchConfig launchConfig)
        {
            this.modules = modules;
            
            this.loadService = loadService;
            this.scenarioPlayer = scenarioPlayer;

            this.playOnInitialize = playOnInitialize;
            this.launchConfig = launchConfig;
        }

        public void Initialize()
        {
            if (!playOnInitialize) return;
            PlayLoadedModel();
        }
        public void PlayLoadedModel()
        {
            var parameters = InitializeModel();
            if (parameters == null) return;
            PlayModule(parameters);
        }
        private ScenarioLaunchModel InitializeModel()
        {
            // var launchModel = dataSource.Load<ScenarioLaunchModel>(scenarioSourcesOrder);

            /*if (launchModel == null)
            {
                Debug.LogWarning($"Empty <b>{nameof(ScenarioLaunchModel)}</b>, abort initialization");
                return null;
            }*/
            
            return new();
        }

        public void PlayModule(ScenarioLaunchModel parameters)
        {
            if (scenarioPlayer.IsPlayed) return; // assert playable
            
            if (parameters == null)
            {
                Debug.LogWarning($"Empty parameters, abort <b>{nameof(PlayModule)}</b>");
                return;
            }
            
            if (modules == null)
            {
                Debug.LogWarning($"Forget to add modules config to {nameof(ScenarioInstaller)}");
            }

            ScenarioModule module = null;
            
            if (modules) module = modules.FirstOrDefault(parameters.Scenario);
            else Debug.LogWarning($"Forget to add modules config to {nameof(ScenarioInstaller)}");

            if (module != null)
            {
                if (launchConfig) module = launchConfig.FirstOrDefault(parameters.Scenario);
                else Debug.LogWarning($"Empty launchConfig");
            }
            
            if (!module)
            {
                Debug.LogWarning(string.IsNullOrEmpty(parameters.Scenario) 
                    ? $"Couldn't find scenario module with empty model"
                    : $"Couldn't find scenario module with name {parameters.Scenario}");
                return;
            }
            
            Debug.Log($"<color=cyan>Loading module:</color> {module}");
            var currentModel = loadService.LoadModelFromJson(module.ScenarioAsset.text);
            Play(currentModel, parameters);
        }

        public void Play(IScenarioModel currentModel, ScenarioLaunchModel parameters)
        {
            scenarioPlayer.Play(currentModel.Graph, currentModel.Context, parameters);
        }
        public void Stop()
        {
            scenarioPlayer.Stop();
        }
    }
}