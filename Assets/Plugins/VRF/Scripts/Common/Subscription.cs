using System;

namespace VRF.Utils
{
    public abstract class Subscription<T>
    {
        protected readonly T bind;
        private readonly Action<T> callback;

        public Subscription(T bind, Action<T> callback)
        {
            this.bind = bind;
            this.callback = callback;
        }

        public virtual void Initialize() { }

        public virtual void Dispose() { }

        protected void InvokeCallback()
        {
            callback?.Invoke(bind);
        }
    }
}