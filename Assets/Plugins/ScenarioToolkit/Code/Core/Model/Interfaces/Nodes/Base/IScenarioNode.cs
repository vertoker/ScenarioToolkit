using System.Threading.Tasks;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Главная модель внутри моделей всего Scenario Framework. Не является чистой моделью, так как
    /// помимо данных, обладает логикой исполнения. Логику и внутренние данные определяют сами ноды
    /// </summary>
    [JsonObject(IsReference = true)]
    public interface IScenarioNode : IHashableSource, IScenarioCompatibilityNode
    {
        public string Name { get; set; }
        
        public string GetStatusString();
    }
}