using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Определяет для плеера, где надо закончить сценарий.
    /// Останавливается, если все EndNode в текущем сценарии были проиграны.
    /// Связан со StartNode. Обязателен для запуска саб-сценариев
    /// </summary>
    public interface IEndNode : IScenarioNodeFlow, 
        IModelReflection<EndNodeV6, IEndNode>, IScenarioCompatibilityEndNode
    {
        public bool InstantEnd { get; set; }
    }
}