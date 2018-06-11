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

        public abstract double PDF(double x);

        public abstract double CDF(double x);

        protected double ErrorFunction(double x)
        {
            // constants
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

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
