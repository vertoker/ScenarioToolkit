using System;
using Scenario.Core.Model.Interfaces;

namespace Scenario.Editor.Content.Nodes
{
    public class ConditionNodeContent : ComponentsNodeContent
    {
        public override string TypeName => "Condition";
        public override string USSClass => "condition";
        public override Type NodeType => IConditionNode.GetModelType;
        public override Type InterfaceNodeType => typeof(IConditionNode);
        public override Type CompatibilityInterfaceNodeType => typeof(IScenarioCompatibilityConditionNode);

        public override IScenarioNode CreateDefault() => IConditionNode.CreateNew();
    }
}