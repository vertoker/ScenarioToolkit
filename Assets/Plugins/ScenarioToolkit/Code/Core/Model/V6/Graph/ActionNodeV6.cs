using System;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Shared;
using Zenject;
using ZLinq;

// Previous: ActionNodeV1
//  Current: ActionNodeV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class ActionNodeV6 : ScenarioNodeComponentsV6<IScenarioAction>, IActionNode
    {
        public event Action<IScenarioAction, int> ActionBeforeFire;
        public event Action<IScenarioAction, int> ActionAfterFire;
        
        public override void Activate(NodeExecutionContext context)
        {
            base.Activate(context);
            
            var isHost = context.IsHost;
            if (!isHost && ComponentsAVE.Contains(UseOnlyHost.Self)) return;
            
            context.RoleFilter.Process(this, context.Graph);
            if (!context.RoleFilter.CanBeExecuted(this, context.IdentityHash)) return;
            
            // Фильтр происходит до IsValid, так как нужно сохранять все компоненты
            // для работы переменных, так как они адресуются к компонентам по индексу
            var variableComponents = context.Variables.ProcessAVE(this);

            var counter = -1;
            using var enumerator = variableComponents.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ++counter;
                var action = enumerator.Current;
                
                if (!action.IsValid(isHost)) continue;

                if (action is IScenarioActionContext actionContext)
                {
                    var actionContextData = actionContext.GetRequestData();
                    actionContextData.Construct(context);
                    Fire(context.Bus, actionContextData, counter);
                    continue;
                }

                Fire(context.Bus, action, counter);
            }
        }

        private void Fire(SignalBus bus, IScenarioAction action, int counter)
        {
            ActionBeforeFire?.Invoke(action, counter);
            bus.Fire((object)action);
            ActionAfterFire?.Invoke(action, counter);
        }
    }
}