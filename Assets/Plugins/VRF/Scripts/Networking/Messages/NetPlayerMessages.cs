using Mirror;
using VRF.Players.Models;
using VRF.Players.Models.Player;
using VRF.Players.Scriptables;

// ReSharper disable InconsistentNaming

// Пакеты для идентификации созданных сетевых игроков

namespace VRF.Networking.Messages
{
    /// <summary>
    /// Пакет отправляется на сервер 
    /// для инициализации сетевого игрока 
    /// от каждого нового клиента
    /// </summary>
    public struct InitNetPlayer_RequestMessage : NetworkMessage
    {
        public uint NetId;
    }
    /// <summary>
    /// Пакет отправляется на конкретного клиента 
    /// для инициализации сетевого игрока 
    /// и содержит в себе код роли на которую он должен ссылаться
    /// </summary>
    public struct InitNetPlayer_Message : NetworkMessage, IPlayerConfigMessage
    {
        public int IdentityCode;
        public uint NetId;
        
        public NetPlayerSpawnData SpawnData { get; set; }
    }
    
    /// <summary>
    /// Пакет отправляется на сервер 
    /// для инициализации сетевого игрока 
    /// от каждого нового клиента
    /// </summary>
    public struct UpdateNetPlayer_RequestMessage : NetworkMessage
    {
        public uint NetId;
        public int NewIdentityCode;
        public PlayerControlModes NewMode;
    }
    /// <summary>
    /// Пакет отправляется на конкретного клиента 
    /// для инициализации сетевого игрока 
    /// и содержит в себе код роли на которую он должен ссылаться
    /// </summary>
    public struct UpdateNetPlayer_Message : NetworkMessage
    {
        public uint NetId;
        public int OldIdentityCode;
        public int NewIdentityCode;
        public PlayerControlModes NewMode;
    }
    
    /// <summary>
    /// Стартовая конфигурация нового игрока, посылается один раз при авторизации игрока
    /// </summary>
    public readonly struct NetPlayerSpawnData
    {
        public readonly PlayerControlModes CurrentMode;

        public NetPlayerSpawnData(PlayerControlModes currentMode)
        {
            CurrentMode = currentMode;
        }
    }
    
    /// <summary>
    /// Интерфейс для сообщений содержащих NetPlayerConfig
    /// </summary>
    public interface IPlayerConfigMessage
    {
        public NetPlayerSpawnData SpawnData { get; }
    }
}