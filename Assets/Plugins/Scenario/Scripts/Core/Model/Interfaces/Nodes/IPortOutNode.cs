using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    public interface IPortOutNode : IScenarioNodeFlow, 
        IModelReflection<PortOutNodeV6, IPortOutNode>, IScenarioCompatibilityPortOutNode
    {
        public IPortInNode InputNode { get; set; }
    }
}