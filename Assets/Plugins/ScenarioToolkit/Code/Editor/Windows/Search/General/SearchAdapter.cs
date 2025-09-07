using UnityEditor.Searcher;

namespace ScenarioToolkit.Editor.Windows.Search.General
{
    public class SearchAdapter : SearcherAdapter
    {
        public override bool HasDetailsPanel => false;
        
        public SearchAdapter(string title) : base(title)
        {
            //m_DefaultItemTemplate = Resources.Load<VisualTreeAsset>("SearcherItem");
        }
    }
}