using System;

namespace VRF.Utils.Activities
{
    /// <summary>
    /// Основной интерфейс для контейнера активности. Это нужно для смешивания bool значений и получения одного bool
    /// </summary>
    public interface IActivityContainer
    {
        public bool Active { get; }
        public event Action<bool> OnUpdate;
        public event Action<bool> OnUpdateInverse;
    }
}