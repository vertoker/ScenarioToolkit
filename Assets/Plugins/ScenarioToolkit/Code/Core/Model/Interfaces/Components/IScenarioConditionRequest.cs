using System.Collections.Generic;
using Scenario.Core.Player;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary> Обобщение для Condition, который посылает данные internal Action после подписки </summary>
    public interface IScenarioConditionRequest : IScenarioCondition, IComponentDefaultValues, IScenarioOnlyHost
    {
        public IScenarioRequestData GetRequestData();
    }

    /// <summary> Internal Action который и является запросом</summary>
    public interface IScenarioRequestData : IScenarioAction, INotSerializableComponent, IScenarioIgnore
    {
        public void Construct(NodeExecutionContext context);
    }
}