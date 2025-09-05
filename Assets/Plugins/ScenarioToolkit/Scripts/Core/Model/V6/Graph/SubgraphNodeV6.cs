using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player;
using Scenario.Utilities.Extensions;
using UnityEngine;

// Previous: SubgraphNodeV5
//  Current: SubgraphNodeV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class SubgraphNodeV6 : ScenarioNodeFlowV6, ISubgraphNode
    {
        public SubgraphLoadType LoadType { get; set; } = SubgraphLoadType.TextAsset;
        public TextAsset Json { get; set; } = null;
        public string StreamingPath { get; set; } = null;
        public string AbsolutePath { get; set; } = null;
        
        public Dictionary<string, ObjectTyped> Variables { get; set; } = new();
        
        [JsonIgnore] public IScenarioModel SubModel { get; private set; }
        [JsonIgnore] public NodeExecutionContext SubContext { get; private set; }
        [JsonIgnore] public ScenarioPlayer SubPlayer { get; private set; } // only host
        [JsonIgnore] private readonly List<UniTaskCompletionSource> completionSources = new();
        [JsonIgnore] private bool isCompleted = true;
        public event Action<ISubgraphNode> OnSubgraphIsReady;
        
        public override void Activate(NodeExecutionContext context)
        {
            base.Activate(context);
            
            if (!this.TryLoadJson(out var json)) return;
            
            SubModel = context.LoadService.LoadModelFromJson(json);
            
            SubContext = context.IsHost ? HostActivate(context, SubModel) 
                : context.CreateSubcontextClient(SubModel);
            
            // Тут создаётся NVE для текущего саб контекста
            // Более старые данные должны быть выше в выдаче, поэтому
            SubContext.Variables.MixVariables(this); // Сначала текущий NVE
            SubContext.Variables.MixVariables(context.Variables); // Потом все родительские
            // Это своеобразная рекурсия, поэтому родительские уже правильно отсортированы
            
            OnSubgraphIsReady?.Invoke(this);
            if (context.IsHost) SubPlayer.Play(SubModel.Graph, SubModel.Context);
        }
        public override void Deactivate(NodeExecutionContext context)
        {
            if (Json)
            {
                if (context.IsHost) HostDeactivate(context);

                SubModel = null;
                SubContext = null;
                SubPlayer = null;
            }
            
            base.Deactivate(context);
        }
        
        private NodeExecutionContext HostActivate(NodeExecutionContext context, IScenarioModel subModel)
        {
            isCompleted = false;
            
            // ReSharper disable once PossibleNullReferenceException
            SubPlayer = context.Player.CreateSubPlayer();
            SubPlayer.CreateSubExecutionContext(subModel.Graph, subModel.Context);
            SubPlayer.ScenarioStopped += ForceEnd;
            return SubPlayer.ExecutionContext;
        }
        private void HostDeactivate(NodeExecutionContext context)
        {
            if (SubPlayer != null)
            {
                SubPlayer.ScenarioStopped -= ForceEnd;
                // ReSharper disable once PossibleNullReferenceException
                context.Player.RemoveSubPlayer(SubPlayer);
            }

            ForceEnd(); // TrySetCanceled may be here
        }
        public override Task WaitForCompletion()
        {
            if (isCompleted) return Task.CompletedTask;
            var completionSource = new UniTaskCompletionSource();
            completionSources.Add(completionSource);
            return completionSource.Task.AsTask();
        }
        
        public void ForceEnd()
        {
            if (!isCompleted)
            {
                isCompleted = true;
                foreach (var completionSource in completionSources)
                    completionSource.TrySetResult();
                completionSources.Clear();
            }
        }
    }
}