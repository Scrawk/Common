using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Test.Shapes
{
    [TestClass]
    public class Segment2fTest
    {

        [TestMethod]
        public void IntersectsBox()
        {
            var min = new Point2f(-2, -1);
            var max = new Point2f(1, 3);
            var box = new Box2f(min, max);

            var seg = new Segment2f(new Point2f(-6,-1), new Point2f(-3,-1));
            Assert.IsFalse(seg.Intersects(box));

            seg = new Segment2f(new Point2f(-1, -4), new Point2f(3, -1));
            Assert.IsFalse(seg.Intersects(box));

            seg = new Segment2f(new Point2f(1, 4), new Point2f(1, 6));
            Assert.IsFalse(seg.Intersects(box));

            seg = new Segment2f(new Point2f(-2, -3), new Point2f(-2, 5));
            Assert.IsTrue(seg.Intersects(box));

            seg = new Segment2f(new Point2f(-1, 2), new Point2f(-1, -2));
            Assert.IsTrue(seg.Intersects(box));

            seg = new Segment2f(new Point2f(-3, 1), new Point2f(2, 1));
            Assert.IsTrue(seg.Intersects(box));

            seg = new Segment2f(new Point2f(-3, 3), new Point2f(3, 2));
            Assert.IsTrue(seg.Intersects(box));

            seg = new Segment2f(new Point2f(-1, 0), new Point2f(2, 2));
            Assert.IsTrue(seg.Intersects(box));

            seg = new Segment2f(new Point2f(1, 3), new Point2f(1, 1));
            Assert.IsTrue(seg.Intersects(box));

            seg = new Segment2f(new Point2f(-2, -1), new Point2f(1, 1));
            Assert.IsTrue(seg.Intersects(box));

            seg = new Segment2f(new Point2f(-1, -1), new Point2f(2, -1));
            Assert.IsTrue(seg.Intersects(box));

            seg = new Segment2f(new Point2f(1, 3), new Point2f(1, 4));
            Assert.IsTrue(seg.Intersects(box));
        }
    }
}
