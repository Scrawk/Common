using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Probability;

namespace Common.Mathematics.Test.Probability
{
    [TestClass]
    public class StatisticsTest
    {

        [TestMethod]
        public void Mean()
        {
            float[] S = new float[] { 10, 8, 10, 10, 11, 11 };
            float[] T = new float[] { 12, 9, 15, 10, 13, 13 };

            float uS = Statistics.Mean(S);
            float uT = Statistics.Mean(T);

            Assert.AreEqual(10, uS);
            Assert.AreEqual(12, uT);
        }

        [TestMethod]
        public void Variance()
        {
            float[] S = new float[] { 10, 8, 10, 10, 11, 11 };
            float[] T = new float[] { 12, 9, 15, 10, 13, 13 };

            float uS = Statistics.Mean(S);
            float uT = Statistics.Mean(T);

            float vS = Statistics.Variance(S, uS);
            float vT = Statistics.Variance(T, uT);

            Assert.AreEqual(1, vS);
            Assert.AreEqual(4, vT);
        }

        [TestMethod]
        public void VectorMean()
        {

            float[,] data = new float[,]
            {
                {3, 5, 1},
                {9, 1, 4},
            };

            float[] mean = Statistics.Mean(data);

            Assert.AreEqual(3, mean.Length);

            float[] expected = new float[] { 6, 3, 2.5f };

            CollectionAssert.AreEqual(expected, mean);
        }

        [TestMethod]
        public void Covariance()
        {

            float[,] data = new float[,]
            {
                {90, 60, 90},
                {90, 90, 30},
                {60, 60, 60},
                {60, 60, 90},
                {30, 30, 30}
            };

            float[] mean = Statistics.Mean(data);

            float[,] covariance = Statistics.Covariance(data, mean);

            Assert.AreEqual(3, covariance.GetLength(0));
            Assert.AreEqual(3, covariance.GetLength(1));

            float[,] expected = new float[,]
            {
                {504, 360, 180 },
                {360, 360, 0 },
                {180, 0, 720 }
            };

            CollectionAssert.AreEqual(expected, covariance);
        }

    }
}
