using System;
using Scenario.Core.DataSource;

namespace ScenarioToolkit.Core.DataSource
{
    public class ScenarioLaunchModelCmdParser// : IModelCmdParser
    {
        public ScenarioMode ScenarioMode { get; private set; }
        public string Scenario { get; private set; }
        
        /*public bool CanParseModel(CommandLineParser parser)
        {
            return parser.ContainsAny("--scenario", "-s");
        }
        public object ParseModel(CommandLineParser parser)
        {
            var config = new ScenarioLaunchModel();
            if (parser.GetDataByAnyTag(out var scenario, "--scenario", "-s"))
                config.Scenario = scenario;
            if (parser.GetDataByAnyTag(out var scenarioNetwork, "--scenario-network", "-sn"))
                if (bool.TryParse(scenarioNetwork, out var useNet))
                    config.UseNetwork = useNet;
            if (parser.GetDataByAnyTag(out var scenarioLog, "--scenario-log", "-sl"))
                if (bool.TryParse(scenarioLog, out var useLog))
                    config.UseLog = useLog;
            
            // TODO дерьмо, надо переписать
            if (parser.GetDataByAnyTag(out var scenarioIdentity, "--scenario-identity", "-si"))
                if (int.TryParse(scenarioIdentity, out var identityHash))
                    config.IdentityHash = identityHash;
            
            return config;
        }*/

        public Type GetModelType() => typeof(ScenarioLaunchModel);
    }
}