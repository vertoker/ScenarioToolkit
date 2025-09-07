using System.Collections.Generic;
using JetBrains.Annotations;
using ZLinq;

namespace ScenarioToolkit.Shared.Extensions
{
    public static class CollectionsExtensions
    {
        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> elements)
        {
            foreach (var element in elements)
                hashSet.Add(element);
        }
        public static void AddRange<TEnumerator, T>(this HashSet<T> hashSet, ValueEnumerable<TEnumerator, T> elements) 
            where TEnumerator : struct, IValueEnumerator<T>
        {
            foreach (var element in elements)
                hashSet.Add(element);
        }
        
        public static void AddRange<TEnumerator, T>(this List<T> list, ValueEnumerable<TEnumerator, T> elements) 
            where TEnumerator : struct, IValueEnumerator<T>
        {
            foreach (var element in elements)
                list.Add(element);
        }
        
        public static void MixVariables<TKey, TValue>(this Dictionary<TKey, TValue> source, 
            [CanBeNull] Dictionary<TKey, TValue> overrideVariables)
        {
            if (overrideVariables != null)
                foreach (var newVariable in overrideVariables)
                    source[newVariable.Key] = newVariable.Value;
        }
    }
}