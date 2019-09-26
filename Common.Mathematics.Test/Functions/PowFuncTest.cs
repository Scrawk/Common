using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class PowFuncTest
    {

        [TestMethod]
        public new void ToString()
        {
            Assert.AreEqual("x", new PowFunc(1).ToString());
            Assert.AreEqual("x", new PowFunc(1, 1).ToString());
            Assert.AreEqual("-x", new PowFunc(-1, 1).ToString());
            Assert.AreEqual("x^2", new PowFunc(1, 2).ToString());
            Assert.AreEqual("x^-2", new PowFunc(1, -2).ToString());
            Assert.AreEqual("-x^-2", new PowFunc(-1, -2).ToString());
            Assert.AreEqual("2x^2", new PowFunc(2, 2).ToString());
            Assert.AreEqual("-2x^-2", new PowFunc(-2, -2).ToString());
        }

        [TestMethod]
        public void Derivative()
        {
            var func = new PowFunc(2, 4);
            var derivtive = func.Derivative();
            Assert.AreEqual("8x^3", derivtive.ToString());

            func = new PowFunc(1.0 / 12.0, 3);
            derivtive = func.Derivative();
            Assert.AreEqual("0.25x^2", derivtive.ToString());

            func = new PowFunc(10);
            derivtive = func.Derivative();
            Assert.AreEqual("10x^9", derivtive.ToString());

            func = new PowFunc(-3, 6);
            derivtive = func.Derivative();
            Assert.AreEqual("-18x^5", derivtive.ToString());

            func = new PowFunc(3, 2);
            derivtive = func.Derivative();
            Assert.AreEqual("6x", derivtive.ToString());

            func = new PowFunc(5, 1);
            derivtive = func.Derivative();
            Assert.AreEqual("5", derivtive.ToString());

            func = new PowFunc(8, -2);
            derivtive = func.Derivative();
            Assert.AreEqual("-16x^-3", derivtive.ToString());

            func = new PowFunc(0.5, -6);
            derivtive = func.Derivative();
            Assert.AreEqual("-3x^-7", derivtive.ToString());

            func = new PowFunc(-5, -3);
            derivtive = func.Derivative();
            Assert.AreEqual("15x^-4", derivtive.ToString());

            func = new PowFunc(-1);
            derivtive = func.Derivative();
            Assert.AreEqual("-x^-2", derivtive.ToString());

            func = new PowFunc(3, 0.5);
            derivtive = func.Derivative();
            Assert.AreEqual("1.5x^-0.5", derivtive.ToString());

            func = new PowFunc(1, 5.0/2.0);
            derivtive = func.Derivative();
            Assert.AreEqual("2.5x^1.5", derivtive.ToString());
        }

    }
}
