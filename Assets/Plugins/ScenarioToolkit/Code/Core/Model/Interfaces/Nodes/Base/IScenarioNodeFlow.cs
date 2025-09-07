using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ScenarioToolkit.Core.Player;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    [JsonObject(IsReference = true)]
    public interface IScenarioNodeFlow : IScenarioNode
    {
        public ActivationType ActivationType { get; set; }
        public HashSet<int> IncomingLinks { get; set; }
        public HashSet<int> OutcomingLinks { get; set; }
        
        public void ClearAll();
        
        public void Activate(NodeExecutionContext context);
        public void Deactivate(NodeExecutionContext context);
        public Task WaitForCompletion();
        
        public void SetAllowNextProcess(bool newAllow);
        public bool IsAllowNextProcess();
    }
}