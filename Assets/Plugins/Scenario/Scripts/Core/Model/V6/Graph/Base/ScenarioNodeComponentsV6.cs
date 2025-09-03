using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using ZLinq;
using ZLinq.Linq;

// Previous: ComponentsNodeV1
//  Current: ScenarioNodeComponentsV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    [JsonObject(IsReference = true)]
    public abstract class ScenarioNodeComponentsV6<TComponent> : ScenarioNodeFlowV6, IScenarioNodeComponents<TComponent> where TComponent : IScenarioComponent
    {
        public List<TComponent> Components { get; set; } = new();
        
        public ValueEnumerable<FromList<TComponent>, TComponent> ComponentsAVE => Components.AsValueEnumerable();
        public ValueEnumerable<FromEnumerable<TComponent>, TComponent> ComponentsEnumerableAVE 
            => (Components as IEnumerable<TComponent>).AsValueEnumerable();
        public IEnumerator<IScenarioComponent> GetEnumerator() => Components.Cast<IScenarioComponent>().GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => Components.GetEnumerator();
        public int Count => Components.Count;
    }
}