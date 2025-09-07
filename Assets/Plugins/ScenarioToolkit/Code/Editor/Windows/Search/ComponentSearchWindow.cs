using ScenarioToolkit.Editor.Windows.GraphEditor;
using ScenarioToolkit.Editor.Windows.Search.General;
using UnityEditor.Searcher;

namespace ScenarioToolkit.Editor.Windows.Search
{
    public class ComponentSearchWindow : BaseSearchWindow
    {
        // TODO Экспериментальная идея: заменить поисковик компонентов на этот (правда я не вижу уникальных фишек дял перехода)
        
        public ComponentSearchWindow(GraphEditorWindow graphEditor) : base(graphEditor, "Component Searcher", 128)
        {
            
        }

        public override void Open()
        {
            throw new System.NotImplementedException();
        }
        public override bool ItemSelectedDelegate(SearcherItem item)
        {
            throw new System.NotImplementedException();
        }
    }
}