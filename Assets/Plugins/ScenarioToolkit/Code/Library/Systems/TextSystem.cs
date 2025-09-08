using Scenario.Base.Components.Actions;
using ScenarioToolkit.Bus;
using ScenarioToolkit.Core.Systems;
using ScenarioToolkit.Shared;
using Zenject;

namespace ScenarioToolkit.Library.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class TextSystem : BaseScenarioSystem
    {
        public TextSystem(ScenarioComponentBus listener) : base(listener)
        {
            Bus.Subscribe<SetTMPText>(TextChanged);
        }

        private void TextChanged(SetTMPText component)
        {
            if (AssertLog.NotNull<SetTMPText>(component.TMPText, nameof(component.TMPText))) return;
            
            component.TMPText.text = component.Text;
        }
    }
}