using Newtonsoft.Json;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Связывает ноды между собой в графе, направление потока исполнения - from -> to
    /// </summary>
    [JsonObject(IsReference = true)]
    public interface IScenarioLinkFlow : IHashable, IModelReflection<ScenarioLinkFlowV6, IScenarioLinkFlow>
    {
        public IScenarioNodeFlow From { get; set; }
        public IScenarioNodeFlow To { get; set; }
    }
}