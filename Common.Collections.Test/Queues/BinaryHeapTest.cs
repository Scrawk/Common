using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Queues;

namespace Common.Collections.Test.Queues
{
    [TestClass]
    public class BinaryHeapTest
    {


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
        public void RemoveFirst()
        {
            var list = TestQueue();

            Assert.AreEqual(0.5f, list.Pop());
            Assert.AreEqual(2, list.Count);

            Assert.AreEqual(1.0f, list.Pop());
            Assert.AreEqual(1, list.Count);

            Assert.AreEqual(1.5f, list.Pop());
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Clear()
        {
            var list = TestQueue();

            list.Clear();
            Assert.AreEqual(0, list.Count);
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
