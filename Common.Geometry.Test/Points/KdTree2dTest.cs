using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

using Common.Geometry.Points;

namespace Common.Geometry.Test.Points
{
    [TestClass]
    public class KdTree2fTest
    {
        [TestMethod]
        public void Count()
        {
            var points = RandomPoints(6, 0, -5, 5);
            var collection = new KdTree2f(points);
            Assert.AreEqual(6, collection.Count);
        }

        [TestMethod]
        public void Clear()
        {
            var points = RandomPoints(5, 0, -5, 5);
            var collection = new KdTree2f(points);

            collection.Clear();
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void Enumerable()
        {
            var points = RandomPoints(5, 0, -5, 5);
            var collection = new KdTree2f(points);

            var list = new List<Vector2f>();
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
            var collection = new KdTree2f(points);

            var list = collection.ToList();
            CollectionAssert.AreEquivalent(points, list);
        }

        [TestMethod]
        public void Closest()
        {
            var points = new List<Vector2f>();

            for (int i = 0; i <= 5; i++)
                points.Add(new Vector2f(i));

            var collection = new KdTree2f(points);

            Assert.AreEqual(new Vector2f(0), collection.Closest(new Vector2f(-1, -1)));
            Assert.AreEqual(new Vector2f(5), collection.Closest(new Vector2f(6, 6)));
            Assert.AreEqual(new Vector2f(2), collection.Closest(new Vector2f(2.1f, 2)));
            Assert.AreEqual(new Vector2f(3), collection.Closest(new Vector2f(3, 3.1f)));
           
        }

        [TestMethod]
        public void Search()
        {
            var points = new List<Vector2f>();
            for (int i = 0; i <= 5; i++)
                points.Add(new Vector2f(i));

            var collection = new KdTree2f(points);

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
            var points = RandomPoints(1000, 0, -5, 5);

            var naive = new PointCollection2f(points);
            var tree = new KdTree2f(points);

            for(int i = 0; i < 100; i++)
            {
                var p = RandomPoint(i, -5, 5);
                Assert.AreEqual(naive.Closest(p), tree.Closest(p));
            }

            for (int i = 0; i < 100; i++)
            {
                var p = RandomPoint(i, -5, 5);
                Assert.AreEqual(naive.SignedDistance(p), tree.SignedDistance(p));
            }

            for (int i = 0; i < 100; i++)
            {
                var p = RandomPoint(i, -5, 5);
                var region = new Circle2f(p, 0.1f);
                var list0 = new List<Vector2f>();
                var list1 = new List<Vector2f>();

                naive.Search(region, list0);
                tree.Search(region, list1);

                CollectionAssert.AreEquivalent(list0, list1);
            }
        }

        private List<Vector2f> RandomPoints(int count, int seed, float min, float max)
        {
            var rnd = new Random(seed);
            List<Vector2f> points = new List<Vector2f>(count);

            for (int i = 0; i < count; i++)
            {
                float x = min + rnd.NextFloat() * (max - min);
                float y = min + rnd.NextFloat() * (max - min);

                points.Add(new Vector2f(x, y));
            }

            return points;
        }
        private Vector2f RandomPoint(int seed, float min, float max)
        {
            var rnd = new Random(seed);
            float x = min + rnd.NextFloat() * (max - min);
            float y = min + rnd.NextFloat() * (max - min);

            return new Vector2f(x, y);
        }

    }
}
