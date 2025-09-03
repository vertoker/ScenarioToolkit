using Scenario.Core;
using Scenario.Core.Systems;
using VRF.Dialog;
using VRF.Scenario.Components.Actions;
using VRF.Scenario.States;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class DialogSystem : BaseScenarioStateSystem<DialogState>
    {
        private readonly DialogService dialogService;

        public DialogSystem(SignalBus bus, DialogService dialogService) : base(bus)
        {
            this.dialogService = dialogService;
            bus.Subscribe<AddDialogOption>(AddDialogOption);

            bus.Subscribe<ClearDialog>(ClearDialog);
        }

        protected override void ApplyState(DialogState state)
        {
            foreach (var dialogLineConfig in state.Dialogs)
            {
                dialogService.AddDialogLine(dialogLineConfig,
                    replica => Bus.Fire(new DialogPlayed() { Line = replica }));
            }
        }

        private void AddDialogOption(AddDialogOption component)
        {
            var dialogLineConfig = component.Line;
            
            State.Dialogs.Add(dialogLineConfig);

            dialogService.AddDialogLine(dialogLineConfig,
                replica => { Bus.Fire(new DialogPlayed() { Line = replica }); });
        }

        private void ClearDialog(ClearDialog component)
        {
            State.Dialogs.Clear();
            
            dialogService.Clear();
        }
    }
}