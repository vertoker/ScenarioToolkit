#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace VRF.Tools.Utility
{
    public abstract class ToggleGpuInstancing
    {
        //[MenuItem(ToolsNamespace.UtilityGPUInstancingEnable)]
        public static void EnableGpuInstancing()
        {
            SetGpuInstancing(true);
        }
 
        //[MenuItem(ToolsNamespace.UtilityGPUInstancingDisable)]
        public static void DisableGpuInstancing()
        {
            SetGpuInstancing(false);
        }
 
        public static void SetGpuInstancing(bool value)
        {
            foreach (var guid in AssetDatabase.FindAssets("t:Material", new[] { "Assets" }))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (material != null)
                {
                    material.enableInstancing = value;
                    EditorUtility.SetDirty(material);
                }
            }
        }
    }
}
#endif