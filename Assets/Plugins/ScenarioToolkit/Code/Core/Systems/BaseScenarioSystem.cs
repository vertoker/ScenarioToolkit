using ScenarioToolkit.Bus;

namespace ScenarioToolkit.Core.Systems
{
    /// <summary>
    /// Основа для любой сценарной системы, в первую очередь нужен как маркер
    /// </summary>
    public abstract class BaseScenarioSystem
    {
        protected readonly ScenarioComponentBus Bus;

        protected BaseScenarioSystem(ScenarioComponentBus bus)
        {
            Bus = bus;
        }
    }
}