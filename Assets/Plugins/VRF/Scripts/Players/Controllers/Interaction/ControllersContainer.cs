using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRF.Players.Controllers.Interaction
{
    public class ControllersContainer<TController>
    {
        private readonly Dictionary<Type, TController> controllers = new ();

        public ControllersContainer(params object[] controllersArgs)
        {
            var interactionControllerType = typeof(InteractionController<>);
            var controllerType = typeof(TController);
            var typeControllerTuples = from type in controllerType.Assembly.GetTypes()
                let baseType = type.BaseType
                where !type.IsAbstract && controllerType.IsAssignableFrom(type) && IsSubclassOfRawGeneric(interactionControllerType, type)
                select (baseType.GetGenericArguments()[0], (TController)Activator.CreateInstance(type, controllersArgs));

            foreach (var (targetType, controller) in typeControllerTuples)
            {
                if (!controllers.TryAdd(targetType, controller))
                {
                    Debug.LogError($"There is more than one controller for {targetType.Name}");
                }
            }
        }

        public TController GetController(Component target, bool checkInParents = false)
        {
            foreach (var (componentType, controller) in controllers)
            {
                var component = checkInParents
                    ? target.GetComponentInParent(componentType)
                    : target.GetComponent(componentType);
                
                if (component != null)
                {
                    if (controller is ISetupable setupable)
                    {
                        setupable.Setup(component);
                    }

                    return controller;
                }
            }

            return default;
        }
        
        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            toCheck = toCheck.BaseType;
            
            while (toCheck != null && toCheck != typeof(object)) 
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                
                if (generic == cur) 
                {
                    return true;
                }
                
                toCheck = toCheck.BaseType;
            }
            
            return false;
        }
    }
}