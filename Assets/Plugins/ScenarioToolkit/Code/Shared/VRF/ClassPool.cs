using UnityEngine;
using UnityEngine.Pool;

namespace VRF.Utils.Pool
{
    /// <summary>
    /// Пул для созданных классов, это просто абстракция для удобства работы пула, не имеет дополнительного
    /// кода для "включения" или "отключения" объектов
    /// </summary>
    /// <typeparam name="TClass">Любой класс (который можно создать без параметров)</typeparam>
    public class ClassPool<TClass> : IObjectPool<TClass> where TClass : class, new()
    {
        private readonly Transform parent;
        
        private readonly ObjectPool<TClass> pool;

        public ClassPool(int startCapacity = 8)
        {
            pool = new ObjectPool<TClass>(Create, null, null, null, true, startCapacity);
        }
        
        private TClass Create() => new();

        public TClass Get()
        {
            return pool.Get();
        }
        public PooledObject<TClass> Get(out TClass v)
        {
            return pool.Get(out v);
        }
        public void Release(TClass element)
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