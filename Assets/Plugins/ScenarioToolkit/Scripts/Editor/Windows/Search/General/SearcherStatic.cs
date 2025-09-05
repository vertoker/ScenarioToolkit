using System.Collections.Generic;
using UnityEditor.Searcher;

namespace Scenario.Editor.Windows.Search.General
{
    public static class SearcherStatic
    {
        public static void AddChildren(this SearcherItem searcherItem, params SearcherItem[] children)
        {
            foreach (var child in children)
                searcherItem.AddChild(child);
        }
        public static void AddChildren(this SearcherItem searcherItem, IEnumerable<SearcherItem> children)
        {
            foreach (var child in children)
                searcherItem.AddChild(child);
        }
        public static void AddChildren(this SearcherItem searcherItem, IReadOnlyCollection<SearcherItem> children)
        {
            searcherItem.Children.Capacity = searcherItem.Children.Count + children.Count;
            foreach (var child in children)
                searcherItem.AddChild(child);
        }
    }
}