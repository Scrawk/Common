using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{

    public class NormalDistribution1 : ContinuousDistribution1
    {

        private double m_factor;

        public NormalDistribution1(double mean, double sigma) : base(mean, sigma)
        {
            m_factor = 1.0 / (sigma * Math.Sqrt(Math.PI * 2.0));
        }

        public override double PDF(double x)
        {
            double d = x - Mean;
            return m_factor * Math.Exp(-(d * d) / (2.0 * Variance));
        }

        public override double CDF(double x)
        {
            double erf = ErrorFunction((x - Mean) / Math.Sqrt(Variance * 2.0));
            return 0.5 * (1.0 + erf);
        }

    }

}
