using Scenario.Core.Model.Interfaces;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Modules.Scenario.Components.Actions
{
    public class UnparentObject : IScenarioAction
    {
        public Transform Target;
    }
}