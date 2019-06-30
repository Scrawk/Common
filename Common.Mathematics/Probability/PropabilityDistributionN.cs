using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Mathematics.Probability
{
    public abstract class PropabilityDistributionN
    {

        public PropabilityDistributionN()
        {

        }

        public abstract double PDF(double[] x);

        public abstract double MahalanobisDistance(double[] x);

    }
}
