using Scenario.Base.Components.Conditions;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Останавливает анимацию в Unity Animator", typeof(PlayAnimation), typeof(AnimationEnded))]
    public struct StopAnimation : IScenarioAction, IComponentDefaultValues
    {
        public Animator Animator;
        [ScenarioMeta("Имя анимации для проигрывания")]
        public string AnimationStateName;
        [ScenarioMeta("Слой анимации для проигрывания")]
        public int AnimationLayer;
        
        public void SetDefault()
        {
            Animator = null;
            AnimationStateName = null;
            AnimationLayer = 0;
        }
    }
}