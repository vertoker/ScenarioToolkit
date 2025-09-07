using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Serialization;
using ScenarioToolkit.Editor.Utilities;
using ScenarioToolkit.Editor.Utilities.Providers;
using ScenarioToolkit.Shared.VRF.Utilities;
using UnityEditor;
using UnityEngine;
using ZLinq;

namespace ScenarioToolkit.Editor.Tools.Files
{
    public static class ScenarioRecursionTools
    {
        private static readonly ScenarioLoadService LoadService = new(new ScenarioSerializationService());
        
        [MenuItem("Tools/Scenario Files/Detect Recursion (File)", false, 4)]
        public static void DetectRecursion()
        {
            const string title = "Detect recursion in file...";
            if (!ProviderUtils.SelectScenario(title, out var filePath)) return;
            DetectRecursion(filePath);
        }

        public static void DetectRecursion(string filePath)
        {
            filePath = VrfPath.FromAbsoluteToProject(filePath);
            var jsonText = File.ReadAllText(filePath);

            var model = LoadService.LoadModelFromJson(jsonText);
            DetectRecursion(model.Graph, LoadService, filePath);
            Debug.Log($"No recursion in file {DirectoryFileHelper.GetFileName(filePath)}");
        }
        public static void DetectRecursion(IScenarioGraph scenarioGraph, 
            ScenarioLoadService loadService, string filePath = null)
        {
            // Функция строит ориентированный граф - дерево
            // И при каждом шаге добавления вершины,
            // идёт проверка графа на циклы
            
            // Начальная вершина дерева
            var entryTree = new TreeGraph(filePath, scenarioGraph);
            
            // Кэш для составления цикла прохода
            // Для определения использует ключ, который точно не должен 
            // совпадать у всех сценариев, а именно путь на json файл
            var recursionCache = new HashSet<string>();
            
            BuildTree(entryTree);
            return;
            
            // Рекурсивная функция для составления дерева
            void BuildTree(TreeGraph localTree)
            {
                foreach (var subgraphNode in localTree.Graph.NodesValuesAVE.OfType<ISubgraphNode>())
                {
                    if (!subgraphNode.Json) continue; // Может быть null
                    var assetPath = AssetDatabase.GetAssetPath(subgraphNode.Json)
                        // Ключ всё равно получается уникальным, пока лежит в папке Resources/
                        .Remove(0, "Assets/Resources/".Length);
                    
                    var localData = loadService.LoadModelFromJson(subgraphNode.Json.text);
                    
                    // Тут в дерево добавляется новая вершина
                    var newLocalTree = localTree.AddTree(assetPath, localData.Graph);
                    // И обязательно надо проверить всё дерево на наличие циклов
                    DetectRecursionImpl(entryTree);
                    // Дерево продолжается строиться, но уже для новой вершины
                    BuildTree(newLocalTree);
                }
            }

            // Функция нахождения циклов в дереве, использует кэш
            void DetectRecursionImpl(TreeGraph localTree)
            {
                // Ключ может быть пустым, тогда эта вершина игнорируется
                if (!string.IsNullOrEmpty(localTree.Key) && !recursionCache.Add(localTree.Key))
                {
                    const string separator = " -> ";
                    var path = string.Join(separator, recursionCache);
                    throw new StackOverflowException($"Detect recursion by path \n{path}{separator}{localTree.Key}");
                }

                foreach (var subTree in localTree.SubGraphs)
                    DetectRecursionImpl(subTree);
                
                if (!string.IsNullOrEmpty(localTree.Key))
                    recursionCache.Remove(localTree.Key);
            }
        }

        public class TreeGraph
        {
            [CanBeNull] public readonly string Key;
            public readonly IScenarioGraph Graph;
            public readonly List<TreeGraph> SubGraphs;

            public TreeGraph(string key, IScenarioGraph graph)
            {
                Key = key;
                Graph = graph;
                SubGraphs = new List<TreeGraph>();
            }

            public TreeGraph AddTree(string key, IScenarioGraph subGraph)
            {
                var newTree = new TreeGraph(key, subGraph);
                SubGraphs.Add(newTree);
                return newTree;
            }
        }
    }
}