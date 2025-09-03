using System.Collections.Generic;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Общая информация по всему сценарию. Используется как в редакторе, так и в плеере
    /// </summary>
    public interface IScenarioContext : IVariableEnvironment, INodeOverrides,
        IModelReflection<ScenarioContextV6, IScenarioContext>
    {
        
    }
}