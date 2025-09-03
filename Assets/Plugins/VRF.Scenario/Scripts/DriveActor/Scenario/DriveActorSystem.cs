using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DriveActor.Scenario.Actions;
using DriveActor.Scenario.Conditions;
using Scenario.Core;
using Scenario.Core.Systems;
using Scenario.Utilities;
using SimpleUI.Extensions;
using UnityEngine;
using VRF.Scenario.DriveActor.Core;
using Zenject;

namespace VRF.Scenario.DriveActor.Scenario
{
    public class DriveActorSystem : BaseScenarioSystem
    {
        private readonly Dictionary<BaseDriveActor, Action> actorConditions = new();
        
        public DriveActorSystem(BaseDriveActor[] actors, SignalBus bus) : base(bus)
        {
            bus.Subscribe<DriveActorEasing>(DriveActorEasing);
            bus.Subscribe<WaitDriveActorTrigger>(WaitDriveActorTrigger);

            foreach (var actor in actors)
                actor.ValueUpdated += DriveActorUpdated;
        }

        private void WaitDriveActorTrigger(WaitDriveActorTrigger component)
        {
            if (AssertLog.NotNull<WaitDriveActorTrigger>(component.Actor, nameof(component.Actor))) return;
            TryAddActorAwait(component);
        }
        private void TryAddActorAwait(WaitDriveActorTrigger component)
        {
            if (actorConditions.ContainsKey(component.Actor))
            {
                Debug.LogWarning("Can't add another actor, it already in system", component.Actor.gameObject);
                return;
            }
            
            actorConditions.Add(component.Actor, Action);
            return;
            
            void Action() => CheckCondition(component);
        }
        private void TryRemoveActorAwait(BaseDriveActor actor)
        {
            if (!actorConditions.Remove(actor))
                Debug.LogWarning($"Can't remove actor, it doesn't in system, drop", actor.gameObject);
        }
        
        private void DriveActorEasing(DriveActorEasing component)
        {
            if (AssertLog.NotNull<DriveActorEasing>(component.Actor, nameof(component.Actor))) return;
            if (AssertLog.Above<DriveActorEasing>(component.Time, 0, nameof(component.Time))) return;
            
            DriveActorEasingTask(component);
        }
        private async void DriveActorEasingTask(DriveActorEasing component)
        {
            var startValue = component.Actor.Value;
            component.Actor.SetValue(startValue);
            //SetValue(component.Actor, startValue);

            var invertTime = 1 / component.Time;
            for (var i = 0f; i <= 1f; i += Time.deltaTime * invertTime)
            {
                await UniTask.Yield();
                var value = Easings.GetEasing(i, component.Type);
                value = Mathf.LerpUnclamped(startValue, component.Value, value);
                component.Actor.SetValue(value);
                //SetValue(component.Actor, value);
            }
            
            component.Actor.SetValue(component.Value);
            //SetValue(component.Actor, component.Value);
            TryRemoveActorAwait(component.Actor);
        }
        
        private void DriveActorUpdated(BaseDriveActor actor, float value)
        {
            if (actorConditions.TryGetValue(actor, out var action))
                action();
        }
        private void CheckCondition(WaitDriveActorTrigger component)
        {
            switch (component.ConditionType)
            {
                case ValueTriggerType.Equal:
                    if (Mathf.Abs(component.Actor.Value - component.Value) < BaseDriveActor.EpsilonEqual) 
                        Fire(component.Actor);
                    return;
                
                case ValueTriggerType.Above:
                    if (component.Actor.Value > component.Value)
                        Fire(component.Actor);
                    return;
                
                case ValueTriggerType.Below:
                    if (component.Actor.Value < component.Value)
                        Fire(component.Actor);
                    return;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void Fire(BaseDriveActor actor)
        {
            var component = new DriveActorTriggered { Actor = actor };
            TryRemoveActorAwait(actor);
            Bus.Fire(component);
        }
    }
}