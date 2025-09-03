using Mirror;

// ReSharper disable InconsistentNaming

// Пакеты, связанные со спауном сестевых предметво игроков

namespace VRF.Networking.Messages
{
    /// <summary>
    /// Запрос на сервер для спавна предмета
    /// </summary>
    public struct NetItemSpawn_RequestMessage : NetworkMessage
    {
        public int AssetHashCode;
        public int RuntimeHashCode;
    }
    
    /// <summary>
    /// Запрос на сервер для уничтожения предмета
    /// </summary>
    public struct NetItemDestroy_RequestMessage : NetworkMessage
    {
        public int AssetHashCode;
        public int RuntimeHashCode;
    }
    
    /// <summary>
    /// Запрос на сервер для включения предмета
    /// </summary>
    public struct NetItemEnable_RequestMessage : NetworkMessage
    {
        public int AssetHashCode;
        public int RuntimeHashCode;
    }
    
    /// <summary>
    /// Запрос на сервер для выключения предмета
    /// </summary>
    public struct NetItemDisable_RequestMessage : NetworkMessage
    {
        public int AssetHashCode;
        public int RuntimeHashCode;
    }
    
    /// <summary>
    /// Запрос на сервер для указания активности Behaviour компонента
    /// </summary>
    public struct NetItemToggleBehaviour_Message : NetworkMessage
    {
        public int AssetHashCode;
        public int RuntimeHashCode;
        
        public int BehaviourIndex;
        public bool Active;
    }
    
    /// <summary>
    /// Запрос на сервер для указания активности всех Behaviour компонентов
    /// </summary>
    public struct NetItemToggleBehaviours_Message : NetworkMessage
    {
        public int AssetHashCode;
        public int RuntimeHashCode;
        
        public bool Active;
    }
}