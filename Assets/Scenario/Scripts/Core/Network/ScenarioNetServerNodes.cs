using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player;
using Scenario.Core.Player.Roles;
using VRF.Networking.Core;
using Zenject;
using Mirror;

namespace Scenario.Core.Network
{
    /// <summary>
    /// Дополнительный серверный класс, отвечающий за отправку и исполнение нод самого сценария.
    /// ActionNode и ConditionNode исполняются тут
    /// </summary>
    public class ScenarioNetServerNodes : BaseScenarioNet
    {
        private readonly ScenarioNetServer server;
        private readonly RoleFilterService roleFilterService;

        private readonly Dictionary<int, (ScenarioPlayer, Dictionary<int, IConditionNode>)> activeConditions = new();

        public ScenarioNetServerNodes(ScenarioNetServer server, SignalBus bus,
            RoleFilterService roleFilterService, [InjectOptional] VRFNetworkManager netManager)
            : base(netManager)
        {
            this.server = server;
            this.roleFilterService = roleFilterService;
        }
        /// <summary> Для Host и Server </summary>
        protected override bool GetNetActiveStatus() => NetworkServer.active;

        protected override void InitializeImpl()
        {
            server.PlayerStarted += PlayerStarted;
            server.PlayerStopped += PlayerStopped;
            server.NodeActivated += NodeActivated;
            server.NodeCompleted += NodeCompleted;

            NetManager.RegisterServerMessage<FireConditionComponentMessage>(Receive);
        }
        protected override void DisposeImpl()
        {
            server.PlayerStarted -= PlayerStarted;
            server.PlayerStopped -= PlayerStopped;
            server.NodeActivated -= NodeActivated;
            server.NodeCompleted -= NodeCompleted;
            
            NetManager.UnregisterServerMessage<FireConditionComponentMessage>();
        }

        private void PlayerStarted(ScenarioPlayer player)
        {
            activeConditions.Add(player.GetHashCode(), (player, new Dictionary<int, IConditionNode>()));
        }
        private void PlayerStopped(ScenarioPlayer player)
        {
            activeConditions.Remove(player.GetHashCode());
        }
        private void NodeActivated(ScenarioPlayer player, IScenarioNode node)
        {
            switch (node)
            {
                case IActionNode:
                    NetworkServer.SendToAll(new ActionMessage
                    {
                        PlayerHash = player.GetHashCode(),
                        NodeHash = node.Hash,
                    });
                    break;
                case IConditionNode conditionNode:
                {
                    var playerHash = player.GetHashCode();
                    AddConditionNode(playerHash, conditionNode);
                
                    NetworkServer.SendToAll(new ConditionActivateMessage
                    {
                        PlayerHash = playerHash,
                        NodeHash = conditionNode.Hash,
                    });
                    break;
                }
                case IStartNode:
                    NetworkServer.SendToAll(new StartNodeMessage
                    {
                        PlayerHash = player.GetHashCode(),
                        NodeHash = node.Hash,
                    });
                    break;
                case IEndNode:
                    NetworkServer.SendToAll(new EndNodeMessage
                    {
                        PlayerHash = player.GetHashCode(),
                        NodeHash = node.Hash,
                    });
                    break;
            }
        }
        private void NodeCompleted(ScenarioPlayer player, IScenarioNode node)
        {
            if (node is IConditionNode conditionNode)
            {
                var playerHash = player.GetHashCode();
                RemoveConditionNode(playerHash, conditionNode);
                
                NetworkServer.SendToAll(new ConditionCompleteMessage
                {
                    PlayerHash = playerHash,
                    NodeHash = conditionNode.Hash,
                });
            }
        }

        private void AddConditionNode(int playerHash, IConditionNode node)
        {
            if (!activeConditions.TryGetValue(playerHash, out var bind)) return;
            
            bind.Item2?.Add(node.Hash, node);
            node.Activate(bind.Item1.ExecutionContext);
        }
        private void RemoveConditionNode(int playerHash, IConditionNode node)
        {
            if (!activeConditions.TryGetValue(playerHash, out var bind)) return;
            
            bind.Item2?.Remove(node.Hash);
            node.Deactivate(bind.Item1.ExecutionContext);
        }
        
        private void Receive(NetworkConnectionToClient client, FireConditionComponentMessage message)
        {
            if (!activeConditions.TryGetValue(message.PlayerHash, out var bind)) return;
            var node = bind.Item2?[message.NodeHash];
            //var identity = NetManager.ServerClients[client];
            
            node?.FireComponent(bind.Item1.ExecutionContext, message.ComponentIndex);
        }
    }
}