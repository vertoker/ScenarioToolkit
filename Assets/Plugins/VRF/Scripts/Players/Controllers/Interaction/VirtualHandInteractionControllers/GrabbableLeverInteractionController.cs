using UnityEngine;
using VRF.Components.Players.Views.Player;
using VRF.VRBehaviours;

namespace VRF.Players.Controllers.Interaction.VirtualHandInteractionControllers
{
    public class GrabbableLeverInteractionController : VirtualHandInteractionController<GrabbableLever>
    {
        private float r;
        
        private const float sideAngleThreshold = 15f;


        public GrabbableLeverInteractionController(PlayerWASDView view) : base(view)
        {
        }

        protected override void SetupInternal()
        {
            r = Vector3.Distance(target.RotatorObject.position, target.PrimaryGrabber.transform.position);
        }

        public override Vector3 GetPosition()
        {
            var rotator = target.RotatorObject;
            var plane = new Plane(rotator.forward, rotator.position);
            var ray = view.Camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f), view.Camera.stereoActiveEye);
            var rayProjection = Vector3.ProjectOnPlane(ray.direction, plane.normal);

            if (Vector3.Angle(ray.direction, rayProjection) > sideAngleThreshold)
            {
                return plane.Raycast(ray, out var hitDistance)
                    ? ray.GetPoint(hitDistance)
                    : view.VirtualHandGrabber.transform.position;
            }

            var alpha = Vector3.Angle(rotator.position - ray.origin, ray.direction);
            var d = Vector3.Distance(ray.origin, rotator.position);
            var a = d * Mathf.Sin(alpha * Mathf.Deg2Rad);

            if (a > r)
            {
                return ray.GetPoint(d + r);
            }
            
            var b = Mathf.Sqrt(r * r - a * a);
            var n = Mathf.Sqrt(d * d - a * a);

            return ray.GetPoint(n - b);
        }

        public override Quaternion GetRotation() => view.VirtualHandGrabber.transform.rotation;
    }
}