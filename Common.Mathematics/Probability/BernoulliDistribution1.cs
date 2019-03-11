using System;
using System.Collections.Generic;

using Common.Core.Mathematics;
using Common.Mathematics.Random;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// The random varible can take on only two values,
    /// 1 and 0, with probabilities p and 1-p.
    /// </summary>
    public class BernoulliDistribution1 : ProbabilityDistribution1
    {

        public BernoulliDistribution1(double probability)
        {
            Probability = DMath.Clamp01(probability);
        }

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
            return string.Format("[BernoulliDistribution1: Probability={0}]", Probability);
        }

        /// <summary>
        /// The probability density function.
        /// Used to specify the probability of the random 
        /// variable falling within a particular range of values
        /// </summary>
        /// <param name="k">A random varible from the distribution. Should be 0 or 1</param>
        /// <returns>The probablity of the function at x.</returns>
        public override double PDF(double k)
        {
            if (k < 0) return 0;
            if (k == 1) return Probability;
            return 1.0 - Probability;
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
            if (k < 0) return 0;
            if (k < 1) return 1.0 - Probability;
            return 1;
        }

        /// <summary>
        /// Sample a value from distribution for a given random varible.
        /// </summary>
        /// <param name="rnd">Generator for a random varible between 0-1 (inculsive)</param>
        /// <returns>A value from the distribution</returns>
        public override double Sample(RandomGenerator rnd)
        {
            if (rnd.NextDouble() <= Probability)
                return 1;
            else
                return 0;
        }
    }
}
