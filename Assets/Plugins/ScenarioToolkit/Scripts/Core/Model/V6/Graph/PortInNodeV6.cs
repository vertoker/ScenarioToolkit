using Scenario.Core.Model.Interfaces;

// Previous: PortInNodeV4
//  Current: PortInNodeV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PortInNodeV6 : ScenarioNodeFlowV6, IPortInNode
    {
        public IPortOutNode OutputNode { get; set; }
    }
}