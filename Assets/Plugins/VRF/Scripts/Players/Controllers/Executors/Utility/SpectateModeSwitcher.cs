using System;
using System.Linq;
using VRF.Players.Controllers.Executors.Base;
using VRF.Players.Controllers.Executors.Movement;
using VRF.Players.Controllers.Executors.Movement.Interfaces;
using VRF.Players.Controllers.Models;
using VRF.Players.Controllers.Models.Enums;
using Zenject;

namespace VRF.Players.Controllers.Executors.Utility
{
    public class SpectateModeSwitcher : BaseModelHandler<SpectateModel>, IInitializable, IDisposable
    {
        private readonly IPlayerModeContainer container;
        private readonly SpectateExecutor executor;

        private bool spectateActive;

        public SpectateModeSwitcher(SpectateModel model, IPlayerModeContainer container, 
            SpectateExecutor executor) : base(model)
        {
            this.container = container;
            this.executor = executor;
        }
        
        public void Initialize()
        {
            executor.OnUpdate += OnUpdate;
        }
        public void Dispose()
        {
            executor.OnUpdate -= OnUpdate;
        }
        
        private void OnUpdate()
        {
            switch (spectateActive)
            {
                // Если активен Idle, то
                // если в модели указано да и в исполнителе есть игроки, за которыми можно следить
                case false when Model.Enabled && Model.SpectateOnAdd && executor.Views.Count != 0:
                    
                    if (container.AvailableModes.Contains(PlayerMode.Spectate))
                        container.SwitchTo(PlayerMode.Spectate);
                    
                    spectateActive = true;
                    break;
                
                // Если активен Spectate, то
                // если в модели указано да и в исполнителе уже нет игроков, за которыми можно следить
                case true when Model.Enabled && Model.FreeMovementOnRemove && executor.Views.Count == 0:
                    
                    foreach (var availableMode in container.AvailableModes)
                    {
                        if (availableMode == PlayerMode.Idle)
                        {
                            container.SwitchTo(PlayerMode.Idle);
                            break;
                        }
                        if (availableMode == PlayerMode.Fly)
                        {
                            container.SwitchTo(PlayerMode.Fly);
                            break;
                        }
                    }
                    
                    spectateActive = false;
                    break;
            }
        }
    }
}