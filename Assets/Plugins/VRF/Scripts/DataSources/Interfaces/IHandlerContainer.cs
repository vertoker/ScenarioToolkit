using System;

namespace VRF.DataSources.Interfaces
{
    public interface IHandlerContainer<in THandler> where THandler : IModelHandler
    {
        public void Add(THandler handler);
        public void Remove(THandler handler);
        public Action GetRemovePromise(THandler handler);
    }
}