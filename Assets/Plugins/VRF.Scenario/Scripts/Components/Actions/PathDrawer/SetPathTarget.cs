using Scenario.Core.Model.Interfaces;
using UnityEngine;

namespace Modules.Scenario.Components.Actions
{
    public struct SetPathTarget : IScenarioAction
    {
        public Transform Target;
    }
}