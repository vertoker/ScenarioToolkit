using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VRF.Editor.MethodTools.Models;
using VRF.Editor.MethodTools.Services;
using VRF.Utilities.Attributes;

namespace VRF.Editor.MethodTools
{
    public class MethodToolsWindow : EditorWindow
    {
        private ListView _listView;
        private TextField _search;
        private Button _update;
        
        private readonly List<ListUnitModel> _models = new();
        private readonly StaticMethodContainer _staticMethodContainer;
        private readonly SearchList _searchList;
        
        [MenuItem("Window/" + nameof(MethodToolsWindow))]
        public static void ShowSelf()
        {
            var window = GetWindow<MethodToolsWindow>();
            window.titleContent = new GUIContent(nameof(MethodToolsWindow));
        }
        public MethodToolsWindow()
        {
            _staticMethodContainer = new StaticMethodContainer(1000, OnAdd, OnClear);
            _searchList = new SearchList(_models);
        }

        private void OnEnable()
        {
            rootVisualElement.Add(CreateHeader());
            _listView = CreateListView();
            rootVisualElement.Add(_listView);
            
            _update.clicked += RefreshTypes;
            _staticMethodContainer.Start();
        }
        private void OnDisable()
        {
            _update.clicked -= RefreshTypes;
            _staticMethodContainer.Stop();
            _models.Clear();
        }
        private void Update()
        {
            _staticMethodContainer.Update();
            if (_searchList.UpdateQuery(_search.value))
                _listView.RefreshItems();
        }

        private void RefreshTypes()
        {
            _staticMethodContainer.Stop();
            _staticMethodContainer.Start();
        }
        private void OnAdd(MethodInfo method, ToolMethodAttribute attribute)
        {
            var model = new ListUnitModel(attribute, method);
            //Debug.Log(model.About);
            
            _models.Add(model);
            _searchList.Add(model);
            _listView.RefreshItems();
        }
        private void OnClear()
        {
            _models.Clear();
            _searchList.Clear();
            _listView.RefreshItems();
        }
        
        private VisualElement CreateHeader()
        {
            var container = new VisualElement
            {
                
            };
            _search = new TextField
            {
                name = "Search"
            };
            _update = new Button
            {
                name = "Update",
                text = "Update"
            };
            
            container.Add(_search);
            container.Add(_update);
            return container;
        }
        private ListView CreateListView()
        {
            return new ListView
            {
                reorderable = false,
                style = { flexGrow = 1f },
                showBorder = true,
                itemsSource = _searchList.Search,
                makeItem = StaticMethodVisualElement.Construct,
                bindItem = BindItem,
                fixedItemHeight = 45,
                selectionType = SelectionType.None,
            };
        }
        private void BindItem(VisualElement element, int index) 
            => _models[index].BindItem(element as StaticMethodVisualElement);
    }
}
