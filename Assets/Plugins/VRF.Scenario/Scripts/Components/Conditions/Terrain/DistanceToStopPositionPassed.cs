using Scenario.Core.Model.Interfaces;
using VRF.VRBehaviours.TerrainSystem;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    public struct DistanceToStopPositionPassed : IScenarioCondition
    {
        public TerrainScene TerrainScene;
    }
}