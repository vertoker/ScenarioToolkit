using System;

namespace VRF.Players.Services.Settings
{
    /// <summary>
    /// Шаблон сервиса для прокидывания параметров из системы UI во внешний мир
    /// (паттерн Observer) (непрямая связь)
    /// </summary>
    public abstract class BaseSettingsParameterObserver<T>
    {
        public event Action<T> OnUpdateParameter;

        public T Value { get; private set; }
        
        public void Update(T value)
        {
            Value = value;
            OnUpdateParameter?.Invoke(value);
        }
    }
}