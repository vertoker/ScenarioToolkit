using ScenarioToolkit.Bus;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using VRF.Scenario.Components.Actions;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    /// <summary>
    /// State система для контроля активности MonoBehaviour скриптов
    /// </summary>
    public class MonoBehaviourActivitySystem : BaseScenarioSystem
    {
        public MonoBehaviourActivitySystem(ScenarioComponentBus listener) : base(listener)
        {
            Bus.Subscribe<SetMonoBehaviourActivity>(SetMonoBehaviourActivity);
        }

        private void SetMonoBehaviourActivity(SetMonoBehaviourActivity component)
        {
            if (AssertLog.NotNull<SetMonoBehaviourActivity>(component.MonoBehaviour, nameof(component.MonoBehaviour))) return;
            
            component.MonoBehaviour.enabled = component.IsActive;
        }
        
        
        /*private void SetActivity(SetMonoBehaviourActivity setMonoBehaviourActivity)
        {
            // if (setMonoBehaviourActivity.MonoBehaviour.gameObject.TryGetComponent<Outlinable>(out var outlinable))
            // {
            //     if (IsCheckable(setMonoBehaviourActivity))
            //         outlinable.OutlineParameters.Color = Color.yellow;
            //     if (IsPhysical(setMonoBehaviourActivity))
            //         outlinable.OutlineParameters.Color = new Color(255 / 255f, 100 / 255f, 16 / 255f, 1);
            // }

            setMonoBehaviourActivity.MonoBehaviour.enabled = setMonoBehaviourActivity.IsActive;
        }

        private bool IsCheckable(SetMonoBehaviourActivity c)
        {
            if (c.MonoBehaviour.GetType() == typeof(Checkable))
                return true;
            if (c.MonoBehaviour is MultiEnabler me && me.MonoBehaviours.OfType<Checkable>().Any())
                return true;

            return false;
        }

        private bool IsPhysical(SetMonoBehaviourActivity c)
        {
            var comp = c.MonoBehaviour.GetType();
            if (comp == typeof(TriggerZone)
                || comp == typeof(ToggleSwitch))
                return true;
            if (c.MonoBehaviour is MultiEnabler me
                && (me.MonoBehaviours.OfType<TriggerZone>().Any()
                    || me.MonoBehaviours.OfType<ToggleSwitch>().Any()))
                return true;

            return false;
        }*/
    }
}