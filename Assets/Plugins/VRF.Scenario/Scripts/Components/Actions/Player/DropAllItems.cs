using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using VRF.BNG_Framework.Scripts.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Освободить все руки (VR и WASD) от предметов", typeof(Grabber))]
    public struct DropAllItems : IScenarioAction
    {
        
    }
}