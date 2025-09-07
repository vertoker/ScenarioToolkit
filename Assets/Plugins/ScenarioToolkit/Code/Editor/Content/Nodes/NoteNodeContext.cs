using System;
using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Nodes;

namespace ScenarioToolkit.Editor.Content.Nodes
{
    public class NoteNodeContext : IEditorNodeContent
    {
        public string TypeName => "Note";
        public string USSClass => "note";
        public Type NodeType => INoteNode.GetModelType;
        public Type InterfaceNodeType => typeof(INoteNode);
        public Type CompatibilityInterfaceNodeType => typeof(IScenarioCompatibilityNoteNode);
        public bool HasInput => false;
        public bool HasOutput => false;

        public string GetName(IScenarioNode node) => $" {node.Name}"; // для экономии места заметок
            //=> string.IsNullOrWhiteSpace(node.Name) ? " (Note)" : $" {node.Name}"; // старый способ
        
        public IEnumerable<IEditorNodeContent.Item> GetItems(IScenarioNode node, IScenarioContext context)
        {
            var noteNode = (INoteNode)node;
            return new[] { new IEditorNodeContent.Item(noteNode.Text, 14) };
        }
        
        public IScenarioNode CreateDefault() => INoteNode.CreateNew();
    }
}