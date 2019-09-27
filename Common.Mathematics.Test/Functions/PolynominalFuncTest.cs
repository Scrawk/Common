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

        [TestMethod]
        public void HigherOrderDerivative()
        {
            var func = new PolynominalFunc(1.0/5.0, 1, 1, -12);
            var derivtive0 = func.Derivative(0);
            var derivtive1 = func.Derivative(1);
            var derivtive2 = func.Derivative(2);
            var derivtive3 = func.Derivative(3);
            var derivtive4 = func.Derivative(4);
            var derivtive5 = func.Derivative(5);

            Assert.AreEqual("0.2x^3 + x^2 + x - 12", derivtive0.ToString());
            Assert.AreEqual("0.6x^2 + 2x + 1", derivtive1.ToString());
            Assert.AreEqual("1.2x + 2", derivtive2.ToString());
            Assert.AreEqual("1.2", derivtive3.ToString());
            Assert.AreEqual("0", derivtive4.ToString());
            Assert.AreEqual("0", derivtive5.ToString());
        }

    }
}
