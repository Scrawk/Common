using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{

    public class DiscreteDistribution1
    {

        public int Size { get; private set; }

        public double Sum { get; private set; }

        public double Mean { get; private set; }

        public double Variance { get; private set; }

        public double Sigma { get; private set; }

        public double Translate { get; set; }

        public double Scale { get; set; }

        private double[] m_probability;

        public DiscreteDistribution1(int size)
        {
            Size = size;
            Scale = 1;
            m_probability = new double[size];
        }

        public DiscreteDistribution1(ContinuousDistribution1 continuous, int size)
        {
            Size = size;
            m_probability = new double[size];

            Mean = continuous.Mean;
            Variance = continuous.Variance;
            Sigma = continuous.Sigma;

            double min = Mean - Sigma * 4.0;
            double max = Mean + Sigma * 4.0;
            double di = (max - min) / size;

            Translate = min;
            Scale = di;

            for (int i = 0; i < size; i++)
            {
                double p = continuous.PDF(min + i * di) * di;
                m_probability[i] = p;
                Sum += p;
            }
        }

        public double this[int i]
        {
            get { return m_probability[i]; }
            set { m_probability[i] = value; }
        }

        public void Update()
        {
            Sum = 0;
            Mean = 0;
            for(int i = 0; i < Size; i++)
            {
                Mean += (i * Scale + Translate) * m_probability[i];
                Sum += m_probability[i];
            }

            Variance = 0;
            for (int i = 0; i < Size; i++)
            {
                double x = (i * Scale + Translate) - Mean;
                Variance += x * x * m_probability[i];
            }

            Sigma = Math.Sqrt(Variance);
        }

        public double RandomNumber(System.Random rnd)
        {
            double r = rnd.NextDouble();

            double sum = m_probability[0];
            int size1 = Size - 1;

            for (int i = 0; i < size1; i++)
            {
                double next = m_probability[i+1];

                if (r >= sum && r < sum + next)
                    return i * Scale + Translate;

                sum += next;
            }

            return size1 * Scale + Translate;
        }

    }

}
