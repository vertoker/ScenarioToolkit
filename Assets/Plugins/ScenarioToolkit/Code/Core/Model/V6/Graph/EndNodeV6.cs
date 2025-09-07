using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Player;

// Previous: EndNodeV5
//  Current: EndNodeV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class EndNodeV6 : ScenarioNodeFlowV6, IEndNode
    {
        public bool InstantEnd { get; set; } = false;
        
        public override void Activate(NodeExecutionContext context)
        {
            base.Activate(context);
            
            if (InstantEnd) // already host check
                context.Player?.Stop();
        }
    }
}