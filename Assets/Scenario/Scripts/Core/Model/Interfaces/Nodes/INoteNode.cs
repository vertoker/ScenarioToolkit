using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Нода, не имеющая логики и необходимая исключительно для заметок внутри самого редактора сценариев
    /// </summary>
    public interface INoteNode : IScenarioNode, 
        IModelReflection<NoteNodeV6, INoteNode>, IScenarioCompatibilityNoteNode
    {
        public string Text { get; set; }
    }
}