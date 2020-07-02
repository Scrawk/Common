using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Probability;

namespace Common.Mathematics.Test.Probability
{
    [TestClass]
    public class ProbabilityEvent1Test
    {
        [TestMethod]
        public void IndependantAnd()
        {
            //The probability of rolling a 2 on six sided dice.
            var a = new ProbabilityEvent1(1.0 / 6.0);
            //The probability of rolling a 5 on six sided dice.
            var b = new ProbabilityEvent1(1.0 / 6.0);
            //the probability of rolling a 2 and 5 on two seperate dice.
            var ab = ProbabilityEvent1.IndependantAnd(a, b);
            ab.Round(4);

            Assert.AreEqual(Math.Round(1.0 / 36.0, 4), ab.Probability);
        }

        [TestMethod]
        public void DependantAnd()
        {
            //If there are 2 red and 3 blue balls in a box

            //The probability of picking a red ball
            var a = new ProbabilityEvent1(2.0 / 5.0);
            //The probability of picking a blue ball if a red ball has been picked.
            var ab = new ProbabilityEvent1(3.0 / 4.0);
            //the probability of rolling a 2 and 5 on two seperate dice.
            var b = ProbabilityEvent1.DependantAnd(a, ab);
            b.Round(4);

            Assert.AreEqual(Math.Round(3.0 / 10.0, 4), Math.Round(b.Probability, 1));
        }

        [TestMethod]
        public void ExclusiveOr()
        {
            //The probability of rolling a 2 on six sided dice.
            var a = new ProbabilityEvent1(1.0 / 6.0);
            //The probability of rolling a 5 on six sided dice.
            var b = new ProbabilityEvent1(1.0 / 6.0);
            //the probability of rolling a 2 or 5 on the same roll
            var ab = ProbabilityEvent1.ExclusiveOr(a, b);
            ab.Round(4);

            Assert.AreEqual(Math.Round(1.0 / 3.0, 4), ab.Probability);
        }

        [TestMethod]
        public void NonExclusiveOr()
        {
            //The probability of rolling a even number on six sided dice.
            var a = new ProbabilityEvent1(1.0 / 2.0);
            //The probability of rolling a multiple of 3 on six sided dice.
            var b = new ProbabilityEvent1(1.0 / 3.0);
            //the probability of rolling a even number or nultiple of 3 (ie 2, 3, 4, 6).
            var ab = ProbabilityEvent1.NonExclusiveOr(a, b);
            ab.Round(4);

            Assert.AreEqual(Math.Round(2.0 / 3.0, 4), ab.Probability);
        }

        [TestMethod]
        public void ExplicitBayes()
        {
            //A hospital administers a test for a disease. 
            //Assuming these threee facts.
            //1. 2% of the population have the disease.
            //2. If a person does not have the disease the test has a 95% chance of giving negative.
            //3. If a person does have the disease the test has a 10% of giving negative.

            //If a patient tests positive what is the probability they actually have the disease?

            //probability of having the disease.
            var a = new ProbabilityEvent1(0.02);
            //probability of testing positive and having the disease.
            var za = new ProbabilityEvent1(0.95);
            //probability of testing positive and not having the disease.
            var z_not_a = new ProbabilityEvent1(0.1);

            //The probability of testing positive and having the disease.
            var az = ProbabilityEvent1.ExplicitBayes(a, za, z_not_a);
            az.Round(2);

            Assert.AreEqual(0.16, az.Probability);
        }

    }
}
