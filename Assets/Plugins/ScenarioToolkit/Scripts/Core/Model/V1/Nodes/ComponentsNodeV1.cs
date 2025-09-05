using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model.Interfaces;

// Previous: ComponentsNode
//  Current: ComponentsNodeV1
//     Next: ScenarioNodeComponentsV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public abstract class ComponentsNodeV1<T> : ScenarioNodeV1 where T : IScenarioComponent
    {
        public List<T> Components { get; set; } = new();
    }
}