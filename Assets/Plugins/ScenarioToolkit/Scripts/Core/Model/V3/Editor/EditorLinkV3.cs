using System;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;

// Previous: EditorLinkV1
//  Current: EditorLinkV3
//     Next: EditorLinkV6

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class EditorLinkV3
    {
        public EditorNodeV3 From { get; set; }
        public EditorNodeV3 To { get; set; }
    }
}