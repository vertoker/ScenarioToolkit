using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scenario.Editor.Windows.SearchLegacy
{
    /// <summary>
    /// Окно поиска, которое ищет все ассеты, но фильтрует по TextAsset и
    /// группирует по папкам (all -> top), костыль для поиска файлов сценария
    /// </summary>
    public class SubgraphAssetsSearchWindow : SearchWindow, ISearchWindowProvider
    {
        private List<TextAsset> textAssets;
        private ObjectField currentObjectField;

        public void Initialize(ObjectField objectField)
        {
            textAssets = AssetDatabase.FindAssets("t:TextAsset", new[] { "Assets/Resources" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => path.EndsWith(".json"))
                .Select(AssetDatabase.LoadAssetAtPath<TextAsset>)
                .ToList();

            currentObjectField = objectField;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry> 
                { new SearchTreeGroupEntry(new GUIContent("Select JSON File")) };

            var groupedTextAssets = textAssets.GroupBy(
                textAsset => Path.GetFileName(Path.GetDirectoryName(AssetDatabase.GetAssetPath(textAsset)))
            );

            foreach (var group in groupedTextAssets)
            {
                entries.Add(new SearchTreeGroupEntry(new GUIContent(group.Key), 1));

                entries.AddRange(group
                    .Select(textAsset => new SearchTreeEntry(new GUIContent(textAsset.name))
                        { level = 2, userData = textAsset }));
            }

            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            var textAsset = entry.userData as TextAsset;
            currentObjectField.value = textAsset;
            currentObjectField.SendEvent(ChangeEvent<Object>.GetPooled(null, textAsset));
            return true;
        }
    }
}