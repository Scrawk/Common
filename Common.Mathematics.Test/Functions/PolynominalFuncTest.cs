using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class PolynominalFuncTest
    {

        [TestMethod]
        public void Derivative()
        {
            var func = new PolynominalFunc(2, -1, 0, 0, 2);
            var derivtive = func.Derivative();
            Assert.AreEqual("8x^3 - 3x^2", derivtive.ToString());

            func = new PolynominalFunc(0.5, 3, -8);
            derivtive = func.Derivative();
            Assert.AreEqual("x + 3", derivtive.ToString());

            func = new PolynominalFunc(new double[] {-1, 3, 0, 0, -2, 1});
            derivtive = func.Derivative();
            Assert.AreEqual("-5x^4 + 12x^3 - 2", derivtive.ToString());
        }

    }
}
