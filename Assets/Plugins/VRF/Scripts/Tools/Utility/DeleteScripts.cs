#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VRF.Utilities;
using VRF.Utilities.Attributes;
using VRF.Utilities.Extensions;

namespace VRF.Tools.Utility
{
    public class DeleteScripts
    {
        // TODO дополните если найдёте подобные компоненты
        public static readonly Type[] GraphicsTypes = {
            typeof(MeshRenderer), typeof(SkinnedMeshRenderer), typeof(MeshFilter),
            typeof(MeshCollider), typeof(TextMesh), typeof(Image),
            typeof(RawImage), typeof(Text), typeof(TextMesh)
        };
        
        [ToolMethod(about: "Удалить ВСЕ скрипты со всех объектов для выделения",
            description: "Ужасная функция, удаляет ВСЕ компоненты (кроме Transform)", 
            methodType: MethodType.Danger)]
        [MenuItem(ToolsNamespace.UtilityDeleteComponentsAllForSelection)]
        public static void UtilityDeleteComponentsAllForSelection()
        {
            DeleteComponentsInRoot(VrfSelection.SelectionTransforms);
        }
        // Эта функция просто бессмысленная
        //[MenuItem(ToolsNamespace.UtilityDeleteComponentsAllForScene)]
        public static void UtilityDeleteComponentsAllForScene()
        {
            DeleteComponentsInRoot(VrfSelection.SceneTransforms);
        }
        
        [ToolMethod(about: "Удалить ВСЕ скрипты (которые не являются графикой) со всех объектов для выделения",
            description: "Ужасная функция, удаляет ВСЕ компоненты " +
                         "(кроме Transform и графики) (графика - всё то, что рендерит)", 
            methodType: MethodType.Danger)]
        [MenuItem(ToolsNamespace.UtilityDeleteComponentsAllNonGraphicsForSelection)]
        public static void UtilityDeleteComponentsAllNonGraphicsForSelection()
        {
            DeleteNonGraphicsComponentsInRoot(VrfSelection.SelectionTransforms, GraphicsTypes);
        }
        // Эта функция просто бессмысленная
        //[MenuItem(ToolsNamespace.UtilityDeleteComponentsAllNonGraphicsForScene)]
        public static void UtilityDeleteComponentsAllNonGraphicsForScene()
        {
            DeleteNonGraphicsComponentsInRoot(VrfSelection.SceneTransforms, GraphicsTypes);
        }
        
        public static void DeleteComponentsInRoot(IEnumerable<Transform> root)
        {
            int compCount = 0, objCount = 0;

            bool DestroyComponents(Transform tr)
            {
                var destroyed = tr.gameObject.DestroyComponents();
                if (destroyed > 0) objCount++;
                compCount += destroyed;
                return true;
            }
            
            root.CheckoutAll(DestroyComponents);
            
            Debug.Log($"Found and removed {compCount} components from {objCount} GameObjects");
        }
        public static void DeleteNonGraphicsComponentsInRoot(IEnumerable<Transform> root, Type[] filterNot)
        {
            int compCount = 0, objCount = 0;

            bool DestroyComponents(Transform tr)
            {
                var destroyed = tr.gameObject.DestroyImmediateComponentsWhereNot(filterNot);
                if (destroyed > 0) objCount++;
                compCount += destroyed;
                return true;
            }
            
            root.CheckoutAll(DestroyComponents);
            
            Debug.Log($"Found and removed {compCount} components from {objCount} GameObjects");
        }
    }
}
#endif