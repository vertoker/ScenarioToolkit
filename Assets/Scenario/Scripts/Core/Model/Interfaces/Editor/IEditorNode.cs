using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Дополнительные данные для редактора от IScenarioNode в обычном графе
    /// </summary>
    public interface IEditorNode : IHashable, IModelReflection<EditorNodeV6, IEditorNode>
    {
        public IScenarioNode Node { get; set; }
        // Позиция верхнего левого угла, это якорь ноды
        public Vector2 Position { get; set; }
        
        public HashSet<int> IncomingLinks { get; set; }
        public HashSet<int> OutcomingLinks { get; set; }
        public HashSet<int> Groups { get; set; }
        
        public void ClearAll();
        
        // Размер тут не указывается, так как он создаётся
        // динамически в редакторе, исходя из стилей
    }
}