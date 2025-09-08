using JetBrains.Annotations;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Bus;
using ScenarioToolkit.Core.Player.Roles;
using ScenarioToolkit.Core.Serialization;
using Zenject;

namespace ScenarioToolkit.Core.Player
{
    /// <summary>
    /// Дополнительные данные для исполнения нод
    /// </summary>
    public class NodeExecutionContext
    {
        // Root
        public int IdentityHash;
        public ScenarioComponentBus Bus;
        public NodeVariablesContext Variables;
        public ScenarioLoadService LoadService;
        public RoleFilterService RoleFilter;
        
        public NodeExecutionContext Parent;
        
        [CanBeNull] public IScenarioGraph Graph;
        
        // Host
        [CanBeNull] public ScenarioPlayer Player;
        public bool IsHost => Player != null;
        
        // Root - это корень всего дерева контекстов, от которого наследуются уже все остальные.
        // Сам по себе он никогда не используется и служит только как нулевой родитель для первого настоящего контекста
        public static NodeExecutionContext CreateRoot(ScenarioComponentBus bus, ScenarioLoadService loadService, 
            RoleFilterService filterService)
        {
            return new NodeExecutionContext(null)
            {
                Bus = bus,
                LoadService = loadService,
                RoleFilter = filterService,
                Variables = new NodeVariablesContext(),
            };
        }
        // Это костыль, но по факту это часть инициализации Root контекста
        public void UpdateIdentityHash(int identityHash)
        {
            IdentityHash = identityHash;
        }

        private NodeExecutionContext(NodeExecutionContext parent)
        {
            Parent = parent;
            IdentityHash = parent?.IdentityHash ?? 0;
        }
        
        // Subcontext - это контекст, который имеет родительский контекст. Даже контекст
        // корневого ScenarioPlayer является родителем Root контекста
        
        public NodeExecutionContext CreateSubcontextHost(ScenarioPlayer player)
        {
            return new NodeExecutionContext(this)
            {
                Bus = Bus,
                LoadService = LoadService,
                RoleFilter = RoleFilter,
                Variables = new NodeVariablesContext(player.GraphContext),
                
                Player = player,
                Graph = player.Graph
            };
        }
        public NodeExecutionContext CreateSubcontextClient(IScenarioModel model)
        {
            return new NodeExecutionContext(this)
            {
                Bus = Bus,
                LoadService = LoadService,
                RoleFilter = RoleFilter,
                Variables = new NodeVariablesContext(model.Context),
                
                Player = null,
                Graph = model.Graph
            };
        }
        public NodeExecutionContext ClearToRoot()
        {
            return new NodeExecutionContext(null)
            {
                Bus = Bus,
                LoadService = LoadService,
                RoleFilter = RoleFilter,
                Variables = new NodeVariablesContext(),
                
                Player = null,
                Graph = null,
            };
        }
    }
}