using Scenario.Base.Components.Actions;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Core.Systems.States;
using ScenarioToolkit.Library.States;
using ScenarioToolkit.Shared;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class MaterialSystem : BaseScenarioStateSystem<MaterialState>
    {
        public MaterialSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<SetMaterial>(SetMaterial);
        }

        protected override void ApplyState(MaterialState state)
        {
            foreach (var materialPath in state.Materials.Keys)
            {
                var material = materialPath.Item1.materials[materialPath.Item2];
                state.DefaultMaterials.Add(materialPath, new MaterialState.Data(material));
            }
            foreach (var material in state.Materials)
            {
                var sharesMaterials = material.Key.Item1.sharedMaterials;
            
                sharesMaterials[material.Key.Item2] = material.Value.Material;
                material.Key.Item1.sharedMaterials = sharesMaterials;
            }
        }

        private void SetMaterial(SetMaterial component)
        {
            if (AssertLog.NotNull<SetMaterial>(component.Renderer, nameof(component.Renderer))) return;
            //if (AssertLog.NotNull<SetMaterial>(component.Material, nameof(component.Material))) return;
            
            var sharesMaterials = component.Renderer.sharedMaterials;
            
            if (AssertLog.AboveEqual<SetMaterial>(component.Index, 0, nameof(component.Index))) return;
            if (AssertLog.Below<SetMaterial>(component.Index, sharesMaterials.Length, nameof(component.Index))) return;

            var key = (component.Renderer, component.Index);
            var dataDefault = new MaterialState.Data(sharesMaterials[component.Index]);
            var dataCurrent = new MaterialState.Data(component.Material);
            State.Materials.SetStateData(State.DefaultMaterials, key, dataDefault, dataCurrent);
            
            sharesMaterials[component.Index] = component.Material;
            component.Renderer.sharedMaterials = sharesMaterials;
        }
    }
}