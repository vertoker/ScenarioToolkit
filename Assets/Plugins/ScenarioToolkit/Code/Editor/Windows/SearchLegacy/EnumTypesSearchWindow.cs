using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ScenarioToolkit.Shared;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ScenarioToolkit.Editor.Windows.SearchLegacy
{
    /// <summary>
    /// Editor окно для поиска enum типа, нужен для undefined enum field
    /// </summary>
    public class EnumTypesSearchWindow : SearchWindow, ISearchWindowProvider
    {
        public event Action<Type> TypeSelected;
        
        private Type[] componentEnumTypes;
        
        public void Initialize()
        {
            componentEnumTypes = ComponentsReflection.AllComponentTypes
                .SelectMany(Reflection.GetComponentFields)
                .Select(f => f.FieldType)
                .Where(t => t.IsEnum)
                .Distinct()
                .ToArray();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = new List<SearchTreeEntry> 
                { new SearchTreeGroupEntry(new GUIContent("Select Enum Type")) };
            entries.AddRange(componentEnumTypes.Select(type => 
                new SearchTreeEntry(CreateTypeNameContent(type)) 
                    { userData = type, level = 1 }));
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            TypeSelected?.Invoke(searchTreeEntry.userData as Type);
            return true;
        }

        private static GUIContent CreateTypeNameContent(Type type)
        {
            var content = new GUIContent
            {
                text = $"{type.PrettyName()}",
                // TODO эта херня не работает из-за экспериментального API Unity
                tooltip = "123"
            };
            return content;
        }
    }
}