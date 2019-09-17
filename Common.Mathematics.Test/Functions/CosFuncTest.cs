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
        public void Derivative()
        {
            var func = new CosFunc(2, 4);
            var derivative = func.Derivative();
            Assert.AreEqual("-8sin(4x)", derivative.ToString());
        }
    }
}
