// using VRF.Networking.Core;
// using Zenject;
// using Mirror;
//
// namespace Scenario.Core.Network
// {
//     /// <summary>
//     /// Дочерний серверный класс, который нужен только для того, чтобы не выдавать ошибки
//     /// об отсутствии клиента на сервере (так как сервер всегда хост).
//     /// Такой костыль нужен, так как участник сети не может иметь больше одного Node Processor.
//     /// Client имеет свой Node Processor в классе сети, а Host тоже имеет эту логику, но уже в Player
//     /// </summary>
//     public class ScenarioNetClientDummy : BaseScenarioNet
//     {
//         public ScenarioNetClientDummy([InjectOptional] VRFNetworkManager netManager) 
//             : base(netManager) { }
//
//         /// <summary> Для Host </summary>
//         protected override bool GetNetActiveStatus() => NetworkClient.active && NetworkServer.active;
//         
//         protected override void InitializeImpl()
//         {
//             //Debug.Log($"Start NetScenario Host Dummy Client");
//
//             // Для хоста, внутренний приём пакетов не требует аутентификации
//             
//             NetManager.RegisterClientMessage<ScenarioStartedMessage>(ScenarioStartedMessage, false);
//             NetManager.RegisterClientMessage<ScenarioStoppedMessage>(ScenarioStoppedMessage, false);
//             NetManager.RegisterClientMessage<SubgraphActivateMessage>(SubGraphStartedMessage, false);
//             NetManager.RegisterClientMessage<SubgraphCompleteMessage>(SubGraphCompletedMessage, false);
//
//             NetManager.RegisterClientMessage<ActionMessage>(ActionMessage, false);
//             NetManager.RegisterClientMessage<ConditionActivateMessage>(ConditionStartedMessage, false);
//             NetManager.RegisterClientMessage<ConditionCompleteMessage>(ConditionCompleteMessage, false);
//             NetManager.RegisterClientMessage<StartNodeMessage>(StartNodeMessage, false);
//             NetManager.RegisterClientMessage<EndNodeMessage>(EndNodeMessage, false);
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
//         private void ScenarioStartedMessage(ScenarioStartedMessage obj)
//         {
//             //Debug.Log(nameof(ScenarioStartedMessage));
//         }
//         private void ScenarioStoppedMessage(ScenarioStoppedMessage obj)
//         {
//             //Debug.Log(nameof(ScenarioStoppedMessage));
//         }
//         private void SubGraphStartedMessage(SubgraphActivateMessage obj)
//         {
//             //Debug.Log(nameof(SubGraphStartedMessage));
//         }
//         private void SubGraphCompletedMessage(SubgraphCompleteMessage obj)
//         {
//             //Debug.Log(nameof(SubGraphCompletedMessage));
//         }
//
//         private void ActionMessage(ActionMessage obj)
//         {
//             //Debug.Log(nameof(ActionMessage));
//         }
//         private void ConditionStartedMessage(ConditionActivateMessage obj)
//         {
//             //Debug.Log(nameof(ConditionStartedMessage));
//         }
//         private void ConditionCompleteMessage(ConditionCompleteMessage obj)
//         {
//             //Debug.Log(nameof(ConditionCompleteMessage));
//         }
//         private void StartNodeMessage(StartNodeMessage obj)
//         {
//             //Debug.Log(nameof(StartNodeMessage));
//         }
//         private void EndNodeMessage(EndNodeMessage obj)
//         {
//             //Debug.Log(nameof(EndNodeMessage));
//         }
//     }
// }