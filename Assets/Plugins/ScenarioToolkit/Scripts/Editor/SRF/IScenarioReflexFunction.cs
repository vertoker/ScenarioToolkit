using UnityEngine.UIElements;

namespace Scenario.Editor.SRF
{
    public interface IScenarioReflexFunction
    {
        public SRFMetadata GetMetadata() { return new SRFMetadata($"Func {GetType().Name}"); }
        public void BuildUI(SRFContext context);
        public void Execute(SRFContext context);
    }
}