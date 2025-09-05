using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.World;
using Scenario.Editor.Model;
using Scenario.Editor.SRF;
using Scenario.Editor.Utilities;
using Scenario.Editor.Windows;
using Scenario.Editor.Windows.GraphEditor.GraphViews;
using Scenario.Utilities.Extensions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using ZLinq;
using Object = UnityEngine.Object;

namespace Scenario.Editor.Content.SRF
{
    public class UpdateNodeFieldsThroughGOCopy : IScenarioReflexFunction
    {
        public SRFMetadata GetMetadata()
        {
            return new SRFMetadata
            {
                FunctionName = "Update Node Fields Through GO Copy",
                FunctionTooltip = "Функция: копирует ноду, копирует родительский объект, обновляет ссылки у " +
                                  "новой ноды на новый объект (если найдёт)",
            };
        }

        public void BuildUI(SRFContext context)
        {
            context.CreateList<NodeRef>("SourceNodes", tooltip:"Исходная нода, которая будет копироваться");
            context.CreateList<GameObject>("ParentGameObjects", tooltip:"Исходный объект, к которому идут ссылки у SourceNode");
        }

        public void Execute(SRFContext context)
        {
            var nodes = context.GetList<NodeRef>("SourceNodes");
            var gameObjects = context.GetList<GameObject>("ParentGameObjects");
            
            if (nodes.Length == 0)
            {
                Debug.LogError("Empty nodes");
                return;
            }
            if (gameObjects.Length == 0)
            {
                Debug.LogError("Empty copy objects");
                return;
            }
            
            var graphController = SWEContext.Graph.GraphController;
            var serialization = SWEContext.Graph.Serialization;
            var sceneProvider = SWEContext.Graph.SceneProvider;

            var editorNodes = new List<GraphElement>();
            foreach (var nodeRef in nodes)
            {
                if (graphController.NodeViews.TryGetValue(nodeRef.Hash, out var nodeView))
                    editorNodes.Add(nodeView);
            }
            if (editorNodes.Count == 0)
            {
                Debug.LogError("Can't found any node views via NodeRef's");
                return;
            }
            
            Dictionary<string, string> idMap = new();
            foreach (var gameObject in gameObjects)
                AppendIDMap(idMap, gameObject);
            
            sceneProvider?.CacheScene();
            AssetDatabase.Refresh();

            var copyModel = graphController.CreateCopyModel(editorNodes);

            var data = serialization.Serialize(copyModel);
            foreach (var idPair in idMap)
            {
                // TODO этот механизм ненадёжен, его очень просто сломать неправильными данными
                data = data.Replace(idPair.Key, idPair.Value);
            }
            copyModel = serialization.Deserialize<CopyModel>(data);
            
            graphController.CopyAdd(copyModel);
        }
        
        private void AppendIDMap(Dictionary<string, string> idMap, GameObject source)
        {
            if (!source) return;
            var duplicate = Object.Instantiate(source, source.transform.position, 
                source.transform.rotation, source.transform.parent);
            
            var oldBehaviours = source.GetComponentsInChildren<ScenarioBehaviour>();
            var newBehaviours = duplicate.GetComponentsInChildren<ScenarioBehaviour>();
            for (var i = 0; i < oldBehaviours.Length; i++)
            {
                var oldId = oldBehaviours[i].GetID();
                var newId = newBehaviours[i];
                newId.RefreshID();
                idMap[oldId] = newId.GetID();
            }
        }
    }
}