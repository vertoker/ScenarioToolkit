using System;
using UnityEngine;

namespace VRF.Scenario.Interfaces
{
    public abstract class Failable : MonoBehaviour
    {
        public event Action Failed;

        protected void OnExamFailed() => Failed?.Invoke();
    }
}