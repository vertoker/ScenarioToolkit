using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Меняет материал для Unity Renderer (любого)")]
    public struct SetMaterial : IScenarioAction, IComponentDefaultValues
    {
        public Renderer Renderer;
        [ScenarioMeta("Установка идёт в renderer.materials по индексу")]
        public int Index;
        [ScenarioMeta("Может быть null", CanBeNull = true)]
        public Material Material;
        
        public void SetDefault()
        {
            Renderer = null;
            Index = 0;
            Material = null;
        }
    }
}