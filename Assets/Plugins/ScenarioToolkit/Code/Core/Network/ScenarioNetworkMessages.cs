using System;
using Mirror;

namespace ScenarioToolkit.Core.Network
{
    // Scenario
    public struct ScenarioStartedMessage : NetworkMessage
    {
        public int PlayerHash;
        public string ScenarioIdentifier;
        public bool UseLog;
        
        public int GetMessageHash() => PlayerHash;
    }
    public struct ScenarioStoppedMessage : NetworkMessage
    {
        public int PlayerHash;
        
        public int GetMessageHash() => PlayerHash;
    }
    
    // Важно понимать, что серверное и клиентское представление о сценариях отличается.
    // Сервер хранит плееры в виде обычного массива и логику наследования и проигрывания идёт из самих плееров,
    // а клиенту нужна дополнительная информация для создания локальной SubgraphNode исходя из следующего сценария
    
    public struct SubgraphActivateMessage : NetworkMessage
    {
        public int ParentPlayerHash;
        public int NodeHash;
        public int SubPlayerHash;
        
        public override int GetHashCode() => HashCode.Combine(ParentPlayerHash, NodeHash);
    }
    public struct SubgraphCompleteMessage : NetworkMessage
    {
        public int ParentPlayerHash;
        public int NodeHash;
        public int SubPlayerHash;
        
        public override int GetHashCode() => HashCode.Combine(ParentPlayerHash, NodeHash);
    }
    
    // PlayerHash и NodeHash это универсальный адрес до любой ноды внутри проигрываемого сценария
    // PlayerHash генерируется на сервере во время воспроизведения и относится к конкретному Player
    // NodeHash записан в самих сценариях и у всех есть данные по этой ноде
    
    // Nodes
    public struct ActionMessage : NetworkMessage
    {
        public int PlayerHash;
        public int NodeHash;
    }
    
    public struct ConditionActivateMessage : NetworkMessage
    {
        public int PlayerHash;
        public int NodeHash;
    }
    public struct ConditionCompleteMessage : NetworkMessage
    {
        public int PlayerHash;
        public int NodeHash;
    }
    public struct FireConditionComponentMessage : NetworkMessage
    {
        public int PlayerHash;
        public int NodeHash;
        public int ComponentIndex;
    }
    
    public struct StartNodeMessage : NetworkMessage
    {
        public int PlayerHash;
        public int NodeHash;
    }
    public struct EndNodeMessage : NetworkMessage
    {
        public int PlayerHash;
        public int NodeHash;
    }
}