using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Scenario.Base.Components.Conditions;
using Scenario.Core.DataSource;
using Scenario.Core.Player;
using Scenario.Core.Scriptables;
using Zenject;

namespace VRF.Scenario.Services
{
    public class ScenarioQueueService : IInitializable, IDisposable
    {
        private readonly ScenarioModuleLoader.ModeParameters parameters;
        private readonly ScenarioModuleLoader loader;
        private readonly ScenarioPlayer player;

        private readonly Queue<ScenarioModule> modulesQueue = new();
        // TODO добавить фильтр на другие сценарии вне этой очереди, пока что тут не очень
        //private readonly Dictionary<ScenarioModule, int> _modulesDict = new();

        private bool active;
        
        public ScenarioQueueService(ScenarioModuleLoader loader, 
            ScenarioPlayer player, ScenarioModuleLoader.ModeParameters parameters)
        {
            this.parameters = parameters;
            this.loader = loader;
            this.player = player;
        }
        
        public void Initialize()
        {
            player.ScenarioStarted += ScenarioStarted;
            player.ScenarioStopped += ScenarioStopped;
        }
        public void Dispose()
        {
            player.ScenarioStarted -= ScenarioStarted;
            player.ScenarioStopped -= ScenarioStopped;
        }

        public void PlayModules(IEnumerable<ScenarioModule> modules)
        {
            foreach (var module in modules)
                modulesQueue.Enqueue(module);

            if (!active)
                loader.Load(modulesQueue.Dequeue(), parameters);
        }
        
        private void ScenarioStarted([CanBeNull] ScenarioLaunchModel launchParameters)
        {
            active = true;
        }
        private void ScenarioStopped()
        {
            active = false;
            if (modulesQueue.TryDequeue(out var module)) 
                loader.Load(module, parameters);
        }
    }
}