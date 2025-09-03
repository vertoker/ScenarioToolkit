using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Телепортирует локального игрока к точке", typeof(PlayerTeleport))]
    public struct TeleportPlayer : IScenarioAction, IComponentDefaultValues
    {
        public Transform Target;
        
        public void SetDefault()
        {
            Target = null;
        }
    }
}