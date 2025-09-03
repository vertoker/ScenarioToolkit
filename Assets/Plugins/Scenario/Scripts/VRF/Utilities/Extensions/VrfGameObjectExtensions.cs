using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы с GameObject'ами и их коллекциями
    /// </summary>
    public static class VrfGameObjectExtensions
    {
        public static GameObject FindByName(this IEnumerable<GameObject> gameObjects, string name)
        {
            return gameObjects.FirstOrDefault(gameObject => gameObject.name == name);
        }
        public static GameObject FindByNameConsiderNull(this IEnumerable<GameObject> gameObjects, string name)
        {
            return gameObjects.FirstOrDefault(gameObject => gameObject != null && gameObject.name == name);
        }

        public static Transform[] GetChildren(this GameObject obj)
            => obj.transform.GetChildren();
        public static GameObject[] GetChildrenGameObjects(this GameObject obj)
            => obj.transform.GetChildrenGameObjects();
        public static List<GameObject> GetChildrenGameObjectsRecursively(this GameObject obj)
            => obj.transform.GetChildrenGameObjectsRecursively();
        public static List<Transform> GetChildrenRecursively(this GameObject obj)
            => obj.transform.GetChildrenRecursively();
        
        public static bool EnsureComponent<T>(this GameObject gameObject) where T : Component
        {
            if (!gameObject.TryGetComponent<T>(out _))
            {
                VrfRuntimeEditor.SetDirty(gameObject.AddComponent<T>());
                return true;
            }
            return false;
        }
        public static bool EnsureComponent<T>(this GameObject gameObject, out T component) where T : Component
        {
            if (!gameObject.TryGetComponent(out component))
            {
                component = gameObject.AddComponent<T>();
                VrfRuntimeEditor.SetDirty(component);
                return true;
            }
            return false;
        }
        
        public static void Destroy(this GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
        public static void DestroyImmediate(this GameObject gameObject)
        {
            Object.DestroyImmediate(gameObject);
        }

        private static readonly Type TransformType = typeof(Transform);
        private static bool Filter(Component component, IList<Type> toDestroy)
        {
            var componentType = component.GetType().UnderlyingSystemType;
            return componentType != TransformType && toDestroy.Any(type => type == componentType);
        }
        private static bool FilterNot(Component component, IList<Type> toStay)
        {
            var componentType = component.GetType().UnderlyingSystemType;
            return componentType != TransformType && toStay.All(type => type != componentType);
        }
        
        public static int DestroyComponents(this GameObject gameObject)
        {
            var components = gameObject.GetComponents<Component>();
            return components.Where(c => c.GetType().UnderlyingSystemType != TransformType).Destroy();
        }
        public static int DestroyImmediateComponents(this GameObject gameObject, bool allowDestroyingAssets = false)
        {
            var components = gameObject.GetComponents<Component>();
            return components.Where(c => c.GetType().UnderlyingSystemType != TransformType).DestroyImmediate(allowDestroyingAssets);
        }
        
        public static int DestroyComponentsWhere(this GameObject gameObject, IList<Type> toDestroy)
        {
            var components = gameObject.GetComponents<Component>();
            return components.Where(c => Filter(c, toDestroy)).Destroy();
        }
        public static int DestroyImmediateComponentsWhere(this GameObject gameObject, IList<Type> toDestroy, bool allowDestroyingAssets = false)
        {
            var components = gameObject.GetComponents<Component>();
            return components.Where(c => Filter(c, toDestroy)).DestroyImmediate(allowDestroyingAssets);
        }
        public static int DestroyComponentsWhereNot(this GameObject gameObject, IList<Type> toStay)
        {
            var components = gameObject.GetComponents<Component>();
            return components.Where(c => FilterNot(c, toStay)).Destroy();
        }
        public static int DestroyImmediateComponentsWhereNot(this GameObject gameObject, IList<Type> toStay, bool allowDestroyingAssets = false)
        {
            var components = gameObject.GetComponents<Component>();
            return components.Where(c => FilterNot(c, toStay)).DestroyImmediate(allowDestroyingAssets);
        }

        public static IEnumerable<TComponent> GetComponents<TComponent>(this IEnumerable<GameObject> objects)
            where TComponent : Component => objects.SelectMany(obj => obj.GetComponents<TComponent>());
    }
}