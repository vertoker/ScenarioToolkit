using Scenario.Core.Model.Interfaces;
using VRF.Scenario.DriveActor.Core;
using VRF.Scenario.DriveActor.Scenario;

// ReSharper disable once CheckNamespace
namespace DriveActor.Scenario.Actions
{
    /// <summary> Создаёт условия для Actor если его value будет соблюдать условие </summary>
    public struct WaitDriveActorTrigger : IScenarioAction
    {
        public BaseDriveActor Actor;
        public ValueTriggerType ConditionType;
        public float Value;
    }
}