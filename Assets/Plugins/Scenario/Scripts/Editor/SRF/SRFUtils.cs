using System;
using System.Linq;
using Scenario.Utilities;

namespace Scenario.Editor.SRF
{
    public static class SRFUtils
    {
        public static readonly Type[] Types
            = Reflection.GetImplementations<IScenarioReflexFunction>().ToArray();
        public static readonly IScenarioReflexFunction[] Instances
            = Reflection.GetInstances<IScenarioReflexFunction>(Types).ToArray();
    }
}