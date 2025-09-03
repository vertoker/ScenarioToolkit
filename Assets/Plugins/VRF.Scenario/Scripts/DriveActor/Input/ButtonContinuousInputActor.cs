using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VRF.Scenario.DriveActor.Core;

namespace VRF.Scenario.DriveActor.Input
{
    public class ButtonContinuousInputActor : BaseTransformStatesInputActor
    {
        [SerializeField] private float speed = 0.001f;
        [SerializeField] private bool inverseSpeed = false;
        [SerializeField] private bool clamp = true;
        [SerializeField] private PlayerLoopTiming timing = PlayerLoopTiming.FixedUpdate;

        private CancellationTokenSource _tokenSource;
        private bool _active;
        
        protected override void OnStateUpdate(int state)
        {
            _active = state == 1;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _tokenSource = new CancellationTokenSource();
            Updater(_tokenSource.Token).Forget();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            
            _active = false;
            _tokenSource?.Cancel(false);
        }
        
        private async UniTaskVoid Updater(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (_active)
                {
                    var value = inverseSpeed ? Actor.Value - speed : Actor.Value + speed;
                    if (clamp) value = Mathf.Clamp(value, 0, BaseDriveActor.OneEpsilon);
                    Actor.SetValue(value);
                }
                await UniTask.Yield(timing);
            }
        }
    }
}