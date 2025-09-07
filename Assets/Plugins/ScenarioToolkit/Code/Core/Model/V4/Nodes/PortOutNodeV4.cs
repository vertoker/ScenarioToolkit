using System.Threading.Tasks;
using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: PortOutNodeV4
//     Next: PortOutNodeV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PortOutNodeV4 : ScenarioNodeV1, IScenarioCompatibilityPortOutNode
    {
        public string Text { get; set; } = string.Empty; // не знаю зачем есть, но должен существовать
        public PortInNodeV4 InputNode { get; set; }
    }
}