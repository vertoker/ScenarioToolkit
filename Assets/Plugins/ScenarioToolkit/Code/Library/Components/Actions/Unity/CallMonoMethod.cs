using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared.Attributes;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Base.Components.Actions
{
    [ScenarioMeta("Вызывает метод в MonoBehavior", typeof(CallStaticMethod))]
    public struct CallMonoMethod : IScenarioAction, IComponentDefaultValues
    {
        public MonoBehaviour MonoBehaviour;
        [ScenarioMeta("Имя метода, должен быть пустой void()")]
        public string MethodName;
        
        public void SetDefault()
        {
            MonoBehaviour = null;
            MethodName = null;
        }
    }
}