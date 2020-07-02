using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Common.Core.Numerics;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// A probability event.
    /// </summary>
    public struct ProbabilityEvent1 : IEquatable<ProbabilityEvent1>, IComparable<ProbabilityEvent1>
    {
        /// <summary>
        /// The events probability. Must be 0-1.
        /// </summary>
        public double Probability;

        /// <summary>
        /// The out come of the event.
        /// </summary>
        public int Outcome;

        public ProbabilityEvent1(double p)
        {
            Probability = MathUtil.Clamp01(p);
            Outcome = 0;
        }

        public ProbabilityEvent1(double p, int outcome)
        {
            Probability = MathUtil.Clamp01(p);
            Outcome = outcome;
        }

        /// <summary>
        /// The probabilty of this event not occuring.
        /// </summary>
        public double Not
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return 1.0 - Probability; }
        }

        /// <summary>
        /// Compare if p1 less likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ProbabilityEvent1 p1, ProbabilityEvent1 p2)
        {
            return p1.Probability < p2.Probability;
        }

        /// <summary>
        /// Compare if p1 less likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ProbabilityEvent1 p1, ProbabilityEvent1 p2)
        {
            return p1.Probability <= p2.Probability;
        }

        /// <summary>
        /// Compare if p1 more likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ProbabilityEvent1 p1, ProbabilityEvent1 p2)
        {
            return p1.Probability > p2.Probability;
        }

        /// <summary>
        /// Compare if p1 more likely than p2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ProbabilityEvent1 p1, ProbabilityEvent1 p2)
        {
            return p1.Probability >= p2.Probability;
        }

        /// <summary>
        /// Are the two events equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ProbabilityEvent1 p1, ProbabilityEvent1 p2)
        {
            return (p1.Probability == p2.Probability && p1.Outcome == p2.Outcome);
        }

        /// <summary>
        /// Are the two events equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ProbabilityEvent1 p1, ProbabilityEvent1 p2)
        {
            return (p1.Probability != p2.Probability || p1.Outcome != p2.Outcome);
        }

        /// <summary>
        /// Are the two events equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is ProbabilityEvent1)) return false;
            ProbabilityEvent1 p = (ProbabilityEvent1)obj;
            return this == p;
        }

        /// <summary>
        /// Are the two events equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ProbabilityEvent1 p)
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
        public int CompareTo(ProbabilityEvent1 other)
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
        public static ProbabilityEvent1 IndependantAnd(ProbabilityEvent1 a, ProbabilityEvent1 b, int outcome = 0)
        {
            return new ProbabilityEvent1(a.Probability * b.Probability, outcome);
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
        public static ProbabilityEvent1 DependantAnd(ProbabilityEvent1 a, ProbabilityEvent1 ab, int outcome = 0)
        {
            return new ProbabilityEvent1(a.Probability * ab.Probability, outcome);
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
        public static ProbabilityEvent1 ExclusiveOr(ProbabilityEvent1 a, ProbabilityEvent1 b, int outcome = 0)
        {
            return new ProbabilityEvent1(a.Probability + b.Probability, outcome);
        }

        /// <summary>
        /// The OR of two non exclusive events (probabilty of 
        /// either or both occuring) and the resulting out come.
        /// </summary>
        /// <param name="a">independant probability a</param>
        /// <param name="b">independant probability b</param>
        /// <param name="outcome"> the outcome of a or b occuring</param>
        /// <returns>The probability of a or b occuring</returns>
        public static ProbabilityEvent1 NonExclusiveOr(ProbabilityEvent1 a, ProbabilityEvent1 b, int outcome = 0)
        {
            return new ProbabilityEvent1(a.Probability + b.Probability - a.Probability * b.Probability, outcome);
        }

        /// <summary>
        /// Determine the probability of a depending on z, P(A|Z)
        /// </summary>
        /// <param name="a">probability of a</param>
        /// <param name="za">probability of z depending on a</param>
        /// <param name="z">probability of z</param>
        /// <param name="outcome"> the outcome of A|Z occuring</param>
        /// <returns>probability of a depending on z</returns>
        public static ProbabilityEvent1 SimpleBayes(ProbabilityEvent1 a, ProbabilityEvent1 za, ProbabilityEvent1 z, int outcome = 0)
        {
            return new ProbabilityEvent1(za.Probability * a.Probability / z.Probability, outcome);
        }

        /// <summary>
        /// Determine the probability of a depending on z, P(A|Z)
        /// </summary>
        /// <param name="a">probability of a</param>
        /// <param name="za">probability of z depending on a</param>
        /// <param name="z_not_a">probability of z depending on not a</param>
        /// <param name="outcome">the outcome of A|Z occuring</param>
        /// <returns>probability of a depending on z</returns>
        public static ProbabilityEvent1 ExplicitBayes(ProbabilityEvent1 a, ProbabilityEvent1 za, ProbabilityEvent1 z_not_a, int outcome = 0)
        {
            var zaa = za.Probability * a.Probability;
            return new ProbabilityEvent1(zaa / (zaa + z_not_a.Probability * a.Not), outcome);
        }

        /// <summary>
        /// Determine the probability of ai depending on z, P(Ai|Z)
        /// </summary>
        /// <param name="index">The index of the probability of interest</param>
        /// <param name="a">List of probabilites a</param>
        /// <param name="za">List of probabilites z depending on a</param>
        /// <param name="outcome">the outcome of Ai|Z occuring</param>
        /// <returns>probability of ai depending on z</returns>
        public static ProbabilityEvent1 GeneralBayes(int index, IList<ProbabilityEvent1> a, IList<ProbabilityEvent1> za, int outcome = 0)
        {
            var zaa = za[index].Probability * a[index].Probability;

            double sum = 0;
            for (int i = 0; i < a.Count; i++)
                sum += za[i].Probability * a[i].Probability;

            return new ProbabilityEvent1(zaa / sum, outcome);
        }
    }
}
