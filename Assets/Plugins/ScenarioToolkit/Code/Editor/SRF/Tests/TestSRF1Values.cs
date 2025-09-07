using Scenario.Core.Model;
using UnityEngine;

namespace ScenarioToolkit.Editor.SRF.Tests
{
    public class TestSRF1Values : IScenarioReflexFunction
    {
        public SRFMetadata GetMetadata()
        {
            return new SRFMetadata
            {
                FunctionName = nameof(TestSRF1Values),
                FunctionTooltip = "Hi 1",
                Disabled = true,
            };
        }
        public void BuildUI(SRFContext context)
        {
            context.CreateParameter<string>("str", "what");
            context.CreateParameter<int>("num", 2);
            context.CreateParameter<Object>("obj", null);
            context.CreateParameter<NodeRef>("ref");
            context.CreateParameter<Quaternion>("quat");
        }
        public void Execute(SRFContext context)
        {
            var str = context.GetParameterValue<string>("str");
            var num = context.GetParameterValue<int>("num");
            var obj = context.GetParameterValue<Object>("obj");
            var rf = context.GetParameterValue<NodeRef>("ref");
            var quat = context.GetParameterValue<Quaternion>("quat");
            Debug.Log(str);
            Debug.Log(num);
            Debug.Log(obj);
            Debug.Log(rf.Hash);
            Debug.Log(quat);
        }
    }
}