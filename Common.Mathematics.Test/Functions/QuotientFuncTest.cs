using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class QuotientFuncTest
    {

        [TestMethod]
        public new void ToString()
        {
            Function func, func1, func2;

            func1 = new ConstFunc(0);
            func2 = new ConstFunc(0);
            func = new QuotientFunc(func1, func2);
            Assert.AreEqual("0 / 0", func.ToString());

            func1 = new ConstFunc(0);
            func2 = new ConstFunc(1);
            func = new QuotientFunc(func1, func2);
            Assert.AreEqual("0 / 1", func.ToString());

            func1 = new ConstFunc(1);
            func2 = new ConstFunc(1);
            func = new QuotientFunc(func1, func2);
            Assert.AreEqual("1 / 1", func.ToString());

            func1 = new ConstFunc(-1);
            func2 = new ConstFunc(-1);
            func = new QuotientFunc(func1, func2);
            Assert.AreEqual("-1 / -1", func.ToString());
        }

        [TestMethod]
        public void Derivative()
        {
            //example 1
            {
                Function func1 = new PowFunc(2);
                Function func2 = new ConstFunc(1);
                Function func3 = new LinearFunc(5);
                Function func = new QuotientFunc(func1, new SumFunc(func2, func3));
                Function derivative = func.Derivative();
                Assert.AreEqual("(2x * (1 + 5x) - x^2 * 5) / ((1 + 5x) * (1 + 5x))", derivative.ToString());
            }

            //example 2
            {
                Function func1 = new ExpFunc(-0.4);
                Function func2 = new LinearFunc(1);
                Function func3 = new PowFunc(2);
                Function func = new QuotientFunc(func1, new SumFunc(func2, func3));
                Function derivative = func.Derivative();
                Assert.AreEqual("(-0.4e^-0.4x * (x + x^2) - e^-0.4x * (1 + 2x)) / ((x + x^2) * (x + x^2))", derivative.ToString());
            }

            //example 3
            {
                Function func1 = new SinFunc(Math.PI);
                Function func2 = new PowFunc(2);
                Function func = new QuotientFunc(func1, func2);
                Function derivative = func.Derivative();
                Assert.AreEqual("(PIcos(PIx) * x^2 - sin(PIx) * 2x) / (x^2 * x^2)", derivative.ToString());
            }

            //example 3
            {
                Function func1 = new ConstFunc(1);
                Function func2 = new ConstFunc(1);
                Function func = new QuotientFunc(func1, func2);
                Function derivative = func.Derivative();
                Assert.AreEqual("0", derivative.ToString());
            }

            //example 3
            {
                Function func1 = new LinearFunc(1);
                Function func2 = new ConstFunc(1);
                Function func = new QuotientFunc(func1, func2);
                Function derivative = func.Derivative();
                Assert.AreEqual("1", derivative.ToString());
            }

        }
    }
}
