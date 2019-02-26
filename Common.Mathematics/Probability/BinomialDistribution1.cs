using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// The random varible is the number k  of 
    /// successes in a collection of n Bernoulli process.
    /// </summary>
    public class BinomialDistribution1 : ContinuousDistribution1
    {

        public BinomialDistribution1()
        {

        }

        /// <summary>
        /// The probability density function.
        /// Used to specify the probability of the random 
        /// variable falling within a particular range of values
        /// </summary>
        /// <param name="x">A random varible from the distribution</param>
        /// <returns>The probablity of the function at x.</returns>
        public override double PDF(double x)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The cumulative distribution function.
        /// It gives the area under the probability density 
        /// function from minus infinity to x. 
        /// </summary>
        /// <param name="x">A random varible from the distribution</param>
        /// <returns>The area of the PDF function from -infiniy to x</returns>
        public override double CDF(double x)
        {
            throw new NotImplementedException();
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
