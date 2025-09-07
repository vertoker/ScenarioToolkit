using System;
using ScenarioToolkit.Shared;

// ReSharper disable once CheckNamespace
namespace Scenario.Core.Model.Interfaces
{
    public interface IHashable
    {
        public int Hash { get; }

        /// <summary> GetHashCode должен идти от внутреннего Node Hash </summary>
        public int GetHashCode() => Hash;

        public static int Combine(IHashable hashable1, IHashable hashable2)
            => Combine(hashable1.Hash, hashable2.Hash);
        public static int Combine(int hash1, int hash2)
            => CryptoUtility.HashMixV1(hash1, hash2);
    }
}