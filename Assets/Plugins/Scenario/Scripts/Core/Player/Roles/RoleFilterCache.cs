using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;
using VRF.Players.Scriptables;

namespace Scenario.Core.Player.Roles
{
    public class RoleFilterCache
    {
        private readonly HashSet<int> includeHashes = new();
        private readonly HashSet<int> excludeHashes = new();
        private readonly List<IScenarioNodeFlow> nodes = new();
        
        public IReadOnlyList<IScenarioNodeFlow> Nodes => nodes;

        public void AddInclude(IScenarioNodeFlow node, PlayerIdentityConfig identity)
        {
            includeHashes.Add(identity.AssetHashCode);
            AddNode(node);
        }
        public void AddExclude(IScenarioNodeFlow node, PlayerIdentityConfig identity)
        {
            excludeHashes.Add(identity.AssetHashCode);
            AddNode(node);
        }
        public void AddNode(IScenarioNodeFlow node)
        {
            nodes.Add(node);
        }
        
        public void Reset()
        {
            includeHashes.Clear();
            excludeHashes.Clear();
            nodes.Clear();
        }
        
        public bool CanExecute(int identityHash)
        {
            // Изначально, если include пустой, то он не участвует в фильтрации
            if (includeHashes.Count == 0)
            {
                // Оптимизация для пустых фильтров
                if (excludeHashes.Count == 0) return true;
                
                // Разрешение полностью зависит от того, есть ли он в exclude списке
                return !excludeHashes.Contains(identityHash);
            }

            // identity должен быть в include и не должен быть в exclude
            return includeHashes.Contains(identityHash) && !excludeHashes.Contains(identityHash);
        }
    }
}