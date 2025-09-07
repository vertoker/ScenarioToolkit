using Scenario.Base.Components.Actions;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Core.Systems.States;
using ScenarioToolkit.Library.States;
using ScenarioToolkit.Shared;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class MaterialPropertiesSystem : BaseScenarioStateSystem<MaterialPropertiesState>
    {
        public MaterialPropertiesSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<SetRendererMaterialColorProperty>(SetRendererMaterialColorProperty);
            bus.Subscribe<SetRendererMaterialFloatProperty>(SetRendererMaterialFloatProperty);
            bus.Subscribe<SetRendererMaterialIntProperty>(SetRendererMaterialIntProperty);
            bus.Subscribe<SetRendererMaterialTextureProperty>(SetRendererMaterialTextureProperty);
            bus.Subscribe<SetRendererMaterialVectorProperty>(SetRendererMaterialVectorProperty);
        }

        protected override void ApplyState(MaterialPropertiesState state)
        {
            foreach (var (key, (defaultValue, value)) in state.Colors)
            {
                if(GetMaterial(key.Renderer, key.MaterialIndex, out var material)) return;
                material.SetColor(key.PropertyName, value);
            }
            
            foreach (var (key, (defaultValue, value)) in state.Floats)
            {
                if(GetMaterial(key.Renderer, key.MaterialIndex, out var material)) return;
                material.SetFloat(key.PropertyName, value);
            }
            
            foreach (var (key, (defaultValue, value)) in state.Ints)
            {
                if(GetMaterial(key.Renderer, key.MaterialIndex, out var material)) return;
                material.SetInt(key.PropertyName, value);
            }
            
            foreach (var (key, value) in state.Textures)
            {
                if(GetMaterial(key.Renderer, key.MaterialIndex, out var material)) return;
                material.SetTexture(key.PropertyName, value.Texture);
            }
            
            foreach (var (key, (defaultValue, value)) in state.Vectors)
            {
                if(GetMaterial(key.Renderer, key.MaterialIndex, out var material)) return;
                material.SetVector(key.PropertyName, value);
            }
        }

        private void SetRendererMaterialColorProperty(SetRendererMaterialColorProperty component)
        {
            if (GetMaterial(component.Renderer, component.MaterialIndex, out var material)) return;
            State.Colors.SetStateData(
                new MaterialPropertiesState.MaterialPropertyInfo(component.Renderer, component.MaterialIndex,
                    component.PropertyName), material.GetColor(component.PropertyName), component.Value);
            material.SetColor(component.PropertyName, component.Value);
        }

        private void SetRendererMaterialFloatProperty(SetRendererMaterialFloatProperty component)
        {
            if (GetMaterial(component.Renderer, component.MaterialIndex, out var material)) return;
            State.Floats.SetStateData(
                new MaterialPropertiesState.MaterialPropertyInfo(component.Renderer, component.MaterialIndex,
                    component.PropertyName), material.GetFloat(component.PropertyName), component.Value);
            material.SetFloat(component.PropertyName, component.Value);
        }

        private void SetRendererMaterialIntProperty(SetRendererMaterialIntProperty component)
        {
            if (GetMaterial(component.Renderer, component.MaterialIndex, out var material)) return;
            State.Ints.SetStateData(
                new MaterialPropertiesState.MaterialPropertyInfo(component.Renderer, component.MaterialIndex,
                    component.PropertyName), material.GetInt(component.PropertyName), component.Value);
            material.SetInt(component.PropertyName, component.Value);
        }

        private void SetRendererMaterialTextureProperty(SetRendererMaterialTextureProperty component)
        {
            if (GetMaterial(component.Renderer, component.MaterialIndex, out var material)) return;
            var bindDefault = new MaterialPropertiesState.TextureData(material.GetTexture(component.PropertyName));
            var bindCurrent = new MaterialPropertiesState.TextureData(component.Value);
            State.Textures.SetStateData(State.DefaultTextureDatas,
                new MaterialPropertiesState.MaterialPropertyInfo(component.Renderer, component.MaterialIndex,
                    component.PropertyName), bindDefault, bindCurrent);
            material.SetTexture(component.PropertyName, component.Value);
        }

        private void SetRendererMaterialVectorProperty(SetRendererMaterialVectorProperty component)
        {
            if (GetMaterial(component.Renderer, component.MaterialIndex, out var material)) return;
            State.Vectors.SetStateData(
                new MaterialPropertiesState.MaterialPropertyInfo(component.Renderer, component.MaterialIndex,
                    component.PropertyName), material.GetVector(component.PropertyName), component.Value);
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