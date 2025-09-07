using System;

namespace ScenarioToolkit.Shared.Attributes
{
    // TODO добавить поддержку этого аттрибута на все объекты, типы которых сериализуются через Converter
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TypeSerializedAsAttribute : Attribute
    {
        public string QualifiedName { get; }

        public TypeSerializedAsAttribute(string typeNamespace, string className, string assemblyName)
        {
            QualifiedName = $"{typeNamespace}.{className}, {assemblyName}";
        }
    }
}