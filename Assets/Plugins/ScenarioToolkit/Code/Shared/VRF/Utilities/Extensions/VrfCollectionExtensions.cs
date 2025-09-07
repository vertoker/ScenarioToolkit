using System;
using System.Collections.Generic;

namespace ScenarioToolkit.Shared.VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы с коллекциями
    /// </summary>
    public static class VrfCollectionExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> source, T item) where T : class
        {
            var enumerator = source.GetEnumerator();

            var counter = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current == item)
                {
                    enumerator.Dispose();
                    return counter;
                }
                counter++;
            }
            
            enumerator.Dispose();
            return -1;
        }
        
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            var enumerator = source.GetEnumerator();

            var counter = 0;
            while (enumerator.MoveNext())
            {
                if (predicate(enumerator.Current))
                {
                    enumerator.Dispose();
                    return counter;
                }
                counter++;
            }
            
            enumerator.Dispose();
            return -1;
        }
        
        public static int IndexOfEquals<T>(this IEnumerable<T> source, T item)
        {
            var enumerator = source.GetEnumerator();

            var counter = 0;
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (current != null && current.Equals(item))
                {
                    enumerator.Dispose();
                    return counter;
                }
                counter++;
            }
            
            enumerator.Dispose();
            return -1;
        }
    }
}