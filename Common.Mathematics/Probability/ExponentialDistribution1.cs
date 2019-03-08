using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// The probability for the waiting time t until 
    /// the next event, for a completely random process.
    /// </summary>
    public class ExponentialDistribution1 : ProbabilityDistribution1
    {

        public ExponentialDistribution1(double time)
        {
            Time = time;
        }

        /// <summary>
        /// The average waiting time for a event to occur.
        /// </summary>
        public double Time { get; private set; }

        /// <summary>
        /// The probability density function.
        /// Used to specify the probability of the random 
        /// variable falling within a particular range of values
        /// </summary>
        /// <param name="x">A random varible from the distribution</param>
        /// <returns>The probablity of the function at x.</returns>
        public override double PDF(double t)
        {
            if (t < 0) return 0;
            return Math.Exp(-t / Time) / Time;
        }

        /// <summary>
        /// The cumulative distribution function.
        /// It gives the area under the probability density 
        /// function from minus infinity to x. 
        /// </summary>
        /// <param name="x">A random varible from the distribution</param>
        /// <returns>The area of the PDF function from -infiniy to x</returns>
        public override double CDF(double t)
        {
            if (t < 0) return 0;
            return 1.0 - Math.Exp(-t / Time);
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
