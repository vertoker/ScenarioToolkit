using System.Threading.Tasks;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player;

// Previous: EndNodeV1
//  Current: EndNodeV5
//     Next: EndNodeV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class EndNodeV5 : ScenarioNodeV1, IScenarioCompatibilityEndNode
    {
        public bool InstantEnd { get; set; } = false;
    }
}