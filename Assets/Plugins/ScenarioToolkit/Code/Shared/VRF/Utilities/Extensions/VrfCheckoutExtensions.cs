using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScenarioToolkit.Shared.VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения, позволяющие обходить иерархию Transform'ов и применять функции к каждому объекту
    /// </summary>
    public static class VrfCheckoutExtensions
    {
        public static void CheckoutAll(this IEnumerable<Transform> parents, Func<Transform, bool> func)
        {
            foreach (var parent in parents)
            {
                CheckoutAll(parent, func);
            }
        }
        
        public static void CheckoutAll(this Transform parent, Func<Transform, bool> func)
        {
            var objects = new Stack<Transform>();
            objects.Push(parent);

            while (objects.TryPop(out var obj))
            {
                if (func.Invoke(obj))
                {
                    var count = obj.childCount;
                    for (var i = 0; i < count; i++)
                        objects.Push(obj.GetChild(i));
                }
            }
        }

        public static void CheckoutAll<T>(this IEnumerable<Transform> parents, Func<T, bool> func) where T : Component
        {
            foreach (var parent in parents)
            {
                CheckoutAll(parent, func);
            }
        }
        
        public static void CheckoutAll<T>(this Transform parent, Func<T, bool> func) where T : Component
        {
            bool Func(Transform obj)
            {
                foreach (var component in obj.GetComponents<T>())
                    func.Invoke(component);
                return true;
            }

            CheckoutAll(parent, Func);
        }
    }
}