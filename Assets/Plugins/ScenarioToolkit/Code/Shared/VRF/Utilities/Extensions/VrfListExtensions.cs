using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы со списками
    /// </summary>
    public static class VrfListExtensions
    {
        public static void MoveItem<T>(this List<T> list, int sourceIndex, int destinationIndex)
        {
            if (sourceIndex == destinationIndex) return;
            list.Insert(destinationIndex, list[sourceIndex]);
            list.RemoveAt(destinationIndex < sourceIndex ? sourceIndex + 1 : sourceIndex);
        }
        public static void SwapItems<T>(this List<T> list, int index1, int index2)
        {
            if (index1 == index2) return;
            (list[index1], list[index2]) = (list[index2], list[index1]);
        }

        public static void EnsureCapacity<T>(this List<T> list, int capacity)
        {
            if (list.Capacity < capacity)
                list.Capacity = capacity;
        }
        public static void EnsureCount<T>(this List<T> list, int count, T element)
        {
            if (list.Count < count)
                list.AddRange(Enumerable.Repeat(element, count - list.Count));
            else if (list.Count > count)
            {
                var toRemove = list.Count - count;
                if (toRemove <= 0 || toRemove >= list.Count) return;
                list.RemoveRange(list.Count - toRemove, toRemove);
            }
        }
        public static void EnsureCount<T>(this List<T> list, int count, int capacity, T element)
        {
            if (count > capacity)
            {
                Debug.LogError("Count can't be bigger than capacity");
                return;
            }
            
            if (list.Capacity < capacity)
                list.Capacity = capacity;
            list.EnsureCount(count, element);
        }
    }
}