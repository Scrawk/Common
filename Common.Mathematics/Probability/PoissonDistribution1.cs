using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// The probability distribution for the number
    /// of events that happen in a given region 
    /// (of time, space, etc) for a completely random process.
    /// </summary>
    public class PoissonDistribution1 : ContinuousDistribution1
    {
        public PoissonDistribution1()
        {

        }

        public override double CDF(double x)
        {
            throw new NotImplementedException();
        }

        public override double PDF(double x)
        {
            throw new NotImplementedException();
        }

        public override double Sample(System.Random rnd)
        {
            throw new NotImplementedException();
        }
    }
}
