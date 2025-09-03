using System;
using Scenario.Core.Model.Interfaces;

// Previous: Link
//  Current: ScenarioLinkV1
//     Next: ScenarioFlowLinkV6

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioLinkV1 : IScenarioCompatibilityLink
    {
        public ScenarioNodeV1 From { get; set; }
        public ScenarioNodeV1 To { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not ScenarioLinkV1 link)
                return false;
            return link.To == To && link.From == From;
        }

        public override int GetHashCode() => HashCode.Combine(From, To);
    }
}