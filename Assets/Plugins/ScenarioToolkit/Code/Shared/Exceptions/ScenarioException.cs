using System;

namespace ScenarioToolkit.Shared.Exceptions
{
    public class ScenarioException : Exception
    {
        public ScenarioException(string message) : base(message)
        {
            
        }
        public ScenarioException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}