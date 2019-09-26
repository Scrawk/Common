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
            //example 1
            {
                var con = new ConstFunc(3);
                var lin = new LinearFunc(1);
                var prod = new ProductFunc(con, lin);
                var func = new LogChainFunc(prod);
                //Console.WriteLine(func);
                Assert.AreEqual("ln(3) + ln(x)", func.ToString());
            }

            //example 2
            {
                var con = new ConstFunc(5);
                var pow = new PowFunc(3);
                var prod = new ProductFunc(con, pow);
                var func = new LogChainFunc(prod);
                //onsole.WriteLine(func);
                Assert.AreEqual("ln(5) + 3 * ln(x)", func.ToString());
            }

            //example 3
            {
                var pow = new PowFunc(8, 2);
                var sum = new SumFunc(new ConstFunc(1), new PowFunc(2));
                var quot = new QuotientFunc(pow, sum);
                var func = new LogChainFunc(quot);
                //Console.WriteLine(func);
                Assert.AreEqual("ln(8) + 2 * ln(x) - ln(1 + x^2)", func.ToString());
            }

            //example 4
            {
                var pow = new PowFunc(4);
                var chain1 = new ChainFunc(new PowFunc(5), new SubFunc(new LinearFunc(), new ConstFunc(2)));
                var chain2 = new ChainFunc(new PowFunc(2), new SumFunc(new LinearFunc(), new ConstFunc(1)));
                var prod = new ProductFunc(pow, chain1, chain2);
                var func = new LogChainFunc(prod);
                //Console.WriteLine(func);
                Assert.AreEqual("4 * ln(x) + 5 * ln(x - 2) + 2 * ln(x + 1)", func.ToString());
            }

            //example 5
            {
                var pow = new PowFunc(-2);
                var exp = new ExpFunc(0.5);
                var sub1 = new SubFunc(new LinearFunc(), new ConstFunc(1));
                var sub2 = new SubFunc(new PowFunc(2), new ConstFunc(1));
                var prod1 = new ProductFunc(pow, exp);
                var prod2 = new ProductFunc(sub1, sub2);
                var quot = new QuotientFunc(prod1, prod2);
                var func = new LogChainFunc(quot);
                //Console.WriteLine(func);
                Assert.AreEqual("-2 * ln(x) + 0.5x - ln(x - 1) - ln(x^2 - 1)", func.ToString());
            }

            //example 6
            {
                var pow = new PowFunc(5);
                var sum = new SumFunc(new LinearFunc(), new ConstFunc(4));
                var chain = new ChainFunc(new PowFunc(2), sum);
                var prod = new ProductFunc(pow, chain);
                var func = new LogChainFunc(prod);
                //Console.WriteLine(func);
                Assert.AreEqual("5 * ln(x) + 2 * ln(x + 4)", func.ToString());
            }

            //example 7
            {
                var sum = new SumFunc(new LinearFunc(2), new ConstFunc(3));
                var chain = new ChainFunc(new PowFunc(4), sum);
                var prod = new ProductFunc(chain, new ExpFunc(-3));
                var func = new LogChainFunc(prod);
                //Console.WriteLine(func);
                Assert.AreEqual("4 * ln(2x + 3) - 3x", func.ToString());
            }

            //example 8
            {
                var sum = new SumFunc(new LinearFunc(), new PowFunc(2));
                var chain1 = new ChainFunc(new PowFunc(6), sum);
                var chain2 = new ChainFunc(new PowFunc(3), new SinFunc());
                var prod = new ProductFunc(chain1, chain2);
                var func = new LogChainFunc(prod);
                //Console.WriteLine(func);
                Assert.AreEqual("6 * ln(x + x^2) + 3 * ln(sin(x))", func.ToString());
            }

        }

        [TestMethod]
        public void Derivative()
        {
            //example 1
            {
                var lin = new LinearFunc();
                var sub1 = new SubFunc(new LinearFunc(), new ConstFunc(1));
                var sub2 = new SubFunc(new PowFunc(2), new ConstFunc(1));
                var chain = new ChainFunc(new PowFunc(3), sub2);
                var prod = new ProductFunc(sub1, chain);
                var quot = new QuotientFunc(lin, prod);
                var func = new LogChainFunc(quot);
                var derivative = func.Derivative();

                //Console.WriteLine(func.Derivative());
                Assert.AreEqual("(x / ((x - 1) * (x^2 - 1)^3)) * (1 / x - 1 / (x - 1) - 3 * (2x * (1 / (x^2 - 1))))", derivative.ToString());
            }

            //example 2
            {
                var pow = new PowFunc(5);
                var prod = new ProductFunc(new ConstFunc(5), new PowFunc(3));
                var sum = new SumFunc(prod, new ConstFunc(2));
                var chain = new ChainFunc(new PowFunc(2), sum);
                var exp = new ExpFunc(8);
                var prod2 = new ProductFunc(pow, chain, exp);
                var func = new LogChainFunc(prod2);
                var derivative = func.Derivative();

                //Console.WriteLine(func.Derivative());
                Assert.AreEqual("(x^5 * (5 * x^3 + 2)^2 * e^8x) * (5 * (1 / x) + 2 * ((5 * 3x^2) * (1 / (5 * x^3 + 2))) + 8)", derivative.ToString());
            }
        }
    }
}
