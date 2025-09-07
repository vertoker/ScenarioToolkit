using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

// Previous: SubgraphNodeV1
//  Current: SubgraphNodeV2
//     Next: SubgraphNodeV5

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class SubgraphNodeV2 : ScenarioNodeV1, IScenarioCompatibilitySubgraphNode
    {
        public TextAsset Json { get; set; } = null;
        public Dictionary<string, ObjectTyped> Variables { get; set; } = new();
    }
}