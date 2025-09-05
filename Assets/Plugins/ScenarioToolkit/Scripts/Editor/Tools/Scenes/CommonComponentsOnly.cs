using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Scenario.Editor.Tools.Scenes
{
    public class CommonComponentsOnly
    {
        [MenuItem("Tools/Scenario Scene/Leave Only Common Components (Selection)", false, 13)]
        private static void RemoveNonCommon()
        {
            var common = FindCommonElements(Selection.gameObjects
                .Select(s => s.GetComponents<Component>().Select(c => c.GetType()).ToList()));
            foreach (var go in Selection.gameObjects)
            {
                var diff = go.GetComponents<Component>().Select(c => c.GetType()).Except(common);
                foreach (var type in diff)
                {
                    var component = go.GetComponent(type);
                    Object.DestroyImmediate(component);
                }
            }
        }

        private static List<T> FindCommonElements<T>(IEnumerable<List<T>> lists) =>
            lists.Aggregate((common, current) => common.Intersect(current).ToList());
    }
}