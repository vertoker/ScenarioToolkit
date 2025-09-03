using System;
using System.IO;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Editor.CustomVisualElements;
using Scenario.Editor.Utilities.Providers;
using Scenario.Editor.Windows.GraphEditor;
using Scenario.Editor.Windows.GraphEditor.GraphViews;
using Scenario.Editor.Windows.SearchLegacy;
using Scenario.Utilities.Extensions;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Scenario.Editor.Windows.ElementEditor.NodeElements
{
    public class SubgraphNodeElement : BaseNodeElement
    {
        private readonly ISubgraphNode subgraphNode;
        private readonly VariableEnvironmentEditor environmentEditor;
        
        private readonly EnumField loadTypeField;
        private readonly ObjectField jsonField;
        private readonly TextField streamingPathField;
        private readonly TextField absolutePathField;
        private readonly Button addButton;

        public SubgraphNodeElement(ScenarioNodeView nodeView, GraphEditorWindow graphEditor) : base(nodeView, graphEditor)
        {
            subgraphNode = (ISubgraphNode)nodeView.ScenarioNode;

            var enumField = CreateActivationTypeEnum(subgraphNode);
            Root.Add(enumField);
            
            loadTypeField = new EnumField(nameof(subgraphNode.LoadType), SubgraphLoadType.TextAsset);
            jsonField = new ObjectField(nameof(subgraphNode.Json)) 
                { objectType = typeof(TextAsset), allowSceneObjects = false, };
            streamingPathField = new TextField(nameof(subgraphNode.StreamingPath)) 
                { value = subgraphNode.StreamingPath, };
            absolutePathField = new TextField(nameof(subgraphNode.AbsolutePath)) 
                { value = subgraphNode.AbsolutePath, };

            loadTypeField.RegisterValueChangedCallback(LoadTypeChanged);
            jsonField.RegisterValueChangedCallback(JsonChanged);
            streamingPathField.RegisterValueChangedCallback(StreamingPathChanged);
            absolutePathField.RegisterValueChangedCallback(AbsolutePathChanged);
            
            environmentEditor = new VariableEnvironmentEditor(Root, this.subgraphNode);
            
            var assetButtons = UIProvider.GetUxmlTree("Nodes/SubgraphAsset").Instantiate();
            Root.Add(assetButtons);
            
            addButton = assetButtons.Q<Button>("find-add");
            var openButton = assetButtons.Q<Button>("open");
            
            addButton.clickable.clicked += AddSubgraph;
            openButton.clickable.clicked += OpenSubgraph;
            environmentEditor.ImportVariablesButton.clickable.clicked += ImportSubgraphContextVariables;
            environmentEditor.ImportVariablesButton.text = "Import Subgraph Context Variables";
            environmentEditor.DataUpdated += OnDataUpdated;
            
            environmentEditor.Load();
            
            Root.Add(loadTypeField);
            LoadTypeAdd(subgraphNode.LoadType);
        }

        private void LoadTypeChanged(ChangeEvent<Enum> evt)
        {
            var loadTypeOld = (SubgraphLoadType)evt.previousValue;
            var loadTypeNew = (SubgraphLoadType)evt.newValue;
            
            LoadTypeRemove(loadTypeOld);
            LoadTypeAdd(loadTypeNew);
            
            subgraphNode.LoadType = loadTypeNew;
            OnDataUpdated();
        }
        private void LoadTypeRemove(SubgraphLoadType loadType)
        {
            switch (loadType)
            {
                case SubgraphLoadType.TextAsset:
                    Root.Remove(jsonField);
                    break;
                case SubgraphLoadType.StreamingAsset:
                    Root.Remove(streamingPathField);
                    break;
                case SubgraphLoadType.AbsoluteAsset:
                    Root.Remove(absolutePathField);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        private void LoadTypeAdd(SubgraphLoadType loadType)
        {
            switch (loadType)
            {
                case SubgraphLoadType.TextAsset:
                    Root.Add(jsonField);
                    break;
                case SubgraphLoadType.StreamingAsset:
                    Root.Add(streamingPathField);
                    break;
                case SubgraphLoadType.AbsoluteAsset:
                    Root.Add(absolutePathField);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        private void JsonChanged(ChangeEvent<Object> evt)
        {
            if (!evt.newValue)
            {
                subgraphNode.Json = null;
                OnDataUpdated();
                return;
            }
            if (evt.newValue is not TextAsset textAsset)
            {
                Debug.LogWarning("File is not TextAsset");
                jsonField.SetValueWithoutNotify(evt.previousValue);
                return;
            }
            
            subgraphNode.Json = textAsset;
            OnDataUpdated();
        }
        private void StreamingPathChanged(ChangeEvent<string> evt)
        {
            var streamingPath = Path.Combine(Application.streamingAssetsPath, evt.newValue);
            if (!File.Exists(streamingPath))
            {
                Debug.LogWarning($"File is not exists by streaming path: {evt.newValue}");
                streamingPathField.SetValueWithoutNotify(evt.previousValue);
                return;
            }
            
            subgraphNode.StreamingPath = streamingPath;
            OnDataUpdated();
        }
        private void AbsolutePathChanged(ChangeEvent<string> evt)
        {
            if (!File.Exists(evt.newValue))
            {
                Debug.LogWarning($"File is not exists by absolute path: {evt.newValue}");
                streamingPathField.SetValueWithoutNotify(evt.previousValue);
                return;
            }
            
            subgraphNode.AbsolutePath = evt.newValue;
            OnDataUpdated();
        }

        public override void RedrawElements()
        {
            base.RedrawElements();

            loadTypeField.SetValueWithoutNotify(subgraphNode.LoadType);
            jsonField.SetValueWithoutNotify(subgraphNode.Json);
            streamingPathField.SetValueWithoutNotify(subgraphNode.StreamingPath);
            absolutePathField.SetValueWithoutNotify(subgraphNode.AbsolutePath);
        }

        private void OpenSubgraph()
        {
            if (!subgraphNode.TryLoadJson(out var json))
            {
                Debug.LogError("Can't load Json with loadType=" +
                               Enum.GetName(typeof(SubgraphLoadType), subgraphNode.LoadType));
                return;
            }
            var absolutePath = subgraphNode.GetFullPath();
            
            // Если тут вылезает null reference, то нужно переоткрыть окно сценария
            GraphEditor.Session.AddCurrent(subgraphNode.Hash);
            GraphEditor.LoadSession(json, absolutePath);
        }
        private void ImportSubgraphContextVariables()
        {
            if (!subgraphNode.TryLoadJson(out var json))
            {
                Debug.LogError("Can't load Json with loadType=" +
                               Enum.GetName(typeof(SubgraphLoadType), subgraphNode.LoadType));
                return;
            }
            
            var model = GraphEditor.LoadService.LoadModelFromJson(json);
            foreach (var variable in model.Context.Variables)
            {
                if (environmentEditor.Contains(variable.Key)) 
                    environmentEditor.Remove(variable.Key);
                environmentEditor.AddVariable(variable.Key, variable.Value, true);
            }
            OnDataUpdated();
        }

        private void AddSubgraph()
        {
            var searchWindowProvider = ScriptableObject.CreateInstance<SubgraphAssetsSearchWindow>();
            searchWindowProvider.Initialize(jsonField);
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(addButton.transform.position)),
                searchWindowProvider);
        }
    }
}