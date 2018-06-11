using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{

    public abstract class ContinuousDistribution2
    {

        public double[] Mean { get; private set; }

        public double[,] Covariance { get; private set; }

        public double Correlation { get; private set; }

        public double SigmaX { get; private set; }

        public double SigmaY { get; private set; }

        public ContinuousDistribution2(double[] mean, double[,] covariance)
        {
            Mean = mean;
            Covariance = covariance;

            SigmaX = Math.Sqrt(covariance[0, 0]);
            SigmaY = Math.Sqrt(covariance[1, 1]);

            Correlation = Covariance[0, 1] / (SigmaX * SigmaY);
        }

        public abstract double PDF(double x, double y);

        public abstract double MahalanobisDistance(double x, double y);

    }

}
