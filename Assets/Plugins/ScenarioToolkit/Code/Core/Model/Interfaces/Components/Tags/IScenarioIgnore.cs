// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary> Тэг для игнорирования компонентов при исполнении
    /// <br></br> Если это Action, то он не выполниться в шине
    /// <br></br> Если это Condition, то он не будет учитываться как выполненное условие
    /// </summary>
    public interface IScenarioIgnore
    {
        
    }
}