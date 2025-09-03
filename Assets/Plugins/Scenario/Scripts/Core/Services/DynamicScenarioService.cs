using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scenario.Core.DataSource;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player;
using Scenario.Core.Serialization;
using Scenario.Core.World;
using UnityEngine;
using VRF.Identities.Core;
using Zenject;

namespace Scenario.Core.Services
{
    /// <summary>
    /// Утилиты и методы для загрузки динамических сценариев любого формата (StreamingAssets/).
    /// Во всех методах отсутствует валидация сценариев, все ошибки сценария на вашей совести
    /// </summary>
    public class DynamicScenarioService
    {
        private readonly ScenarioLauncherService launcherService;
        private readonly ScenarioLoadService loadService;
        private readonly IdentityService identityService;

        public DynamicScenarioService(ScenarioLauncherService launcherService, 
            ScenarioLoadService loadService, IdentityService identityService)
        {
            this.launcherService = launcherService;
            this.loadService = loadService;
            this.identityService = identityService;
        }
        
        public void LoadPlay(string scenarioPath = "Scenario/scenario.json", 
            bool useNetwork = true, bool useLog = false, bool useIdentity = false, bool force = false)
        {
            var fileInfo = new FileInfo(ToFullPath(scenarioPath));
            var data = LoadScenario(fileInfo);
            var model = new ScenarioLaunchModel { Scenario = fileInfo.Name, UseNetwork = useNetwork, UseLog = useLog,
                IdentityHash = useIdentity ? identityService.SelfIdentity.AssetHashCode : 0 };
            Play(data, model, force);
        }
        public void Play(string scenarioJson, ScenarioLaunchModel model = null, bool force = false)
        {
            if (force) Stop();
            model ??= new ScenarioLaunchModel();
            var currentModel = loadService.LoadModelFromJson(scenarioJson);
            
            Debug.Log($"<color=cyan>Play Json in DiContainer</color> size={scenarioJson.Length} name={model.Scenario}");
            launcherService.Play(currentModel, model);
        }
        public void Stop()
        {
            launcherService.Stop();
        }
        
        public void LoadPlay(ScenarioPlayer player, string scenarioPath = "Scenario/scenario.json", 
            bool useNetwork = true, bool useLog = false, bool useIdentity = false, bool force = false)
        {
            var fileInfo = new FileInfo(ToFullPath(scenarioPath));
            var data = LoadScenario(fileInfo);
            var model = new ScenarioLaunchModel { Scenario = fileInfo.Name, UseNetwork = useNetwork, UseLog = useLog,
                IdentityHash = useIdentity ? identityService.SelfIdentity.AssetHashCode : 0 };
            Play(player, data, model, force);
        }
        public void Play(ScenarioPlayer player, string scenarioJson, ScenarioLaunchModel model = null, bool force = false)
        {
            if (force) Stop(player);
            model ??= new ScenarioLaunchModel();
            var currentModel = loadService.LoadModelFromJson(scenarioJson);
            
            Debug.Log($"<color=cyan>Play Json in Player</color> size={scenarioJson.Length} name={model.Scenario}");
            player.Play(currentModel.Graph, currentModel.Context, model);
        }
        public void Stop(ScenarioPlayer player)
        {
            player.Stop();
        }
        
        public void LoadPlay(ScenarioLauncherInstance instance, string scenarioPath = "Scenario/scenario.json", 
            bool useNetwork = true, bool useLog = false, bool useIdentity = false, bool force = false)
        {
            var fileInfo = new FileInfo(ToFullPath(scenarioPath));
            var data = LoadScenario(fileInfo);
            var model = new ScenarioLaunchModel { Scenario = fileInfo.Name, UseNetwork = useNetwork, UseLog = useLog,
                IdentityHash = useIdentity ? identityService.SelfIdentity.AssetHashCode : 0 };
            Play(instance, data, model, force);
        }
        public void Play(ScenarioLauncherInstance instance, string scenarioJson, ScenarioLaunchModel model = null, bool force = false)
        {
            if (force) Stop(instance);
            if (model == null && !instance.TryGetLaunchModel(out model))
                model = new ScenarioLaunchModel();
            var currentModel = loadService.LoadModelFromJson(scenarioJson);
            
            Debug.Log($"<color=cyan>Play Json in Instance</color> size={scenarioJson.Length} name={model.Scenario}");
            instance.Player.Play(currentModel.Graph, currentModel.Context, model);
        }
        public void Stop(ScenarioLauncherInstance instance)
        {
            instance.Player.Stop();
        }
        
        // Utility
        
        public static string ToFullPath(string relativePath) => Application.streamingAssetsPath + '/' + relativePath;
        
        public static string LoadScenario(string scenarioPath = "Scenario/scenario.json")
            => File.ReadAllText(ToFullPath(scenarioPath));
        public static string LoadScenario(FileInfo fileScenario)
            => File.ReadAllText(fileScenario.FullName);
        
        public static IEnumerable<FileInfo> EnumerateScenarios(string streamingFolder = "Scenario/", 
            SearchOption searchOption = SearchOption.TopDirectoryOnly, string searchPattern = "*.json")
        {
            return Directory
                .EnumerateFiles(ToFullPath(streamingFolder), searchPattern, searchOption)
                .Select(p => new FileInfo(p));
        }
    }
}