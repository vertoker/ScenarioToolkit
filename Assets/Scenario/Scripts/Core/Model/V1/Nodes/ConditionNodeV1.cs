using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Player;
using Scenario.Utilities;

// Previous: ConditionNode
//  Current: ConditionNodeV1
//     Next: ConditionNodeV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class ConditionNodeV1 : ComponentsNodeV1<IScenarioCondition>, IScenarioCompatibilityConditionNode
    {
        
    }
}