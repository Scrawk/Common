using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Queues;

namespace Common.Collections.Test.Queues
{
    [TestClass]
    public class BinaryHeapTest
    {

        [TestMethod]
        public void Count()
        {
            var list = new PriorityList<float>();
            list.Add(0);
            Assert.AreEqual(1, list.Count);
            list.RemoveValue(0);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Capacity()
        {
            var list = new BinaryHeap<float>();
            list.Capacity = 3;
            Assert.AreEqual(3, list.Capacity);

            list.Add(1);
            list.Add(0.5f);
            list.Add(1.5f);
            list.RemoveValue(1);

            list.Capacity = 0;
            list.Add(1);
        }

        [TestMethod]
        public void Add()
        {
            var list = TestQueue();

            Assert.AreEqual(3, list.Count);
            CollectionAssert.AreEqual(new float[] { 0.5f, 1.0f, 1.5f }, list.ToList());

            list.Clear();
            list.Add(new float[] { 0.5f, 1.0f, 1.5f });

            Assert.AreEqual(3, list.Count);
            CollectionAssert.AreEqual(new float[] { 0.5f, 1.0f, 1.5f }, list.ToList());
        }

        [TestMethod]
        public void Contains()
        {
            var list = TestQueue();

            Assert.IsTrue(list.ContainsValue(1));
            Assert.IsTrue(list.ContainsValue(0.5f));
            Assert.IsTrue(list.ContainsValue(1.5f));
            Assert.IsFalse(list.ContainsValue(-1));
            Assert.IsFalse(list.ContainsValue(0));
            Assert.IsFalse(list.ContainsValue(2.5f));
        }

        [TestMethod]
        public void Remove()
        {
            var list = TestQueue();

            list.RemoveValue(0.5f);
            Assert.IsFalse(list.ContainsValue(0.5f));
            CollectionAssert.AreEqual(new float[] { 1.0f, 1.5f }, list.ToList());

            list.RemoveValue(1.5f);
            Assert.IsFalse(list.ContainsValue(1.5f));
            CollectionAssert.AreEqual(new float[] { 1.0f }, list.ToList());

            list.RemoveValue(1.0f);
            Assert.IsFalse(list.ContainsValue(1.0f));
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void RemoveFirst()
        {
            var list = TestQueue();

            list.RemoveFirst();
            Assert.IsFalse(list.ContainsValue(0.5f));

            list.RemoveFirst();
            Assert.IsFalse(list.ContainsValue(1.0f));

            list.RemoveFirst();
            Assert.IsFalse(list.ContainsValue(1.5f));

            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Peek()
        {
            var list = new BinaryHeap<float>();

            list.Add(1);
            Assert.AreEqual(1, list.Peek());

            list.Add(0.5f);
            Assert.AreEqual(0.5f, list.Peek());

            list.Add(1.5f);
            Assert.AreEqual(0.5f, list.Peek());
        }

        [TestMethod]
        public void Clear()
        {
            var list = TestQueue();

            list.Clear();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Order()
        {
            int num = 1000;
            Random rnd = new Random(0);

            var list = new BinaryHeap<double>();

            for (int i = 0; i < num; i++)
                list.Add(rnd.NextDouble());

            double v = list.Peek();
            list.RemoveValue(v);

            while (list.Count != 0)
            {
                double next = list.Peek();
                list.RemoveValue(next);
                Assert.IsTrue(v <= next);
                v = next;
            }
        }

        [TestMethod]
        public void ToList()
        {
            var list = TestQueue();
            CollectionAssert.AreEqual(new float[] { 0.5f, 1.0f, 1.5f }, list.ToList());
        }

        private BinaryHeap<float> TestQueue()
        {
            var list = new BinaryHeap<float>();

            list.Add(1);
            list.Add(0.5f);
            list.Add(1.5f);

            return list;
        }
    }
}
