using ScenarioToolkit.Shared.VRF.Utilities.Extensions;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

#else
using VRF.Utilities.Exceptions;
#endif

namespace ScenarioToolkit.Shared.VRF.Utilities
{
    /// <summary>
    /// Различные инструменты для создания, уничтожения объектов и валидации компонентов
    /// </summary>
    public static class VrfTools
    {
        public static bool EditorInCreationMode()
        {
            var e = Event.current;
            return e.type is EventType.ValidateCommand && e.commandName is "Duplicate" or "Paste";
        }

        public static void DestroyAllChildren(this Transform parent)
        {
            var lastCount = parent.childCount;
            for (var i = 0; i < lastCount; i++)
            {
                var child = parent.GetChild(parent.childCount - 1);
                Object.Destroy(child.gameObject);
            }
            VrfRuntimeEditor.SetDirty(parent);
        }
        
        public static void DestroyImmediateAllChildren(this Transform parent)
        {
            var lastCount = parent.childCount;
            for (var i = 0; i < lastCount; i++)
            {
                var child = parent.GetChild(parent.childCount - 1);
                Object.DestroyImmediate(child.gameObject);
            }
            VrfRuntimeEditor.SetDirty(parent);
        }
        
        public static void DestroyImmediateAllChildrenWithColliders(this Transform parent)
        {
            var lastCount = parent.childCount;
            for (var i = 0; i < lastCount; i++)
            {
                var child = parent.GetChild(parent.childCount - 1);
                foreach (var collider in child.GetComponents<Collider>())
                    collider.enabled = false;
                Object.DestroyImmediate(child.gameObject);
            }
            VrfRuntimeEditor.SetDirty(parent);
        }

        public static GameObject Instantiate(GameObject preset)
        {
#if UNITY_EDITOR
            return (GameObject)PrefabUtility.InstantiatePrefab(preset);
#else
            return Object.Instantiate(preset);
#endif
        }
        
        /// <summary>
        /// Убедиться, что на сцене существует только один компонент заданного типа.
        /// Если компонентов больше одного, удалить все, кроме одного
        /// </summary>
        public static TComponent ValidateSingleComponent<TComponent>() where TComponent : Component
        {
            var replacers = VrfSelection.SceneTransforms.FindAll<TComponent>();
            TComponent component;
            
            switch (replacers.Count)
            {
                case > 1:
                {
                    for (var i = 1; i < replacers.Count; i++)
                        Object.DestroyImmediate(replacers[i]);
                    component = replacers[0];
                    break;
                }
                case < 1:
                {
                    var replacerObj = new GameObject();
                    component = replacerObj.AddComponent<TComponent>();
                    VrfRuntimeEditor.SetDirty(component);
                    break;
                }
                default:
                {
                    component = replacers[0];
                    break;
                }
            }

            return component;
        }
    }
}