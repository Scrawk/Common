using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Colors;

namespace Common.Core.Test.Colors
{
    [TestClass]
    public class ColorRGBATest
    {
        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(ColorRGBA.White.Equals(new ColorRGBA(1, 1, 1, 1)));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(ColorRGBA.White != new ColorRGBA(2, 2, 2, 2));
        }

        [TestMethod]
        public void AreEqualWithError()
        {
            Assert.IsTrue(ColorRGBA.AlmostEqual(Random(0), Random(0), 1e-6f));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(ColorRGBA.White + ColorRGBA.White, new ColorRGBA(2, 2, 2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(ColorRGBA.White - ColorRGBA.White, new ColorRGBA(0, 0, 0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(ColorRGBA.White * new ColorRGBA(2, 2, 2, 2), new ColorRGBA(2, 2, 2, 2));
            Assert.AreEqual(ColorRGBA.White * 2, new ColorRGBA(2, 2, 2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(ColorRGBA.White / new ColorRGBA(2, 2, 2, 2), new ColorRGBA(0.5f, 0.5f, 0.5f, 0.5f));
            Assert.AreEqual(ColorRGBA.White / 2, new ColorRGBA(0.5f, 0.5f, 0.5f, 0.5f));
        }


        [TestMethod]
        public void AccessedByIndex()
        {

            ColorRGBA vd = new ColorRGBA();
            vd[0] = 1;
            vd[1] = 2;
            vd[2] = 3;
            vd[3] = 4;

            Assert.AreEqual(vd[0], 1);
            Assert.AreEqual(vd[1], 2);
            Assert.AreEqual(vd[2], 3);
            Assert.AreEqual(vd[3], 4);
        }

        [TestMethod]
        public void min()
        {
            ColorRGBA vd = ColorRGBA.White;
            vd = ColorRGBA.Min(vd, 0.5f);

            Assert.AreEqual(vd, new ColorRGBA(0.5f, 0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Max()
        {
            ColorRGBA vd = ColorRGBA.White;
            vd = ColorRGBA.Max(vd, 1.5f);

            Assert.AreEqual(vd, new ColorRGBA(1.5f, 1.5f, 1.5f, 1.5f));
        }

        [TestMethod]
        public void Clamp()
        {
            ColorRGBA vd = new ColorRGBA(0.4f, 1.6f, 0.1f, 1.7f);
            vd = ColorRGBA.Clamp(vd, 0.5f, 1.5f);

            Assert.AreEqual(vd, new ColorRGBA(0.5f, 1.5f, 0.5f, 1.5f));
        }

        ColorRGBA Random(int seed)
        {
            Random rnd = new RandomNum(seed);
            return new ColorRGBA((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
        }
    }
}
