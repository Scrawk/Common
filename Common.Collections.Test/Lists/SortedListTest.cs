using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Lists;

namespace Common.Collections.Test.Lists
{
    [TestClass]
    public class Collections_Lists_SortedListTest
    {
        [TestMethod]
        public void Add()
        {

            SortedList<float> list = new SortedList<float>(new FloatComparer());

            list.Add(1);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(1, list.Root.Item);

            list.Add(0.5f);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(0.5f, list.Root.Item);

            list.Add(1.5f);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(0.5f, list.Root.Item);

            CollectionAssert.AreEqual(new float[] { 0.5f, 1.0f, 1.5f }, list.ToList());

        }

        [TestMethod]
        public void Remove()
        {

            SortedList<float> list = new SortedList<float>(new FloatComparer());

            list.Add(1);
            list.Add(0.5f);
            list.Add(1.5f);

            list.Remove(0.5f);

            Assert.AreEqual(1.0f, list.Root.Item);
            Assert.AreEqual(2, list.Count);

            list.Remove(1.5f);

            Assert.AreEqual(1.0f, list.Root.Item);
            Assert.AreEqual(1, list.Count);

            list.Remove(1.0f);

            Assert.IsNull(list.Root);
            Assert.AreEqual(0, list.Count);

        }

        [TestMethod]
        public void Order()
        {
            int num = 1000;
            Random rnd = new Random(0);

            SortedList<float> list = new SortedList<float>(new FloatComparer());

            for (int i = 0; i < num; i++)
                list.Add((float)rnd.NextDouble());

            float v = list.Root.Item;
            list.Remove(v);

            while (list.Count != 0)
            {
                float next = list.Root.Item;
                list.Remove(next);
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
