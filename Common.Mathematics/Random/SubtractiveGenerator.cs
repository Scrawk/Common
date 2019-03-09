
using System.Collections;

namespace Common.Mathematics.Random
{
    /// <summary>
    /// Generate a random number using the subtractive generator method.
    /// </summary>
	public class SubtractiveGenerator : RandomGenerator
	{
		private const uint MAX = 1000000000;

        private uint[] m_state = new uint[55];

        private uint[] m_temp = new uint[55];

        private int m_pos;
		
		public SubtractiveGenerator(uint seed) : base(seed)
		{
            
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[SubtractiveGenerator: Seed={0}]", Seed);
        }

        /// <summary>
        /// A random int between 0 - MaxInt.
        /// </summary>
        public override int Next()
        {
            uint temp = mod(m_state[(m_pos + 1) % 55] - m_state[(m_pos + 32) % 55]);
            m_pos = (m_pos + 1) % 55;
            m_state[m_pos] = temp;

            return (int)(temp % int.MaxValue);
        }

        /// <summary>
        /// Update the seed.
        /// </summary>
        public override void UpdateSeed(uint seed)
        {
            Seed = seed;

            m_temp[0] = mod(seed);
            m_temp[1] = 1;

            for (int i = 2; i < 55; ++i)
                m_temp[i] = mod(m_temp[i - 2] - m_temp[i - 1]);

            for (int i = 0; i < 55; ++i)
                m_state[i] = m_temp[(34 * (i + 1)) % 55];

            m_pos = 54;
            for (int i = 55; i < 220; ++i)
                Next();
        }

        private uint mod(uint n)
        {
            return ((n % MAX) + MAX) % MAX;
        }
	

	}
}






