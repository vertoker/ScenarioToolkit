using System;
using System.Collections.Generic;

namespace Scenario.Utilities.Collections
{
    /// <summary>
    ///     This is a dictionary guaranteed to have only one of each value and key.
    ///     It may be searched either by TFirst or by TSecond, giving a unique answer because it is 1 to 1.
    /// </summary>
    /// <typeparam name="TFirst">The type of the "key"</typeparam>
    /// <typeparam name="TSecond">The type of the "value"</typeparam>
    public class BiDictionary<TFirst, TSecond>
    {
        private readonly Dictionary<TFirst, TSecond> firstToSecond = new();
        private readonly Dictionary<TSecond, TFirst> secondToFirst = new();

        public IReadOnlyDictionary<TFirst, TSecond> FirstToSecond => firstToSecond;
        public IReadOnlyDictionary<TSecond, TFirst> SecondToFirst => secondToFirst;

        public Dictionary<TFirst, TSecond> GetFirstToSecond() => firstToSecond;
        public Dictionary<TSecond, TFirst> GetSecondToFirst() => secondToFirst;
        
        public int Count => firstToSecond.Count;
        
        public void Clear()
        {
            firstToSecond.Clear();
            secondToFirst.Clear();
        }
        
        public void Add(TFirst first, TSecond second)
        {
            if (firstToSecond.ContainsKey(first) || secondToFirst.ContainsKey(second))
                throw new ArgumentException("Duplicate first or second");

            firstToSecond.Add(first, second);
            secondToFirst.Add(second, first);
        }
        public TSecond GetByFirst(TFirst first)
        {
            if (!firstToSecond.TryGetValue(first, out var second))
                throw new KeyNotFoundException(typeof(TFirst).Name);
            return second;
        }
        public TFirst GetBySecond(TSecond second)
        {
            if (!secondToFirst.TryGetValue(second, out var first))
                throw new KeyNotFoundException(typeof(TSecond).Name);
            return first;
        }
        public void RemoveByFirst(TFirst first)
        {
            if (!firstToSecond.Remove(first, out var second))
                throw new ArgumentException("first");

            secondToFirst.Remove(second);
        }
        public void RemoveBySecond(TSecond second)
        {
            if (!secondToFirst.Remove(second, out var first))
                throw new ArgumentException("second");

            firstToSecond.Remove(first);
        }
        
        
        public bool TryAdd(TFirst first, TSecond second)
        {
            if (firstToSecond.ContainsKey(first) || secondToFirst.ContainsKey(second))
                return false;

            firstToSecond.Add(first, second);
            secondToFirst.Add(second, first);
            return true;
        }
        public bool TryGetByFirst(TFirst first, out TSecond second) => firstToSecond.TryGetValue(first, out second);
        public bool TryGetBySecond(TSecond second, out TFirst first) => secondToFirst.TryGetValue(second, out first);

        public bool TryRemoveByFirst(TFirst first)
        {
            if (!firstToSecond.Remove(first, out var second))
                return false;
            secondToFirst.Remove(second);
            return true;
        }
        public bool TryRemoveBySecond(TSecond second)
        {
            if (!secondToFirst.Remove(second, out var first))
                return false;
            firstToSecond.Remove(first);
            return true;
        }
    }
}