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
        public void Derivative()
        {
            var func = new TanFunc(3);
            var derivative = func.Derivative();
            Assert.AreEqual("3 / (cos(3x) * cos(3x))", derivative.ToString());
        }
    }
}
