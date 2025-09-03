using Scenario.Core.Model.Interfaces;
using Scenario.Utilities.Attributes;
using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;

// ReSharper disable once CheckNamespace
namespace VRF.Scenario.Components.Conditions
{
    [ScenarioMeta("HingeJoint стал равен определённому углу", typeof(HingeJoint))]
    public struct HingeJointAngleReached : IScenarioCondition
    {
        public HingeHelper HingeHelper;
        public int Angle;
    }
}