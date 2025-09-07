using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScenarioToolkit.Core.World;
using UnityEditor;
using UnityEngine;

namespace ScenarioToolkit.Editor.Tools.Scenes
{
    public class ScenarioDuplicatesTools
    {
        [MenuItem("Tools/Scenario Scene/Find Scenario Id Duplicates (Scene)", false, 12)]
        private static void Find()
        {
            var behaviours = Object.FindObjectsByType<ScenarioBehaviour>(FindObjectsSortMode.None);
            var duplicates = behaviours.GroupBy(x => x.GetID()).Where(group => group.Count() > 1);
            
            foreach (var dup in duplicates)
            {
                var sb = new StringBuilder();
                sb.Append("Id: ").AppendLine(dup.Key);
                foreach (var dupBeh in dup)
                    sb.AppendLine(string.Join('/', GetGameObjectPath(dupBeh.transform).Reverse()));
                Debug.Log(sb);
            }
        }
        
        private static IEnumerable<string> GetGameObjectPath(Transform obj)
        {
            yield return obj.gameObject.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent;
                yield return obj.gameObject.name;
            }
        }
    }
}