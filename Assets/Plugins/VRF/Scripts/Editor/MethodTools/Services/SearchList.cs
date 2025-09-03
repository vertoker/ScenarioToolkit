using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VRF.Editor.MethodTools.Models;

namespace VRF.Editor.MethodTools.Services
{
    public class SearchList
    {
        private readonly List<ListUnitModel> _source;
        private readonly List<ListUnitModel> _search;
        private string _searchQuery = string.Empty;

        public IList Search => _search;

        public SearchList(List<ListUnitModel> source)
        {
            _source = source;
            _search = new List<ListUnitModel>(source.Capacity);
        }

        public bool UpdateQuery(string newValue)
        {
            newValue = newValue.ToLower();
            if (_searchQuery == newValue) return false;
            _searchQuery = newValue;
            UpdateList();
            return true;
        }
        private void UpdateList()
        {
            _search.Clear();

            _search.AddRange(_searchQuery.Length == 0 ? _source 
                : _source.Where(m => m.IsSearchable(_searchQuery)));
        }

        public void Add(ListUnitModel model)
        {
            if (_searchQuery.Length != 0 && 
                !model.IsSearchable(_searchQuery))
                return;
            _search.Add(model);
        }
        public void Clear()
        {
            _search.Clear();
        }
    }
}