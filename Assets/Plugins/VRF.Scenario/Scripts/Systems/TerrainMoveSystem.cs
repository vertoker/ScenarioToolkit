using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Scenario.Core;
using Scenario.Core.Systems;
using UnityEngine;
using VRF.Components.Players.Views.Player;
using VRF.Identities;
using VRF.Identities.Core;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.Components.Conditions;
using VRF.Scenario.MonoBehaviours;
using VRF.Utils.Colliders;
using VRF.VRBehaviours.TerrainSystem;
using Zenject;

namespace VRF.Scenario.Systems
{
    // TODO адская херота, надо удалить или полностью переписать
    public class TerrainMoveSystem : BaseScenarioSystem, IFixedTickable
    {
        private const float StopDistance = 300;
        private const float SpeedChangeModifier = 0.1f;

        public event Action<float, float> SetSpeedOverTime;
        public event Action<float> SetPosition; 

        private readonly TerrainLoader terrainLoader;
        private readonly IdentityService identityService;
        private readonly CharacterController localPlayerController;
        private readonly Speedometer[] speedometers;
        private readonly TrainAudio[] trainAudios;
        private readonly List<GameObject> movingObjects = new();
        private readonly HashSet<TerrainScene> closeScenes = new();

        private bool moveLocalPlayer = false;
        private float speed;

        private CancellationTokenSource speedTokenSource = new();

        public TerrainMoveSystem(SignalBus bus, TerrainLoader terrainLoader,
                                 IEnumerable<Speedometer> speedometers,
                                 IEnumerable<TrainAudio> trainAudios,
                                 IdentityService identityService,
                                 [InjectOptional] LazyInject<PlayerVRView> localPlayerLazy) : base(bus)
        {
            this.terrainLoader = terrainLoader;
            this.identityService = identityService;
            this.trainAudios = trainAudios.ToArray();
            this.speedometers = speedometers.ToArray();
            
            if (localPlayerLazy != null && localPlayerLazy.Value && localPlayerLazy.Value.GroundProvider)
            {
                localPlayerLazy.Value.GroundProvider.TriggerEnter += c =>
                {
                    if (c.gameObject.name == "Ground")
                        moveLocalPlayer = true;
                };
                localPlayerLazy.Value.GroundProvider.TriggerExit += c =>
                {
                    if (c.gameObject.name == "Ground")
                        moveLocalPlayer = false;
                };
            }

            bus.Subscribe<SetTerrainSpeed>(component => SetSpeed(component.Speed, component.Time));

            bus.Subscribe<SetNextTerrain>(component => terrainLoader.LoadExtraTerrain(component.TerrainScene, component.ResetNextTerrainAfterUse));

            bus.Subscribe<MoveWithTerrain>(component => movingObjects.Add(component.MovingObject));
            bus.Subscribe<NotMoveWithTerrain>(component => movingObjects.Remove(component.MovingObject));

            bus.Subscribe<LoadExtraTerrain>(component => terrainLoader.LoadExtraTerrain(component.TerrainScene, false));
            bus.Subscribe<StopAtTerrain>(StopAtTerrain);
            bus.Subscribe<SetCurrentPosition>(SetCurrentPosition);

            bus.Subscribe<AccelerateTo>(SetSpeed);
            SetSpeed(0);
        }

        private void SetSpeed(AccelerateTo value)
        {
            var timeToReachSpeed = Mathf.Abs(Mathf.Abs(speed) - value.Speed) * SpeedChangeModifier;
            SetSpeed(-value.Speed, timeToReachSpeed);
        }

        private void SetCurrentPosition(SetCurrentPosition position)
        {
            terrainLoader.SetCurrentPosition(position.Position);
        }

