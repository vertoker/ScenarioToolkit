using Scenario.Core.Model.Interfaces;

namespace ScenarioToolkit.External.Components.Actions.PathDrawer
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