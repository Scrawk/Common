using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Queues;

namespace Common.Collections.Test.Lists
{
    [TestClass]
    public class Collections_Queues_BinaryHeapTest
    {
        [TestMethod]
        public void Add()
        {
            var list = new BinaryHeap<float>(10);

            list.Add(1);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(1, list.Peek());

            list.Add(0.5f);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(0.5f, list.Peek());

            list.Add(1.5f);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(0.5f, list.Peek());

            Assert.AreEqual(3, list.Count);
            CollectionAssert.AreEqual(new float[] { 0.5f, 1.0f, 1.5f }, list.ToList());
        }

        [TestMethod]
        public void Remove()
        {

            var list = new BinaryHeap<float>();

            list.Add(1);
            list.Add(0.5f);
            list.Add(1.5f);

            list.Remove(0.5f);

            Assert.AreEqual(1.0f, list.Peek());
            Assert.AreEqual(2, list.Count);

            list.Remove(1.5f);

            Assert.AreEqual(1.0f, list.Peek());
            Assert.AreEqual(1, list.Count);

            list.Remove(1.0f);

            Assert.AreEqual(0, list.Count);

        }

        [TestMethod]
        public void Order()
        {
            int num = 1000;
            Random rnd = new Random(0);

            var list = new BinaryHeap<float>();

            for (int i = 0; i < num; i++)
                list.Add((float)rnd.NextDouble());

            float v = list.Peek();
            list.Remove(v);

            while (list.Count != 0)
            {
                float next = list.Peek();
                list.Remove(next);
                Assert.IsTrue(v <= next);
                v = next;
            }

        }
    }
}
