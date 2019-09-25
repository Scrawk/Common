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
            Assert.AreEqual("2x * g(3cos(x), h(x^2))", derivative.ToString());

            Function pow = new PowFunc(3);
            Function lin = new LinearFunc(-1);
            Function con = new ConstFunc(1);
            h = new SumFunc(pow, lin, con);
            func = new ChainFunc(new PowFunc(5), h);
            derivative = func.Derivative();
            Assert.AreEqual("(3x^2 + -1) * g(5x^4, h(x^3 + -1x + 1))", derivative.ToString());

            pow = new PowFunc(2);
            con = new ConstFunc(1);
            h = new SubFunc(pow, con);
            func = new ChainFunc(new LogFunc(), h);
            derivative = func.Derivative();
            Assert.AreEqual("2x * g((1 / x), h(x^2 - 1))", derivative.ToString());

            Function sin = new SinFunc(3);
            func = new ChainFunc(new PowFunc(0.5), sin);
            derivative = func.Derivative();
            Assert.AreEqual("3cos(3x) * g(0.5x^-0.5, h(sin(3x)))", derivative.ToString());

        }
    }
}
