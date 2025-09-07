using Scenario.Base.Components.Actions;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Conditions
{
    [ScenarioMeta("Начало проигрывания анимации", typeof(PlayAnimation), typeof(AnimationEnded))]
    public struct AnimationStarted : IScenarioCondition
    {
        public Animator Animator;
        public string AnimationStateName;
    }
}