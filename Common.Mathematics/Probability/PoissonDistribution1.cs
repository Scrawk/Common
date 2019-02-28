using System;
using System.Collections.Generic;

using Common.Core.Mathematics;

namespace Common.Mathematics.Probability
{
    
    /// <summary>
    /// The probability distribution for the number
    /// of events that happen in a given region 
    /// (of time, space, etc) for a completely random process.
    /// </summary>
    public class PoissonDistribution1 : ContinuousDistribution1
    {

        public PoissonDistribution1(int lambda)
        {
            Lambda = lambda;
            ExpLambda = Math.Exp(-lambda);
        }

        /// <summary>
        /// The average number of events in the time interval.
        /// </summary>
        public double Lambda { get; private set; }

        /// <summary>
        /// Math.Exp(-lambda)
        /// </summary>
        private double ExpLambda { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[PoissonDistribution1: Lambda={0}]", Lambda);
        }

        /// <summary>
        /// The probability density function.
        /// Used to specify the probability of the random 
        /// variable falling within a particular range of values
        /// </summary>
        /// <param name="k">A random varible from the distribution</param>
        /// <returns>The probablity of the function at x.</returns>
        public override double PDF(double k)
        {
            int K = (int)k;
            if (K < 0) return 0;

            return Math.Pow(Lambda, k) * ExpLambda / (double)IMath.FactorialBI(K);
        }

        /// <summary>
        /// The cumulative distribution function.
        /// It gives the area under the probability density 
        /// function from minus infinity to x. 
        /// </summary>
        /// <param name="k">A random varible from the distribution</param>
        /// <returns>The area of the PDF function from -infiniy to x</returns>
        public override double CDF(double k)
        {
            int K = (int)k;
            if (K < 0) return 0;

            double sum = 0;
            for (int i = 0; i < K; i++)
                sum += Math.Pow(Lambda, i) / (double)IMath.FactorialBI(i);

            return ExpLambda * sum;
        }

        /// <summary>
        /// Sample a value from distribution for a given random varible.
        /// </summary>
        /// <param name="rnd">Generator for a random varible between 0-1 (inculsive)</param>
        /// <returns>A value from the distribution</returns>
        public override double Sample(System.Random rnd)
        {
            throw new NotImplementedException();
        }
    }
}
