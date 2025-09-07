using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: PortInNodeV4
//     Next: PortInNodeV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class PortInNodeV4 : ScenarioNodeV1, IScenarioCompatibilityPortInNode
    {
        public string Text { get; set; } = string.Empty; // не знаю зачем есть, но должен существовать
        public PortOutNodeV4 OutputNode { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ActivationType ActivationType { get; set; } = ActivationType.AND;
    }
}