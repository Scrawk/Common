using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class LinearFuncTest
    {
        [TestMethod]
        public new void ToString()
        {
            Assert.AreEqual("x", new LinearFunc().ToString());
            Assert.AreEqual("x", new LinearFunc(1).ToString());
            Assert.AreEqual("-x", new LinearFunc(-1).ToString());
            Assert.AreEqual("3x", new LinearFunc(3).ToString());
            Assert.AreEqual("-4x", new LinearFunc(-4).ToString());
        }

        [TestMethod]
        public void Derivative()
        {
            var func = new LinearFunc(3);
            var derivative = func.Derivative();
            Assert.AreEqual("3", derivative.ToString());
        }
    }
}
