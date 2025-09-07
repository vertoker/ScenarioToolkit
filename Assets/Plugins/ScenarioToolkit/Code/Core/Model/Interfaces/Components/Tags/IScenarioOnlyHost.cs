// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary> Тэг для исполнения компонента только на хосте (сервере)
    /// <br></br> Если это Action, то просто блокирует отправку к клиенту
    /// <br></br> Если это Condition, то просто не принимает пакеты выполненного компонента от клиента
    /// </summary>
    public interface IScenarioOnlyHost
    {
        
    }
}