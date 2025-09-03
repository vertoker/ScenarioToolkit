using System;
using JetBrains.Annotations;
using NaughtyAttributes;
using Scenario.Core.DataSource;
using Scenario.Core.Scriptables;
using Scenario.Core.Services;
using UnityEngine;
using VRF.DataSources.LocalCache;
using VRF.Identities;
using VRF.Identities.Core;
using VRF.Scenes.Project;
using Zenject;
using Object = UnityEngine.Object;

namespace VRF.Scenario.Services
{
    public class ScenarioModuleLoader
    {
        [Serializable]
        public class ModeParameters
        {
            [SerializeField] private bool stopCurrentScenario = true;
            [SerializeField] private bool forceReloadScene = true;
            // add instance
            
            public bool StopCurrentScenario { get => stopCurrentScenario; set => stopCurrentScenario = value; }
            public bool ForceReloadScene { get => forceReloadScene; set => forceReloadScene = value; }
        }

        [Serializable]
        public class LoadParameters
        {
            [SerializeField] private bool forceReloadScene = true;
            [SerializeField] private string scenePath = string.Empty;
            
            public bool ForceReloadScene { get => forceReloadScene; set => forceReloadScene = value; }
            public string ScenePath { get => scenePath; set => scenePath = value; }
        }

        private readonly LocalCacheDataSource dataSource;
        private readonly ScenesService scenesService;
        [CanBeNull] private readonly IdentityService identityService;
        private readonly ScenarioLauncherService launcherService;

        public ScenarioModuleLoader(LocalCacheDataSource dataSource,
            ScenesService scenesService, 
            [InjectOptional] IdentityService identityService,
            [InjectOptional] ScenarioLauncherService launcherService)
        {
            this.dataSource = dataSource;
            this.scenesService = scenesService;
            this.identityService = identityService;
            this.launcherService = launcherService;
        }

        public void Load(ScenarioModule module, ModeParameters parameters, Object context = null)
        {
            if (!module)
            {
                Debug.LogWarning($"Empty {nameof(ScenarioModule)}, drop", context);
                return;
            }
            
            var model = module.GetModel(identityService?.SelfIdentity);
            dataSource.Save(model);

            if (parameters.StopCurrentScenario)
            {
                launcherService.Stop();
            }

            if (scenesService.CurrentScene.path != module.ScenePath || parameters.ForceReloadScene)
            {
                scenesService.LoadScene(module.ScenePath);
            }
            else if (launcherService == null)
            {
                Debug.LogWarning($"Can't resolve {nameof(ScenarioLauncherService)} on scene {module.ScenePath}", context);
            }
            else
            {
                launcherService.PlayLoadedModel();
            }
        }

        public void Load(string scenePath, bool forceReloadScene, Object context = null)
        {
            // Ужасное форматирование, лучше переписать
            if (scenesService.CurrentScene.path != scenePath || forceReloadScene)
                scenesService.LoadScene(scenePath);
            else if (launcherService == null)
                Debug.LogWarning($"Can't resolve {nameof(ScenarioLauncherService)} on scene {scenePath}", context);
            else launcherService.PlayLoadedModel();
        }
    }
}