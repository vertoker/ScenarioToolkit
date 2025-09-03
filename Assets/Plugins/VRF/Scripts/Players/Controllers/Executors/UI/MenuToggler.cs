using SimpleUI.Anim;
using SimpleUI.Core;
using UnityEngine.InputSystem;
using VRF.Players.Controllers.Utility;
using VRF.Players.Services;
using VRF.Utils.Activities;

namespace VRF.Players.Controllers.Executors.UI
{
    public class MenuToggler<TScreen> : ScreenToggler<TScreen> where TScreen : ScreenBase 
    {
        public readonly ActivityChain HideoutActivity = new(true);
        private readonly CursorHideoutService hideoutService;

        public MenuToggler(ScreensManager manager, CursorHideoutService hideoutService, InputAction esc)
            // TODO нет анимаций, потому что WASD. Потом придумать, как это исправить
            : base(manager, esc, AnimParameters.NoAnim, AnimParameters.NoAnim, AnimParameters.NoAnim)
        {
            this.hideoutService = hideoutService;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            
            OnOpen += Open;
            OnClose += Close;
            
            HideoutActivity.OnUpdateInverse += hideoutService.SetHideoutActive;
            HideoutActivity.Add(0, false);
        }
        public override void Dispose()
        {
            HideoutActivity.Clear();
            
            OnOpen -= Open;
            OnClose -= Close;
            
            base.Dispose();
        }
        
        private void Open() => HideoutActivity.Remove(0);
        private void Close() => HideoutActivity.Add(0, false);
    }
}