using System;
using System.Collections.Generic;

using Common.Mathematics.Random;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// A continuous or discrete probability distribution. 
    /// </summary>
    public abstract class ProbabilityDistribution1
    {

        public ProbabilityDistribution1()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[ProbabilityDistribution1: ]");
        }

        /// <summary>
        /// The probability density function.
        /// Used to specify the probability of the random 
        /// variable falling within a particular range of values
        /// </summary>
        /// <param name="x">A random varible from the distribution</param>
        /// <returns>The probablity of the function at x.</returns>
        public abstract double PDF(double x);

        /// <summary>
        /// The cumulative distribution function.
        /// It gives the area under the probability density 
        /// function from minus infinity to x. 
        /// </summary>
        /// <param name="x">A random varible from the distribution</param>
        /// <returns>The area of the PDF function from -infiniy to x</returns>
        public abstract double CDF(double x);

        /// <summary>
        /// Sample a value from distribution for a given random varible.
        /// </summary>
        /// <param name="rnd">Generator for a random varible between 0-1 (inculsive)</param>
        /// <returns>A value from the distribution</returns>
        public abstract double Sample(RandomGenerator rnd);

    }
}
