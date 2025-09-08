using Modules.Scenario.Components.Actions;
using ScenarioToolkit.Bus;
using ScenarioToolkit.Core.Systems;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
    public class ParentingObjectSystem : BaseScenarioSystem
    {
        public ParentingObjectSystem(ScenarioComponentBus bus) : base(bus)
        {
            bus.Subscribe<UnparentObject>(UnparentObject);
            bus.Subscribe<ParentObject>(ParentObject);
        }

        private void UnparentObject(UnparentObject obj) => obj.Target.SetParent(null, true);
        private void ParentObject(ParentObject obj) => obj.Object.SetParent(obj.Parent, true);
    }
}