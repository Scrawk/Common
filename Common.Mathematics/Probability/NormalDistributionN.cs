using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Mathematics.Probability
{

    public class NormalDistributionN : ContinuousDistributionN
    {

        private double m_factor;

        private double[,] m_inverse;

        private double m_determinant;

        public NormalDistributionN(double[] mean, double[,] covariance)
        {
            Dimension = mean.Length;
            Mean = mean;
            Covariance = covariance;

            m_inverse = MatrixMxN.Inverse(covariance);
            m_determinant = MatrixMxN.Determinant(covariance);

            m_factor = 1.0 / Math.Sqrt(Math.Pow(2.0 * Math.PI, Dimension) * m_determinant);

        }

        public int Dimension { get; private set; }

        public double[] Mean { get; private set; }

        public double[,] Covariance { get; private set; }

        public override double PDF(double[] x)
        {

            if (x.Length != Dimension)
                throw new ArgumentException("X must be the same size as distributions dimension.");

            double[] xm = new double[Dimension];
            for (int i = 0; i < Dimension; i++)
                xm[i] = x[i] - Mean[i];

            double[] xmTC = MatrixMxN.MultiplyMatrix(xm, m_inverse);

            double dp = 0;
            for (int i = 0; i < Dimension; i++)
                dp += xmTC[i] * xm[i];

            return m_factor * Math.Exp(-0.5 * dp);
        }

        public override double MahalanobisDistance(double[] x)
        {

            if (x.Length != Dimension)
                throw new ArgumentException("X must be the same size as distributions dimension.");

            double[] xm = new double[Dimension];
            for (int i = 0; i < Dimension; i++)
                xm[i] = x[i] - Mean[i];

            double[] xmTC = MatrixMxN.MultiplyMatrix(xm, m_inverse);

            double dp = 0;
            for (int i = 0; i < Dimension; i++)
                dp += xmTC[i] * xm[i];

            return Math.Sqrt(dp);
        }

    }

}
