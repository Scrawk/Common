using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// The random varible can take on only two values,
    /// 1 and 0, with probabilities p and 1-p.
    /// </summary>
    public class BernoulliDistribution1 : ContinuousDistribution1
    {
        public BernoulliDistribution1()
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
