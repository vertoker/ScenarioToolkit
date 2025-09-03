using System.Threading;
using Cysharp.Threading.Tasks;
using SimpleUI.Extensions;
using UnityEngine;
using VRF.Scenario.DriveActor.Core;

namespace VRF.Scenario.DriveActor.Input
{
    public class ButtonConstantInputActor : BaseTransformStatesInputActor
    {
        [SerializeField] private float startValue = 0;
        [SerializeField] private float endValue = 1;
        [SerializeField] private float time = 1f;
        [SerializeField] private Easings.Type easingType = Easings.Type.Linear;
        [SerializeField] private bool constantSpeed = true;

        private CancellationTokenSource tokenSource;
        private float currentValue;
        
        protected override void OnStateUpdate(int state)
        {
            tokenSource?.Cancel();
            tokenSource = new CancellationTokenSource();
            Updater(state, tokenSource.Token);
        }

        private async void Updater(int state, CancellationToken token)
        {
            var start = currentValue;
            var end = state == 1 ? endValue : startValue;
            var timeElapsed = CalculateTime(start, end);

            var inverseTime = 1f / timeElapsed;
            for (var i = 0f; i <= 1f; i += Time.deltaTime * inverseTime)
            {
                var t = Easings.GetEasing(i, easingType);
                currentValue = Mathf.LerpUnclamped(start, end, t);
                Actor.SetValue(currentValue);
                
                if (token.IsCancellationRequested) return;
                await UniTask.Yield();
            }
            
            Actor.SetValue(end);
            currentValue = end;
        }

        private float CalculateTime(float startValue2, float endValue2)
        {
            if (constantSpeed)
            {
                var diff1 = Mathf.Abs(endValue - startValue);
                var diff2 = Mathf.Abs(endValue2 - startValue2);
                return time * (diff1 / diff2);
            }
            return time;
        }
    }
}