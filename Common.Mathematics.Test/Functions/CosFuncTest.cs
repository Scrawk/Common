using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class CosFuncTest
    {
        [TestMethod]
        public new void ToString()
        {
            Assert.AreEqual("cos(x)", new CosFunc().ToString());
            Assert.AreEqual("cos(x)", new CosFunc(1, 1).ToString());
            Assert.AreEqual("cos(-x)", new CosFunc(1, -1).ToString());
            Assert.AreEqual("-cos(x)", new CosFunc(-1, 1).ToString());
            Assert.AreEqual("-cos(-x)", new CosFunc(-1, -1).ToString());
            Assert.AreEqual("cos(2x)", new CosFunc(2).ToString());
            Assert.AreEqual("-cos(2x)", new CosFunc(-1, 2).ToString());
            Assert.AreEqual("-2cos(3x)", new CosFunc(-2, 3).ToString());
            Assert.AreEqual("-2cos(-x)", new CosFunc(-2, -1).ToString());
        }

        [TestMethod]
        public void Derivative()
        {
            var func = new CosFunc(2, 4);
            var derivative = func.Derivative();
            Assert.AreEqual("-8sin(4x)", derivative.ToString());
        }
    }
}
