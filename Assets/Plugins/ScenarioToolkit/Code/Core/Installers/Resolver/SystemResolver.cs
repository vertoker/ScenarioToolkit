using System;
using System.Collections.Generic;
using Zenject;

namespace Scenario.Core.Installers.Resolver
{
    public class SystemResolver
    {
        private readonly List<Action> actions = new();
        private readonly DiContainer container;
        
        public SystemResolver(DiContainer container)
        {
            this.container = container;
        }
        
        public void AddPromiseResolve<TSystem>() where TSystem : class
        {
            actions.Add(Action);
            return;
            
            void Action() => container.TryResolve<TSystem>();
        }

        public void Resolve()
        {
            foreach (var action in actions)
                action.Invoke();
        }
    }
}