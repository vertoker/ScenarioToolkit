using Scenario.Core.Model.Interfaces;
using UnityEngine;

namespace ScenarioToolkit.Library.Components.Actions.Unity
{
    public struct SetAnimatorParameter : IScenarioAction
    {
        public Animator Animator;
        public AnimatorControllerParameterType ParameterType;
        public string ParameterName;
        public string Value;
    }
}