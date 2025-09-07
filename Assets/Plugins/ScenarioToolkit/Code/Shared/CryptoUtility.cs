using System;
using Scenario.Core.Model.Interfaces;
using Random = UnityEngine.Random;

namespace Scenario.Utilities
{
#if ENABLE_IL2CPP
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.NullChecks,        false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOption (Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
#endif
    public static class CryptoUtility
    {
        public static string GetRandomString(int length = 6) => Guid.NewGuid().ToString()[..length];
        
        public static int HashMixV1(int hash1, int hash2)
        {
            // https://stackoverflow.com/questions/1646807/quick-and-simple-hash-code-combinations
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + hash1;
                hash = hash * 31 + hash2;
                return hash;
            }
        }

        public static int RandomizeHash() => Random.Range(int.MinValue, int.MaxValue);
        
        /// <summary> Генерация хэша через GetHashCode </summary>
        public static int InitializeHash(this IHashableSource hashableSource)
        {
            var newHash = hashableSource.GetBaseHashCode();
            hashableSource.Hash = newHash;
            return newHash;
        }
        /// <summary> Генерация хэша через Random Range </summary>
        public static int RandomizeHash(this IHashableSource hashableSource)
        {
            var newHash = RandomizeHash();
            hashableSource.Hash = newHash;
            return newHash;
        }
    }
}