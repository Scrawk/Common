using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Queues;

namespace Common.Collections.Test.Queues
{
    [TestClass]
    public class Collections_Queues_PriorityQueueTest
    {
        [TestMethod]
        public void Push()
        {

            PriorityQueue<float> queue = new PriorityQueue<float>(new FloatComparer());

            queue.Push(1);
            Assert.AreEqual(1, queue.Count);
            Assert.AreEqual(1, queue.Root.Item);

            queue.Push(0.5f);
            Assert.AreEqual(2, queue.Count);
            Assert.AreEqual(0.5f, queue.Root.Item);

            queue.Push(1.5f);
            Assert.AreEqual(3, queue.Count);
            Assert.AreEqual(0.5f, queue.Root.Item);

            CollectionAssert.AreEqual(new float[] { 0.5f, 1.0f, 1.5f }, queue.ToList());

        }

        [TestMethod]
        public void Pop()
        {

            PriorityQueue<float> queue = new PriorityQueue<float>(new FloatComparer());

            queue.Push(1);
            queue.Push(0.5f);
            queue.Push(1.5f);

            Assert.AreEqual(0.5f, queue.Pop());
            Assert.AreEqual(2, queue.Count);

            Assert.AreEqual(1.0f, queue.Pop());
            Assert.AreEqual(1, queue.Count);

            Assert.AreEqual(1.5f, queue.Pop());
            Assert.AreEqual(0, queue.Count);

        }

        [TestMethod]
        public void Order()
        {
            int num = 1000;
            Random rnd = new Random(0);

            PriorityQueue<float> queue = new PriorityQueue<float>(new FloatComparer());

            for (int i = 0; i < num; i++)
                queue.Push((float)rnd.NextDouble());

            float v = queue.Pop();

            while(queue.Count != 0)
            {
                float next = queue.Pop();
                Assert.IsTrue(v <= next);
                v = next;
            }

        }
    }

    internal class FloatComparer : IComparer<float>
    {
        public int Compare(float f0, float f1)
        {
            return f0.CompareTo(f1);
        }
    }
}
