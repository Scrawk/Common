
using System.Collections;

namespace Common.Mathematics.Random
{
    /// <summary>
    /// Generate a random number using the subtractive generator method.
    /// </summary>
	public class SubtractiveGenerator : RandomGenerator
	{
		static int MAX = 1000000000;
		static double INV_MAX = 1.0 / MAX;

        /// <summary>
        /// Random double between 0 and 1 (inclusive).
        /// </summary>
		public override double Value { get { return Next() * INV_MAX; } }

        int[] m_state = new int[55];

        int[] m_temp = new int[55];

        int m_pos;
		
		public SubtractiveGenerator(int seed) : base(seed)
		{
            
		}

        /// <summary>
        /// Update the seed.
        /// </summary>
        public override void UpdateSeed(int seed)
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

        private int mod(int n)
        {
            return ((n % MAX) + MAX) % MAX;
        }
		
		private int Next() 
		{
			int temp = mod(m_state[(m_pos + 1) % 55] - m_state[(m_pos + 32) % 55]);
			m_pos = (m_pos + 1) % 55;
			m_state[m_pos] = temp;
			return temp;
		}

	}
}






