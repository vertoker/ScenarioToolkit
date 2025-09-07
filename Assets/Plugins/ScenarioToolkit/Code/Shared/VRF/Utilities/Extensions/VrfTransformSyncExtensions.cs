using UnityEngine;

namespace VRF.Utilities.Extensions
{
    /// <summary>
    /// Расширения для приведения параметров Transform (позиция, вращение, скейл) от одного объекта к другому
    /// </summary>
    public static class VrfTransformSyncExtensions
    {
        #region Global
        public static void SyncTo(this Transform observer, Transform receiver) 
            => Sync(observer, receiver);
        public static void SyncFrom(this Transform receiver, Transform observer) 
            => Sync(observer, receiver);
        private static void Sync(Transform observer, Transform receiver)
        {
            SyncPos(observer, receiver);
            SyncRot(observer, receiver);
            // Для стандартной реализации не нужно
            //SyncLocalScale(observer, receiver);
        }
        
        public static void SyncTo(this Transform observer, Transform receiver,
            bool pos, bool rot, bool sca) => Sync(observer, receiver, pos, rot, sca);
        public static void SyncFrom(this Transform receiver, Transform observer,
            bool pos, bool rot, bool sca) => Sync(observer, receiver, pos, rot, sca);
        private static void Sync(Transform observer, Transform receiver, bool pos, bool rot, bool sca)
        {
            if (pos) SyncPos(observer, receiver);
            if (rot) SyncRot(observer, receiver);
            if (sca) SyncLocalScale(observer, receiver);
        }
        
        public static void SyncLocalTo(this Transform observer, Transform receiver) 
            => SyncLocal(observer, receiver);
        public static void SyncLocalFrom(this Transform receiver, Transform observer) 
            => SyncLocal(observer, receiver);
        private static void SyncLocal(Transform observer, Transform receiver)
        {
            SyncLocalPos(observer, receiver);
            SyncLocalRot(observer, receiver);
            // Для стандартной реализации не нужно
            //SyncLocalScale(observer, receiver);
        }
        
        public static void SyncLocalTo(this Transform observer, Transform receiver,
            bool pos, bool rot, bool sca) => SyncLocal(observer, receiver, pos, rot, sca);
        public static void SyncLocalFrom(this Transform receiver, Transform observer,
            bool pos, bool rot, bool sca) => SyncLocal(observer, receiver, pos, rot, sca);
        private static void SyncLocal(Transform observer, Transform receiver, bool pos, bool rot, bool sca)
        {
            if (pos) SyncLocalPos(observer, receiver);
            if (rot) SyncLocalRot(observer, receiver);
            if (sca) SyncLocalScale(observer, receiver);
        }
        #endregion

        #region Pos
        public static void SyncPosTo(this Transform observer, Transform receiver) 
            => SyncPos(observer, receiver);
        public static void SyncPosFrom(this Transform receiver, Transform observer) 
            => SyncPos(observer, receiver);
        private static void SyncPos(Transform observer, Transform receiver)
        {
            receiver.position = observer.position;
        }
        
        public static void SyncPosTo(this Transform observer, Transform receiver,
            bool x, bool y, bool z) => SyncPos(observer, receiver, x, y, z);
        public static void SyncPosFrom(this Transform receiver, Transform observer,
            bool x, bool y, bool z) => SyncPos(observer, receiver, x, y, z);
        private static void SyncPos(Transform observer, Transform receiver, bool x, bool y, bool z)
        {
            var observerPos = observer.position;
            var receiverPos = receiver.position;
            receiver.position = new Vector3(
                x ? observerPos.x : receiverPos.x,
                y ? observerPos.y : receiverPos.y,
                z ? observerPos.z : receiverPos.z);
        }
        
        public static void SyncLocalPosTo(this Transform observer, Transform receiver) 
            => SyncLocalPos(observer, receiver);
        public static void SyncLocalPosFrom(this Transform receiver, Transform observer) 
            => SyncLocalPos(observer, receiver);
        private static void SyncLocalPos(Transform observer, Transform receiver)
        {
            receiver.localPosition = observer.localPosition;
        }
        
        public static void SyncLocalPosTo(this Transform observer, Transform receiver,
            bool x, bool y, bool z) => SyncLocalPos(observer, receiver, x, y, z);
        public static void SyncLocalPosFrom(this Transform receiver, Transform observer,
            bool x, bool y, bool z) => SyncLocalPos(observer, receiver, x, y, z);
        private static void SyncLocalPos(Transform observer, Transform receiver, bool x, bool y, bool z)
        {
            var observerPos = observer.localPosition;
            var receiverPos = receiver.localPosition;
            receiver.localPosition = new Vector3(
                x ? observerPos.x : receiverPos.x,
                y ? observerPos.y : receiverPos.y,
                z ? observerPos.z : receiverPos.z);
        }
        #endregion

        #region Rot
        public static void SyncRotTo(this Transform observer, Transform receiver) 
            => SyncRot(observer, receiver);
        public static void SyncRotFrom(this Transform receiver, Transform observer) 
            => SyncRot(observer, receiver);
        private static void SyncRot(Transform observer, Transform receiver)
        {
            receiver.rotation = observer.rotation;
        }
        
