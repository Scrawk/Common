using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

using Common.Geometry.Points;

namespace Common.Geometry.Test.Points
{
    [TestClass]
    public class PointGrid2fTest
    {
        [TestMethod]
        public void Count()
        {
            var bounds = new Box2f(-5, 5);
            var collection = new PointGrid2f(bounds, 0.5f);

            Assert.AreEqual(0, collection.Count);

            collection.Add(new Vector2f(1, 1));
            Assert.AreEqual(1, collection.Count);

            collection.Add(RandomPoints(5, 0, bounds));
            Assert.AreEqual(6, collection.Count);

            collection.Remove(new Vector2f(1, 1));
            Assert.AreEqual(5, collection.Count);

        }

        [TestMethod]
        public void Clear()
        {
            var bounds = new Box2f(-5, 5);
            var collection = new PointGrid2f(bounds, 0.5f);

            collection.Add(RandomPoints(5, 0, bounds));
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void Enumerable()
        {
            var bounds = new Box2f(-5, 5);
            var collection = new PointGrid2f(bounds, 0.5f);

            var points = RandomPoints(5, 0, bounds);
            collection.Add(points);

            var list = new List<Vector2f>();

            foreach (var p in collection)
                list.Add(p);

            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void ToList()
        {
            var bounds = new Box2f(-5, 5);
            var collection = new PointGrid2f(bounds, 0.5f);

            var points = RandomPoints(5, 0, bounds);
            collection.Add(points);

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void Add()
        {
            var bounds = new Box2f(-5, 5);
            var collection = new PointGrid2f(bounds, 0.5f);

            var points = RandomPoints(5, 0, bounds);

            foreach (var p in points)
                collection.Add(p);

            collection.Add(new Vector2f(-5, -5));
            points.Add(new Vector2f(-5, -5));

            collection.Add(new Vector2f(5, 5));
            points.Add(new Vector2f(5, 5));

            var list = collection.ToList();

            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void AddOutOfBounds()
        {
            var bounds = new Box2f(-5, 5);
            var collection = new PointGrid2f(bounds, 0.5f);

            Assert.IsTrue(collection.Add(new Vector2f(-4.999f, 4.999f)));
            Assert.IsFalse(collection.Add(new Vector2f(-5.001f, 0)));
            Assert.IsFalse(collection.Add(new Vector2f(5.001f, 0)));
            Assert.IsFalse(collection.Add(new Vector2f(0, -5.001f)));
            Assert.IsFalse(collection.Add(new Vector2f(0, 5.001f)));
        }

        [TestMethod]
        public void Remove()
        {
            var bounds = new Box2f(-5, 5);
            var collection = new PointGrid2f(bounds, 0.5f);

            var points = RandomPoints(5, 0, bounds);
            collection.Add(points);

            Assert.IsTrue(collection.Remove(points[1]));
            Assert.IsTrue(points.Remove(points[1]));

            Assert.IsTrue(collection.Remove(points[3]));
            Assert.IsTrue(points.Remove(points[3]));

            Assert.IsFalse(collection.Remove(new Vector2f(10, 10)));
            Assert.IsFalse(points.Remove(new Vector2f(10, 10)));

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void Closest()
        {
            var bounds = new Box2f(0, 5);
            var collection = new PointGrid2f(bounds, 1);

            for (int i = 0; i <= 5; i++)
                collection.Add(new Vector2f(i));

            Assert.AreEqual(new Vector2f(0), collection.Closest(new Vector2f(-1, -1)));
            Assert.AreEqual(new Vector2f(5), collection.Closest(new Vector2f(6, 6)));
            Assert.AreEqual(new Vector2f(2), collection.Closest(new Vector2f(2.1f, 2)));
            Assert.AreEqual(new Vector2f(3), collection.Closest(new Vector2f(3, 3.1f)));
        }

        [TestMethod]
        public void Search()
        {
            var bounds = new Box2f(0, 5);
            var collection = new PointGrid2f(bounds, 0.5f);

            for (int i = 0; i <= 5; i++)
                collection.Add(new Vector2f(i));

            var results = new List<Vector2f>();
            var region = new Circle2f(new Vector2f(2.5f), 4);
            collection.Search(region, results);

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(list, results);

            results = new List<Vector2f>();
            region = new Circle2f(new Vector2f(2), 0.1f);
            collection.Search(region, results);

            list = new List<Vector2f>() { new Vector2f(2) };
            CollectionAssert.AreEquivalent(list, results);

            results = new List<Vector2f>();
            region = new Circle2f(new Vector2f(3.5f), 1);
            collection.Search(region, results);

            list = new List<Vector2f>() { new Vector2f(3), new Vector2f(4) };
            CollectionAssert.AreEquivalent(list, results);
        }

        [TestMethod]
        public void CompareToNaive()
        {
            var bounds = new Box2f(-5, 5);
            var points = RandomPoints(1000, 0, bounds);

            var naive = new PointCollection2f(points);
            var grid = new PointGrid2f(bounds, 0.5f, points);

            for (int i = 0; i < 100; i++)
            {
                var p = RandomPoint(i, bounds);
                Assert.AreEqual(naive.Closest(p), grid.Closest(p));
            }

            for (int i = 0; i < 100; i++)
            {
                var p = RandomPoint(i, bounds);
                var region = new Circle2f(p, 0.1f);
                var list0 = new List<Vector2f>();
                var list1 = new List<Vector2f>();

                naive.Search(region, list0);
                grid.Search(region, list1);

                CollectionAssert.AreEquivalent(list0, list1);
            }
        }

        private List<Vector2f> RandomPoints(int count, int seed, Box2f bounds)
        {
            var rnd = new Random(seed);
            List<Vector2f> points = new List<Vector2f>(count);

            for (int i = 0; i < count; i++)
            {
                float x = bounds.Min.x + rnd.NextFloat() * bounds.Width;
                float y = bounds.Min.y + rnd.NextFloat() * bounds.Height;

                points.Add(new Vector2f(x, y));
            }

            return points;
        }

        private Vector2f RandomPoint(int seed, Box2f bounds)
        {
            var rnd = new Random(seed);
            float x = bounds.Min.x + rnd.NextFloat() * bounds.Width;
            float y = bounds.Min.y + rnd.NextFloat() * bounds.Height;

            return new Vector2f(x, y);
        }
    }
}
