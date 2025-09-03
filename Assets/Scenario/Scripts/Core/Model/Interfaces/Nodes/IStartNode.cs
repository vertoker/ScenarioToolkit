using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Определяет для плеера, где надо начинать проигрывание сценария.
    /// Стартует все ноды в сценарии. Связан с EndNode. Обязателен для запуска
    /// </summary>
    public interface IStartNode : IScenarioNodeFlow, 
        IModelReflection<StartNodeV6, IStartNode>, IScenarioCompatibilityStartNode
    {
        
    }
}