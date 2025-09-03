using Zenject;

namespace Scenario.Core.Systems
{
    /// <summary>
    /// Основа для любой сценарной системы, в первую очередь нужен как маркер
    /// </summary>
    public abstract class BaseScenarioSystem
    {
        protected readonly SignalBus Bus;

        protected BaseScenarioSystem(SignalBus bus)
        {
            Bus = bus;
        }
    }
}