using System.Collections.Generic;
using ScenarioToolkit.Shared.VRF.Utilities.Extensions;
using UnityEditor.Searcher;

namespace ScenarioToolkit.Editor.Windows.Search.General
{
    public class SearchDatabase : SearcherDatabase
    {
        public SearchDatabase() : base(null)
        {
            
        }
        public void ClearAddItems(IReadOnlyCollection<SearcherItem> db)
        {
            // Чисто оптимизация листов
            m_ItemList.Clear();
            m_ItemList.EnsureCapacity(db.Count);
            
            var nextId = 0;
            foreach (var item in db)
                AddItemToIndex(item, ref nextId, null);
            BuildIndex();
        }
    }
}