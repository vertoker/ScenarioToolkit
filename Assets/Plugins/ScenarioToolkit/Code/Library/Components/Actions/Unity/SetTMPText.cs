using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using TMPro;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Устанавливает текст для TMP_Text")]
    public struct SetTMPText : IScenarioAction, IComponentDefaultValues
    {
        public TMP_Text TMPText;
        public string Text;
        
        public void SetDefault()
        {
            TMPText = null;
            Text = string.Empty;
        }
    }
}