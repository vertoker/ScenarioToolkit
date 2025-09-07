using System;
using System.Collections.Generic;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы со словарями
    /// </summary>
    public static class VrfDictionaryExtensions
    {
        public static void SwapItems<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key1, TKey key2) where TKey : class
        {
            if (key1 == key2) return;
            (dictionary[key1], dictionary[key2]) = (dictionary[key2], dictionary[key1]);
        }
        public static void SwapItemsEquals<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key1, TKey key2)
        {
            if (key1.Equals(key2)) return;
            (dictionary[key1], dictionary[key2]) = (dictionary[key2], dictionary[key1]);
        }
        
        public static bool TryAddValue<TKey, TValue>(this Dictionary<Type, List<TValue>> dictionary, TValue value)
        {
            var key = typeof(TKey);
            return dictionary.TryAddValue(key, value);
        }
        public static bool TryRemoveValue<TKey, TValue>(this Dictionary<Type, List<TValue>> dictionary, TValue value)
        {
            var key = typeof(TKey);
            return dictionary.TryRemoveValue(key, value);
        }
        
        public static bool TryAddValue<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
        {
            if (!dictionary.TryGetValue(key, out var list))
            {
                list = new List<TValue>();
                dictionary.Add(key, list);
            }
            list.Add(value);
            return true;
        }
        public static bool TryRemoveValue<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
        {
            if (!dictionary.TryGetValue(key, out var list)) return false;
            if (!list.Remove(value)) return false;
            if (list.Count == 0) dictionary.Remove(key);
            return true;
        }
    }
}