// using System;
// using System.Collections.Generic;
// using Mirror;
// using Zenject;
//
// namespace ScenarioToolkit.Network
// {
//     /// <summary>
//     /// Основной серверный класс, отвечающий за обработку плееров и объединения их ивентов о вызове нод.
//     /// Обеспечивает ядро сетевой обработки, отвечает за сами сценарии и SubgraphNode в частности
//     /// </summary>
//     public class ScenarioNetServer : BaseScenarioNet // NetHost
//     {
//         public event Action<ScenarioPlayer> PlayerAdded;
//         public event Action<ScenarioPlayer> PlayerRemoved;
//         public event Action<ScenarioPlayer> PlayerStarted;
//         public event Action<ScenarioPlayer> PlayerStopped;
//         public event Action<ScenarioPlayer, IScenarioNode> NodeActivated;
//         public event Action<ScenarioPlayer, IScenarioNode> NodeCompleted;
//         
//         public event Action<ScenarioStartedMessage> PlayerStartedMessage;
//         public event Action<ScenarioStoppedMessage> PlayerStoppedMessage;
//         public event Action<SubgraphActivateMessage> SubgraphActivatedMessage;
//         public event Action<SubgraphCompleteMessage> SubgraphCompletedMessage;
//         
//         private readonly ScenarioPlayer rootPlayer;
//         private readonly ScenarioSceneProvider sceneProvider;
//         
//         // Сервер хранит все плееры в виде обычного массива, несмотря на их родителей и вложенность
//         // И также хранятся сгенерированные методы, чтобы безболезненно делать отписку
//         private readonly Dictionary<ScenarioPlayer, PlayerEvents> playersEvents = new();
//
//         public ScenarioNetServer(ScenarioPlayer rootPlayer, 
//             ScenarioSceneProvider sceneProvider,
//             [InjectOptional] VRFNetworkManager netManager) 
//             : base(netManager)
//         {
//             this.rootPlayer = rootPlayer;
//             this.sceneProvider = sceneProvider;
//         }
//         /// <summary> Для Host и Server </summary>
//         protected override bool GetNetActiveStatus() => NetworkServer.active;
//
//         protected override void InitializeImpl()
//         {
//             //Debug.Log($"Start NetScenario Server/Host");
//
//             AddPlayer(rootPlayer);
//             foreach (var scenarioInstance in sceneProvider.ScenarioInstances.Values)
//             {
//                 if (scenarioInstance)
//                     AddPlayer(scenarioInstance.Player);
//             }
//         }
//         protected override void DisposeImpl()
//         {
//             RemovePlayer(rootPlayer);
//             foreach (var scenarioInstance in sceneProvider.ScenarioInstances.Values)
//             {
//                 if (scenarioInstance)
//                     RemovePlayer(scenarioInstance.Player);
//             }
//         }
//         
//         private void ScenarioStarted(ScenarioPlayer player, ScenarioLaunchModel launchModel)
//         {
//             if (launchModel == null) return;
//             if (!launchModel.UseNetwork) return;
//             if (string.IsNullOrEmpty(launchModel.Scenario)) return;
//             
//             PlayerStarted?.Invoke(player);
//
//             var msg = new ScenarioStartedMessage
//             {
//                 PlayerHash = player.GetHashCode(),
//                 ScenarioIdentifier = launchModel.Scenario,
//                 UseLog = launchModel.UseLog,
//             };
//             PlayerStartedMessage?.Invoke(msg);
//             NetworkServer.SendToAll(msg);
//         }
//         private void ScenarioStopped(ScenarioPlayer player)
//         {
//             PlayerStopped?.Invoke(player);
//
//             var msg = new ScenarioStoppedMessage
//             {
//                 PlayerHash = player.GetHashCode(),
//             };
//             PlayerStoppedMessage?.Invoke(msg);
//             NetworkServer.SendToAll(msg);
//         }
//         
//         private int AddPlayer(ScenarioPlayer player)
//         {
//             var events = new PlayerEvents(ScenarioStartedLocal, ScenarioStoppedLocal, NodeActivatedLocal, NodeCompletedLocal);
//             playersEvents.Add(player, events);
//             
//             player.ScenarioStarted += events.PlayerStarted;
//             player.ScenarioStopped += events.PlayerStopped;
//             // Тут достаточно тонкий момент, так как процессы активации и деактивации у нод
//             // разные, отчего сетевая логика может быть нужна до или после этих действий.
//             // Если сетевые ивенты вдруг перестанут работать, то это хорошая причина
//             // разделить логику серверных методов для каждой ноды отдельно
//             player.NodeBeforeActivated += events.NodeActivated;
//             player.NodeBeforeCompleted += events.NodeCompleted;
//             
//             PlayerAdded?.Invoke(player);
//             
//             // Для идентификации саб плеера у Client нужен уникальный Hash
//             // И созданный на сервере во время сессии GetHashCode() подходит идеально
//             return player.GetHashCode();
//             
//             
//             // Все эти методы генерируются при вызову AddPlayer, добавляя локальное окружение метода.
//             // Это необходимо, чтобы позже проще делать отписку от ивентов конкретного Player
//             
//             void ScenarioStartedLocal(ScenarioLaunchModel model) => ScenarioStarted(player, model);
//             void ScenarioStoppedLocal() => ScenarioStopped(player);
//
//             void SubgraphNodeOnNetActivateReady(ISubgraphNode node) => SubgraphNodeActivated(player, node);
//
//             void NodeActivatedLocal(IScenarioNode node)
//             {
//                 if (node is ISubgraphNode subgraphNode)
//                     // Супер костыль, который в сети активирует ноду до запуска следующего Player.
//                     // Является следствием того, что у ноды есть только Activate и Deactivate.
//                     // Ничего плохого не вижу, так как ивент чисто для реализации ISubgraphNode
//                     subgraphNode.OnSubgraphIsReady += SubgraphNodeOnNetActivateReady;
//                 
//                 // ReSharper disable once PossibleNullReferenceException
//                 NodeActivated.Invoke(player, node);
//             }
//
//             void NodeCompletedLocal(IScenarioNode node)
//             {
//                 if (node is ISubgraphNode subgraphNode)
//                 {
//                     subgraphNode.OnSubgraphIsReady -= SubgraphNodeOnNetActivateReady;
//                     SubgraphNodeDeactivated(player, subgraphNode);
//                 }
//                 else
//                 {
//                     // ReSharper disable once PossibleNullReferenceException
//                     NodeCompleted.Invoke(player, node);
//                 }
//             }
//         }
//         private int RemovePlayer(ScenarioPlayer player)
//         {
//             playersEvents.Remove(player, out var events);
//             
//             player.ScenarioStarted -= events.PlayerStarted;
//             player.ScenarioStopped -= events.PlayerStopped;
//             player.NodeBeforeActivated -= events.NodeActivated;
//             player.NodeBeforeCompleted -= events.NodeCompleted;
//             
//             //PlayerStopped?.Invoke(player);
//             PlayerRemoved?.Invoke(player);
//             
//             return player.GetHashCode();
//         }
//         
//         private void SubgraphNodeActivated(ScenarioPlayer parentPlayer, ISubgraphNode subgraphNode)
//         {
//             if (subgraphNode.SubPlayer == null) return;
//             
//             var msg = new SubgraphActivateMessage
//             {
//                 ParentPlayerHash = parentPlayer.GetHashCode(),
//                 NodeHash = subgraphNode.Hash,
//                 SubPlayerHash = subgraphNode.SubPlayer.GetHashCode(),
//             };
//             
//             AddPlayer(subgraphNode.SubPlayer);
//             SubgraphActivatedMessage?.Invoke(msg);
//             NetworkServer.SendToAll(msg);
//         }
//         private void SubgraphNodeDeactivated(ScenarioPlayer parentPlayer, ISubgraphNode subgraphNode)
//         {
//             if (subgraphNode.SubPlayer == null) return;
//             
//             var msg = new SubgraphCompleteMessage
//             {
//                 ParentPlayerHash = parentPlayer.GetHashCode(),
//                 NodeHash = subgraphNode.Hash,
//                 SubPlayerHash = subgraphNode.SubPlayer.GetHashCode(),
//             };
//             
//             SubgraphCompletedMessage?.Invoke(msg);
//             RemovePlayer(subgraphNode.SubPlayer);
//             NetworkServer.SendToAll(msg);
//         }
//         
//         private readonly struct PlayerEvents
//         {
//             public readonly Action<ScenarioLaunchModel> PlayerStarted;
//             public readonly Action PlayerStopped;
//             public readonly Action<IScenarioNode> NodeActivated;
//             public readonly Action<IScenarioNode> NodeCompleted;
//
//             public PlayerEvents(Action<ScenarioLaunchModel> playerStarted, Action playerStopped, 
//                 Action<IScenarioNode> nodeActivated, Action<IScenarioNode> nodeCompleted)
//             {
//                 PlayerStarted = playerStarted;
//                 PlayerStopped = playerStopped;
//                 NodeActivated = nodeActivated;
//                 NodeCompleted = nodeCompleted;
//             }
//         }
//     }
// }