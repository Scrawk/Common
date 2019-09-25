using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class LogFuncTest
    {

        [TestMethod]
        public void Derivative()
        {
            var func = new LogFunc(1, 10);
            var derivtive = func.Derivative();
            Assert.AreEqual("1 / 2.30258509299405x", derivtive.ToString());

            func = new LogFunc(3, 2);
            derivtive = func.Derivative();
            Assert.AreEqual("3 / 0.693147180559945x", derivtive.ToString());

            func = new LogFunc(12);
            derivtive = func.Derivative();
            Assert.AreEqual("12 / x", derivtive.ToString());
        }

    }
}
