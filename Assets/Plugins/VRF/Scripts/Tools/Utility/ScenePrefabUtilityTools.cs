#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace VRF.Tools.Utility
{
    public static class ScenePrefabUtilityTools
    {
        //[MenuItem("Tools/Prefabs/Make prefab as part of mesh", false)]
        public static void MakePrefabAsPartOfMesh()
        {
            foreach (var tr in Selection.transforms)
                MakeMeshFromGameObject(tr);
        }

        private static void MakeMeshFromGameObject(Transform tr)
        {
            var nextParent = new GameObject(tr.gameObject.name).transform;

            nextParent.parent = tr.parent;
            nextParent.position = tr.position;
            tr.parent = nextParent;
            tr.gameObject.name = "mesh";
        }
    }
}
#endif