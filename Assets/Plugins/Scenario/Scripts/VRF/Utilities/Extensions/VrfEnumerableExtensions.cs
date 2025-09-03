using System;
using System.Collections.Generic;
using System.Linq;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы с перечислениями
    /// </summary>
    public static class VrfEnumerableExtensions
    {
        public static List<T> Merge<T>(this IEnumerable<IEnumerable<T>> enumerables, int capacity = 4)
        {
            var list = new List<T>(capacity);
            
            foreach (var enumerable2 in enumerables)
                list.AddRange(enumerable2);

            return list;
        }
        
        public static List<T> Concat<T>(this IEnumerable<T> enumerable, params IEnumerable<T>[] enumerables)
        {
            var list = new List<T>();
            
            list.AddRange(enumerable);
            foreach (var enumerable2 in enumerables)
                list.AddRange(enumerable2);

            return list;
        }
        
        public static List<T> ConcatArray<T>(params IEnumerable<T>[] enumerables)
        {
            var list = new List<T>();
            
            foreach (var enumerable2 in enumerables)
                list.AddRange(enumerable2);

            return list;
        }
        public static List<T> ConcatArray<T>(int capacity, params IEnumerable<T>[] enumerables)
        {
            var list = new List<T>(capacity);
            
            foreach (var enumerable2 in enumerables)
                list.AddRange(enumerable2);

            return list;
        }

        public static KeyValuePair<TItem, float> MinItem<TItem>(this IEnumerable<TItem> enumerable, Func<TItem, float> predicator)
        {
            using var enumerator = enumerable.GetEnumerator();

            var minValue = float.MaxValue;
            var minItem = default(TItem);
            
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                var value = predicator(item);
                
                if (value < minValue)
                {
                    minValue = value;
                    minItem = item;
                }
            }
            
            return new KeyValuePair<TItem, float>(minItem, minValue);
        }
        public static KeyValuePair<TItem, float> MaxItem<TItem>(this IEnumerable<TItem> enumerable, Func<TItem, float> predicator)
        {
            using var enumerator = enumerable.GetEnumerator();

            var maxValue = float.MinValue;
            var maxItem = default(TItem);
            
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                var value = predicator(item);
                
                if (value > maxValue)
                {
                    maxValue = value;
                    maxItem = item;
                }
            }
            
            return new KeyValuePair<TItem, float>(maxItem, maxValue);
        }
        public static KeyValuePair<TItem, int> MinItem<TItem>(this IEnumerable<TItem> enumerable, Func<TItem, int> predicator)
        {
            using var enumerator = enumerable.GetEnumerator();

            var minValue = int.MaxValue;
            var minItem = default(TItem);
            
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                var value = predicator(item);
                
                if (value < minValue)
                {
                    minValue = value;
                    minItem = item;
                }
            }
            
            return new KeyValuePair<TItem, int>(minItem, minValue);
        }
        public static KeyValuePair<TItem, int> MaxItem<TItem>(this IEnumerable<TItem> enumerable, Func<TItem, int> predicator)
        {
            using var enumerator = enumerable.GetEnumerator();

            var maxValue = int.MinValue;
            var maxItem = default(TItem);
            
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                var value = predicator(item);
                
                if (value > maxValue)
                {
                    maxValue = value;
                    maxItem = item;
                }
            }
            
            return new KeyValuePair<TItem, int>(maxItem, maxValue);
        }
        
        private static readonly Random Rng = new();  
        
        // https://stackoverflow.com/questions/273313/randomize-a-listt
        public static IOrderedEnumerable<T> Shuffle<T>(this IEnumerable<T> list) => list.OrderBy(_ => Rng.Next());
    }
}