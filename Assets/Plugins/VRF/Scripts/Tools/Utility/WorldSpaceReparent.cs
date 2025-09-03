#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VRF.Utilities.Attributes;

namespace VRF.Tools.Utility
{
    public class WorldSpaceReparent
    {
        private static Transform parent;

        [ToolMethod(about: "Запомнить выделенный объект как родителя", methodType: MethodType.Regular)]
        [MenuItem("Tools/GameObject/Mark As Parent %&N")]
        static void MarkAsParent()
        {
            parent = Selection.activeGameObject.transform;
        }

        
        [MenuItem("Tools/GameObject/MarkAsParent %&N", true)]
        static bool ValidateMarkAsParent()
        {
            return Selection.activeGameObject;
        }

        [ToolMethod(about: "Переместить выделенный объект в родителя", methodType: MethodType.Regular)]
        [MenuItem("Tools/GameObject/Reparent %&B")]
        static void Reparent()
        {
            foreach (var go in Selection.gameObjects)
                go.transform.SetParent(parent, true);
        }

        [MenuItem("Tools/GameObject/Reparent %&B", true)]
        static bool ValidateReparent()
        {
            return Selection.activeGameObject && parent;
        }
    }
}
#endif