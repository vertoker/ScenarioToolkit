using Scenario.Core.Model.Interfaces;

// Previous: NoteNodeV2
//  Current: NoteNodeV6
//     Next: 

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class NoteNodeV6 : ScenarioNodeV6, INoteNode
    {
        public string Text { get; set; } = string.Empty;
    }
}