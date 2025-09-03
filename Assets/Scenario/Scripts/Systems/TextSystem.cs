using Scenario.Base.Components.Actions;
using Scenario.Core.Systems;
using Scenario.Core.Systems.States;
using Scenario.States;
using Scenario.Utilities;
using Zenject;

namespace Scenario.Systems
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public class TextSystem : BaseScenarioStateSystem<TextState>
    {
        public TextSystem(SignalBus listener) : base(listener)
        {
            Bus.Subscribe<SetTMPText>(TextChanged);
        }

        protected override void ApplyState(TextState state)
        {
            foreach (var (key, (defaultData, data)) in state.Texts)
            {
                key.text = data;
            }
        }

        private void TextChanged(SetTMPText component)
        {
            if (AssertLog.NotNull<SetTMPText>(component.TMPText, nameof(component.TMPText))) return;
            
            State.Texts.SetStateData(component.TMPText, component.TMPText.text, component.Text);
            
            component.TMPText.text = component.Text;
        }
    }
}