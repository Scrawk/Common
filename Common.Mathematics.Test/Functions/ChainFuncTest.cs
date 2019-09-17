using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class ChainFuncTest
    {
        [TestMethod]
        public void Derivative()
        {

            Function h = new PowFunc(2);
            Function g = new SinFunc(3, 1);
            Function func = new ChainFunc(g, h);
            var derivative = func.Derivative();
            Assert.AreEqual("2x * 3cos(x)(x^2)", derivative.ToString());

        }
    }
}
