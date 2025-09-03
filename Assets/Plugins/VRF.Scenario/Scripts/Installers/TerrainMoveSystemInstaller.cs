using System;
using NaughtyAttributes;
using Scenario.Core.Installers.Systems;
using Scenario.Utilities.Extensions;
using UnityEngine;
using VRF.Scenario.MonoBehaviours;
using VRF.Scenario.Systems;
using VRF.VRBehaviours.TerrainSystem;

namespace VRF.Scenario.Installers
{
    [Obsolete] // TODO адская херота, надо удалить или полностью переписать
    public class TerrainMoveSystemInstaller : BaseSystemInstaller
    {
        [SerializeField] private bool useNetwork;
        [SerializeField, ShowIf(nameof(useNetwork))] private TerrainMoveNetwork network;
        
        public override void InstallBindings()
        {
            var speedometers = FindObjectsByType<Speedometer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            var trainSounds = FindObjectsByType<TrainAudio>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            
            Container.BindScenarioSystem<TerrainMoveSystem>(GetResolver())
                .WithArguments(speedometers, trainSounds);

            if (useNetwork && network)
                Container.BindInstance(network);
        }
    }
}