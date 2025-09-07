using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Вызывает статический метод в статическом классе", typeof(CallMonoMethod))]
    public struct CallStaticMethod : IScenarioAction, IComponentDefaultValues
    {
        [ScenarioMeta("Статический класс, принимает только имя (поиск не гарантирован)")]
        public string ClassName;
        [ScenarioMeta("Имя метода, должен быть пустой void()")]
        public string MethodName;

        public void SetDefault()
        {
            ClassName = null;
            MethodName = null;
        }
    }
}