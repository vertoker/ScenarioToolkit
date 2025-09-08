using ScenarioToolkit.Core.Installers.Systems;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Core.Installers.Resolver
{
    public class SystemResolverInstaller : MonoInstaller
    {
        public SystemResolver Resolver { get; private set; }

        public override void InstallBindings()
        {
            Resolver = new SystemResolver(Container);
            Container.BindInterfacesAndSelfTo<SystemResolver>().FromInstance(Resolver).AsSingle();
        }
        
        public void UpdateSystemsInstaller()
        {
            var systemInstallers = FindObjectsByType<BaseSystemInstaller>(FindObjectsSortMode.None);
            foreach (var systemInstaller in systemInstallers)
                systemInstaller.ResolverInstaller = this;
        }
    }
}