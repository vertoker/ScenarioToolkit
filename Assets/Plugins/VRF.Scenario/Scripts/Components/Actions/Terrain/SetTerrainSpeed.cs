using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    public struct SetTerrainSpeed : IScenarioAction
    {
        public float Speed;
        public float Time;
    }
}