using Scenario.Base.Components.Actions;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class MaterialPropertiesSystem : BaseScenarioSystem
    {
        public MaterialPropertiesSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<SetRendererMaterialColorProperty>(SetRendererMaterialColorProperty);
            bus.Subscribe<SetRendererMaterialFloatProperty>(SetRendererMaterialFloatProperty);
            bus.Subscribe<SetRendererMaterialIntProperty>(SetRendererMaterialIntProperty);
            bus.Subscribe<SetRendererMaterialTextureProperty>(SetRendererMaterialTextureProperty);
            bus.Subscribe<SetRendererMaterialVectorProperty>(SetRendererMaterialVectorProperty);
        }

        private void SetRendererMaterialColorProperty(SetRendererMaterialColorProperty component)
        {
            if (GetMaterial(component.Renderer, component.MaterialIndex, out var material)) return;
            material.SetColor(component.PropertyName, component.Value);
        }

        private void SetRendererMaterialFloatProperty(SetRendererMaterialFloatProperty component)
        {
            if (GetMaterial(component.Renderer, component.MaterialIndex, out var material)) return;
            material.SetFloat(component.PropertyName, component.Value);
        }

        private void SetRendererMaterialIntProperty(SetRendererMaterialIntProperty component)
        {
            if (GetMaterial(component.Renderer, component.MaterialIndex, out var material)) return;
            material.SetInt(component.PropertyName, component.Value);
        }

        private void SetRendererMaterialTextureProperty(SetRendererMaterialTextureProperty component)
        {
            if (GetMaterial(component.Renderer, component.MaterialIndex, out var material)) return;
            material.SetTexture(component.PropertyName, component.Value);
        }

        private void SetRendererMaterialVectorProperty(SetRendererMaterialVectorProperty component)
        {
            if (GetMaterial(component.Renderer, component.MaterialIndex, out var material)) return;
            material.SetVector(component.PropertyName, component.Value);
        }

        private bool GetMaterial(Renderer renderer, int materialIndex, out Material material)
        {
            material = null;
            if (AssertLog.NotNull<SetMaterial>(renderer, nameof(renderer))) return true;
            
            var materials = renderer.materials;
            
            if (AssertLog.AboveEqual<SetMaterial>(materialIndex, 0, nameof(materialIndex))) return true;
            if (AssertLog.Below<SetMaterial>(materialIndex, materials.Length, nameof(materialIndex))) return true;
            
            material = materials[materialIndex];
            return false;
        }
    }
}