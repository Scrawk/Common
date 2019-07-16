using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Probability;

namespace Common.Mathematics.Test.Probability
{
    [TestClass]
    public class NormalDistribution1Test
    {

        [TestMethod]
        public void ZNormalDistribution()
        {

            NormalDistribution1 distribution = new NormalDistribution1(0.0, 1.0);

            Assert.AreEqual(0.0, distribution.Mean);
            Assert.AreEqual(1.0, distribution.Variance);
            Assert.AreEqual(1.0, distribution.Sigma);

            Assert.AreEqual(0.00005, Math.Round(distribution.CDF(-3.9), 5));
            Assert.AreEqual(0.00034, Math.Round(distribution.CDF(-3.4), 5));
            Assert.AreEqual(0.00187, Math.Round(distribution.CDF(-2.9), 5));
            Assert.AreEqual(0.00820, Math.Round(distribution.CDF(-2.4), 5));
            Assert.AreEqual(0.02872, Math.Round(distribution.CDF(-1.9), 5));
            Assert.AreEqual(0.08076, Math.Round(distribution.CDF(-1.4), 5));
            Assert.AreEqual(0.18406, Math.Round(distribution.CDF(-0.9), 5));
            Assert.AreEqual(0.34458, Math.Round(distribution.CDF(-0.4), 5));
            Assert.AreEqual(0.50000, Math.Round(distribution.CDF(0.0), 5));
            Assert.AreEqual(0.65542, Math.Round(distribution.CDF(0.4), 5));
            Assert.AreEqual(0.81594, Math.Round(distribution.CDF(0.9), 5));
            Assert.AreEqual(0.91924, Math.Round(distribution.CDF(1.4), 5));
            Assert.AreEqual(0.97128, Math.Round(distribution.CDF(1.9), 5));
            Assert.AreEqual(0.99180, Math.Round(distribution.CDF(2.4), 5));
            Assert.AreEqual(0.99813, Math.Round(distribution.CDF(2.9), 5));
            Assert.AreEqual(0.99966, Math.Round(distribution.CDF(3.4), 5));
            Assert.AreEqual(0.99995, Math.Round(distribution.CDF(3.9), 5));

        }

        [TestMethod]
        public void Probability()
        {

            /*
                An average light bulb manufactured by the Acme Corporation lasts 300 days with a standard deviation of 50 days. 
                Assuming that bulb life is normally distributed, what is the probability that an Acme light bulb will last at most 365 days?

                Solution: Given a mean score of 300 days and a standard deviation of 50 days, we want to find the cumulative probability 
                that bulb life is less than or equal to 365 days. Thus, we know the following:

                The value of the normal random variable is 365 days.
                The mean is equal to 300 days.
                The standard deviation is equal to 50 days.

                We enter these values into the Normal Distribution Calculator and compute the cumulative probability. 
                The answer is: P( X < 365) = 0.90. Hence, there is a 90% chance that a light bulb will burn out within 365 days.
            */

            NormalDistribution1 distribution = new NormalDistribution1(300, 50);

            Assert.AreEqual(0.90, Math.Round(distribution.CDF(365), 2));

            /*
                Suppose scores on an IQ test are normally distributed. If the test has a mean of 100 and a standard deviation of 10, 
                what is the probability that a person who takes the test will score between 90 and 110?

                Solution: Here, we want to know the probability that the test score falls between 90 and 110. 
                The "trick" to solving this problem is to realize the following:

                P( 90 < X < 110 ) = P( X < 110 ) - P( X < 90 )

                We use the Normal Distribution Calculator to compute both probabilities on the right side of the above equation.

                To compute P( X < 110 ), we enter the following inputs into the calculator: 
                The value of the normal random variable is 110, the mean is 100, and the standard deviation is 10. We find that P( X < 110 ) is 0.84.

                To compute P( X < 90 ), we enter the following inputs into the calculator: 
                The value of the normal random variable is 90, the mean is 100, and the standard deviation is 10. We find that P( X < 90 ) is 0.16.

                We use these findings to compute our final answer as follows:

                P( 90 < X < 110 ) = P( X < 110 ) - P( X < 90 )
                P( 90 < X < 110 ) = 0.84 - 0.16
                P( 90 < X < 110 ) = 0.68

                Thus, about 68% of the test scores will fall between 90 and 110
             */

            distribution = new NormalDistribution1(100, 10);

            Assert.AreEqual(0.84, Math.Round(distribution.CDF(110), 2));
            Assert.AreEqual(0.16, Math.Round(distribution.CDF(90), 2));

        }

    }

}
