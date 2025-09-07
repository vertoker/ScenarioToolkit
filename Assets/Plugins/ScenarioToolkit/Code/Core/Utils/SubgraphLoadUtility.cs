using System;
using System.IO;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.VRF.Utilities;
using UnityEngine;

namespace ScenarioToolkit.Shared.Extensions
{
    public static class SubgraphLoadUtility
    {
        public static string GetStreamingPathAsAbsolute(this ISubgraphNode subgraphNode) 
            => Path.Combine(Application.streamingAssetsPath, subgraphNode.StreamingPath);
        
        public static bool TryLoadJson(this ISubgraphNode subgraphNode, out string json)
        {
            json = string.Empty;
            switch (subgraphNode.LoadType)
            {
                case SubgraphLoadType.TextAsset:
                    if (!subgraphNode.Json) return false;
                    json = subgraphNode.Json.text;
                    return true;
                
                case SubgraphLoadType.StreamingAsset:
                    var streamingPath = subgraphNode.GetStreamingPathAsAbsolute();
                    if (!File.Exists(streamingPath)) return false;
                    json = File.ReadAllText(streamingPath);
                    return true;
                
                case SubgraphLoadType.AbsoluteAsset:
                    if (!File.Exists(subgraphNode.AbsolutePath)) return false;
                    json = File.ReadAllText(subgraphNode.AbsolutePath);
                    return true;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static string GetFullPath(this ISubgraphNode subgraphNode)
        {
            switch (subgraphNode.LoadType)
            {
                case SubgraphLoadType.TextAsset:
#if UNITY_EDITOR
                    var relativePath = UnityEditor.AssetDatabase.GetAssetPath(subgraphNode.Json);
                    var absolutePath = VrfPath.FromProjectToAbsolute(relativePath);
                    return absolutePath;
#else
                    Debug.LogError("Get absolute path of asset implemented only in UnityEditor, check Resources");
                    return null;
#endif
                case SubgraphLoadType.StreamingAsset:
                    var streamingPath = subgraphNode.GetStreamingPathAsAbsolute();
                    return streamingPath;
                
                case SubgraphLoadType.AbsoluteAsset:
                    return subgraphNode.AbsolutePath;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}