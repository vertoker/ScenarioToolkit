using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;

// Previous: 
//  Current: Link
//     Next: ScenarioLinkV1

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public class Link
    {
        public ScenarioNode From;
        public ScenarioNode To;
        
        public ScenarioLinkV1 ConvertV1(Dictionary<ScenarioNode, ScenarioNodeV1> nodeDict)
        {
            return new ScenarioLinkV1
            {
                From = nodeDict[From],
                To = nodeDict[To],
            };
        }
    }
}