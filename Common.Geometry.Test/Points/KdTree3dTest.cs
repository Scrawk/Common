using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

using Common.Geometry.Points;

namespace Common.Geometry.Test.Points
{
    [TestClass]
    public class KdTree3fTest
    {
        [TestMethod]
        public void Count()
        {
            var points = RandomPoints(6, 0, -5, 5);
            var collection = new KdTree3f<Point3f>(points);
            Assert.AreEqual(6, collection.Count);
        }

        [TestMethod]
        public void Clear()
        {
            var points = RandomPoints(5, 0, -5, 5);
            var collection = new KdTree3f<Point3f>(points);

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void Enumerable()
        {
            var points = RandomPoints(5, 0, -5, 5);
            var collection = new KdTree3f<Point3f>(points);

            var list = new List<Point3f>();
            foreach (var p in collection)
            {
                list.Add(p);
            }

            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void ToList()
        {
            var points = RandomPoints(5, 0, -5, 5);
            var collection = new KdTree3f<Point3f>(points);

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void Closest()
        {
            var points = new List<Point3f>();

            for (int i = 0; i <= 5; i++)
                points.Add(new Point3f(i));

            var collection = new KdTree3f<Point3f>(points);

            Assert.AreEqual(new Point3f(0), collection.Closest(new Point3f(-1)));
            Assert.AreEqual(new Point3f(5), collection.Closest(new Point3f(6)));
            Assert.AreEqual(new Point3f(2), collection.Closest(new Point3f(2.1f, 2, 1.9f)));
            Assert.AreEqual(new Point3f(3), collection.Closest(new Point3f(3, 3.1f, 2.9f)));

        }

        [TestMethod]
        public void Search()
        {
            var points = new List<Point3f>();
            for (int i = 0; i <= 5; i++)
                points.Add(new Point3f(i));

            var collection = new KdTree3f<Point3f>(points);

            var results = new List<Point3f>();
            var region = new Sphere3f(new Vector3f(2.5f), 5);
            collection.Search(region, results);

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(list, results);

            results = new List<Point3f>();
            region = new Sphere3f(new Vector3f(2), 0.1f);
            collection.Search(region, results);

            list = new List<Point3f>() { new Point3f(2) };
            CollectionAssert.AreEquivalent(list, results);

            results = new List<Point3f>();
            region = new Sphere3f(new Vector3f(3.5f), 1);
            collection.Search(region, results);

            list = new List<Point3f>() { new Point3f(3), new Point3f(4) };
            CollectionAssert.AreEquivalent(list, results);
        }

        [TestMethod]
        public void CompareToNaive()
        {
            var points = RandomPoints(1000, 0, -5, 5);

            var naive = new PointCollection3f<Point3f>(points);
            var tree = new KdTree3f<Point3f>(points);

            for (int i = 0; i < 100; i++)
            {
                var p = RandomPoint(i, -5, 5);
                Assert.AreEqual(naive.Closest(p), tree.Closest(p));
            }

            for (int i = 0; i < 100; i++)
            {
                var p = RandomPoint(i, -5, 5);
                var region = new Sphere3f(new Vector3f(p.x, p.y, p.z), 0.1f);
                var list0 = new List<Point3f>();
                var list1 = new List<Point3f>();

                naive.Search(region, list0);
                tree.Search(region, list1);

                CollectionAssert.AreEquivalent(list0, list1);
            }
        }

        private List<Point3f> RandomPoints(int count, int seed, float min, float max)
        {
            var rnd = new Random(seed);
            List<Point3f> points = new List<Point3f>(count);

            for (int i = 0; i < count; i++)
            {
                float x = min + rnd.NextFloat() * (max - min);
                float y = min + rnd.NextFloat() * (max - min);
                float z = min + rnd.NextFloat() * (max - min);

                points.Add(new Point3f(x, y, z));
            }

            return points;
        }
        private Point3f RandomPoint(int seed, float min, float max)
        {
            var rnd = new Random(seed);
            float x = min + rnd.NextFloat() * (max - min);
            float y = min + rnd.NextFloat() * (max - min);
            float z = min + rnd.NextFloat() * (max - min);

            return new Point3f(x, y, z);
        }

    }
}
