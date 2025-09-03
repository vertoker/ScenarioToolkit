using System;
using Scenario.Core.Model.Interfaces;

// Previous: SerializableLinkElement
//  Current: EditorLinkV1
//     Next: EditorLinkV3

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class EditorLinkV1
    {
        public EditorNodeV1 From { get; set; }
        public EditorNodeV1 To { get; set; }
    }
}