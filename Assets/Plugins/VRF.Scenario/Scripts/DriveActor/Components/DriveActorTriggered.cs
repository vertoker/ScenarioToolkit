using Scenario.Core.Model.Interfaces;
using VRF.Scenario.DriveActor.Core;

// ReSharper disable once CheckNamespace
namespace DriveActor.Scenario.Conditions
{
    /// <summary> Actor (его value) выполнил условие </summary>
    public struct DriveActorTriggered : IScenarioCondition
    {
        public BaseDriveActor Actor;
    }
}