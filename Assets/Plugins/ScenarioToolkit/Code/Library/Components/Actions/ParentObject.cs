using Scenario.Core.Model.Interfaces;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Modules.Scenario.Components.Actions
{
    public class ParentObject : IScenarioAction
    {
        public Transform Object;
        public Transform Parent;
    }
}