using System.Collections.Generic;
using Newtonsoft.Json;
using ZLinq.Linq;
using ZLinq;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    /// <summary>
    /// Абстракция над обычными нодами для тех нод, которые имеют в себе список компонентов
    /// </summary>
    /// <typeparam name="TComponent">Тип компонента</typeparam>
    [JsonObject(IsReference = true)]
    public interface IScenarioNodeComponents<TComponent> : IScenarioNodeComponents where TComponent : IScenarioComponent
    {
        public List<TComponent> Components { get; set; }
        public ValueEnumerable<FromList<TComponent>, TComponent> ComponentsAVE { get; }
        public ValueEnumerable<FromEnumerable<TComponent>, TComponent> ComponentsEnumerableAVE { get; }
    }

    /// <summary>
    /// Абстракция над обычными нодами для тех нод, которые имеют в себе список компонентов
    /// </summary>
    [JsonObject(IsReference = true)]
    public interface IScenarioNodeComponents : IScenarioNodeFlow, IReadOnlyCollection<IScenarioComponent>
    {
        
    }
}