using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Polygons;
using Common.Geometry.Shapes;

namespace Common.Geometry.Test.Polygons
{
    [TestClass]
    public class Polygon2fTest
    {
        [TestMethod]
        public void Calculate()
        {
            var polygon = CreatePolygon();

            Assert.AreEqual(12, polygon.Area);
            Assert.AreEqual(16, polygon.Length);
            Assert.AreEqual(true, polygon.IsCW);
            Assert.AreEqual(new Vector2f(0), polygon.Centroid);
            Assert.AreEqual(new Box2f(-2,2), polygon.Bounds);
        }

        [TestMethod]
        public void GetPosition()
        {
            var polygon = CreatePolygon();
            float eps = 1e-4f;
            float len = polygon.Length;

            Assert.IsTrue(Vector2f.AlmostEqual(polygon.GetPosition(0.0f), new Vector2f(1, 2), eps));
            Assert.IsTrue(Vector2f.AlmostEqual(polygon.GetPosition(1.0f), new Vector2f(1, 2), eps));
            Assert.IsTrue(Vector2f.AlmostEqual(polygon.GetPosition(3.0f / len), new Vector2f(2, 0), eps));
            Assert.IsTrue(Vector2f.AlmostEqual(polygon.GetPosition(5.2f / len), new Vector2f(1, -1.2), eps));
            Assert.IsTrue(Vector2f.AlmostEqual(polygon.GetPosition(9.8f / len), new Vector2f(-1.8, -1), eps));
        }

        private Polygon2f CreatePolygon()
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

            var polygon = new Polygon2f(positions);
            polygon.Calculate();

            return polygon;
        }
    }
}
