using Mirror;

// ReSharper disable InconsistentNaming

// Пакеты, связанные со сменой сцен

namespace VRF.Networking.Messages
{
    /// <summary>
    /// Уведомление о том, что сцена у локального игрока загружена
    /// </summary>
    public struct SceneUpdate_Message : NetworkMessage
    {
        public int SceneIndex;
    }
}