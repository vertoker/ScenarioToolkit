using System;
using UnityEditor.Searcher;

namespace ScenarioToolkit.Editor.Windows.Search.General
{
    public class SearchItem<T> : SearcherItem
    {
        public Type GetGenericType => typeof(T);

        //private string mName;
        //public override string Name => mName;
        
        public T ContextItem { get; private set; }

        //public SearchItem() : base(string.Empty) { }
        public SearchItem(T contextItem, string name, params string[] synonyms) : base(name, string.Empty)
        {
            ContextItem = contextItem;
            Synonyms = synonyms;
            //mName = name;
        }
        public SearchItem<T> Initialize(T newContextItem, string name, string help = "")
        {
            ContextItem = newContextItem;
            //mName = name;
            Help = help;
            return this;
        }
    }
}