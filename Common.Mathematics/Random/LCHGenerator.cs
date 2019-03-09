using System;
using System.Collections.Generic;

namespace Common.Mathematics.Random
{

    /// <summary>
    /// A linear congruential generator (LCG) is an algorithm that yields
    /// a sequence of pseudo-randomized numbers calculated with a discontinuous 
    /// piecewise linear equation. The method represents one of the oldest 
    /// and best-known pseudorandom number generator algorithms.
    /// If increment = 0, the generator is often called a multiplicative congruential generator
    /// https://en.wikipedia.org/wiki/Linear_congruential_generator
    /// </summary>
    public class LCHGenerator : RandomGenerator
    {

        private ulong m_modulus;

        private ulong m_multiplier;

        private ulong m_increment;

        private ulong m_next;

        public LCHGenerator(uint seed) : base(seed)
        {
            m_modulus = 281474976710656;
            m_multiplier = 25214903917;
            m_increment = 11;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[LCHGenerator: Seed={0}]", Seed);
        }

        /// <summary>
        /// A random int between 0 - MaxInt.
        /// </summary>
        public override int Next()
        {
            m_next = (m_multiplier * m_next + m_increment) % m_modulus;
            return (int)(m_next % int.MaxValue);
        }

        /// <summary>
        /// Update seed.
        /// Called when seed changes.
        /// </summary>
        public override void UpdateSeed(uint seed)
        {
            Seed = seed;
            m_next = seed;
        }

    }
}
