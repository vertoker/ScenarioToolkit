using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model.Interfaces;
using UnityEngine;

// Previous: ScenarioGraphV1
//  Current: ScenarioGraphV5
//     Next: ScenarioGraphV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class ScenarioGraphV5
    {
        public HashSet<ScenarioNodeV1> Nodes { get; set; } = new();
        public HashSet<ScenarioLinkV1> Links { get; set; } = new();
    }
}