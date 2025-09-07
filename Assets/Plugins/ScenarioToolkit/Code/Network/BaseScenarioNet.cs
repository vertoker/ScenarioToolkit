using System;
using Mirror;
using Zenject;

namespace ScenarioToolkit.Network
{
    /// <summary>
    /// Базовый класс для сетевых реализаций сценария
    /// </summary>
    public abstract class BaseScenarioNet : IInitializable, IDisposable
    {
        protected readonly NetworkManager NetManager;
        
        private readonly bool netActive;
        private bool initialized;
        
        public BaseScenarioNet([InjectOptional] NetworkManager netManager)
        {
            NetManager = netManager;
            netActive = netManager;
        }
        protected abstract bool GetNetActiveStatus();
        
        public virtual void Initialize()
        {
            if (!netActive) return;
            if (GetNetActiveStatus())
                TryInitialize();
        }
        public virtual void Dispose()
        {
            if (!netActive) return;
            TryDispose();
        }

        private void TryInitialize()
        {
            if (initialized) return;
            initialized = true;
            InitializeImpl();
        }
        private void TryDispose()
        {
            if (!initialized) return;
            initialized = false;
            DisposeImpl();
        }
        
        protected abstract void InitializeImpl();
        protected abstract void DisposeImpl();
    }
}