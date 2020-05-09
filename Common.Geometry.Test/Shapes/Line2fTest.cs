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
        public void AreParallel()
        {
            var ab = new Line2f((0, 0), (1, 1));
            var cd = new Line2f((1, 1), (0, 0));

            Assert.IsTrue(ab.IsParallel(cd));

            ab = new Line2f((1, 1), (0, 0));
            cd = new Line2f((0, 0), (1, 1));

            Assert.IsTrue(ab.IsParallel(cd));

            ab = new Line2f((0, 0), (1, 1));
            cd = new Line2f((0, 0), (1, 1));

            Assert.IsTrue(ab.IsParallel(cd));

            ab = new Line2f((4, 0), (5, 1));
            cd = new Line2f((0, 0), (1, 1));

            Assert.IsFalse(ab.IsParallel(cd));

            ab = new Line2f((0, 2), (1, 3));
            cd = new Line2f((0, 0), (1, 1));

            Assert.IsFalse(ab.IsParallel(cd));

            ab = new Line2f((0, 0), (2, 1));
            cd = new Line2f((0, 0), (1, 1));

            Assert.IsFalse(ab.IsParallel(cd));

        }
    }
}
