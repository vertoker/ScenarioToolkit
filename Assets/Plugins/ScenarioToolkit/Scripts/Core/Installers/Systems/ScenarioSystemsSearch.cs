using System;
using UnityEngine;

namespace Scenario.Core.Installers.Systems
{
    public class ScenarioSystemsSearch : MonoBehaviour
    {
        public void CreateSystemInstaller(Type installerType)
        {
            var installer = new GameObject(installerType.Name, installerType);
            installer.transform.SetParent(transform);
            
            installer.transform.localPosition = Vector3.zero;
            installer.transform.localRotation = Quaternion.identity;
            installer.transform.localScale = Vector3.one;
        }
    }
}