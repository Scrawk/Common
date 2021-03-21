using System;
using System.Collections.Generic;

namespace Common.Core.Random
{
    /// <summary>
    /// The Mersenne Twister is a pseudorandom number generator (PRNG). 
    /// It is by far the most widely used general-purpose PRNG.
    /// Its name derives from the fact that its period length is chosen to be a Mersenne prime. 
    /// For all the details on this algorithm, see the original paper:
    /// http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/ARTICLES/mt.pdf
    /// https://en.wikipedia.org/wiki/Mersenne_Twister
    /// https://github.com/cslarsen/mersenne-twister/blob/master/mersenne-twister.cpp
    /// </summary>
    public class MersenneTwister : RandomGenerator
    {

        /// <summary>
        /// We have an array of 624 32-bit values, and there are 31 unused bits, so we
        /// have a seed value of 624*32-31 = 19937 bits.
        /// </summary>
        private const int SIZE = 624;
        private const int PERIOD = 397;
        private const int DIFF = SIZE - PERIOD;
        private const uint MAGIC = 0x9908b0df;

        private uint[] MT = new uint[SIZE];

        private uint[] MT_TEMPERED = new uint[SIZE];

        private int index = SIZE;

        public MersenneTwister()
        {

        }

        public MersenneTwister(int seed) : base(seed)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[MersenneTwister: Seed={0}]", Seed);
        }

        /// <summary>
        /// A random int between 0 - MaxInt.
        /// </summary>
        public override int Next()
        {
            if (index == SIZE)
            {
                GenerateNumbers();
                index = 0;
            }

            return (int)(MT_TEMPERED[index++] % int.MaxValue);
        }

        /// <summary>
        /// Update seed.
        /// Called when seed changes.
        /// </summary>
        public override void UpdateSeed(int seed)
        {
            /*
            * The equation below is a linear congruential generator (LCG), one of the
            * oldest known pseudo-random number generator algorithms, in the form
            * X_(n+1) = = (a*X_n + c) (mod m).
            *
            * We've implicitly got m=32 (mask + word size of 32 bits), so there is no
            * need to explicitly use modulus.
            *
            * What is interesting is the multiplier a.  The one we have below is
            * 0x6c07865 --- 1812433253 in decimal, and is called the Borosh-Niederreiter
            * multiplier for modulus 2^32.
            *
            * It is mentioned in passing in Knuth's THE ART OF COMPUTER
            * PROGRAMMING, Volume 2, page 106, Table 1, line 13.  LCGs are
            * treated in the same book, pp. 10-26
            *
            * You can read the original paper by Borosh and Niederreiter as well.  It's
            * called OPTIMAL MULTIPLIERS FOR PSEUDO-RANDOM NUMBER GENERATION BY THE
            * LINEAR CONGRUENTIAL METHOD (1983) at
            * http://www.springerlink.com/content/n7765ku70w8857l7/
            *
            * You can read about LCGs at:
            * http://en.wikipedia.org/wiki/Linear_congruential_generator
            *
            * From that page, it says: "A common Mersenne twister implementation,
            * interestingly enough, uses an LCG to generate seed data.",
            *
            * Since we're using 32-bits data types for our MT array, we can skip the
            * masking with 0xFFFFFFFF below.
            */

            Seed = seed;
            MT[0] = (uint)seed;
            index = SIZE;

            for (uint i = 1; i < SIZE; ++i)
                MT[i] = 0x6c078965 * (MT[i - 1] ^ MT[i - 1] >> 30) + i;
        }

        private void GenerateNumbers()
        {
            int i = 0;
            uint y = 0;

            // i = [0 ... 226]
            while (i < DIFF)
            {
                int expr = i + PERIOD;
                y = M32(MT[i]) | L31(MT[i + 1]);
                MT[i] = MT[expr] ^ (y >> 1) ^ (((y << 31) >> 31) & MAGIC);
                ++i;
            }

            // i = [227 ... 622]
            while (i < SIZE - 1)
            {
                int expr = i - DIFF;
                y = M32(MT[i]) | L31(MT[i + 1]);
                MT[i] = MT[expr] ^ (y >> 1) ^ (((y << 31) >> 31) & MAGIC);
                ++i;
            }

            // i = 623, last step rolls over
            y = M32(MT[SIZE - 1]) | L31(MT[0]);
            MT[SIZE - 1] = MT[PERIOD - 1] ^ (y >> 1) ^ (((y << 31) >> 31) & MAGIC);

            // Temper all numbers in a batch
            for (i = 0; i < SIZE; ++i)
            {
                y = MT[i];
                y ^= y >> 11;
                y ^= y << 7 & 0x9d2c5680;
                y ^= y << 15 & 0xefc60000;
                y ^= y >> 18;
                MT_TEMPERED[i] = y;
            }

            index = 0;
        }

        /// <summary>
        /// 32nd MSB
        /// </summary>
        private uint M32(uint x)
        {
            return 0x80000000 & x;
        }

        /// <summary>
        /// 31 LSBs
        /// </summary>
        private uint L31(uint x)
        {
            return 0x7FFFFFFF & x;
        }
    }
}
