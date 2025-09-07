using Newtonsoft.Json;

namespace ScenarioToolkit.Core.Serialization.Converters.Base
{
    public abstract class BaseScenarioJsonConverter : JsonConverter, IScenarioJsonConverter
    {
        public ScenarioSerializationSettings Settings { get; private set; }
        
        public virtual void Set(ScenarioSerializationSettings settings)
        {
            Settings = settings;
        }
    }
    
    public abstract class BaseScenarioJsonConverter<T> : JsonConverter<T>, IScenarioJsonConverter
    {
        public ScenarioSerializationSettings Settings { get; private set; }
        
        public virtual void Set(ScenarioSerializationSettings settings)
        {
            Settings = settings;
        }
    }
}