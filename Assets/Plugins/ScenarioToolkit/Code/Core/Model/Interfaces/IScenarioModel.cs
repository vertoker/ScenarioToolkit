using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Главная модель всего Scenario Framework, именно через этот интерфейс работает функционал всего модуля.
    /// Также им обозначается последняя актуальная модель сценария (и только одна)
    /// </summary>
    public interface IScenarioModel : IModelReflection<ScenarioModelV6, IScenarioModel>, 
        IScenarioCompatibilityModel
    {
        public IScenarioContext Context { get; set; }
        public IScenarioGraph Graph { get; set; }
        public IEditorGraph EditorGraph { get; set; }
    }
}