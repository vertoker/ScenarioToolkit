using System.Collections.Generic;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Сущность группы для нод, существует только в редакторе сценариев
    /// </summary>
    public interface IEditorGroup : IHashableSource, IModelReflection<EditorGroupV6, IEditorGroup>
    {
        public string Name { get; set; }
        public HashSet<IEditorNode> Nodes { get; set; }
        public Vector2 Position { get; set; }
    }
}