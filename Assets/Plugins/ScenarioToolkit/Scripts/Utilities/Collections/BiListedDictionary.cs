using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario.Utilities.Collections
{
    public class BiListedDictionary<TKey, TListedValue>
    {
        private readonly Dictionary<TKey, HashSet<TListedValue>> keyToListedValues = new();
        private readonly Dictionary<TListedValue, TKey> listedValueToKey = new();

        public IEnumerable<TKey> Keys => keyToListedValues.Keys;
        public IReadOnlyCollection<TListedValue> GetReadOnlyListedValues(TKey key) => keyToListedValues[key];
        public IReadOnlyDictionary<TListedValue, TKey> ListedValueToKey => listedValueToKey;

        public Dictionary<TKey, HashSet<TListedValue>> GetKeyToListedValue() => keyToListedValues;
        public Dictionary<TListedValue, TKey> GetListedValueToKey() => listedValueToKey;
        
        public int CountKeys => keyToListedValues.Count;
        public int CountListedValues => keyToListedValues.Count;
        
        public void Clear()
        {
            keyToListedValues.Clear();
            listedValueToKey.Clear();
        }
        
        
        public void AddKey(TKey key)
        {
            keyToListedValues.Add(key, new HashSet<TListedValue>());
        }
        public void AddListedValue(TKey key, TListedValue listedValue)
        {
            if (listedValueToKey.ContainsKey(listedValue))
                throw new ArgumentException("Duplicate listed value");

            if (!keyToListedValues.TryGetValue(key, out var listedValues))
            {
                listedValues = new HashSet<TListedValue>();
                keyToListedValues.Add(key, listedValues);
            }
            else if (listedValues.Contains(listedValue))
                throw new ArgumentException("Duplicate listed value");
            
            listedValues.Add(listedValue);
            listedValueToKey.Add(listedValue, key);
        }
        
        public HashSet<TListedValue> GetByKey(TKey key)
        {
            if (!keyToListedValues.TryGetValue(key, out var listedValues))
                throw new KeyNotFoundException(typeof(TKey).Name);
            return listedValues;
        }
        public TKey GetByListedValue(TListedValue listedValue)
        {
            if (!listedValueToKey.TryGetValue(listedValue, out var key))
                throw new KeyNotFoundException(typeof(TListedValue).Name);
            return key;
        }
        
        public void RemoveKey(TKey key)
        {
            if (!keyToListedValues.Remove(key, out var listedValues))
                throw new ArgumentException("key");

            foreach (var second in listedValues)
                listedValueToKey.Remove(second);
        }
        public void RemoveListedValue(TListedValue listedValue)
        {
            if (!listedValueToKey.Remove(listedValue, out var key))
                throw new ArgumentException("key");

            var listedValues = keyToListedValues[key];
            if (!listedValues.Remove(listedValue))
                throw new ArgumentException("listed value");
        }
        
        public bool TryAddKey(TKey key) => keyToListedValues.TryAdd(key, new HashSet<TListedValue>());
        public bool TryAddListedValue(TKey key, TListedValue listedValue)
        {
            if (listedValueToKey.ContainsKey(listedValue)) return false;

            if (!keyToListedValues.TryGetValue(key, out var listedValues))
            {
                listedValues = new HashSet<TListedValue>();
                keyToListedValues.Add(key, listedValues);
            }
            else if (listedValues.Contains(listedValue)) return false;
            
            listedValues.Add(listedValue);
            listedValueToKey.Add(listedValue, key);
            return true;
        }

        public bool TryGetByKey(TKey key, out HashSet<TListedValue> listedValues) => keyToListedValues.TryGetValue(key, out listedValues);
        public bool TryGetByListedValue(TListedValue listedValue, out TKey key) => listedValueToKey.TryGetValue(listedValue, out key);
        
        public bool TryRemoveKey(TKey key)
        {
            if (!keyToListedValues.Remove(key, out var listedValues)) return false;

            foreach (var second in listedValues)
                listedValueToKey.Remove(second);
            return true;
        }
        public bool TryRemoveListedValue(TListedValue listedValue)
        {
            if (!listedValueToKey.Remove(listedValue, out var key)) return false;

            var listedValues = keyToListedValues[key];
            return listedValues.Remove(listedValue);
        }
    }
}