using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Дополнительные данные для редактора от IScenarioLink в обычном графе
    /// </summary>
    public interface IEditorLink : IHashable, IModelReflection<EditorLinkV6, IEditorLink>
    {
        public IEditorNode From { get; set; }
        public IEditorNode To { get; set; }
    }
}