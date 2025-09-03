using UnityEngine;
using VRF.VRBehaviours;

namespace VRF.Scenario.DriveActor.Core
{
    public abstract class BaseTransformStatesInputActor : BaseInputActor
    {
        [SerializeField] private GOTransformStates transformStates;
        private bool _subscribed;

        public GOTransformStates TransformStates => transformStates;

        protected virtual void OnEnable()
        {
            if (transformStates && !_subscribed)
            {
                transformStates.StateChanged += StateChanged;
                _subscribed = true;
            }
        }
        protected virtual void OnDisable()
        {
            if (transformStates && _subscribed)
            {
                transformStates.StateChanged -= StateChanged;
                _subscribed = false;
            }
        }
        
        private void StateChanged(GOTransformStates states, int state)
        {
            //Debug.Log(state);
            OnStateUpdate(state);
        }
        protected abstract void OnStateUpdate(int state);
    }
}