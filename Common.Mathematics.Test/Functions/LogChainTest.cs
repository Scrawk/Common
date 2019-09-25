using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class LogChainFuncTest
    {
        [TestMethod]
        public void Constuctor()
        {
            Function con = new ConstFunc(3);
            Function lin = new LinearFunc(1);
            ProductFunc prod = new ProductFunc(con, lin);
            var func = new LogChainFunc(prod);
            Assert.AreEqual("ln(3) + ln(x)", func.ToString());

            con = new ConstFunc(5);
            Function pow = new PowFunc(3);
            prod = new ProductFunc(con, pow);
            func = new LogChainFunc(prod);
            Assert.AreEqual("ln(5) + 3 * ln(x)", func.ToString());

            pow = new PowFunc(8, 2);
            Function sum = new SumFunc(new ConstFunc(1), new PowFunc(2));
            QuotientFunc quot = new QuotientFunc(pow, sum);
            func = new LogChainFunc(quot);
            Assert.AreEqual("ln(8) + 2 * ln(x) - ln(1 + x^2)", func.ToString());

            pow = new PowFunc(4);
            Function chain1 = new ChainFunc(new PowFunc(5), new SubFunc(new LinearFunc(), new ConstFunc(2)));
            Function chain2 = new ChainFunc(new PowFunc(2), new SumFunc(new LinearFunc(), new ConstFunc(1)));
            prod = new ProductFunc(pow, chain1, chain2);
            func = new LogChainFunc(prod);
            Console.WriteLine(func);
           //ssert.AreEqual("ln(8) + 2 * ln(x) - ln(1 + x^2)", func.ToString());
        }

        [TestMethod]
        public void Derivative()
        {

        }
    }
}
