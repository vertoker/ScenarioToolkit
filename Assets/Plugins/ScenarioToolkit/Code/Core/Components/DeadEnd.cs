using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;
using Scenario.Utilities.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [ScenarioMeta("Компонент-тупик, который нельзя пройти, только ручной пропуск")]
    public struct DeadEnd : IScenarioCondition, IComponentDefaultValues
    {
        [ScenarioMeta("Чтобы нельзя было просто кинуть компонент в шину и пройти тупик")]
        public string Password;
        
        public void SetDefault()
        {
            Password = CryptoUtility.GetRandomString();
        }
    }
}