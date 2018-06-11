using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Probability;

namespace Common.Mathematics.Test.Probability
{
    [TestClass]
    public class Mathematics_Probability_NormalDistributionNTest
    {
        [TestMethod]
        public void CompareToDistribution2()
        {

            double[] mean = new double[]
            {
                1, 2
            };

            double[,] covariance = new double[,]
            {
                { 4, 0.5 },
                { 0.5, 1 }
            };

            NormalDistribution2 distribution2 = new NormalDistribution2(mean, covariance);
            NormalDistributionN distributionN = new NormalDistributionN(mean, covariance);

            for (float y = -4; y <= 4; y +=1.0f)
            {
                for (float x = -4; x <= 4; x += 1.0f)
                {
                    double p2 = distribution2.PDF(x, y);
                    double pN = distributionN.PDF(new double[] { x, y });

                    Assert.AreEqual(Math.Round(p2, 6), Math.Round(pN, 6));

                    double d2 = distribution2.MahalanobisDistance(x, y);
                    double dN = distributionN.MahalanobisDistance(new double[] { x, y });

                    Assert.AreEqual(Math.Round(d2, 6), Math.Round(dN, 6));
                }
            }

        }
    }
}
