using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Lists;

namespace Common.Collections.Test.Lists
{
    [TestClass]
    public class Collections_Lists_LALinkedListTest
    {
        [TestMethod]
        public void AddFirst()
        {
            var list = new LALinkedList<string>();

            list.AddFirst("John");

            Assert.AreEqual("John", list.First.Value);
            Assert.AreEqual("John", list.Last.Value);
            Assert.AreEqual(1, list.Count);

            list.AddFirst("Fred");

            Assert.AreEqual("Fred", list.First.Value);
            Assert.AreEqual("John", list.Last.Value);
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void AddLast()
        {
            var list = new LALinkedList<string>();

            list.AddLast("John");

            Assert.AreEqual("John", list.First.Value);
            Assert.AreEqual("John", list.Last.Value);
            Assert.AreEqual(1, list.Count);

            list.AddLast("Fred");

            Assert.AreEqual("John", list.First.Value);
            Assert.AreEqual("Fred", list.Last.Value);
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void RemoveFirst()
        {
            var list = new LALinkedList<string>();

            list.AddFirst("John");
            list.AddFirst("Fred");

            list.RemoveFirst();

            Assert.AreEqual("John", list.First.Value);
            Assert.AreEqual("John", list.Last.Value);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void RemoveLast()
        {
            var list = new LALinkedList<string>();

            list.AddLast("John");
            list.AddLast("Fred");

            list.RemoveLast();

            Assert.AreEqual("John", list.First.Value);
            Assert.AreEqual("John", list.Last.Value);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void Remove()
        {
            var list = new LALinkedList<string>();

            list.Remove("John");

            list.AddLast("John");
            list.AddLast("Fred");
            list.AddLast("Sam");

            list.Remove(null);

            Assert.AreEqual(3, list.Count);

            list.Remove("Fred");

            Assert.IsTrue(!list.Contains("Fred"));
            Assert.AreEqual(2, list.Count);

            list.Remove("John");

            Assert.IsTrue(!list.Contains("John"));
            Assert.AreEqual(1, list.Count);

            list.Remove("Sam");

            Assert.IsTrue(!list.Contains("Sam"));
            Assert.AreEqual(0, list.Count);

        }

        [TestMethod]
        public void Clear()
        {
            var list = new LALinkedList<string>();

            list.AddFirst("John");
            list.AddFirst("Fred");

            list.Clear();

            Assert.IsNull(list.First);
            Assert.IsNull(list.Last);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Contains()
        {
            var list = new LALinkedList<string>();

            list.AddFirst("John");
            list.AddFirst("Fred");

            Assert.IsTrue(list.Contains("John"));
            Assert.IsTrue(list.Contains("Fred"));
            Assert.IsFalse(list.Contains("Sam"));
            Assert.IsFalse(list.Contains(null));
        }

        [TestMethod]
        public void Enumeration()
        {
            var list = new LALinkedList<string>();

            list.AddLast("John");
            list.AddLast("Fred");
            list.AddLast("Sam");

            string[] expected = new string[] { "John", "Fred", "Sam" };

            int count = 0;
            var e = list.GetEnumerator();
            while (e.MoveNext())
            {
                Assert.AreEqual(expected[count], e.Current);
                count++;
            }

            Assert.AreEqual(3, count);

            count = 0;
            foreach(string s in list)
            {
                Assert.AreEqual(expected[count], s);
                count++;
            }

            Assert.AreEqual(3, count);

        }

        [TestMethod]
        public void Nested_Enumeration()
        {
            var list = new LALinkedList<string>();

            list.AddLast("John");
            list.AddLast("Fred");
            list.AddLast("Sam");

            int count = 0;
            foreach (string s1 in list)
            {
                foreach (string s2 in list)
                {
                    count++;
                }
            }

            Assert.AreEqual(3*3, count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ModifiedDuringEnumeration_ThrowsException()
        {
            var list = new LALinkedList<string>();

            list.AddLast("John");
            list.AddLast("Fred");
            list.AddLast("Sam");

            foreach(string s in list)
            {
                list.Remove(s);
            }
        }

     }
}
