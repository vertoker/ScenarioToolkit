using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;
using UnityEngine;

// Previous: ScenarioNode
//  Current: ScenarioNodeV1
//     Next: ScenarioNodeV6

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    [JsonObject(IsReference = true)]
    public abstract class ScenarioNodeV1 : IScenarioCompatibilityNode
    {
        public string Name { get; set; } = string.Empty;
        public int Hash { get; set; }
        
        public void InitializeHash() => Hash = base.GetHashCode();
        public void RandomizeHash() => Hash = Random.Range(int.MinValue, int.MaxValue);
        public override int GetHashCode() => Hash;
        
        [JsonIgnore] private bool allowNextProcess = true; // поведение по умолчанию

        public virtual void Activate(NodeExecutionContext context)
        {
            
        }
        public virtual void Deactivate(NodeExecutionContext context)
        {
            allowNextProcess = true;
        }
        public virtual Task WaitForCompletion() => Task.CompletedTask;

        public void SetAllowNextProcess(bool newAllow) => allowNextProcess = newAllow;
        public bool IsAllowNextProcess() => allowNextProcess;

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