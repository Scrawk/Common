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
    }
}
