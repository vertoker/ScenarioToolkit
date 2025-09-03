using System;
using System.Collections.Generic;
using UnityEngine;
using VRF.Components;
using Zenject;
using Object = UnityEngine.Object;

namespace VRF.Players.Services.Views
{
    /// <summary> Спаунер/деспаунер всех View объектов в VRF. Можно указать родителя для определённого типа.
    /// Заточен под систему View и автоматически добавляет их в контейнер заспауненых </summary>
    public abstract class BaseViewSpawnerService
    {
        private readonly IInstantiator instantiator;
        private readonly ViewsSpawnedContainer container;
        private readonly Dictionary<Type, Transform> registeredParents;

        protected BaseViewSpawnerService(IInstantiator instantiator, ViewsSpawnedContainer container)
        {
            this.instantiator = instantiator;
            this.container = container;
            registeredParents = new Dictionary<Type, Transform>();
        }
        
        public TView SpawnView<TView>(TView source) where TView : BaseView
        {
            var parent = GetParent(typeof(TView));
            var prefab = instantiator.InstantiatePrefab(source.gameObject, 
                parent.position, parent.rotation, parent);
            
            var view = prefab.GetComponent<TView>();
            container.Add(view);
            return view;
        }
        public void DestroyView<TView>(TView instantiated) where TView : BaseView
        {
            container.Remove(instantiated);
            Object.Destroy(instantiated.gameObject);
        }
        public void DestroyView(GameObject instantiated)
        {
            if (instantiated.TryGetComponent<BaseView>(out var view))
                container.Remove(view);
            Object.Destroy(instantiated);
        }
        
        public GameObject Spawn<TComponentGroup>(GameObject source) where TComponentGroup : Component
        {
            var parent = GetParent(typeof(TComponentGroup));
            return instantiator.InstantiatePrefab(source, parent.position, parent.rotation, parent);
        }

        private Transform GetParent(Type type)
        {
            if (!registeredParents.TryGetValue(type, out var parent))
            {
                var name = $"{type.Name}";
                parent = new GameObject(name).transform;
                registeredParents.Add(type, parent);
            }
            
            return parent;
        }
        
        /// <summary> Добавить определённого родителя под определённый тип </summary>
        public void RegisterParent<TComponent>(Transform spawnPoint) where TComponent : Component
        {
            registeredParents.Add(typeof(TComponent), spawnPoint);
        }
        /// <summary> Убрать определённого родителя по определённому типу </summary>
        public void UnregisterParent<TComponent>() where TComponent : Component
        {
            registeredParents.Remove(typeof(TComponent));
        }
    }
}