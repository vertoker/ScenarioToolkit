using System;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Исполняющая нода, главная единица в выполнении сценария,
    /// по большей части из неё все сценарии и состоят.
    /// Использует компоненты и посылает их в шину событий.
    /// Через неё можно активировать ConditionNode
    /// </summary>
    public interface IActionNode : IScenarioNodeComponents<IScenarioAction>, 
        IModelReflection<ActionNodeV6, IActionNode>, IScenarioCompatibilityActionNode
    {
        public event Action<IScenarioAction, int> ActionBeforeFire;
        public event Action<IScenarioAction, int> ActionAfterFire;
    }
}