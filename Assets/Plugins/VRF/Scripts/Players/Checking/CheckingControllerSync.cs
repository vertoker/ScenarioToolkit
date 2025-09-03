using System;
using UnityEngine;
using VRF.VRBehaviours.Checking;
using Zenject;

namespace VRF.Players.Checking
{
    public class CheckingControllerSync : MonoBehaviour
    {
        [SerializeField] private CheckingController[] checkingControllers;

        private Action<Checkable>[] onStartControllerActions;
        private CheckingController activeController;
        
        [InjectOptional] private CheckingModel checkingModel;
        
        private void OnEnable()
        {
            if (checkingModel is { UseOnlyOne: true })
            {
                onStartControllerActions = new Action<Checkable>[checkingControllers.Length];
                
                for (var i = 0; i < checkingControllers.Length; i++)
                {
                    var i2 = i;
                    onStartControllerActions[i] = _ => CheckStarted(checkingControllers[i2]);
                    checkingControllers[i].CheckStarted += onStartControllerActions[i];
                    checkingControllers[i].CheckStopped += CheckStopped;
                }
            }
        }
        private void OnDisable()
        {
            if (checkingModel is { UseOnlyOne: true })
            {
                for (var i = 0; i < checkingControllers.Length; i++)
                {
                    checkingControllers[i].CheckStarted -= onStartControllerActions[i];
                    checkingControllers[i].CheckStopped -= CheckStopped;
                }
            }
        }
        
        private void CheckStarted(CheckingController controller)
        {
            if (activeController)
                activeController.StopCheck();
            activeController = controller;
        }
        private void CheckStopped(Checkable checkable)
        {
            activeController = null;
        }
    }
}