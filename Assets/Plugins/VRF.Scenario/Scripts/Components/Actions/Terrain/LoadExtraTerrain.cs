using Scenario.Core.Model.Interfaces;
using VRF.VRBehaviours.TerrainSystem;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    public struct LoadExtraTerrain : IScenarioAction
    {
        public TerrainScene TerrainScene;
    }
}