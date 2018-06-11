using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Mathematics.Probability
{
    public abstract class ContinuousDistributionN
    {

        public int Dimension { get; private set; }

        public double[] Mean { get; private set; }

        public double[,] Covariance { get; private set; }

        protected double[,] Inverse { get; private set; }

        protected double Determinant { get; private set; }

        public ContinuousDistributionN(double[] mean, double[,] covariance)
        {
            Dimension = mean.Length;
            Mean = mean;
            Covariance = covariance;

            Inverse = MatrixMxN.Inverse(covariance);
            Determinant = MatrixMxN.Determinant(covariance);
        }

        public abstract double PDF(double[] x);

        public abstract double MahalanobisDistance(double[] x);

    }
}
