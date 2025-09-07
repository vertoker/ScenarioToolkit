using Scenario.Base.Components.Actions;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Core.Systems.States;
using ScenarioToolkit.Library.States;
using ScenarioToolkit.Shared;
using ScenarioToolkit.Shared.Extensions;
using UnityEngine;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class SetTransformSystem : BaseScenarioStateSystem<SetTransformState>
    {
        public SetTransformSystem(SignalBus bus) : base(bus)
        {
            Bus.Subscribe<SetTransform>(SetTransform);
            Bus.Subscribe<SetPosition>(SetPosition);
            Bus.Subscribe<SetEuler>(SetEuler);
            Bus.Subscribe<SetQuaternion>(SetQuaternion);
            Bus.Subscribe<SetScale>(SetScale);
            
            Bus.Subscribe<AddPosition>(AddPosition);
            Bus.Subscribe<AddEuler>(AddEuler);
            Bus.Subscribe<MulQuaternion>(MulQuaternion);
            Bus.Subscribe<MulScale>(MulScale);
        }

        public override void SetState(IState state) { ApplyState(State); }
        protected override void ApplyState(SetTransformState state)
        {
            foreach (var posPair in state.Positions)
            {
                var pos = posPair.Value.GetPosition(posPair.Key);
                SetPosition(pos);
            }
            foreach (var eulPair in state.Eulers)
            {
                var eul = eulPair.Value.GetEuler(eulPair.Key);
                SetEuler(eul);
            }
            foreach (var quaPair in state.Quaternions)
            {
                var qua = quaPair.Value.GetQuaternion(quaPair.Key);
                SetQuaternion(qua);
            }
            foreach (var scaPair in state.Scales)
            {
                var sca = scaPair.Value.GetScale(scaPair.Key);
                SetScale(sca);
            }
        }
        
        private void SetTransform(SetTransform component)
        {
            SetPosition(component.GetPosition());
            SetEuler(component.GetEuler());
            SetScale(component.GetScale());
        }
        private void SetPosition(SetPosition component)
        {
            if (AssertLog.NotNull<SetTransform>(component.Transform, nameof(component.Transform))) return; 
            
            State.Positions[component.Transform] = new SetTransformState.PosData(component);

            if (component.Local)
                component.Transform.localPosition = component.Position;
            else component.Transform.position = component.Position;
        }
        private void SetEuler(SetEuler component)
        {
            if (AssertLog.NotNull<SetEuler>(component.Transform, nameof(component.Transform))) return;

            State.Eulers[component.Transform] = new SetTransformState.EulData(component);

            if (component.Local)
                component.Transform.localEulerAngles = component.Euler;
            else component.Transform.eulerAngles = component.Euler;
        }
        private void SetQuaternion(SetQuaternion component)
        {
            if (AssertLog.NotNull<SetQuaternion>(component.Transform, nameof(component.Transform))) return;

            State.Quaternions[component.Transform] = new SetTransformState.QuaData(component);

            if (component.Local)
                component.Transform.localRotation = component.Quaternion;
            else component.Transform.rotation = component.Quaternion;
        }
        private void SetScale(SetScale component)
        {
            if (AssertLog.NotNull<SetScale>(component.Transform, nameof(component.Transform))) return;

            State.Scales[component.Transform] = new SetTransformState.ScaData(component);

            if (component.Local)
                component.Transform.localScale = component.Scale;
            else component.Transform.SetLossyScale(component.Scale);
        }
        
        
        private void AddPosition(AddPosition component)
        {
            if (AssertLog.NotNull<AddPosition>(component.Transform, nameof(component.Transform))) return;
            if (component.Position == Vector3.zero) return;
            
            State.Positions[component.Transform] = new SetTransformState.PosData(component);
            
            if (component.Local)
                component.Transform.localPosition += component.Position;
            else component.Transform.position += component.Position;
        }
        private void AddEuler(AddEuler component)
        {
            if (AssertLog.NotNull<AddEuler>(component.Transform, nameof(component.Transform))) return;
            if (component.Euler == Vector3.zero) return;
            
            State.Eulers[component.Transform] = new SetTransformState.EulData(component);
            
            if (component.Local)
                component.Transform.localEulerAngles += component.Euler;
            else component.Transform.eulerAngles += component.Euler;
        }
        private void MulQuaternion(MulQuaternion component)
        {
            if (AssertLog.NotNull<MulQuaternion>(component.Transform, nameof(component.Transform))) return;
            if (component.Quaternion == Quaternion.identity) return;
            
            State.Quaternions[component.Transform] = new SetTransformState.QuaData(component);
            
            if (component.Local)
                component.Transform.localRotation *= component.Quaternion;
            else component.Transform.rotation *= component.Quaternion;
        }
        private void MulScale(MulScale component)
        {
            if (AssertLog.NotNull<MulScale>(component.Transform, nameof(component.Transform))) return;
            if (component.Scale == Vector3.one) return;
            
            State.Scales[component.Transform] = new SetTransformState.ScaData(component);
            
            if (component.Local)
                component.Transform.localScale = Vector3.Scale(component.Transform.localScale, component.Scale);
            else component.Transform.SetLossyScale(Vector3.Scale(component.Transform.lossyScale, component.Scale));
        }
    }
}