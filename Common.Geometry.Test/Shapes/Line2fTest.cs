using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Test.Shapes
{
    [TestClass]
    public class Line2fTest
    {
        [TestMethod]
        public void AreEquivalent()
        {
            //Vertical cases.............................

            var ab = new Line2f((0, 0), (0, 1));
            var cd = new Line2f((0, 0), (0, 1));

            Assert.IsTrue(ab.AreEquivalent(cd));

            ab = new Line2f((0, 0), (0, 1));
            cd = new Line2f((0, 0), (0, -1));

            Assert.IsTrue(ab.AreEquivalent(cd));

            ab = new Line2f((1, 0), (1, 1));
            cd = new Line2f((0, 0), (0, 1));

            Assert.IsFalse(ab.AreEquivalent(cd));

            ab = new Line2f((0, 0), (0, 1));
            cd = new Line2f((0, 0), (1, 0));

            Assert.IsFalse(ab.AreEquivalent(cd));

            ab = new Line2f((0, 0), (0, 1000));
            cd = new Line2f((0, 0), (0, 1));

            Assert.IsTrue(ab.AreEquivalent(cd));

            //Horizontal cases.............................

            ab = new Line2f((0, 0), (1, 0));
            cd = new Line2f((0, 0), (1, 0));

            Assert.IsTrue(ab.AreEquivalent(cd));

            ab = new Line2f((0, 0), (1, 0));
            cd = new Line2f((0, 0), (-1, 0));

            Assert.IsTrue(ab.AreEquivalent(cd));

            ab = new Line2f((0, 1), (1, 1));
            cd = new Line2f((0, 0), (-1, 0));

            Assert.IsFalse(ab.AreEquivalent(cd));

            ab = new Line2f((0, 0), (1, 0));
            cd = new Line2f((0, 0), (0, 1));

            Assert.IsFalse(ab.AreEquivalent(cd));

            ab = new Line2f((0, 0), (1000, 0));
            cd = new Line2f((0, 0), (1, 0));

            Assert.IsTrue(ab.AreEquivalent(cd));

            //Other cases.............................

            ab = new Line2f((0, 0), (1, 1));
            cd = new Line2f((0, 0), (1, 1));

            Assert.IsTrue(ab.AreEquivalent(cd));

            ab = new Line2f((0, 0), (1, 1));
            cd = new Line2f((0, 0), (-1, -1));

            Assert.IsTrue(ab.AreEquivalent(cd));

            ab = new Line2f((0, 0), (1, -1));
            cd = new Line2f((0, 0), (1, 1));

            Assert.IsFalse(ab.AreEquivalent(cd));

            ab = new Line2f((0, 0), (1, 1));
            cd = new Line2f((0, 0), (-1, 1));

            Assert.IsFalse(ab.AreEquivalent(cd));

            ab = new Line2f((0, 0), (1, -1));
            cd = new Line2f((0, 0), (-1, 1));

            Assert.IsTrue(ab.AreEquivalent(cd));

            ab = new Line2f((1, 0), (1, 1));
            cd = new Line2f((0, 0), (1, 1));

            Assert.IsFalse(ab.AreEquivalent(cd));

            ab = new Line2f((0, 1), (1, 1));
            cd = new Line2f((0, 0), (1, 1));

            Assert.IsFalse(ab.AreEquivalent(cd));

            ab = new Line2f((2, 0), (2, 1));
            cd = new Line2f((0, 0), (1, 1));

            Assert.IsFalse(ab.AreEquivalent(cd));

            ab = new Line2f((2, 0), (-2, 1));
            cd = new Line2f((0, 0), (1, 1));

            Assert.IsFalse(ab.AreEquivalent(cd));
        }
    }
}
