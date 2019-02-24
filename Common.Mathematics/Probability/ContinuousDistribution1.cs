using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{
    public abstract class ContinuousDistribution1
    {

        public double Mean { get; private set; }

        public double Variance { get; private set; }

        public double Sigma { get; private set; }

        public ContinuousDistribution1(double mean, double sigma)
        {
            Mean = mean;
            Sigma = sigma;
            Variance = sigma * sigma;
        }

        public override string ToString()
        {
            return string.Format("[ContinuousDistribution1: Mean={0}, Sigma={1}]", Mean, Sigma);
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
        /// <param name="x">A random varible between 0-1 (inculsive)</param>
        /// <param name="deviations">The number of standard deviations to sample between</param>
        /// <returns>A value from the distribution</returns>
        public virtual double Sample(double x, double deviations = 4)
        {
            return 0;
        }

    }
}
