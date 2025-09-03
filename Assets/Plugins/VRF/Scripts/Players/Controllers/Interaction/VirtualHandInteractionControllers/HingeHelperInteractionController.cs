using UnityEngine;
using VRF.BNG_Framework.Scripts.Core;
using VRF.Components.Players.Views.Player;

namespace VRF.Players.Controllers.Interaction.VirtualHandInteractionControllers
{
    public class HingeHelperInteractionController : VirtualHandInteractionController<HingeHelper>
    {
        public HingeHelperInteractionController(PlayerWASDView view) : base(view) { }

        public override Vector3 GetPosition() => target.transform.position;

        public override Quaternion GetRotation()
        {
            var hingeHelperTransform = target.transform;
            var plane = new Plane(hingeHelperTransform.up, hingeHelperTransform.position);
            var ray = view.Camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f), view.Camera.stereoActiveEye);

            var hitPlane = plane.Raycast(ray, out var hitDistance);

            if (!hitPlane)
            {
                return view.VirtualHandGrabber.transform.rotation;
            }

            var hitPoint = ray.GetPoint(hitDistance);

            return Quaternion.LookRotation(-hingeHelperTransform.up, hitPoint - hingeHelperTransform.position);
        }
    }
}