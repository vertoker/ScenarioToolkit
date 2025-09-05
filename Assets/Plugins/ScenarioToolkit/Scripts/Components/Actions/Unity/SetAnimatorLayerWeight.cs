using Scenario.Core.Model.Interfaces;
using UnityEngine;

namespace Scenario.Components.Actions.Unity
{
    public struct SetAnimatorLayerWeight : IScenarioAction
    {
        public Animator Animator;
        public int LayerIndex;
        public float Weight;
    }
}