using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Probability;

namespace Common.Mathematics.Test.Probability
{
    [TestClass]
    public class Mathematics_Probability1iTest
    {
        [TestMethod]
        public void IndependantAnd()
        {
            //The probability of rolling a 2 on six sided dice.
            var a = new Probability1i(1.0 / 6.0);
            //The probability of rolling a 5 on six sided dice.
            var b = new Probability1i(1.0 / 6.0);
            //the probability of rolling a 2 and 5 on two seperate dice.
            var ab = Probability1i.IndependantAnd(a, b);
            ab.Round(4);

            Assert.AreEqual(Math.Round(1.0 / 36.0, 4), ab.Probability);
        }

        [TestMethod]
        public void DependantAnd()
        {
            //If there are 2 red and 3 blue balls in a box

            //The probability of picking a red ball
            var a = new Probability1i(2.0 / 5.0);
            //The probability of picking a blue ball if a red ball has been picked.
            var ab = new Probability1i(3.0 / 4.0);
            //the probability of rolling a 2 and 5 on two seperate dice.
            var b = Probability1i.DependantAnd(a, ab);
            b.Round(4);

            Assert.AreEqual(Math.Round(3.0 / 10.0, 4), Math.Round(b.Probability, 1));
        }

        [TestMethod]
        public void ExclusiveOr()
        {
            //The probability of rolling a 2 on six sided dice.
            var a = new Probability1i(1.0 / 6.0);
            //The probability of rolling a 5 on six sided dice.
            var b = new Probability1i(1.0 / 6.0);
            //the probability of rolling a 2 or 5 on the same roll
            var ab = Probability1i.ExclusiveOr(a, b);
            ab.Round(4);

            Assert.AreEqual(Math.Round(1.0 / 3.0, 4), ab.Probability);
        }

        [TestMethod]
        public void NonExclusiveOr()
        {
            //The probability of rolling a even number on six sided dice.
            var a = new Probability1i(1.0 / 2.0);
            //The probability of rolling a multiple of 3 on six sided dice.
            var b = new Probability1i(1.0 / 3.0);
            //the probability of rolling a even number or nultiple of 3 (ie 2, 3, 4, 6).
            var ab = Probability1i.NonExclusiveOr(a, b);
            ab.Round(4);

            Assert.AreEqual(Math.Round(2.0 / 3.0, 4), ab.Probability);
        }
    }
}
