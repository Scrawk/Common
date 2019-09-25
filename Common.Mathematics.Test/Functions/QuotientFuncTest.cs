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
        public void Derivative()
        {
            Function func1 = new PowFunc(2);
            Function func2 = new ConstFunc(1);
            Function func3 = new LinearFunc(5);
            Function func = new QuotientFunc(func1, new SumFunc(func2, func3));
            Function derivative = func.Derivative();

            Assert.AreEqual("(2x * (1 + 5x) - x^2 * 5) / ((1 + 5x) * (1 + 5x))", derivative.ToString());

            func1 = new ExpFunc(-0.4);
            func2 = new LinearFunc(1);
            func3 = new PowFunc(2);
            func = new QuotientFunc(func1, new SumFunc(func2, func3));
            derivative = func.Derivative();

            Assert.AreEqual("(-0.4e^-0.4x * (x + x^2) - e^-0.4x * (1 + 2x)) / ((x + x^2) * (x + x^2))", derivative.ToString());

            func1 = new SinFunc(Math.PI);
            func2 = new PowFunc(2);
            func = new QuotientFunc(func1, func2);
            derivative = func.Derivative();

            Assert.AreEqual("(PIcos(PIx) * x^2 - sin(PIx) * 2x) / (x^2 * x^2)", derivative.ToString());

        }
    }
}
