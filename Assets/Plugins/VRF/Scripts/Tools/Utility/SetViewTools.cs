#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VRF.Utilities.Attributes;

namespace VRF.Tools.Utility
{
    public static class SetViewTools
    {
        [ToolMethod(about: "Выровнить камеру на сцене по вращению (починить кривой горизонт)",
            description: "", methodType: MethodType.Regular)]
        [MenuItem(ToolsNamespace.UtilityViewNormalize)]
        private static void UtilityViewNormalize()
        {
            foreach (var view in SceneView.sceneViews)
                ((SceneView)view).rotation = Quaternion.identity;
        }
        
        [ToolMethod(about: "Зарофлить с камерой и развернуть её рандомно",
            description: "", methodType: MethodType.Regular)]
        [MenuItem(ToolsNamespace.UtilityViewRoflalize)]
        private static void UtilityViewRoflalize()
        {
            foreach (var view in SceneView.sceneViews)
            {
                var x = Random.Range(0f, 360f);
                var y = Random.Range(0f, 360f);
                var z = Random.Range(0f, 360f);
                ((SceneView)view).rotation = Quaternion.Euler(x, y, z);
            }
        }
    }
}
#endif