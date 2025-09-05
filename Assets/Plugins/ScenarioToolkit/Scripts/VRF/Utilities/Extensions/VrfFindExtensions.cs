using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения с множеством методов поиска для разных типов, перечислений
    /// </summary>
    public static class VrfFindExtensions
    {
        #region FindAll NonRecursive
        public static List<T> FindAllNonRecursive<T>(this IEnumerable<Transform> parents)
        {
            var list = new List<T>();
            foreach (var parent in parents)
                FindAllNonRecursive(parent, list);
            return list;
        }
        public static List<T> FindAllNonRecursive<T>(this Transform parent)
        {
            var list = new List<T>();
            FindAllNonRecursive(parent, list);
            return list;
        }
        public static List<T> FindAllNonRecursive<T>(this IEnumerable<Transform> parents, Func<Transform, bool> predicate)
        {
            var list = new List<T>();
            foreach (var parent in parents)
                FindAllNonRecursive(parent, list, predicate);
            return list;
        }
        public static List<T> FindAllNonRecursive<T>(this Transform parent, Func<Transform, bool> predicate)
        {
            var list = new List<T>();
            FindAllNonRecursive(parent, list, predicate);
            return list;
        }
        
        public static List<T> FindAllActiveSelfNonRecursive<T>(this IEnumerable<Transform> parents) 
            => parents.FindAllNonRecursive<T>(t => t.gameObject.activeSelf);
        public static List<T> FindAllActiveSelfNonRecursive<T>(this Transform parent)
            => parent.FindAllNonRecursive<T>(t => t.gameObject.activeSelf);
        public static List<T> FindAllActiveHierarchyNonRecursive<T>(this IEnumerable<Transform> parents)
            => parents.FindAllNonRecursive<T>(t => t.gameObject.activeInHierarchy);
        public static List<T> FindAllActiveHierarchyNonRecursive<T>(this Transform parent)
            => parent.FindAllNonRecursive<T>(t => t.gameObject.activeInHierarchy);
        
        public static void FindAllNonRecursive<T>(this Transform parent, List<T> output)
        {
            void Func(Component component) => output.AddRange(component.GetComponents<T>());
            
            Func(parent);
            var length = parent.childCount;
            for (var i = 0; i < length; i++)
                Func(parent.GetChild(i));
        }
        public static void FindAllNonRecursive<T>(this Transform parent, List<T> output, Func<Transform, bool> predicate)
        {
            void Func(Transform tr)
            {
                if (predicate.Invoke(tr))
                    output.AddRange(tr.GetComponents<T>());
            }
            
            Func(parent);
            var length = parent.childCount;
            for (var i = 0; i < length; i++)
                Func(parent.GetChild(i));
        }
        #endregion

        #region FindAll Generic
        public static List<T> FindAll<T>(this IEnumerable<Transform> parents)
        {
            var list = new List<T>();
            foreach (var parent in parents)
                FindAll(parent, list);
            return list;
        }
        public static List<T> FindAll<T>(this Transform parent)
        {
            var list = new List<T>();
            FindAll(parent, list);
            return list;
        }
        public static List<T> FindAll<T>(this IEnumerable<Transform> parents, Func<Transform, bool> predicate)
        {
            var list = new List<T>();
            foreach (var parent in parents)
                FindAll(parent, list, predicate);
            return list;
        }
        public static List<T> FindAll<T>(this Transform parent, Func<Transform, bool> predicate)
        {
            var list = new List<T>();
            FindAll(parent, list, predicate);
            return list;
        }
        
        public static List<T> FindAllActiveSelf<T>(this IEnumerable<Transform> parents)
            => parents.FindAll<T>(t => t.gameObject.activeSelf);
        public static List<T> FindAllActiveSelf<T>(this Transform parent)
            => parent.FindAll<T>(t => t.gameObject.activeSelf);
        public static List<T> FindAllActiveHierarchy<T>(this IEnumerable<Transform> parents)
            => parents.FindAll<T>(t => t.gameObject.activeInHierarchy);
        public static List<T> FindAllActiveHierarchy<T>(this Transform parent)
            => parent.FindAll<T>(t => t.gameObject.activeInHierarchy);
        
        public static void FindAll<T>(this Transform parent, List<T> output)
        {
            bool Func(Transform obj)
            {
                output.AddRange(obj.GetComponents<T>());
                return true;
            }

            parent.CheckoutAll(Func);
        }
        public static void FindAll<T>(this Transform parent, List<T> output, Func<Transform, bool> predicate)
        {
            bool Func(Transform obj)
            {
                if (predicate.Invoke(obj))
                {
                    output.AddRange(obj.GetComponents<T>());
                    return true;
                }
                return false;
            }

            parent.CheckoutAll(Func);
        }
        #endregion

        #region FindAll
        public static List<Component> FindAll(this IEnumerable<Transform> parents, Type type)
        {
            var list = new List<Component>();
            foreach (var parent in parents)
                FindAll(parent, list, type);
            return list;
        }
        public static List<Component> FindAll(this Transform parent, Type type)
        {
            var list = new List<Component>();
            FindAll(parent, list, type);
            return list;
        }
        public static List<Component> FindAll(this IEnumerable<Transform> parents, Type type, Func<Transform, bool> predicate)
        {
            var list = new List<Component>();
            foreach (var parent in parents)
                FindAll(parent, list, type, predicate);
            return list;
        }
        public static List<Component> FindAll(this Transform parent, Type type, Func<Transform, bool> predicate)
        {
            var list = new List<Component>();
            FindAll(parent, list, type, predicate);
            return list;
        }
        
        public static List<Component> FindAllActiveSelf(this IEnumerable<Transform> parents, Type type) 
            => parents.FindAll(type, t => t.gameObject.activeSelf);
        public static List<Component> FindAllActiveSelf(this Transform parent, Type type)
            => parent.FindAll(type, t => t.gameObject.activeSelf);
        public static List<Component> FindAllActiveHierarchy(this IEnumerable<Transform> parents, Type type)
            => parents.FindAll(type, t => t.gameObject.activeInHierarchy);
        public static List<Component> FindAllActiveHierarchy(this Transform parent, Type type)
            => parent.FindAll(type, t => t.gameObject.activeInHierarchy);
        
        public static void FindAll(this Transform parent, List<Component> output, Type type)
        {
            bool Func(Transform obj)
            {
                output.AddRange(obj.GetComponents(type));
                return true;
            }

            parent.CheckoutAll(Func);
        }
        public static void FindAll(this Transform parent, List<Component> output, Type type, Func<Transform, bool> predicate)
        {
            bool Func(Transform obj)
            {
                if (predicate.Invoke(obj))
                {
                    output.AddRange(obj.GetComponents(type));
                    return true;
                }
                return false;
            }

            parent.CheckoutAll(Func);
        }
        #endregion
    }
}