using KBCore.Refs;
using UnityEngine;

namespace VRF.IK
{
    /// <summary>
    /// Функционал для скрытия частей тела IK игрока
    /// </summary>
    public class PlayerHideIK : ValidatedMonoBehaviour
    {
        [Header("Conditions")]
        [SerializeField] private bool show = true;
        [SerializeField] private bool showHead = true;
        
        [SerializeField] private bool showLeftArm = true;
        [SerializeField] private bool showRightArm = true;
        
        [SerializeField] private bool showLeftHand = true;
        [SerializeField] private bool showRightHand = true;
        
        [SerializeField] private bool showLeftLeg = true;
        [SerializeField] private bool showRightLeg = true;
        
        [Header("Scale Factors")]
        [SerializeField] private Vector3 showBoneScale = new(1f, 1f, 1f);
        [SerializeField] private Vector3 hideBoneScale = new(0.0001f, 0.0001f, 0.0001f);
        
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject[] bodyRenderers;
        
        [SerializeField, HideInInspector] private Transform hipsBone;
        [SerializeField, HideInInspector] private Transform headBone;
        
        [SerializeField, HideInInspector] private Transform leftHandJoint;
        [SerializeField, HideInInspector] private Transform rightHandJoint;
        
        [SerializeField, HideInInspector] private Transform leftShoulderJoint;
        [SerializeField, HideInInspector] private Transform rightShoulderJoint;
        
        [SerializeField, HideInInspector] private Transform leftLegJoint;
        [SerializeField, HideInInspector] private Transform rightLegJoint;

        public bool Show { get => show; 
            set { show = value; UpdateScale(hipsBone, show); } }
        public bool ShowHead { get => showHead; 
            set { showHead = value; UpdateScale(headBone, showHead); } }
        public bool ShowLeftArm { get => showLeftArm;
            set { showLeftArm = value; UpdateScale(leftHandJoint, showLeftArm); } }
        public bool ShowRightArm { get => showRightArm;
            set { showRightArm = value; UpdateScale(rightHandJoint, showRightArm); } }
        public bool ShowLeftHand { get => showLeftHand; 
            set { showLeftHand = value; UpdateScale(leftShoulderJoint, showLeftHand); } }
        public bool ShowRightHand { get => showRightHand; 
            set { showRightHand = value; UpdateScale(rightShoulderJoint, showRightHand); } }
        public bool ShowLeftLeg { get => showLeftLeg; 
            set { showLeftLeg = value; UpdateScale(leftLegJoint, showLeftLeg); } }
        public bool ShowRightLeg { get => showRightLeg; 
            set { showRightLeg = value; UpdateScale(rightLegJoint, showRightLeg); } }

        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateBones();
            UpdateAll();
        }
        
        private void UpdateBones()
        {
            hipsBone = animator.GetBoneTransform(HumanBodyBones.Hips);
            headBone = animator.GetBoneTransform(HumanBodyBones.Head);
            
            leftHandJoint = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            rightHandJoint = animator.GetBoneTransform(HumanBodyBones.RightHand);
            
            leftShoulderJoint = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
            rightShoulderJoint = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
            
            leftLegJoint = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
            rightLegJoint = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
        }

        private void UpdateScale(Transform target, bool isShow)
        {
            if (target)
                target.localScale = isShow ? showBoneScale : hideBoneScale;
        }

        private void UpdateAll()
        {
            UpdateScale(hipsBone, show);
            UpdateScale(headBone, showHead);
            
            UpdateScale(leftHandJoint, showLeftArm);
            UpdateScale(rightHandJoint, showRightArm);
            
            UpdateScale(leftShoulderJoint, showLeftHand);
            UpdateScale(rightShoulderJoint, showRightHand);
            
            UpdateScale(leftLegJoint, showLeftLeg);
            UpdateScale(rightLegJoint, showRightLeg);
        }
        
        private void Awake()
        {
            DisableRenderer();
        }
        
        public void EnableRenderer()
        {
            foreach (var bodyRenderer in bodyRenderers)
                bodyRenderer.SetActive(true);
        }
        public void DisableRenderer()
        {
            foreach (var bodyRenderer in bodyRenderers)
                bodyRenderer.SetActive(false);
        }
    }
}