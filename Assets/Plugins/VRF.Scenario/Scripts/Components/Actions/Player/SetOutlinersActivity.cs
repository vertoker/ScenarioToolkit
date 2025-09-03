using EPOOutline;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
[ScenarioMeta("Отключает/включает все outliner скрипты на сцене", typeof(Outliner))]
public struct SetOutlinersActivity : IScenarioAction, IComponentDefaultValues
{
    public bool IsActive;
    
    public void SetDefault()
    {
        IsActive = true;
    }
}