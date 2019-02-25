using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{

    public abstract class ContinuousDistribution2
    {

        public ContinuousDistribution2()
        {

        }

        public abstract double PDF(double x, double y);

        public abstract double MahalanobisDistance(double x, double y);

    }

}
