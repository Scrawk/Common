using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Colors;

namespace Common.Core.Test.Colors
{
    [TestClass]
    public class ColorRGBTest
    {
        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(ColorRGB.White.Equals(new ColorRGB(1, 1, 1)));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(ColorRGB.White != new ColorRGB(2, 2, 2));
        }

        [TestMethod]
        public void AreEqualWithError()
        {
            Assert.IsTrue(ColorRGB.AlmostEqual(Random(0), Random(0), 1e-6f));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(ColorRGB.White + ColorRGB.White, new ColorRGB(2, 2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(ColorRGB.White - ColorRGB.White, new ColorRGB(0, 0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(ColorRGB.White * new ColorRGB(2, 2, 2), new ColorRGB(2, 2, 2));
            Assert.AreEqual(ColorRGB.White * 2, new ColorRGB(2, 2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(ColorRGB.White / new ColorRGB(2, 2, 2), new ColorRGB(0.5f, 0.5f, 0.5f));
            Assert.AreEqual(ColorRGB.White / 2, new ColorRGB(0.5f, 0.5f, 0.5f));
        }


        [TestMethod]
        public void AccessedByIndex()
        {

            ColorRGB vd = new ColorRGB();
            vd[0] = 1;
            vd[1] = 2;
            vd[2] = 3;

            Assert.AreEqual(vd[0], 1);
            Assert.AreEqual(vd[1], 2);
            Assert.AreEqual(vd[2], 3);

        }

        [TestMethod]
        public void Min()
        {
            ColorRGB vd = ColorRGB.White;
            vd = ColorRGB.Min(vd, 0.5f);

            Assert.AreEqual(vd, new ColorRGB(0.5f, 0.5f, 0.5f));
        }

        [TestMethod]
        public void Max()
        {
            ColorRGB vd = ColorRGB.White;
            vd = ColorRGB.Max(vd, 1.5f);

            Assert.AreEqual(vd, new ColorRGB(1.5f, 1.5f, 1.5f));
        }

        [TestMethod]
        public void Clamp()
        {
            ColorRGB vd = new ColorRGB(0.4f, 1.6f, 0.1f);
            vd = ColorRGB.Clamp(vd, 0.5f, 1.5f);

            Assert.AreEqual(vd, new ColorRGB(0.5f, 1.5f, 0.5f));
        }

        [TestMethod]
        public void ToHSV()
        {

            for (int i = 0; i < 10; i++)
            {
                ColorRGB rgb = Random(i);

                ColorHSV hsv = rgb.hsv;

                ColorRGB col = hsv.rgb;

                Assert.IsTrue(ColorRGB.AlmostEqual(rgb, col, 1e-6f));
            }
        }

        ColorRGB Random(int seed)
        {
            Random rnd = new Random(seed);
            return new ColorRGB((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
        }
    }
}
