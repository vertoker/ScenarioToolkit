using System;
using Cysharp.Threading.Tasks;
using Scenario.Base.Components.Actions;
using Scenario.Base.Components.Conditions;
using ScenarioToolkit.Bus;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Library.Components.Actions.Unity;
using ScenarioToolkit.Shared;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    /// <summary>
    /// State-timer система для запуска анимация в Animator
    /// </summary>
    public class AnimatorPlaySystem : BaseScenarioSystem
    {
        public AnimatorPlaySystem(ScenarioComponentBus listener) : base(listener)
        {
            listener.Subscribe<PlayAnimation>(PlayAnimation);
            listener.Subscribe<StopAnimation>(StopAnimation);
            listener.Subscribe<SetAnimatorParameter>(SetAnimatorParameter);
            listener.Subscribe<SetAnimatorLayerWeight>(SetAnimatorLayerWeight);
        }

        private void PlayAnimation(PlayAnimation component)
        {
            if (AssertLog.NotNull<PlayAnimation>(component.Animator, nameof(component.Animator))) return;
            if (AssertLog.NotEmpty<PlayAnimation>(component.AnimationStateName, nameof(component.AnimationStateName))) return;
            
            if (component.Force)
                Bus.Fire(component.ConvertStop());

            component.Animator.enabled = true;
            PlayAnimationImpl(component);
            
            Bus.Fire(new AnimationStarted
            {
                Animator = component.Animator,
                AnimationStateName = component.AnimationStateName,
            });
        }
        private void PlayAnimationImpl(PlayAnimation component, float normalizedTime = 0)
        {
            component.Animator.Play(component.AnimationStateName, component.AnimationLayer, component.AnimationLayer);
            if (!component.Loop) WaitAnimation(component, normalizedTime);
        }

        private async void WaitAnimation(PlayAnimation component, float normalizedTime = 0)
        {
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate); // TODO не обновляется в том же кадре
            
            var state = component.Animator.GetCurrentAnimatorStateInfo(component.AnimationLayer);

            var waitTime = TimeSpan.FromSeconds(state.length * (1 - normalizedTime) - Time.deltaTime);
            await UniTask.Delay(waitTime);

            Bus.Fire(new StopAnimation
            {
                Animator = component.Animator,
                AnimationStateName = component.AnimationStateName,
            });
        }

        private void StopAnimation(StopAnimation component)
        {
            if (AssertLog.NotNull<StopAnimation>(component.Animator, nameof(component.Animator))) return;
            if (AssertLog.NotEmpty<StopAnimation>(component.AnimationStateName, nameof(component.AnimationStateName))) return;
            
            Bus.Fire(new AnimationEnded
            {
                Animator = component.Animator,
                AnimationStateName = component.AnimationStateName,
            });
        }

        // TODO добавить в states для всего что ниже

        private void SetAnimatorParameter(SetAnimatorParameter component)
        {
            if (AssertLog.NotNull<SetAnimatorParameter>(component.Animator, nameof(component.Animator))) return;
            if (AssertLog.NotEmpty<SetAnimatorParameter>(component.ParameterName, nameof(component.ParameterName))) return;

            var animator = component.Animator;
            var parameterName = component.ParameterName;
            var value = component.Value;
            
            switch (component.ParameterType)
            {
                case AnimatorControllerParameterType.Float:
                    if (AssertLog.IsTrue(float.TryParse(value, out var resFloat), "Value is not a float")) break;
                    
                    animator.SetFloat(parameterName, resFloat);
                    break;
                
                case AnimatorControllerParameterType.Int:
                    if (AssertLog.IsTrue(int.TryParse(value, out var resInt), "Value is not an integer")) break;
                    
                    animator.SetInteger(parameterName, resInt);
                    break;
                
                case AnimatorControllerParameterType.Bool:
                    if (AssertLog.IsTrue(bool.TryParse(value, out var resBool), "Value is not a bool")) break;
                    
                    animator.SetBool(parameterName, resBool);
                    break;
                
                case AnimatorControllerParameterType.Trigger:
                    animator.SetTrigger(parameterName);
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void SetAnimatorLayerWeight(SetAnimatorLayerWeight component)
        {
            if (AssertLog.NotNull<SetAnimatorLayerWeight>(component.Animator, nameof(component.Animator))) return;
            component.Animator.SetLayerWeight(component.LayerIndex, component.Weight);
        }
    }
}