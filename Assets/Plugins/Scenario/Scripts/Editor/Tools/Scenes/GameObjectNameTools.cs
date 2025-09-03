using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Scenario.Editor.Tools.Scenes
{
    public class GameObjectNameTools
    {
        [MenuItem("Tools/Scenario Scene/Check Unique GameObject Names (Selection)", false, 11)]
        public static void CheckUniqueNames()
        {
            var selectedObjects = Selection.gameObjects;
            var nameSet = new HashSet<string>();
            foreach (var obj in selectedObjects)
            {
                var name = obj.name;
                if (nameSet.Contains(name))
                    Debug.LogWarning($"Name duplicate (method 1) {name}");
                nameSet.Add(name);
            }
        
        
            var duplicates = selectedObjects.GroupBy(x => x.name)
                .Where(group => group.Count() > 1);
            foreach (var dup in duplicates)
                Debug.LogWarning($"Name duplicate (method 2) {dup.Key}");
        }
        
        [MenuItem("Tools/Scenario Scene/Generate Unique GameObject Names (Selection)", false, 11)]
        public static void GenerateUniqueNames()
        {
            var selectedObjects = Selection.gameObjects;
            var names = new Dictionary<string, int>();
            foreach (var obj in selectedObjects)
            {
                var oldName = obj.name.TrimEnd("1234567890()_. ".ToCharArray());
                names.TryAdd(oldName, 0);
                var index = names[oldName];
                var newName = oldName + $"_{index}";
                names[oldName] = index + 1;
                if (newName != oldName)
                    Debug.Log($"Renamed {oldName} to {newName}");
                obj.name = newName;
            }
        }
    }
}