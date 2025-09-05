using NaughtyAttributes;
using Scenario.Core.Installers.Resolver;
using UnityEngine;
using Zenject;

namespace Scenario.Core.Installers.Systems
{
    public abstract class BaseSystemInstaller : MonoInstaller
    {
        [SerializeField] private SystemResolverInstaller resolverInstaller;
        
        public SystemResolverInstaller ResolverInstaller
        {
            get => resolverInstaller;
            set => resolverInstaller = value;
        }

        public SystemResolver GetResolver()
        {
            //Debug.Log(name);
            return resolverInstaller.Resolver;
        }

        [ContextMenu(nameof(OnValidate))]
        public virtual void OnValidate()
        {
            //ResolverInstaller ??= FindFirstObjectByType<SystemResolverInstaller>();
            var newName = GetType().Name;
            if (name != newName)
                name = newName;
        }
    }
}