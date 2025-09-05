using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Умножает значения на Transform.rotation")]
    public struct MulQuaternion : IScenarioAction, IComponentDefaultValues
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

        public Quaternion GetQuaternion()
        {
            if (Local) return Transform.localRotation * Quaternion;
            return Transform.rotation * Quaternion;
        }
    }
}