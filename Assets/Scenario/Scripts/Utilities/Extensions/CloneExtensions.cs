using System.Collections.Generic;

namespace Scenario.Utilities.Extensions
{
    public static class CloneExtensions
    {
        public static HashSet<T> Clone<T>(this HashSet<T> source) => Clone(source, source.Count);
        public static HashSet<T> Clone<T>(this HashSet<T> source, int capacity)
        {
            var result = new HashSet<T>(capacity);
            foreach (var item in source)
                result.Add(item);
            return result;
        }
        
        public static List<T> Clone<T>(this List<T> source) => Clone(source, source.Capacity);
        public static List<T> Clone<T>(this List<T> source, int capacity)
        {
            var result = new List<T>(capacity);
            result.AddRange(source);
            return result;
        }

        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> source) => Clone(source, source.Count);
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> source, int capacity)
        {
            var result = new Dictionary<TKey, TValue>(capacity);
            foreach (var pair in source)
                result.Add(pair.Key, pair.Value);
            return result;
        }
    }
}