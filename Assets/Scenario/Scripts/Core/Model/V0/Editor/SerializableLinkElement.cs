using System.Collections.Generic;
using Scenario.Core.Model;

// Previous: 
//  Current: SerializableLinkElement
//     Next: EditorLinkV1

// ReSharper disable once CheckNamespace
namespace Scenario.GraphEditor.Elements.Serialization
{
    public class SerializableLinkElement
    {
        public SerializableNodeElement From;
        public SerializableNodeElement To;

        public EditorLinkV1 Convert(Dictionary<SerializableNodeElement, EditorNodeV1> nodeConvert)
        {
            return new EditorLinkV1
            {
                From = nodeConvert[From],
                To = nodeConvert[To],
            };
        }
    }
}