using System;
using System.Collections.Generic;
using UnityEngine;
using VRF.Components;
using VRF.Utilities.Extensions;

namespace VRF.Players.Services.Views
{
    /// <summary>
    /// Контейнер всех созданных view объектов, заноситься через view spawner (но можно и вручную)
    /// </summary>
    public class ViewsSpawnedContainer
    {
        // Использует RuntimeHashCode
        private readonly Dictionary<int, BaseView> views = new();
        // Использует AssetHashCode
        private readonly Dictionary<int, List<BaseView>> groups = new();

        public event Action<BaseView> OnAdd; 
        public event Action<BaseView> OnRemove; 
        
        public void Add(GameObject obj) => Add(obj.GetComponent<BaseView>());
        public void Remove(GameObject obj) => Remove(obj.GetComponent<BaseView>());
        
        public void Add(Component component) => Add(component.GetComponent<BaseView>());
        public void Remove(Component component) => Remove(component.GetComponent<BaseView>());
        
        public void Add(BaseView view)
        {
            views.Add(view.RuntimeHashCode, view);
            groups.TryAddValue(view.AssetHashCode, view);
            OnAdd?.Invoke(view);
        }
        public void Remove(BaseView view)
        {
            views.Remove(view.RuntimeHashCode);
            groups.TryRemoveValue(view.AssetHashCode, view);
            OnRemove?.Invoke(view);
        }

        public BaseView GetSpawnedView(int runtimeHashCode)
            => views[runtimeHashCode];
        public TView GetSpawnedView<TView>(int runtimeHashCode) 
            where TView : BaseView 
            => (TView)views[runtimeHashCode];
    }
}