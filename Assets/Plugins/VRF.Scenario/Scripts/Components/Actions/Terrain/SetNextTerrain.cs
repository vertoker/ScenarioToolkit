using Scenario.Core.Model.Interfaces;
using VRF.VRBehaviours.TerrainSystem;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    public struct SetNextTerrain : IScenarioAction
    {
        public TerrainScene TerrainScene;
        public bool ResetNextTerrainAfterUse;
    }
}