using Scenario.Core.Model.Interfaces;

namespace Modules.Scenario.Components.Actions
{
    public struct TogglePathTarget : IScenarioAction, IComponentDefaultValues
    {
        public bool Active;
        
        public void SetDefault()
        {
            Active = true;
        }
    }
}