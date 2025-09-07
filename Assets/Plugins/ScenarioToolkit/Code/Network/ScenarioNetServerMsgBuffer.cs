// using System.Collections.Generic;
// using Mirror;
// using Zenject;
//
// namespace ScenarioToolkit.Network
// {
//     /// <summary>
//     /// Сценарный серверный буффер сообщений, отправляет текущее состояние сценариев в виде пакетов новому клиенту
//     /// </summary>
//     public class ScenarioNetServerMsgBuffer : BaseScenarioNet
//     {
//         private readonly ScenarioNetServer server;
//
//         private readonly Dictionary<int, ScenarioStartedMessage> playerGraphs = new();
//         private readonly Dictionary<int, SubgraphActivateMessage> subGraphs = new();
//
//         public ScenarioNetServerMsgBuffer(ScenarioNetServer server, 
//             [InjectOptional] VRFNetworkManager netManager) : base(netManager)
//         {
//             this.server = server;
//         }
//         /// <summary> Для Host и Server </summary>
//         protected override bool GetNetActiveStatus() => NetworkServer.active;
//
//         protected override void InitializeImpl()
//         {
//             server.PlayerStartedMessage += PlayerStartedMessage;
//             server.PlayerStoppedMessage += PlayerStoppedMessage;
//             server.SubgraphActivatedMessage += SubgraphActivatedMessage;
//             server.SubgraphCompletedMessage += SubgraphCompletedMessage;
//             
//             NetManager.OnServerPlayerAuthorized += OnServerPlayerAuthorized;
//         }
//         protected override void DisposeImpl()
//         {
//             server.PlayerStartedMessage -= PlayerStartedMessage;
//             server.PlayerStoppedMessage -= PlayerStoppedMessage;
//             server.SubgraphActivatedMessage -= SubgraphActivatedMessage;
//             server.SubgraphCompletedMessage -= SubgraphCompletedMessage;
//             
//             NetManager.OnServerPlayerAuthorized -= OnServerPlayerAuthorized;
//         }
//
//         private void PlayerStartedMessage(ScenarioStartedMessage msg)
//         {
//             playerGraphs.Add(msg.PlayerHash, msg);
//         }
//         private void PlayerStoppedMessage(ScenarioStoppedMessage msg)
//         {
//             playerGraphs.Remove(msg.PlayerHash);
//         }
//         private void SubgraphActivatedMessage(SubgraphActivateMessage msg)
//         {
//             // Уникальный хэш = проигрываемый player + хэш ноды
//             subGraphs.Add(msg.GetHashCode(), msg);
//         }
//         private void SubgraphCompletedMessage(SubgraphCompleteMessage msg)
//         {
//             subGraphs.Remove(msg.GetHashCode());
//         }
//
//         private void OnServerPlayerAuthorized(NetworkConnectionToClient connection, PlayerIdentityConfig identity)
//         {
//             // Чисто теоретически порядок отправки сообщения имеет значение
//             // Но по идее и без него будет всё на новом клиенте работать
//
//             foreach (var player in playerGraphs) // Сначала сценарии
//                 connection.Send(player.Value);
//             foreach (var subgraph in subGraphs) // Потом контексты в этих саб-сценариях
//                 connection.Send(subgraph.Value);
//         }
//     }
// }