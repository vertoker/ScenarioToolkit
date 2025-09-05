using System;
using Scenario.Core.Model.Interfaces;

namespace Scenario.Editor.Content.Nodes
{
    public class ActionNodeContent : ComponentsNodeContent
    {
        public override string TypeName => "Action";
        public override string USSClass => "action";
        public override Type NodeType => IActionNode.GetModelType;
        public override Type InterfaceNodeType => typeof(IActionNode);
        public override Type CompatibilityInterfaceNodeType => typeof(IScenarioCompatibilityActionNode);
        
        public override IScenarioNode CreateDefault() => IActionNode.CreateNew();
    }
}