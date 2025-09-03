using UnityEngine;

namespace VRF.UI.Settings.Core.Interfaces
{
    /// <summary>
    /// Интерфейс со всеми данными пользовательских настроек игрока, нот только для чтения
    /// </summary>
    public interface IReadOnlySettingsModel
    {
        public float MouseSensitivityX { get; }
        public float MouseSensitivityY { get; }
        public string MicrophoneDevice { get; }
    }
}