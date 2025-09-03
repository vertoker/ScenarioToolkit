using System;
using KBCore.Refs;
using UnityEngine;
using Zenject;

namespace VRF.Components
{
    /// <summary>
    /// Module - исполнительная единица внутри View.
    /// Сам по себе действовать не должен,
    /// поэтому для его инициализации нужен view компонент
    /// </summary>
    [RequireComponent(typeof(BaseView))]
    public abstract class BaseModule : ValidatedMonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField, Self] private BaseView view;
        
        /// <summary>
        /// Через имеющийся view компонент может связываться с другими module
        /// компонентами или другими связанными View объектами
        /// </summary>
        protected BaseView View => view;
        
        public bool Initialized { get; private set; }
        
        public Type ModuleType => GetType().UnderlyingSystemType;
        
        public virtual void Initialize()
        {
            // Удобный бинд для инициализации и последующего использования
            Initialized = true;
        }
        public virtual void Dispose()
        {
            Initialized = false;
        }
    }
}