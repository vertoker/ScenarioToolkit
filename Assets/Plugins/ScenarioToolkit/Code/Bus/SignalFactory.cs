using UnityEngine.Pool;

namespace ScenarioToolkit.Bus
{
    internal static class SignalFactory
    {
        public static ObjectPool<SignalDeclaration> CreateDeclarationPool(
            bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            return new ObjectPool<SignalDeclaration>(Create, 
                null, null, null, 
                collectionCheck, defaultCapacity, maxSize);
            
            SignalDeclaration Create() => new();
        }
        public static ObjectPool<SignalSubscription> CreateSubscriptionPool(
            bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            return new ObjectPool<SignalSubscription>(Create, 
                null, null, null, 
                collectionCheck, defaultCapacity, maxSize);
            
            SignalSubscription Create() => new();
        }
    }
}