using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Probability
{

    public class NormalDistributionN : PropabilityDistributionN
    {

        private double m_factor;

        private Matrix m_inverse;

        private double m_determinant;

        public NormalDistributionN(Vector mean, Matrix covariance)
        {
            Mean = mean.Copy();
            Covariance = covariance.Copy();

            m_determinant = Covariance.Determinant;
            m_inverse = Covariance.GetInverse(m_determinant);

            m_factor = 1.0 / Math.Sqrt(Math.Pow(2.0 * Math.PI, Dimension) * m_determinant);
        }

        public NormalDistributionN(double[] mean, double[,] covariance)
        {
            Mean = new Vector(mean);
            Covariance = new Matrix(covariance);

            m_determinant = Covariance.Determinant;
            m_inverse = Covariance.GetInverse(m_determinant);
            
            m_factor = 1.0 / Math.Sqrt(Math.Pow(2.0 * Math.PI, Dimension) * m_determinant);
        }

        public override int Dimension => Mean.Dimension;

        public Vector Mean { get; private set; }

        public Matrix Covariance { get; private set; }

        /// <summary>
        /// TODO - check this works
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public override double PDF(Vector x)
        {
            if (x.Dimension != Dimension)
                throw new ArgumentException("X must be the same size as distributions dimension.");

            var xm = new Vector(Dimension);
            for (int i = 0; i < Dimension; i++)
                xm[i] = x[i] - Mean[i];

            Vector xmTC = m_inverse * xm;
            double dp = Vector.Dot(xmTC, xm);

            return m_factor * Math.Exp(-0.5 * dp);
        }

        /// <summary>
        /// TODO - check this works
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public override double MahalanobisDistance(Vector x)
        {
            if (x.Dimension != Dimension)
                throw new ArgumentException("X must be the same size as distributions dimension.");

            var xm = new Vector(Dimension);
            for (int i = 0; i < Dimension; i++)
                xm[i] = x[i] - Mean[i];

            Vector xmTC = m_inverse * xm;
            double dp = Vector.Dot(xmTC, xm);

            return Math.Sqrt(dp);
        }

    }

}