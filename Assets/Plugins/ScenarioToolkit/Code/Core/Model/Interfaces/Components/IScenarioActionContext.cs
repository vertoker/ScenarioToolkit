using Scenario.Core.Player;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    public interface IScenarioActionContext : IScenarioAction, IComponentDefaultValues
    {
        public IScenarioActionContextData GetRequestData();
    }
    
    public interface IScenarioActionContextData : IScenarioAction, INotSerializableComponent, IScenarioIgnore
    {
        public void Construct(NodeExecutionContext context);
    }
}