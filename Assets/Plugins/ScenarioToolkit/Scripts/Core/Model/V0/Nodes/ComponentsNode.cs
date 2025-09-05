using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: ComponentsNode
//     Next: ComponentsNodeV1

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
    public abstract class ComponentsNode : ScenarioNode, IEnumerable<IScenarioComponent>
    {
        public abstract IEnumerator<IScenarioComponent> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    
    public abstract class ComponentsNode<T>: ComponentsNode where T : IScenarioComponent
    {
        public List<T> Components { get; set; } = new();
        
        public override IEnumerator<IScenarioComponent> GetEnumerator() => Components
            .Cast<IScenarioComponent>().GetEnumerator();
    }
}