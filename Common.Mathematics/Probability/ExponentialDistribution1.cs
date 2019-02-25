using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// The probability for the waiting time t until 
    /// the next event, for a completely random process.
    /// </summary>
    public class ExponentialDistribution1 : ContinuousDistribution1
    {
        public ExponentialDistribution1()
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
