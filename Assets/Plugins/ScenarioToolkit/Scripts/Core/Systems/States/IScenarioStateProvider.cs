namespace Scenario.Core.Systems.States
{
    public interface IScenarioStateProvider
    {
        public IState GetState();
        public void SetState(IState state);
    }
}