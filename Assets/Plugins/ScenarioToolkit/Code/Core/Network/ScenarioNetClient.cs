// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using Mirror;
// using Scenario.Core.Model.Interfaces;
// using Scenario.Core.Player;
// using Scenario.Core.Player.Roles;
// using Scenario.Core.Scriptables;
// using Scenario.Core.Serialization;
// using UnityEngine;
// using VRF.Identities;
// using VRF.Identities.Core;
// using VRF.Networking.Core;
// using Zenject;
//
// namespace Scenario.Core.Network
// {
//     /// <summary>
//     /// Основной класс клиента, принимает, отправляет и запускает обработку
//     /// всех нод, которыми оперирует сервер. Является Node Processor наравне с Player,
//     /// только проигрывание нод вызывает сам сервер
//     /// </summary>
//     public class ScenarioNetClient : BaseScenarioNet // NetClient
//     {
//         private readonly ScenarioModules modules;
//         private readonly ScenarioLoadService loadService;
//         private readonly NodeExecutionContext rootExecutionContext;
//
//         private readonly struct ClientGraph
//         {
//             public readonly int GraphHash;
//             public readonly string ScenarioIdentifier;
//             public readonly bool UseLog;
//             
//             public readonly IScenarioModel Model;
//             public readonly NodeExecutionContext ExecutionContext;
//
//             public ClientGraph(ScenarioStartedMessage message, IScenarioModel model, NodeExecutionContext executionContext)
//             {
//                 GraphHash = message.PlayerHash;
//                 ScenarioIdentifier = message.ScenarioIdentifier;
//                 UseLog = message.UseLog;
//                 
//                 Model = model;
//                 ExecutionContext = executionContext;
//             }
//         }
//         
//         private readonly Dictionary<int, ClientGraph> clientGraphs = new();
//         private readonly Dictionary<int, ISubgraphNode> subgraphsCache = new();
//         
//         public ScenarioNetClient(ScenarioModules modules, 
//             ScenarioLoadService loadService, SignalBus bus, 
//             RoleFilterService roleFilterService,
//             [InjectOptional] VRFNetworkManager netManager,
//             [InjectOptional] IdentityService identityService) 
//             : base(netManager)
//         {
//             this.modules = modules;
//             this.loadService = loadService;
//             var identityHash = identityService != null && identityService.SelfIdentity 
//                 ? identityService.SelfIdentity.AssetHashCode : 0;
//             // На клиенте рут создаётся сразу же при создании
//             rootExecutionContext = NodeExecutionContext.CreateRoot(bus, loadService, roleFilterService);
//             rootExecutionContext.UpdateIdentityHash(identityHash);
//         }
//         
//         /// <summary> Только Client, для Host и Server это недоступно </summary>
//         protected override bool GetNetActiveStatus() => NetworkClient.active && !NetworkServer.active;
//
//         protected override void InitializeImpl()
//         {
//             //Debug.Log($"Start NetScenario Client");
//             
//             NetManager.RegisterClientMessage<ScenarioStartedMessage>(ScenarioStartedMessage);
//             NetManager.RegisterClientMessage<ScenarioStoppedMessage>(ScenarioStoppedMessage);
//             NetManager.RegisterClientMessage<SubgraphActivateMessage>(SubgraphActivateMessage);
//             NetManager.RegisterClientMessage<SubgraphCompleteMessage>(SubgraphCompleteMessage);
//             
//             NetManager.RegisterClientMessage<ActionMessage>(ActionMessage);
//             NetManager.RegisterClientMessage<ConditionActivateMessage>(ConditionStartedMessage);
//             NetManager.RegisterClientMessage<ConditionCompleteMessage>(ConditionCompleteMessage);
//             NetManager.RegisterClientMessage<StartNodeMessage>(StartNodeMessage);
//             NetManager.RegisterClientMessage<EndNodeMessage>(EndNodeMessage);
//         }
//         protected override void DisposeImpl()
//         {
//             NetManager.UnregisterClientMessage<ScenarioStartedMessage>();
//             NetManager.UnregisterClientMessage<ScenarioStoppedMessage>();
//             NetManager.UnregisterClientMessage<SubgraphActivateMessage>();
//             NetManager.UnregisterClientMessage<SubgraphCompleteMessage>();
//             
//             NetManager.UnregisterClientMessage<ActionMessage>();
//             NetManager.UnregisterClientMessage<ConditionActivateMessage>();
//             NetManager.UnregisterClientMessage<ConditionCompleteMessage>();
//             NetManager.UnregisterClientMessage<StartNodeMessage>();
//             NetManager.UnregisterClientMessage<EndNodeMessage>();
//         }
//
//         private void ScenarioStartedMessage(ScenarioStartedMessage message)
//         {
//             IScenarioModel model;
//             NodeExecutionContext nodeContext;
//             
//             if (subgraphsCache.Remove(message.PlayerHash, out var subgraphNode))
//             {
//                 // Старт саб-плеера
//                 model = subgraphNode.SubModel;
//                 nodeContext = subgraphNode.SubContext;
//             }
//             else
//             {
//                 // Старт корневого плеера
//                 var module = modules.FirstOrDefault(message.ScenarioIdentifier);
//                 model = loadService.LoadModelFromJson(module.ScenarioAsset.text);
//                 nodeContext = rootExecutionContext.CreateSubcontextClient(model);
//             }
//             
//             AddModel(message, model, nodeContext);
//
//             if (message.UseLog)
//                 Debug.Log($"<b>Scenario Started</b>: (<b>{message.PlayerHash}</b>) {GetStatusString(message.ScenarioIdentifier)}");
//         }
//         private void ScenarioStoppedMessage(ScenarioStoppedMessage message)
//         {
//             RemoveModel(message.PlayerHash, out var clientGraph);
//
//             if (clientGraph.UseLog)
//                 Debug.Log($"<b>Scenario Stopped</b>: (<b>{clientGraph.GraphHash}</b>) {GetStatusString(clientGraph.ScenarioIdentifier)}");
//         }
//         
//         private void SubgraphActivateMessage(SubgraphActivateMessage message)
//         {
//             if (!TryGetNode(message.ParentPlayerHash, message.NodeHash, out var flowNode, out var clientGraph)) return;
//             
//             if (clientGraph.UseLog)
//             {
//                 Debug.Log($"<b>{flowNode.GetType().Name} Activated</b>: (<b>{clientGraph.GraphHash}</b>) {flowNode.GetStatusString()}");
//                 Debug.Log($"<b>SubScenario Added</b>: child:<b>{message.SubPlayerHash}</b> parent:<b>{message.ParentPlayerHash}</b>");
//             }
//             
//             // На данный момент, главная идея этого метода - подготовка к старту саб-плеера
//             // Понимаю, что сделано не очень, но пока работает - лучше не трогать
//             
//             var subgraphNode = (ISubgraphNode)flowNode;
//             subgraphNode.Activate(clientGraph.ExecutionContext);
//             subgraphsCache.Add(message.SubPlayerHash, subgraphNode);
//         }
//         private void SubgraphCompleteMessage(SubgraphCompleteMessage message)
//         {
//             if (!TryGetNode(message.ParentPlayerHash, message.NodeHash, out var flowNode, out var clientGraph)) return;
//             
//             if (clientGraph.UseLog)
//             {
//                 Debug.Log($"<b>{flowNode.GetType().Name} Completed</b>: (<b>{clientGraph.GraphHash}</b>) {flowNode.GetStatusString()}");
//                 Debug.Log($"<b>SubScenario Removed</b>: child:<b>{message.SubPlayerHash}</b> parent:<b>{message.ParentPlayerHash}</b>");
//             }
//             
//             var subgraphNode = (ISubgraphNode)flowNode;
//             subgraphNode.Deactivate(clientGraph.ExecutionContext);
//         }
//
//         private void AddModel(ScenarioStartedMessage message, IScenarioModel model, NodeExecutionContext executionContext)
//         {
//             if (!clientGraphs.TryAdd(message.PlayerHash, new ClientGraph(message, model, executionContext)))
//                 Debug.LogError($"Can't add {message.PlayerHash}");
//         }
//         private void RemoveModel(int graphHash, out ClientGraph clientGraph)
//         {
//             if (!clientGraphs.Remove(graphHash, out clientGraph))
//                 Debug.LogError($"Can't remove {graphHash}");
//         }
//
//         private void ActionMessage(ActionMessage message)
//         {
//             if (!TryGetNode(message.PlayerHash, message.NodeHash, out var flowNode, out var clientGraph)) return;
//             
//             if (clientGraph.UseLog)
//                 Debug.Log($"<b>{flowNode.GetType().Name} Activated</b>: (<b>{clientGraph.GraphHash}</b>) {flowNode.GetStatusString()}");
//             flowNode.Activate(clientGraph.ExecutionContext);
//             
//             if (clientGraph.UseLog)
//                 Debug.Log($"<b>{flowNode.GetType().Name} Completed</b>: (<b>{clientGraph.GraphHash}</b>) {flowNode.GetStatusString()}");
//             flowNode.Deactivate(clientGraph.ExecutionContext);
//         }
//         private void ConditionStartedMessage(ConditionActivateMessage message)
//         {
//             if (!TryGetNode(message.PlayerHash, message.NodeHash, out var flowNode, out var clientGraph)) return;
//             var conditionNode = (IConditionNode)flowNode;
//             
//             conditionNode.ConditionCompleted += ConditionCompleted;
//             conditionNode.NodeCompleted += NodeCompleted;
//             
//             if (clientGraph.UseLog)
//                 Debug.Log($"<b>{flowNode.GetType().Name} Activated</b>: (<b>{clientGraph.GraphHash}</b>) {flowNode.GetStatusString()}");
//             conditionNode.Activate(clientGraph.ExecutionContext);
//             
//             return;
//             
//             void ConditionCompleted(IScenarioCondition component, int index)
//             {
//                 SendCompleted(message.PlayerHash, message.NodeHash, index);
//             }
//             void NodeCompleted()
//             {
//                 conditionNode.ConditionCompleted -= ConditionCompleted;
//                 conditionNode.NodeCompleted -= NodeCompleted;
//             }
//         }
//         private void ConditionCompleteMessage(ConditionCompleteMessage message)
//         {
//             if (!TryGetNode(message.PlayerHash, message.NodeHash, out var flowNode, out var clientGraph)) return;
//             var conditionNode = (IConditionNode)flowNode;
//             
//             if (clientGraph.UseLog)
//                 Debug.Log($"<b>{flowNode.GetType().Name} Completed</b>: (<b>{clientGraph.GraphHash}</b>) {flowNode.GetStatusString()}");
//             conditionNode.Deactivate(clientGraph.ExecutionContext);
//         }
//
//         private void SendCompleted(int playerHash, int nodeHash, int componentIndex)
//         {
//             NetworkClient.Send(new FireConditionComponentMessage
//             {
//                 PlayerHash = playerHash,
//                 NodeHash = nodeHash,
//                 ComponentIndex = componentIndex,
//             });
//         }
//
//         private void StartNodeMessage(StartNodeMessage message)
//         {
//             if (!TryGetNode(message.PlayerHash, message.NodeHash, out var flowNode, out var clientGraph)) return;
//             
//             if (clientGraph.UseLog)
//                 Debug.Log($"<b>{flowNode.GetType().Name} Activated</b>: (<b>{clientGraph.GraphHash}</b>) {flowNode.GetStatusString()}");
//             flowNode.Activate(clientGraph.ExecutionContext);
//             
//             if (clientGraph.UseLog)
//                 Debug.Log($"<b>{flowNode.GetType().Name} Completed</b>: (<b>{clientGraph.GraphHash}</b>) {flowNode.GetStatusString()}");
//             flowNode.Deactivate(clientGraph.ExecutionContext);
//         }
//         private void EndNodeMessage(EndNodeMessage message)
//         {
//             if (!TryGetNode(message.PlayerHash, message.NodeHash, out var flowNode, out var clientGraph)) return;
//             
//             if (clientGraph.UseLog)
//                 Debug.Log($"<b>{flowNode.GetType().Name} Activated</b>: (<b>{clientGraph.GraphHash}</b>) {flowNode.GetStatusString()}");
//             flowNode.Activate(clientGraph.ExecutionContext);
//             
//             if (clientGraph.UseLog)
//                 Debug.Log($"<b>{flowNode.GetType().Name} Completed</b>: (<b>{clientGraph.GraphHash}</b>) {flowNode.GetStatusString()}");
//             flowNode.Deactivate(clientGraph.ExecutionContext);
//         }
//         
//         private bool TryGetNode(int playerHash, int nodeHash, out IScenarioNodeFlow flowNode)
//         {
//             if (!clientGraphs.TryGetValue(playerHash, out var clientGraph))
//             {
//                 Debug.LogWarning($"[Net] Can't find player {playerHash} on client");
//                 flowNode = null; return false;
//             }
//             
//             var node = clientGraph.Model.Graph.GetNode(nodeHash);
//             if (node == null)
//             {
//                 Debug.LogWarning($"[Net] Can't find node {nodeHash} in player {playerHash} on client");
//                 flowNode = null; return false;
//             }
//             
//             flowNode = node as IScenarioNodeFlow;
//             if (flowNode == null)
//             {
//                 Debug.LogWarning($"[Net] Null node {nodeHash} in player {playerHash} on client");
//                 return false;
//             }
//             return true;
//         }
//         private bool TryGetNode(int playerHash, int nodeHash, out IScenarioNodeFlow flowNode, out ClientGraph clientGraph)
//         {
//             if (!clientGraphs.TryGetValue(playerHash, out clientGraph))
//             {
//                 Debug.LogWarning($"[Net] Can't find player {playerHash} on client");
//                 flowNode = null; return false;
//             }
//
//             var node = clientGraph.Model.Graph.GetNode(nodeHash);
//             if (node == null)
//             {
//                 Debug.LogWarning($"[Net] Can't find node {nodeHash} in player {playerHash} on client");
//                 flowNode = null; return false;
//             }
//
//             flowNode = node as IScenarioNodeFlow;
//             if (flowNode == null)
//             {
//                 Debug.LogWarning($"[Net] Null node {nodeHash} in player {playerHash} on client");
//                 return false;
//             }
//             return true;
//         }
//         private string GetStatusString(string scenarioIdentifier)
//         {
//             var builder = new StringBuilder();
//             
//             builder.Append($"identifier:<b>{scenarioIdentifier}</b>");
//             
//             builder.Append($" useNet:<b>true</b>");
//             if (rootExecutionContext.IdentityHash != 0) 
//                 builder.Append($" identityHash:<b>{rootExecutionContext.IdentityHash}</b>");
//             builder.Append($" useLog:<b>true</b>");
//
//             return builder.ToString();
//         }
//     }
// }