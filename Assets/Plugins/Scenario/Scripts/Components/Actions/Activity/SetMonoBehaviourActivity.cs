using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает активность MonoBehaviour через enabled")]
    public struct SetMonoBehaviourActivity : IScenarioAction, IComponentDefaultValues
    {
        public MonoBehaviour MonoBehaviour;
        public bool IsActive;
        
        public void SetDefault()
        {
            MonoBehaviour = null;
            IsActive = true;
        }
    }
}