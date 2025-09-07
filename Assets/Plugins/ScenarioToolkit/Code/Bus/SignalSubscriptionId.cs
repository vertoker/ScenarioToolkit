using System;
using System.Diagnostics;

namespace ScenarioToolkit.Bus
{
    [DebuggerStepThrough]
    internal readonly struct SignalSubscriptionId : IEquatable<SignalSubscriptionId>
    {
        public Type SignalType { get; }
        public object Token { get; }
        
        public SignalSubscriptionId(Type signalType, object token)
        {
            SignalType = signalType;
            Token = token;
        }
        
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + SignalType.GetHashCode();
                hash = hash * 29 + Token.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object that)
        {
            if (that is SignalSubscriptionId id)
                return Equals(id);

            return false;
        }

        public bool Equals(SignalSubscriptionId other)
        {
            return SignalType == other.SignalType && Equals(Token, other.Token);
        }

        public static bool operator == (SignalSubscriptionId left, SignalSubscriptionId right)
        {
            return left.Equals(right);
        }

        public static bool operator != (SignalSubscriptionId left, SignalSubscriptionId right)
        {
            return !left.Equals(right);
        }
    }
}