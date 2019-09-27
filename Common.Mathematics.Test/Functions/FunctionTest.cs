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
    }
}
