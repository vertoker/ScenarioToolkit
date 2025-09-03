using Mirror;

namespace Scenario.Core.Systems.States
{
    public class NetworkContinuousData
    {
        public double CreationTime = NetworkTime.time;
        public float Seconds;

        public NetworkContinuousData(float seconds)
        {
            Seconds = seconds;
        }

        public double GetPassedTime() => NetworkTime.time - CreationTime;
        public double GetRemainingTime() => Seconds - GetPassedTime();
    }
}