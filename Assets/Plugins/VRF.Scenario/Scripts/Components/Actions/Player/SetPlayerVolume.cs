using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine.Rendering;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает graphics volume игроку", typeof(ResetPlayerVolume))]
    public struct SetPlayerVolume : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta(CanBeNull = true)]
        public VolumeProfile Profile;

        public void SetDefault()
        {
            Profile = null;
        }
    }
}