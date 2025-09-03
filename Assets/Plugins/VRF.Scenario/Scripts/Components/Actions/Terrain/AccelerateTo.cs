using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    public struct AccelerateTo : IScenarioAction, IComponentDefaultValues
    {
        public float Speed;
        
        public void SetDefault()
        {
            Speed = 1;
        }
    }
}