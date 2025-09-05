using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы с компонентами
    /// </summary>
    public static class VrfComponentExtensions
    {
        public static void CopyComponentTo<T>(this T origin, T target) where T : Component
        {
            var type1 = origin.GetType();
            var type2 = target.GetType();
            
            if (type1 != type2)
            {
                Debug.LogError($"The type \"{type1.AssemblyQualifiedName}\" of \"{origin}\" does not match the type \"{type2.AssemblyQualifiedName}\" of \"{target}\"!");
                return;
            }

            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
            var propertyInfos = type1.GetProperties(flags);

            foreach (var propertyInfo in propertyInfos) 
            {
                if (propertyInfo.CanWrite) 
                {
                    try
                    { propertyInfo.SetValue(target, propertyInfo.GetValue(origin, null), null); }
                    catch { /* ignored */ }
                }
            }

            var fieldInfos = type1.GetFields(flags);
            foreach (var fieldInfo in fieldInfos)
                fieldInfo.SetValue(target, fieldInfo.GetValue(origin));
        }
        
        public static bool IsEmptyComponents(this Component self) => self.GetComponents<Component>().Length == 1;
        
        public static bool HasComponent<T>(this Component component)
        {
            return component.TryGetComponent<T>(out _);
        }

        public static bool HasComponentInParent<T>(this Component component)
        {
            return component.GetComponentInParent<T>() != null;
        }

        public static bool HasComponentInChildren<T>(this Component component)
        {
            return component.GetComponentInChildren<T>() != null;
        }

        public static void Destroy(this Component component)
        {
            Object.Destroy(component);
        }
        public static void DestroyImmediate(this Component component, bool allowDestroyingAssets = false)
        {
            Object.DestroyImmediate(component, allowDestroyingAssets);
        }
        public static int Destroy(this IEnumerable<Component> components)
        {
            var counter = 0;
            foreach (var component in components)
            {
                Object.Destroy(component);
                counter++;
            }
            return counter;
        }
        public static int DestroyImmediate(this IEnumerable<Component> components, bool allowDestroyingAssets = false)
        {
            var counter = 0;
            foreach (var component in components)
            {
                Object.DestroyImmediate(component, allowDestroyingAssets);
                counter++;
            }
            return counter;
        }

        public static Component FindByName(this IEnumerable<Component> components, string name)
        {
            return components.FirstOrDefault(component => component.name == name);
        }
    }
}