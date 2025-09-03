using Mirror;

// ReSharper disable InconsistentNaming

// Пакеты, связанные с изменением внешнего вида сетевых игроков

namespace VRF.Networking.Messages
{
    /// <summary>
    /// Пакет отправляется на сервер
    /// для обновления IK объекта
    /// </summary>
    public struct NetAppearanceUpdate_RequestMessage : NetworkMessage
    {
        public int IdentityCode;
        public int AppearanceCode;
    }
    
    /// <summary>
    /// Пакет отправляется на сервер
    /// для сброса IK объекта
    /// </summary>
    public struct NetNetAppearanceReset_RequestMessage : NetworkMessage
    {
        public int IdentityCode;
    }
    
    /// <summary>
    /// Пакет отправляется на клиентов
    /// для обновления IK объекта
    /// </summary>
    public struct NetAppearanceUpdate_Message : NetworkMessage
    {
        public int IdentityCode;
        public int AppearanceCode;
    }
    
    /// <summary>
    /// Пакет отправляется на клиентов
    /// для сброса IK объекта
    /// </summary>
    public struct NetAppearanceReset_Message : NetworkMessage
    {
        public int IdentityCode;
    }
}