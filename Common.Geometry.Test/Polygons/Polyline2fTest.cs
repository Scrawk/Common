using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Polygons;
using Common.Geometry.Shapes;

namespace Common.Geometry.Test.Polygons
{
    [TestClass]
    public class Polyline2fTest
    {
        [TestMethod]
        public void Calculate()
        {
            var polyline = CreatePolyline();

            Assert.AreEqual(14, polyline.Length);
            Assert.AreEqual(new Box2f(-2, 2), polyline.Bounds);
        }

        [TestMethod]
        public void GetPosition()
        {
            var polyline = CreatePolyline();
            float eps = 1e-4f;
            float len = polyline.Length;

            Assert.IsTrue(Vector2f.AlmostEqual(polyline.GetPosition(0.0f), new Vector2f(1, 2), eps));
            Assert.IsTrue(Vector2f.AlmostEqual(polyline.GetPosition(1.0f), new Vector2f(-1, 2), eps));
            Assert.IsTrue(Vector2f.AlmostEqual(polyline.GetPosition(3.0f / len), new Vector2f(2, 0), eps));
            Assert.IsTrue(Vector2f.AlmostEqual(polyline.GetPosition(5.2f / len), new Vector2f(1, -1.2), eps));
            Assert.IsTrue(Vector2f.AlmostEqual(polyline.GetPosition(9.8f / len), new Vector2f(-1.8, -1), eps));
        }

        private Polyline2f CreatePolyline()
        {
            var positions = new Vector2f[]
            {
                new Vector2f(1,2),
                new Vector2f(1,1),
                new Vector2f(2,1),
                new Vector2f(2,-1),
                new Vector2f(1,-1),
                new Vector2f(1,-2),
                new Vector2f(-1,-2),
                new Vector2f(-1,-1),
                new Vector2f(-2,-1),
                new Vector2f(-2,1),
                new Vector2f(-1,1),
                new Vector2f(-1,2)
            };

            var polyline = new Polyline2f(0.01f, positions);
            polyline.Calculate();

            return polyline;
        }
    }
}
