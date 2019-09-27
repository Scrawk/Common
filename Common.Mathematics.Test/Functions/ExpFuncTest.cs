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
        public new void ToString()
        {
            Assert.AreEqual("e^x", new ExpFunc().ToString());
            Assert.AreEqual("e^x", new ExpFunc(1,1).ToString());
            Assert.AreEqual("-e^x", new ExpFunc(-1,1).ToString());
            Assert.AreEqual("e^-x", new ExpFunc(1,-1).ToString());
            Assert.AreEqual("e^2x", new ExpFunc(2).ToString());
            Assert.AreEqual("2e^3x", new ExpFunc(2, 3).ToString());
            Assert.AreEqual("-2e^-3x", new ExpFunc(-2, -3).ToString());
        }

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

        [TestMethod]
        public void HigherOrderDerivative()
        {
            var func = new ExpFunc(100, 0.1);
            var derivtive0 = func.Derivative(0);
            var derivtive1 = func.Derivative(1);
            var derivtive2 = func.Derivative(2);
            var derivtive3 = func.Derivative(3);
            var derivtive4 = func.Derivative(4);

            Assert.AreEqual("100e^0.1x", derivtive0.ToString());
            Assert.AreEqual("10e^0.1x", derivtive1.ToString());
            Assert.AreEqual("e^0.1x", derivtive2.ToString());
            Assert.AreEqual("0.1e^0.1x", derivtive3.ToString());
            Assert.AreEqual("0.01e^0.1x", derivtive4.ToString());
        }

    }
}
