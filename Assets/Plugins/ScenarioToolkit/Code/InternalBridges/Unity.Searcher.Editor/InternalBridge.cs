using UnityEditor.Searcher;

namespace Scenario.InternalBridges.Unity.Searcher.Editor
{
    public static class InternalBridge
    {
        public static void OverwriteDatabase(SearcherItem item, SearcherDatabaseBase database)
        {
            item.OverwriteDatabase(database);
        }
    }
}