using System;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player;
using VRF.Players.Scriptables;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Нода условия, работает по обратному принципу от ActionNode,
    /// а именно при активации подписывается на компоненты в шине и ждёт,
    /// когда придут абсолютно идентичные компоненты.
    /// Также принимает компоненты от ActionNode
    /// </summary>
    public interface IConditionNode : IScenarioNodeComponents<IScenarioCondition>, 
        IModelReflection<ConditionNodeV6, IConditionNode>, IScenarioCompatibilityConditionNode
    {
        public event Action NodeCompleted;
        public event Action<IScenarioCondition, int> ConditionCompleted;
        
        public bool IsCompleted();
        public bool IsEmpty();
        
        public bool TryCompleteCondition(IScenarioCondition condition);
        public void FireComponent(NodeExecutionContext context, int conditionIndex);
        public void ForceEnd();
    }
}