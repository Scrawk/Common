using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class PowFuncTest
    {

        [TestMethod]
        public new void ToString()
        {
            Assert.AreEqual("x", new PowFunc(1).ToString());
            Assert.AreEqual("x", new PowFunc(1, 1).ToString());
            Assert.AreEqual("-x", new PowFunc(-1, 1).ToString());
            Assert.AreEqual("x^2", new PowFunc(1, 2).ToString());
            Assert.AreEqual("x^-2", new PowFunc(1, -2).ToString());
            Assert.AreEqual("-x^-2", new PowFunc(-1, -2).ToString());
            Assert.AreEqual("2x^2", new PowFunc(2, 2).ToString());
            Assert.AreEqual("-2x^-2", new PowFunc(-2, -2).ToString());
        }

        [TestMethod]
        public void Derivative()
        {
            var func = new PowFunc(2, 4);
            var derivtive = func.Derivative();
            Assert.AreEqual("8x^3", derivtive.ToString());

            func = new PowFunc(1.0 / 12.0, 3);
            derivtive = func.Derivative();
            Assert.AreEqual("0.25x^2", derivtive.ToString());

            func = new PowFunc(10);
            derivtive = func.Derivative();
            Assert.AreEqual("10x^9", derivtive.ToString());

            func = new PowFunc(-3, 6);
            derivtive = func.Derivative();
            Assert.AreEqual("-18x^5", derivtive.ToString());

            func = new PowFunc(3, 2);
            derivtive = func.Derivative();
            Assert.AreEqual("6x", derivtive.ToString());

            func = new PowFunc(5, 1);
            derivtive = func.Derivative();
            Assert.AreEqual("5", derivtive.ToString());

            func = new PowFunc(8, -2);
            derivtive = func.Derivative();
            Assert.AreEqual("-16x^-3", derivtive.ToString());

            func = new PowFunc(0.5, -6);
            derivtive = func.Derivative();
            Assert.AreEqual("-3x^-7", derivtive.ToString());

            func = new PowFunc(-5, -3);
            derivtive = func.Derivative();
            Assert.AreEqual("15x^-4", derivtive.ToString());

            func = new PowFunc(-1);
            derivtive = func.Derivative();
            Assert.AreEqual("-x^-2", derivtive.ToString());

            func = new PowFunc(3, 0.5);
            derivtive = func.Derivative();
            Assert.AreEqual("1.5x^-0.5", derivtive.ToString());

            func = new PowFunc(1, 5.0 / 2.0);
            derivtive = func.Derivative();
            Assert.AreEqual("2.5x^1.5", derivtive.ToString());
        }

        [TestMethod]
        public void HigherOrderDerivative()
        {
            var func = new PowFunc(2, 4);
            var derivtive0 = func.Derivative(0);
            var derivtive1 = func.Derivative(1);
            var derivtive2 = func.Derivative(2);
            var derivtive3 = func.Derivative(3);
            var derivtive4 = func.Derivative(4);
            var derivtive5 = func.Derivative(5);
            var derivtive6 = func.Derivative(6);

            Assert.AreEqual("2x^4", derivtive0.ToString());
            Assert.AreEqual("8x^3", derivtive1.ToString());
            Assert.AreEqual("24x^2", derivtive2.ToString());
            Assert.AreEqual("48x", derivtive3.ToString());
            Assert.AreEqual("48", derivtive4.ToString());
            Assert.AreEqual("0", derivtive5.ToString());
            Assert.AreEqual("0", derivtive6.ToString());
        }

        [TestMethod]
        public void AntiDerivative()
        {
            var func = new PowFunc(10, 4);
            var antiderivative = func.AntiDerivative();
            Assert.AreEqual("2x^5", antiderivative.ToString());

            func = new PowFunc(0.12, 2);
            antiderivative = func.AntiDerivative();
            Assert.AreEqual("0.04x^3", antiderivative.ToString());

            func = new PowFunc(-7, 6);
            antiderivative = func.AntiDerivative();
            Assert.AreEqual("-x^7", antiderivative.ToString());

            func = new PowFunc(1.0/4.0, 1);
            antiderivative = func.AntiDerivative();
            Assert.AreEqual("0.125x^2", antiderivative.ToString());

            func = new PowFunc(-5);
            antiderivative = func.AntiDerivative();
            Assert.AreEqual("-0.25x^-4", antiderivative.ToString());

            func = new PowFunc(-2);
            antiderivative = func.AntiDerivative();
            Assert.AreEqual("-x^-1", antiderivative.ToString());

            func = new PowFunc(0.5);
            antiderivative = func.AntiDerivative();
            Assert.AreEqual("0.666666666666667x^1.5", antiderivative.ToString());

            func = new PowFunc(9, 0.5);
            antiderivative = func.AntiDerivative();
            Assert.AreEqual("6x^1.5", antiderivative.ToString());

            func = new PowFunc(4, -1);
            antiderivative = func.AntiDerivative();
            Assert.AreEqual("4ln(x)", antiderivative.ToString());
        }

    }
}
