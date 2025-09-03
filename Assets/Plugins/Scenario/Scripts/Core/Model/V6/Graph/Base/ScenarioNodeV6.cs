using System.Text;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

// Previous: ScenarioNodeV1
//  Current: ScenarioNodeV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    [JsonObject(IsReference = true)]
    public abstract class ScenarioNodeV6 : IScenarioNode
    {
        public string Name { get; set; } = string.Empty;
        public int Hash { get; set; }
        
        public int GetBaseHashCode() => base.GetHashCode();
        
        public string GetStatusString()
        {
            var builder = new StringBuilder();
            
            //builder.Append($"type:<b>{GetType().Name}</b>");
            builder.Append($"hash:<b>{Hash}</b>");
            if (!string.IsNullOrEmpty(Name))
                builder.Append($" name:<b>{Name}</b>");

            return builder.ToString();
        }
    }
}