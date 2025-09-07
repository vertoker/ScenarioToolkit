using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

// Previous: SubgraphNodeV2
//  Current: SubgraphNodeV5
//     Next: SubgraphNodeV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class SubgraphNodeV5 : ScenarioNodeV1, IScenarioCompatibilitySubgraphNode
    {
        public SubgraphLoadType LoadType { get; set; } = SubgraphLoadType.TextAsset;
        public TextAsset Json { get; set; } = null;
        public string StreamingPath { get; set; } = null;
        public string AbsolutePath { get; set; } = null;
        
        public Dictionary<string, ObjectTyped> Variables { get; set; } = new();
    }
}