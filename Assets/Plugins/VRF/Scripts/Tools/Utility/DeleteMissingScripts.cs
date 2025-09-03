#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRF.Utilities;
using VRF.Utilities.Attributes;
using VRF.Utilities.Extensions;

namespace VRF.Tools.Utility
{
    public class DeleteMissingScripts : Editor
    {
        public const string AboutMissingScripts = "Missing скрипты это реализация MonoBehaviour скрипта в объекте, " +
                                                  "скрипт которого был удалён/изменён или просто утерян";

        [ToolMethod(about: "Удалить missing скрипты со всех объектов для выделения",
            description: AboutMissingScripts, methodType: MethodType.Regular)]
        [MenuItem(ToolsNamespace.UtilityDeleteMissingComponentsForSelection)]
        private static void UtilityDeleteMissingComponentsForSelection()
        {
            DeleteMissingComponentsInRoot(VrfSelection.SelectionTransforms);
        }

        [ToolMethod(about: "Удалить missing скрипты со всех объектов для сцены",
            description: AboutMissingScripts, methodType: MethodType.Danger)]
        [MenuItem(ToolsNamespace.UtilityDeleteMissingComponentsForScene)]
        private static void UtilityDeleteMissingComponentsForScene()
        {
            DeleteMissingComponentsInRoot(VrfSelection.SceneTransforms);
        }

        private static void DeleteMissingComponentsInRoot(IEnumerable<Transform> root)
        {
            int compCount = 0, objCount = 0;

            bool DestroyMissingComponents(Transform tr)
            {
                var obj = tr.gameObject;
                var count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(obj);

                if (count > 0)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
                    EditorUtility.SetDirty(obj);
                    compCount += count;
                    objCount++;
                }

                return true;
            }

            root.CheckoutAll(DestroyMissingComponents);

            Debug.Log($"Found and removed {compCount} missing scripts from {objCount} GameObjects");
        }
    }
}
#endif