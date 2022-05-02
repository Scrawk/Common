using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Core.Shapes;


namespace Common.Core.Test.Shapes
{
    [TestClass]
    public class Ray2fTest
    {
        [TestMethod]
        public void IntersectRay()
        {
            var ray1 = new Ray2f(new Point2f(0, 0), new Vector2f(1, 0));
            var ray2 = new Ray2f(new Point2f(1, 2), new Vector2f(0, -1));

            float s, t;
            ray1.Intersects(ray2, out s, out t);

            Assert.IsTrue(MathUtil.AlmostEqual(1, s));
            Assert.IsTrue(MathUtil.AlmostEqual(2, t));
            Assert.IsTrue(Point2f.AlmostEqual(new Point2f(1, 0), ray1.GetPosition(s)));
            Assert.IsTrue(Point2f.AlmostEqual(new Point2f(1, 0), ray2.GetPosition(t)));
        }
    }
}
