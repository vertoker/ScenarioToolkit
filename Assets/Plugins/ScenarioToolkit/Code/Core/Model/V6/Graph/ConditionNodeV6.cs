using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Scenario.Core.Model.Interfaces;
using Scenario.Utilities;
using ScenarioToolkit.Core.Player;
using ScenarioToolkit.Shared;
using ScenarioToolkit.Shared.Extensions;
using ZLinq;

// Previous: ConditionNodeV1
//  Current: ConditionNodeV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class ConditionNodeV6 : ScenarioNodeComponentsV6<IScenarioCondition>, IConditionNode
    {
        public event Action NodeCompleted;
        public event Action<IScenarioCondition, int> ConditionCompleted;
        
        [JsonIgnore] private readonly List<UniTaskCompletionSource> completionSources = new();
        /// <summary> Отфильтрованные компоненты </summary>
        [JsonIgnore] private readonly List<IScenarioCondition> validatedComponents = new();
        /// <summary> Нода ждёт прихода этих компонентов </summary>
        [JsonIgnore] private readonly HashSet<IScenarioCondition> waitedComponents = new();
        [JsonIgnore] private readonly HashSet<Type> subscriptionTypes = new();
        
        public override void Activate(NodeExecutionContext context)
        {
            base.Activate(context);
            
            var isHost = context.IsHost;
            if (!isHost && ComponentsAVE.Contains(UseOnlyHost.Self)) return;
            
            context.RoleFilter.Process(this, context.Graph);
            if (!context.RoleFilter.CanBeExecuted(this, context.IdentityHash)) return;
            
            // Фильтр происходит до IsValid, так как нужно сохранять все компоненты
            // для работы переменных, так как они адресуются к компонентам по индексу
            var newNodes = context.Variables.ProcessAVE(this).Where(c => c.IsValid(isHost));
            validatedComponents.AddRange(newNodes);
            
            if (IsEmpty()) return;

            foreach (var component in validatedComponents)
                waitedComponents.Add(component); // List -> HashSet
            
            foreach (var conditionType in waitedComponents.AsValueEnumerable().Select(component => component.GetType()))
            {
                if (!subscriptionTypes.Add(conditionType)) continue;
                context.Bus.Subscribe(conditionType, OnConditionReceived);
            }

            foreach (var component in validatedComponents)
            {
                if (component is IScenarioConditionRequest request)
                {
                    var data = request.GetRequestData();
                    data.Construct(context);
                    context.Bus.Fire((object)data);
                }
            }
        }
        public override void Deactivate(NodeExecutionContext context)
        {
            foreach (var subscriptionType in subscriptionTypes)
                context.Bus.Unsubscribe(subscriptionType, OnConditionReceived);
            
            subscriptionTypes.Clear();
            validatedComponents.Clear();
            waitedComponents.Clear();
            ForceEnd();
            
            base.Deactivate(context);
        }
        
        public bool IsCompleted() => waitedComponents.Count == 0;
        public bool IsEmpty() => validatedComponents.Count == 0;
        
        public override Task WaitForCompletion()
        {
            // TODO экспериментально, МОЖЕТ вызывать досрочное завершение если есть IsScenarioRequest компонент
            if (IsCompleted()) return Task.CompletedTask; // Если задача была выполнена моментально
            
            var completionSource = new UniTaskCompletionSource();
            completionSources.Add(completionSource);
            return completionSource.Task.AsTask();
        }

        private void OnConditionReceived(object obj) => TryCompleteCondition((IScenarioCondition)obj);
        public bool TryCompleteCondition(IScenarioCondition condition)
        {
            if (IsEmpty()) return false;
            var conditionCompleted = waitedComponents.Remove(condition);
            
            if (conditionCompleted)
                ConditionCompleted?.Invoke(condition, validatedComponents.IndexOf(condition));
            if (IsCompleted()) ForceEnd();
            
            return conditionCompleted;
        }
        public void FireComponent(NodeExecutionContext context, int conditionIndex)
        {
            var component = validatedComponents[conditionIndex];
            context.Bus.Fire(component);
        }

        // when complete call completionSource.TrySetResult();
        // when cancel   call completionSource.TrySetCanceled();
        // when failed   call completionSource.TrySetException();
        public void ForceEnd()
        {
            if (IsAllowNextProcess())
                TryEnd(CompleteAction);
            else TryEnd(CancelAction);
        }
        
        private void CompleteAction(UniTaskCompletionSource source) => source.TrySetResult();
        private void FailAction(UniTaskCompletionSource source) => source.TrySetException(new Exception());
        private void CancelAction(UniTaskCompletionSource source) => source.TrySetCanceled();
        
        private void TryEnd(Action<UniTaskCompletionSource> action)
        {
            if (completionSources.Count == 0) return;

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < completionSources.Count; i++)
                action(completionSources[i]);
            
            completionSources.Clear();
            NodeCompleted?.Invoke();
        }
    }
}