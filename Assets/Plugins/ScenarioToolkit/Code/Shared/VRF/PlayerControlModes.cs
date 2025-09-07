using System;

namespace VRF.Players.Models
{
    /// <summary>
    /// Все возможные методы управления. Скорее всего
    /// больше чем WASD и VR не будет
    /// </summary>
    [Flags]
    public enum PlayerControlModes
    {
        VR = 1,
        WASD = 2,
    }
}