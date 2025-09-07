using System.Collections.Generic;
using Scenario.Core.Model.Interfaces;
using ScenarioToolkit.Shared;

namespace ScenarioToolkit.Editor.Windows.ElementEditor
{
    /// <summary>
    /// Контроллер данных для перезаписей полей в компонентах. Именно тут
    /// определяется логика сохранения данных по принципу: источник = минимальный размер,
    /// внутреннее состояние = полные данные
    /// </summary>
    public class NodeOverridesController
    {
        private INodeOverrides NodeOverrides { get; set; }
        private IScenarioNode currentNode;

        public List<IComponentVariables> CurrentOverrides { get; } = new(4);
        
        public void BindScenario(INodeOverrides nodeOverrides)
        {
            NodeOverrides = nodeOverrides;
        }
        
        // Для оптимизации пространства и упрощения работы редактора, я сделал внутренний список
        // Внутренний список сделан так, чтобы он минимально очищался и никогда не был null
        // Сериализуемый список оптимизируются при помощи null для экономии места при сериализации
        
        // Mode 1 -> overrides = null
        // Mode 2 -> overrides = [ null, null, ( { "member1", "var1" }, { "member2", "var1" } ) ]
        // Mode 3 -> overrides = [ ( { "member1", "var1" }, { "member2", "var1" } ), ( { "member1", "var2" } ) ]
        
        // Очень важно, что если хотя бы одно поле, хотя бы одного компонента было перезаписано, то он строит
        // полный список компонентов, но там где ничего нет, ставить null.
        // Сделано так, потому что компоненты хранятся в виде обычного списка и единственный способ 
        // их адресации - индекс в общем списке компонентов конкретной ноды
        
        public void Load<TComponent>(IScenarioNodeComponents<TComponent> node) 
            where TComponent : IScenarioComponent
        {
            if (currentNode != null) return;
            currentNode = node;

            FitOverrides(node);
            if (NodeOverrides?.NodeOverrides == null) return;
            var nodeOverrides = NodeOverrides.NodeOverrides.GetValueOrDefault(node.Hash);
            LoadSerializedOverrides(nodeOverrides);
        }
        private void FitOverrides<TComponent>(IScenarioNodeComponents<TComponent> node) where TComponent : IScenarioComponent
        {
            var length = node.Components.Count;
            var lengthDelta = length - CurrentOverrides.Count;

            if (lengthDelta > 0)
            {
                for (var i = 0; i < lengthDelta; i++)
                    CurrentOverrides.Add(IComponentVariables.CreateNew());
            }
            else if (lengthDelta < 0)
            {
                for (var i = 0; i < -lengthDelta; i++)
                    CurrentOverrides.RemoveAt(CurrentOverrides.Count - 1);
            }

            for (var i = 0; i < length; i++)
            {
                CurrentOverrides[i].Clear();
                var component = node.Components[i];
                if (component == null) continue;
                
                var members = component.GetComponentFields();
                foreach (var member in members)
                    CurrentOverrides[i].Insert(member.Name, string.Empty);
            }
        }
        private void LoadSerializedOverrides(List<IComponentVariables> nodeOverrides)
        {
            if (nodeOverrides == null) return;

            var length = nodeOverrides.Count;
            for (var i = 0; i < length; i++)
            {
                var componentOverrides = nodeOverrides[i];
                if (componentOverrides == null) continue;

                foreach (var memberOverride in componentOverrides.MemberVariables)
                    CurrentOverrides[i].Insert(memberOverride);
            }
        }

        public void Save(bool makeNull = true)
        {
            if (currentNode == null) return;
            var serializedOverrides = new List<IComponentVariables>();
            var anyData = false;

            var length = CurrentOverrides.Count;
            for (var i = 0; i < length; i++)
            {
                serializedOverrides.Add(null);
                
                foreach (var memberOverride in CurrentOverrides[i].MemberVariables)
                {
                    if (string.IsNullOrWhiteSpace(memberOverride.VariableName)) continue;

                    anyData = true;
                    serializedOverrides[i] ??= IComponentVariables.CreateNew();
                    serializedOverrides[i].Insert(memberOverride);
                }
            }

            if (NodeOverrides is { NodeOverrides: not null })
            {
                if (anyData)
                    NodeOverrides.NodeOverrides[currentNode.Hash] = serializedOverrides;
                else NodeOverrides.NodeOverrides.Remove(currentNode.Hash);
            }

            if (makeNull) currentNode = null;
        }

        public void Add(IScenarioComponent component)
        {
            if (currentNode == null) return;
            
            var members = component.GetComponentFields();
            var componentVariables = IComponentVariables.CreateNew();
            foreach (var member in members)
                componentVariables.Insert(member.Name, string.Empty);
            CurrentOverrides.Add(componentVariables);
        }
        public void Insert(int index, IScenarioComponent component)
        {
            if (currentNode == null) return;
            
            var members = component.GetComponentFields();
            var componentVariables = IComponentVariables.CreateNew();
            foreach (var member in members)
                componentVariables.Insert(member.Name, string.Empty);
            CurrentOverrides.Insert(index, componentVariables);
        }
        public void Remove(int componentIndex)
        {
            if (currentNode == null) return;
            CurrentOverrides.RemoveAt(componentIndex);
        }
        public void Swap(int oldIndex, int newIndex)
        {
            if (currentNode == null) return;
            (CurrentOverrides[oldIndex], CurrentOverrides[newIndex]) =
                (CurrentOverrides[newIndex], CurrentOverrides[oldIndex]);
        }
    }
}