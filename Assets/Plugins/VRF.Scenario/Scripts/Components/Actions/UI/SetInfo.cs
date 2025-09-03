using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;
using VRF.Scenario.UI.Game.InfoTip;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Actions
{
    [ScenarioMeta("Устанавливает данные в подсказку на руке", typeof(InfoTipView))]
    public struct SetInfo : IScenarioAction
    {
        public string Text;
        public Sprite Sprite;
    }
}