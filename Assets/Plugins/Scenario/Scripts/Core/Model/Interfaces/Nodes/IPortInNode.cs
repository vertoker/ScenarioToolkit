using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    public interface IPortInNode : IScenarioNodeFlow,
        IModelReflection<PortInNodeV6, IPortInNode>, IScenarioCompatibilityPortInNode
    {
        public IPortOutNode OutputNode { get; set; }
    }
}