using UnityEngine;

namespace VRF.Scenario.DriveActor.Core
{
    public abstract class BaseInputActor : MonoBehaviour
    {
        [SerializeField] private BaseDriveActor driveActor;
        
        public BaseDriveActor Actor => driveActor;
    }
}