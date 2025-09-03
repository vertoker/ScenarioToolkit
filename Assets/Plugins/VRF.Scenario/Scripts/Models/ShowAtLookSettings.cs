using System;
using VRF.Scenario.Interfaces;

namespace VRF.Scenario.Models
{
    [Serializable]
    public class ShowAtLookSettings : IShowAtLookSettingsProvider
    {
        public float distanceSpawn;
        public float scaleMultiplier;
        public float offsetAngle;
        
        public ShowAtLookSettings(float distanceSpawn, float scaleMultiplier, float offsetAngle)
        {
            this.distanceSpawn = distanceSpawn > 0 ? distanceSpawn : 0;
            this.scaleMultiplier = scaleMultiplier > 0 ? scaleMultiplier : 0;
            this.offsetAngle = offsetAngle;
        }

        public ShowAtLookSettings GetSettings() => this;
    }
}