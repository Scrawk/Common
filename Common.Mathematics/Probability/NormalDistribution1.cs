﻿using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// A gaussian or normal distribution.
    /// Given enough samples many other distributions
    /// reduce to guassian.
    /// </summary>
    public class NormalDistribution1 : ProbabilityDistribution1
    {

        private double m_factor;

        public NormalDistribution1(double mean, double sigma)
        {
            Mean = mean;
            Sigma = sigma;
            Variance = sigma * sigma;
            m_factor = 1.0 / (sigma * Math.Sqrt(Math.PI * 2.0));
        }

        public double Mean { get; private set; }

        public double Variance { get; private set; }

        public double Sigma { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[NormalDistribution1: Mean={0}, Sigma={1}]", Mean, Sigma);
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
            double d = x - Mean;
            return m_factor * Math.Exp(-(d * d) / (2.0 * Variance));
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
            double erf = ErrorFunction((x - Mean) / Math.Sqrt(Variance * 2.0));
            return 0.5 * (1.0 + erf);
        }

        /// <summary>
        /// Sample a value from distribution for a given random varible.
        /// </summary>
        /// <param name="rnd">Generator for a random varible between 0-1 (inculsive)</param>
        /// <returns>A value from the distribution</returns>
        private bool m_useLast;
        private double m_y2;
        public override double Sample(System.Random rnd)
        {
            double x1, x2, w, y1;

            if (m_useLast)
            {
                y1 = m_y2;
                m_useLast = false;
            }
            else
            {
                do
                {
                    x1 = 2.0 * rnd.NextDouble() - 1.0;
                    x2 = 2.0 * rnd.NextDouble() - 1.0;
                    w = x1 * x1 + x2 * x2;
                }
                while (w >= 1.0);

                w = Math.Sqrt(-2.0 * Math.Log(w) / w);
                y1 = x1 * w;
                m_y2 = x2 * w;
                m_useLast = true;
            }

            return Mean + y1 * Sigma;
        }
        
        private double ErrorFunction(double x)
        {
            // constants
            const double a1 = 0.254829592;
            const double a2 = -0.284496736;
            const double a3 = 1.421413741;
            const double a4 = -1.453152027;
            const double a5 = 1.061405429;
            const double p = 0.3275911;

            // Save the sign of x
            int sign = 1;
            if (x < 0) sign = -1;

            x = Math.Abs(x);

            // A&S formula 7.1.26
            double t = 1.0 / (1.0 + p * x);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            return sign * y;
        }

    }

}
