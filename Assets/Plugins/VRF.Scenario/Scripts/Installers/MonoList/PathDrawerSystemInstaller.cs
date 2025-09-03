using Dreamteck.Splines;
using Modules.Scenario.Systems;
using UnityEngine;
using Zenject;

namespace Modules.Scenario.Installers
{
    public class PathDrawerSystemInstaller : MonoInstaller
    {
        [SerializeField] private SplineComputer splineComputer;
        [SerializeField] private GameObject arrowPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PathDrawerSystem>()
                .AsSingle()
                .WithArguments(splineComputer, arrowPrefab)
                .NonLazy();
        }
    }
}