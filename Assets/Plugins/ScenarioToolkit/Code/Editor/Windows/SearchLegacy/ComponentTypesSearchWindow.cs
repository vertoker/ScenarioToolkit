using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Utilities;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Scenario.Editor.Windows.SearchLegacy
{
    /// <summary>
    /// Editor окно для поиска компонентов, фильтр по типу
    /// </summary>
    public class ComponentTypesSearchWindow : SearchWindow, ISearchWindowProvider
    {
        private event Action<Type> TypeSelected;

        private static ComponentTypesSearchWindow Instance;
        private IEnumerable<Type> items;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            // TODO Чисто теоретически наверное можно добавить поддержку русской раскладки
            var entries = new List<SearchTreeEntry> 
                { new SearchTreeGroupEntry(new GUIContent("Select Component Type")) };
            entries.AddRange(items.Select(type => 
                new SearchTreeEntry(CreateTypeNameContent(type)) { userData = type, level = 1 }));
            return entries;
        }

        private static GUIContent CreateTypeNameContent(Type type)
        {
            var content = new GUIContent
            {
                text = type.Name,
                // TODO эта херня не работает из-за Experimental.GraphView API Unity
                //tooltip = "123"
            };
            return content;
        }
        
        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            TypeSelected?.Invoke(searchTreeEntry.userData as Type);
            return true;
        }

        private void OpenInternal<T>(Action<Type> onCreate, Func<Type, bool> onFilterItems = null)
        {
            TypeSelected += onCreate;
            var searchWindow = CreateInstance<ComponentTypesSearchWindow>();
            var implementations = Reflection.GetImplementations<T>();
            if (onFilterItems != null)
                implementations = implementations.Where(onFilterItems);
            searchWindow.items = implementations;
            Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), searchWindow);
            searchWindow.TypeSelected += t => onCreate?.Invoke(t);
        }

        public static void Open<T>(Action<Type> onCreate, Func<Type, bool> onFilterItems = null)
        {
            Instance ??= CreateInstance<ComponentTypesSearchWindow>();
            Instance.OpenInternal<T>(onCreate, onFilterItems);
        }
    }
}