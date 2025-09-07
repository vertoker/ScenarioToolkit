using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;

// Previous: ScenarioLinkV1
//  Current: ScenarioFlowLinkV6
//     Next: 

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    [JsonObject(IsReference = true)]
    public class ScenarioLinkFlowV6 : IScenarioLinkFlow
    {
        public IScenarioNodeFlow From { get; set; }
        public IScenarioNodeFlow To { get; set; }
        public int Hash => IHashable.Combine(From, To);
    }
}