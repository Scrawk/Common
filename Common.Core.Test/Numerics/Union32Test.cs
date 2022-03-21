using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;

namespace Common.Core.Test.Numerics
{
    [TestClass]
    public class Union32Test
    {
        [TestMethod]
        public void Constuction()
        {
            Union32 data;

            data = -1;
            Assert.AreEqual(-1, data.Int);
            data = 123456789;
            Assert.AreEqual(123456789, data.Int);

            data = uint.MaxValue;
            Assert.AreEqual(uint.MaxValue, data.UInt);

            data = 1.0f;
            Assert.AreEqual(1.0f, data.Float);
            data = 0.33333f;
            Assert.AreEqual(0.33333f, data.Float);
            data = float.PositiveInfinity;
            Assert.AreEqual(float.PositiveInfinity, data.Float);
            data = float.NegativeInfinity;
            Assert.AreEqual(float.NegativeInfinity, data.Float);
            data = float.NaN;
            Assert.AreEqual(float.NaN, data.Float);

        }
    }
}
