using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Устанавливает int в material property через renderer материалы")]
    public struct SetRendererMaterialIntProperty : IScenarioAction, IComponentDefaultValues
    {
        public Renderer Renderer;
        [ScenarioMeta("Установка идёт в renderer.materials по индексу")]
        public int MaterialIndex;
        public string PropertyName;
        
        public int Value;
        
        public void SetDefault()
        {
            Renderer = null;
            MaterialIndex = 0;
            PropertyName = string.Empty;
            
            Value = 0;
        }
    }
}