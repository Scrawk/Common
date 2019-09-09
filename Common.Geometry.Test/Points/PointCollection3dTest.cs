using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

using Common.Geometry.Points;

namespace Common.Geometry.Test.Points
{
    [TestClass]
    public class PointCollection3fTest
    {
        [TestMethod]
        public void Count()
        {
            var collection = new PointCollection3f();
            Assert.AreEqual(0, collection.Count);

            collection.Add(new Vector3f(1, 1));
            Assert.AreEqual(1, collection.Count);

            collection.Add(RandomPoints(5, 0, -5, 5));
            Assert.AreEqual(6, collection.Count);

            collection.Remove(new Vector3f(1, 1));
            Assert.AreEqual(5, collection.Count);
        }

        [TestMethod]
        public void Clear()
        {
            var collection = new PointCollection3f();

            collection.Add(RandomPoints(5, 0, -5, 5));
            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void Enumerable()
        {
            var collection = new PointCollection3f();
            var points = RandomPoints(5, 0, -5, 5);
            collection.Add(points);

            var list = new List<Vector3f>();

            foreach (var p in collection)
                list.Add(p);

            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void ToList()
        {
            var collection = new PointCollection3f();
            var points = RandomPoints(5, 0, -5, 5);
            collection.Add(points);

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void Add()
        {
            var collection = new PointCollection3f();

            var points = RandomPoints(5, 0, -5, 5);

            foreach (var p in points)
                collection.Add(p);

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void Remove()
        {
            var collection = new PointCollection3f();

            var points = RandomPoints(5, 0, -5, 5);
            collection.Add(points);

            Assert.IsTrue(collection.Remove(points[1]));
            Assert.IsTrue(points.Remove(points[1]));

            Assert.IsTrue(collection.Remove(points[3]));
            Assert.IsTrue(points.Remove(points[3]));

            Assert.IsFalse(collection.Remove(new Vector3f(10, 10)));
            Assert.IsFalse(points.Remove(new Vector3f(10, 10)));

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void Closest()
        {
            var collection = new PointCollection3f();

            for (int i = 0; i <= 5; i++)
                collection.Add(new Vector3f(i));

            Assert.AreEqual(new Vector3f(0), collection.Closest(new Vector3f(-1)));
            Assert.AreEqual(new Vector3f(5), collection.Closest(new Vector3f(6)));
            Assert.AreEqual(new Vector3f(2), collection.Closest(new Vector3f(2.1f, 2, 1.9f)));
            Assert.AreEqual(new Vector3f(3), collection.Closest(new Vector3f(3, 3.1f, 2.0f)));
        }

        [TestMethod]
        public void Search()
        {
            var collection = new PointCollection3f();

            for (int i = 0; i <= 5; i++)
                collection.Add(new Vector3f(i));

            var results = new List<Vector3f>();
            var region = new Sphere3f(new Vector3f(2.5f), 5);
            collection.Search(region, results);

            var list = collection.ToList();
            Assert.AreEqual(6, results.Count);
            CollectionAssert.AreEquivalent(list, results);

            results = new List<Vector3f>();
            region = new Sphere3f(new Vector3f(2), 0.1f);
            collection.Search(region, results);

            Console.WriteLine(region.Contains(new Vector3f(2)));

            list = new List<Vector3f>() { new Vector3f(2) };
            CollectionAssert.AreEquivalent(list, results);

            results = new List<Vector3f>();
            region = new Sphere3f(new Vector3f(3.5f), 1);
            collection.Search(region, results);

            list = new List<Vector3f>() { new Vector3f(3), new Vector3f(4) };
            CollectionAssert.AreEquivalent(list, results);
        }

        private List<Vector3f> RandomPoints(int count, int seed, float min, float max)
        {
            var rnd = new Random(seed);
            List<Vector3f> points = new List<Vector3f>(count);

            for (int i = 0; i < count; i++)
            {
                float x = min + rnd.NextFloat() * (max - min);
                float y = min + rnd.NextFloat() * (max - min);
                float z = min + rnd.NextFloat() * (max - min);

                points.Add(new Vector3f(x, y, z));
            }

            return points;
        }
    }
}
