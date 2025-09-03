using System;
using UnityEngine;
using VRF.UI.Settings.Core.Interfaces;

namespace VRF.UI.Settings.Core
{
    /// <summary>
    /// Модель со всеми данными пользовательских настроек игрока.
    /// Используется в DataSources. Оптимизирован для сериализации в JSON
    /// </summary>
    [Serializable]
    public class SettingsModel : IReadOnlySettingsModel
    {
        [field:SerializeField] public float MouseSensitivityX { get; set; } = 1;
        [field:SerializeField] public float MouseSensitivityY { get; set; } = 1;
        [field:SerializeField] public string MicrophoneDevice { get; set; } = string.Empty;
    }
}