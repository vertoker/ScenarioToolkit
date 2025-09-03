using System;
using Scenario.Core.Model.Interfaces;

// Previous: EditorLinkV3
//  Current: EditorLinkV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class EditorLinkV6 : IEditorLink
    {
        public IEditorNode From { get; set; }
        public IEditorNode To { get; set; }
        
        public int Hash => IHashable.Combine(From, To);
    }
}