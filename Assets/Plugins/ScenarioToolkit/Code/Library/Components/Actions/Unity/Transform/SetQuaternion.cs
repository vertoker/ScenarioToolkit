using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Устанавливает значения для Transform.rotation")]
    public struct SetQuaternion : IScenarioAction, IComponentDefaultValues
    {
        public Transform Transform;
        public Quaternion Quaternion;
        [ScenarioMeta("Выбор между rotation и localRotation")]
        public bool Local;
        
        public void SetDefault()
        {
            Transform = null;
            Quaternion = Quaternion.identity;
            Local = true;
        }
    }
}