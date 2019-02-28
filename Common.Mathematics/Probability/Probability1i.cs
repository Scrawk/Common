using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Common.Core.Mathematics;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// A probability event.
    /// </summary>
    public struct Probability1i : IEquatable<Probability1i>, IComparable<Probability1i>
    {
        /// <summary>
        /// The events probability. Must be 0-1.
        /// </summary>
        public double P;

        /// <summary>
        /// The out come of the event.
        /// </summary>
        public int Outcome;

        public Probability1i(double p, int outcome)
        {
            P = DMath.Clamp01(p);
            Outcome = outcome;
        }

        /// <summary>
        /// Compare if p1 less likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Probability1i p1, Probability1i p2)
        {
            return p1.P < p2.P;
        }

        /// <summary>
        /// Compare if p1 less likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Probability1i p1, Probability1i p2)
        {
            return p1.P <= p2.P;
        }

        /// <summary>
        /// Compare if p1 more likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Probability1i p1, Probability1i p2)
        {
            return p1.P > p2.P;
        }

        /// <summary>
        /// Compare if p1 more likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Probability1i p1, Probability1i p2)
        {
            return p1.P >= p2.P;
        }

        /// <summary>
        /// Are the two events equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Probability1i p1, Probability1i p2)
        {
            return (p1.P == p2.P && p1.Outcome == p2.Outcome);
        }

        /// <summary>
        /// Are the two events equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Probability1i p1, Probability1i p2)
        {
            return (p1.P != p2.P || p1.Outcome != p2.Outcome);
        }

        /// <summary>
        /// Are the two events equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is Probability1i)) return false;
            Probability1i p = (Probability1i)obj;
            return this == p;
        }

        /// <summary>
        /// Are the two events equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Probability1i p)
        {
            return this == p;
        }

        /// <summary>
        /// The events hash code.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ P.GetHashCode();
                hash = (hash * 16777619) ^ Outcome.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Probability1i: Probability={0}, Outcome={1}]", P, Outcome);
        }

        /// <summary>
        /// Compare two events by there probability..
        /// </summary>
        public int CompareTo(Probability1i other)
        {
            return P.CompareTo(other.P);
        }
    }
}
