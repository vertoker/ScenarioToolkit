using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace VRF.Players.Models.Player
{
    /// <summary>
    /// Модель с кастомной конфигурацией локального игрока.
    /// Используется в DataSources. Оптимизирован для сериализации в JSON
    /// </summary>
    [Serializable]
    public class PlayerModel
    {
        /// <summary> Приоритетный режим управления. Будет использоваться, только если у
        /// игрока появится выбор. Может быть или VR или WASD </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [field: SerializeField] public PlayerControlModes PriorityControlMode { get; set; } = PlayerControlModes.VR;
    }
}