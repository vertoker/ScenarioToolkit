using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Устанавливает лимиты для Unity HingeJoint")]
    public struct SetHingeLimits : IScenarioAction
    {
        public HingeJoint HingeJoint;
        [ScenarioMeta("В градусах")]
        public float MinAngle;
        [ScenarioMeta("В градусах")]
        public float MaxAngle;
    }
}