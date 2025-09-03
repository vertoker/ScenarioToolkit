using System;
using Cysharp.Threading.Tasks;
using KBCore.Refs;
using NaughtyAttributes;
using UnityEngine;

namespace VRF.IK
{
    public enum ActiveFoot
    {
        None,
        Left,
        Right
    }
    
    /// <summary>
    /// Контроллер для указания шага
    /// </summary>
    public class PlayerFootsIK : ValidatedMonoBehaviour
    {
        [Header("Core")]
        [SerializeField] private float walkingDistanceMultiplier = 2f;
        [SerializeField] private float stepTime = 0.2f;
        
        [Header("Height")]
        [SerializeField, MinMaxSlider(0, 1)] private Vector2 heightBoundsStep = new(0.1f, 0.3f);
        //[SerializeField] private float stepLengthStandard = 0.6f;
        
        [Header("Triggers")]
        [SerializeField] private float stepTriggerToIdle = 0.1f;
        [SerializeField] private float stepTriggerToWalk = 0.4f;
        [SerializeField] private float stepTriggerInstant = 1f;
        
        [Header("References")]
        [SerializeField] private PlayerIK playerIK;

        [Header("Debug")]
        private Vector2 lastLeftFootPos;
        private Vector2 lastRightFootPos;
        private Vector2 lastPlayer;
        private float lastAngle;
        private ActiveFoot activeFoot;
        
        //[SerializeField, ReadOnly]
        private Vector2 currentLeftFootPos;
        //[SerializeField, ReadOnly]
        private Vector2 currentRightFootPos;
        //[SerializeField, ReadOnly]
        private Vector2 currentHeelHeights;
        
        public void InitIK()
        {
            Reset();
        }

        public void UpdateIK()
        {
            var leftDistance = Vector2.Distance(lastLeftFootPos, playerIK.LeftLeg.PointXZ);
            var rightDistance = Vector2.Distance(lastRightFootPos, playerIK.RightLeg.PointXZ);

            if (activeFoot != ActiveFoot.None) return;
            
            if (ExecuteInstant(leftDistance, rightDistance)) return;

            if (Execute(leftDistance, rightDistance, stepTriggerToWalk,
                    ExecuteWalkLeftStep, ExecuteWalkRightStep)) return;
            if (Execute(leftDistance, rightDistance, stepTriggerToIdle,
                    ExecuteIdleLeftStep, ExecuteIdleRightStep)) return;
        }

        private bool ExecuteInstant(float leftDistance, float rightDistance)
        {
            var leftLegOutOfBounds = leftDistance > stepTriggerInstant;
            var rightLegOutOfBounds = rightDistance > stepTriggerInstant;
            
            if (leftLegOutOfBounds && rightLegOutOfBounds)
            {
                if (leftDistance > rightDistance)
                    ResetLeft(); else ResetRight();
            }
            else if (leftLegOutOfBounds) ResetLeft();
            else if (rightLegOutOfBounds) ResetRight();
            else return false;

            return true;
        }

        private bool Execute(float leftDistance, float rightDistance, float stepTrigger,
            Action leftExecute, Action rightExecute)
        {
            var leftLegCondition = leftDistance >= stepTrigger;
            var rightLegCondition = rightDistance >= stepTrigger;

            if (leftLegCondition && rightLegCondition)
            {
                /*var direction = playerIK.CurrentPosXZ - _lastPlayer;
                var forward = playerIK.PlayerForward;
                var angle = Vector2.SignedAngle(forward, direction);*/

                if (leftDistance > rightDistance)
                    leftExecute(); else rightExecute();
            }
            else if (leftLegCondition) leftExecute();
            else if (rightLegCondition) rightExecute();
            else return false;
            
            return true;
        }

        private Vector2 CalculateWalkStep(Vector2 origin, Vector2 target)
        {
            var direction = target - origin;
            return origin + direction * walkingDistanceMultiplier;
        }
        
