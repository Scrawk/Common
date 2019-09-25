using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class PoductFuncTest
    {
        [TestMethod]
        public void Derivative()
        {
            Function func1 = new LinearFunc(4);
            Function func2 = new SinFunc(2);
            Function func = new ProductFunc(func1, func2);
            Function derivative = func.Derivative();
            Assert.AreEqual("4 * sin(2x) + 4x * 2cos(2x)", derivative.ToString());

            func1 = new LinearFunc(3);
            func2 = new ConstFunc(0);
            func = new ProductFunc(func1, func2);
            derivative = func.Derivative();
            Assert.AreEqual("0", derivative.ToString());

            func1 = new PowFunc(2);
            func2 = new LogFunc();
            func = new ProductFunc(func1, func2);
            derivative = func.Derivative();
            Assert.AreEqual("2x * ln(x) + x^2 * (1 / x)", derivative.ToString());

            func1 = new PowFunc(3);
            func2 = new SinFunc(Math.PI);
            func = new ProductFunc(func1, func2);
            derivative = func.Derivative();
            Assert.AreEqual("3x^2 * sin(PIx) + x^3 * PIcos(PIx)", derivative.ToString());

            func1 = new ConstFunc(6);
            func2 = new PowFunc(2);
            Function func3 = new ExpFunc(5);
            func = new ProductFunc(func1, func2, func3);
            derivative = func.Derivative();
            Assert.AreEqual("6 * 2x * e^5x + 6 * x^2 * 5e^5x", derivative.ToString());
        }
    }
}
