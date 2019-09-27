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
        public new void ToString()
        {
            Assert.AreEqual("cos(x)", new CosFunc().ToString());
            Assert.AreEqual("cos(x)", new CosFunc(1, 1).ToString());
            Assert.AreEqual("cos(-x)", new CosFunc(1, -1).ToString());
            Assert.AreEqual("-cos(x)", new CosFunc(-1, 1).ToString());
            Assert.AreEqual("-cos(-x)", new CosFunc(-1, -1).ToString());
            Assert.AreEqual("cos(2x)", new CosFunc(2).ToString());
            Assert.AreEqual("-cos(2x)", new CosFunc(-1, 2).ToString());
            Assert.AreEqual("-2cos(3x)", new CosFunc(-2, 3).ToString());
            Assert.AreEqual("-2cos(-x)", new CosFunc(-2, -1).ToString());
        }

        [TestMethod]
        public void Derivative()
        {
            var func = new CosFunc(2, 4);
            var derivative = func.Derivative();
            Assert.AreEqual("-8sin(4x)", derivative.ToString());
        }

        [TestMethod]
        public void AntiDerivative()
        {
            Function func, antiderivative;

            func = new CosFunc(5, Math.PI);
            antiderivative = func.AntiDerivative();
            Assert.AreEqual("1.59154943091895sin(PIx)", antiderivative.ToString());

            func = new SubFunc(new CosFunc(2, 1.0/3.0), new PowFunc(-2));
            antiderivative = func.AntiDerivative();
            Assert.AreEqual("6sin(0.333333333333333x) + x^-1", antiderivative.ToString());

            //Console.WriteLine(antiderivative);
        }
    }
}
