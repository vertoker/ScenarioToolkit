using Scenario.Base.Components.Actions;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Conditions
{
    [ScenarioMeta("Окончание проигрывания анимации", typeof(StopAnimation), typeof(AnimationStarted), typeof(PlayAnimation))]
    public struct AnimationEnded : IScenarioCondition
    {
        public Animator Animator;
        public string AnimationStateName;
    }
}