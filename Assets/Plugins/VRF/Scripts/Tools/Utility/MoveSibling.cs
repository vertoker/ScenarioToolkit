#if UNITY_EDITOR
using UnityEditor;
using VRF.Utilities;
using VRF.Utilities.Attributes;

namespace VRF.Tools.Utility
{
    public static class MoveSibling
    {
        [ToolMethod(about: "Передвинуть объект в иерархии вверх",
            description: "", methodType: MethodType.Regular)]
        [MenuItem(ToolsNamespace.UtilityMoveSiblingToTop)]
        private static void UtilityMoveSiblingToTop()
        {
            foreach (var selection in VrfSelection.SelectionTransforms)
            {
                selection.SetAsFirstSibling();
            }
        }
        
        [ToolMethod(about: "Передвинуть объект в иерархии вниз",
            description: "", methodType: MethodType.Regular)]
        [MenuItem(ToolsNamespace.UtilityMoveSiblingToBottom)]
        private static void UtilityMoveSiblingToBottom()
        {
            foreach (var selection in VrfSelection.SelectionTransforms)
            {
                selection.SetAsLastSibling();
            }
        }
    }
}
#endif