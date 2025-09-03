using Scenario.Core.Model.Interfaces;
using VRF.VRBehaviours.TerrainSystem;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    public struct DistanceToStopPositionReached : IScenarioCondition
    {
        public TerrainScene TerrainScene;
    }
}