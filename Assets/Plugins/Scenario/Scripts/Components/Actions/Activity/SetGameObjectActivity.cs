using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Устанавливает активность GameObject через SetActive")]
    public struct SetGameObjectActivity : IScenarioAction, IComponentDefaultValues
    {
        public GameObject GameObject;
        public bool IsActive;
        
        public void SetDefault()
        {
            GameObject = null;
            IsActive = true;
        }
    }
}