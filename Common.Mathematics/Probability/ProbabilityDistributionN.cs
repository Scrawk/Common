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

        public abstract int Dimension { get; }

        public abstract double PDF(Vector x);

        public abstract double MahalanobisDistance(Vector x);

    }
}