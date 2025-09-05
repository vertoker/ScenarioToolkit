using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scenario.Core.DataSource;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player.Roles;
using Scenario.Core.Serialization;
using Scenario.Utilities.Extensions;
using UnityEngine;
using Zenject;
using ZLinq;
using ZLinq.Linq;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Scenario.Core.Player
{
    using ActiveNodesType = ValueEnumerable<FromHashSet<IScenarioNodeFlow>, IScenarioNodeFlow>;
    using CompletedNodesType = ValueEnumerable<FromHashSet<IScenarioNodeFlow>, IScenarioNodeFlow>;
    using ActiveStartedNodesType = ValueEnumerable<FromHashSet<IStartNode>, IStartNode>;
    using EndNodesToCompleteType = ValueEnumerable<FromHashSet<IEndNode>, IEndNode>;
    using SubPlayersType = ValueEnumerable<FromHashSet<ScenarioPlayer>, ScenarioPlayer>;
    
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    /// <summary> Ядро, которое проигрывает сценарии </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScenarioPlayer
    {
        public event Action<ScenarioLaunchModel> ScenarioStarted;
        public event Action ScenarioStopped;
        
        /// <summary> Нода сейчас станет активной </summary>
        public event Action<IScenarioNodeFlow> NodeBeforeActivated;
        /// <summary> Нода стала активной </summary>
        public event Action<IScenarioNodeFlow> NodeAfterActivated;
        /// <summary> Нода сейчас станет завершённой </summary>
        public event Action<IScenarioNodeFlow> NodeBeforeCompleted;
        /// <summary> Нода стала завершённой </summary>
        public event Action<IScenarioNodeFlow> NodeAfterCompleted;
        
        /// <summary> Ноды графа в которых происходит ожидание сигналов из шины </summary>
        private readonly HashSet<IScenarioNodeFlow> activeNodes = new();
        /// <summary> Выполненные ноды графа </summary>
        private readonly HashSet<IScenarioNodeFlow> completedNodes = new();
        /// <summary> Ноды, которые надо выполнить </summary>
        private readonly HashSet<IStartNode> activeStartedNodes = new();
        /// <summary> Ноды, которые надо выполнить </summary>
        private readonly HashSet<IEndNode> endNodesToComplete = new();
        /// <summary> Количество next нод, которые найдены, но ещё не активированы </summary>
        private int processedNodes;
        
        public IEnumerable<IScenarioNodeFlow> ActiveNodes => activeNodes;
        public IEnumerable<IScenarioNodeFlow> CompletedNodes => completedNodes;
        public IEnumerable<IStartNode> ActiveStartedNodes => activeStartedNodes;
        public IEnumerable<IEndNode> EndNodesToComplete => endNodesToComplete;
        
        public ActiveNodesType ActiveNodesAVE => activeNodes.AsValueEnumerable();
        public CompletedNodesType CompletedNodesAVE => completedNodes.AsValueEnumerable();
        public ActiveStartedNodesType ActiveStartedNodesAVE => activeStartedNodes.AsValueEnumerable();
        public EndNodesToCompleteType EndNodesToCompleteAVE => endNodesToComplete.AsValueEnumerable();
        
        /// <summary> Запущен сценарий любой (анонимный, из модуля) </summary>
        public event Action<ScenarioPlayer> SubPlayerAdded;
        /// <summary> Все конечные ноды выполнились </summary>
        public event Action<ScenarioPlayer> SubPlayerRemoved;

        /// <summary> Родитель, который им управляет. У Root он null </summary>
        public ScenarioPlayer Parent { get; private set; }
        /// <summary> Родитель, который им управляет. У Root он null </summary>
        private readonly HashSet<ScenarioPlayer> subPlayers = new();
        
        public IEnumerable<ScenarioPlayer> SubPlayers => subPlayers;
        public SubPlayersType SubPlayersAVE => subPlayers.AsValueEnumerable();
        
        public NodeExecutionContext ExecutionContext { get; private set; }
        public IScenarioGraph Graph { get; private set; }
        public IScenarioContext GraphContext { get; private set; }
        public ScenarioLaunchModel LaunchParameters { get; private set; } = new();
        
        public bool IsInitialized => Graph != null;
        public bool IsPlayed { get; private set; }
        public int Hash => GetHashCode();
        
        
        [Inject]
        public ScenarioPlayer(SignalBus bus, ScenarioLoadService loadService, RoleFilterService filterService)
        {
            ExecutionContext = NodeExecutionContext.CreateRoot(bus, loadService, filterService);
            // Контекст в root плеере является subContext от Root контекста
        }
        private ScenarioPlayer(ScenarioPlayer parent)
        {
            Parent = parent;
            LaunchParameters = parent.LaunchParameters;
        }
        // Данные методы были созданы исключительно, чтобы можно было провести инициализацию контекста
        // перед началом сценария, а для контекста нужны данные о самом сценарии
        public void CreateSubExecutionContext(IScenarioGraph newGraph, IScenarioContext newContext)
        {
            if (IsInitialized) return;
            Graph = newGraph;
            GraphContext = newContext;
            ExecutionContext = Parent != null 
                ? Parent.ExecutionContext.CreateSubcontextHost(this) 
                : ExecutionContext.CreateSubcontextHost(this);
        }
        private void ClearSubExecutionContext()
        {
            if (!IsInitialized) return;
            Graph = null;
            GraphContext = null;
            ExecutionContext = Parent != null 
                ? null : ExecutionContext.ClearToRoot();
        }
        
        
        // Для LaunchModel предусмотрено следующее использование: для root - надо, для subPlayers - нет
        public void ForcePlay(IScenarioGraph newGraph, IScenarioContext newContext, ScenarioLaunchModel newParameters = null)
        {
            Stop();
            Play(newGraph, newContext, newParameters);
        }
        public void Play(IScenarioGraph newGraph, IScenarioContext newContext, ScenarioLaunchModel newParameters = null)
        {
            if (IsPlayed) return;
            IsPlayed = true;
            CreateSubExecutionContext(newGraph, newContext);

            if (newParameters != null)
            {
                LaunchParameters = newParameters;
                ExecutionContext.UpdateIdentityHash(LaunchParameters.IdentityHash);
            }
            LaunchParameters ??= new ScenarioLaunchModel();
            
            foreach (var endNode in Graph.NodesValuesAVE.OfType<IEndNode>())
                endNodesToComplete.Add(endNode);
            
            if (LaunchParameters.UseLog)
                Debug.Log($"<b>Scenario Started</b>: (<b>{GetHashCode()}</b>) {LaunchParameters.GetStatusString()}");
            ScenarioStarted?.Invoke(LaunchParameters);

            var startArray = Graph.NodesValuesAVE.OfType<IStartNode>().ToArray();
            activeStartedNodes.AddRange(startArray);
            foreach (var startNode in startArray)
                Activate(startNode);
            TryEndScenario(); // Если ничего не запустилось, то можно смело завершать
        }
        public void Stop()
        {
            if (!IsPlayed) return;
            IsPlayed = false;

            if (activeNodes.Count > 0)
            {
                // Вынужденная мера, так как ноды удаляются при деактивации
                var copyActiveNodes = activeNodes.ToArray();
                foreach (var scenarioNode in copyActiveNodes)
                    scenarioNode.Deactivate(ExecutionContext);
            }
            
            // Необходимо, чтобы принудительно завершать все саб сценарии
            while (subPlayers.Count > 0)
                RemoveSubPlayer(subPlayers.First());
            
            if (LaunchParameters.UseLog)
                Debug.Log($"<b>Scenario Stopped</b>: (<b>{GetHashCode()}</b>) {LaunchParameters.GetStatusString()}");
            ScenarioStopped?.Invoke();
            
            ClearSubExecutionContext();
            LaunchParameters = null;
            
            activeNodes.Clear();
            completedNodes.Clear();
            activeStartedNodes.Clear();
            endNodesToComplete.Clear();
            
            processedNodes = 0;
        }
        
        public void ProgressNextNodes(IScenarioNodeFlow flowNode)
        {
            Deactivate(flowNode);
            if (!flowNode.IsAllowNextProcess()) return;
            if (!IsPlayed) return;

            var nextNodes = Graph.GetOutcomingNodesAVE(flowNode).ToArray();
            // Попытка принудительно завершить сценарий если выполняться нечему и EndNode не сработал
            if (processedNodes == 0 && nextNodes.Length == 0) TryBreakScenario();
            
            if (!IsPlayed) return;
            processedNodes += nextNodes.Length;
            foreach (var nextNode in nextNodes)
            {
                if (!IsPlayed) break;
                if (CanProgress(nextNode))
                {
                    processedNodes--;
                    Activate(nextNode);
                }
            }
        }
        
        /// <summary>
        ///     Нода становится активной:<br />
        ///     - Если это нода действия, выполняет их.<br />
        ///     - Если это нода условий, начинает ожидание ее условий.
        /// </summary>
        public void Activate(IScenarioNodeFlow flowNode)
        {
            completedNodes.Remove(flowNode);
            activeNodes.Add(flowNode);
            
            if (LaunchParameters.UseLog)
                Debug.Log($"<b>{flowNode.GetType().Name} Activated</b>: (<b>{GetHashCode()}</b>) {flowNode.GetStatusString()}");
            
            NodeBeforeActivated?.Invoke(flowNode);
            flowNode.Activate(ExecutionContext);
            NodeAfterActivated?.Invoke(flowNode);
            
            WaitForNodeCompletion(flowNode);
        }
        /// <summary>
        ///     Нода перестает быть активной, добавляется в список завершенных<br /> 
        ///     - Для ноды действия должны быть посланы все компоненты в шину <br />
        ///     - Для ноды условия должны прийти все компоненты, которая она ждёт
        /// </summary>
        public void Deactivate(IScenarioNodeFlow flowNode)
        {
            if (!IsPlayed) return; // Из-за Activate кнопки
            
            activeNodes.Remove(flowNode);
            completedNodes.Add(flowNode);
            
            if (LaunchParameters.UseLog)
                Debug.Log($"<b>{flowNode.GetType().Name} Completed</b>: (<b>{GetHashCode()}</b>) {flowNode.GetStatusString()}");
            
            NodeBeforeCompleted?.Invoke(flowNode);
            flowNode.Deactivate(ExecutionContext);
            NodeAfterCompleted?.Invoke(flowNode);

            TryRemoveSpecificNode(flowNode);
            TryEndScenario();
        }

        private void TryRemoveSpecificNode(IScenarioNodeFlow flowNode)
        {
            if (flowNode is IStartNode startNode)
                activeStartedNodes.Remove(startNode);
            else if (flowNode is IEndNode endNode)
                endNodesToComplete.Remove(endNode);
        }
        private void TryBreakScenario()
        {
            if (activeNodes.Count == 0 && activeStartedNodes.Count == 0)
                Stop();
        }
        private void TryEndScenario()
        {
            // TODO если нету ни одной EndNode, то исполнятся все отдельные деревья StartNode, кроме последнего
            if (endNodesToComplete.Count == 0 && activeStartedNodes.Count == 0)
                Stop();
        }

        private async void WaitForNodeCompletion(IScenarioNodeFlow flowNode)
        {
            try
            {
                await flowNode.WaitForCompletion();
                ProgressNextNodes(flowNode);
            }
            catch (OperationCanceledException)
            {
                // ignored
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        private bool CanProgress(IScenarioNodeFlow flowNode)
        {
            var completeness = Graph.GetIncomingNodesAVE(flowNode)
                .Select(n => completedNodes.Contains(n));
            
            if (flowNode is IScenarioNodeComponents componentsNode)
            {
                if (componentsNode.Any(c => c is UseOr))
                    return completeness.Any(c => c); // or
            }
            
            if (flowNode.ActivationType == ActivationType.OR)
                return completeness.Any(c => c); // or
            
            return completeness.All(c => c); // and
        }
        
        public ScenarioPlayer CreateSubPlayer()
        {
            var subPlayer = new ScenarioPlayer(this);
            subPlayers.Add(subPlayer);
            
            if (LaunchParameters.UseLog)
                Debug.Log($"<b>SubScenario Added</b>: child:<b>{subPlayer.GetHashCode()}</b> parent:<b>{GetHashCode()}</b>");
            SubPlayerAdded?.Invoke(subPlayer);
            
            return subPlayer;
        }
        public void RemoveSubPlayer(ScenarioPlayer subPlayer)
        {
            subPlayer.Stop();
            
            if (LaunchParameters.UseLog)
                Debug.Log($"<b>SubScenario Removed</b>: child:<b>{subPlayer.GetHashCode()}</b> parent:<b>{GetHashCode()}</b>");
            SubPlayerRemoved?.Invoke(subPlayer);
            
            subPlayers.Remove(subPlayer);
        }

        public void SkipActiveNodes()
        {
            if (Graph == null)
                return;

            foreach (var node in activeNodes)
                ProgressNextNodes(node);
        }
        
        public void LogActiveNodes()
        {
            var sb = new StringBuilder();
            sb.Append("<b>================</b>\n");
            sb.Append("<b><color=#FF5A5A>Actions:</color></b>\n");
            sb.Append(string.Join('\n', activeNodes.OfType<IActionNode>().Select(n => n.ToString()))).Append('\n');
            sb.Append("<b><color=#7EA1FF>Conditions:</color></b>\n");
            sb.Append(string.Join('\n', activeNodes.OfType<IConditionNode>().Select(n => n.ToString()))).Append('\n');
            sb.Append("<b>================</b>\n");
            Debug.Log(sb.ToString());
        }
    }
}