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
            //Console.WriteLine(derivative.ToString());
            Assert.AreEqual("2x * 3cos(x^2)", derivative.ToString());

            Function pow = new PowFunc(3);
            Function lin = new LinearFunc(-1);
            Function con = new ConstFunc(1);
            h = new SumFunc(pow, lin, con);
            func = new ChainFunc(new PowFunc(5), h);
            derivative = func.Derivative();
            //Console.WriteLine(derivative.ToString());
            Assert.AreEqual("(3x^2 + -1) * 5(x^3 + -1x + 1)^4", derivative.ToString());

            pow = new PowFunc(2);
            con = new ConstFunc(1);
            h = new SubFunc(pow, con);
            func = new ChainFunc(new LogFunc(), h);
            derivative = func.Derivative();
            //Console.WriteLine(derivative.ToString());
            Assert.AreEqual("2x * (1 / (x^2 - 1))", derivative.ToString());

            Function sin = new SinFunc(3);
            func = new ChainFunc(new PowFunc(0.5), sin);
            derivative = func.Derivative();
            //Console.WriteLine(derivative.ToString());
            Assert.AreEqual("3cos(3x) * 0.5sin(3x)^-0.5", derivative.ToString());

        }
    }
}