        private void ExecuteIdleLeftStep()
        {
            activeFoot = ActiveFoot.Left;
            LeftStepAction(playerIK.LeftLeg.PointXZ);
        }
        private void ExecuteIdleRightStep()
        {
            activeFoot = ActiveFoot.Right;
            RightStepAction(playerIK.RightLeg.PointXZ);
        }
        
        private void ExecuteWalkLeftStep()
        {
            activeFoot = ActiveFoot.Left;
            var nextStep = CalculateWalkStep(lastLeftFootPos, playerIK.LeftLeg.PointXZ);
            LeftStepAction(nextStep);
        }
        private void ExecuteWalkRightStep()
        {
            activeFoot = ActiveFoot.Right;
            var nextStep = CalculateWalkStep(lastLeftFootPos, playerIK.RightLeg.PointXZ);
            RightStepAction(nextStep);
        }

        private void LeftStepAction(Vector2 target)
        {
            StepAction(lastLeftFootPos, target, 
                LStepY, LStepXZ, LStepForce);
        }
        private void RightStepAction(Vector2 target)
        {
            StepAction(lastRightFootPos, target, 
                RStepY, RStepXZ, RStepForce);
        }


        #region Core
        private float GetCurrentStepHeight(Vector2 startPoint, Vector2 endPoint)
        {
            //var distance = Vector2.Distance(startPoint, endPoint);
            return Mathf.Lerp(heightBoundsStep.x, heightBoundsStep.y, playerIK.HeadProgressClamped);
        }
        
        private async void StepAction(Vector2 lastFootPos, Vector2 nextFootPos, 
            Action<float> stepY, Action<Vector2> stepXZ, Action<Vector2> endStep)
        {
            var currentHeightStep = GetCurrentStepHeight(lastFootPos, nextFootPos);
            
            for (var t = 0f; t <= 1f; t += Time.deltaTime / stepTime)
            {
                await UniTask.Yield();
                
                var pos = Vector2.Lerp(lastFootPos, nextFootPos, t);
                var heightProgress = Mathf.Sin(t * Mathf.PI);
                
                stepY(heightProgress * currentHeightStep);
                stepXZ(pos);
            }
            
            endStep(nextFootPos);
            
            ResetPlayer();
            activeFoot = ActiveFoot.None;
        }
        #endregion

        #region Setup
        private void LStepY(float y) => currentHeelHeights = new Vector2(y, currentHeelHeights.y);
        private void RStepY(float y) => currentHeelHeights = new Vector2(currentHeelHeights.x, y);
        private void LStepXZ(Vector2 xz) => currentLeftFootPos = xz;
        private void RStepXZ(Vector2 xz) => currentRightFootPos = xz;
        
        private void LStepForce(Vector2 endPos)
        {
            lastLeftFootPos = endPos;
            LStepXZ(endPos);
            LStepY(0);
        }
        private void RStepForce(Vector2 endPos)
        {
            lastRightFootPos = endPos;
            RStepXZ(endPos);
            RStepY(0);
        }
        
        private void ResetPlayer()
        {
            if (!playerIK) return;
            lastPlayer = playerIK.CurrentPosXZ;
            lastAngle = playerIK.CurrentAngle;
        }
        private void ResetLeft()
        {
            LStepForce(playerIK.LeftLeg.PointXZ);
            ResetPlayer();
        }
        private void ResetRight()
        {
            RStepForce(playerIK.RightLeg.PointXZ);
            ResetPlayer();
        }
        private void Reset()
        {
            LStepForce(playerIK.LeftLeg.PointXZ);
            RStepForce(playerIK.RightLeg.PointXZ);
            ResetPlayer();
        }
        #endregion

        public Vector3 OverrideLeftFoot(Vector3 leftFoot)
        {
            return new Vector3(currentLeftFootPos.x, leftFoot.y + currentHeelHeights.x, currentLeftFootPos.y);
        }
        public Vector3 OverrideRightFoot(Vector3 rightFoot)
        {
            return new Vector3(currentRightFootPos.x, rightFoot.y + currentHeelHeights.y, currentRightFootPos.y);
        }
    }
}