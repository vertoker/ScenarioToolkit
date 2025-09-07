using Zenject;

namespace ScenarioToolkit.Shared.VRF.Utilities.VRF
{
    /// <summary>
    /// Расширения для DI контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        public static TContract EnsureBind<TContract>(this DiContainer container, TContract item) where TContract : class
        {
            var list = container.TryResolve<TContract>();
            if (list != null) return list;

            if (item != null)
            {
                container.BindInstance(item).AsSingle();
                return item;
            }

            return null;
        }
    }
}