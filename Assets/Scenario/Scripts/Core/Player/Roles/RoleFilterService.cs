using System.Collections.Generic;
using System.Linq;
using Mirror;
using Scenario.Core.Model;
using Scenario.Core.Model.Interfaces;
using Scenario.Core.Network;
using UnityEngine;
using VRF.Networking.Core;
using VRF.Players.Scriptables;
using VRF.Utils.Pool;
using Zenject;
using ZLinq;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Scenario.Core.Player.Roles
{
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    /// <summary>
    /// Универсальный сервис для обработки ролей в сценариях.
    /// Индивидуален для сервера и клиента. Работает как локально, так и в сети
    /// </summary>
    public class RoleFilterService
    {
        // Пул кэшов для оптимизации и reusable классов
        private readonly ClassPool<RoleFilterCache> filterCachePool = new();
        
        // Глобальный кэш, сюда записывают обычные NetFilter компоненты
        private readonly Dictionary<IScenarioNodeFlow, RoleFilterCache> filterCache = new();
        // Локальный кэш, контекст одной ноды, сюда записывают NetNodeFilter компоненты
        private readonly RoleFilterCache filterNodeCache = new();
        
        public void Process(IScenarioNodeFlow flowNode, IScenarioGraph graph)
        {
            if (flowNode is not IScenarioNodeComponents componentsNode) return;
            
            // Фильтр для отсутствия повторений
            if (filterCache.ContainsKey(flowNode))
            {
                Debug.LogError($"Repeated node: {flowNode.GetStatusString()} (reset filters in scenario)");
                return;
            }
            
            // Очищаем локальный кэш
            filterNodeCache.Reset();
            ProcessPreviousNodes(flowNode, graph);
            
            foreach (var component in componentsNode)
                Process(component, flowNode);
        }
        
        public bool CanBeExecuted(IScenarioNodeFlow flowNode, int identityHash)
        {
            // Если роли нет, то функция отключена и ноду можно исполнить
            if (identityHash == 0) return true;
            
            // Если в глобальном кэше нет ноды
            if (!filterCache.TryGetValue(flowNode, out var cache))
                // То надо проверить только локальный кэш ноды
                return filterNodeCache.CanExecute(identityHash);
            // Иначе нужно делать полную проверку глобального и локального кэша
            return filterNodeCache.CanExecute(identityHash)
                   && cache.CanExecute(identityHash);
        }
        
        // Обрабатывает все предыдущие ноды от ноды на то, есть ли они уже в кэше
        // и делает продолжение от найденных наследников если нашёл предка в кэше
        private void ProcessPreviousNodes(IScenarioNodeFlow flowNode, IScenarioGraph graph)
        {
            // Оптимизация если нод с ролями нет вообще
            if (filterCache.Count == 0) return;
            
            var added = false;
            // Проходит по всем нодам, которые ведут к ней
            foreach (var prevNode in graph.GetIncomingNodesAVE(flowNode))
            {
                // Проверяет наличие в кэше предка от ноды
                if (filterCache.TryGetValue(prevNode, out var cache))
                {
                    // Надо добавить только одного наследника от ноды
                    if (!added)
                    {
                        filterCache.Add(flowNode, cache);
                        cache.AddNode(flowNode);
                        added = true;
                    }
                    // Иначе принудительно вызывает Reset для ноды предка,
                    // так как произошла коллизия и сессия фильтрации не была закрыта
                    else
                    {
                        var cacheReset = TryAddAndGet(prevNode);
                        ReleaseAndRemoveNodes(cacheReset);
                    }
                }
            }
        }

        private void Process(IScenarioComponent component, IScenarioNodeFlow flowNode)
        {
            switch (component)
            {
                case RoleFilter netInclude:
                    if (!netInclude.Identity) break;
                    var cacheInclude = TryAddAndGet(flowNode);
                    cacheInclude.AddInclude(flowNode, netInclude.Identity);
                    break;
                case RoleFilterExclude netExclude:
                    if (!netExclude.Identity) break;
                    var cacheExclude = TryAddAndGet(flowNode);
                    cacheExclude.AddExclude(flowNode, netExclude.Identity);
                    break;
                
                case RoleNodeFilter netNodeInclude:
                    if (!netNodeInclude.Identity) break;
                    filterNodeCache.AddInclude(flowNode, netNodeInclude.Identity);
                    break;
                case RoleNodeFilterExclude netNodeExclude:
                    if (!netNodeExclude.Identity) break;
                    filterNodeCache.AddExclude(flowNode, netNodeExclude.Identity);
                    break;
                
                case RoleFilterReset:
                    var cacheReset = TryAddAndGet(flowNode);
                    ReleaseAndRemoveNodes(cacheReset);
                    break;
            }
        }
        
        private RoleFilterCache TryAddAndGet(IScenarioNodeFlow flowNode)
        {
            if (filterCache.TryGetValue(flowNode, out var cache))
                return cache;

            cache = filterCachePool.Get();
            filterCache.Add(flowNode, cache);
            return cache;
        }
        private void ReleaseAndRemoveNodes(RoleFilterCache cache)
        {
            foreach (var node in cache.Nodes)
                filterCache.Remove(node);
            cache.Reset();
            filterCachePool.Release(cache);
        }
    }
}