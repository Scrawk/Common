using System;

namespace Common.Mathematics.Random
{
    /// <summary>
    /// A random number with a gaussian distribution.
    /// </summary>
	public class GaussianRandom
	{
        /// <summary>
        /// A random double between 0 and 1 (inclusive).
        /// </summary>
        public double Value { get { return Gaussian(); } }

        /// <summary>
        /// The mean value of the distribution.
        /// </summary>
        public double Mean { get; private set; }

        /// <summary>
        /// The standard deviation of the distribution.
        /// </summary>
        public double StdDev { get; private set; }

        private IRandomGenerator Generator { get; set; }

        private bool m_useLast;
        
        private double m_y2;

        public GaussianRandom(double mean, double stdDev, int seed)
		{
			Mean = mean;
			StdDev = stdDev;
            Generator = new SubtractiveGenerator(seed);
		}

        private double Gaussian()
		{
			double x1, x2, w, y1;
			
			if(m_useLast) 
			{
				y1 = m_y2;
				m_useLast = false;
			} 
			else 
			{
				do 
				{
					x1 = 2.0 * Generator.Value - 1.0;
                    x2 = 2.0 * Generator.Value - 1.0;
					w  = x1 * x1 + x2 * x2;
				} 
				while (w >= 1.0);
				
				w = Math.Sqrt((-2.0 * Math.Log(w)) / w);
				y1 = x1 * w;
				m_y2 = x2 * w;
				m_useLast = true;
			}
			
			return Mean + y1 * StdDev;
		}

	}

}











