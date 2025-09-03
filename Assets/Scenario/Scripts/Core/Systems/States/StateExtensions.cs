using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scenario.Core.Systems.States
{
    public static class StateExtensions
    {
        public static void SetStateActivity<TKey>(this Dictionary<TKey, bool> activityContainer, TKey key, bool last, bool next)
        {
            // Если значение поменялось
            if (last == next) return;
            // То происходит замена наличия
            // Если его нет, то добавляется
            // Если он есть, то он удаляется
            if (!activityContainer.Remove(key))
                activityContainer.Add(key, next);
        }
        
        public static void SetStateData<TKey, TBind>(this Dictionary<TKey, (TBind, TBind)> container, 
            TKey key, TBind bindDefault, TBind bindCurrent) where TBind : IEquatable<TBind>
        {
            // Если есть override для конкретного TKey
            if (container.TryGetValue(key, out var binds))
            {
                // Если новое значение равно default
                if (binds.Item1.Equals(bindCurrent))
                    // Default = Current, значит override никогда не существовало
                    container.Remove(key);
                else
                {
                    // Перезапись current
                    container[key] = (binds.Item1, bindCurrent);
                }
            }
            else
            {
                container[key] = (bindDefault, bindCurrent);
            }
        }
        
        public static void SetStateData<TKey, TData>(this Dictionary<TKey, TData> container, 
            Dictionary<TKey, TData> defaultContainer,
            TKey key, TData dataDefault, TData dataCurrent) where TData : IEquatable<TData>
        {
            // Если есть override для конкретного TKey
            if (container.TryGetValue(key, out var data))
            {
                // Если новое значение равно default
                if (data.Equals(dataDefault))
                {
                    // Default = Current, значит override никогда не существовало
                    container.Remove(key);
                    defaultContainer.Remove(key);
                }
                else
                {
                    // Перезапись current
                    container[key] = dataCurrent;
                }
            }
            else
            {
                // создать current и default
                container[key] = dataCurrent;
                defaultContainer[key] = dataDefault;
            }
        }

        public static bool AssertAdd<TKey>(this HashSet<TKey> hashSet, TKey key)
        {
            if (hashSet.Add(key)) return false;
            Debug.LogWarning($"Can't add {nameof(TKey)}, it's already added", null);
            return true;
        }

        public static bool AssertRemove<TKey>(this HashSet<TKey> hashSet, TKey key)
        {
            if (hashSet.Remove(key)) return false;
            Debug.LogWarning($"Can't remove {nameof(TKey)}, can't find it", null);
            return true;
        }
    }
}