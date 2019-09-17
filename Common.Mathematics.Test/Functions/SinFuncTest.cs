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
        public void Derivative()
        {
            var func = new SinFunc(0.25, 2);
            var derivative = func.Derivative();
            Assert.AreEqual("0.5cos(2x)", derivative.ToString());
        }
    }
}
