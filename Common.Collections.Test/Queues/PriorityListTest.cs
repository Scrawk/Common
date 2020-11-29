using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Queues;

namespace Common.Collections.Test.Queues
{
    [TestClass]
    public class PriorityListTest
    {

        [TestMethod]
        public void Count()
        {
            var list = new PriorityList<float>();
            list.Add(0);
            Assert.AreEqual(1, list.Count);
            list.Remove(0);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Capacity()
        {
            var list = new PriorityList<float>();
            list.Capacity = 10;
            Assert.AreEqual(10, list.Capacity);
            list.Capacity = 1;
            Assert.AreEqual(1, list.Capacity);
            list.Capacity = 0;
            Assert.AreEqual(0, list.Capacity);
        }

        [TestMethod]
        public void Peek()
        {
            var list = TestQueue();
            Assert.AreEqual(0.5f, list.Peek());
        }

        [TestMethod]
        public void Pop()
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
        public void Remove()
        {
            var list = new PriorityList<TestComparable>();

            var a = new TestComparable(0.5f);
            var b = new TestComparable(1.0f);
            var c = new TestComparable(1.5f);
            var d = new TestComparable(1.0f);

            list.Add(a);
            list.Add(b);
            list.Add(c);

            Assert.AreEqual(3, list.Count);
            Assert.IsFalse(list.Remove(d));
            Assert.IsTrue(list.Remove(b));
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void ToList()
        {
            var list = TestQueue();
            CollectionAssert.AreEqual(new float[] { 0.5f, 1.0f, 1.5f }, list.ToList());
        }

        [TestMethod]
        public void Clear()
        {
            var list = TestQueue();

            list.Clear();
            Assert.AreEqual(0, list.Count);
        }

        private PriorityList<float> TestQueue()
        {
            var list = new PriorityList<float>();

            list.Add(1);
            list.Add(0.5f);
            list.Add(1.5f);

            return list;
        }

    }
}
