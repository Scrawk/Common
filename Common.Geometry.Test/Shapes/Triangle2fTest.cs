using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Core.Shapes;

namespace Common.Geometry.Test.Shapes
{
    [TestClass]
    public class Triangle2fTest
    {
        [TestMethod]
        public void Area()
        {
            var a = new Point2f(0, 1);
            var b = new Point2f(0, 0);
            var c = new Point2f(1, 0);
            var triangle = new Triangle2f(a, b, c);

            Assert.AreEqual(0.5f, triangle.SignedArea);
            Assert.AreEqual(0.5f, triangle.Area);
        }

        [TestMethod]
        public void IntersectsBox()
        {
            var a = new Point2f(-2, -1);
            var b = new Point2f(1, -1);
            var c = new Point2f(1, 1);
            var triangle = new Triangle2f(a, b, c);

            var box = new Box2f(new Point2f(-3, -3), new Point2f(-2,-2));
            //Assert.IsFalse(triangle.Intersects(box));

            box = new Box2f(new Point2f(-2, -1), new Point2f(1, 2));
            //Assert.IsFalse(triangle.Intersects(box));

            box = new Box2f(new Point2f(1, -3), new Point2f(2, -2));
            //Assert.IsFalse(triangle.Intersects(box));
        }

    }
}
