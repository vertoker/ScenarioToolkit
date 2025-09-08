using Cysharp.Threading.Tasks;
using ScenarioToolkit.Bus;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared.VRF.Utilities;
using UnityEngine;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Components.Conditions;
using Zenject;

namespace ScenarioToolkit.External.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class MovingSystem : BaseScenarioSystem
    {
        public MovingSystem(ScenarioComponentBus bus) : base(bus)
        {
            bus.Subscribe<Move>(Move);
            bus.Subscribe<AnimateTransform>(AnimateTransform);
        }
        
        private void AnimateTransform(AnimateTransform component)
        {
            AnimateTransform(
                component.MovingObject,
                new TransformInfo(component.MovingObject),
                new TransformInfo(component.Target),
                component.Time,
                component.AnimatePosition,
                component.AnimateRotation,
                component.AnimateScale,
                component.Ease);
        }

        private async void AnimateTransform(
            Transform movingObject,
            TransformInfo startTransformInfo,
            TransformInfo endTransformInfo,
            float time,
            bool animatePosition,
            bool animateRotation,
            bool animateScale,
            Easings.Type ease,
            float startTime = 0)
        {
            var remainingTime = time - startTime;

            var startPosition = startTransformInfo.Position;
            var startRotation = Quaternion.Euler(startTransformInfo.EulerAngles);
            var startScale = startTransformInfo.Scale;

            var targetPosition = endTransformInfo.Position;
            var targetRotation = Quaternion.Euler(endTransformInfo.EulerAngles);
            var targetScale = endTransformInfo.Scale;

            while (remainingTime > 0)
            {
                var t = (time - remainingTime) / time;
                t = Easings.GetEasing(t, ease);
                UpdateObject(t);

                await UniTask.Yield();
                remainingTime -= Time.deltaTime;
            }

            UpdateObject(1);
            
            Bus.Fire(new MoveEnded { MovingObject = movingObject });

            return;

            void UpdateObject(float t)
            {
                if (animatePosition)
                    movingObject.position = Vector3.Lerp(startPosition, targetPosition, t);
                if (animateRotation)
                    movingObject.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
                if (animateScale)
                    movingObject.localScale = Vector3.Lerp(startScale, targetScale, t);
            }
        }

        private void Move(Move component)
        {
            AnimateTransform(new AnimateTransform()
            {
                MovingObject = component.MovingObject,
                Target = component.Target,
                Time = component.Time,
                AnimatePosition = true,
                AnimateRotation = false,
                AnimateScale = false,
                Ease = component.Ease,
            });
        }
        
        public class TransformInfo
        {
            public Vector3 Position;
            public Vector3 EulerAngles;
            public Vector3 Scale;
            
            public TransformInfo(){}

            public TransformInfo(Transform transform)
            {
                Position = transform.position;
                EulerAngles = transform.eulerAngles;
                Scale = transform.localScale;
            }
        }
    }
}