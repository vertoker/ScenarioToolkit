#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRF.Utilities;
using VRF.Utilities.Attributes;
using VRF.Utilities.Extensions;

namespace VRF.Tools.Utility
{
    public class SelectMissingScripts
    {
        [ToolMethod(about: "Выделить все объекты с missing скриптами для выделения",
            description: DeleteMissingScripts.AboutMissingScripts, 
            methodType: MethodType.Readonly)]
        [MenuItem(ToolsNamespace.UtilitySelectMissingComponentsForSelection)]
        private static void UtilitySelectMissingComponentsForSelection()
        {
            SelectMissingComponentsInRoot(VrfSelection.SelectionTransforms);
        }
        
        [ToolMethod(about: "Выделить все объекты с missing скриптами для сцены",
            description: DeleteMissingScripts.AboutMissingScripts, 
            methodType: MethodType.Readonly)]
        [MenuItem(ToolsNamespace.UtilitySelectMissingComponentsForScene)]
        private static void UtilitySelectMissingComponentsForScene()
        {
            SelectMissingComponentsInRoot(VrfSelection.SceneTransforms);
        }
        
        public static void SelectMissingComponentsInRoot(IEnumerable<Transform> root)
        {
            var selection = new List<GameObject>();
            int compCount = 0, objCount = 0;

            bool SelectMissingComponents(Transform tr)
            {
                var obj = tr.gameObject;
                var count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj);
                
                if (count > 0)
                {
                    selection.Add(obj);
                    compCount += count;
                    objCount++;
                }

                return true;
            }
            
            root.CheckoutAll(SelectMissingComponents);

            Selection.objects = selection.ToArray();
            
            Debug.Log($"Found and selected {compCount} missing scripts from {objCount} GameObjects");
        }
    }
}
#endif