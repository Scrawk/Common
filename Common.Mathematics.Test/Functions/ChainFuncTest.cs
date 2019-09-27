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
        public new void ToString()
        {
            Function chain, g, h;

            g = new ConstFunc(2);
            h = new ConstFunc(3);
            chain = new ChainFunc(g, h);
            Assert.AreEqual("2", chain.ToString());

            g = new LinearFunc(2);
            h = new ConstFunc(3);
            chain = new ChainFunc(g, h);
            Assert.AreEqual("2(3)", chain.ToString());

            g = new LinearFunc(2);
            h = new LinearFunc(1);
            chain = new ChainFunc(g, h);
            Assert.AreEqual("2x", chain.ToString());

            g = new LinearFunc(1);
            h = new LinearFunc(1);
            chain = new ChainFunc(g, h);
            Assert.AreEqual("x", chain.ToString());

            g = new LinearFunc(2);
            h = new LinearFunc(3);
            chain = new ChainFunc(g, h);
            Assert.AreEqual("2(3x)", chain.ToString());

            g = new PowFunc(2);
            h = new LinearFunc(3);
            chain = new ChainFunc(g, h);
            Assert.AreEqual("(3x)^2", chain.ToString());

            g = new PowFunc(2);
            h = new ProductFunc(new ConstFunc(3), new ConstFunc(4));
            chain = new ChainFunc(g, h);
            Assert.AreEqual("(3 * 4)^2", chain.ToString());
        }


        [TestMethod]
        public void Derivative()
        {
            //example 1
            {
                var h = new PowFunc(2);
                var g = new SinFunc(3, 1);
                var func = new ChainFunc(g, h);
                var derivative = func.Derivative();
                //Console.WriteLine(derivative.ToString());
                Assert.AreEqual("2x * 3cos(x^2)", derivative.ToString());
            }

            //example 2
            {
                var pow = new PowFunc(3);
                var lin = new LinearFunc(-1);
                var con = new ConstFunc(1);
                var h = new SumFunc(pow, lin, con);
                var func = new ChainFunc(new PowFunc(5), h);
                var derivative = func.Derivative();
                //Console.WriteLine(derivative.ToString());
                Assert.AreEqual("(3x^2 - 1) * 5(x^3 - x + 1)^4", derivative.ToString());
            }

            //example 3
            {
                var pow = new PowFunc(2);
                var con = new ConstFunc(1);
                var h = new SubFunc(pow, con);
                var func = new ChainFunc(new LogFunc(), h);
                var derivative = func.Derivative();
                //Console.WriteLine(derivative.ToString());
                Assert.AreEqual("2x * (1 / (x^2 - 1))", derivative.ToString());
            }

            //example 4
            {
                var sin = new SinFunc(3);
                var func = new ChainFunc(new PowFunc(0.5), sin);
                var derivative = func.Derivative();
                //Console.WriteLine(derivative.ToString());
                Assert.AreEqual("3cos(3x) * 0.5(sin(3x))^-0.5", derivative.ToString());
            }

        }
    }
}
