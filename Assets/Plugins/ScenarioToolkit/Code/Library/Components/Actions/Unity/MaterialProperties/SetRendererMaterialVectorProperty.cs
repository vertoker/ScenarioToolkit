using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Устанавливает Vector в material property через renderer материалы")]
    public struct SetRendererMaterialVectorProperty : IScenarioAction, IComponentDefaultValues
    {
        public Renderer Renderer;
        [ScenarioMeta("Установка идёт в renderer.materials по индексу")]
        public int MaterialIndex;
        public string PropertyName;
        
        public Vector4 Value;
        
        public void SetDefault()
        {
            Renderer = null;
            MaterialIndex = 0;
            PropertyName = string.Empty;
            
            Value = Vector4.zero;
        }
    }
}