        public static void SyncRotTo(this Transform observer, Transform receiver,
            bool x, bool y, bool z, bool w) => SyncRot(observer, receiver, x, y, z, w);
        public static void SyncRotFrom(this Transform receiver, Transform observer,
            bool x, bool y, bool z, bool w) => SyncRot(observer, receiver, x, y, z, w);
        private static void SyncRot(Transform observer, Transform receiver, bool x, bool y, bool z, bool w)
        {
            var observerRot = observer.rotation;
            var receiverRot = receiver.rotation;
            receiver.rotation = new Quaternion(
                x ? observerRot.x : receiverRot.x,
                y ? observerRot.y : receiverRot.y,
                z ? observerRot.z : receiverRot.z,
                w ? observerRot.w : receiverRot.w);
        }
        
        public static void SyncLocalRotTo(this Transform observer, Transform receiver) 
            => SyncLocalRot(observer, receiver);
        public static void SyncLocalRotFrom(this Transform receiver, Transform observer) 
            => SyncLocalRot(observer, receiver);
        private static void SyncLocalRot(Transform observer, Transform receiver)
        {
            receiver.localRotation = observer.localRotation;
        }
        
        public static void SyncLocalRotTo(this Transform observer, Transform receiver,
            bool x, bool y, bool z, bool w) => SyncLocalRot(observer, receiver, x, y, z, w);
        public static void SyncLocalRotFrom(this Transform receiver, Transform observer,
            bool x, bool y, bool z, bool w) => SyncLocalRot(observer, receiver, x, y, z, w);
        private static void SyncLocalRot(Transform observer, Transform receiver, bool x, bool y, bool z, bool w)
        {
            var observerRot = observer.localRotation;
            var receiverRot = receiver.localRotation;
            receiver.localRotation = new Quaternion(
                x ? observerRot.x : receiverRot.x,
                y ? observerRot.y : receiverRot.y,
                z ? observerRot.z : receiverRot.z,
                w ? observerRot.w : receiverRot.w);
        }
        #endregion

        #region Euler
        public static void SyncEulerTo(this Transform observer, Transform receiver) 
            => SyncEuler(observer, receiver);
        public static void SyncEulerFrom(this Transform receiver, Transform observer) 
            => SyncEuler(observer, receiver);
        private static void SyncEuler(Transform observer, Transform receiver)
        {
            receiver.eulerAngles = observer.eulerAngles;
        }
        
        public static void SyncEulerTo(this Transform observer, Transform receiver,
            bool x, bool y, bool z) => SyncEuler(observer, receiver, x, y, z);
        public static void SyncEulerFrom(this Transform receiver, Transform observer,
            bool x, bool y, bool z) => SyncEuler(observer, receiver, x, y, z);
        private static void SyncEuler(Transform observer, Transform receiver, bool x, bool y, bool z)
        {
            var observerEuler = observer.eulerAngles;
            var receiverEuler = receiver.eulerAngles;
            receiver.eulerAngles = new Vector3(
                x ? observerEuler.x : receiverEuler.x,
                y ? observerEuler.y : receiverEuler.y,
                z ? observerEuler.z : receiverEuler.z);
        }
        
        public static void SyncLocalEulerTo(this Transform observer, Transform receiver) 
            => SyncLocalEuler(observer, receiver);
        public static void SyncLocalEulerFrom(this Transform receiver, Transform observer) 
            => SyncLocalEuler(observer, receiver);

        private static void SyncLocalEuler(Transform observer, Transform receiver)
        {
            receiver.localEulerAngles = observer.localEulerAngles;
        }
        
        public static void SyncLocalEulerTo(this Transform observer, Transform receiver,
            bool x, bool y, bool z) => SyncLocalEuler(observer, receiver, x, y, z);
        public static void SyncLocalEulerFrom(this Transform receiver, Transform observer,
            bool x, bool y, bool z) => SyncLocalEuler(observer, receiver, x, y, z);
        private static void SyncLocalEuler(Transform observer, Transform receiver, bool x, bool y, bool z)
        {
            var observerEuler = observer.localEulerAngles;
            var receiverEuler = receiver.localEulerAngles;
            receiver.localEulerAngles = new Vector3(
                x ? observerEuler.x : receiverEuler.x,
                y ? observerEuler.y : receiverEuler.y,
                z ? observerEuler.z : receiverEuler.z);
        }
        #endregion

        #region Scale
        public static void SyncLocalScaleTo(this Transform observer, Transform receiver) 
            => SyncLocalScale(observer, receiver);
        public static void SyncLocalScaleFrom(this Transform receiver, Transform observer) 
            => SyncLocalScale(observer, receiver);
        private static void SyncLocalScale(Transform observer, Transform receiver)
        {
            receiver.localScale = observer.localScale;
        }
        
        public static void SyncLocalScaleTo(this Transform observer, Transform receiver,
            bool x, bool y, bool z) => SyncLocalScale(observer, receiver, x, y, z);
        public static void SyncLocalScaleFrom(this Transform receiver, Transform observer,
            bool x, bool y, bool z) => SyncLocalScale(observer, receiver, x, y, z);
        private static void SyncLocalScale(Transform observer, Transform receiver, bool x, bool y, bool z)
        {
            var observerScale = observer.localScale;
            var receiverScale = receiver.localScale;
            receiver.localScale = new Vector3(
                x ? observerScale.x : receiverScale.x,
                y ? observerScale.y : receiverScale.y,
                z ? observerScale.z : receiverScale.z);
        }
        #endregion
    }
}