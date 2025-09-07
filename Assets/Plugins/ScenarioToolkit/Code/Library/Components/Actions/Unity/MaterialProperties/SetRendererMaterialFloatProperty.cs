using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Устанавливает float в material property через renderer материалы")]
    public struct SetRendererMaterialFloatProperty : IScenarioAction, IComponentDefaultValues
    {
        public Renderer Renderer;
        [ScenarioMeta("Установка идёт в renderer.materials по индексу")]
        public int MaterialIndex;
        public string PropertyName;
        
        public float Value;
        
        public void SetDefault()
        {
            Renderer = null;
            MaterialIndex = 0;
            PropertyName = string.Empty;
            
            Value = 1f;
        }
    }
}