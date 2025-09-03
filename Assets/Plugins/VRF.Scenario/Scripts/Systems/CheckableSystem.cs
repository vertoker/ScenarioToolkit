using Scenario.Core;
using Scenario.Core.Systems;
using VRF.Components.Players;
using VRF.Components.Players.Views.Player;
using VRF.Players.Checking;
using VRF.Players.Core;
using VRF.Players.Models;
using VRF.Scenario.Components.Conditions;
using VRF.VRBehaviours;
using VRF.VRBehaviours.Checking;
using Zenject;

namespace VRF.Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class CheckableSystem : BaseScenarioSystem
    {
        private readonly PlayersContainer playersContainer;
        private CheckingController[] checkers;
        
        public CheckableSystem(SignalBus listener, PlayersContainer playersContainer) : base(listener)
        {
            this.playersContainer = playersContainer;
            
            this.playersContainer.PlayerChanged += PlayerChanged;
        }

        private void PlayerChanged()
        {
            Unsubscribe();
            UpdateCheckers();
            Subscribe();
        }

        private void UpdateCheckers()
        {
            checkers = playersContainer.CurrentValue.View switch
            {
                PlayerVRView playerVR => new[] { playerVR.CheckingLeft, playerVR.CheckingRight },
                PlayerWASDView playerWASD => new[] { playerWASD.CheckingController },
                _ => checkers
            };
        }

        private void Subscribe()
        {
            if(checkers == null) return;
            
            foreach (var checker in checkers)
            {
                checker.CheckSuccessful += OnCheckSuccess;
            }
        }

        private void Unsubscribe()
        {
            if(checkers == null) return;

            foreach (var checker in checkers)
            {
                checker.CheckSuccessful -= OnCheckSuccess;
            }
        }

        private void OnCheckSuccess(Checkable checkable)
        {
            Bus.Fire(new CheckableChecked
            {
                Checkable = checkable
            });
        }
    }
}