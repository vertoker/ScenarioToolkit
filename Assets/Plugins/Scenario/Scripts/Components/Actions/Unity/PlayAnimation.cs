using Scenario.Base.Components.Conditions;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Проигрывает анимацию в Unity Animator", typeof(StopAnimation), typeof(AnimationStarted))]
    public struct PlayAnimation : IScenarioAction, IComponentDefaultValues
    {
        public Animator Animator;
        [ScenarioMeta("Имя анимации для проигрывания")]
        public string AnimationStateName;
        [ScenarioMeta("Слой анимации для проигрывания")]
        public int AnimationLayer;
        [ScenarioMeta("Для анимации не запускается ожидание")]
        public bool Loop;
        [ScenarioMeta("Перед запуском анимации в шину посылается StopAnimation")]
        public bool Force;
        
        public void SetDefault()
        {
            Animator = null;
            AnimationStateName = null;
            AnimationLayer = 0;
            Loop = false;
            Force = false;
        }

        public StopAnimation ConvertStop()
        {
            return new StopAnimation
            {
                Animator = Animator,
                AnimationStateName = AnimationStateName,
                AnimationLayer = AnimationLayer,
            };
        }
    }
}