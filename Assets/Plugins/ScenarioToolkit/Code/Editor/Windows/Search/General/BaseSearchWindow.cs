using System.Collections.Generic;
using ScenarioToolkit.Editor.Windows.GraphEditor;
using UnityEditor.Searcher;

namespace ScenarioToolkit.Editor.Windows.Search.General
{
    public abstract class BaseSearchWindow
    {
        public GraphEditorWindow GraphEditor { get; }

        public readonly List<SearcherItem> Root;
        public readonly SearchDatabase Database;
        public readonly Searcher Searcher;
        
        public readonly SearcherWindow.Alignment Alignment = 
            new(SearcherWindow.Alignment.Vertical.Top, SearcherWindow.Alignment.Horizontal.Left);
        
        public BaseSearchWindow(GraphEditorWindow graphEditor, string adapterTitle, int reservedItems)
        {
            GraphEditor = graphEditor;
            Root = new List<SearcherItem>(reservedItems);
            
            Database = new SearchDatabase();
            var adapter = new SearchAdapter(adapterTitle);
            Searcher = new Searcher(Database, adapter);
        }

        public abstract void Open();
        public abstract bool ItemSelectedDelegate(SearcherItem item);
    }
}