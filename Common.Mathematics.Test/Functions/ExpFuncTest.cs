using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class ExpFuncTest
    {

        [TestMethod]
        public new void ToString()
        {
            Assert.AreEqual("e^x", new ExpFunc().ToString());
            Assert.AreEqual("e^x", new ExpFunc(1,1).ToString());
            Assert.AreEqual("-e^x", new ExpFunc(-1,1).ToString());
            Assert.AreEqual("e^-x", new ExpFunc(1,-1).ToString());
            Assert.AreEqual("e^2x", new ExpFunc(2).ToString());
            Assert.AreEqual("2e^3x", new ExpFunc(2, 3).ToString());
            Assert.AreEqual("-2e^-3x", new ExpFunc(-2, -3).ToString());
        }

        [TestMethod]
        public void Derivative()
        {
            var func = new ExpFunc(100, 0.1);
            var derivtive = func.Derivative();
            Assert.AreEqual("10e^0.1x", derivtive.ToString());

            func = new ExpFunc(4, -2);
            derivtive = func.Derivative();
            Assert.AreEqual("-8e^-2x", derivtive.ToString());
        }

        [TestMethod]
        public void HigherOrderDerivative()
        {
            var func = new ExpFunc(100, 0.1);
            var derivtive0 = func.Derivative(0);
            var derivtive1 = func.Derivative(1);
            var derivtive2 = func.Derivative(2);
            var derivtive3 = func.Derivative(3);
            var derivtive4 = func.Derivative(4);

            Assert.AreEqual("100e^0.1x", derivtive0.ToString());
            Assert.AreEqual("10e^0.1x", derivtive1.ToString());
            Assert.AreEqual("e^0.1x", derivtive2.ToString());
            Assert.AreEqual("0.1e^0.1x", derivtive3.ToString());
            Assert.AreEqual("0.01e^0.1x", derivtive4.ToString());
        }

        [TestMethod]
        public void AntiDerivative()
        {
            Function func, antiderivtive;

            func = new ExpFunc(10, 2);
            antiderivtive = func.AntiDerivative();
            Assert.AreEqual("5e^2x", antiderivtive.ToString());

            func = new ExpFunc(0.25, -5);
            antiderivtive = func.AntiDerivative();
            Assert.AreEqual("-0.05e^-5x", antiderivtive.ToString());

            func = new SumFunc(new LinearFunc(), new ExpFunc());
            antiderivtive = func.AntiDerivative();
            Assert.AreEqual("0.5x^2 + e^x", antiderivtive.ToString());

        }

        [TestMethod]
        public void Base()
        {
            var func = new ExpFunc(0.2, 0.05);
            Assert.AreEqual(1.05, Math.Round(func.b, 2));

            func = ExpFunc.FromBase(15, 0.86);
            Assert.AreEqual(15, Math.Round(func.a, 2));
            Assert.AreEqual(-0.15, Math.Round(func.c, 2));
        }

        [TestMethod]
        public void Life()
        {
            var func = ExpFunc.FromBase(10000, 1.04);
            Assert.AreEqual(17.67, Math.Round(func.Life(2), 2));

            func = ExpFunc.FromBase(10, 0.88);
            Assert.AreEqual(5.42, Math.Round(func.Life(2), 2));
        }

    }
}
