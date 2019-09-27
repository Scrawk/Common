using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class SinFuncTest
    {
        [TestMethod]
        public new void ToString()
        {
            Assert.AreEqual("sin(x)", new SinFunc().ToString());
            Assert.AreEqual("sin(x)", new SinFunc(1, 1).ToString());
            Assert.AreEqual("sin(-x)", new SinFunc(1, -1).ToString());
            Assert.AreEqual("-sin(x)", new SinFunc(-1, 1).ToString());
            Assert.AreEqual("-sin(-x)", new SinFunc(-1, -1).ToString());
            Assert.AreEqual("sin(2x)", new SinFunc(2).ToString());
            Assert.AreEqual("-sin(2x)", new SinFunc(-1, 2).ToString());
            Assert.AreEqual("-2sin(3x)", new SinFunc(-2, 3).ToString());
            Assert.AreEqual("-2sin(-x)", new SinFunc(-2, -1).ToString());
        }

        [TestMethod]
        public void Derivative()
        {
            var func = new SinFunc(0.25, 2);
            var derivative = func.Derivative();
            Assert.AreEqual("0.5cos(2x)", derivative.ToString());
        }

        [TestMethod]
        public void HigherOrderDerivative()
        {
            var func = new SinFunc(6, Math.PI);
            var derivtive0 = func.Derivative(0);
            var derivtive1 = func.Derivative(1);
            var derivtive2 = func.Derivative(2);
            var derivtive3 = func.Derivative(3);
            var derivtive4 = func.Derivative(4);

            Assert.AreEqual("6sin(PIx)", derivtive0.ToString());
            Assert.AreEqual("18.8495559215388cos(PIx)", derivtive1.ToString());
            Assert.AreEqual("-59.2176264065361sin(PIx)", derivtive2.ToString());
            Assert.AreEqual("-186.037660081799cos(PIx)", derivtive3.ToString());
            Assert.AreEqual("584.454546204014sin(PIx)", derivtive4.ToString());
        }
    }
}
