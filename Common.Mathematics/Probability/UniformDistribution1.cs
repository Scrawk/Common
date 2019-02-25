using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{
    /// <summary>
    /// The uniform distribution is constant over a given range 
    /// and zero otherwise.
    /// </summary>
    public class UniformDistribution1 : ContinuousDistribution1
    {

        public UniformDistribution1(double mean, double width)
        {
            Min = mean - width / 2;
            Max = mean + width / 2;
            Width = width;
            Mean = mean;
        }

        public double Min { get; private set; }

        public double Max { get; private set; }

        public double Width { get; private set; }

        public double Mean { get; private set; }

        public override string ToString()
        {
            return string.Format("[UniformDistribution1: Min={0}, Max={1}]", Min, Max);
        }

        public override double PDF(double x)
        {
            if (x < Min || x > Max) return 0;
            return Mean;
        }

        public override double CDF(double x)
        {
            if(x < Min) return 0;
            if (x > Max) return 1;

            return x / Width;
        }

        public override double Sample(System.Random rnd)
        {
            return Min + rnd.NextDouble() * Width;
        }
    }
}
