using Zenject;

namespace ScenarioToolkit.Core.Installers.Resolver
{
    public class SystemBootstrap : IInitializable
    {
        private readonly SystemResolver resolver;

        public SystemBootstrap(SystemResolver resolver)
        {
            this.resolver = resolver;
            Resolve();
        }
        
        // нужно исключительно чтобы оно создалось без внешних зависимостей
        public void Initialize() { /*Resolve();*/ }

        private void Resolve()
        {
            resolver.Resolve();
        }
    }
}