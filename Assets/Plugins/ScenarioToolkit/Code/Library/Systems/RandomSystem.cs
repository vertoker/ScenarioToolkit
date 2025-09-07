using Cysharp.Threading.Tasks;
using Modules.Scenario.Components.Actions;
using Scenario.Core.Systems;
using UnityEngine;
using Zenject;

namespace Scenario.Systems
{
    public class RandomSystem : BaseScenarioSystem
    {
        public RandomSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<SendRandomNumber>(SendRandomNumber);
        }

        private async void SendRandomNumber(SendRandomNumber component)
        {
            await UniTask.Yield();
            
            var randomNumber = Random.Range(component.Min, component.Max + 1);

            Bus.Fire(new RandomNumberSent() { Number = randomNumber });
        }
    }
}