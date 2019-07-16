using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Colors;

namespace Common.Core.Test.Colors
{
    [TestClass]
    public class ColorHSVTest
    {
        [TestMethod]
        public void AreEqual()
        {
            Assert.IsTrue(ColorHSV.White.Equals(new ColorHSV(0, 0, 1)));
        }

        [TestMethod]
        public void AreNotEqual()
        {
            Assert.IsTrue(ColorHSV.White != new ColorHSV(2, 2, 2));
        }

        [TestMethod]
        public void AreEqualWithError()
        {
            Assert.IsTrue(Random(0).EqualsWithError(Random(0), 1e-6f));
        }

        [TestMethod]
        public void Add()
        {
            Assert.AreEqual(ColorHSV.Green + ColorHSV.Green, new ColorHSV(240.0f / 360.0f, 2, 2));
        }

        [TestMethod]
        public void Sub()
        {
            Assert.AreEqual(ColorHSV.Green - ColorHSV.Green, new ColorHSV(0, 0, 0));
        }

        [TestMethod]
        public void Mul()
        {
            Assert.AreEqual(ColorHSV.Green * new ColorHSV(2, 2, 2), new ColorHSV(240.0f / 360.0f, 2, 2));
            Assert.AreEqual(ColorHSV.Green * 2, new ColorHSV(240.0f / 360.0f, 2, 2));
        }

        [TestMethod]
        public void Div()
        {
            Assert.AreEqual(ColorHSV.Green / new ColorHSV(2, 2, 2), new ColorHSV(60.0f / 360.0f, 0.5f, 0.5f));
            Assert.AreEqual(ColorHSV.Green / 2, new ColorHSV(60.0f / 360.0f, 0.5f, 0.5f));
        }


        [TestMethod]
        public void AccessedByIndex()
        {

            ColorHSV vd = new ColorHSV();
            vd[0] = 1;
            vd[1] = 2;
            vd[2] = 3;

            Assert.AreEqual(vd[0], 1);
            Assert.AreEqual(vd[1], 2);
            Assert.AreEqual(vd[2], 3);

        }

        [TestMethod]
        public void DefaultColors()
        {
            Assert.IsTrue(ColorHSV.Black.rgb == ColorRGB.Black);
            Assert.IsTrue(ColorHSV.White.rgb == ColorRGB.White);
            Assert.IsTrue(ColorHSV.Green.rgb == ColorRGB.Green);
            Assert.IsTrue(ColorHSV.Red.rgb == ColorRGB.Red);
            Assert.IsTrue(ColorHSV.Blue.rgb == ColorRGB.Blue);
        }

        [TestMethod]
        public void ToHSV()
        {

            for (int i = 0; i < 10; i++)
            {
                ColorHSV hsv = Random(i);

                ColorRGB rgb = hsv.rgb;

                ColorHSV col = rgb.hsv;

                Assert.IsTrue(hsv.EqualsWithError(col, 1e-6f));
            }
        }

        ColorHSV Random(int seed)
        {
            Random rnd = new Random(seed);
            return new ColorHSV((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
        }
    }
}
