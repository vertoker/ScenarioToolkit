using Scenario.Core.Model.Interfaces;

// Previous: PortOutNodeV4
//  Current: PortOutNodeV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PortOutNodeV6 : ScenarioNodeFlowV6, IPortOutNode
    {
        public IPortInNode InputNode { get; set; }
    }
}