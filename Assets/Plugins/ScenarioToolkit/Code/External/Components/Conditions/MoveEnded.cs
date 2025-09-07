using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;
using VRF.Scenario.Components.Actions;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("Окончено движение от Move", typeof(Move))]
    public struct MoveEnded : IScenarioCondition
    {
        public Transform MovingObject;
    }
}