using System;
using UnityEngine;

namespace VRF.Players.Controllers.Scriptables
{
    /// <summary>
    /// Основа для конфигурирующего конфига сервисов управления локального игрока
    /// </summary>
    public abstract class BaseLocalPlayerConfig : ScriptableObject
    {
        public Type ConfigType => GetType().UnderlyingSystemType;
    }
}