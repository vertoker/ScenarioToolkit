using Newtonsoft.Json;
using Zenject;

namespace Scenario.Core.Serialization.Converters.Base
{
    public interface IScenarioJsonConverter
    {
        public ScenarioSerializationSettings Settings { get; }
        
        public void Set(ScenarioSerializationSettings settings);
    }
}