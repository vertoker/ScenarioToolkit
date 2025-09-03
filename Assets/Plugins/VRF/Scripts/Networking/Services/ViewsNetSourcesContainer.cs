using System.Collections.Generic;
using UnityEngine;
using VRF.Components;

namespace VRF.Networking.Services
{
    /// <summary>
    /// Контейнер всех исходных view, заполняется из RegisterNetPrefabs
    /// </summary>
    public class ViewsNetSourcesContainer
    {
        // Использует AssetHashCode
        private readonly Dictionary<int, BaseView> sources = new();
        
        public void Add(GameObject obj) => Add(obj.GetComponent<BaseView>());
        public void Remove(GameObject obj) => Remove(obj.GetComponent<BaseView>());
        
        public void Add(Component component) => Add(component.GetComponent<BaseView>());
        public void Remove(Component component) => Remove(component.GetComponent<BaseView>());
        
        public void Add(BaseView view) => sources.Add(view.AssetHashCode, view);
        public void Remove(BaseView view) => sources.Remove(view.AssetHashCode);
        public void Clear() => sources.Clear();

        public BaseView GetAssetView(int assetHashCode) => sources[assetHashCode];
        public TView GetAssetView<TView>(int assetHashCode) where TView : BaseView => (TView)sources[assetHashCode];
    }
}