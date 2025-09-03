using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для работы с Transform
    /// </summary>
    public static class VrfTransformExtensions
    {
        public static Transform[] GetChildren(this Transform parent)
        {
            var length = parent.childCount;
            var children = new Transform[length];

            for (var i = 0; i < length; i++)
                children[i] = parent.GetChild(i);

            return children;
        }
        public static GameObject[] GetChildrenGameObjects(this Transform parent)
        {
            var length = parent.childCount;
            var children = new GameObject[length];

            for (var i = 0; i < length; i++)
                children[i] = parent.GetChild(i).gameObject;

            return children;
        }
        
        public static Transform FirstOrDefaultChildren(this Transform parent, Func<Transform, bool> predicate)
        {
            var length = parent.childCount;

            for (var i = 0; i < length; i++)
            {
                var child = parent.GetChild(i);
                if (predicate(child)) return child;
            }

            return null;
        }
        
        public static List<GameObject> GetChildrenGameObjectsRecursively(this Transform parent)
        {
            var objects = new List<GameObject>();
            parent.CheckoutAll(Checkout);
            return objects;

            bool Checkout(Transform tr)
            {
                objects.Add(tr.gameObject);
                return true;
            }
        }
        public static List<Transform> GetChildrenRecursively(this Transform parent)
        {
            var transforms = new List<Transform>();
            parent.CheckoutAll(Checkout);
            return transforms;

            bool Checkout(Transform tr)
            {
                transforms.Add(tr);
                return true;
            }
        }
        
        public static Transform[] GetChildren(this IEnumerable<Transform> parents)
        {
            using var enumerator = parents.GetEnumerator();
            var length = 0;
            
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                    length += enumerator.Current.childCount;
            }
            
            var children = new Transform[length];
            enumerator.Reset();

            var counter = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                {
                    var length2 = enumerator.Current.childCount;
                    for (var i = 0; i < length2; i++)
                    {
                        children[counter] = enumerator.Current.GetChild(i);
                        counter++;
                    }
                }
            }
            
            return children;
        }
        public static GameObject[] GetChildrenGameObjects(this IEnumerable<Transform> parents)
        {
            using var enumerator = parents.GetEnumerator();
            var length = 0;
            
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                    length += enumerator.Current.childCount;
            }
            
            var children = new GameObject[length];
            enumerator.Reset();

            var counter = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                {
                    var length2 = enumerator.Current.childCount;
                    for (var i = 0; i < length2; i++)
                    {
                        children[counter] = enumerator.Current.GetChild(i).gameObject;
                        counter++;
                    }
                }
            }
            
            return children;
        }
        
        public static Transform FirstOrDefaultChildren(this IEnumerable<Transform> parents, Func<Transform, bool> predicate)
        {
            using var enumerator = parents.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                var parent = enumerator.Current;
                if (parent == null) continue;
                
                var length = parent.childCount;
                
                for (var i = 0; i < length; i++)
                {
                    var child = parent.GetChild(i);
                    if (predicate(child)) return child;
                }
            }

            return null;
        }
        public static Transform[] FirstOrDefaultChildren(this IEnumerable<Transform> parents)
        {
            using var enumerator = parents.GetEnumerator();
            var length = 0;
            
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                    length += enumerator.Current.childCount;
            }
            
            var children = new Transform[length];
            enumerator.Reset();

            var counter = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                {
                    var length2 = enumerator.Current.childCount;
                    for (var i = 0; i < length2; i++)
                    {
                        children[counter] = enumerator.Current.GetChild(i);
                        counter++;
                    }
                }
            }
            
            return children;
        }

        public static void SwapSublingIndexes(this Transform current, Transform other) =>
            VrfTransform.SwapSublingIndexes(current, other);
        
        public static Vector3 Average(this IEnumerable<Transform> transforms)
        {
            using var enumerator = transforms.GetEnumerator();
            var vector = new Vector3();
            var counter = 0;

            while (enumerator.MoveNext())
            {
                if (enumerator.Current == null) continue;
                vector += enumerator.Current.position;
                counter++;
            }

            return vector / counter;
        }
        public static Vector3 AverageLocal(this IEnumerable<Transform> transforms)
        {
            using var enumerator = transforms.GetEnumerator();
            var vector = new Vector3();
            var counter = 0;

            while (enumerator.MoveNext())
            {
                if (enumerator.Current == null) continue;
                vector += enumerator.Current.localPosition;
                counter++;
            }

            return vector / counter;
        }
        
        public static bool IsEmptyChildren(this Transform self) => self.childCount == 0;

        public static void ResetGlobal(this Transform self)
        {
            self.ResetGlobalNoScale();
            var parent = self.parent;
            self.parent = null;
            self.localScale = Vector3.one;
            self.parent = parent;
        }
        public static void ResetGlobalNoScale(this Transform self)
        {
            self.position = Vector3.zero;
            self.rotation = Quaternion.identity;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(self);
#endif
        }
        public static void ResetLocal(this Transform self)
        {
            self.ResetLocalNoScale();
            self.localScale = Vector3.one;
        }
        public static void ResetLocalNoScale(this Transform self)
        {
            self.localPosition = Vector3.zero;
            self.localRotation = Quaternion.identity;
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(self);
#endif
        }
        public static void ResetToParent(this Transform self, Transform parent)
        {
            self.parent = parent;
            self.ResetLocal();
        }
        
        private static readonly MethodInfo LocalMethodAngles =
            VrfTypes.Transform.GetMethod("GetLocalEulerAngles", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly PropertyInfo LocalPropertyAngles =
            VrfTypes.Transform.GetProperty("rotationOrder", BindingFlags.Instance | BindingFlags.NonPublic);
        
        public static Vector3 GetInspectorLocalEulerAngles(this Transform transform)
        {
            var rotationOrder = LocalPropertyAngles.GetValue(transform, null);
            var localEulerAngles = LocalMethodAngles.Invoke(transform, new[] { rotationOrder });
            return (Vector3)localEulerAngles;
        }
        
        
        public static IEnumerable<TComponent> GetComponents<TComponent>(this IEnumerable<Transform> transforms)
            where TComponent : Component => transforms.SelectMany(tr => tr.GetComponents<TComponent>());
    }
}