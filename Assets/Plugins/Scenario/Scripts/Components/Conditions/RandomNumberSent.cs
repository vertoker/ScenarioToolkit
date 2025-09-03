using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Modules.Scenario.Components.Actions
{
    public struct RandomNumberSent : IScenarioCondition
    {
        public int Number;
    }
}