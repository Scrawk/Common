using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class FunctionTest
    {

        [TestMethod]
        public void TangentAndNormal()
        {
            Function func, tangent, normal;

            func = new PowFunc(3);
            tangent = func.Tangent(1);
            normal = func.Normal(1);
            Assert.AreEqual("3 * (x - 1) + 1", tangent.ToString());
            Assert.AreEqual("(-1 / 3) * (x - 1) + 1", normal.ToString());

            func = new ChainFunc(new LogFunc(), new LinearFunc(0.5));
            tangent = func.Tangent(2);
            normal = func.Normal(2);

            Assert.AreEqual("0.5 * (x - 2)", tangent.ToString());
            Assert.AreEqual("(-1 / 0.5) * (x - 2)", normal.ToString());
        }

        [TestMethod]
        public void Integrate()
        {
            Function func, func1, func2;

            func = new PowFunc(2);
            Assert.AreEqual(0.33, Math.Round(func.Integrate(0, 1), 2));

            func = new PowFunc(15, 4);
            Assert.AreEqual(6, func.Integrate(-1, 1));

            func = new PowFunc(4, -2);
            Assert.AreEqual(2, func.Integrate(1, 2));

            func1 = new LinearFunc(2);
            func2 = new PowFunc(4, -1);
            func = new SumFunc(func1, func2);
            Assert.AreEqual(2.87, Math.Round(func.Integrate(1, 1.5), 2));

            func1 = new PowFunc(-2);
            func2 = new ExpFunc(4, -2);
            func = new SumFunc(func1, func2);
            Assert.AreEqual(1.07, Math.Round(func.Integrate(1, 5), 2));

            func1 = new PowFunc(0.5);
            func2 = new PowFunc(1.0 / 5.0, 2);
            func = new SumFunc(func1, func2);
            Assert.AreEqual(0.73, Math.Round(func.Integrate(0, 1), 2));

            func1 = new SinFunc(8, 2);
            func2 = new ConstFunc(1.0);
            func = new SumFunc(func1, func2);
            Assert.AreEqual(4.79, Math.Round(func.Integrate(Math.PI / 4, Math.PI / 2), 2));
        }
    }
}
