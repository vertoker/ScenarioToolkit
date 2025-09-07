using System;
using Scenario.Base.Components.Actions;
using Scenario.Core.Systems;
using UnityEngine;
using Zenject;

using LogAction = Scenario.Base.Components.Actions.Log;

namespace Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class LogSystem : BaseScenarioSystem
    {
        public LogSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<LogAction>(LogAction);
        }
        
        private static void LogAction(LogAction component)
        {
            if (string.IsNullOrWhiteSpace(component.Message)) return;
            
            const string prefix = "<b>Scenario Log</b>: ";
            switch (component.Type)
            {
                case ScenarioLogType.Log:        Debug.Log(prefix + component.Message);        break;
                case ScenarioLogType.LogWarning: Debug.LogWarning(prefix + component.Message); break;
                case ScenarioLogType.LogError:   Debug.LogError(prefix + component.Message);   break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}