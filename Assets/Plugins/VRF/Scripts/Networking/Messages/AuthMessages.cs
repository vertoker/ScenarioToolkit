using Mirror;

// ReSharper disable InconsistentNaming

// Пакеты для авторизации клиентов в VRF Authenticator

namespace VRF.Networking.Messages
{
    /// <summary>
    /// Запрос на никнейм игрока
    /// </summary>
    public struct AuthNickname_RequestMessage : NetworkMessage, IPlayerConfigMessage
    {
        public string Nickname;
        
        public NetPlayerSpawnData spawnData;
        public NetPlayerSpawnData SpawnData => spawnData;
    }
    
    /// <summary>
    /// Запрос на никнейм и пароль игрока
    /// </summary>
    public struct AuthNicknamePassword_RequestMessage : NetworkMessage, IPlayerConfigMessage
    {
        public string Nickname;
        public string Password;
        
        public NetPlayerSpawnData spawnData;
        public NetPlayerSpawnData SpawnData => spawnData;
    }
    
    /// <summary>
    /// Запрос на ID игрока
    /// </summary>
    public struct AuthIDRequest_Message : NetworkMessage, IPlayerConfigMessage
    {
        public int ID;
        
        public NetPlayerSpawnData spawnData;
        public NetPlayerSpawnData SpawnData => spawnData;
    }
    
    /// <summary> Ответ от сервера (пока что только после авторизации) </summary>
    public struct AuthResponse_Message : NetworkMessage
    {
        public byte Code;
        public string Message;
    }
}