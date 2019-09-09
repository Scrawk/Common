using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

using Common.Geometry.Points;

namespace Common.Geometry.Test.Points
{
    [TestClass]
    public class PointGrid3fTest
    {
        [TestMethod]
        public void Count()
        {
            var bounds = new Box3f(-5, 5);
            var collection = new PointGrid3f(bounds, 0.5f);

            Assert.AreEqual(0, collection.Count);

            collection.Add(new Vector3f(1, 1, 1));
            Assert.AreEqual(1, collection.Count);

            collection.Add(RandomPoints(5, 0, bounds));
            Assert.AreEqual(6, collection.Count);

            collection.Remove(new Vector3f(1, 1, 1));
            Assert.AreEqual(5, collection.Count);

        }

        [TestMethod]
        public void Clear()
        {
            var bounds = new Box3f(-5, 5);
            var collection = new PointGrid3f(bounds, 0.5f);

            collection.Add(RandomPoints(5, 0, bounds));
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void Enumerable()
        {
            var bounds = new Box3f(-5, 5);
            var collection = new PointGrid3f(bounds, 0.5f);

            var points = RandomPoints(5, 0, bounds);
            collection.Add(points);

            var list = new List<Vector3f>();

            foreach (var p in collection)
                list.Add(p);

            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void ToList()
        {
            var bounds = new Box3f(-5, 5);
            var collection = new PointGrid3f(bounds, 0.5f);

            var points = RandomPoints(5, 0, bounds);
            collection.Add(points);

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void Add()
        {
            var bounds = new Box3f(-5, 5);
            var collection = new PointGrid3f(bounds, 0.5f);

            var points = RandomPoints(5, 0, bounds);

            foreach (var p in points)
                collection.Add(p);

            collection.Add(new Vector3f(-5, -5, -5));
            points.Add(new Vector3f(-5, -5, -5));

            collection.Add(new Vector3f(5, 5, 5));
            points.Add(new Vector3f(5, 5, 5));

            var list = collection.ToList();

            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void AddOutOfBounds()
        {
            var bounds = new Box3f(-5, 5);
            var collection = new PointGrid3f(bounds, 0.5f);

            Assert.IsTrue(collection.Add(new Vector3f(-4.999f, 4.999f, 4.999f)));
            Assert.IsFalse(collection.Add(new Vector3f(-5.001f, 0, 0)));
            Assert.IsFalse(collection.Add(new Vector3f(5.001f, 0, 0)));
            Assert.IsFalse(collection.Add(new Vector3f(0, -5.001f, 0)));
            Assert.IsFalse(collection.Add(new Vector3f(0, 5.001f, 0)));
            Assert.IsFalse(collection.Add(new Vector3f(0, 0, -5.001f)));
            Assert.IsFalse(collection.Add(new Vector3f(0, 0, 5.001f)));
        }

        [TestMethod]
        public void Remove()
        {
            var bounds = new Box3f(-5, 5);
            var collection = new PointGrid3f(bounds, 0.5f);

            var points = RandomPoints(5, 0, bounds);
            collection.Add(points);

            Assert.IsTrue(collection.Remove(points[1]));
            Assert.IsTrue(points.Remove(points[1]));

            Assert.IsTrue(collection.Remove(points[3]));
            Assert.IsTrue(points.Remove(points[3]));

            Assert.IsFalse(collection.Remove(new Vector3f(10, 10, 10)));
            Assert.IsFalse(points.Remove(new Vector3f(10, 10, 10)));

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void Closest()
        {
            var bounds = new Box3f(0, 5);
            var collection = new PointGrid3f(bounds, 1);

            for (int i = 0; i <= 5; i++)
                collection.Add(new Vector3f(i));

            Assert.AreEqual(new Vector3f(0), collection.Closest(new Vector3f(-1, -1, -1)));
            Assert.AreEqual(new Vector3f(5), collection.Closest(new Vector3f(6, 6, 6)));
            Assert.AreEqual(new Vector3f(2), collection.Closest(new Vector3f(2.1f, 2, 1.9f)));
            Assert.AreEqual(new Vector3f(3), collection.Closest(new Vector3f(3, 3.1f, 2.9f)));
        }

        [TestMethod]
        public void Search()
        {
            var bounds = new Box3f(0, 5);
            var collection = new PointGrid3f(bounds, 0.5f);

            for (int i = 0; i <= 5; i++)
                collection.Add(new Vector3f(i));

            var results = new List<Vector3f>();
            var region = new Sphere3f(new Vector3f(2.5f), 5);
            collection.Search(region, results);

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(list, results);

            results = new List<Vector3f>();
            region = new Sphere3f(new Vector3f(2), 0.1f);
            collection.Search(region, results);

            list = new List<Vector3f>() { new Vector3f(2) };
            CollectionAssert.AreEquivalent(list, results);

            results = new List<Vector3f>();
            region = new Sphere3f(new Vector3f(3.5f), 1);
            collection.Search(region, results);

            list = new List<Vector3f>() { new Vector3f(3), new Vector3f(4) };
            CollectionAssert.AreEquivalent(list, results);
        }

        [TestMethod]
        public void CompareToNaive()
        {
            var bounds = new Box3f(-5, 5);
            var points = RandomPoints(10000, 0, bounds);

            var naive = new PointCollection3f(points);
            var grid = new PointGrid3f(bounds, 0.5f, points);

            for (int i = 0; i < 100; i++)
            {
                var p = RandomPoint(i, bounds);
                Assert.AreEqual(naive.Closest(p), grid.Closest(p));
            }

            for (int i = 0; i < 100; i++)
            {
                var p = RandomPoint(i, bounds);
                Assert.AreEqual(naive.SignedDistance(p), grid.SignedDistance(p));
            }

            for (int i = 0; i < 100; i++)
            {
                var p = RandomPoint(i, bounds);
                var region = new Sphere3f(p, 0.1f);
                var list0 = new List<Vector3f>();
                var list1 = new List<Vector3f>();

                naive.Search(region, list0);
                grid.Search(region, list1);

                CollectionAssert.AreEquivalent(list0, list1);
            }
        }

        private List<Vector3f> RandomPoints(int count, int seed, Box3f bounds)
        {
            var rnd = new Random(seed);
            List<Vector3f> points = new List<Vector3f>(count);

            for (int i = 0; i < count; i++)
            {
                float x = bounds.Min.x + rnd.NextFloat() * bounds.Width;
                float y = bounds.Min.y + rnd.NextFloat() * bounds.Height;
                float z = bounds.Min.z + rnd.NextFloat() * bounds.Depth;

                points.Add(new Vector3f(x, y, z));
            }

            return points;
        }

        private Vector3f RandomPoint(int seed, Box3f bounds)
        {
            var rnd = new Random(seed);
            float x = bounds.Min.x + rnd.NextFloat() * bounds.Width;
            float y = bounds.Min.y + rnd.NextFloat() * bounds.Height;
            float z = bounds.Min.z + rnd.NextFloat() * bounds.Depth;

            return new Vector3f(x, y, z);
        }
    }
}
