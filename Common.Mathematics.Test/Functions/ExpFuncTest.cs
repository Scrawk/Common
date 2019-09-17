using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class ExpFuncTest
    {

        [TestMethod]
        public void Derivative()
        {
            var func = new ExpFunc(100, 0.1);
            var derivtive = func.Derivative();
            Assert.AreEqual("10e^0.1x", derivtive.ToString());

            func = new ExpFunc(4, -2);
            derivtive = func.Derivative();
            Assert.AreEqual("-8e^-2x", derivtive.ToString());
        }

    }
}
