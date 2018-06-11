using System;
using System.Collections.Generic;

namespace Common.Mathematics.Probability
{

    public class DiscreteDistribution2
    {

        public int Size { get; private set; }

        public double[] Mean { get; private set; }

        public double[] Translate { get; set; }

        private double[,] m_probability;

        public DiscreteDistribution2(int size)
        {

            Size = size;
            Mean = new double[2];
            Translate = new double[2];

            m_probability = new double[size, size];
     
        }

        public double this[int i, int j]
        {
            get { return m_probability[i, j]; }
            set { m_probability[i, j] = value; }
        }

        public void Update()
        {
            Array.Clear(Mean, 0, 2);

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Mean[0] += (i + Translate[0]) * m_probability[i, j];
                    Mean[1] += (j + Translate[1]) * m_probability[i, j];
                }    
            }

        }

    }

}
