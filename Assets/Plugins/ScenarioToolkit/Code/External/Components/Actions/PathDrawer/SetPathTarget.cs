using Scenario.Core.Model.Interfaces;
using UnityEngine;

namespace ScenarioToolkit.External.Components.Actions.PathDrawer
{
    public struct SetPathTarget : IScenarioAction
    {
        public Transform Target;
    }
}