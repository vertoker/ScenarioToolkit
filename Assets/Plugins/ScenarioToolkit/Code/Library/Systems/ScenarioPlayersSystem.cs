using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using Zenject;
using ZLinq;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class ScenarioPlayersSystem : BaseScenarioSystem
    {
        public ScenarioPlayersSystem(SignalBus bus) : base(bus)
        {
            bus.Subscribe<ListenNodesContext>(ListenNodesContext);
            bus.Subscribe<SetNodeActivityContext>(SetNodeActivityContext);
            bus.Subscribe<SetNodeNextProcessContext>(SetNodeNextProcessContext);
        }

        private void ListenNodesContext(ListenNodesContext component)
        {
            component.Player.NodeAfterActivated += NodeActivated;
            component.Player.NodeAfterCompleted += NodeCompleted;
        }
        private void SetNodeActivityContext(SetNodeActivityContext component)
        {
            if (AssertLog.IsFalse(component.Source.Node.Hash == 0, "NodeRef is empty")) return;
            var node = component.Player.Graph.GetFlowNode(component.Source.Node.Hash);
            if (AssertLog.NotNull<ScenarioPlayersSystem>(node, nameof(node))) return;

            if (component.Source.SetIf != SetNodeActivityIf.NotSpecified)
            {
                var isActive = component.Player.ActiveNodesAVE.Contains(node);
                if (isActive && component.Source.SetIf == SetNodeActivityIf.OnlyIfDisabled) return;
                if (!isActive && component.Source.SetIf == SetNodeActivityIf.OnlyIfEnabled) return;
            }
            
            if (component.Source.Active)
                component.Player.Activate(node);
            else component.Player.Deactivate(node);
        }
        private void SetNodeNextProcessContext(SetNodeNextProcessContext component)
        {
            if (AssertLog.IsFalse(component.Source.Node.Hash == 0, "NodeRef is empty")) return;
            var node = component.Player.Graph.GetFlowNode(component.Source.Node.Hash);
            if (AssertLog.NotNull<ScenarioPlayersSystem>(node, nameof(node))) return;
            
            node.SetAllowNextProcess(component.Source.NextProcess);
        }
        
        private void NodeActivated(IScenarioNode node)
        {
            Bus.Fire(new NodeActivated { Node = new NodeRef(node) });
        }
        private void NodeCompleted(IScenarioNode node)
        {
            Bus.Fire(new NodeDeactivated { Node = new NodeRef(node) });
        }
    }
}