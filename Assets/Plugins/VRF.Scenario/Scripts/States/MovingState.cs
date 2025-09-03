using System.Collections.Generic;
using Newtonsoft.Json;
using Scenario.Core.Systems.States;
using SimpleUI.Extensions;
using UnityEngine;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Systems;

namespace VRF.Scenario.States
{
    public class MovingState : IState
    {
        public Dictionary<Transform, Data> Transforms = new();
        
        public class Data : NetworkContinuousData
        {
            public MovingSystem.TransformInfo StartTransformInfo;
            public MovingSystem.TransformInfo EndTransformInfo;
            public bool AnimatePosition;
            public bool AnimateRotation;
            public bool AnimateScale;
            public Easings.Type Ease;
            
            [JsonConstructor]
            public Data(MovingSystem.TransformInfo startTransformInfo, 
                MovingSystem.TransformInfo endTransformInfo,
                float time,
                bool animatePosition, 
                bool animateRotation, 
                bool animateScale, 
                Easings.Type ease) : base(time)
            {
                StartTransformInfo = startTransformInfo;
                EndTransformInfo = endTransformInfo;
                AnimatePosition = animatePosition;
                AnimateRotation = animateRotation;
                AnimateScale = animateScale;
                Ease = ease;
            }

            public Data(AnimateTransform component) : base(component.Time)
            {
                StartTransformInfo = new MovingSystem.TransformInfo(component.MovingObject);
                EndTransformInfo = new MovingSystem.TransformInfo(component.Target);
                AnimatePosition = component.AnimatePosition;
                AnimateRotation = component.AnimateRotation;
                AnimateScale = component.AnimateScale;
                Ease = component.Ease;
            }
        }

        public void Clear()
        {
            Transforms.Clear();
        }
    }
}