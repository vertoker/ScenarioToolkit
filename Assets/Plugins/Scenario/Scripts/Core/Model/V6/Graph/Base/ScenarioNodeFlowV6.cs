using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player;

// Previous: 
//  Current: ScenarioFlowNodeV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    [JsonObject(IsReference = true)]
    public abstract class ScenarioNodeFlowV6 : ScenarioNodeV6, IScenarioNodeFlow
    {
        public ActivationType ActivationType { get; set; } = ActivationType.AND;
        public HashSet<int> IncomingLinks { get; set; } = new();
        public HashSet<int> OutcomingLinks { get; set; } = new();
        
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
        
        public void ClearAll()
        {
            IncomingLinks.Clear();
            OutcomingLinks.Clear();
        }
    }
}