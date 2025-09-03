using System;
using UnityEngine;

namespace Scenario.Editor.SRF.Tests
{
    public class TestSRF2Array : IScenarioReflexFunction
    {
        public SRFMetadata GetMetadata()
        {
            return new SRFMetadata
            {
                FunctionName = nameof(TestSRF2Array),
                FunctionTooltip = "Hi 2",
                Disabled = true,
            };
        }
        public void BuildUI(SRFContext context)
        {
            context.CreateParameter<int>("count", 2, "Размер массива", OnValueInitialized, OnValueChanged);
        }
        private void OnValueInitialized(SRFContext context, string name, int current)
        {
            for (var i = 1; i <= current; i++)
                context.CreateParameter<string>($"obj{i}", Convert.ToString(i, 2));
        }
        private void OnValueChanged(SRFContext context, string name, int prevValue, int nextValue)
        {
            for (var i = 1; i <= prevValue; i++)
                context.RemoveParameter($"obj{i}");
            for (var i = 1; i <= nextValue; i++)
                context.CreateParameter<string>($"obj{i}", Convert.ToString(i, 2));
        }
        
        public void Execute(SRFContext context)
        {
            var count = context.GetParameterValue<int>("count");
            var strings = new string[count];
            for (var i = 0; i < count; i++)
                strings[i] = context.GetParameterValue<string>($"obj{i+1}");
            
            for (var i = 0; i < count; i++)
                Debug.Log(strings[i]);
            // next do stuff
        }
    }
}