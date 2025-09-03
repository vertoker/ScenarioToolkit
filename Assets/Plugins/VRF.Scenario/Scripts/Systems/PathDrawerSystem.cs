using System;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using Modules.Scenario.Components.Actions;
using Scenario.Core.Systems;
using UnityEngine;
using UnityEngine.AI;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Players.Views.Player;
using VRF.Players.Core;
using Zenject;
using Object = UnityEngine.Object;

namespace Modules.Scenario.Systems
{
    public class PathDrawerSystem : BaseScenarioSystem, ITickable
    {
        private const float speed = 1f;
        private const float arrowLength = 1f;
        private readonly GameObject arrowPrefab;

        private readonly List<SplineFollower> arrows = new();
        private readonly Transform parent;
        private readonly NavMeshPath path;
        private Transform cameraTransform;
        private readonly float updateTime = 1f;
        private float elapsed = 0f;
        private SplineComputer spline;
        private PlayersContainer playersContainer;

        private Transform target;
        private bool canSetTarget = true;

        public PathDrawerSystem(SignalBus listener,
            SplineComputer pathSpline,
            GameObject arrowPrefab,
            PlayersContainer playersContainer) : base(listener)
        {
            this.arrowPrefab = arrowPrefab;
            this.playersContainer = playersContainer;
            spline = pathSpline;
            path = new NavMeshPath();
            elapsed = 0.0f;
            var view = playersContainer.CurrentValue.View;
            if(view && view.Camera)
                cameraTransform = view.Camera.transform;
            playersContainer.PlayerChanged += UpdateCameraTransform;
            parent = new GameObject("Arrows").transform;
            
            Bus.Subscribe<SetPathTarget>(SetPathTarget);
            Bus.Subscribe<ResetPathTarget>(ResetPathTarget);
            Bus.Subscribe<TogglePathTarget>(TogglePathTarget);
            PlayerTeleport.OnBeforeTeleport += UpdatePath;
        }

        private void UpdateCameraTransform()
        {
            var view = playersContainer.CurrentValue.View;
            if(view && view.Camera)
                cameraTransform = view.Camera.transform;
        }

        private void SetPathTarget(SetPathTarget component)
        {
            if (canSetTarget)
            {
                target = component.Target;
            }
        }
        private void ResetPathTarget(ResetPathTarget component)
        {
            target = null;
        }
        private void TogglePathTarget(TogglePathTarget component)
        {
            canSetTarget = component.Active;
        }
        
        public void Tick()
        {
            if (!cameraTransform) return;
            
            elapsed += Time.deltaTime;
            if (elapsed > updateTime)
            {
                elapsed %= updateTime;
                UpdatePath();
            }

            var arrowCount = arrows.Count;
            var totalLength = spline.CalculateLength();
            var neededCount = totalLength / arrowLength;

            var t = Time.time * speed % 1.0f;
            var delta = arrowCount - neededCount;
            var realArrowLength = totalLength / arrowCount;


            while (Mathf.Abs(delta) > 0.5f)
            {
                switch (delta)
                {
                    case > 0:
                        Object.Destroy(arrows[^1].gameObject);
                        arrows.RemoveAt(arrows.Count - 1);
                        break;
                    case < 0:
                        var newArrow = Object.Instantiate(arrowPrefab, parent);
                        var splineController = newArrow.AddComponent<SplineFollower>();
                        splineController.spline = spline;
                        splineController.wrapMode = SplineFollower.Wrap.Loop;
                        splineController.followSpeed = 0;
                        arrows.Add(splineController);
                        break;
                }

                arrowCount = arrows.Count;
                delta = arrowCount - neededCount;
            }

            for (var i = 0; i < arrowCount; i++)
                arrows[i].SetDistance((float)i / arrowCount * totalLength + realArrowLength * t);
        }

        private void UpdatePath()
        {
            if (!cameraTransform)
                return; //TODO: GENERALIZE

            if (!spline)
                spline = Object.FindFirstObjectByType<SplineComputer>();
            if (cameraTransform && target)
            {
                NavMesh.SamplePosition(cameraTransform.position, out var hit, 2.5f, NavMesh.AllAreas);
                if (hit.hit)
                {
                    NavMesh.CalculatePath(hit.position, target.position, NavMesh.AllAreas, path);
                }

                spline.SetPoints(path.corners.Select(pos => new SplinePoint(pos + Vector3.up * 0.5f)).ToArray());
            }
            else
            {
                spline.SetPoints(Array.Empty<SplinePoint>());
            }

            spline.RebuildImmediate(true);
        }
    }
}