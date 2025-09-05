using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Modules.Scenario.Components.Actions
{
    public struct SendRandomNumber : IScenarioAction
    {
        public int Min;
        public int Max;
    }
}