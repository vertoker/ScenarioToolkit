using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions.Items
{
    public struct PlayerBeltSetActive : IScenarioAction, IComponentDefaultValues
    {
        public bool Active;
        
        public void SetDefault()
        {
            Active = true;
        }
    }
}