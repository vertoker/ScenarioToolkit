using Scenario.Core.Model.Interfaces;
using SimpleUI.Extensions;
using VRF.Scenario.DriveActor.Core;

// ReSharper disable once CheckNamespace
namespace DriveActor.Scenario.Actions
{
    /// <summary> Анимирует value у Actor до другого значения по времени </summary>
    public struct DriveActorEasing : IScenarioAction, IComponentDefaultValues
    {
        public BaseDriveActor Actor;
        public float Value;
        public Easings.Type Type;
        public float Time;
        
        public void SetDefault()
        {
            Actor = null;
            Value = 0;
            Type = Easings.Type.Linear;
            Time = 1;
        }
    }
}