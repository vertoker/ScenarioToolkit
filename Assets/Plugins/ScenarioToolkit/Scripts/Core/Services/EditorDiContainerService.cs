using System;
using Zenject;

namespace Scenario.Core.Services
{
    /// <summary>
    /// Костыль для работы GraphEditor с текущим игровым контейнером
    /// </summary>
    public static class EditorDiContainerService
    {
        public static DiContainer Container { get; private set; }
        public static event Action ContainerUpdated;
        public static event Action ContainerRemoved;

        public static void OnUpdateContainer(DiContainer container)
        {
            Container = container;
            ContainerUpdated?.Invoke();
        }
        public static void OnRemoveContainer()
        {
            Container = null;
            ContainerRemoved?.Invoke();
        }
    }
}