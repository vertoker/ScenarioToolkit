using Scenario.Base.Components.Actions;
using ScenarioToolkit.Bus;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class MaterialSystem : BaseScenarioSystem
    {
        public MaterialSystem(ScenarioComponentBus bus) : base(bus)
        {
            bus.Subscribe<SetMaterial>(SetMaterial);
        }

        private void SetMaterial(SetMaterial component)
        {
            if (AssertLog.NotNull<SetMaterial>(component.Renderer, nameof(component.Renderer))) return;
            //if (AssertLog.NotNull<SetMaterial>(component.Material, nameof(component.Material))) return;
            
            var sharesMaterials = component.Renderer.sharedMaterials;
            
            if (AssertLog.AboveEqual<SetMaterial>(component.Index, 0, nameof(component.Index))) return;
            if (AssertLog.Below<SetMaterial>(component.Index, sharesMaterials.Length, nameof(component.Index))) return;
            
            sharesMaterials[component.Index] = component.Material;
            component.Renderer.sharedMaterials = sharesMaterials;
        }
    }
}