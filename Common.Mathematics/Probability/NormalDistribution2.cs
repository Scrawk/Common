using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Mathematics.Probability
{

    public class NormalDistribution2 : ContinuousDistribution2
    {

        private double m_factor;

        public NormalDistribution2(double[] mean, double[,] covariance) : base(mean, covariance)
        {
            double ox = SigmaX;
            double oy = SigmaY;
            double p = Correlation;
            double p1 = 1.0 - p * p;

            m_factor = 1.0 / (Math.PI * 2.0 * ox * oy * Math.Sqrt(p1));
        }

        public override double PDF(double x, double y)
        {
            double o2x = Covariance[0, 0];
            double o2y = Covariance[1, 1];
            double ox = SigmaX;
            double oy = SigmaY;
            double p = Correlation;
            double p1 = 1.0 - p * p;

            double dx = x - Mean[0];
            double dy = y - Mean[1];

            double xx = (dx * dx) / o2x;
            double yy = (dy * dy) / o2y;
            double xy = (2.0 * p * dx * dy) / (ox*oy);

            return m_factor * Math.Exp(-1.0 / (2.0 * p1) * (xx + yy - xy));
        }

        public override double MahalanobisDistance(double x, double y)
        {
            Matrix2x2d C = (new Matrix2x2d(Covariance)).Inverse;
            Vector2d xm = new Vector2d(x - Mean[0], y - Mean[1]);

            Vector2d xmTC = new Vector2d();
            xmTC.x = xm.x * C.m00 + xm.y * C.m10;
            xmTC.y = xm.x * C.m01 + xm.y * C.m11;

            return Math.Sqrt(Vector2d.Dot(xmTC, xm));
        }

    }

}
