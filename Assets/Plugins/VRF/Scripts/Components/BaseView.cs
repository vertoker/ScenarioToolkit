using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using VRF.Utils.Identifying;

namespace VRF.Components
{
    /// <summary>
    /// View - идентификатор, который вешается на корень
    /// объекта в одном экземпляре и предназначен для:
    /// - Идентификации объекта по типу самого View
    /// - Хранилища Module объектов
    /// </summary>
    //[DisallowMultipleComponent] // оказалось неудобно
    public abstract class BaseView : IdentifiedMonoBehaviour
    {
        [SerializeField, ReadOnly] private BaseModule[] modules = Array.Empty<BaseModule>();
        
        public IReadOnlyList<BaseModule> Modules => modules;

        public Type ViewType => GetType().UnderlyingSystemType;
        
        // Каждый наследник определяет то, когда и как будут 
        // инициализироваться модули
        protected void InitializeInternal()
        {
            foreach (var module in Modules)
                module.Initialize();
        }
        protected void DisposeInternal()
        {
            foreach (var module in Modules)
                module.Dispose();
        }
        // Де инициализация модулей всегда происходит при уничтожении объекта
        private void OnDestroy()
        {
            DisposeInternal();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            modules = GetComponents<BaseModule>();
        }
    }
}