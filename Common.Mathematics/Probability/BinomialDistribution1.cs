using System;
using System.Collections.Generic;
using System.Numerics;

using Common.Core.Mathematics;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// The random varible is the number k  of 
    /// successes in a collection of n Bernoulli process.
    /// </summary>
    public class BinomialDistribution1 : ProbabilityDistribution1
    {

        public BinomialDistribution1(int trials, double probability)
        {
            Trials = trials;
            TrailsFactorial = IMath.FactorialBI(Trials);
            Probability = DMath.Clamp01(probability);
        }

        /// <summary>
        /// The number of trials.
        /// </summary>
        public int Trials { get; private set; }

        /// <summary>
        /// The constant factorial of the number of trials.
        /// </summary>
        private BigInteger TrailsFactorial { get; set; }

        /// <summary>
        /// The probibilty of the random varible
        /// being equal to 1.
        /// </summary>
        public double Probability { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[BinomialDistribution1: Probability={0}, Trials={1}]", Probability, Trials);
        }

        /// <summary>
        /// The probability density function.
        /// Used to specify the probability of the random 
        /// variable falling within a particular range of values
        /// </summary>
        /// <param name="k">A random varible from the distribution. 
        /// Should be a whole number representing the number of 
        /// successful outcome in trials</param>
        /// <returns>The probablity of the function at x.</returns>
        public override double PDF(double k)
        {
            int K = (int)k;

            return BinomialCoefficient(K) * Math.Pow(Probability, K) * Math.Pow(1.0 - Probability, Trials - K);
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
                sum += BinomialCoefficient(i) * Math.Pow(Probability, i) * Math.Pow(1.0 - Probability, Trials - i);

            return sum;
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

        /// <summary>
        /// The binomial coefficient using trials as n.
        /// </summary>
        private double BinomialCoefficient(int k)
        {
            return (double)(TrailsFactorial / (IMath.FactorialBI(k) * IMath.FactorialBI(Trials - k)));
        }
    }
}
