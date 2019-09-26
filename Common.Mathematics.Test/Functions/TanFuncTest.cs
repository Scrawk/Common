using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class TanFuncTest
    {
        [TestMethod]
        public new void ToString()
        {
            Assert.AreEqual("tan(x)", new TanFunc().ToString());
            Assert.AreEqual("tan(x)", new TanFunc(1, 1).ToString());
            Assert.AreEqual("tan(-x)", new TanFunc(1, -1).ToString());
            Assert.AreEqual("-tan(x)", new TanFunc(-1, 1).ToString());
            Assert.AreEqual("-tan(-x)", new TanFunc(-1, -1).ToString());
            Assert.AreEqual("tan(2x)", new TanFunc(2).ToString());
            Assert.AreEqual("-tan(2x)", new TanFunc(-1, 2).ToString());
            Assert.AreEqual("-2tan(3x)", new TanFunc(-2, 3).ToString());
            Assert.AreEqual("-2tan(-x)", new TanFunc(-2, -1).ToString());
        }

        [TestMethod]
        public void Derivative()
        {
            var func = new TanFunc(3);
            var derivative = func.Derivative();
            Assert.AreEqual("3 / (cos(3x) * cos(3x))", derivative.ToString());
        }
    }
}