        public void FixedTick()
        {
            if (terrainLoader.IsInitialized)
            {
                var moveDistance = (speed / 3.6f) * Time.fixedDeltaTime;
                terrainLoader.SetCurrentPosition(terrainLoader.CurrentPosition + moveDistance);
                SetPosition?.Invoke(terrainLoader.CurrentPosition + moveDistance);
                
                
                var movement = Vector3.left * moveDistance;

                foreach (var movingObject in movingObjects)
                    movingObject.transform.position += movement;

                if (moveLocalPlayer)
                    localPlayerController.Move(movement);
                
                
            }


            var stopScenes = terrainLoader.GetPossibleStopBinds(StopDistance).Select(b => b.Scene).ToList();

            foreach (var possibleStopScene in stopScenes)
            {
                if (!possibleStopScene.Scene.Contains("Terrain_Переезд")) //TODO: FIX THAT!!!
                    continue;

                if (closeScenes.Add(possibleStopScene))
                    Bus.Fire(new DistanceToStopPositionReached() { TerrainScene = possibleStopScene });
            }

            foreach (var closeScene in closeScenes.ToArray())
            {
                if (stopScenes.Contains(closeScene))
                    continue;
                closeScenes.Remove(closeScene);
                Bus.Fire(new DistanceToStopPositionPassed() { TerrainScene = closeScene });
            }
        }

        private void SetSpeed(float value)
        {
            speed = value;

            foreach (var speedometer in speedometers)
                speedometer.SetSpeed(speed);
            foreach (var audio in trainAudios)
                audio.SetSpeed(Mathf.Abs(speed));
        }

        private void SetSpeed(float targetSpeed, float time)
        {
            speedTokenSource?.Cancel();

            speedTokenSource = new CancellationTokenSource();

            SetSpeedOverTime?.Invoke(targetSpeed, time);
            SetSpeed(targetSpeed, time, speedTokenSource);
        }

        private async void SetSpeed(float targetSpeed, float time, CancellationTokenSource tokenSource)
        {
            var startSpeed = speed;
            var remainingTime = time;

            while (remainingTime > 0)
            {
                await UniTask.Yield();

                if (tokenSource.IsCancellationRequested)
                {
                    return;
                }

                remainingTime -= Time.deltaTime;

                SetSpeed(Mathf.Lerp(startSpeed, targetSpeed, 1 - remainingTime / time));
            }

            SetSpeed(targetSpeed);
            if (targetSpeed == 0)
                Bus.Fire(new StoppedAtTerrain() { TerrainScene = terrainLoader.CurrentTerrainScene });
        }

        private async void StopAtTerrain(StopAtTerrain component)
        {
            terrainLoader.SortBinds();
            var bind = terrainLoader.GetPossibleStopBinds(StopDistance)
                                    .FirstOrDefault(b => b.Scene == component.TerrainScene);

            var distanceToClosestStopPosition = 30f;
            if (bind != null) //TODO Maybe it'll work
                distanceToClosestStopPosition = terrainLoader.GetDistanceToStopPosition(bind.Scene);

            //if (distanceToClosestStopPosition > 30f)
              //  distanceToClosestStopPosition = 30f;

            /* Full calculations
            var startSpeed = Mathf.Abs(speed);
            const float endSpeed = 0f;
            var acceleration = (endSpeed * endSpeed - startSpeed * startSpeed) / (2 * distanceToClosestStopPosition);
            var d = 4 * startSpeed * startSpeed + 8 * acceleration * distanceToClosestStopPosition;
            var sqrtD = Mathf.Sqrt(d);
            var time1 = (-2 * startSpeed + sqrtD) / (2 * acceleration);
            var time2 = (-2 * startSpeed - sqrtD) / (2 * acceleration);
             */

            var startSpeed = Mathf.Abs(speed / 3.6f);
            var acceleration = -startSpeed * startSpeed / (2 * distanceToClosestStopPosition);
            var time = -startSpeed / acceleration;


            SetSpeed(0, time);
            await UniTask.WaitForSeconds(time);
            Bus.Fire(new StoppedAtTerrain() { TerrainScene = component.TerrainScene });
        }
    }
}