using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Scenario.Core.Model.Interfaces;

namespace Scenario.Core.Nodes
{
    /// <summary>
    /// Интерфейс для статического контента для EditorNode. Содержит
    /// дополнительную информацию и методы расширения для разных EditorNode
    /// </summary>
    public interface IEditorNodeContent
    {
        public string TypeName { get; }
        public string USSClass { get; }
        public Type NodeType { get; }
        public Type InterfaceNodeType { get; }
        public Type CompatibilityInterfaceNodeType { get; }
        public bool HasInput { get; }
        public bool HasOutput { get; }

        public string GetName(IScenarioNode node);
        public IEnumerable<Item> GetItems(IScenarioNode node, [CanBeNull] IScenarioContext context);
        public IScenarioNode CreateDefault();
        
        public readonly struct Item
        {
            public readonly string Text;
            public readonly int Font;

            public Item(string text)
            {
                Text = text;
                Font = 12;
            }
            public Item(string text, int font)
            {
                Text = text;
                Font = font;
            }
        }
    }
}