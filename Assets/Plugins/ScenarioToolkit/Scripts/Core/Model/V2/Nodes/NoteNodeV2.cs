using Scenario.Core.Model.Interfaces;

// Previous: 
//  Current: NoteNodeV2
//     Next: NoteNodeV6

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class NoteNodeV2 : ScenarioNodeV1, IScenarioCompatibilityNoteNode
    {
        public string Text { get; set; } = string.Empty;
    }
}