using System;
using System.Collections.Generic;

using Common.Mathematics.Random;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// The uniform distribution is constant over a given range 
    /// and zero otherwise.
    /// </summary>
    public class UniformDistribution1 : ProbabilityDistribution1
    {

        public UniformDistribution1(double mean, double width)
        {
            Min = mean - width / 2;
            Max = mean + width / 2;
            Width = width;
            Height = 1 / Width;
            Mean = mean;
        }

        public double Min { get; private set; }

        public double Max { get; private set; }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public double Mean { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[UniformDistribution1: Mean={0}, Width={1}, Height={2}]", Mean, Width, Height);
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
            if (x < Min || x > Max) return 0;
            return Height;
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
            if(x < Min) return 0;
            if (x > Max) return 1;

            return (x - Min) / Width;
        }

        /// <summary>
        /// Sample a value from distribution for a given random varible.
        /// </summary>
        /// <param name="rnd">Generator for a random varible between 0-1 (inculsive)</param>
        /// <returns>A value from the distribution</returns>
        public override double Sample(RandomGenerator rnd)
        {
            return Min + rnd.NextDouble() * Width;
        }
    }
}
