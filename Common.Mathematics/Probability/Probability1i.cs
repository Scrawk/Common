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
        public double Probability;

        /// <summary>
        /// The out come of the event.
        /// </summary>
        public int Outcome;

        public Probability1i(double p)
        {
            Probability = DMath.Clamp01(p);
            Outcome = 0;
        }

        public Probability1i(double p, int outcome)
        {
            Probability = DMath.Clamp01(p);
            Outcome = outcome;
        }

        /// <summary>
        /// Compare if p1 less likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Probability1i p1, Probability1i p2)
        {
            return p1.Probability < p2.Probability;
        }

        /// <summary>
        /// Compare if p1 less likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(Probability1i p1, Probability1i p2)
        {
            return p1.Probability <= p2.Probability;
        }

        /// <summary>
        /// Compare if p1 more likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Probability1i p1, Probability1i p2)
        {
            return p1.Probability > p2.Probability;
        }

        /// <summary>
        /// Compare if p1 more likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(Probability1i p1, Probability1i p2)
        {
            return p1.Probability >= p2.Probability;
        }

        /// <summary>
        /// Are the two events equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Probability1i p1, Probability1i p2)
        {
            return (p1.Probability == p2.Probability && p1.Outcome == p2.Outcome);
        }

        /// <summary>
        /// Are the two events equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Probability1i p1, Probability1i p2)
        {
            return (p1.Probability != p2.Probability || p1.Outcome != p2.Outcome);
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
                hash = (hash * 16777619) ^ Probability.GetHashCode();
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
            return string.Format("[Probability1i: Probability={0}, Outcome={1}]", Probability, Outcome);
        }

        /// <summary>
        /// Compare two events by there probability.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Probability1i other)
        {
            return Probability.CompareTo(other.Probability);
        }

        /// <summary>
        /// Round the probability to a number of digits.
        /// </summary>
        /// <param name="digits"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Round(int digits = 0)
        {
            Probability = Math.Round(Probability, digits);
        }

        /// <summary>
        /// The AND of two independant events (probabilty that
        /// both happen, but the outcome of one does not affect the other)
        /// and the resulting out come.
        /// </summary>
        /// <param name="a">independant probability a</param>
        /// <param name="b">independant probability b</param>
        /// <param name="outcome"> the outcome of a and b events occuring</param>
        /// <returns>The probability of a and b occuring</returns>
        public static Probability1i IndependantAnd(Probability1i a, Probability1i b, int outcome = 0)
        {
            return new Probability1i(a.Probability * b.Probability, outcome);
        }

        /// <summary>
        /// The AND of two dependant events (probabilty that
        /// both happen, and the outcome of one does affect the other)
        /// and the resulting out come.
        /// </summary>
        /// <param name="a">independant probability a</param>
        /// <param name="ab">probability b occuring if a has occured</param>
        /// <param name="outcome"> the outcome of a and b events occuring</param>
        /// <returns>The probability of a and b occuring</returns>
        public static Probability1i DependantAnd(Probability1i a, Probability1i ab, int outcome = 0)
        {
            return new Probability1i(a.Probability * ab.Probability, outcome);
        }

        /// <summary>
        /// The OR of two exclusive events (probabilty one or the 
        /// other happen, but if one occurs the other can not)
        /// and the resulting out come.
        /// </summary>
        /// <param name="a">independant probability a</param>
        /// <param name="b">independant probability b</param>
        /// <param name="outcome"> the outcome of a or b occuring</param>
        /// <returns>The probability of a or b occuring</returns>
        public static Probability1i ExclusiveOr(Probability1i a, Probability1i b, int outcome = 0)
        {
            return new Probability1i(a.Probability + b.Probability, outcome);
        }

        /// <summary>
        /// The OR of two non exclusive events (probabilty of 
        /// either or both occuring) and the resulting out come.
        /// </summary>
        /// <param name="a">independant probability a</param>
        /// <param name="b">independant probability b</param>
        /// <param name="outcome"> the outcome of a or b occuring</param>
        /// <returns>The probability of a or b occuring</returns>
        public static Probability1i NonExclusiveOr(Probability1i a, Probability1i b, int outcome = 0)
        {
            return new Probability1i(a.Probability + b.Probability - a.Probability * b.Probability, outcome);
        }
    }
}
