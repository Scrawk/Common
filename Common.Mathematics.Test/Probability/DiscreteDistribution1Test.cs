using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Probability;

namespace Common.Mathematics.Test.Probability
{
    [TestClass]
    public class Mathematics_Probability_DiscreteDistribution1Test
    {

        [TestMethod]
        public void Translate()
        {
            DiscreteDistribution1 distribution = new DiscreteDistribution1(4);

            distribution[0] = 0.25;
            distribution[1] = 0.25;
            distribution[2] = 0.25;
            distribution[3] = 0.25;

            distribution.Translate = -1.5;

            distribution.Update();

            Assert.AreEqual(0.0, distribution.Mean);
        }

        [TestMethod]
        public void Scale()
        {
            DiscreteDistribution1 distribution = new DiscreteDistribution1(4);

            distribution[0] = 0.25;
            distribution[1] = 0.25;
            distribution[2] = 0.25;
            distribution[3] = 0.25;

            distribution.Scale = 10.0;

            distribution.Update();

            Assert.AreEqual(15.0, distribution.Mean);
        }

        [TestMethod]
        public void MeanAndVariance()
        {
            DiscreteDistribution1 distribution = new DiscreteDistribution1(4);

            distribution[0] = 0.25;
            distribution[1] = 0.5;
            distribution[2] = 0.15;
            distribution[3] = 0.1;

            distribution.Update();

            Assert.AreEqual(1.1, distribution.Mean);
            Assert.AreEqual(0.79, distribution.Variance);
        }

        [TestMethod]
        public void FromContinuous()
        {
            ContinuousDistribution1 continuous = new NormalDistribution1(10, 4);
            DiscreteDistribution1 distribution = new DiscreteDistribution1(continuous, 1024);

            Assert.AreEqual(1.0, Math.Round(distribution.Sum, 3));
            Assert.AreEqual(continuous.Mean, distribution.Mean);
            Assert.AreEqual(continuous.Sigma, distribution.Sigma);

        }

        [TestMethod]
        public void RandomNumber()
        {
            ContinuousDistribution1 continuous = new NormalDistribution1(2, 3);
            DiscreteDistribution1 distribution = new DiscreteDistribution1(continuous, 1024);

            System.Random rnd = new System.Random(0);

            int size = 100000;
            double[] arr = new double[size];

            for(int i = 0; i < size; i++)
                arr[i] = distribution.RandomNumber(rnd);

            double mean = Statistics.Mean(arr);
            double sigma = Math.Sqrt(Statistics.Variance(arr, mean));

            Assert.AreEqual(continuous.Mean, Math.Round(mean, 1));
            Assert.AreEqual(continuous.Sigma, Math.Round(sigma, 1));
        }

    }
}
