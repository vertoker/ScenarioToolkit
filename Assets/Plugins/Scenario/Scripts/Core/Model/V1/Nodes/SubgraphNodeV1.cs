using System;
using System.Threading.Tasks;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player;
using UnityEngine;

// Previous: SubgraphNode
//  Current: SubgraphNodeV1
//     Next: SubgraphNodeV2

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class SubgraphNodeV1 : ScenarioNodeV1, IScenarioCompatibilitySubgraphNode
    {
        public TextAsset Json { get; set; } = null;
    }
}