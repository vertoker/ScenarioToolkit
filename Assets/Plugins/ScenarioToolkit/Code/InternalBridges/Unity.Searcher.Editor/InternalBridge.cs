using UnityEditor.Searcher;

namespace ScenarioToolkit.InternalBridges.Unity.Searcher.Editor
{
    public static class InternalBridge
    {
        public static void OverwriteDatabase(SearcherItem item, SearcherDatabaseBase database)
        {
            item.OverwriteDatabase(database);
        }
    }
}