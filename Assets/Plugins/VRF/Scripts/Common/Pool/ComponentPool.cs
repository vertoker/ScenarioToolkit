using UnityEngine;
using UnityEngine.Pool;

namespace VRF.Utils.Pool
{
    /// <summary>
    /// Надстройка над стандартным Unity Pool классом, который реализует распространённый пул,
    /// а именно обычный GameObject и его включение/отключение своего GameObject при взаимодействии с пулом
    /// </summary>
    /// <typeparam name="TComponent">Любой Unity Component</typeparam>
    public class ComponentPool<TComponent> : IObjectPool<TComponent> where TComponent : Component
    {
        private readonly TComponent sourceComponent;
        private readonly Transform parent;
        
        private readonly ObjectPool<TComponent> pool;

        public ComponentPool(TComponent sourceComponent, Transform parent = null, int startCapacity = 8)
        {
            this.sourceComponent = sourceComponent;
            this.parent = parent;
            
            pool = new ObjectPool<TComponent>(Create, OnGet, OnRelease, OnDestroy, true, startCapacity);
        }
        
        private TComponent Create()
        {
            var component = Object.Instantiate(sourceComponent, parent);
            //component.gameObject.SetActive(false);
            return component;
        }
        private void OnGet(TComponent component)
        {
            component.gameObject.SetActive(true);
        }
        private void OnRelease(TComponent component)
        {
            component.gameObject.SetActive(false);
        }
        private void OnDestroy(TComponent component)
        {
            Object.Destroy(component.gameObject);
        }

        public TComponent Get()
        {
            return pool.Get();
        }
        public PooledObject<TComponent> Get(out TComponent v)
        {
            return pool.Get(out v);
        }
        public void Release(TComponent element)
        {
            pool.Release(element);
        }
        public void Clear()
        {
            pool.Clear();
        }
        public int CountInactive => pool.CountInactive;
    }
}