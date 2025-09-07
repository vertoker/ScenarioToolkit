using System;
using Newtonsoft.Json;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Запускает сценарий в себе, не ограничен во вложенности и количеству нод.
    /// Нода исполняется от ивентов дочернего плеера и считается выполненной,
    /// если дочерний плеер остановился или закончил своё проигрывание
    /// </summary>
    public interface ISubgraphNode : IScenarioNodeFlow, IVariableEnvironment,
        IModelReflection<SubgraphNodeV6, ISubgraphNode>, IScenarioCompatibilitySubgraphNode
    {
        public SubgraphLoadType LoadType { get; set; }
        public TextAsset Json { get; set; }
        public string StreamingPath { get; set; }
        public string AbsolutePath { get; set; }
        
        [JsonIgnore] public IScenarioModel SubModel { get; }
        [JsonIgnore] public NodeExecutionContext SubContext { get; }
        [JsonIgnore] public ScenarioPlayer SubPlayer { get; } // only host
        
        public event Action<ISubgraphNode> OnSubgraphIsReady;
        
        public void ForceEnd();
    }
}